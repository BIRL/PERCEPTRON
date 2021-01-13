using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using PerceptronAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Validation;  //Added on 12Sep2019.. WHy its not needed before...???

namespace PerceptronAPI.Repository
{
    public class SqlDatabase : IDataAccessLayer
    {

        //Store Query Parameters
        public string StoreSearchParameters(SearchParametersDto parameters)
        {
            using (var db = new PerceptronDatabaseEntities())
            {
                db.SearchQueries.Add(parameters.SearchQuerry);
                db.SearchParameters.Add(parameters.SearchParameters);
                db.SearchFiles.AddRange(parameters.SearchFiles);
                db.PtmFixedModifications.Add(parameters.FixedMods);
                db.PtmVariableModifications.Add(parameters.VarMods);

                db.SaveChanges();

            }
            return "Ok";
        }


        public void StoreSearchFiles(SearchParametersDto param)
        {
            using (var db = new PerceptronDatabaseEntities())
            {
                db.SaveChanges();
            }
        }

        //Stores Server Status
        public List<SearchQuery> RetrieveSearchQueries()
        {
            using (var db = new PerceptronDatabaseEntities())
            {
                return db.SearchQueries.ToList();
            }
        }

        //Returns Progress
        public int RetrieveQueryProgress(string qid)
        {
            using (var db = new PerceptronDatabaseEntities())
            {
                return Convert.ToInt32(db.SearchQueries.Where(x => x.QueryId == qid).Select(x => x.Progress));
            }
        }

        public List<SearchQuery> RetrieveUsersSearchQueries(string userid)
        {
            using (var db = new PerceptronDatabaseEntities())
            {
                return db.SearchQueries.Where(x => x.UserId == userid).ToList();
            }
        }

        public ResultsDto RetrieveResultByRid(string rid)
        {
            using (var db = new PerceptronDatabaseEntities())
            {
                return new ResultsDto
                {
                    //InsilicioLeft = db.ResultInsilicoMatchLefts.Where(x => x.ResultId == rid).ToList(),
                    //InsilicoRight = db.ResultInsilicoMatchRights.Where(x => x.ResultId == rid).ToList(),
                    //PtmSites = db.ResultPtmSites.Where(x => x.ResultId == rid).ToList(),
                    Results = db.SearchResults.First(x => x.ResultId == rid)
                };
            }
        }

        public ResultViewDto retrieve_searchview_db(string qid)
        {
            using (var db = new PerceptronDatabaseEntities())
            {
                return new ResultViewDto
                {
                    Paramters = new SearchParametersDto
                    {
                        SearchParameters = db.SearchParameters.First(x => x.QueryId == qid),
                        FixedMods = db.PtmFixedModifications.First(x => x.QueryId == qid),
                        SearchFiles = db.SearchFiles.Where(x => x.QueryId == qid).ToList(),
                        SearchQuerry = db.SearchQueries.First(x => x.QueryId == qid),
                        VarMods = db.PtmVariableModifications.First(x => x.QueryId == qid)
                    },
                    ExecutionTime = db.ExecutionTimes.First(x => x.QueryId == qid),
                    Results =
                        db.SearchResults.Where(x => x.QueryId == qid)
                            .Select(x => RetrieveResultByRid(x.ResultId))
                            .ToList()
                };
            }
        }


