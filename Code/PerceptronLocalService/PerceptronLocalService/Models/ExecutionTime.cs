//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PerceptronLocalService.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ExecutionTime
    {
        public int FileId { get; set; }
        public System.DateTime JobSubmission { get; set; }
        public string QueryId { get; set; }
        public string InsilicoTime { get; set; }
        public string PtmTime { get; set; }
        public string TunerTime { get; set; }
        public string MwFilterTime { get; set; }
        public string PstTime { get; set; }
        public string TotalTime { get; set; }
        public string FileName { get; set; }
        public string TruncationEngineTime { get; set; }
    }
}
