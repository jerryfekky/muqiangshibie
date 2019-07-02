using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCVAPP.Model
{
    public class ResultMessage
    {
        public string success { get; set; }
        public int data { get; set; }
        public string messageType { get; set; }
        public string errorMessage { get; set; }
        public ResultMessage() { }
    }
    public class Buildingwallresult
    {
        public string success { get; set; }
        public BuildingWallImage data { get; set; }
        public string adminRootUrl { get; set; }
    }
}
