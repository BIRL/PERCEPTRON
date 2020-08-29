//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Globalization;
//using System.IO;
//using System.IO.Compression;
//using System.Linq;
//using System.Text;
//using MySql.Data.MySqlClient;
//using PerceptronLocalService.Interfaces;
//using PerceptronLocalService.DTO;

//namespace PerceptronLocalService.Repository
//{


//    public class MySqlDatabase : IDataAccessLayer
//    {
//        private MySqlConnection _connection;

//        //Constructor
//        public MySqlDatabase()
//        {
//            Initialize();
//        }

//        //Initialize values
//        private void Initialize()
//        {
//            const string connectionString = "Data Source=localhost;" +
//                                            "Initial Catalog=Protein;" +
//                                            "User id= root;" +
//                                            "Password=********;";
//            _connection = new MySqlConnection(connectionString);
//        }

//        //open _connection to database
//        private bool OpenConnection()
//        {
//            try
//            {
//                _connection.Open();
//                return true;
//            }
//            catch (MySqlException ex)
//            {
//                //When handling errors, you can your application's response based 
//                //on the error number.
//                //The two most common error numbers when connecting are as follows:
//                //0: Cannot connect to server.
//                //1045: Invalid user name and/or password.
//                switch (ex.Number)
//                {
//                    case 0:
//                        Console.WriteLine("Cannot connect to server.  Contact administrator");
//                        break;

//                    case 1045:
//                        Console.WriteLine("Invalid username/password, please try again");
//                        break;
//                    default:
//                        Console.WriteLine("Error!");
//                        break;
//                }
//                return false;
//            }
//        }


//        private void CloseConnection()
//        {
//            try
//            {
//                _connection.Close();
//            }
//            catch (MySqlException)
//            {
//            }
//        }



//        public string GetEmailFromUserId(string id)
//        {
//            var query = "select Email from user where UserId='" + id + "';";
//            string returnString;
//            if (OpenConnection() != true)
//            {
//                returnString = "_connection failed!";
//            }
//            else
//            {
//                try
//                {
//                    var cmd = new MySqlCommand(query, _connection);
//                    var reader = cmd.ExecuteReader();
//                    returnString = reader.Read() ? reader[0].ToString() : "Incorrect Username or Password!";
//                    _connection.Close();
//                }
//                catch (Exception e)
//                {
//                    returnString = e.Message;
//                }
//            }

//            return returnString;
//        }


//        //Contains unmodified left Insilico masses
//        public void InsertLeftInsilicoMasses(string pid, double mw, string ions)
//        {
//            var query3 = "INSERT INTO leftions VALUES";
//            query3 += "('" + pid + "'," + mw + ",'" + ions + "');";
//            if (OpenConnection())
//            {
//                var cmd2 = new MySqlCommand(query3, _connection);
//                cmd2.ExecuteNonQuery();
//                _connection.Close();
//            }
//        }

//        //Contains unmodified right Insilico masses
//        public void InsertRightIsilicoMasses(string pid, double mw, string ions)
//        {
//            var query3 = "INSERT INTO rightions VALUES";
//            query3 += "('" + pid + "'," + mw + ",'" + ions + "');";
//            if (OpenConnection())
//            {
//                var cmd2 = new MySqlCommand(query3, _connection);
//                cmd2.ExecuteNonQuery();
//                _connection.Close();
//            }
//        }

//        //Store Peak List Data
//        public void StorePeakList(string FileUniqueId, string peakDataMassesString, string peakDataIntensitiesString)
//        {
//            if (OpenConnection())
//            {
//                var query = "INSERT INTO [PerceptronDatabase].[dbo].[PeakListData] VALUES ('" + FileUniqueId + "', '" + peakDataMassesString + "', '" + peakDataIntensitiesString + "')";
//                var cmd = new MySqlCommand(query, _connection);
//                try
//                {
//                    cmd.ExecuteNonQuery();
//                }
//                catch (MySqlException e)
//                {

//                }

//                CloseConnection();
//            }

//        }

//        //Store results
//        public string StoreResults(SearchResultsDto res, string abcd, string FileUniqueId, int fileId)
//        {
//            var resultString = "You are Registered!";
//            if (OpenConnection())
//            {
//                var query0 = "INSERT INTO timings VALUES('" + res.QueryId + "','" + res.Times.InsilicoTime + "','" +
//                             res.Times.PtmTime + "','" + res.Times.TunerTime + "','" + res.Times.MwFilterTime +
//                             "','" + res.Times.PstTime + "','" + res.Times.TotalTime + "','" + abcd + "');";
//                var cmd1 = new MySqlCommand(query0, _connection);
//                try
//                {
//                    cmd1.ExecuteNonQuery();

