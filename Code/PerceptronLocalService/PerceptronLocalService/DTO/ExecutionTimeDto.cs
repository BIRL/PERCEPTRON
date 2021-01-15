using System;

namespace PerceptronLocalService.DTO
{
    public class ExecutionTimeDto
    {
        public string FileReadingTime;
        public string TunerTime;
        public string PstTime;


        public string InsilicoTime;
        public string PtmTime;
        
        public string MwFilterTime;
        
        public string TotalTime;
        
        public string TruncationEngineTime;
        public DateTime JobSubmission;
    }
}
