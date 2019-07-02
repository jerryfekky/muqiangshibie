using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;
using Newtonsoft.Json;

namespace OpenCVAPP.OpenCV
{
   public class ContoursMethod
    {
        public static double[] per=new double[256];
        public static Mat src = new Mat();
        public static string path = "";
        
        public static string point = "";
        public static Point[][] ContoursBoost(Mat img)
        {
            Mat src = new Mat(),dst=new Mat();
            img.CopyTo(src);
            Point[][] Maxcontours = new Point[default][];
            HierarchyIndex[] hierarchys;
            int max = 0,state=0;
            while (state<2)
            {
                Mat gray = new Mat();
                //src = ImageMethod.EqualizeHistForColorImage(src);
                Point[][] contours = new Point[default][];
                if (state == 1)
                {
                    //Cv2.BilateralFilter(img,src,5,10,5/2);
                    ImageMethod.HistogramEqualization(src, src);
                    Cv2.CvtColor(src, gray, ColorConversionCodes.BGRA2GRAY);
                    Cv2.EqualizeHist(gray,gray);
                    ImageMethod.ImageEnhancementMethod2(gray, gray);
                }
                else
                {
                    src=ImageMethod.normalizeMat(src);
                    Cv2.CvtColor(src, gray, ColorConversionCodes.BGRA2GRAY);
                }
                Mat x = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(3, 3), new Point(-1, -1));//!!!调整size
                Cv2.MorphologyEx(gray, gray, MorphTypes.Erode, x);//!!!调整MorphTypes
                Cv2.Erode(gray,gray,x+2);
                int thresh = (100 / 4) * 2 + 1;//自适应阈值化
                //Getprobability(gray);
                Cv2.AdaptiveThreshold(gray, gray, 255, 0, ThresholdTypes.Binary, thresh, thresh / 3);
                //Cv2.Threshold(gray,gray,GetThreshold(per),255,ThresholdTypes.BinaryInv);
                //Cv2.Dilate(gray, gray, x);
                new Window("gray", WindowMode.FreeRatio, gray);
                Cv2.FindContours(gray, out contours, out hierarchys, RetrievalModes.External, ContourApproximationModes.ApproxSimple, null);
                if (contours.Length > max)
                {
                    Maxcontours = contours;
                    max = contours.Length;
                }
                state += 1;
            }

            
            DrawContours(Maxcontours, img);
            return Maxcontours;
        }

