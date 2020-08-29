//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Globalization;
//using System.IO;
//using System.IO.Compression;
//using System.Linq;
//using System.Text;
//using MySql.Data.MySqlClient;
//using PerceptronAPI.Models;

//namespace PerceptronAPI.Repository
//{
//    public class MySqlDatabase
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
//                                            "Initial Catalog=proteomics;" +
//                                            "User id= root;" +
//                                            "Password=Birl1234;";
//            _connection = new MySqlConnection(connectionString);
//        }

//        //open _connection to database
//        public bool OpenConnection()
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


//        public string Registeration(string id, string pwd, string name, string email, string g)
//        {
//            var returnMessage = "You are Registered!";
//            var query = "INSERT INTO user VALUES('" + id + "','" + name + "','" + email + "','" + pwd + "','" + 0 + "')";

//            //open _connection
//            if (OpenConnection() != true) return returnMessage;
//            //create command and assign the query and _connection from the constructor
//            var cmd = new MySqlCommand(query, _connection);

//            try
//            {
//                //Execute command
//                cmd.ExecuteNonQuery();
//                query = "INSERT INTO session VALUES('" + id + "','" + g + "', now() , Null);";
//                cmd = new MySqlCommand(query, _connection);
//                cmd.ExecuteNonQuery();
//                //Making directory for new User.
//                Directory.CreateDirectory(@"D:\UploadedData\" + id);
//            }
//            catch (MySqlException e)
//            {
//                returnMessage = e.Message;
//            }
//            //close _connection
//            CloseConnection();
//            return returnMessage;
//        }


//        //Update statements
//        public bool UpdatePassword(string newPassword, string id)
//        {
//            var query = "UPDATE user SET Password='" + newPassword + "' WHERE UserId='" + id + "'";
//            //Open _connection
//            if (OpenConnection() != true)
//                return false;
//            bool returnStatus;
//            try
//            {
//                //create mysql command
//                var cmd = new MySqlCommand
//                {
//                    CommandText = query,
//                    Connection = _connection
//                };
//                //Assign the query using CommandText
//                //Assign the _connection using Connection

//                //Execute query
//                cmd.ExecuteNonQuery();
//                returnStatus = true;
//            }
//            catch
//            {
//                returnStatus = false;
//            }

//            CloseConnection();
//            return returnStatus;
//        }

//        public bool UpdateInfo(string newEmail, string id, string newName)
//        {
//            var query = "UPDATE user SET Email='" + newEmail + "', Name='" + newName + "' WHERE UserId='" + id + "'";
//            //Open _connection
//            if (OpenConnection() != true)
//                return false;
//            bool returnStatus;
//            try
//            {
//                //create mysql command
//                var cmd = new MySqlCommand
//                {
//                    CommandText = query,
//                    Connection = _connection
//                };
//                //Assign the query using CommandText
//                //Assign the _connection using Connection

//                //Execute query
//                cmd.ExecuteNonQuery();
//                returnStatus = true;
//            }
//            catch
//            {
//                returnStatus = false;
//            }

//            //close _connection
//            CloseConnection();
//            return returnStatus;
//        }

//        //Login
//        public string Login(string id, string pwd, string g)
//        {
//            string returnString;
//            var query = "select * from user where UserId='" + id + "'and Password='" + pwd + "' and Verified = 1;";

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
//                    if (reader.Read())
//                    {
//                        returnString = "You are Logged In Successfully, Happy Searching!";
//                        reader.Close();
//                        query = "INSERT INTO session VALUES('" + id + "','" + g + "', now() , Null);";
//                        cmd = new MySqlCommand(query, _connection);
//                        cmd.ExecuteNonQuery();
//                    }
//                    else
//                        returnString = "Incorrect Username or Password!";
//                    _connection.Close();
//                }
//                catch (Exception e)
//                {
//                    returnString = e.Message;
//                }
//            }

//            return returnString;
//        }


//        public string GetEmail(string id)
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

//        private static string Decrypt_Password(string encryptpassword)
//        {
//            var encodePwd = new UTF8Encoding();
//            var decode = encodePwd.GetDecoder();
//            var todecodeByte = Convert.FromBase64String(encryptpassword);
//            var charCount = decode.GetCharCount(todecodeByte, 0, todecodeByte.Length);
//            var decodedChar = new char[charCount];
//            decode.GetChars(todecodeByte, 0, todecodeByte.Length, decodedChar, 0);
//            var pwdstring = new string(decodedChar);
//            return pwdstring;
//        }

