using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenCVAPP.Model;

namespace OpenCVAPP.OpenCV
{
    public class GetInfo
    {
        public static string AddBuildingWall(string jsons)//添加立面信息
        {
            BuildingWall data = JsonConvert.DeserializeObject<BuildingWall>(jsons);
            string result = HTTPService.AddBuildingWall(data);
            return result;
        }
        public static string AddBuildingWallImg(string jsons)//添加立面图片信息
        {
            BuildingWallImage data = JsonConvert.DeserializeObject<BuildingWallImage>(jsons);
            string result = HTTPService.AddWallImage(data);
            return result;
        }
        public static string AddBuildingWallBlock(string jsons)//添加幕墙板块信息
        {
            int count = 0;
            try
            {
                string path = ConfigurationSettings.AppSettings["Savepath"];
                AddBuildingBlock data = JsonConvert.DeserializeObject<AddBuildingBlock>(jsons);
                BuildingWallBlock block = new BuildingWallBlock();
                string Response = "";
                for (int i = 0; i < data.blocks.Count(); i++)
                {
                    block.adjustedLocationTopLeftX = data.blocks[i].adjustedLocationTopLeftX;
                    block.adjustedLocationTopLeftY = data.blocks[i].adjustedLocationTopLeftY;
                    block.blockLocationTopLeftX = data.blocks[i].blockLocationTopLeftX;
                    block.blockLocationTopLeftY = data.blocks[i].blockLocationTopLeftY;
                    block.buildingWallBlockNo = data.blocks[i].buildingWallBlockNo;
                    block.buildingWallId = data.buildingWallId;
                    block.buildingWallImageId = data.buildingWallImageId;
                    block.width = data.blocks[i].width;
                    block.height = data.blocks[i].height;
                    if (data.blocks[i].name == null) data.blocks[i].name = data.blocks[i].value;
                    Response = HTTPService.AddWallBlock(block, data.blocks[i].name, data.blocks[i].value);
                    count++;
                }
                string savepath = path + DateTime.Now.ToString("yyyyMMddhhmmssff") + ".jpg";
                Base64Convert.Base64ToFileAndSave(data.resultImageFile, savepath);//文件流保存到文件
                int resultImageFileId = UploadFile.Upload(savepath);
                File.Delete(savepath);
                string result = HTTPService.UpdateWallimg(data.buildingWallImageId, resultImageFileId);
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(count);
                Console.WriteLine(ex.Message + "," + ex.TargetSite + "," + ex.Source);
                return ex.Message + "," + ex.TargetSite+","+ex.Source;
                
            }

        }
        public static string GetBuildingImg(int id)//获取需要修改的信息
        {
            string result = HTTPService.GetUpBuildingInfo(id);
            return result;
        }
        public static string UpdateBuildingBlock(string jsons)//更新板块信息
        {
            UpdateBlock block = JsonConvert.DeserializeObject<UpdateBlock>(jsons);
            string result = "";
            for (int i = 0; i < block.blocks.Count(); i++)
            {
                result = HTTPService.UpdateWallBlock(block.blocks[i], block.blockvalue[i]);
            }
            return result;
        }
        public static string GetBuidlingWallImgInfo(string id)//获取立面图片信息
        {
            int[] ID = JsonConvert.DeserializeObject<int[]>(id);

            string result = HTTPService.GetBuildingImgInfo(ID);
            return result;
        }
        public static string UpdateBuildingAssemble(string jsons,string code,string img)//立面拼装信息
        {
            string save = ConfigurationSettings.AppSettings["Savepath"];
            save+= DateTime.Now.ToString("yyyyMMddhhmmssff") + ".jpg";
            Base64Convert.Base64ToFileAndSave(img,save);
            JObject jResult = JObject.Parse(jsons);
            JArray jData = (JArray)jResult["data"];
            List<AssembleInfo> results = jData.ToObject<List<AssembleInfo>>();
            Console.WriteLine(code);
            string result = "";
            int buildingWallId = 0;
            for (int i = 0; i < results.Count(); i++)
            {
                buildingWallId = HTTPService.UpdateBuidlingAssemble(results[i]);
            }
            result = HTTPService.UpdateBuildingWallAssemble(buildingWallId,code,save);
            Console.WriteLine(result);
            return result;
        }
        public static string GetAssembleInfo(string id)//获取拼装的信息
        {
            int[] ID = JsonConvert.DeserializeObject<int[]>(id);
            string result = HTTPService.GetAssembleInfo(ID);
            return result;
        }
        public static string GetAssembleImgInfo(int id)//获取拼装信息
        {
            string result = HTTPService.GetAssembleImgInfo(id);
            return result;
        }
    }
}
