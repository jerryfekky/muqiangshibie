using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCVAPP.Model
{
    public class BuildingWall
    {
        /// <summary>
        /// 
        /// </summary>
        public int adjustedImageFileId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int buildingId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string buildingWallNo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string buildingWallTypeCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        //public string createTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string description { get; set; }
        /// <summary>
        /// 
        /// </summary>
        //public int id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int imageFileId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int sysUserId { get; set; }
    }
    public class GetBuildingWall
    {
        public int id { get; set; }
        public string building_wall_no { get; set; }
        public string name { get; set; }
        public string building_wall_type_code { get; set; }
        public int image_file_id { get; set; }
        public int building_id { get; set; }
        public int adjusted_image_file_id { get; set; }
        public string description { get; set; }
        public string wall_together_code { get; set; }
    }
}