//        //Activate Account
//        public bool Activate(string email)
//        {
//            string returnString;
//            var mail = Decrypt_Password(email);
//            var query = "select * from user where Email='" + mail + "';";

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
//                    if (reader.Read())
//                    {
//                        returnString = "Login Successfull!";
//                        reader.Close();
//                        query = "UPDATE user SET Verified=" + 1 + " where Email='" + mail + "';";
//                        cmd = new MySqlCommand(query, _connection);
//                        cmd.ExecuteReader();
//                    }
//                    else
//                        returnString = "Incorrect Username or Password!";
//                    _connection.Close();
//                }
//                catch (Exception e)
//                {
//                    returnString = e.Message;
//                }
//            }


//            return returnString == "Login Successfull!";
//        }

//        //Contains unmodified left Insilico masses
//        public void Ileft(string pid, double mw, string ions)
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
//        public void Iright(string pid, double mw, string ions)
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

//        //Store results
//        public string store_results(Results res, string abcd, int fileId)
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

//                    foreach (Proteins protein in res.FinalProt)
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
//                                    headerTag + "','" + protein.Sequence + "'," + protein.EstScore +
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

//        //Validate Session
//        public bool Session_validator(string id, string g)
//        {
//            bool resultState;
//            var query = "select * from session where UserId='" + id + "'and SessionId='" + g + "';";

//            if (!OpenConnection()) return false;
//            try
//            {
//                var cmd = new MySqlCommand(query, _connection);

//                var reader = cmd.ExecuteReader();
//                if (reader.Read())
//                {
//                    resultState = true;
//                    reader.Close();
//                }
//                else
//                    resultState = false;

//                _connection.Close();
//            }
//            catch (Exception e)
//            {
//                Debug.WriteLine(e.Message);
//                resultState = false;
//            }

//            return resultState;
//        }

//        //Validate ID
//        public bool valid_id(string id)
//        {
//            bool resultState;
//            var query = "select * from user where UserId='" + id + "';";

//            if (!OpenConnection()) return false;
//            try
//            {
//                var cmd = new MySqlCommand(query, _connection);

//                var reader = cmd.ExecuteReader();
//                resultState = !reader.Read();
//                _connection.Close();
//            }
//            catch
//            {
//                resultState = false;
//            }
//            return resultState;
//        }

//        //Logout
//        public bool Logout(string id, string guid)
//        {
//            var success = true;
//            if (!OpenConnection()) return false;
//            try
//            {
//                var query = "update session set EndTime = now() where UserId = '" + id + "' and SessionID='" + guid +
//                            "'";
//                var cmd = new MySqlCommand(query, _connection);
//                cmd.ExecuteNonQuery();
//            }
//            catch
//            {
//                success = false;
//            }

//            return success;
//        }

//        //Store Query Parameters
//        //public int QueryStore(SearchParameter param)
//        //{
//        //    var success = false;

//        //    if (OpenConnection())
//        //    {
//        //        try
//        //        {
//        //            success = true;