//                    foreach (ProteinDto protein in res.FinalProt)
//                    {
//                        var resId = Guid.NewGuid();
//                        string headerTag;
//                        if (protein.Header[0] == '>')
//                        {
//                            headerTag = protein.Header.Contains('|')
//                                ? protein.Header.Substring(4, 6)
//                                : protein.Header.Substring(1, protein.Header.Length - 1);
//                        }
//                        else
//                            headerTag = protein.Header;


//                        var query = "INSERT INTO results VALUES('" + resId + "','" + res.QueryId + "','" +
//                                    headerTag + "','" + protein.Sequence + "'," + protein.PstScore +
//                                    "," + protein.InsilicoScore + ","
//                                    + protein.PtmScore + "," + protein.Score + "," +
//                                    protein.MwScore + "," + protein.Mw + "," + fileId + ")";
//                        var cmd = new MySqlCommand(query, _connection);
//                        cmd.ExecuteNonQuery();


//                        MySqlCommand cmd2;
//                        var temp = "";
//                        string query3;
//                        if (protein.PtmParticulars != null)
//                        {
//                            foreach (var ptmSite in protein.PtmParticulars)
//                            {
//                                temp = ptmSite.AminoAcid.Aggregate(temp, (current, t) => current + t);
//                                query3 = "INSERT INTO ptm_sites VALUES('" + resId + "'," +
//                                         ptmSite.Index + "," +
//                                         ptmSite.Score + ","
//                                         + ptmSite.ModWeight + ",'" +
//                                         ptmSite.ModName + "','" +
//                                         ptmSite.Site + "','" + temp + "');";
//                                //string query2 = "update logindata set guid ='" + g + "'where id ='" + id + "';";
//                                cmd2 = new MySqlCommand(query3, _connection);
//                                cmd2.ExecuteNonQuery();
//                            }
//                        }

//                        //////////////////////////////////////
//                        var matchLeft = String.Join(",",
//                            protein.InsilicoDetails.PeaklistMassLeft.Select(
//                                x => x.ToString(CultureInfo.InvariantCulture)).ToArray());
//                        query3 = "INSERT INTO insilico_matches_left VALUES ('" + resId + "','" + matchLeft + "')";
//                        cmd2 = new MySqlCommand(query3, _connection);
//                        cmd2.ExecuteNonQuery();

//                        var matchRight = String.Join(",",
//                            protein.InsilicoDetails.PeaklistMassRight.Select(
//                                x => x.ToString(CultureInfo.InvariantCulture)).ToArray());
//                        query3 = "INSERT INTO insilico_matches_right VALUES ('" + resId + "','" + matchRight + "')";
//                        cmd2 = new MySqlCommand(query3, _connection);
//                        cmd2.ExecuteNonQuery();
//                    }
//                }
//                catch (MySqlException e)
//                {
//                    resultString = e.Message;
//                }
//                CloseConnection();
//            }
//            else
//            {
//                resultString = "Unfortunatenly you can not register.";
//            }


//            return resultString;
//        }

//        //Gets Server Status
//        public List<SearchQueryDto> ServerStatus()
//        {
//            var result = new List<SearchQueryDto>();
//            const string query = "select * from server_status where progress = 0 ORDER BY time ASC;";

//            if (!OpenConnection()) return result;
//            try
//            {
//                var cmd = new MySqlCommand(query, _connection);
//                var reader = cmd.ExecuteReader();
//                while (reader.Read())
//                {
//                    var temp = new SearchQueryDto
//                    {
//                        QueryId = reader["qID"].ToString(),
//                        UserId = reader["uID"].ToString(),
//                        Progress = reader["progress"].ToString(),
//                        CreationTime = reader["time"].ToString()
//                    };
//                    result.Add(temp);
//                }
//                reader.Close();
//            }
//            catch (Exception)
//            {
//                // ignored
//            }
//            _connection.Close();
//            return result;
//        }

