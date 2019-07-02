using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCVAPP.Model
{
    public class BuildingWallBlock
    {

        //url:http://dev.strosoft.com:6090/api/service?serviceName=AddBuildingWallBlock&data={}
        public int id { get; set; }
        public int buildingWallId { get; set; }//立面ID
        public int buildingWallImageId { get; set; }//立面图片ID
        public int width { get; set; }//宽度
        public int height { get; set; }//高度
        public int blockLocationTopLeftX { get; set; }//板块位置左上角横坐标
        public int blockLocationTopLeftY { get; set; }//板块位置左上角纵坐标
        public int adjustedLocationTopLeftX { get; set; } //拼图左上角横坐标
        public int adjustedLocationTopLeftY { get; set; } //拼图左上角纵坐标
        public string buildingWallBlockNo { get; set; }//编码

        public BuildingWallBlock(int buildingWallId, int buildingWallImageId, int width, int height, int blockLocationTopLeftX, int blockLocationTopLeftY, int adjustedLocationTopLeftX, int adjustedLocationTopLeftY,string buildingWallBlockNo)
        {
            this.buildingWallId = buildingWallId;
            this.buildingWallImageId = buildingWallImageId;
            this.width = width;
            this.height = height;
            this.blockLocationTopLeftX = blockLocationTopLeftX;
            this.blockLocationTopLeftY = blockLocationTopLeftY;
            this.adjustedLocationTopLeftX = adjustedLocationTopLeftX;
            this.adjustedLocationTopLeftY = adjustedLocationTopLeftY;
            this.buildingWallBlockNo = buildingWallBlockNo;
        }
        public BuildingWallBlock()
        {

        }
    }
}
