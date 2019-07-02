using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;

namespace OpenCVAPP.Model
{
    public class Positions
    {
        public  string FileName { get; set; }//文件路径
        public  string Message { get; set; }//其他信息

        public List<Point[]> point2 { get; set; }//坐标点

        public List<Point> Center { get; set; }//中心位置
        public List<string> Texture { get; set; }//类型 
        public int imgFileId { get; set; }
        public int adjustedImageFileId { get; set; }
        public int buildingWallImageId { get; set; }
        public int resultImageFileId { get; set; }
    }
    public class TexturePoint
    {
        public Point[][] point2 { get; set; }//坐标点

        public Point Center { get; set; }//中心位置
        public string Texture { get; set; }//类型 
    }
}
