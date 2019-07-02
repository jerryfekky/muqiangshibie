using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OpenCVAPP.Model;
using OpenCvSharp;

namespace OpenCVAPP.OpenCV
{
    public class Img_Rectity
    {
        private static List<string> Texture = new List<string>();
        private static List<Point> Centerpoint = new List<Point>();
        private static Mat newsrc = new Mat();
        /// <summary>
        /// 图像截取、矫正
        /// </summary>
        public static string Pic_Rectity(string FileName, string point_json,int originalImageFileId,int buildingWallId)
        {
            BuildingWallImage building = new BuildingWallImage();
            string path = ConfigurationSettings.AppSettings["Savepath"];
            string savepath = path + DateTime.Now.ToString("yyyyMMddhhmmss") + ".jpg";
            Base64Convert.Base64ToFileAndSave(FileName,savepath);//文件流保存到文件
            Positions positions = new Positions();
            string filepath = path;
            Mat src = Cv2.ImRead(savepath); //原始图像
            List<Point> getpt = JsonConvert.DeserializeObject<List<Point>>(point_json);
            Mat dst = new Mat();
            //new Window("1", WindowMode.FreeRatio, src);
            Mat roi = Mat.Zeros(src.Size(), MatType.CV_8U);
            Point[] points = getpt.ToArray();
            int maxX = points[0].X, maxY = points[0].Y, minX = points[0].X, minY = points[0].Y;
            Point[][] contours = { points };
            for (int i = 0; i < points.Length; i++)//求出最小坐标
            {
                if (points[i].X > maxX)
                    maxX = points[i].X;
                if (points[i].Y > maxY)
                    maxY = points[i].Y;
                if (points[i].X < minX)
                    minX = points[i].X;
                if (points[i].Y < minY)
                    minY = points[i].Y;
            }
            int width = maxX - minX;
            int height = maxY - minY;
            Cv2.DrawContours(roi, contours, 0, Scalar.All(255), -1);
            src.CopyTo(dst, roi);//保存到新画布
            //new Window("转化前", WindowMode.FreeRatio, dst);

            dst = dst.SubMat(new Rect(minX, minY, width, height));

            Point2f[] pt = new Point2f[4];
            for (int i = 0; i < points.Length; i++)//图像坐标
            {
                pt[i].X = points[i].X - minX;
                pt[i].Y = points[i].Y - minY;
            }
            Point2f[] box = new Point2f[4];//画布角点
            box[0] = new Point(0, 0);
            box[1] = new Point(width, 0);
            box[2] = new Point(width, height);
            box[3] = new Point(0, height); 
            //图像的拉伸处理
            var h = Cv2.GetPerspectiveTransform(pt, box);
            Cv2.WarpPerspective(dst, dst, h, new Size(width, height));
            string time= DateTime.Now.ToString("yyyyMMddhhmmssff") + ".jpg";
            filepath += time;
            Cv2.ImWrite(filepath, dst);
            positions.FileName = ConfigurationSettings.AppSettings["Returnpath"] + time;
            positions.point2 = Findarea(dst);
            positions.imgFileId = UploadFile.Upload(savepath);
            positions.adjustedImageFileId = UploadFile.Upload(filepath);
            building.adjustedImageFileId = positions.adjustedImageFileId;
            //building.selectedImageFileId = positions.adjustedImageFileId;
            building.areaTopLeftX = points[0].X;
            building.areaTopLeftY = points[0].Y;
            building.areaTopRightX = points[1].X;
            building.areaTopRightY = points[1].Y;
            building.areaBottomLeftX = points[2].X;
            building.areaBottomLeftY = points[2].Y;
            building.areaBottomRightX = points[3].X;
            building.areaBottomRightY = points[3].Y;
            building.buildingWallId = buildingWallId;
            building.originalImageFileId = originalImageFileId;
            building.resultImageFileId = 1870;
            string result=HTTPService.AddWallImage(building);
            ResultMessage s = JsonConvert.DeserializeObject<ResultMessage>(result);
            positions.buildingWallImageId = s.data;
            string js = JsonConvert.SerializeObject(positions);
            Console.WriteLine(js);
            File.Delete(savepath);
            return js;
        }
                /// <summary>
        /// 图像轮廓识别
        /// </summary>
        /// <param name="src"></param>
        public static List<Point[]> Findarea(Mat img)
        {
            //Mat img = src;
            Mat src = new Mat();
            img.CopyTo(src);
            Point[][] contours=new Point[default][];
            Point2f[] point2Fs = new Point2f[] { };
            List<Point2f[]> point2 = new List<Point2f[]>();
            Point p0 = new Point(0, 0), p1 = new Point(0, 0), p2 = new Point(0, 0), p3 = new Point(0, 0);
            contours= ContoursMethod.ContoursBoost(src);//06.28
            int resultnum = 0;
            List<Point[]> Excontours = new List<Point[]>();
            //Point[][] Excont =contours;
            for (int i = 0; i < contours.Length; i++)
            {
                RotatedRect rect = Cv2.MinAreaRect(contours[i]);
                double area = Cv2.ContourArea(rect.Points(), false);
                if (area < 800)
                    continue;
                point2Fs = rect.Points();
                Point[] po = change(rect.Points());

                //point2.Add(point2Fs);
                Excontours.Add(po);
                
                for (int z = 0; z < point2Fs.Length; z++)//小数位精度——2
                {
                    point2Fs[z].X = (float)Math.Round(point2Fs[z].X,2);
                    point2Fs[z].Y = (float)Math.Round(point2Fs[z].Y, 2);
                }
                point2.Add(point2Fs);
                for (int j = 0; j < 3; j++)
                {
                    p0 = new Point(point2Fs[j].X, point2Fs[j].Y);
                    p1 = new Point(point2Fs[j + 1].X, point2Fs[j + 1].Y);
                    Cv2.Line(src, p0, p1, Scalar.Red, 2, LineTypes.Link8);
                }
                p2 = new Point(point2Fs[3].X, point2Fs[3].Y);
                p3 = new Point(point2Fs[0].X, point2Fs[0].Y);
                Point TP = new Point((((p0.X + p1.X) / 2)), ((p1.Y + p2.Y) / 2));
                Cv2.Line(src, p2, p3, Scalar.Red, 2, LineTypes.Link8);
                resultnum++;
            }
            Console.WriteLine("剔除后的轮廓数：" + resultnum);
            //new Window("result", WindowMode.FreeRatio, src);
            //Window.WaitKey(0);
            return Excontours;
        }
        /// <summary>
        /// Point2f转Point
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static Point[] change(Point2f[] point)
        {
            Point[] points = new Point[point.Length];
            for (int i = 0; i < point.Length; i++)
            {
                points[i].X = (int)Math.Round(point[i].X, 0);
                points[i].Y = (int)Math.Round(point[i].Y, 0);
            }
            return points;
        }
        /// <summary>
        /// 材质识别
        /// </summary>
        public static List<string> TextureFind(Mat img,Point[] points)
        {
            Mat src = img;
            Mat hsv = new Mat(), threshold = new Mat();
            Mat dst = new Mat(), dst2 = new Mat();
            Scalar HsvWhiteLow = new Scalar(0, 0, 1);//白 white
            Scalar HsvWhiteHigh = new Scalar(0.1156, 0.2480, 0.9804);
            Scalar HsvGraLow = new Scalar(0, 0, 46);//灰 gray
            Scalar HsvGraHigh = new Scalar(180, 43, 220);
            Scalar HsvGrayLow = new Scalar(0.4815,0.0720,0.4902);//深灰
            Scalar HsvGrayHigh = new Scalar(0.4583,0.0656,0.2392);
            Scalar HsvGlassLow = new Scalar(171,1,80);//Glass gray
            Scalar HsvGlassHigh = new Scalar(171,5,60);
            Scalar HsvBlueLow = new Scalar(100, 43, 46);//蓝 blue
            Scalar HsvBlueHigh = new Scalar(124, 255, 255);
            Cv2.CvtColor(src, hsv, ColorConversionCodes.BGR2HSV);
            Scalar[] HsvLow = { HsvWhiteLow, HsvGlassLow, HsvGraLow, HsvBlueLow,HsvGrayLow };
            Scalar[] HsvHigh = { HsvWhiteHigh, HsvGlassHigh, HsvGraHigh, HsvBlueHigh,HsvGrayHigh};
            string[] textcolor = { "White", "Glass", "Blue","Metal ash","Gray"};
            Point[][] contours= { points};
            int cc = 0;
            Point center=new Point();
            HierarchyIndex[] hierarchy;
            
            //List<string> Texture = new List<string>();
            //List<Point> Centerpoint = new List<Point>();
            for (int color = 0; color < HsvLow.Length; color++)
            {
                
                Cv2.InRange(hsv, HsvLow[color], HsvHigh[color], threshold);
                Cv2.Threshold(threshold, threshold, 1, 255, ThresholdTypes.Binary);
                Cv2.CopyMakeBorder(threshold, dst, 1, 1, 1, 1, BorderTypes.Constant, 0);
                //Cv2.FindContours(dst, out contours, out hierarchy, RetrievalModes.Tree, ContourApproximationModes.ApproxSimple);
                int count = 0;
                if(count<1)
                {
                    Rect bound = Cv2.BoundingRect(points);
                    
                    center = new Point(bound.X + bound.Width / 2, bound.Y + bound.Height / 2);
                    int[] vs = { center.X,center.Y};
                    char x = threshold.At<char>(vs);
                    if (x>0)
                    {
                        //Cv2.DrawContours(src, contours, 0, Scalar.Red, 1, LineTypes.Link8);
                        //Cv2.PutText(src, textcolor[color], center, HersheyFonts.HersheyComplex,1, Scalar.Black, 1, LineTypes.Link8);
                        //Cv2.Circle(src, center, 10, Scalar.Gold);
                        cc = color;
                       
                    }
                }
            }
            Centerpoint.Add(center);
            switch (textcolor[cc])
            {
                case "Gray":
                case "White": Texture.Add("石头"); break;
                case "Glass":
                case "Blue": Texture.Add("玻璃"); break;
                case "Metal ash": Texture.Add("金属"); break;
                default: Texture.Add("其他"); break;
            }
            //Console.WriteLine(textcolor[cc] + "区域数量:" + count);
            //Cv2.NamedWindow("result_txe", 0);
            //Cv2.ResizeWindow("result_txe", 500, 500);
            //Cv2.ImShow("result_txe", src);
            //
            //Window.WaitKey();
            return Texture;
        }
        public static string GetCase2Texture(string FileName,string points)
        {
            Positions positions = new Positions();
            
            string filepath = ConfigurationSettings.AppSettings["Savepath"];
            string urlpath = ConfigurationSettings.AppSettings["Returnpath"];
            string NewFile= FileName.Replace(urlpath, filepath);
            Mat src = Cv2.ImRead(NewFile); 
            List<Point[]> getpt = JsonConvert.DeserializeObject<List<Point[]>>(points);
            positions.point2 = getpt;
            positions.Texture = null;
            Texture.Clear();
            Console.WriteLine(positions.point2.Count());
            //List<string> Texture = new List<string>();
            for (int i = 0; i < positions.point2.Count(); i++)
            {
                positions.Texture = TextureFind(src, positions.point2[i]);
            }
            string save = filepath+DateTime.Now.ToString("yyyyMMddhhmmssff") + ".jpg";
            Cv2.ImWrite(save,src);
            //new Window("result", src);
            //Window.WaitKey();
            positions.resultImageFileId = UploadFile.Upload(save);
            positions.point2 = null;
            positions.Center = null;
            string js = JsonConvert.SerializeObject(positions);
            Console.WriteLine(js);
            
            return js;
        }

    }
}