//        //            //var query1 = "INSERT INTO query VALUES('" + param.Queryid + "','" + param.UserId + "','" +
//        //            //             param.Title + "','" + param.ProtDb + "','" +
//        //            //             param.InsilicoFragType + "','" + param.FilterDb + "','" + param.PtmTolerance +
//        //            //             "','"
//        //            //             + param.MwTolUnit + "','" + param.MwTolerance + "','" + param.HopThreshhold + "','" +
//        //            //             param.MinimumEstLength + "','" + param.MaximumEstLength + "','" + param.GuiMass +
//        //            //             "','" + param.HandleIons + "'," + param.Autotune + ",'" + param.HopTolUnit + "'," + param.MwSweight + "," + param.PstSweight + "," +
//        //            //             param.InsilicoSweight + "," + param.NumberOfOutputs + "," + param.DenovoAllow +
//        //            //             "," + param.PtmAllow + ")";
//        //            var query1 = "INSERT INTO query VALUES('" + param.Title + "','" + param.Blind_PTM + "','" +
//        //                         param.Button + "','" + param.Cyst_Chem_Moficiations + "','" + param.Database +
//        //                         "','"
//        //                         + param.Denovo_Allow + "','" + param.Fixed_Modifications + "','" + param.Fragmentation_Type + "','" +
//        //                         param.List_of_Modifications + "','" + param.Mass_Tolerance + "','" + param.Max_Tag_Length +
//        //                         "','" + param.Blind_PTM + "'," + param.Slider3 + ",'" + param.Slider2 + "'," + param.Slider1 + "," + param.Tuner_mass + "," +
//        //                         param.Uploaded_File + "," + param.Variable_Modifications + "," + param.Meth_Chem_Modifications + "," + param.Terminal_Modification + ","
//        //                         + param.Peptide_Tolerance + "," + param.Queryid + "," + param.Receive_Top + "," + param.Protein_Mass + "," + param.Min_Tag_Length +
//        //                         "," + param.Special_Ions + ")";


//        //            var cmd = new MySqlCommand(query1, _connection);
//        //            cmd.ExecuteNonQuery();

//        //            //foreach (var file in param.PeakListFile)
//        //            //{
//        //            //    query1 = "INSERT INTO data_file VALUES('" + param.Queryid + "','" + file + "','" + param.FileType + "')";
//        //            //    cmd = new MySqlCommand(query1, _connection);
//        //            //    cmd.ExecuteNonQuery();
//        //            //}

//        //            //foreach (var varPtm in param.PtmCodeVar)
//        //            //{
//        //            //    query1 = "INSERT INTO variable_modifications VALUES('" + param.Queryid + "','" + varPtm + "')";
//        //            //    cmd = new MySqlCommand(query1, _connection);
//        //            //    cmd.ExecuteNonQuery();
//        //            //}


//        //            //foreach (var fixPtm in param.PtmCodeFix)
//        //            //{
//        //            //    query1 = "INSERT INTO fixed_modifications VALUES('" + param.Queryid + "','" + fixPtm + "')";
//        //            //    cmd = new MySqlCommand(query1, _connection);
//        //            //    cmd.ExecuteNonQuery();
//        //            //}

//        //            //var query6 = "INSERT INTO server_status  (`qID`, `uID`, `progress`) VALUES('" + param.Queryid + "','" + param.UserId + "', 0" + ")";
//        //            //cmd = new MySqlCommand(query6, _connection);
//        //            //cmd.ExecuteNonQuery();
//        //        }
//        //        catch (Exception)
//        //        {
//        //            success = false;
//        //        }
//        //    }

//        //    CloseConnection();

//        //    if (success == false)
//        //        return -1;
//        //    return 1;
//        //}

//        //Stores Server Status
//        public List<Job> ServerStatus()
//        {
//            var result = new List<Job>();
//            var temp = new Job();
//            var query = "select * from server_status;";

//            if (!OpenConnection()) return result;
//            try
//            {
//                var cmd = new MySqlCommand(query, _connection);
//                var reader = cmd.ExecuteReader();
//                while (reader.Read())
//                {
//                    temp.Qid = reader.GetValue(0).ToString();
//                    temp.Uid = reader.GetValue(1).ToString();
//                    temp.Progress = reader.GetValue(2).ToString();
//                    result.Add(temp);
//                }
//                reader.Close();
//            }
//            catch (Exception)
//            {
//                // ignored
//            }
//            return result;
//        }

//        //Returns Progress
//        public int Get_Progress(string qid)
//        {
//            var query = "select Progress from server_status where qID='" + qid + "';";
//            var progress = 0;

//            if (!OpenConnection()) return progress;
//            try
//            {
//                var cmd = new MySqlCommand(query, _connection);
//                var reader = cmd.ExecuteReader();
//                if (reader.Read())
//                {
//                    progress = Convert.ToInt32(reader.GetValue(0));
//                    reader.Close();
//                }
//                else
//                    progress = -1;

//                _connection.Close();
//            }
//            catch (Exception e)
//            {
//                Debug.WriteLine(e.Message);
//                progress = -1;
//            }

//            return progress;
//        }

