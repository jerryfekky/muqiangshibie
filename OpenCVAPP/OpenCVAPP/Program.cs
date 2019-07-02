using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;
using OpenCVAPP.OpenCV;
using OpenCVAPP.Model;
using Newtonsoft.Json;
using System.Configuration;
using System.Web;

namespace OpenCVAPP
{
    class Program
    {
        static void Main(string[] args)
        {
        //string json = "[[{\"X\":129,\"Y\":257},{\"X\":223,\"Y\":257},{\"X\":223,\"Y\":418},{\"X\":129,\"Y\":418}],[{\"X\":230,\"Y\":58},{\"X\":319,\"Y\":58},{\"X\":319,\"Y\":208},{\"X\":230,\"Y\":208}]]";
        //string files = "https://www.toolkip.com/miniProgram/opencvtest/opencv/2019060503544552.jpg";
        //UploadFile.Upload(@"D:\\OpenCV\\ING\\curtainwall\\3e8265573ebf047a10a772f107c6d54.jpg");
        //string jsons = "[[{'X':1443,'Y':2585},{'X':1657,'Y':2585},{'X':1664,'Y':2694},{'X':1443,'Y':2702}],[{'X':1682,'Y':2593},{'X':1885,'Y':2589},{'X':1893,'Y':2671},{'X':1704,'Y':2702}]]";
        //string json = "[{'X':571,'Y':991},{'X':880,'Y':995},{'X':880,'Y':1431},{'X':512,'Y':1431}]";
        //string file=Base64Convert.FileToBase64(@"C:\toolkipweb\miniProgram\opencvtest\opencv\ffa5c72bb1e6d6ae8a7313480774c2b(2).jpg");
        //bool save = Base64Convert.Base64ToFileAndSave(Class1.fffff, @"C:\toolkipweb\miniProgram\opencvtest\opencv\333.jpg");
        //int originalImageFileId = 967, buildingWallId = 66;
        //Img_Rectity.Pic_Rectity(file, json,originalImageFileId,buildingWallId);
        //Img_Rectity.GetCase2Texture(files,jsons);
        //string json = "{\"buildingWallId\":66,\"buildingWallImageId\":31,\"blocks\":[{\"width\":49,\"height\":61,\"blockLocationTopLeftX\":319,\"blockLocationTopLeftY\":374,\"value\":\"石材 - 背栓式\"},{\"width\":113,\"height\":62,\"blockLocationTopLeftX\":199,\"blockLocationTopLeftY\":373,\"value\":\"石材 - 背栓式\"},{\"width\":114,\"height\":63,\"blockLocationTopLeftX\":77,\"blockLocationTopLeftY\":372,\"value\":\"石材 - 背栓式\"},{\"width\":111,\"height\":63,\"blockLocationTopLeftX\":81,\"blockLocationTopLeftY\":301,\"value\":\"石材 - 背栓式\"},{\"width\":60,\"height\":214,\"blockLocationTopLeftX\":308,\"blockLocationTopLeftY\":81,\"value\":\"石材 - 背栓式\"},{\"width\":107,\"height\":214,\"blockLocationTopLeftX\":200,\"blockLocationTopLeftY\":81,\"value\":\"石材 - 背栓式\"},{\"width\":98,\"height\":48,\"blockLocationTopLeftX\":99,\"blockLocationTopLeftY\":25,\"value\":\"石材 - 背栓式\"}]}";
        //UploadFile.Upload(@"C:\toolkipweb\miniProgram\opencvtest\opencv\111.jpg");
        //int a=UploadFile.Upload(@"C:\toolkipweb\miniProgram\opencvtest\opencv\real(1).jpg");
        //HTTPService.GetUpBuildingInfo(252);
        //HTTPService.UpdateWallimg(121,99);
        //GetInfo.AddBuildingWallBlock(Class1.aaa);
        //GetInfo.GetAssembleImgInfo(2606);
        //string jsons = "{\"data\":[{\"positionX\":92,\"positionY\":290,\"width\":102,\"height\":121,\"id\":459}]}";
        //string code = "\"{\"玻璃\":[2180,2179,2178,2177,2184,2183,2182,2181,2188,2187,2186,2185,2192,2191,2196,2195,2194,2193],\"玻璃 - 隐框\":[2217,2223,2222,2221,2220,2219,2218,2215,2214,2203,2204,2202,2201,2200,2199,2198,2197,2205,2213,2212,2211,2210,2209,2208,2207,2206,2216,2224],\"石头\":[2190,2189]}";
        //GetInfo.UpdateBuildingAssemble(jsons,code,file);
        //string a = "Insi546755KDIjpbMjE4MCwyMTc5LDIxNzgsMjE3NywyMTg0LDIxODMsMjE4MiwyMTgxLDIxODgsMjE4NywyMTg2LDIxODUsMjE5MiwyMTkxLDIxOTYsMjE5NSwyMTk0LDIxOTNdLCLnjrvnkoMgLSDpmpDmoYYiOlsyMjE3LDIyMjMsMjIyMiwyMjIxLDIyMjAsMjIxOSwyMjE4LDIyMTUsMjIxNCwyMjAzLDIyMDQsMjIwMiwyMjAxLDIyMDAsMjE5OSwyMTk4LDIxOTcsMjIwNSwyMjEzLDIyMTIsMjIxMSwyMjEwLDIyMDksMjIwOCwyMjA3LDIyMDYsMjIxNiwyMjI0XSwi55 z5aS0IjpbMjE5MCwyMTg5XX0=";
        //a = Convert.ToBase64String(Encoding.UTF8.GetBytes(a));
        //Console.WriteLine(a);
        //a = a.Replace(" ","+");
        //Console.WriteLine(Encoding.UTF8.GetString(Convert.FromBase64String(a)));
        //GetInfo.GetAssembleInfo("[459,430]");
        //GetInfo.GetBuidlingWallImgInfo("[427,430]");
        //06.13
        //Servicetest.aaa();

            string file = @"D:\OpenCV\ING\curtainwall\IMG_20190421_130008.jpg";
            //string file = @"C:\toolkipweb\miniProgram\opencvtest\opencv\sss.jpg";
            string point= "[{'X':718,'Y':1523},{'X':2391,'Y':1673},{'X':2982,'Y':3406},{'X':108,'Y':3324}]";
            //test.Pic_Rectity(file,point);

            ContoursMethod.OpenImgFile();
            



            //Mat src = Cv2.ImRead(file);
            //Mat dst = new Mat();
            //Cv2.CvtColor(src,src,ColorConversionCodes.BGR2GRAY);
            //src=ImageMethod.EqualizeHistForColorImage(src);
            //new Window("result",WindowMode.FreeRatio,src);
            //Window.WaitKey();
            //test.FindContours(Cv2.ImRead(file));
            //Img_Rectity.Findarea(Cv2.ImRead(file));
            //Mat s = Cv2.ImRead(@"D:\OpenCV\ING\curtainwall\31d90ea3a1f9f305404ad9778cfec61.jpg");
            //Mat src = Cv2.ImRead(@"C:\toolkipweb\miniProgram\opencvtest\opencv\sss.jpg");
            //Img_Rectity.Findarea(src);
            //Console.WriteLine(s.Width+","+s.Height);
            //test.FindContours(s);
            //string value = "啦啦啦";
            //Encoding utf8 = Encoding.GetEncoding(65001);
            //byte[] temp = utf8.GetBytes(value);
            //value = HttpUtility.UrlEncode(temp);
            //Console.WriteLine(value);
            //string res=HttpSender.Sender.Post("http://dev.strosoft.com:6090/BSS/api/Service?service_name=UpdateBuildingWallBlockValue&data={id:1495,value:'"+value+"'}","");
            //Console.WriteLine(res);
        }
    }
}
