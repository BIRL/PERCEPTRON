using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace PerceptronFastaReader.Repository
{
    public class MySqlDatabase
    {
        private MySqlConnection _connection;

        //Constructor
        public MySqlDatabase()
        {
            Initialize();
        }

        //Initialize values
        private void Initialize()
        {
            const string connectionString = "Data Source=******;" +  // Add here Server Name
                                            "Initial Catalog=Human;" +  //Add here Database Name
                                            "User id= root;" +
                                            "Password=******;";  //Add here Server Name Password
            _connection = new MySqlConnection(connectionString);
        }

        //open _connection to database
        private bool OpenConnection()
        {
            try
            {
                _connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                //When handling errors, you can your application's response based 
                //on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                switch (ex.Number)
                {
                    case 0:
                        Console.WriteLine("Cannot connect to server.  Contact administrator");
                        break;

                    case 1045:
                        Console.WriteLine("Invalid username/password, please try again");
                        break;
                    default:
                        Console.WriteLine("Error!");
                        break;
                }
                return false;
            }
        }


        private void CloseConnection()
        {
            try
            {
                _connection.Close();
            }
            catch (MySqlException)
            {
            }
        }
    }
}
