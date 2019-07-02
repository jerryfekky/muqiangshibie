using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCVAPP.OpenCV
{
    class ImageMethod
    {
        private static object responses;
        private static Scalar points;

        //1. 基于直方图均衡化彩色图像增强（增强后检测效果提升较小）
        public static Mat HistogramEqualization(Mat inMat, Mat outMat)//输入图像、输出图像
        {
            Mat[] inMatMV = new Mat[3];
            Cv2.Split(inMat, out inMatMV);//通道分离函数
            for (int i = 0; i < 3; i++)
            {
                Cv2.EqualizeHist(inMatMV[i], inMatMV[i]);
            }
            Cv2.Merge(inMatMV, outMat);
            return outMat;
        }

        //2. 基于拉普拉斯算子彩色图像增强（有质的提升）
        public static Mat LaplacianMethod(Mat inMat, Mat outMat, int ddept, int ksize, double scale, double delta, BorderTypes borderType)//输入图像、输出图像、目标图像深度、二阶导数的滤波器孔径尺寸、比例因子、结果存入目标图像前的可选值、边界模式
        {
            Cv2.Laplacian(inMat, outMat, ddept, ksize, scale, delta, borderType);
            return outMat;
        }

        //3. 彩色图像灰度处理
        public static Mat GrayProcessingMethod(Mat inMat, Mat outMat, ColorConversionCodes codeType, int dstCn)
        {
            Cv2.CvtColor(inMat, outMat, codeType, dstCn);
            return outMat;
        }

        //4. 图像增强方法1（输入图像为灰度图像）(通过Sobel算子X轴方向与Y轴方向的梯度进行融合进行增强) 
        public static Mat ImageEnhancementMethod1(Mat inMat, Mat outMat)
        {
            Mat soX = new Mat();
            Mat soY = new Mat();
            Cv2.Sobel(inMat, soX, MatType.CV_8U, 1, 0, 3, 1, 1);//获取Sobel处理的X轴方向梯度
            Cv2.Sobel(inMat, soY, MatType.CV_8U, 0, 1, 3, 1, 1);//获取Sobel处理的Y轴方向梯度
            Cv2.AddWeighted(soX, 0.5, soY, 0.5, 0, outMat);//加权,对其进行合并梯度处理
            return outMat;
        }

        //5. 图像增强方法2（输入图像为灰度图像）(通过clahe函数进行自适应直方图均衡化对其进行图像增强) 
        public static Mat ImageEnhancementMethod2(Mat inMat, Mat outMat)
        {
            //Mat mat5 = new Mat();
            using (CLAHE clahe = Cv2.CreateCLAHE())
            {
                //clahe.ClipLimit = 20;

                clahe.TilesGridSize = new Size(4, 4);
                clahe.Apply(inMat, outMat);
                //clahe.ClipLimit = 40;
                //clahe.Apply(src, dst2);
                //clahe.TilesGridSize = new Size(4, 4);
                //clahe.Apply(src, dst3);
            }
            return outMat;
        }

        //6. 高斯处理（输入图像为灰度图像）
        public static Mat GaussianMethod(Mat inMat, Mat outMat, int a, int b, double sigmaX, double sigmaY, BorderTypes borderType)
        {
            Cv2.GaussianBlur(inMat, outMat, new Size(a, b), sigmaX, sigmaX, borderType);
            return outMat;
        }

        //7. 形态学去噪（输入图像为黑白图像）
        public static Mat MorphologicalMethod(Mat inMat, Mat outMat, int a, int b, MorphTypes types)
        {
            Mat x = Cv2.GetStructuringElement(MorphShapes.Ellipse, new Size(a, b));//调整size
            Cv2.MorphologyEx(inMat, outMat, types, x);
            //输入图像、输出图像、types为open时：对图像先腐蚀再膨胀，可以排除小团物体；types为闭运算时（对图像先膨胀再腐蚀，可以排除小型黑洞）
            return outMat;
        }

        //8. 基于对数Log变换的图像增强
        public static Mat LogTransformation(Mat inMat, Mat outMat)//输入图像、输出图像
        {
            Mat dataMat = new Mat(inMat.Rows, 2, MatType.CV_32FC3, points);
            Mat dataMat1 = new Mat(inMat.Cols, 2, MatType.CV_32FC3, points);
            Mat imageLog = new Mat(dataMat.Size(), MatType.CV_32FC3);
            //new Window("dataMat.Rows的值", WindowMode.AutoSize, outMat);
            //Console.WriteLine($"dataMat.Rows的值 {dataMat.Rows} ");
            //Console.WriteLine($"dataMat.Cols的值 {dataMat.Cols} ");
            for (int i = 0; i < dataMat.Rows; i++)
            {
                for (int j = 0; j < dataMat.Cols; j++)
                {
                    //imageGamma = MorphologicalMethod(1+ dataMat[i][j][0]);

                    //imageLog.At<Vec3f>(i, j)[0] = Math.Log(1 + dataMat.At<Vec3b>(i, j)[0]);
                    //imageLog.At<Vec3f>(i, j)[1] = Math.Log(1 + dataMat.At<Vec3b>(i, j)[1]);
                    //imageLog.At<Vec3f>(i, j)[2] = Math.Log(1 + dataMat.At<Vec3b>(i, j)[2]);
                    imageLog.Set<double>(i, j, Math.Log(1 + dataMat.At<double>(i, j)));
                    //imageLog.Set<double>(i, j, Math.Log(1 + dataMat.At<Vec3b>(i, j)[1]));
                    //imageLog.Set<double>(i, j, Math.Log(1 + dataMat.At<Vec3b>(i, j)[2]));
                }
            }
            outMat = inMat;
            //new Window("基于对数LOG变换的彩色图像增强图像", WindowMode.AutoSize, outMat);
            return outMat;
        }
        //9.Gamma校正(效果不好，图像变黑)
        public static void gamma(Mat src,Mat dst)
        {
            Mat x = new Mat();
            src.ConvertTo(x,MatType.CV_32FC3);
            Mat i = new Mat();
            Cv2.Pow(x, 3, i);
            Cv2.Normalize(i,dst,0,255,NormTypes.MinMax,MatType.CV_8UC3);
            new Window("result",WindowMode.FreeRatio,dst);
            Window.WaitKey();
        }
        //自适应对比度增强
        public static void adaptContrastEnhancement(Mat src,Mat dst,int maxcg)
        {
            Mat ycc = new Mat();
            Cv2.CvtColor(src,ycc,ColorConversionCodes.RGB2YCrCb);
            Mat[] channels = new Mat[3];
            Cv2.Split(ycc,out channels);
            Mat localMeansMatrix = new Mat(src.Rows, src.Cols, MatType.CV_32FC1);
            Mat localVarianceMatrix = new Mat(src.Rows,src.Cols,MatType.CV_32FC1);
            Mat temp = channels[0].Clone();
            Scalar mean;
            Scalar dev;
            Cv2.MeanStdDev(temp,out mean,out dev);
            double meanGlobal = mean.Val0;
            Mat enhanceMatrix = new Mat(src.Rows,src.Cols,MatType.CV_8UC1);
            for (int i = 0; i < src.Rows; i++)
            {
                for (int j = 0; j < src.Cols; j++)
                {
                    if (localVarianceMatrix.At<float>(i, j) >= 0.01)
                    {
                        double cg = 0.2 * meanGlobal / localVarianceMatrix.At<float>(i, j);
                        double cgs = cg > maxcg ? maxcg : cg;
                        cgs = cgs < 1 ? 1 : cgs;
                        int e = Convert.ToInt32(localVarianceMatrix.At<float>(i, j) + cgs * (temp.At<int>(i, j) - localMeansMatrix.At<float>(i, j)));
                        if (e > 255) e = 255;
                        else if (e < 0) e = 0;
                        enhanceMatrix.Set<int>(i, j,e);


                    }
                    else enhanceMatrix.Set<int>(i,j,temp.At<int>(i, j));
                }
            }
            channels[0] = enhanceMatrix;
            Cv2.Merge(channels,ycc);
            Cv2.CvtColor(ycc,dst,ColorConversionCodes.YCrCb2RGB);
        }
        //矩阵归一化
        public static Mat normalizeMat(Mat src)
        {
            Mat img = new Mat();
            src.CopyTo(img);
            double min = 0, max = 0;
            Cv2.MinMaxLoc(img, out min,out max);
            Mat norMat = (img - min) / (max - min) * 255;
            Mat dst = new Mat();
            norMat.ConvertTo(dst,MatType.CV_8U);
            return dst;
        }
        //调整对比度和亮度
        public static void SetImageCast(Mat img)
        {
            double alpha = 1.0;
            int beta = 0;
            Mat src = new Mat();
            img.CopyTo(src);
            Mat dst = Mat.Zeros(src.Size(),src.Type());
            for (int i = 0; i < src.Rows; i++)
            {
                for (int j = 0; j < src.Cols; j++)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        dst.Set<double>(i,j,alpha*(src.At<Vec3b>(i,j)[k])+beta);
                    }
                }
            }
        }
        //彩色图片直方图均衡化
        public static Mat EqualizeHistForColorImage(Mat src)
        {
            Mat Image = new Mat(src.Size(),src.Depth(),3);
            int max_channel = 4;
            Mat[] ImageChannel = new Mat[4];
            int i = 0;
            for ( i = 0; i < src.Channels(); i++)
            {
                ImageChannel[i] = new Mat(src.Size(), src.Depth(), 1);
            }
            Cv2.Split(src,out ImageChannel);
            for (i = 0; i < src.Channels(); i++)
            {
                Cv2.EqualizeHist(ImageChannel[i],ImageChannel[i]);
            }
            Cv2.Merge(ImageChannel,Image);
            return Image;
        }
    }
}
