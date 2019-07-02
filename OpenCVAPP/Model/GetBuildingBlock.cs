using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCVAPP.Model
{
    public class GetBuildingBlock
    {
        /// <summary>
        /// 
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string building_wall_block_no { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int building_wall_image_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int width { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int height { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int block_location_top_left_x { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int block_location_top_left_y { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int adjusted_location_top_left_x { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int adjusted_location_top_left_y { get; set; }

        public int building_wall_id { get; set; }
    }


}
