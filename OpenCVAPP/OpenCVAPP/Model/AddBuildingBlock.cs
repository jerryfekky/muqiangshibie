using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCVAPP.Model
{
    public class AddBuildingBlock
    {
        public string resultImageFile { get; set; }
        public int buildingWallId { get; set; }
        public int buildingWallImageId { get; set; }
        public List<BuildingBlockList> blocks { get; set; }
    }
    public class BuildingBlockList
    {
        public int width { get; set; }
        public int height { get; set; }
        public int blockLocationTopLeftX { get; set; }
        public int blockLocationTopLeftY { get; set; }
        public int adjustedLocationTopLeftX { get; set; } = 0;
        public int adjustedLocationTopLeftY { get; set; } = 0;
        public string buildingWallBlockNo { get; set; } = DateTime.Now.ToString("yyyyMMddhhmmssff");
        public string value { get; set; }
        public string name { get; set; }
    }
}
