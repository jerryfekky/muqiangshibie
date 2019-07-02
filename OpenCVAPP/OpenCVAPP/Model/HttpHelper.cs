using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OpenCVAPP.Model
{
    public class HttpHelper
    {
        /// <summary>
        /// Http上传文件
        /// </summary>
        public static string HttpUploadFile(string url, string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                // 设置参数
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                CookieContainer cookieContainer = new CookieContainer();
                request.CookieContainer = cookieContainer;
                request.AllowAutoRedirect = true;
                request.AllowWriteStreamBuffering = false;
                request.SendChunked = true;
                request.Method = "POST";
                request.Timeout = 300000;

                string boundary = DateTime.Now.Ticks.ToString("X"); // 随机分隔线
                request.ContentType = "multipart/form-data;charset=utf-8;boundary=" + boundary;
                byte[] itemBoundaryBytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "\r\n");
                byte[] endBoundaryBytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");
                int pos = path.LastIndexOf("\\");
                string fileName = path.Substring(pos + 1);

                //请求头部信息
                StringBuilder sbHeader = new StringBuilder(string.Format("Content-Disposition:form-data;name=\"file\";filename=\"{0}\"\r\nContent-Type:application/octet-stream\r\n\r\n", fileName));
                byte[] postHeaderBytes = Encoding.UTF8.GetBytes(sbHeader.ToString());
                request.ContentLength = itemBoundaryBytes.Length + postHeaderBytes.Length + fs.Length + endBoundaryBytes.Length;
                using (Stream postStream = request.GetRequestStream())
                {
                    postStream.Write(itemBoundaryBytes, 0, itemBoundaryBytes.Length);
                    postStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);
                    int bytesRead = 0;

                    int arrayLeng = fs.Length <= 4096 ? (int)fs.Length : 4096;
                    byte[] bArr = new byte[arrayLeng];
                    int counter = 0;
                    while ((bytesRead = fs.Read(bArr, 0, arrayLeng)) != 0)
                    {
                        counter++;
                        postStream.Write(bArr, 0, bytesRead);
                    }
                    postStream.Write(endBoundaryBytes, 0, endBoundaryBytes.Length);
                }

                //发送请求并获取相应回应数据
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    //直到request.GetResponse()程序才开始向目标网页发送Post请求
                    using (Stream instream = response.GetResponseStream())
                    {
                        StreamReader sr = new StreamReader(instream, Encoding.UTF8);
                        //返回结果网页（html）代码
                        string content = sr.ReadToEnd();
                        return content;
                    }
                }
            }
        }
    }
}
