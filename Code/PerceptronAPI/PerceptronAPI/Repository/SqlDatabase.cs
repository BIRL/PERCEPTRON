using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using PerceptronAPI.Models;
using System.ComponentModel.DataAnnotations;  //Added on 12Sep2019.. WHy its not needed before...???

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
                //db.PtmFixedModifications.AddRange(param.FixedMods);
                //db.PtmVariableModifications.AddRange(param.VarMods);
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
                    InsilicioLeft = db.ResultInsilicoMatchLefts.Where(x => x.ResultId == rid).ToList(),
                    InsilicoRight = db.ResultInsilicoMatchRights.Where(x => x.ResultId == rid).ToList(),
                    PtmSites = db.ResultPtmSites.Where(x => x.ResultId == rid).ToList(),
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
                        FixedMods = db.PtmFixedModifications.Where(x => x.QueryId == qid).ToList(),
                        SearchFiles = db.SearchFiles.Where(x => x.QueryId == qid).ToList(),
                        SearchQuerry = db.SearchQueries.First(x => x.QueryId == qid),
                        VarMods = db.PtmVariableModifications.Where(x => x.QueryId == qid).ToList()
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
                        Score = (double)dataReader["Score"]
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
                        Mods = 2,
                        MolW = (double)dataReader["Mw"],
                        ProteinName = "Protein "+j,
                        ProteinRank = 1,
                        ResultId = dataReader["ResultId"].ToString(),
                        Score = (double)dataReader["Score"],
                        TerminalMods = "no"
                    };
                    ++j;
                    summaryResults.Add(temp);
                }
                dataReader.Close();
                cmd.Dispose();
                sqlConnection1.Close();
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
                        time=dataReader["CreationTime"].ToString()
                    };
                    summaryResults.Add(temp);
                }
                dataReader.Close();
                cmd.Dispose();
                sqlConnection1.Close();
            }

           
            return summaryResults.OrderByDescending(x=> x.time).ToList();
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
                    CommandText =
                        "Select count( QueryId) as two FROM SearchResults",
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
                var resultInsilicoLeft = db.ResultInsilicoMatchLefts.Where(x => x.ResultId == rid).ToList();
                var resultInsilicoRight = db.ResultInsilicoMatchRights.Where(x => x.ResultId == rid).ToList();
                var ptmVarmod = db.PtmVariableModifications.Where(x => x.QueryId == qid).ToList();
                var ptmFixedmod = db.PtmFixedModifications.Where(x => x.QueryId == qid).ToList();
                var ptmSite = db.ResultPtmSites.Where(x => x.ResultId == rid).ToList();
                var execTime = db.ExecutionTimes.Where(x => x.QueryId == qid).ToList();
                var searchQuery = db.SearchQueries.Where(x => x.QueryId == qid).ToList();


             

                if (searchParameters.Count != 0)
                    detiledResults.Paramters.SearchParameters = searchParameters.Any() ? GetSearchParametersDtoModel(searchParameters.First()) : new SearchParameter();

                detiledResults.Paramters.FixedMods = ptmFixedmod;
                detiledResults.Paramters.VarMods = ptmVarmod;

                if (searchQuery.Count != 0)
                    detiledResults.Paramters.SearchQuerry = searchQuery.First();

                detiledResults.Results.InsilicioLeft = resultInsilicoLeft;
                detiledResults.Results.InsilicoRight = resultInsilicoRight;


                detiledResults.Results.PtmSites = ptmSite;
                if (searchResult.Count != 0)
                    detiledResults.Results.Results = searchResult.First();
                if (execTime.Count != 0)
                    detiledResults.ExecutionTime = execTime.First();



            }
            return detiledResults;
        }
        private SearchParameter GetSearchParametersDtoModel(SearchParameter searchParameters)
        {
            var searchParameter = new SearchParameter
            {
                QueryId = searchParameters.QueryId,
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
                NeutralLoss = searchParameters.NeutralLoss,
                PSTTolerance = searchParameters.PSTTolerance,


                PeptideTolerance = searchParameters.PeptideTolerance,
                PeptideToleranceUnit = searchParameters.PeptideToleranceUnit,
                TerminalModification = searchParameters.TerminalModification,
                SliderValue = searchParameters.SliderValue,
                CysteineChemicalModification = searchParameters.CysteineChemicalModification,
                MethionineChemicalModification = searchParameters.MethionineChemicalModification

            };
            return searchParameter;
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