//        //Retrieve list of all searches by specific user
//        public List<Searchlist> retrieve_searches_db(string userid)
//        {
//            var results = new List<Searchlist>();
//            var query = "SELECT QueryId, SearchTitle, Date, PeakListFileAddress FROM proteomics.query where UserId='" +
//                        userid + "';";

//            if (!OpenConnection()) return results;
//            try
//            {
//                var cmd = new MySqlCommand(query, _connection);
//                using (var reader = cmd.ExecuteReader())
//                {
//                    while (reader.Read())
//                    {
//                        var temp = new Searchlist
//                        {
//                            Qid = reader["QueryId"].ToString(),
//                            Title = reader["SearchTitle"].ToString(),
//                            Date = reader["Date"].ToString(),
//                            File = reader["PeakListFileAddress"].ToString()
//                        };
//                        results.Add(temp);
//                    }
//                }
//                _connection.Close();
//            }
//            catch (Exception e)
//            {
//                Debug.WriteLine(e.Message);
//            }

//            return results;
//        }

//        //Sets the Progress of Querry
//        public int Set_Progress(string qid, int p)
//        {
//            var query = "UPDATE server_status SET Progress=" + p + " Where qID='" + qid + "';";
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

//        //Returns User Details
//        public UserDetails Get_user(string uid)
//        {
//            var uD = new UserDetails();
//            var query = "select * from user WHERE UserId='" + uid + "';";

//            if (!OpenConnection()) return uD;
//            try
//            {
//                var cmd = new MySqlCommand(query, _connection);
//                var reader = cmd.ExecuteReader();
//                if (reader.Read())
//                {
//                    uD.UName = reader.GetValue(0).ToString();
//                    uD.Name = reader.GetValue(1).ToString();
//                    uD.Email = reader.GetValue(2).ToString();
//                    uD.RPass = reader.GetValue(2).ToString();
//                }
//                else
//                {
//                    uD.Name = "not found";
//                    uD.Email = "not found";
//                    uD.RPass = "not found";
//                }
//                reader.Close();
//            }
//            catch (Exception e)
//            {
//                Debug.WriteLine(e.Message);
//            }
//            return uD;
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


//        //Returns Search History of specific query ie. search Parameters and Results
//        public Searchview retrieve_searchview_db(string qid)
//        {
//            var result = new Searchview();
//            var qp = new QuerryParameters();
//            var res = new Results();
//            var qidArr = qid.Split('z');
//            qid = qidArr[0];
//            var fileId = Convert.ToInt32(qidArr[1]);
//            var query1 = "SELECT * FROM proteomics.query where QueryId='" + qid + "';";
//            var queryT = "SELECT * FROM proteomics.timings where querry_ID='" + qid + "';";

//            if (OpenConnection())
//            {
//                try
//                {
//                    using (var trans = _connection.BeginTransaction())
//                    {
//                        var cmd = new MySqlCommand(queryT, _connection);
//                        var reader = cmd.ExecuteReader();
//                        if (reader.Read())
//                        {
//                            res.Times.InsilicoTime = reader.GetValue(1).ToString();
//                            res.Times.PtmTime = reader.GetValue(2).ToString();
//                            res.Times.TunerTime = reader.GetValue(3).ToString();
//                            res.Times.MwFilterTime = reader.GetValue(4).ToString();
//                            res.Times.PstTime = reader.GetValue(5).ToString();
//                            res.Times.TotalTime = reader.GetValue(6).ToString();
//                            res.Times.FileName = reader.GetValue(7).ToString();
//                            res.Times.TruncationEngineTime = reader.GetValue(8).ToString();
//                        }
//                        reader.Close();


