using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCVAPP.Model
{
    public class ReturnImgInfo
    {
        public string BuildingImg { get; set; }
        public string BuildingadjustImg { get; set; }
        public List<BuildingWallBlock> ImgInfo { get; set; }
        public List<GetBlockValue> blockvalue { get; set; }
    }
}