        public List<ScanResults> Scan_Results(string qid)
        {
            qid = qid.Trim('"');
            var scanResults = new List<ScanResults>();
            try
            {
                using (var db = new PerceptronDatabaseEntities())
                {
                    var sqlConnection1 =
                        new SqlConnection(
                            "Server= CHIRAGH-I; Database= PerceptronDatabase; Integrated Security=SSPI;");
                    var cmd = new SqlCommand
                    {
                        CommandText =
                    //"SELECT *\nFROM SearchResults E\nWHERE E.QueryId = '" + qid + "' FROM SearchFiles E2 Where E2.QueryId=E.QueryId AND E2.FileUniqueId = E.FileUniqueId))",  // E.ProteinRank = '1',


                    /// ITS HEALTHY BELOW
                    "SELECT P.FileId, P.Mw, P.Header, P.Score, P.FileUniqueId, P.ProteinRank, R.FileName \nFROM SearchFiles as R, SearchResults as P \nWHERE P.Queryid = '" + qid +
                        "' AND P.ProteinRank = '" + 1 + "' AND P.FileUniqueId=R.FileUniqueId ORDER BY P.Queryid Desc ",
                        ////// ITS HEALTHY ABOVE
                        CommandType = CommandType.Text,
                        Connection = sqlConnection1
                    };
                    sqlConnection1.Open();
                    var SearchFileList = db.SearchFiles.Where(x => x.QueryId == qid).ToList();

                    //var parameters = db.SearchParameters.Where(x => x.QueryId == qid).ToList().First();

                    int j = 0;
                    var dataReader = cmd.ExecuteReader();


                    if (SearchFileList.Count == 1)   //Checking if no of files more than one then, it will be considered as batch mode and PERCEPTRON will not fetch the data from database.
                    {
                        var FileSpecificData = db.SearchResults.Where(x => x.QueryId == qid).ToList();

                        while (dataReader.Read())
                        {
                            var temp = new ScanResults
                            {
                                FileId = dataReader["FileId"].ToString(),
                                FileName = Path.GetFileName(dataReader["FileName"].ToString()),
                                ProteinId = dataReader["Header"].ToString(),
                                Score = (double)dataReader["Score"],
                                MolW = (double)dataReader["Mw"],
                                FileUniqueId = dataReader["FileUniqueId"].ToString(),
                                //SearchModeMessage = "SingleMode"       // Just for showing whether its Single ode or Batch Mode

                                //Truncation = "No",
                                //Frags = 1,
                                //Mods = 1,

                            };
                            scanResults.Add(temp);
                        }
                    }
                    dataReader.Close();
                    cmd.Dispose();
                    sqlConnection1.Close();
                }
            }

            catch (DbEntityValidationException e)
            {
                int asd = 1;

            }


            return scanResults;
        }

        public ZipResultsDownloadInfo ScanResultFile(string QueryId)
        {
            var ZipResultFileInfo = new ZipResultsDownloadInfo();
            using (new PerceptronDatabaseEntities())
            {
                var sqlConnection1 =
                    new SqlConnection(
                        "Server= CHIRAGH-I; Database= PerceptronDatabase; Integrated Security=SSPI;");
                var cmd = new SqlCommand
                {
                    CommandText =
                        "SELECT QueryId, ZipFileWithQueryId, ZipFileName \nFROM ZipResultsDownloadInfo\nWHERE QueryId = '" + QueryId + "'",
                    CommandType = CommandType.Text,
                    Connection = sqlConnection1
                };
                sqlConnection1.Open();

                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    ZipResultFileInfo.QueryId = QueryId;
                    ZipResultFileInfo.ZipFileWithQueryId = dataReader["ZipFileWithQueryId"].ToString();
                    ZipResultFileInfo.ZipFileName = dataReader["ZipFileName"].ToString();
                }

                dataReader.Close();
                cmd.Dispose();
                sqlConnection1.Close();
            }
            return ZipResultFileInfo;
        }

        public ScanInputDataDto ScanInputData(string qid)   // ITS HEALTHY WAS IN USED WHEN API COMPILE THE RESUTLS
        {
            var FileUniqueIdsList = new List<string>();
            var FileNamesList = new List<string>();
            var UniqueFileNameList = new List<string>();
            var searchParameters = new SearchParameter();

            using (new PerceptronDatabaseEntities())
            {
                var sqlConnection1 =
                    new SqlConnection(
                        "Server= CHIRAGH-I; Database= PerceptronDatabase; Integrated Security=SSPI;");
                var cmd = new SqlCommand
                {
                    CommandText =
                        "SELECT FileUniqueId, FileName, UniqueFileName \nFROM SearchFiles\nWHERE QueryId = '" + qid + "'",
                    CommandType = CommandType.Text,
                    Connection = sqlConnection1
                };
                sqlConnection1.Open();

                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    FileUniqueIdsList.Add(dataReader["FileUniqueId"].ToString());
                    FileNamesList.Add(dataReader["FileName"].ToString());
                    UniqueFileNameList.Add(dataReader["UniqueFileName"].ToString());
                }

                dataReader.Close();
                cmd.Dispose();
                sqlConnection1.Close();
            }
            using (var db = new PerceptronDatabaseEntities())
            {
                searchParameters = db.SearchParameters.Where(x => x.QueryId == qid).ToList().First();
            }