//                        cmd = new MySqlCommand(query1, _connection);
//                        reader = cmd.ExecuteReader();
//                        if (reader.Read())
//                        {
//                            qp.QueryId = reader.GetValue(0).ToString();
//                            qp.UserId = reader.GetValue(1).ToString();
//                            qp.Title = reader.GetValue(2).ToString();
//                            qp.ProtDb = reader.GetValue(3).ToString();
//                            //qp.OutputFormat = Convert.ToInt32(reader.GetValue(4));
//                            qp.InsilicoFragType = reader.GetValue(5).ToString();
//                            qp.FilterDb = Convert.ToInt32(reader.GetValue(6));
//                            qp.PtmTolerance = Convert.ToDouble(reader.GetValue(7));
//                            qp.MwTolUnit = reader.GetValue(8).ToString();
//                            qp.MwTolerance = Convert.ToDouble(reader.GetValue(9));
//                            qp.HopThreshhold = Convert.ToDouble(reader.GetValue(10));
//                            qp.MinimumEstLength = Convert.ToInt32(reader.GetValue(11));
//                            qp.MaximumEstLength = Convert.ToInt32(reader.GetValue(12));
//                            qp.GuiMass = Convert.ToDouble(reader.GetValue(13));
//                            qp.HandleIons = reader.GetValue(14).ToString();
//                            qp.Autotune = Convert.ToInt32(reader.GetValue(15));
//                            qp.PeakListFile = reader.GetValue(16).ToString().Split('<');
//                            qp.FileType = reader.GetValue(17).ToString();
//                            qp.HopTolUnit = reader.GetValue(18).ToString();
//                            qp.MwSweight = Convert.ToDouble(reader.GetValue(19));
//                            qp.PstSweight = Convert.ToDouble(reader.GetValue(20));
//                            qp.InsilicoSweight = Convert.ToDouble(reader.GetValue(21));
//                            qp.NumberOfOutputs = Convert.ToInt32(reader.GetValue(22));
//                            qp.DenovoAllow = Convert.ToInt32(reader.GetValue(23));
//                            qp.PtmAllow = Convert.ToInt32(reader.GetValue(24));
//                            qp.NeutralLoss = Convert.ToDouble(reader.GetValue(25));   //NeutralLoss Added!!!
//                            qp.PSTTolerance = Convert.ToDouble(reader.GetValue(26));   //PSTTolerance Added!!!


//                            qp.PeptideTolerance = Convert.ToDouble(reader.GetValue(27));
//                            qp.PeptideToleranceUnit = reader.GetValue(28).ToString();
//                            qp.TerminalModification = reader.GetValue(29).ToString();
//                            qp.SliderValue = Convert.ToDouble(reader.GetValue(30));
//                            qp.CysteineChemicalModification = reader.GetValue(31).ToString();
//                            qp.MethionineChemicalModification = reader.GetValue(32).ToString();


//                        }

//                        reader.Close();
//                        query1 = "SELECT fixed_mod FROM fixed_modifications where QueryId='" + qid + "';";
//                        cmd = new MySqlCommand(query1, _connection);
//                        using (reader = cmd.ExecuteReader())
//                        {
//                            qp.PtmCodeFix = new List<int>();
//                            while (reader.Read())
//                            {
//                                qp.PtmCodeFix.Add(Convert.ToInt32(reader["fixed_mod"]));
//                            }
//                        }

//                        reader.Close();
//                        query1 = "SELECT variable_mod FROM variable_modifications where QueryId='" + qid + "';";
//                        cmd = new MySqlCommand(query1, _connection);
//                        using (reader = cmd.ExecuteReader())
//                        {
//                            qp.PtmCodeVar = new List<int>();
//                            while (reader.Read())
//                            {
//                                qp.PtmCodeVar.Add(Convert.ToInt32(reader["variable_mod"]));
//                            }
//                        }
//                        reader.Close();
//                        var query2 = "SELECT * FROM results where Querry_ID='" + qid + "' AND file_ID = " + fileId +
//                                     " ORDER BY Score DESC LIMIT " + qp.NumberOfOutputs + ";";
//                        cmd = new MySqlCommand(query2, _connection);
//                        res.QueryId = qid;
//                        res.FinalProt = new List<Proteins>();
//                        var resid = new List<string>();

//                        using (reader = cmd.ExecuteReader())
//                        {
//                            while (reader.Read())
//                            {
//                                var tempProt = new Proteins();
//                                resid.Add(reader.GetValue(0).ToString());
//                                tempProt.Header = reader.GetValue(2).ToString();
//                                tempProt.Sequence = reader.GetValue(3).ToString();
//                                tempProt.EstScore = Convert.ToDouble(reader.GetValue(4));
//                                tempProt.InsilicoScore = Convert.ToDouble(reader.GetValue(5));
//                                tempProt.PtmScore = Convert.ToDouble(reader.GetValue(6));
//                                tempProt.Score = Convert.ToDouble(reader.GetValue(7));
//                                tempProt.MwScore = Convert.ToDouble(reader.GetValue(8));
//                                tempProt.Mw = Convert.ToDouble(reader.GetValue(9));