//        //Get Query
//        public SearchParametersDto GetParameters(string qid)
//        {
//            var query = "SELECT * FROM proteomics.query where QueryId='" + qid + "';";
//            var qp = new SearchParametersDto();
//            if (!OpenConnection()) return qp;
//            var cmd = new MySqlCommand(query, _connection);
//            var reader = cmd.ExecuteReader();
//            if (reader.Read())
//            {
//                qp.Queryid = reader.GetValue(0).ToString();
//                qp.EmailId = reader.GetValue(1).ToString();
//                qp.Title = reader.GetValue(2).ToString();
//                qp.ProtDb = reader.GetValue(3).ToString();
//                qp.InsilicoFragType = reader.GetValue(4).ToString();
//                qp.FilterDb = Convert.ToInt32(reader.GetValue(5));
//                qp.PtmTolerance = Convert.ToDouble(reader.GetValue(6));
//                qp.MwTolUnit = reader.GetValue(7).ToString();
//                qp.MwTolerance = Convert.ToDouble(reader.GetValue(8));
//                qp.HopThreshhold = Convert.ToDouble(reader.GetValue(9));
//                qp.MinimumPstLength = Convert.ToInt32(reader.GetValue(10));
//                qp.MaximumPstLength = Convert.ToInt32(reader.GetValue(11));
//                qp.GuiMass = Convert.ToDouble(reader.GetValue(12));
//                qp.HandleIons = reader.GetValue(13).ToString();
//                qp.Autotune = Convert.ToInt32(reader.GetValue(14));
//                qp.HopTolUnit = reader.GetValue(15).ToString();
//                qp.MwSweight = Convert.ToDouble(reader.GetValue(16));
//                qp.PstSweight = Convert.ToDouble(reader.GetValue(17));
//                qp.InsilicoSweight = Convert.ToDouble(reader.GetValue(18));
//                qp.NumberOfOutputs = Convert.ToInt32(reader.GetValue(19));
//                qp.DenovoAllow = Convert.ToInt32(reader.GetValue(20));
//                qp.PtmAllow = Convert.ToInt32(reader.GetValue(21));
//                qp.NeutralLoss = Convert.ToDouble(reader.GetValue(22));
//                qp.PSTTolerance = Convert.ToDouble(reader.GetValue(23));
//                qp.PeptideTolerance = Convert.ToDouble(reader.GetValue(24));
//                qp.PeptideToleranceUnit = reader.GetValue(25).ToString();
//                qp.TerminalModification = reader.GetValue(26).ToString();  // 26 to 29
//                qp.Truncation = Convert.ToInt32(reader.GetValue(27));

//                qp.SliderValue = Convert.ToDouble(reader.GetValue(28));
//                qp.CysteineChemicalModification = reader.GetValue(29).ToString();
//                qp.MethionineChemicalModification = reader.GetValue(30).ToString();

//            }

//            reader.Close();
//            _connection.Close();

//            GetFiles(qp);
//            return qp;
//        }

//        //Get Data files
//        public void GetFiles(SearchParametersDto qp)
//        {
//            var files = new List<string>();
//            var type = new List<string>();
//            var query = "SELECT * FROM proteomics.data_file where QueryId = '" + qp.Queryid + "';";
//            if (!OpenConnection()) return;
//            var cmd = new MySqlCommand(query, _connection);
//            var reader = cmd.ExecuteReader();
//            while (reader.Read())
//            {
//                var file = reader["File"].ToString();
//                type.Add(reader["Type"].ToString());
//                files.Add(file);
//            }
//            qp.FileType = type.ToArray();
//            qp.PeakListFileName = files.ToArray();
//            _connection.Close();
//        }

//        //Sets the progress of Querry
//        public int Set_Progress(string qid, int p)
//        {
//            var query = "UPDATE server_status SET progress=" + p + " Where qID='" + qid + "';";
//            var succcess = 1;

//            if (!OpenConnection()) return succcess;
//            try
//            {
//                var cmd = new MySqlCommand(query, _connection);
//                var reader = cmd.ExecuteReader();
//                if (reader.Read())
//                {
//                    succcess = Convert.ToInt32(reader.GetValue(0));
//                    reader.Close();
//                }
//                else
//                    succcess = -1;
//            }
//            catch (Exception e)
//            {
//                Debug.WriteLine(e.Message);
//                succcess = -1;
//            }
//            _connection.Close();
//            return succcess;
//        }


//        private static string Decompress(string compressedText)
//        {
//            var gzBuffer = Convert.FromBase64String(compressedText);
//            using (var ms = new MemoryStream())
//            {
//                var msgLength = BitConverter.ToInt32(gzBuffer, 0);
//                ms.Write(gzBuffer, 4, gzBuffer.Length - 4);
//                var buffer = new byte[msgLength];
//                ms.Position = 0;
//                using (var zip = new GZipStream(ms, CompressionMode.Decompress))
//                {
//                    zip.Read(buffer, 0, buffer.Length);
//                }
//                return Encoding.UTF8.GetString(buffer);
//            }
//        }
//    }
//}
