using System.Collections.Generic;

namespace PerceptronAPI.Models
{
    public class FastaWriterProteinDataDto
    {
        public string ProteinDescription;
        public string Sequence;

        public FastaWriterProteinDataDto()
        {
            ProteinDescription = "";
            Sequence = "";
        }
    }
}