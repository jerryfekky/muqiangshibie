using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using OpenCVAPP.Model;

namespace OpenCVAPP.OpenCV
{
    public class UploadFile
    {
        public static int Upload(string filename)
        {
            string url = ConfigurationSettings.AppSettings["ServiceUrl"];
            //string url = "http://dev.strosoft.com:6090/BSS/api/Service?serviceName=UploadFile";
            string result = HttpHelper.HttpUploadFile(url,filename);
            JObject jResult = JObject.Parse(result);
            //bool success = (bool)jResult["success"];
            //if (!success)
            //{
            //    string errorMessage = (string)jResult["error_message"];
            //    throw new Exception(errorMessage);
            //}
            //if (jResult["data"] == null)
            //{
            //    return null;
            //}
            //Console.WriteLine(result);
            JObject jData = (JObject)jResult["data"];
            UploadResult res = jData.ToObject<UploadResult>();
            return res.id;

        }
    }
}
