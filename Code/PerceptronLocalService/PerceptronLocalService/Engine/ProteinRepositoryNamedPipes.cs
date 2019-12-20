using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using PerceptronLocalService.DTO;
using PerceptronLocalService.Interfaces;

namespace PerceptronLocalService.Engine
{
    class ProteinRepositoryNamedPipes : IProteinRepository
    {
        public List<ProteinDto> ExtractProteins(double mw, SearchParametersDto parameters)
        {
            var fPam = new MolecularWeightServiceParametersDto()
            {
                mw = mw,
                Parameters = parameters
            };

            SendParamters(fPam);

            var proteins = GetProteins();

            return proteins;
        }


        static void SendParamters(MolecularWeightServiceParametersDto param)
        {


            using (NamedPipeClientStream pipeClient =
          new NamedPipeClientStream(".", "ParameterPipe", PipeDirection.Out))
            using (StreamWriter sw = new StreamWriter(pipeClient))
            {
                pipeClient.Connect();

                string serializedData = string.Empty;                   // The string variable that will hold the serialized data

                XmlSerializer serializer = new XmlSerializer(param.GetType());
                using (StringWriter strw = new StringWriter())
                {
                    serializer.Serialize(strw, param);
                    serializedData = strw.ToString();
                }




                // Read user input and send that to the client process.

                // {
                sw.AutoFlush = true;
                // Console.Write("Enter text: ");
                sw.WriteLine(serializedData);


            }


        }

        static MolecularWeightServiceParametersDto GenerateDummyParamsForTesting()
        {
            var tol = 1000;//parameters.MwTolerance;
            var database = "Human";// parameters.ProtDb;
            var filterDb = 0;//parameters.FilterDb;
            var mw = 6555;

            var parameters = new SearchParametersDto
            {
                MwTolerance = tol,
                ProtDb = database,
                FilterDb = filterDb
            };

            MolecularWeightServiceParametersDto Fpam = new MolecularWeightServiceParametersDto
            {
                Parameters = parameters,
                mw = mw
            };

            return Fpam;
        }

        static List<ProteinDto> GetProteins()
        {
            List<ProteinDto> result = new List<ProteinDto>();

            using (NamedPipeClientStream pipeClient =
         new NamedPipeClientStream(".", "ProteinPipe", PipeDirection.In))
            using (StreamReader sr = new StreamReader(pipeClient))
            {
                pipeClient.Connect();

                string temp;
                string serializedData = string.Empty;
                while ((temp = sr.ReadLine()) != null)
                {
                    serializedData += temp;
                }

                XmlSerializer deserializer = new XmlSerializer(typeof(List<ProteinDto>));

                using (TextReader tr = new StringReader(serializedData))
                {
                    result = (List<ProteinDto>)deserializer.Deserialize(tr);
                }


            }

            return result;
        }

    }
}
