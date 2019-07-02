using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;

namespace OpenCVAPP.OpenCV
{
    public class FindBody
    {
        public static void bodytype()
        {
            Mat src = Cv2.ImRead(@"D:\OpenCV\Output\石头\stone7.jpg");
            Mat hsv = new Mat();
            double maxH = 190, minH = 70, maxS = 255, minS =100;
            Cv2.CvtColor(src,hsv,ColorConversionCodes.BGR2HSV);
            Mat[] channels= { };
            Cv2.Split(src,out channels);
            Mat mask = new Mat(),mask1=new Mat(), mask2 = new Mat();
            Mat Hmask=new Mat(),Smask=new Mat();
            Cv2.Threshold(channels[0],mask1,maxH,255,ThresholdTypes.BinaryInv);
            Cv2.Threshold(channels[0], mask2, minH, 255, ThresholdTypes.Binary);
            if (minH < maxH) Hmask = mask1 & mask2;
            else Hmask = mask1 | mask2;
            Cv2.NamedWindow("H", 0);
            Cv2.ResizeWindow("H", 500, 500);
            Cv2.ImShow("H", Hmask);
            Cv2.Threshold(channels[1], mask1, maxS, 255, ThresholdTypes.BinaryInv);
            Cv2.Threshold(channels[1], mask2, minS, 255, ThresholdTypes.Binary);
            Smask = mask1 & mask2;
            mask = Hmask & Smask;
            Cv2.NamedWindow("S",0);
            Cv2.ResizeWindow("S",500,500);
            Cv2.ImShow("S",Smask);
            Cv2.NamedWindow("mask", 0);
            Cv2.ResizeWindow("mask", 500, 500);
            Cv2.ImShow("mask", mask);
            Mat detected = new Mat(src.Size(),MatType.CV_8UC3,new Scalar(0,0,0));
            src.CopyTo(detected,mask);
            Cv2.NamedWindow("dst", 0);
            Cv2.ResizeWindow("dst", 500, 500);
            Cv2.ImShow("dst", detected);
            Window.WaitKey();
        }
        
        private static Point[][] contours;
        private static HierarchyIndex[] hierarchys;
        public static void type2()
        {
            Mat src = Cv2.ImRead(@"D:\OpenCV\ING\curtainwall\aaaaa.png");
            //src=Findarea(src);
            //Window.WaitKey();
            Mat hsv = new Mat(),threshold=new Mat();
            Mat dst = new Mat(),dst2=new Mat();
            //Scalar HsvRedLow = new Scalar(0, 40, 40);
            //Scalar HsvRedHigh = new Scalar(40, 255, 255);
            //Scalar HsvGreenLow = new Scalar(41, 40, 40);
            //Scalar HsvGreenHigh = new Scalar(90, 255, 255);
            //Scalar HsvBlueLow = new Scalar(100, 40, 40);
            //Scalar HsvBlueHigh = new Scalar(140, 255, 255);
            Scalar HsvRedLow = new Scalar(0, 0,1);//白 white
            Scalar HsvRedHigh = new Scalar(0.1156, 0.2480, 0.9804);
            Scalar HsvGreenLow = new Scalar(0, 0, 46);//灰 gray
            Scalar HsvGreenHigh = new Scalar(180, 43, 220);
            Scalar HsvBlueLow = new Scalar(100, 43, 46);//蓝 blue
            Scalar HsvBlueHigh = new Scalar(124, 255, 255);
            Cv2.CvtColor(src,hsv,ColorConversionCodes.BGR2HSV);
            Cv2.NamedWindow("hsv", 0);
            Cv2.ResizeWindow("hsv", 500, 500);
            Cv2.ImShow("hsv", hsv);
            Scalar[] HsvLow = { HsvRedLow,HsvGreenLow,HsvBlueLow};
            Scalar[] HsvHigh = { HsvRedHigh,HsvGreenHigh,HsvBlueHigh};
            string[] textcolor = {"White","Gray","Blue" };
            Point[][] contours;//= Findarea(src);
            HierarchyIndex[] hierarchy;// = hierarchys;
            for (int color = 0; color <3; color++)
            {
                Cv2.InRange(hsv, HsvLow[color], HsvHigh[color], threshold);
                Cv2.Threshold(threshold, threshold, 1, 255, ThresholdTypes.Binary);
                Cv2.CopyMakeBorder(threshold, dst, 1, 1, 1, 1, BorderTypes.Constant, 0);

                Mat a = Cv2.GetStructuringElement(MorphShapes.Ellipse, new Size(5, 5));//5.23
                Cv2.MorphologyEx(dst, dst, MorphTypes.Open, a);
                //dst = threshold;
                
                //Cv2.CvtColor(src,dst2,ColorConversionCodes.BGR2GRAY);
                //Cv2.Threshold(dst2,dst2,100,255,ThresholdTypes.Binary);
                //Cv2.FindContours(dst2, out contours, out hierarchy, RetrievalModes.Tree, ContourApproximationModes.ApproxSimple);
                //Cv2.ImShow("ss",dst2);
                Cv2.NamedWindow(textcolor[color]+"轮廓", 0);
                Cv2.ResizeWindow(textcolor[color] + "轮廓", 500, 500);
                Cv2.ImShow(textcolor[color] + "轮廓", dst);
                Cv2.FindContours(dst, out contours, out hierarchy, RetrievalModes.Tree, ContourApproximationModes.ApproxSimple);
                //Console.WriteLine(contours.Length);
                int count = 0;
                for (int i = 0; i < contours.Length; i++)
                {
                    if (contours[i].Length <200)
                        continue;
                    Rect bound = Cv2.BoundingRect(contours[i]);
                    //Cv2.DrawContours(src, contours, i, Scalar.Black, 1, LineTypes.Link8, hierarchy, 0, new Point(0, 0));
                    Point center = new Point(bound.X + bound.Width / 2, bound.Y + bound.Height / 2);

                    //int[] bc = { center.X, center.Y };
                    //char x = threshold.At<char>(bc);
                    //Console.WriteLine(x);
                    if (true)
                    {
                        count++;
                       Cv2.PutText(src,textcolor[color],center,HersheyFonts.HersheyComplex,1,Scalar.Black,1,LineTypes.Link8);
                       Cv2.Circle(src,center,10,Scalar.Gray);
                    }
                }
                Console.WriteLine(textcolor[color]+"区域数量:"+count);
            }
            Cv2.NamedWindow("result", 0);
            Cv2.ResizeWindow("result", 500, 500);
            Cv2.ImShow("result", src);
            Window.WaitKey();
        }








