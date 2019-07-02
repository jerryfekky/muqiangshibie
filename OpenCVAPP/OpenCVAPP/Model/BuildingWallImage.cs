using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCVAPP.Model
{
    public class BuildingWallImage
    {
        //url:http://dev.strosoft.com/api/service?serviceName=AddBuildingWallImage&data={}
        //public int id { get; set; }
        public int buildingWallId { get; set; }//立面id
        public int originalImageFileId { get; set; }//原始图片ID
       // public string description { get; set; }//描述
        //public string name { get; set; }//标识名称
        //public int sysUserId { get; set; }//创建人id
        //public int selectedImageFileId { get; set; }//选取区域图片ID
        public int adjustedImageFileId { get; set; }//矫正后图片ID
        public int resultImageFileId { get; set; }//检测结果图片ID
        public int areaTopLeftX { get; set; }//左上角横坐标
        public int areaTopLeftY { get; set; }//左上角纵坐标
        public int areaTopRightX { get; set; }//右上角横坐标
        public int areaTopRightY { get; set; }//右上角纵坐标
        public int areaBottomLeftX { get; set; }//左下角横坐标
        public int areaBottomLeftY { get; set; }//左下角纵坐标
        public int areaBottomRightX { get; set; }//右下角横坐标
        public int areaBottomRightY { get; set; }//右下角纵坐标
        public int positionX { get; set; }//水平位置
        public int positionY { get; set; }//垂直位置
        public int width { get; set; }
        public int height { get; set; }
        public BuildingWallImage() { }
        public BuildingWallImage(int buildingWallId, int originalImageFileId, string description, string name, int sysUserId, int selectedImageFileId,
          int adjustedImageFileId, int resultImageFileId, int areaTopLeftX, int areaTopLeftY, int areaTopRightX, int areaTopRightY, int areaBottomLeftX, int areaBottomLeftY,
          int areaBottomRightX, int areaBottomRightY, int positionX, int positionY,int width,int height)
        {
            this.adjustedImageFileId = adjustedImageFileId;
            this.areaBottomLeftX = areaBottomLeftX;
            this.areaBottomLeftY = areaBottomLeftY;
            this.areaBottomRightX = areaBottomRightX;
            this.areaBottomRightY = areaBottomRightY;
            this.areaTopLeftX = areaTopLeftX;
            this.areaTopLeftY = areaTopLeftY;
            this.areaTopRightX = areaTopRightX;
            this.areaTopRightY = areaTopRightY;
            this.buildingWallId = buildingWallId;
            //this.description = description;
            //this.name = name;
            this.originalImageFileId = originalImageFileId;
            this.positionX = positionX;
            this.positionY = positionY;
            this.width = width;
            this.height = height;
            //this.resultImageFileId = resultImageFileId;
            //this.selectedImageFileId = selectedImageFileId;
            //this.sysUserId = sysUserId;
        }
    }
}
