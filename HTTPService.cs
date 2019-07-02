using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using HttpSender;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenCVAPP.Model;
using System.Net;

namespace OpenCVAPP.OpenCV
{
    public class HTTPService
    {
        public static string test()
        {
            string Response = Sender.Get("http://dev.strosoft.com:6090/BSS/api/Service?service_name=AddBuildingWallBlockValue&data={buildingWallBlockId:1,buildingWallBlockParameterId:1,value:\"AAAAAAAAA\"}") ;

            Console.WriteLine(Response);
            return Response;
        }
        //添加立面图片
        public static string AddWallImage(BuildingWallImage data)
        {
            string jsondata = JsonConvert.SerializeObject(data);
            string Response = Sender.Post("http://dev.strosoft.com:6090/BSS/api/Service?service_name=AddBuildingWallImage&data="+jsondata,"");
            //Buildingwallresult result = JsonConvert.DeserializeObject<Buildingwallresult>(Response);
            //Console.WriteLine(result.data);
            return Response;
        }
        //添加慕墙信息
        public static string AddWallBlock(BuildingWallBlock block,string name,string value)
        {
            value = Convert.ToBase64String(Encoding.UTF8.GetBytes(value));
            //value = Encoding.UTF8.GetString(Convert.FromBase64String(value));
            string jsondata = JsonConvert.SerializeObject(block);
            string Response = Sender.Post("http://dev.strosoft.com:6090/BSS/api/Service?service_name=AddBuildingWallBlock&data="+jsondata,"");
            ResultMessage result = JsonConvert.DeserializeObject<ResultMessage>(Response);
            Console.WriteLine(Response);
            Response = Sender.Post("http://dev.strosoft.com:6090/BSS/api/Service?service_name=AddBuildingWallBlockParameter&data={name:\"" + name+"\"}", "");
            ResultMessage result1 = JsonConvert.DeserializeObject<ResultMessage>(Response);
            Response = Sender.Post("http://dev.strosoft.com:6090/BSS/api/Service?service_name=AddBuildingWallBlockValue&data={\"buildingWallBlockId\":" + result.data+",\"buildingWallBlockParameterId\":"+result1.data+",\"value\":\""+value+ "\"}", "");

            Console.WriteLine(Response);
            return Response;
            
        }
        //创建立面
        public static string AddBuildingWall(BuildingWall data)
        {
            //Buildingwallresult result = new Buildingwallresult();
            string js =JsonConvert.SerializeObject(data);
            string Response = Sender.Post("http://dev.strosoft.com:6090/BSS/api/Service?service_name=AddBuildingWall&data="+js,"");
            //result = JsonConvert.DeserializeObject<Buildingwallresult>(response);
            //Console.WriteLine(result.data);
            return Response;
        }
        //获取立面矫正图-修改
        public static string GetUpBuildingInfo(int id)
        {
            try
            {
                string path = ConfigurationSettings.AppSettings["Savepath"];
                string path2 = ConfigurationSettings.AppSettings["Returnpath"];
                string time = DateTime.Now.ToString("yyyyMMddhhmmssff") + ".jpg";
                string savepath = path + time;
                string returnpath = path2 + time;
                string Response = Sender.Get("http://dev.strosoft.com:6090/BSS/api/Service?service_name=GetBuildingWallImage&data={id:" + id + "}");
                //Response = "{\"success\":true,\"data\":{\"adjustedImageFileId\":626,\"areaBottomLeftX\":1944,\"areaBottomLeftY\":3237,\"areaBottomRightX\":938,\"areaBottomRightY\":3229,\"areaTopLeftX\":1049,\"areaTopLeftY\":2144,\"areaTopRightX\":1851,\"areaTopRightY\":2155,\"buildingWallId\":133,\"createTime\":{\"date\":4,\"day\":2,\"hours\":18,\"minutes\":4,\"month\":5,\"nanos\":0,\"seconds\":48,\"time\":1559642688000,\"timezoneOffset\":-480,\"year\":119},\"description\":\"\",\"id\":70,\"name\":\"ç«\u008bé\u009d¢A\",\"originalImageFileId\":625,\"positionX\":0,\"positionY\":0,\"resultImageFileId\":628,\"selectedImageFileId\":627,\"sysUserId\":1},\"adminRootUrl\":\"http://localhost:8080/BSS/\"}";
                JObject jResult = JObject.Parse(Response);
                JObject jData = (JObject)jResult["data"];
                BuildingWallImage result = jData.ToObject<BuildingWallImage>();
                Console.WriteLine(result.adjustedImageFileId);
                Response = Sender.Get("http://dev.strosoft.com:6090/BSS/api/Service?service_name=GetTable&data={TableName:\"sys_upload_file\",Condition:\"id =" + result.adjustedImageFileId + "\"}");
                jResult = JObject.Parse(Response);
                jData = (JObject)jResult["data"][0];
                UploadResult Imgurl = jData.ToObject<UploadResult>();
                Console.WriteLine(Imgurl.path);//获取到了图片 "http://dev.strosoft.com:6090/BSS"+Imgurl.path
                Response = Sender.Get("http://dev.strosoft.com:6090/BSS/api/Service?service_name=GetViewBuildingWallBlockList&data={buildingWallImageId:" + id + ",pageSize:200}");
                jResult = JObject.Parse(Response);
                JArray jarray = (JArray)jResult["data"]["dataList"];
                List<GetBuildingBlock> block = jarray.ToObject<List<GetBuildingBlock>>();
                GetBlockList blocks = new GetBlockList();
                BlockTemp temp = new BlockTemp();
                List<GetBlockValue> bv = new List<GetBlockValue>();
                 List<BuildingWallBlock> bl = new List<BuildingWallBlock>();
                for (int i = 0; i < block.Count(); i++)
                {
                    GetBlockValue Blockvalue = new GetBlockValue();
                    BuildingWallBlock blo = new BuildingWallBlock();
                    Response = Sender.Get("http://dev.strosoft.com:6090/BSS/api/Service?service_name=GetBuildingWallBlockValueList&data={buildingWallBlockId:" + block[i].id + ",pageSize:200}");//block[i].id
                    Console.WriteLine(Response);
                    jResult = JObject.Parse(Response);
                    jData = (JObject)jResult["data"]["dataList"][0];
                    BlockValueSQL bs = jData.ToObject<BlockValueSQL>();
                    Blockvalue.id = bs.id;
                    Blockvalue.buildingWallBlockId = bs.building_wall_block_id;
                    Blockvalue.buildingWallBlockParameterId = bs.building_wall_block_parameter_id;
                    bs.value = bs.value.Replace(" ","+");
                    Blockvalue.value = Encoding.UTF8.GetString(Convert.FromBase64String(bs.value));
                    blo.adjustedLocationTopLeftX = block[i].adjusted_location_top_left_x;
                    blo.adjustedLocationTopLeftY = block[i].adjusted_location_top_left_y;
                    blo.blockLocationTopLeftX = block[i].block_location_top_left_x;
                    blo.blockLocationTopLeftY = block[i].block_location_top_left_y;
                    blo.buildingWallBlockNo = block[i].building_wall_block_no;
                    blo.buildingWallImageId = block[i].building_wall_image_id;
                    blo.buildingWallId = block[i].building_wall_id;
                    blo.id = block[i].id;
                    blo.height = block[i].height;
                    blo.width = block[i].width;
                    bv.Add(Blockvalue);
                    bl.Add(blo);
                }
                blocks.Blockvalue = bv; blocks.block = bl;
                blocks.WallImg = "http://dev.strosoft.com:6090/BSS/" + Imgurl.path;
                WebClient download = new WebClient();
                download.DownloadFile(blocks.WallImg,savepath);
                blocks.WallImg = returnpath;
                string js = JsonConvert.SerializeObject(blocks);

                Console.WriteLine(js);
                return js;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message+"未查到数据或数据表信息不完整！";
            }
           
        } 
        //更新板块信息
        public static string UpdateWallBlock(BuildingWallBlock block,GetBlockValue blockvalue)
        {
            blockvalue.value= Convert.ToBase64String(Encoding.UTF8.GetBytes(blockvalue.value));
            string update = JsonConvert.SerializeObject(block);
            string Response = Sender.Post("http://dev.strosoft.com:6090/BSS/api/Service?service_name=UpdateBuildingWallBlock&data="+update, "");
            update = JsonConvert.SerializeObject(blockvalue);
            Response = Sender.Post("http://dev.strosoft.com:6090/BSS/api/Service?service_name=UpdateBuildingWallBlockParameter&data="+update, "");
            return Response;
        }
        //更新立面图片检测结果
        public static string UpdateWallimg(int id,int resultid)
        {
            string Response = Sender.Post("http://dev.strosoft.com:6090/BSS/api/Service?service_name=UpdateBuildingWallImage&data={id:"+id+ ",resultImageFileId:"+resultid+"}", "");

            return Response;
        }
        //获取立面图片信息
        public static string GetBuildingImgInfo(int[] id)
        {
            string path = ConfigurationSettings.AppSettings["Savepath"];
            string path2 = ConfigurationSettings.AppSettings["Returnpath"];
            string js = "";
            //BuildingWallImage Rinfo = new BuildingWallImage();
            
            List<ReturnImgInfo> img = new List<ReturnImgInfo>();
           
            for (int i = 0; i < id.Length; i++)
            {
                ReturnImgInfo info = new ReturnImgInfo();
                List<BuildingWallBlock> bl = new List<BuildingWallBlock>();
                List<GetBlockValue> bv = new List<GetBlockValue>();
                string Response = Sender.Get("http://dev.strosoft.com:6090/BSS/api/Service?service_name=GetBuildingWallImage&data={id:" + id[i] + " }");
                //Response = "{\"success\":true,\"data\":{\"adjustedImageFileId\":1999,\"areaBottomLeftX\":737,\"areaBottomLeftY\":1309,\"areaBottomRightX\":368,\"areaBottomRightY\":1309,\"areaTopLeftX\":368,\"areaTopLeftY\":872,\"areaTopRightX\":737,\"areaTopRightY\":872,\"buildingWallId\":66,\"createTime\":{\"date\":17,\"day\":1,\"hours\":14,\"minutes\":56,\"month\":5,\"nanos\":0,\"seconds\":30,\"time\":1560754590000,\"timezoneOffset\":-480,\"year\":119},\"description\":\"\",\"id\":338,\"name\":\"\",\"originalImageFileId\":967,\"positionX\":0,\"positionY\":0,\"resultImageFileId\":1998,\"selectedImageFileId\":\"\",\"sysUserId\":\"\"}}";

                JObject jResult = JObject.Parse(Response);
                JObject jData = (JObject)jResult["data"];
                BuildingWallImage result = jData.ToObject<BuildingWallImage>();
                Response = Sender.Get("http://dev.strosoft.com:6090/BSS/api/Service?service_name=GetTable&data={TableName:\"sys_upload_file\",Condition:\"id =" + result.resultImageFileId + "\"}");
                jResult = JObject.Parse(Response);
                jData = (JObject)jResult["data"][0];
                info.BuildingImg = "http://dev.strosoft.com:6090/BSS/" + jData.ToObject<UploadResult>().path;
                Response = Sender.Get("http://dev.strosoft.com:6090/BSS/api/Service?service_name=GetTable&data={TableName:\"sys_upload_file\",Condition:\"id =" + result.adjustedImageFileId + "\"}");
                jResult = JObject.Parse(Response);
                jData = (JObject)jResult["data"][0];
                info.BuildingadjustImg = "http://dev.strosoft.com:6090/BSS/" + jData.ToObject<UploadResult>().path;
                Response = Sender.Get("http://dev.strosoft.com:6090/BSS/api/Service?service_name=GetViewBuildingWallBlockList&data={buildingWallImageId:" + id[i] + ",pageSize:200}");
                jResult = JObject.Parse(Response);
                JArray jarray = (JArray)jResult["data"]["dataList"];
                List<GetBuildingBlock> block = jarray.ToObject<List<GetBuildingBlock>>();
                
                GetBlockList blocks = new GetBlockList();
                for (int j = 0; j < block.Count(); j++)
                {
                    GetBlockValue Blockvalue = new GetBlockValue();
                    BuildingWallBlock blo = new BuildingWallBlock();
                    Response = Sender.Get("http://dev.strosoft.com:6090/BSS/api/Service?service_name=GetBuildingWallBlockValueList&data={buildingWallBlockId:" + block[j].id + ",pageSize:200}");//block[i].id
                    //Console.WriteLine(Response);
                    jResult = JObject.Parse(Response);
                    jData = (JObject)jResult["data"]["dataList"][0];
                    BlockValueSQL bs = jData.ToObject<BlockValueSQL>();
                    Blockvalue.id = bs.id;
                    Blockvalue.buildingWallBlockId = bs.building_wall_block_id;
                    Blockvalue.buildingWallBlockParameterId = bs.building_wall_block_parameter_id;
                    bs.value = bs.value.Replace(" ", "+");
                    Blockvalue.value = Encoding.UTF8.GetString(Convert.FromBase64String(bs.value));
                    blo.adjustedLocationTopLeftX = block[j].adjusted_location_top_left_x;
                    blo.adjustedLocationTopLeftY = block[j].adjusted_location_top_left_y;
                    blo.blockLocationTopLeftX = block[j].block_location_top_left_x;
                    blo.blockLocationTopLeftY = block[j].block_location_top_left_y;
                    blo.buildingWallBlockNo = block[j].building_wall_block_no;
                    blo.buildingWallImageId = block[j].building_wall_image_id;
                    blo.buildingWallId = block[j].building_wall_id;
                    blo.id = block[j].id;
                    blo.height = block[j].height;
                    blo.width = block[j].width;
                    bl.Add(blo);
                    bv.Add(Blockvalue);
                }
                info.ImgInfo = bl;
                info.blockvalue = bv;
                WebClient download = new WebClient();
                string time = DateTime.Now.ToString("yyyyMMddhhmmssff") + ".jpg";
                string savepath = path + time;
                string returnpath = path2 + time;
                download.DownloadFile(info.BuildingImg, savepath);
                 time = DateTime.Now.ToString("yyyyMMddhhmmssff") + ".jpg";
                string savepath2 = path + time;
                string returnpath2 = path2 + time;
                download.DownloadFile(info.BuildingadjustImg, savepath);
                info.BuildingImg = returnpath;
                info.BuildingadjustImg = returnpath2;
                img.Add(info);
                
            }
            js = JsonConvert.SerializeObject(img);
            //Console.WriteLine(js);
            return js;
            
        }
        //立面拼装信息
        public static int UpdateBuidlingAssemble(AssembleInfo info)
        {
            string data = JsonConvert.SerializeObject(info);
            string Response = Sender.Post("http://dev.strosoft.com:6090/BSS/api/Service?service_name=UpdateBuildingWallImage&data="+data, "");
            Response = Sender.Get("http://dev.strosoft.com:6090/BSS/api/Service?service_name=GetBuildingWallImage&data={id:" + info.id + "}");
            JObject jResult = JObject.Parse(Response);
            int buildingWallId = (int)jResult["data"]["buildingWallId"];
            return buildingWallId;
        }
        public static string UpdateBuildingWallAssemble(int buildingWallId,string code,string img)
        {
            int adjustid = UploadFile.Upload(img);
            code = Convert.ToBase64String(Encoding.UTF8.GetBytes(code));
            string Response = Sender.Post("http://dev.strosoft.com:6090/BSS/api/Service?service_name=UpdateBuildingWall&data={id:"+buildingWallId+ ",wallTogetherCode:\"" + code+ "\",adjustedImageFileId:"+adjustid+"}", "");
            Response = Sender.Get("http://dev.strosoft.com:6090/BSS/api/Service?service_name=GetTable&data={TableName:\"sys_upload_file\",Condition:\"id ="+adjustid+"\"}");
            JObject jResult = JObject.Parse(Response);
            string path = "http://dev.strosoft.com:6090/BSS/" + (string)jResult["data"][0]["path"];
            Dictionary<string, object> myDictionary = new Dictionary<string, object>();
            myDictionary.Add("adjustid",adjustid);
            myDictionary.Add("path",path);
            return JsonConvert.SerializeObject(myDictionary);
        }
        //获取拼装信息
        public static string GetAssembleInfo(int[] id)
        {
            string save = ConfigurationSettings.AppSettings["Savepath"];
            string resp = ConfigurationSettings.AppSettings["Returnpath"];
            Assembles assemble = new Assembles();
            List<AssembleInfo> info = new List<AssembleInfo>();
            List<Assembles> semble = new List<Assembles>();
            int buildingWallId = 0;
            int adjustImg = 0;
            int reslutImg = 0;
            string Response = "";
            string path, path2;
            WebClient download = new WebClient();
            for (int i = 0; i < id.Length; i++)
            {
                
                //Response = Sender.Get("http://dev.strosoft.com:6090/BSS/api/Service?service_name=GetBuildingWallImageList&data={adjustedImageFileId:"+id[i]+"}");
                //JObject jResult = JObject.Parse(Response);
                //JObject jData = (JObject)jResult["data"];
                //id[i]= (int)jResult["data"]["dataList"][0]["id"];
                int[] ids = {id[i] };
                Response = Sender.Get("http://dev.strosoft.com:6090/BSS/api/Service?service_name=GetBuildingWallImage&data={id:" + id[i] + "}");
                //Response = "{ \"success\":true,\"data\":{ \"adjustedImageFileId\":1879,\"areaBottomLeftX\":\"\",\"areaBottomLeftY\":\"\",\"areaBottomRightX\":\"\",\"areaBottomRightY\":\"\",\"areaTopLeftX\":\"\",\"areaTopLeftY\":\"\",\"areaTopRightX\":\"\",\"areaTopRightY\":\"\",\"buildingWallId\":69,\"createTime\":{ \"date\":30,\"day\":4,\"hours\":10,\"minutes\":11,\"month\":4,\"nanos\":0,\"seconds\":11,\"time\":1559182271000,\"timezoneOffset\":-480,\"year\":119},\"description\":\"234\",\"height\":96,\"id\":1,\"name\":\"信息楼\",\"originalImageFileId\":491,\"positionX\":99,\"positionY\":98,\"resultImageFileId\":69,\"selectedImageFileId\":\"\",\"sysUserId\":139,\"width\":97},\"adminRootUrl\":\"http://localhost:8080/BSS/\"}";
                JObject jResult = JObject.Parse(Response);
                JObject jData = (JObject)jResult["data"];
                buildingWallId = (int)jResult["data"]["buildingWallId"];
                adjustImg = (int)jResult["data"]["adjustedImageFileId"];
                reslutImg = (int)jResult["data"]["resultImageFileId"];
                AssembleInfo result = jData.ToObject<AssembleInfo>();
                Response = Sender.Get("http://dev.strosoft.com:6090/BSS/api/Service?service_name=GetTable&data={TableName:\"sys_upload_file\",Condition:\"id =" + adjustImg + "\"}");
                jResult = JObject.Parse(Response);
                path = "http://dev.strosoft.com:6090/BSS/"+(string)jResult["data"][0]["path"];
                Response = Sender.Get("http://dev.strosoft.com:6090/BSS/api/Service?service_name=GetTable&data={TableName:\"sys_upload_file\",Condition:\"id =" + reslutImg + "\"}");
                jResult = JObject.Parse(Response);
                path2 = "http://dev.strosoft.com:6090/BSS/"+(string)jResult["data"][0]["path"];
                string time= DateTime.Now.ToString("yyyyMMddhhmmssff") + ".jpg";
                download.DownloadFile(path, save+time);
                result.adjustImg = resp + time;
                time = DateTime.Now.ToString("yyyyMMddhhmmssff") + ".jpg";
                download.DownloadFile(path2, save + time);
                result.resultImg = resp + time;
                string getblock = GetBuildingImgInfo(ids);
                 JArray array = JArray.Parse(getblock);
                 JArray jarray = (JArray)array[0]["ImgInfo"];
                result.ImgInfo = jarray.ToObject<List<BuildingWallBlock>>();
                jarray = (JArray)array[0]["blockvalue"];
                result.blockvalue = jarray.ToObject<List<GetBlockValue>>();
                info.Add(result);
            }
            assemble.AssembleInfos = info;
            Response = Sender.Get("http://dev.strosoft.com:6090/BSS/api/Service?service_name=GetBuildingWall&data={id:" + buildingWallId + "}");
            string code= (string)JObject.Parse(Response)["data"]["wallTogetherCode"];
            code=code.Replace(" ","+");
            assemble.wall_together_code = Encoding.UTF8.GetString(Convert.FromBase64String(code));
            string js = JsonConvert.SerializeObject(assemble);
            Console.WriteLine(js);
            return js;
        }
        public static string GetAssembleImgInfo(int id)
        {
            string Response = Sender.Get("http://dev.strosoft.com:6090/BSS/api/Service?service_name=GetBuildingWallList&data={adjustedImageFileId:"+id+"}");
            JObject jResult = JObject.Parse(Response);
            JObject jData = (JObject)jResult["data"]["dataList"][0];
            GetBuildingWall building = jData.ToObject<GetBuildingWall>();
            string code = building.wall_together_code.Replace(" ", "+");
            code = Encoding.UTF8.GetString(Convert.FromBase64String(code));
            int[] ids = JObject.Parse(code)["buildingWallImageId"].ToObject<int[]>();
            string result=GetAssembleInfo(ids);
            Console.WriteLine(code);
            return result;
        }
    }
}
 