            var ScanInputDataInfo = new ScanInputDataDto(FileUniqueIdsList, FileNamesList, UniqueFileNameList, searchParameters);
            return ScanInputDataInfo;
        }



        public ScanResultsDownloadDataDto ScanResultsDownloadData(string qid, string FileUniqueId)
        {
            //var tempScanResultsDownloadDataDto = new ScanResultsDownloadDataDto();
            
            //var ListOfPeakData = new List<PeakListData>();
            var ListOfSearchResults = new List<SearchResult>();
            var PtmSites = new List<ResultPtmSite>();
            //var ResultIds = new List<string>();

            using (var db = new PerceptronDatabaseEntities())
            {
                try
                {
                    ListOfSearchResults.AddRange(db.SearchResults.Where(x => x.FileUniqueId == FileUniqueId)); //Fetching Results from PerceptronDatabase based on the Peaklist files using FileUniqueId
                    var NoOfResultIds = ListOfSearchResults.Select(x => x.ResultId).Distinct().ToList();

                    
                    for (int i = 0; i < ListOfSearchResults.Count; i++)
                    {
                        var ResultId = ListOfSearchResults[i].ResultId;
                        PtmSites.AddRange(db.ResultPtmSites.Where(x => x.ResultId == ResultId));
                    }
                    var PtmVarMod = new List<PtmVariableModification>();
                    var PtmFixedMod = new List<PtmFixedModification>();
                }
                catch (Exception e)
                {
                    ListOfSearchResults = new List<SearchResult>();
                    PtmSites = new List<ResultPtmSite>();
                }
                
            }
            var tempScanResultsDownloadDataDto = new ScanResultsDownloadDataDto(ListOfSearchResults, PtmSites);
            return tempScanResultsDownloadDataDto;

        }

        public List<SummaryResults> Summary_results(string qid, string fid)
        {
            qid = qid.Trim('"');
            var summaryResults = new List<SummaryResults>();
            var db = new PerceptronDatabaseEntities();
            using (new PerceptronDatabaseEntities())
            {
                var sqlConnection1 =
                    new SqlConnection(
                        "Server= CHIRAGH-I; Database= PerceptronDatabase; Integrated Security=SSPI;");
                var cmd = new SqlCommand
                {
                    CommandText =
                        "SELECT *\nFROM SearchResults E\nWHERE E.QueryId = '" + qid + "' AND E.fileid = '" + fid + "' ORDER BY E.Score DESC ",
                    CommandType = CommandType.Text,
                    Connection = sqlConnection1
                };
                sqlConnection1.Open();
                int j = 0;
                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    var temp = new SummaryResults
                    {
                        ProteinId = dataReader["Header"].ToString(),
                        Mods = 2, /// Mods is a Number of Ptm Sites and "2" is not actual value so, actual one 
                                  /// cannot be updated here so, here "2" will be considered as a placeholder only
                        MolW = (double)dataReader["Mw"],
                        ProteinName = "Protein " + j,
                        ProteinRank = (int)dataReader["ProteinRank"], //1,
                        ResultId = dataReader["ResultId"].ToString(),
                        Score = (double)dataReader["Score"],
                        TerminalMods = dataReader["TerminalModification"].ToString()
                    };
                    ++j;
                    summaryResults.Add(temp);
                }
                dataReader.Close();
                cmd.Dispose();
                sqlConnection1.Close();
            }
            using (var dbInfo = new PerceptronDatabaseEntities())
            {
                for (int i = 0; i < summaryResults.Count; i++)
                {
                    string ResultId = summaryResults[i].ResultId;
                    //int NoOfPtmSites = 0; 
                    //if (dbInfo.ResultPtmSites.Where(x => x.ResultId == ResultId).Count() != 0)
                    //    NoOfPtmSites = dbInfo.ResultPtmSites.Where(x => x.ResultId == ResultId).First().Index.Split(',').Select(int.Parse).ToList().Count;  // Just wanted to get Number of Ptm Sites


                    summaryResults[i].Mods = NoOfPTMMods(ResultId, dbInfo); //Updated 20200821 //NoOfPtmSites;  // Updating the actual value of "Mods" here...
                }
                summaryResults = summaryResults.OrderBy(x => x.ProteinRank).ToList();
            }
            return summaryResults;
        }

        private int NoOfPTMMods(string ResultId, PerceptronDatabaseEntities dbInfo) //Added 20200821
        {
            int NoOfPtmSites = 0;
            if (dbInfo.ResultPtmSites.Where(x => x.ResultId == ResultId).Count() != 0)
                NoOfPtmSites = dbInfo.ResultPtmSites.Where(x => x.ResultId == ResultId).First().Index.Split(',').Select(int.Parse).ToList().Count;  // Just wanted to get Number of Ptm Sites
            return NoOfPtmSites;
        }

        private int NoOfMatchedFrags(SearchResult searchResult)
        {
            int LeftMatches = 0; int RightMathces = 0;
            if (searchResult.LeftMatchedIndex != "")
            {
                var LeftMatchedIndex = searchResult.LeftMatchedIndex.Split(',').Select(int.Parse).ToList();
                LeftMatches = LeftMatchedIndex.Count;
            }

            if (searchResult.RightMatchedIndex != "")
            {
                var RightMatchedIndex = searchResult.RightMatchedIndex.Split(',').Select(int.Parse).ToList();
                RightMathces = RightMatchedIndex.Count;
            }

            int NoOfMatches = LeftMatches + RightMathces;
            return NoOfMatches;
        }

        public List<UserHistory> GetUserHistory(string Uid)
        {

            var summaryResults = new List<UserHistory>();
            using (new PerceptronDatabaseEntities())
            {
                var sqlConnection1 =
                    new SqlConnection(
                        "Server= CHIRAGH-I; Database= PerceptronDatabase; Integrated Security=SSPI;");
                var cmd = new SqlCommand
                {
                    CommandText =
                        "SELECT P.Queryid, P.Title, R.CreationTime, R.Progress \nFROM SearchQueries as R, SearchParameters as P \nWHERE P.UserId = '" + Uid + "'AND P.Queryid=R.QueryId ",
                    CommandType = CommandType.Text,
                    Connection = sqlConnection1
                };
                sqlConnection1.Open();

                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    var temp = new UserHistory
                    {
                        title = dataReader["Title"].ToString(),
                        qid = dataReader["Queryid"].ToString(),
                        time = dataReader["CreationTime"].ToString(),
                        progress = dataReader["Progress"].ToString()
                    };
                    summaryResults.Add(temp);
                }
                dataReader.Close();
                cmd.Dispose();
                sqlConnection1.Close();
            }

            for (int index = 0; index < summaryResults.Count; index++)
            {
                if (summaryResults[index].progress == "0")
                {
                    summaryResults[index].progress = "In Queue";
                }


                else if (summaryResults[index].progress == "10")
                {
                    summaryResults[index].progress = "Running";
                }


                else if (summaryResults[index].progress == "100")
                {
                    summaryResults[index].progress = "Completed";
                }

                else if (summaryResults[index].progress == "-100")
                {
                    summaryResults[index].progress = "Result Expired";
                }

                else  // WHEN (summaryResults[index].progress == "-1")
                {
                    summaryResults[index].progress = "Error in Query";
                }
            }
            return summaryResults.OrderByDescending(x => x.time).ToList();
        }

        public stat stat()
        {

            var temp = new stat();
            using (var db = new PerceptronDatabaseEntities())
            {
                var sqlConnection1 =
                    new SqlConnection(
                        "Server= CHIRAGH-I; Database= PerceptronDatabase; Integrated Security=SSPI;");
                var cmd = new SqlCommand
                {
                    CommandText =
                        "SELECT count( distinct UserId) as one  FROM SearchQueries",
                    CommandType = CommandType.Text,
                    Connection = sqlConnection1
                };
                sqlConnection1.Open();
                int j = 0;
                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {

                    temp.user = dataReader["one"].ToString();
                }
                dataReader.Close();
                cmd.Dispose();
                sqlConnection1.Close();
            }
            using (var db = new PerceptronDatabaseEntities())
            {
                var sqlConnection1 =
                    new SqlConnection(
                        "Server= CHIRAGH-I; Database= PerceptronDatabase; Integrated Security=SSPI;");

                var cmd = new SqlCommand
                {
                    CommandText = "Select count( QueryId) as two FROM SearchQueries WHERE Progress = 100",

                    //"Select count( QueryId) as two FROM SearchResults",

                    CommandType = CommandType.Text,
                    Connection = sqlConnection1
                };


                sqlConnection1.Open();
                int j = 0;
                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {

                    temp.search = dataReader["two"].ToString();
                }
                dataReader.Close();
                cmd.Dispose();
                sqlConnection1.Close();
            }
            return temp;
        }


        public DetailedResults Detailed_Results(string qid, string rid)
        {
            var detiledResults = new DetailedResults();
            ////using (new PerceptronDatabaseEntities())            /// ITS HEALTHY
            ////{
            ////    var sqlConnection1 =
            ////        new SqlConnection(
            ////            "Server= CHIRAGH-I; Database= PerceptronDatabase; Integrated Security=SSPI;");
            ////    var cmd = new SqlCommand
            ////    {
            ////        CommandText =
            ////            "SELECT QueryId \nFROM SearchResults \nWHERE ResultId = '" + rid + "'",
            ////        CommandType = CommandType.Text,
            ////        Connection = sqlConnection1
            ////    };
            ////    sqlConnection1.Open();

            ////    var dataReader = cmd.ExecuteReader();
            ////    while (dataReader.Read())
            ////        qid = dataReader["QueryId"].ToString();

            ////    dataReader.Close();
            ////    cmd.Dispose();
            ////    sqlConnection1.Close();
            ////}
            ///

            try
            {
                using (var db = new PerceptronDatabaseEntities())
                {
                    var searchParameters = db.SearchParameters.Where(x => x.QueryId == qid).ToList();
                    var searchResult = db.SearchResults.Where(x => x.ResultId == rid).ToList();
                    //var resultInsilicoLeft = db.ResultInsilicoMatchLefts.Where(x => x.ResultId == rid).ToList();
                    //var resultInsilicoRight = db.ResultInsilicoMatchRights.Where(x => x.ResultId == rid).ToList();

                    //var ptmVarmod = db.PtmVariableModifications.First(x => x.QueryId == qid);
                    //var ptmFixedmod = db.PtmFixedModifications.First(x => x.QueryId == qid);

                    var ptmSite = db.ResultPtmSites.Where(x => x.ResultId == rid).ToList();
                    var execTime = db.ExecutionTimes.Where(x => x.QueryId == qid).ToList();
                    var searchQuery = db.SearchQueries.Where(x => x.QueryId == qid).ToList();


                    //if (db.ResultPtmSites.Where(x => x.ResultId == rid).Count() != 0)
                    //    NoOfPtmSites = db.ResultPtmSites.Where(x => x.ResultId == rid).First().Index.Split(',').Select(int.Parse).ToList().Count;  // Just wanted to get Number of Ptm 


                    if (searchParameters.Count != 0)
                        detiledResults.Paramters.SearchParameters = searchParameters.Any() ? GetSearchParametersDtoModel(searchParameters.First()) : new SearchParameter();

                    //detiledResults.Paramters.FixedMods = ptmFixedmod;
                    //detiledResults.Paramters.VarMods = ptmVarmod;

                    if (searchQuery.Count != 0)
                        detiledResults.Paramters.SearchQuerry = searchQuery.First();

                    //detiledResults.Results.InsilicioLeft = resultInsilicoLeft;
                    //detiledResults.Results.InsilicoRight = resultInsilicoRight;


                    detiledResults.Results.NoOfPtmSites = NoOfPTMMods(rid, db);    //Updated 20200821
                    detiledResults.Results.NoOfMatchedFragments = NoOfMatchedFrags(searchResult.First());   //Updated 20200821

                    if (searchResult.Count != 0)    //Why if for Safe...?
                        detiledResults.Results.Results = searchResult.First();
                    if (execTime.Count != 0)
                        detiledResults.ExecutionTime = execTime.First();



                }
                
            }
            catch(Exception e)
            {
                int fsdfs = 1;
            }
            return detiledResults;

        }
        //DownloadAllResults
        


        ///////Extracting File Unique Ids and FileNames against File Unique Ids
        //FileUniqueIdsList = ListOfSearchResults.Select(x => x.FileUniqueId).Distinct().ToList();
        //var tempFiles = db.SearchFiles.Where(x => x.QueryId == qid).ToList();
        //for (int indexFileUniqueId = 0; indexFileUniqueId < FileUniqueIdsList.Count; indexFileUniqueId++)
        //{
        //    for (int index = 0; index < tempFiles.Count; index++)
        //    {
        //        if (tempFiles[index].FileUniqueId == FileUniqueIdsList[indexFileUniqueId])
        //        {
        //            FileNamesList.Add(tempFiles[index].FileName);
        //            break;
        //        }
        //    }

        //}



        public DetailedProteinHitView DetailedProteinHitView_Results(string qid, string rid)
        {
            string FileId = ""; var GetPeakListData = new PeakListData();
            var DetailedProteinHitViewResults = new DetailedProteinHitView();
            using (new PerceptronDatabaseEntities())
            {
                var sqlConnection1 =
                    new SqlConnection(
                        "Server= CHIRAGH-I; Database= PerceptronDatabase; Integrated Security=SSPI;");
                var cmd = new SqlCommand
                {
                    CommandText =
                        //  "SELECT SR.Queryid, SR.FileUniqueId \nFROM SearchResults \nWHERE ResultId = '" + rid + "' as R, AND SELECT PeakListData as P \nWHERE P.FileUniqueId = SR.FileUniqueId ",
                        "SELECT QueryId, FileUniqueId \nFROM SearchResults \nWHERE ResultId = '" + rid + "'",

                    CommandType = CommandType.Text,
                    Connection = sqlConnection1
                };
                sqlConnection1.Open();

                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    qid = dataReader["QueryId"].ToString();
                    FileId = dataReader["FileUniqueId"].ToString();
                    //GetPeakListData = dataReader["PeakListData"];
                }
                dataReader.Close();
                cmd.Dispose();
                sqlConnection1.Close();
            }
            using (var db = new PerceptronDatabaseEntities())
            {
                var searchParameters = db.SearchParameters.Where(x => x.QueryId == qid).ToList();
                var searchResult = db.SearchResults.Where(x => x.ResultId == rid).ToList();
                var peakListData = db.PeakListDatas.Where(x => x.FileUniqueId == FileId).ToList();
                // var FileInfo = db.SearchFiles.Where(x => x.FileUniqueId == FileName).ToList();
                //NO NEED 
                //var resultInsilicoLeft = db.ResultInsilicoMatchLefts.Where(x => x.ResultId == rid).ToList();
                //var resultInsilicoRight = db.ResultInsilicoMatchRights.Where(x => x.ResultId == rid).ToList();

                /*  WILL USE IT LATER  */
                var ptmVarmod = db.PtmVariableModifications.Where(x => x.QueryId == qid).ToList();
                var ptmFixedmod = db.PtmFixedModifications.Where(x => x.QueryId == qid).ToList();
                var ptmSite = db.ResultPtmSites.Where(x => x.ResultId == rid).ToList();

                if (ptmSite.Count != 0)
                {
                    DetailedProteinHitViewResults.Results.PtmSitesInfo = ptmSite.First();
                }


                /*  WILL USE IT LATER  */

                DetailedProteinHitViewResults.Results.Results = searchResult.First();
                DetailedProteinHitViewResults.searchParameters = searchParameters.First();
                DetailedProteinHitViewResults.PeakListData = peakListData.First();

                //#4TTB


                //NO NEED 
                //var execTime = db.ExecutionTimes.Where(x => x.QueryId == qid).ToList();
                //var searchQuery = db.SearchQueries.Where(x => x.QueryId == qid).ToList();


                //if (searchParameters.Count != 0)
                //    DetailedProteinHitViewResults.Paramters.SearchParameters = searchParameters.Any() ? GetSearchParametersDtoModel(searchParameters.First()) : new SearchParameter();

                //DetailedProteinHitViewResults.Paramters.FixedMods = ptmFixedmod;
                //DetailedProteinHitViewResults.Paramters.VarMods = ptmVarmod;

                //if (searchQuery.Count != 0)
                //    DetailedProteinHitViewResults.Paramters.SearchQuerry = searchQuery.First();


                //DetailedProteinHitViewResults.Results.PtmSites = ptmSite;
                //if (searchResult.Count != 0)
                //    DetailedProteinHitViewResults.Results.Results = searchResult.First();


                //if (execTime.Count != 0)
                //    DetailedProteinHitViewResults.ExecutionTime = execTime.First();



            }
            return DetailedProteinHitViewResults;
        }

        private SearchParameter GetSearchParametersDtoModel(SearchParameter searchParameters)
        {
            var searchParameter = new SearchParameter
            {
                QueryId = searchParameters.QueryId,
                Autotune = searchParameters.Autotune,
                DenovoAllow = searchParameters.DenovoAllow,
                MassMode = searchParameters.MassMode,
                FilterDb = searchParameters.FilterDb,
                HandleIons = searchParameters.HandleIons,
                HopThreshhold = searchParameters.HopThreshhold,
                HopTolUnit = searchParameters.HopTolUnit,
                InsilicoFragType = searchParameters.InsilicoFragType,
                UserId = searchParameters.UserId,
                Title = searchParameters.Title,
                ProteinDatabase = searchParameters.ProteinDatabase,
                PtmTolerance = searchParameters.PtmTolerance,
                MinimumPstLength = searchParameters.MinimumPstLength,
                MaximumPstLength = searchParameters.MaximumPstLength,
                MwTolerance = searchParameters.MwTolerance,
                MwSweight = searchParameters.MwSweight,
                PstSweight = searchParameters.PstSweight,
                InsilicoSweight = searchParameters.InsilicoSweight,
                NumberOfOutputs = searchParameters.NumberOfOutputs,
                PtmAllow = searchParameters.PtmAllow,
                NeutralLoss = searchParameters.NeutralLoss,
                PSTTolerance = searchParameters.PSTTolerance,


                PeptideTolerance = searchParameters.PeptideTolerance,
                PeptideToleranceUnit = searchParameters.PeptideToleranceUnit,
                TerminalModification = searchParameters.TerminalModification,
                SliderValue = searchParameters.SliderValue,
                CysteineChemicalModification = searchParameters.CysteineChemicalModification,
                MethionineChemicalModification = searchParameters.MethionineChemicalModification,
                EmailId = searchParameters.EmailId,
                Truncation = searchParameters.Truncation,
                FDRCutOff = searchParameters.FDRCutOff,
                JobSubmission = searchParameters.JobSubmission

            };
            return searchParameter;
        }

        public void StoringCompiledResults(List<ResultsDownloadDataCompile> CompiledResults)
        {

        }
        public SearchParameter GetSearchParmeters(string qid)
        {
            var SearchParameter = new SearchParameter();
            using (new PerceptronDatabaseEntities())
            {
                var sqlConnection1 =
                    new SqlConnection(
                        "Server= CHIRAGH-I; Database= PerceptronDatabase; Integrated Security=SSPI;");
                var cmd = new SqlCommand
                {
                    CommandText =
                        "SELECT QueryId \nFROM SearchResults \nWHERE QueryId = '" + qid + "'",
                    CommandType = CommandType.Text,
                    Connection = sqlConnection1
                };
                sqlConnection1.Open();

                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                    qid = dataReader["QueryId"].ToString();

                dataReader.Close();
                cmd.Dispose();
                sqlConnection1.Close();
            }
            using (var db = new PerceptronDatabaseEntities())
            {
                var searchParameters = db.SearchParameters.Where(x => x.QueryId == qid).ToList();

                if (searchParameters.Count != 0)
                    SearchParameter = searchParameters.Any() ? GetSearchParametersDtoModel(searchParameters.First()) : new SearchParameter();

            }
            return SearchParameter;
        }

        public string UpdatingDatabase(string DatabaseName, List<FastaReaderProteinDataDto> FastaReaderProteinInfo)  //For Updating Database Enteries by FastaReader (.fasta to SQL query)
        {
            string Message = "Database Successfully Updated";

            try
            {
                string ConnetionString = "Data Source=CHIRAGH-I;Initial Catalog=" + DatabaseName + ";Integrated Security=True";
                SqlConnection Connection = new SqlConnection(ConnetionString);
                Connection.Open();

                var Query0 = "DELETE FROM " + DatabaseName + ".dbo.ProteinInfoes";      //Deleting previous data from the Database to avoid error like "Duplicating Primary Key Rule"
                var Command0 = new SqlCommand(Query0, Connection);
                Command0.ExecuteNonQuery();

                var QueryInfo = "";


                for (int index = 0; index < FastaReaderProteinInfo.Count; index++)
                {
                    QueryInfo = "Insert INTO " + DatabaseName + ".dbo.ProteinInfoes (ID, ProteinDescription, MW, Seq, Insilico, InsilicoR) Values ('"
                     + FastaReaderProteinInfo[index].ID + "','" + FastaReaderProteinInfo[index].ProteinDescription + "','" + FastaReaderProteinInfo[index].MolecularWeight + "','" + FastaReaderProteinInfo[index].Sequence + "','" + FastaReaderProteinInfo[index].InsilicoLeft + "','" + FastaReaderProteinInfo[index].InsilicoRight + "')";

                    var Command = new SqlCommand(QueryInfo, Connection);
                    Command.ExecuteNonQuery();
                }

                Connection.Close();
            }

            catch (Exception e)
            {
                Message = "Error";
            }
            return Message;
        }

        public List<FastaWriterProteinDataDto> ReadingDataBase(string DatabaseName)  //For Downloading Database Enteries by FastaWriter (SQL Database to .fasta)
        {
            var FastaWriterProteinInfo = new List<FastaWriterProteinDataDto>();
            try
            {
                using (new PerceptronDatabaseEntities())
                {
                    var sqlConnection1 =
                        new SqlConnection(
                            "Server= CHIRAGH-I; Database= " + DatabaseName + "; Integrated Security=SSPI;");
                    var cmd = new SqlCommand
                    {
                        CommandText =
                            "SELECT ProteinDescription, Seq \nFROM ProteinInfoes",
                        CommandType = CommandType.Text,
                        Connection = sqlConnection1
                    };
                    sqlConnection1.Open();

                    var dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                    {
                        var tempFastaWriterProteinInfo = new FastaWriterProteinDataDto()
                        {
                            ProteinDescription = dataReader["ProteinDescription"].ToString(),
                            Sequence = dataReader["Seq"].ToString(),
                        };
                        FastaWriterProteinInfo.Add(tempFastaWriterProteinInfo);
                    }

                    dataReader.Close();
                    cmd.Dispose();
                    sqlConnection1.Close();
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return FastaWriterProteinInfo;
        }


        //private SearchParameter GetresultInsilicoLeftDtoModel(List<ResultInsilicoMatchLeft> resultInsilicoLeft)
        //{
        //    var searchParameter = new ResultInsilicoMatchLeft
        //    {
        //        //ResultId = resultInsilicoLeft.r
        //    };
        //    return searchParameter;
        //}
    }
}

