using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using PerceptronLocalService;

namespace PerceptronLocalService.Utility
{
    public class AssignPtrToArray
    {
        public IntPtr ArrayPtr(List<double> InsilicoList)
        {
            var InsilicoArray = new double[] { };
            int size = 1;
            IntPtr InsilicoArrayPtr = Marshal.AllocHGlobal(size); //Test me
            Marshal.Copy(InsilicoArray, 0, InsilicoArrayPtr, InsilicoArray.Length);

            if (InsilicoList.Count != 0)
            {
                InsilicoArray = InsilicoList.ToArray();
                size = Marshal.SizeOf(InsilicoArray[0]) * InsilicoArray.Length;
                InsilicoArrayPtr = Marshal.AllocHGlobal(size);
                Marshal.Copy(InsilicoArray, 0, InsilicoArrayPtr, InsilicoArray.Length);
            }
            return InsilicoArrayPtr;
        }
    }
}
