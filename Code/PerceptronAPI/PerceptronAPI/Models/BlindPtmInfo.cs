using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace PerceptronAPI.Models
{
    public class BlindPtmInfo
    {
        public int BlindPtmLocalizationStart;
        public double BlindPtmLocalizationMass;
        public int BlindPtmLocalizationEnd;

        public BlindPtmInfo(string BlindPtmInfo)
        {
            var BlindPtmLocalizationInfo = BlindPtmInfo.Split(',').ToList<string>();
            var BlindPtmLocalizationStart = Convert.ToInt16(BlindPtmLocalizationInfo[0]);
            var BlindPtmLocalizationMass = Convert.ToDouble(BlindPtmLocalizationInfo[1]);
            var BlindPtmLocalizationEnd = Convert.ToInt16(BlindPtmLocalizationInfo[2]);
        }
    }
}