        public static void DrawContours(Point[][] contours,Mat dst)
        {
            Point2f[] point2Fs = new Point2f[default];
            int resultnum = 0;
            double min=190000,max=0;
            Point p0 = new Point(0, 0), p1 = new Point(0, 0), p2 = new Point(0, 0), p3 = new Point(0, 0);
            List<Point[]> Excontours = new List<Point[]>();
            for (int i = 0; i < contours.Length; i++)
            {
                RotatedRect rect = Cv2.MinAreaRect(contours[i]);
                double area =Math.Abs(Cv2.ContourArea(rect.Points(), false));
                if (area < 800)
                    continue;
                if (area >= max)
                    max = area;
                if(area<=min)
                    min = area;
                point2Fs = rect.Points();
                Point[] po = test.change(rect.Points());
                //point2.Add(point2Fs);
                Excontours.Add(po);
                for (int z = 0; z < point2Fs.Length; z++)//小数位精度——2
                {
                    point2Fs[z].X = (float)Math.Round(point2Fs[z].X, 2);
                    point2Fs[z].Y = (float)Math.Round(point2Fs[z].Y, 2);
                }
                for (int j = 0; j < 3; j++)
                {
                    p0 = new Point(point2Fs[j].X, point2Fs[j].Y);
                    p1 = new Point(point2Fs[j + 1].X, point2Fs[j + 1].Y);
                    Cv2.Line(dst, p0, p1, Scalar.Red, 3, LineTypes.Link8);
                }
                p2 = new Point(point2Fs[3].X, point2Fs[3].Y);
                p3 = new Point(point2Fs[0].X, point2Fs[0].Y);
                Point TP = new Point((((p0.X + p1.X) / 2)), ((p1.Y + p2.Y) / 2));
                Cv2.Line(dst, p2, p3, Scalar.Red, 3, LineTypes.Link8);
                resultnum++;
            }
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("最小轮廓为："+min);
            Console.WriteLine("最大轮廓为："+max);
            Console.WriteLine("剔除后的轮廓数：" + resultnum);
            string path = @"D:\OpenCV\ING\image\result1.jpg";
            Cv2.ImWrite(path, dst);
            new Window("result", WindowMode.FreeRatio, dst);
            Window.WaitKey();
        }
        public static int GetThreshold(double[] prob)
        {
            int threshold = 0;
            double maxf = 0;
            for (int crrct = 0; crrct < 255; crrct++)
            {
                double W1 = 0, W2 = 0, X1 = 0, X2 = 0 ;
                int i = 0;
                for (i = 0; i <=crrct; i++)
                {
                    X1 += i * prob[i];
                    W1 += prob[i];
                }
                for ( ; i < 256; i++)
                {
                    X2 += i + prob[i];
                    W2 += prob[i];
                }
                if (W2 == 0 || X2 == 0) continue;
                X1 /= W1;
                X2 /= W2;
                double D1 = 0, D2 = 0;
                for (i = 0; i <=crrct; i++)
                    D1 += Math.Pow((i - X1) *prob[i],2.0);
                for (; i < 256; i++)
                    D2 += Math.Pow((i-X2)*prob[i],2.0);
                D1 /= W1;
                D2 /= W2;
                double dw = Math.Pow(D1,2.0)*W1+Math.Pow(D2,2.0)*W2;
                double db = W1 * W2 * Math.Pow(X2 - X1, 2.0);
                double f = db / (db + dw);
                if (maxf < f)
                {
                    maxf = f;
                    threshold = crrct;
                }
            }
            return threshold;
        }
        public static void Getprobability(Mat src)
        {
            int width = src.Width, height = src.Height;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    //per[(int)cvGet2D(src,i,j).val[0]]++;
                    
                }
            }
            int pix = width * height;
            for (int i = 0; i < 256; i++)
            {
                per[i] = per[i] / pix;
            }
        }
        public static void GetFilter(Mat img)
        {
            Mat src = new Mat();
            img.CopyTo(src);
            new Window("src",WindowMode.FreeRatio,img);
            Point anchor = new Point(-1,-1);
            double delta = 0;
            int ddepth = -1;
            int ind = 0;
            int key = 0;
            int kernel_size = 0;
            Mat kernel = new Mat();
            while (ind<=5)
            {
                key = Window.WaitKey(500);
                if ((char)key == 27) break;
                kernel_size = 3 + 2 * (ind % 5);
                kernel = Mat.Ones(new Size(kernel_size, kernel_size),MatType.CV_32F)/(float)(kernel_size*kernel_size);
                Cv2.Filter2D(src,src,ddepth,kernel,anchor,delta,BorderTypes.Default);
                new Window("dst"+ind.ToString(),WindowMode.FreeRatio,src);
                ind++;
            }
            Window.WaitKey();
        }
        public static void GetGausian(Mat img) 
        {
            Mat src = new Mat();
            img.CopyTo(src);
            new Window("img", WindowMode.FreeRatio, img);
            Cv2.BilateralFilter(src,img,5,10,5/2);
            new Window("dst",WindowMode.FreeRatio,img);
            Window.WaitKey();
            
        }

        public static void OpenImgFile()
        {
            Console.WriteLine("输入路径：");
            path = Console.ReadLine();
            src = Cv2.ImRead(path);
            Cv2.NamedWindow("src",WindowMode.FreeRatio);
            Cv2.SetMouseCallback("src",Onmouse);
            Cv2.ImShow("src",src);
            Cv2.WaitKey();
            
        }
        public static int count = 0;
        public static List<Point> points = new List<Point>();
        private static void Onmouse(MouseEvent @event, int x, int y, MouseEvent flags, IntPtr userdata)
        {
            if ((@event==MouseEvent.LButtonDown))
            {
                Point pt = new Point(x,y);
                string text = x + "\n" + y;
                Console.WriteLine("{'X':"+x+",'Y':"+y+"}");
                points.Add(new Point(x,y));
                if (count-1!=-1)
                {
                    if (points[count - 1] != null)
                        Cv2.Line(src, points[count - 1], points[count], Scalar.Red,2);
                    if (count == 3)
                        Cv2.Line(src, points[count], points[0], Scalar.Red,2);
                }
                
                Cv2.PutText(src,text,pt,HersheyFonts.HersheyComplex,1,Scalar.AliceBlue);
                Cv2.Circle(src,pt,2,Scalar.Red);
                Cv2.ImShow("src",src);
                if (count==3)
                {
                    Cv2.DestroyWindow("src");
                    point=JsonConvert.SerializeObject(points);
                    test.Pic_Rectity(path,point);
                }
                count++;
            }
            
        }

    }
}
