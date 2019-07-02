using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCVAPP.Model
{
    public class UploadResult
    {
        public string succsess { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string path { get; set; }
        public UploadResult() { }
        public UploadResult(string succsess,int id,string name,string path)
        {
            this.succsess = succsess;
            this.id = id;
            this.name = name;
            this.path = path;
        }

    }
}
