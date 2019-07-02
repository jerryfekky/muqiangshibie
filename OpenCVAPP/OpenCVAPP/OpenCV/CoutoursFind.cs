using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OpenCvSharp;

namespace OpenCVAPP.OpenCV
{
    public class CoutoursFind
    {
        public static void Pic_Rectity(string FileName, string point_json)
        {
            Mat src = Cv2.ImRead(FileName); //原始图像
            List<Point> getpt = JsonConvert.DeserializeObject<List<Point>>(point_json);
            Mat dst = new Mat();
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
            Console.WriteLine(dst.Width+","+dst.Height);
            
            //Cv2.ImWrite(@"C:\toolkipweb\miniProgram\opencvtest\opencv\sss.bmp",dst);
            string a = FindContours(dst);
            Window.WaitKey();

        }
        public static string FindContours(Mat img)
        {
            Mat src = img;
            Mat gray = new Mat(), dst = new Mat(), hsvImage = new Mat();
            Point[][] contours;
            HierarchyIndex[] hierarchys;
            Point2f[] point2Fs = new Point2f[] { };
            List<Point2f[]> point2 = new List<Point2f[]>();
            Point p0 = new Point(0, 0), p1 = new Point(0, 0), p2 = new Point(0, 0), p3 = new Point(0, 0);
            //ImageMethod.HistogramEqualization(src, src);
            Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY, 0);

            Mat x = Cv2.GetStructuringElement(MorphShapes.Ellipse, new Size(6, 6));//!!!调整size
            Cv2.MorphologyEx(gray, gray, MorphTypes.Open, x);//!!!调整MorphTypes
            Cv2.Erode(gray, gray, x);
            //Cv2.Dilate(gray,gray,x);
            int thresh_size = (100 / 4) * 2 + 1;//自适应阈值化
            Cv2.AdaptiveThreshold(gray, gray, 255, 0, ThresholdTypes.Binary, thresh_size, thresh_size / 3);
            new Window("gray", WindowMode.FreeRatio, gray);

            Cv2.FindContours(gray, out contours, out hierarchys, RetrievalModes.External, ContourApproximationModes.ApproxSimple, null);//!!调整两个modes
            int resultnum = 0;
            Point[][] Excontours = contours;
            for (int i = 0; i < hierarchys.Length; i++)
            {
                double area = Cv2.ContourArea(contours[i], false);
                if (area < 50)
                    continue;
                RotatedRect rect = Cv2.MinAreaRect(contours[i]);
                point2Fs = rect.Points();
                Point[] po = change(rect.Points());
                //point2.Add(point2Fs);
                Excontours[resultnum] = po;
                for (int z = 0; z < point2Fs.Length; z++)//小数位精度——2
                {
                    point2Fs[z].X = (float)Math.Round(point2Fs[z].X, 2);
                    point2Fs[z].Y = (float)Math.Round(point2Fs[z].Y, 2);
                }
                point2.Add(point2Fs);
                for (int j = 0; j < 3; j++)
                {
                    p0 = new Point(point2Fs[j].X, point2Fs[j].Y);
                    p1 = new Point(point2Fs[j + 1].X, point2Fs[j + 1].Y);
                    Cv2.Line(src, p0, p1, Scalar.Red, 3, LineTypes.Link8);
                }
                p2 = new Point(point2Fs[3].X, point2Fs[3].Y);
                p3 = new Point(point2Fs[0].X, point2Fs[0].Y);
                Point TP = new Point((((p0.X + p1.X) / 2)), ((p1.Y + p2.Y) / 2));
                Cv2.Line(src, p2, p3, Scalar.Red, 3, LineTypes.Link8);
                resultnum++;
            }
            Console.WriteLine("剔除后的轮廓数：" + Excontours.Length);
            string json = JsonConvert.SerializeObject(Excontours);
            //Console.WriteLine(json);
            string path = @"C:\toolkipweb\miniProgram\opencvtest\opencv\test.jpg";
            Cv2.ImWrite(path,src);
            path = "https://www.toolkip.com/miniProgram/opencvtest/opencv/test.jpg";
            new Window("result", WindowMode.FreeRatio, src);
            Window.WaitKey();
            return path+"--"+json;
        }
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
        public static void DrawLine()
        {
            string json = "";
            Mat src = Cv2.ImRead(@"C:\toolkipweb\miniProgram\opencvtest\opencv\2019061305281148.jpg");
            Point[][] point = JsonConvert.DeserializeObject<Point[][]>(json);
            for (int i = 0; i < point.Length; i++)
            {
                Cv2.Line(src, point[i][0], point[i][1], Scalar.Red);
                Cv2.Line(src, point[i][1], point[i][2], Scalar.Red);
                Cv2.Line(src, point[i][2], point[i][3], Scalar.Red);
                Cv2.Line(src, point[i][3], point[i][0], Scalar.Red);
            }
            new Window("result", WindowMode.FreeRatio, src);
            Window.WaitKey();
        }
    }
}
