//using PerceptronLocalService.Testing;
using Topshelf;

namespace PerceptronLocalService
{
    public class Program
    {

        public static void Main(string[] args)
        {
            //PstGenerationBenchmarking.Benchnmark();
           // PstScoringBenchmarking.Benchnmark();

            HostFactory.Run(host =>
            {
                host.SetServiceName("PerceptronLocalService"); //cannot contain spaces or / or \
                host.SetDisplayName("Perceptron");
                host.SetDescription("Perceptron local service to run toolbox");
                host.StartAutomatically();

                //host.RunAs("service account name", "the password");
                //Don't think you like to expose your password in the code. =P
                //We can set it manually for one time after installing the windows service in the services.msc

                host.Service<PerceptronService>();
            });

        }
        
    }
}
