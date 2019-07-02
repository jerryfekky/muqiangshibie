using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCVAPP.Model
{
    public class GetBlockValue
    {
        public int id { get; set; }
        public int buildingWallBlockId { get; set; }
        public int buildingWallBlockParameterId { get; set; }
        public string value { get; set; }
    }
    public class BlockValueSQL
    {
        public int id { get; set; }
        public int building_wall_block_id { get; set; }
        public int building_wall_block_parameter_id { get; set; }
        public string value { get; set; }
    }
}
