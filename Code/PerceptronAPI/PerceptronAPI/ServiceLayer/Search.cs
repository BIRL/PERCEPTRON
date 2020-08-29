//using PerceptronAPI.Models;
//using PerceptronAPI.Repository;

//namespace PerceptronAPI.ServiceLayer
//{
//    public class Search
//    {
//        static MySqlDatabase Db = new MySqlDatabase();
        
//        /// <summary>
//        /// UseSQLDB.
//        /// Assign  MIssing params
//        /// SQLDB -> StoreSearchParameters
//        /// </summary>
//        /// <param name="parameters"></param>
//        /// <returns></returns>


//        public static string ProteinSearch(SearchParametersDto parameters)
//        {
//            //var newJob = new Job
//            //{
//            //    Qid = parameters.Queryid,
//            //    Uid = parameters.UserId,
//            //    Progress = "Initiating Search!"
//            ////};
//            //CurrentJobs.Initialize();
//            //CurrentJobs.current.Add(newJob);
//            //int q = Db.QueryStore(parameters);
//            //if (q == -1)
//            //    return "Not ok";
//            SqlDatabase p=new SqlDatabase();
//            p.StoreSearchParameters(parameters);
//            return "ok";
//        }

//        public static string ProteinSearch1(SearchParametersDto parameters)
//        {
//            //var newJob = new Job
//            //{
//            //    Qid = parameters.Queryid,
//            //    Uid = parameters.UserId,
//            //    Progress = "Initiating Search!"
//            ////};
//            //CurrentJobs.Initialize();
//            //CurrentJobs.current.Add(newJob);
//            //int q = Db.QueryStore(parameters);
//            //if (q == -1)
//            //    return "Not ok";
//            SqlDatabase p = new SqlDatabase();
//            p.StoreSearchFiles(parameters);
//            return "ok";
//        }


//        public static string Progress_reporter(string qid)
//        {
//            // DBConnect db = new DBConnect();
//            //return db.Get_Progress(qid).ToString();
//            //string pg = "-1";
//            //if (CurrentJobs.current != null)
//            //{
//            //    for (int i = 0; i < CurrentJobs.current.Count; i++)
//            //    {
//            //        if (CurrentJobs.current[i].qid == qid)
//            //            pg = CurrentJobs.current[i].progress;
//            //        if (pg == "Done")
//            //            CurrentJobs.current.RemoveAt(i);
//            //    }
//            //}
//            return "1";
//        }
//    }
//}