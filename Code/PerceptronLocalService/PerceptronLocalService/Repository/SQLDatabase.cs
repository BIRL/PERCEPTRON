using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerceptronLocalService.DTO;
using PerceptronLocalService.Interfaces;
using PerceptronLocalService.Models;
using PerceptronLocalService.Utility;

namespace PerceptronLocalService.Repository
{
    class SQLDatabase : IDataAccessLayer
    {
        //public string GetEmailFromUserId(string id)
        //{
        //    string email;
        //    using (var db = new PerceptronContext())
        //    {
        //        var query = from b in db.SearchQueryTable
        //            where b.UserId == id
        //            select b.Email;
        //        email = query.Any() ? query.First() : Constants.EmailNotFound;
        //    }
        //    return email;
        //}

        public void StorePeakList(string FileUniqueId, string peakDataMassesString, string peakDataIntensitiesString)
        {
            var peakList = new PeakListData
            {
                FileUniqueId = FileUniqueId,
                PeakListMasses = peakDataMassesString,
                PeakListIntensities = peakDataIntensitiesString
            };
            using (var db = new PerceptronDatabaseEntities())
            {
                db.PeakListDatas.Add(peakList);
                db.SaveChanges();
            }

        }

        public string StoreResults(SearchResultsDto res, string fileName, string FileUniqueId, int fileId)
        {
            string message = Constants.ResultsSotredSuccessfully; //Spelling mistake?#PROBLEM_DETECTED
            using (var db = new PerceptronDatabaseEntities())
            {
                var executionTime = GetExecutionTimeModel(res, fileName);
                db.ExecutionTimes.Add(executionTime);
                //db.SaveChanges();

                foreach (ProteinDto protein in res.FinalProt)
                {
                    var resId = Guid.NewGuid();
                    var headerTag = GetHeaderTag(protein.Header);
                    var searchResult = GetSearchResultModel(res.QueryId, fileId, headerTag, protein, resId, FileUniqueId);
                    db.SearchResults.Add(searchResult);
                    //db.SaveChanges();


                    if (protein.PtmParticulars == null) continue;
                    foreach (var ptmSite in protein.PtmParticulars)
                    {                        
                        var resultPtmSites = GetResultPtmSitesModel(resId, ptmSite);
                        db.ResultPtmSites.Add(resultPtmSites);
                        //db.SaveChanges();
                    }

                    var resultInsilicoMatchLeft = GetResultInsilicoMatchLeftModel(resId, protein.InsilicoDetails.PeaklistMassLeft);
                    db.ResultInsilicoMatchLefts.Add(resultInsilicoMatchLeft);
                    //db.SaveChanges();

                    var resultInsilicoMatchRight = GetResultInsilicoMatchLeftModel(resId, protein.InsilicoDetails.PeaklistMassRight);
                    db.ResultInsilicoMatchLefts.Add(resultInsilicoMatchRight); //#PROBLEM_DETECTED??? Why this-->ResultInsilicoMatchLeftTable WHy not ResultInsilicoMatchRights
                    //db.SaveChanges();
                }

                db.SaveChanges();
            }
            return message;
        }

        public List<SearchQueryDto> ServerStatus()
        {
            List<SearchQueryDto> searchQueryList;
            using (var db = new PerceptronDatabaseEntities())
            {
                var query = (from b in db.SearchQueries
                            where b.Progress == Constants.QueryNotStarted
                            select b).ToList();
                var query1 = query.Select(GetSearchQueryDto);

                searchQueryList = query1.Any() ? query1.ToList() : new List<SearchQueryDto>();
            }
            return searchQueryList;
        }      

        public SearchParametersDto GetParameters(string qid)  // Getting search parameters from different tables of PerceptronDatabase
        {
            SearchParametersDto searchParametersDto;
            using (var db = new PerceptronDatabaseEntities())
            {
                var searchParameters = from b in db.SearchParameters
                            where b.QueryId == qid
                            select b;

                var ptmFixed = (from b in db.PtmFixedModifications
                             where b.QueryId == qid
                             select b.ModificationCode).ToList();

                var ptmVariable = (from b in db.PtmVariableModifications
                                where b.QueryId == qid
                                select b.ModificationCode).ToList();

                var files = (from b in db.SearchFiles
                                   where b.QueryId == qid
                                   select b).ToList();
                var fileType = files.Select(b => b.FileType).ToArray();
                var fileName = files.Select(b => b.FileName).ToArray();
                var fileUniqueName = files.Select(b => b.UniqueFileName).ToArray();
                var FileUniqueIdArray = files.Select(b => b.FileUniqueId).ToArray();

                searchParametersDto = searchParameters.Any() ? GetSearchParametersDtoModel(searchParameters.First(), ptmVariable, ptmFixed, fileType, fileName, fileUniqueName, FileUniqueIdArray) : new SearchParametersDto();
            }
            return searchParametersDto;
        }

