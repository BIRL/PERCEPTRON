using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Mono.CSharp;
using PerceptronLocalService.DTO;
using PerceptronLocalService.Interfaces;

namespace PerceptronLocalService.Engine
{
    class ProteinRepositorySql : IProteinRepository
    {
        public List<ProteinDto> ExtractProteins(double mw, SearchParametersDto parameters)
        {            
            var query = GetQuery(mw, parameters);

            var connectionString = GetConnectionString(parameters.ProtDb);
            List<SerializedProteinDataDto> prot;
            using (var connection = new SqlConnection(connectionString))
            {
                prot = connection.Query<SerializedProteinDataDto>(query).ToList();

            }

            var proteins = new List<ProteinDto>();

            foreach (var proteinInfo in prot)
            {
                var insilico = new InsilicoObjectDto()
                {
                    InsilicoMassLeft = proteinInfo.Insilico.Split(',').Select(double.Parse).ToList(),
                    InsilicoMassRight = proteinInfo.InsilicoR.Split(',').Select(double.Parse).ToList() // InsilicoR
                };

                insilico.InsilicoMassLeft.RemoveAt(insilico.InsilicoMassLeft.Count - 1); // JUST IN CASE::: as this will be the MW of protein - water
                insilico.InsilicoMassRight.RemoveAt(insilico.InsilicoMassRight.Count - 1);

                var protein = new ProteinDto()
                {
                    Header = proteinInfo.ID,
                    InsilicoDetails = insilico,
                    Mw = proteinInfo.MW,
                    Sequence = proteinInfo.Seq

                };
                
                proteins.Add(protein);

            }
            return proteins;
        }

        private string GetConnectionString(string db)
        {
            switch (db)
            {
                case  "Ecoli":
                    return  ConfigurationManager.ConnectionStrings["EcoliConnectionStringName"].ConnectionString;
                    //break;
                case "Bacteria":
                    return ConfigurationManager.ConnectionStrings["EcoliConnectionStringName"].ConnectionString;
                    //break;
                case "TrEMBL":// FARHAN's TESTING
                    return ConfigurationManager.ConnectionStrings["TrEMBLConnectionStringName"].ConnectionString;
                    //break;
                case "Human":
                    return ConfigurationManager.ConnectionStrings["ProteinConnectionStringName"].ConnectionString;
                    //break;
                default:  // Default is also Human
                    return ConfigurationManager.ConnectionStrings["ProteinConnectionStringName"].ConnectionString;

            }
        
        }

        private string GetQuery(double mw, SearchParametersDto parameters)
        {
            string DatabaseName = parameters.ProtDb;

            if (parameters.ProtDb == "Bacteria")
            {
                parameters.ProtDb = "Ecoli";
                DatabaseName = parameters.ProtDb;
            }

            string subquery1 = "Select * From  [";
            string subquery2 = "].[dbo].[ProteinInfoes]";


            string query = subquery1 + DatabaseName + subquery2;   //"Select * From  [ProteinDB].[dbo].[ProteinInfoes]";
            if (parameters.FilterDb == 1)
            {
                query += " Where MW > " + (mw - parameters.MwTolerance) + " AND MW < " + (mw + parameters.MwTolerance);
            }
            return query;
        }
    }
}
