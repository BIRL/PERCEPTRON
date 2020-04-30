using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PerceptronAPI.Models
{
    public class Results
    {
        public string QueryId;
        public List<Proteins> FinalProt;
        public ExecutionTime Times;

        public Results(string qId, List<Proteins> prt, ExecutionTime t)
        {
            QueryId = qId;
            FinalProt = prt;
            Times = t;
        }
        public Results()
        {
            FinalProt = new List<Proteins>();
            Times = new ExecutionTime();
        }
    }
}