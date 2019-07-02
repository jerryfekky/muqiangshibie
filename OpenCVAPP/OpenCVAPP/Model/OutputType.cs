using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCVAPP.Model
{
   public class OutputType
    {
       public string imgType { get; set; }
       public string imgFile { get; set; }
        public OutputType(string imgType,string imgFile)
        {
            this.imgFile = imgFile;
            this.imgType = imgType;
        }
        public OutputType()
        {

        }
    }
}
