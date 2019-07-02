using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HttpSender;

namespace OpenCVAPP.OpenCV
{
    class Servicetest
    {
        public static void aaa()
        {
            string Response = Sender.Post(UrlEncode("http://dev.strosoft.com:6090/BSS/api/Service?service_name=UpdateBuildingWall&data={id:" + 66 + ",wallTogetherCode:\"" + "啦啦啦啦" + "\"}"), "");
            Console.WriteLine(Response);
        }
        public static string UrlEncode(string url)
        {
            byte[] bs = Encoding.GetEncoding("GBK").GetBytes(url);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < bs.Length; i++)
            {
                if (bs[i] < 128)
                    sb.Append((char)bs[i]);
                else
                {
                    sb.Append("%" + bs[i++].ToString("x").PadLeft(2, '0'));
                    sb.Append("%" + bs[i].ToString("x").PadLeft(2, '0'));
                }
            }
            return sb.ToString();
        }
    }
}