//try
//{
//    PtmVarMod.AddRange(db.PtmVariableModifications.Where(x => x.QueryId == qid));
//}
//catch
//{
//    PtmVarMod = new List<PtmVariableModification>();
//}

//try
//{
//    PtmFixedMod.AddRange(db.PtmFixedModifications.Where(x => x.QueryId == qid));
//}
//catch
//{
//    PtmFixedMod = new List<PtmFixedModification>();
//}

//if (PtmVarMod.Count != 0 || PtmFixedMod.Count != 0)  //searchParameters[0].PtmAllow != "False" || 
//{
//ListOfSearchResults.Select(x => x.ResultId).Distinct();

//public List<string> ScanResultsAgainstFileUniqueId(string qid, string FileId)
//{
//    var FileUniqueIdsList = new List<string>();
//    var R = new List<string>();
//    using (new PerceptronDatabaseEntities())
//    {
//        var sqlConnection1 =
//            new SqlConnection(
//                "Server= CHIRAGH-I; Database= PerceptronDatabase; Integrated Security=SSPI;");
//        var cmd = new SqlCommand
//        {
//            CommandText =

//                "SELECT ResultId \nFROM SearchResults \nWHERE QueryId = '" + qid + "' And FileUniqueId = '" + FileId + "'",

//            CommandType = CommandType.Text,
//            Connection = sqlConnection1
//        };
//        sqlConnection1.Open();

