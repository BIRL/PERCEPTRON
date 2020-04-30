using System;
//using PerceptronLocalService.Testing;
using Topshelf;

namespace PerceptronLocalService
{
    public class PerceptronService : ServiceControl
    {
        private static Perceptron _instance;

        public bool Start(HostControl hostControl)
        {
            try
            {
                //Execute my existing console application code
                _instance = new Perceptron();
                _instance.Start();

                return true;
            }
            catch(Exception e)
            {
                var temp = e.Message;
                //Logging.DumpError(e.Message);
                return false;
            }
        }

        public bool Stop(HostControl hostControl)
        {
            try
            {
                _instance.Stop();
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
