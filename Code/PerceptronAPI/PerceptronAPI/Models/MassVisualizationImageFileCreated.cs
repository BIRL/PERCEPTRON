using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PerceptronAPI.Models
{
    public class MassVisualizationImageFileCreated
    {
        public string Name;
        public bool FileCreated;

        public MassVisualizationImageFileCreated(string cName, bool cFileCreated)
        {
            Name = cName;
            FileCreated = cFileCreated;

        }
    }

    
}