//        var dataReader = cmd.ExecuteReader();
//        while (dataReader.Read())
//        {
//            FileUniqueIdsList.Add(dataReader["FileUniqueId"].ToString());
//            R.Add(dataReader["ResultId"].ToString());
//        }

//        dataReader.Close();
//        cmd.Dispose();
//        sqlConnection1.Close();
//    }
//    return FileUniqueIdsList;
//}



///////////////PREVIOUSLY, WAS USED IN SCAN RESULTS
//////if (j == t.Count())
//////    break;
//////int startPos = t[j].FileName.LastIndexOf("C:\\inetpub\\wwwroot\\PerceptronAPI\\App_Data\\") + "C:\\inetpub\\wwwroot\\PerceptronAPI\\App_Data\\".Length;
//////int length = t[j].FileName.Length - startPos;
//////t[j].FileName = t[j].FileName.Substring(startPos, length);

//////for (int index = 0; index < FileSpecificData.Count; index++)    //Loop & Conditional Statement Required to Filter the files. Only those files will be selected having "Search Results" into the Database.
//////{
//////    if (FileSpecificData[index].FileUniqueId == t[j].FileUniqueId)
//////    {
//////        var temp = new ScanResults
//////        {
//////            FileId = "1",//dataReader["FileId"].ToString(),
//////            FileName = t[j].FileName,
//////            Frags = 1,
//////            Mods = 1,
//////            MolW = (double)dataReader["Mw"],
//////            Truncation = "No",
//////            ProteinId = dataReader["Header"].ToString(),
//////            Score = (double)dataReader["Score"],
//////            FileUniqueId = t[j].FileUniqueId
//////        };
//////        scanResults.Add(temp);
//////        break;
//////    }
//////}

//////++j;