        public static Point[][]  Findarea(Mat src)
        {
            Mat img = src;
            Mat gray = new Mat();
            Mat black = new Mat();
            //Point[][] contours;
            //HierarchyIndex[] hierarchy;
            Point2f[] point2Fs = new Point2f[] { };
            List<Point2f[]> point2 = new List<Point2f[]>();
            Point p0 = new Point(0, 0), p1 = new Point(0, 0), p2 = new Point(0, 0), p3 = new Point(0, 0);
            Mat soX = new Mat(), soY = new Mat();

            Cv2.Laplacian(img, gray, 0, 1, 1, 0, BorderTypes.Default);
            Cv2.CvtColor(img, gray, ColorConversionCodes.BGR2GRAY, 0);
            //Cv2.Sobel(gray, soX, MatType.CV_8U, 1, 0);
            //Cv2.Sobel(gray, soY,MatType.CV_8U, 0,1);
            //Cv2.AddWeighted(soX,0.5,soY,0.5,0,gray);
            Cv2.Blur(gray, gray, new Size(10, 10));
            int thresh_size = (100 / 4) * 2 + 1;//自适应阈值化
            Cv2.AdaptiveThreshold(gray, black, 255, 0, ThresholdTypes.Binary, thresh_size, thresh_size / 3);
            //Cv2.Threshold(gray,black,100,255,ThresholdTypes.Binary);
            Mat x = Cv2.GetStructuringElement(MorphShapes.Ellipse, new Size(5, 5));//!!!调整size
            Cv2.MorphologyEx(black, black, MorphTypes.Open, x);//!!!调整MorphTypes
            //new Window("二值图", WindowMode.FreeRatio, black);
            Cv2.FindContours(black, out contours, out hierarchys, RetrievalModes.External, ContourApproximationModes.ApproxSimple, null);
            int resultnum = 0;
            Point[][] Excontours= contours ;
            for (int i = 0; i < hierarchys.Length; i++)
            {
                if (contours[i].Length < 100)
                    continue;
                RotatedRect rect = Cv2.MinAreaRect(contours[i]);
                point2Fs = rect.Points();
                Point[] po=change(rect.Points());
                //point2.Add(point2Fs);
                Excontours[resultnum] = po;
                for (int j = 0; j < 3; j++)
                {
                    p0 = new Point(point2Fs[j].X, point2Fs[j].Y);
                    p1 = new Point(point2Fs[j + 1].X, point2Fs[j + 1].Y);
                    Cv2.Line(img, p0, p1, Scalar.Red, 1, LineTypes.Link8);
                }
                p2 = new Point(point2Fs[3].X, point2Fs[3].Y);
                p3 = new Point(point2Fs[0].X, point2Fs[0].Y);
                Point TP = new Point((((p0.X + p1.X) / 2)), ((p1.Y + p2.Y) / 2));
                Cv2.Line(img, p2, p3, Scalar.Red, 1, LineTypes.Link8);
                //Cv2.PutText(img, resultnum.ToString(), TP, HersheyFonts.HersheyComplex, 2, Scalar.Blue, 1, LineTypes.Link8, false);
                resultnum++;
            }
            Console.WriteLine("剔除后的轮廓数："+resultnum);
            
            new Window("results", WindowMode.FreeRatio, img);
            return Excontours;
            Window.WaitKey(0);
        }
        public static Point[] change(Point2f[] point)
        {
            Point[] points = new Point[point.Length];
            for (int i = 0; i < point.Length; i++)
            {
                points[i].X = (int)Math.Round(point[i].X,0);
                points[i].Y = (int)Math.Round(point[i].Y, 0);
            }
            return points;
        }
    }
}