        //public string GetCreationTime(string qid)
        //{
        //    var temp = new SearchQuery();

        //    using (var db = new PerceptronDatabaseEntities())
        //    {
        //        var query = (from b in db.SearchQueryTable
        //                     where b.QueryId == qid
        //                     select b).ToList();
        //        temp = query[0];
        //    }
        //    return temp.CreationTime;
        //}



        public int Set_Progress(string qid, int progress)
        {
            using (var db = new PerceptronDatabaseEntities())
            {
                var result = db.SearchQueries.SingleOrDefault(b => b.QueryId == qid);
                if (result != null)
                {
                    result.Progress = progress.ToString();
                    db.SaveChanges();
                }
            }
            return 1;
        }

        private ResultInsilicoMatchLeft GetResultInsilicoMatchLeftModel(Guid resId, List<double> peaklistMass)
        {
            var resultInsilicoMatch = new ResultInsilicoMatchLeft
            {
                ResultId = resId.ToString(),
                MatchedPeaks = String.Join(",",
                        peaklistMass.Select(
                        x => x.ToString(CultureInfo.InvariantCulture)).ToArray())
            };
            return resultInsilicoMatch;
        }

        private ResultPtmSite GetResultPtmSitesModel(Guid resId, PostTranslationModificationsSiteDto ptmSite) //ResultPtmSites
        {
            var ptmSites = new ResultPtmSite
            {
                ResultId = resId.ToString(),
                AminoAcid = ptmSite.AminoAcid.Aggregate("", (current, t) => current + t),
                Index = ptmSite.Index,
                ModName = ptmSite.ModName,
                ModWeight = ptmSite.ModWeight,
                Score = ptmSite.Score,
                Site = ptmSite.Site  //#MIGHTGETERROR //Converted it char(char(7000)) to avoiding string (nvarchar(MAX))
                //Aik sa zyada modification sites of modifications ho skti hain...?
            };
            return ptmSites;
        }

        private static string GetHeaderTag(string rawHeader)
        {
            string headerTag;
            if (rawHeader[0] == '>')
            {
                headerTag = rawHeader.Contains('|')
                    ? rawHeader.Substring(4, 6)
                    : rawHeader.Substring(1, rawHeader.Length - 1);
            }
            else
                headerTag = rawHeader;
            return headerTag;
        }

        private SearchResult GetSearchResultModel(string queryTd, int fileId, string headerTag, ProteinDto protein, Guid resId, string FileUniqueId)
        {
            var searchResult = new SearchResult
            {
                QueryId = queryTd,
                Sequence = protein.Sequence,
                OriginalSequence = protein.OriginalSequence,
                FileId = fileId.ToString(),
                Header = headerTag,
                InsilicoScore = protein.InsilicoScore,
                Mw = protein.Mw,
                MwScore = protein.MwScore,
                PstScore = protein.PstScore,
                PtmScore = protein.PtmScore,
                ResultId = resId.ToString(),
                Score = protein.Score,
                TerminalModification = protein.TerminalModification,
                PSTTags = protein.PstTagsWithComma,

                TruncationSite = protein.Truncation,
                TruncationIndex = protein.TruncationIndex,


                //For Results Visualizations
                RightMatchedIndex = string.Join(",", protein.RightMatchedIndex),
                RightPeakIndex = string.Join(",", protein.RightPeakIndex),
                RightType = string.Join(",", protein.RightType),
                LeftMatchedIndex = string.Join(",", protein.LeftMatchedIndex),
                LeftPeakIndex = string.Join(",", protein.LeftPeakIndex),
                LeftType = string.Join(",", protein.LeftType),



                InsilicoMassLeft = string.Join(",", protein.InsilicoDetails.InsilicoMassLeft),
                InsilicoMassRight = string.Join(",", protein.InsilicoDetails.InsilicoMassRight),
                InsilicoMassLeftAo = string.Join(",", protein.InsilicoDetails.InsilicoMassLeftAo),
                InsilicoMassLeftBo = string.Join(",", protein.InsilicoDetails.InsilicoMassLeftBo),
                InsilicoMassLeftAstar = string.Join(",", protein.InsilicoDetails.InsilicoMassLeftAstar),
                InsilicoMassLeftBstar = string.Join(",", protein.InsilicoDetails.InsilicoMassLeftBstar),
                InsilicoMassRightYo = string.Join(",", protein.InsilicoDetails.InsilicoMassRightYo),
                InsilicoMassRightYstar = string.Join(",", protein.InsilicoDetails.InsilicoMassRightYstar),
                InsilicoMassRightZo = string.Join(",", protein.InsilicoDetails.InsilicoMassRightZo),
                InsilicoMassRightZoo = string.Join(",", protein.InsilicoDetails.InsilicoMassRightZoo),

                FileUniqueId = FileUniqueId


            };
            return searchResult;
        }