//                                res.FinalProt.Add(tempProt);
//                            }
//                        }
//                        reader.Close();


//                        for (var i = 0; i < resid.Count; i++)
//                        {
//                            query2 = "SELECT * FROM ptm_sites where result_id='" + resid[i] + "';";
//                            cmd = new MySqlCommand(query2, _connection);
//                            using (reader = cmd.ExecuteReader())
//                            {
//                                while (reader.Read())
//                                {
//                                    var tempSite = new Sites
//                                    {
//                                        Index = Convert.ToInt32(reader.GetValue(1)),
//                                        Score = Convert.ToInt32(reader.GetValue(2)),
//                                        ModWeight = Convert.ToInt32(reader.GetValue(3)),
//                                        ModName = reader.GetValue(4).ToString(),
//                                        Site = Convert.ToChar(reader.GetValue(5))
//                                    };
//                                    var tempaa = reader.GetValue(6).ToString();

//                                    foreach (char aminoacid in tempaa)
//                                    {
//                                        tempSite.AminoAcid.Add(aminoacid);
//                                    }


//                                    res.FinalProt[i].PtmParticulars.Add(tempSite);
//                                }
//                            }
//                        }
//                        reader.Close();
//                        string xx;
//                        for (var i = 0; i < resid.Count; i++)
//                        {
//                            query2 = "SELECT matched_peak_left FROM insilico_matches_left where result_id='" + resid[i] +
//                                     "';";
//                            cmd = new MySqlCommand(query2, _connection);
//                            using (reader = cmd.ExecuteReader())
//                            {
//                                if (reader.Read())
//                                {
//                                    xx = reader.GetValue(0).ToString();

//                                    res.FinalProt[i].InsilicoDetails.PeaklistMassLeft =
//                                        xx.Split(',').Select(double.Parse).ToList();
//                                }
//                            }

//                            query2 = "SELECT Ions FROM leftions where ProteinID='" + res.FinalProt[i].Header + "';";
//                            cmd = new MySqlCommand(query2, _connection);
//                            using (reader = cmd.ExecuteReader())
//                            {
//                                if (!reader.Read()) continue;
//                                xx = reader.GetValue(0).ToString();
//                                xx = Decompress(xx);
//                                res.FinalProt[i].InsilicoDetails.InsilicoMassLeft =
//                                    xx.Split(',').Select(double.Parse).ToList();
//                            }
//                        }
//                        reader.Close();

//                        for (var i = 0; i < resid.Count; i++)
//                        {
//                            query2 = "SELECT matchedpeak_right FROM insilico_matches_right where result_id='" + resid[i] +
//                                     "';";
//                            cmd = new MySqlCommand(query2, _connection);
//                            using (reader = cmd.ExecuteReader())
//                            {
//                                if (reader.Read())
//                                {
//                                    xx = reader.GetValue(0).ToString();
//                                    res.FinalProt[i].InsilicoDetails.PeaklistMassRight =
//                                        xx.Split(',').Select(double.Parse).ToList();
//                                }
//                            }


//                            query2 = "SELECT Ions FROM rightions where ProteinID='" + res.FinalProt[i].Header + "';";
//                            cmd = new MySqlCommand(query2, _connection);
//                            using (reader = cmd.ExecuteReader())
//                            {
//                                if (reader.Read())
//                                {
//                                    xx = reader.GetValue(0).ToString();
//                                    xx = Decompress(xx);
//                                    res.FinalProt[i].InsilicoDetails.InsilicoMassRight =
//                                        xx.Split(',').Select(double.Parse).ToList();
//                                }
//                            }
//                        }
//                        reader.Close();
//                        result.Param = qp;
//                        result.Result = res;
//                        trans.Commit();
//                        _connection.Close();
//                    }
//                }


//                catch (Exception e)
//                {
//                    Debug.WriteLine(e.Message);
//                }

//                return result;
//            }
//            return result;
//        }
//    }
//}