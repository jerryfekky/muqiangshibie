using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCVAPP.Model
{
    public class AssembleInfo
    {
        public int id { get; set; }
        public int positionX { get; set; }
        public int positionY { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public string adjustImg { get; set; }
        public string resultImg { get; set; }
        public List<BuildingWallBlock> ImgInfo { get; set; }
        public List<GetBlockValue> blockvalue { get; set; }

    }
    public class Assembles
    {
        public string wall_together_code { get; set; }
        
        public List<AssembleInfo> AssembleInfos { get; set; }
       
    }
}
