using System;
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
        public void StoreSearchParameters(SearchParametersDto param)
        {
            using (var db = new PerceptronDatabaseEntities())
            {
                db.SearchQueries.Add(param.SearchQuerry);
                db.SearchParameters.Add(param.SearchParameters);
                db.SearchFiles.AddRange(param.SearchFiles);
                db.PtmFixedModifications.Add(param.FixedMods);
                db.PtmVariableModifications.Add(param.VarMods);

                db.SaveChanges();

            }
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
            using (var db = new PerceptronDatabaseEntities())
            {
                var sqlConnection1 =
                    new SqlConnection(
                        "Server= CHIRAGH-II; Database= PerceptronDatabase; Integrated Security=SSPI;");
                var cmd = new SqlCommand
                {
                    CommandText =
                        "SELECT *\nFROM SearchResults E\nWHERE E.QueryId = '" + qid +
                        "' AND ABS(E.score - (SELECT max(E2.score)  FROM SearchResults E2 Where E2.QueryId=E.QueryId AND E2.FileId = E.FileId)) <= 0.01",
                    CommandType = CommandType.Text,
                    Connection = sqlConnection1
                };
                sqlConnection1.Open();
                var t = db.SearchFiles.Where(x => x.QueryId == qid).ToList();
                //var parameters = db.SearchParameters.Where(x => x.QueryId == qid).ToList().First();
                int j = 0;
                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    if (j == t.Count())
                        break;
                    int startPos = t[j].FileName.LastIndexOf("C:\\inetpub\\wwwroot\\PerceptronAPI\\App_Data\\") + "C:\\inetpub\\wwwroot\\PerceptronAPI\\App_Data\\".Length;
                    int length = t[j].FileName.Length - startPos;
                    t[j].FileName = t[j].FileName.Substring(startPos, length);
                    var temp = new ScanResults
                    {
                        FileId = dataReader["FileId"].ToString(),
                        FileName = t[j].FileName,
                        Frags = 1,
                        Mods = 1,
                        MolW = (double)dataReader["Mw"],
                        Truncation = "No",
                        ProteinId = dataReader["Header"].ToString(),
                        Score = (double)dataReader["Score"],
                        FileUniqueId = t[j].FileUniqueId
                    };
                    ++j;
                    scanResults.Add(temp);
                }
                dataReader.Close();
                cmd.Dispose();
                sqlConnection1.Close();
            }

            return scanResults;
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
                        "Server= CHIRAGH-II; Database= PerceptronDatabase; Integrated Security=SSPI;");
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
                    int NoOfPtmSites = 0;
                    if (dbInfo.ResultPtmSites.Where(x => x.ResultId == ResultId).Count() != 0)
                        NoOfPtmSites = dbInfo.ResultPtmSites.Where(x => x.ResultId == ResultId).First().Index.Split(',').Select(int.Parse).ToList().Count;  // Just wanted to get Number of Ptm Sites
                    summaryResults[i].Mods = NoOfPtmSites;  // Updating the actual value of "Mods" here...
                }
            }
            return summaryResults;
        }

        public List<UserHistory> GetUserHistory(string Uid)
        {

            var summaryResults = new List<UserHistory>();
            using (new PerceptronDatabaseEntities())
            {
                var sqlConnection1 =
                    new SqlConnection(
                        "Server= CHIRAGH-II; Database= PerceptronDatabase; Integrated Security=SSPI;");
                var cmd = new SqlCommand
                {
                    CommandText =
                        "SELECT P.Queryid, P.Title, R.CreationTime \nFROM SearchQueries as R, SearchParameters as P \nWHERE P.UserId = '" + Uid + "'AND P.Queryid=R.QueryId ",
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
                        time = dataReader["CreationTime"].ToString()
                    };
                    summaryResults.Add(temp);
                }
                dataReader.Close();
                cmd.Dispose();
                sqlConnection1.Close();
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
                        "Server= CHIRAGH-II; Database= PerceptronDatabase; Integrated Security=SSPI;");
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
                        "Server= CHIRAGH-II; Database= PerceptronDatabase; Integrated Security=SSPI;");

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
            using (new PerceptronDatabaseEntities())
            {
                var sqlConnection1 =
                    new SqlConnection(
                        "Server= CHIRAGH-II; Database= PerceptronDatabase; Integrated Security=SSPI;");
                var cmd = new SqlCommand
                {
                    CommandText =
                        "SELECT QueryId \nFROM SearchResults \nWHERE ResultId = '" + rid + "'",
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
                var searchResult = db.SearchResults.Where(x => x.ResultId == rid).ToList();
                //var resultInsilicoLeft = db.ResultInsilicoMatchLefts.Where(x => x.ResultId == rid).ToList();
                //var resultInsilicoRight = db.ResultInsilicoMatchRights.Where(x => x.ResultId == rid).ToList();
                
                //var ptmVarmod = db.PtmVariableModifications.First(x => x.QueryId == qid);
                //var ptmFixedmod = db.PtmFixedModifications.First(x => x.QueryId == qid);

                var ptmSite = db.ResultPtmSites.Where(x => x.ResultId == rid).ToList();
                var execTime = db.ExecutionTimes.Where(x => x.QueryId == qid).ToList();
                var searchQuery = db.SearchQueries.Where(x => x.QueryId == qid).ToList();

                int NoOfPtmSites = 0;
                if (db.ResultPtmSites.Where(x => x.ResultId == rid).Count() != 0)
                    NoOfPtmSites = db.ResultPtmSites.Where(x => x.ResultId == rid).First().Index.Split(',').Select(int.Parse).ToList().Count;  // Just wanted to get Number of Ptm 


                if (searchParameters.Count != 0)
                    detiledResults.Paramters.SearchParameters = searchParameters.Any() ? GetSearchParametersDtoModel(searchParameters.First()) : new SearchParameter();

                //detiledResults.Paramters.FixedMods = ptmFixedmod;
                //detiledResults.Paramters.VarMods = ptmVarmod;

                if (searchQuery.Count != 0)
                    detiledResults.Paramters.SearchQuerry = searchQuery.First();

                //detiledResults.Results.InsilicioLeft = resultInsilicoLeft;
                //detiledResults.Results.InsilicoRight = resultInsilicoRight;


                detiledResults.Results.NoOfPtmSites = NoOfPtmSites;
                if (searchResult.Count != 0)
                    detiledResults.Results.Results = searchResult.First();
                if (execTime.Count != 0)
                    detiledResults.ExecutionTime = execTime.First();



            }
            return detiledResults;
        }
        //DownloadAllResults
        public ScanResultsDownloadDataDto ScanResultsDownloadData(string qid)
        {
            var tempScanResultsDownloadDataDto = new ScanResultsDownloadDataDto();
            var FileUniqueIdsList = new List<string>();
            var FileNamesList = new List<string>();
            var ListOfPeakData = new List<PeakListData>();
            var ListOfSearchResults = new List<SearchResult>();
            var ResultIds = new List<string>();
            using (new PerceptronDatabaseEntities())
            {
                var sqlConnection1 =
                    new SqlConnection(
                        "Server= CHIRAGH-II; Database= PerceptronDatabase; Integrated Security=SSPI;");
                var cmd = new SqlCommand
                {
                    CommandText =

                        "SELECT FileUniqueId, FileName \nFROM SearchFiles\nWHERE QueryId = '" + qid + "'",

                    CommandType = CommandType.Text,
                    Connection = sqlConnection1
                };
                sqlConnection1.Open();

                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    FileUniqueIdsList.Add(dataReader["FileUniqueId"].ToString());
                    FileNamesList.Add(dataReader["FileName"].ToString());
                }

                dataReader.Close();
                cmd.Dispose();
                sqlConnection1.Close();
            }
            using (var db = new PerceptronDatabaseEntities())
            {
                var searchParameters = db.SearchParameters.Where(x => x.QueryId == qid).ToList();
                
                ListOfSearchResults.AddRange(db.SearchResults.Where(x => x.QueryId == qid));
                var NoOfResultIds = ListOfSearchResults.Select(x => x.ResultId).Distinct().ToList();

                var PtmSites = new List<ResultPtmSite>();
                for (int i = 0; i < ListOfSearchResults.Count; i++)
                {
                    var ResultId = ListOfSearchResults[i].ResultId;
                    PtmSites.AddRange(db.ResultPtmSites.Where(x => x.ResultId == ResultId));
                }
                var PtmVarMod = new List<PtmVariableModification>();
                var PtmFixedMod = new List<PtmFixedModification>();
                
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
                    //ListOfSearchResults.Select(x => x.ResultId).Distinct();(x 
                    
                
                tempScanResultsDownloadDataDto = new ScanResultsDownloadDataDto(FileUniqueIdsList, FileNamesList, ListOfSearchResults, searchParameters.First(), PtmSites);
            }
            return tempScanResultsDownloadDataDto;
        }

        
        

        public DetailedProteinHitView DetailedProteinHitView_Results(string qid, string rid)
        {
            string FileId = ""; var GetPeakListData = new PeakListData();
            var DetailedProteinHitViewResults = new DetailedProteinHitView();
            using (new PerceptronDatabaseEntities())
            {
                var sqlConnection1 =
                    new SqlConnection(
                        "Server= CHIRAGH-II; Database= PerceptronDatabase; Integrated Security=SSPI;");
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
                Truncation = searchParameters.Truncation

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
                        "Server= CHIRAGH-II; Database= PerceptronDatabase; Integrated Security=SSPI;");
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


        public Test GetImagePathMassSpectrum(string qid)
        {
            var InfoPath = new Test();
            string Path = "";
            using (new PerceptronDatabaseEntities())
            {
                var sqlConnection1 =
                    new SqlConnection(
                        "Server= CHIRAGH-II; Database= PerceptronDatabase; Integrated Security=SSPI;");
                var cmd = new SqlCommand
                {
                    CommandText =
                        "SELECT Path \nFROM Test \nWHERE QueryId = '" + qid + "'",
                    CommandType = CommandType.Text,
                    Connection = sqlConnection1
                };
                sqlConnection1.Open();

                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                    Path = dataReader["Path"].ToString();

                dataReader.Close();
                cmd.Dispose();
                sqlConnection1.Close();
            }
            InfoPath.Path = Path;
            InfoPath.QueryId = qid;
            return InfoPath;

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

//public List<string> ScanResultsAgainstFileUniqueId(string qid, string FileId)
//{
//    var FileUniqueIdsList = new List<string>();
//    var R = new List<string>();
//    using (new PerceptronDatabaseEntities())
//    {
//        var sqlConnection1 =
//            new SqlConnection(
//                "Server= CHIRAGH-II; Database= PerceptronDatabase; Integrated Security=SSPI;");
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