        private ExecutionTime GetExecutionTimeModel(SearchResultsDto res, string fileName)
        {
            var executionTime = new ExecutionTime
            {
                QueryId = res.QueryId,
                FileName = fileName,
                InsilicoTime = res.Times.InsilicoTime,
                MwFilterTime = res.Times.MwFilterTime,
                PstTime = res.Times.PstTime,
                PtmTime = res.Times.PtmTime,
                TotalTime = res.Times.TotalTime,
                TunerTime = res.Times.TunerTime,
                TruncationEngineTime = res.Times.TruncationEngineTime

            };

            return executionTime;
        }

        private SearchQueryDto GetSearchQueryDto(SearchQuery searchQuery)
        {
            var searchQueryDto = new SearchQueryDto
            {
                QueryId = searchQuery.QueryId,
                CreationTime = searchQuery.CreationTime,
                Progress = searchQuery.Progress,
                UserId = searchQuery.UserId
            };
            return searchQueryDto;
        }

        private SearchParametersDto GetSearchParametersDtoModel(SearchParameter searchParameters, List<int> ptmVariable,
          List<int> ptmFixed, string[] fileType, string[] fileName, string[] fileUniqueName, string[] FileUniqueIdArray)
        {
            var searchParametersDto = new SearchParametersDto
            {
                Queryid = searchParameters.QueryId,
                Autotune = searchParameters.Autotune,
                DenovoAllow = searchParameters.DenovoAllow,
                FilterDb = searchParameters.FilterDb,
                GuiMass = searchParameters.GuiMass,
                HandleIons = searchParameters.HandleIons,
                HopThreshhold = searchParameters.HopThreshhold,
                HopTolUnit = searchParameters.HopTolUnit,
                InsilicoFragType = searchParameters.InsilicoFragType,
                UserId = searchParameters.UserId,
                Title = searchParameters.Title,
                ProtDb = searchParameters.ProtDb,
                PtmTolerance = searchParameters.PtmTolerance,
                MinimumPstLength = searchParameters.MinimumPstLength,
                MaximumPstLength = searchParameters.MaximumPstLength,
                MwTolerance = searchParameters.MwTolerance,
                MwSweight = searchParameters.MwSweight,
                PstSweight = searchParameters.PstSweight,
                InsilicoSweight = searchParameters.InsilicoSweight,
                NumberOfOutputs = searchParameters.NumberOfOutputs,
                PtmAllow = searchParameters.PtmAllow,
                PtmCodeVar = ptmVariable,
                PtmCodeFix = ptmFixed,
                FileType = fileType,
                PeakListFileName = fileName,
                PeakListUniqueFileNames = fileUniqueName,
                FileUniqueIdArray = FileUniqueIdArray,
                NeutralLoss = searchParameters.NeutralLoss,  //Added 12Sep2019
                PSTTolerance = searchParameters.PSTTolerance,

                PeptideTolerance = searchParameters.PeptideTolerance,
                PeptideToleranceUnit = searchParameters.PeptideToleranceUnit,
                TerminalModification = searchParameters.TerminalModification,
                Truncation = searchParameters.Truncation,
                SliderValue = searchParameters.SliderValue,

                CysteineChemicalModification = searchParameters.CysteineChemicalModification,
                MethionineChemicalModification = searchParameters.MethionineChemicalModification,
                EmailId = searchParameters.EmailId

            };
            return searchParametersDto;
        }
       
    }
}
