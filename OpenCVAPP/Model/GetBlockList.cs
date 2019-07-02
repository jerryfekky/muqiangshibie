using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCVAPP.Model
{
    public class GetBlockList
    {
        public string WallImg { get; set; }
        public List<GetBlockValue> Blockvalue { get; set; }
        public List<BuildingWallBlock> block { get; set; }

    }
    public class BlockTemp
    {
        public string WallImg { get; set; }
        public GetBlockValue Blockvalue { get; set; }
        public BuildingWallBlock block { get; set; }
    }
}
