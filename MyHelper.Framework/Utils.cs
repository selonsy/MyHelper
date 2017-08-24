// <summary>  
// Copyright：Sichen International Co. Ltd.
// Author：Devin
// Date：2016-09-01  
// Modifyed：selonsy  
// ModifyTime：2016-09-01  
// Desc：
// 通用方法类
// </summary> 

using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;

namespace Devin
{
    /// <summary>
    /// 通用方法类
    /// </summary>
    public abstract class Utils
    {

        /// <summary>
        /// 数组比较(linq方式)
        /// </summary>
        /// <param name="arr1">第一个数组</param>
        /// <param name="arr2">第二个数组</param>
        /// <returns></returns>
        public static bool compareArr_1(int[] arr1, int[] arr2)
        {
            var q = from a in arr1 join b in arr2 on a equals b select a;
            bool flag = arr1.Length == arr2.Length && q.Count() == arr1.Length;
            return flag;
        }

        /// <summary>
        /// 数组比较(双重for循环方式)
        /// </summary>
        /// <param name="arr1">第一个数组</param>
        /// <param name="arr2">第二个数组</param>
        /// <param name="strict">是否启用严格模式(顺序和值都要求一致)</param>
        /// <returns></returns>
        public static bool compareArr_2(int[] arr1, int[] arr2, bool strict)
        {
            //初始化一个bool数组,初始值全为false;
            bool[] flag = new bool[arr1.Length];
            bool strictFlag = true;
            for (int i = 0; i < arr1.Length; i++)
            {
                if (arr1[i] != arr2[i]) strictFlag = false;
                for (int j = 0; j < arr2.Length; j++)
                {
                    //遇到有相同的值,对应的bool数组的值设为true;
                    if (arr1[i] == arr2[j]) flag[i] = true;
                }
            }
            if (strict) { return strictFlag; }
            foreach (var item in flag)
            {
                //遍历bool数组,还有false,就说明有不同的值,结果返回false
                if (item == false) return false;
            }
            return true;
        }

        /// <summary>
        /// 时间戳转为C#格式时间
        /// </summary>
        /// <param name="timeStamp">Unix时间戳格式</param>
        /// <returns>C#格式时间</returns>
        public static DateTime ConvertInt2DateTime(string timeStamp)
        {
            DateTime dtStart = new DateTime(1970, 1, 1);
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }

        /// <summary>
        /// DateTime时间格式转换为Unix时间戳格式
        /// </summary>
        /// <param name="time"> DateTime时间格式</param>
        /// <returns>Unix时间戳格式</returns>
        public static int ConvertDateTime2Int(DateTime time)
        {
            DateTime startTime = new DateTime(1970, 1, 1);
            return (int)(time - startTime).TotalSeconds;
        }

        /// <summary>
        /// 获取32位长度的GUID
        /// </summary>
        /// <returns></returns>
        public static string GetNewGuid()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }

#if NETFRAMEWORK
        /// <summary>
        /// 查看端口是否被占用
        /// </summary>
        /// <param name="port">端口号</param>
        /// <returns></returns>
        public static bool PortInUse(int port)
        {
            bool inUse = false;

            IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] ipEndPoints = ipProperties.GetActiveTcpListeners();

            foreach (IPEndPoint endPoint in ipEndPoints)
            {
                if (endPoint.Port == port)
                {
                    inUse = true;
                    break;
                }
            }
            return inUse;
        }
#endif
    }

    /// <summary>
    /// 图片下载帮助类
    /// </summary>
    public class PictureDownloader
    {
        /// <summary>
        /// 下载图片
        /// </summary>
        /// <param name="picUrl">图片Http地址</param>
        /// <param name="savePath">保存路径(绝对地址)</param>
        /// <param name="timeOut">Request最大请求时间(单位毫秒),如果为-1则无限制,默认10秒钟</param>
        /// <returns></returns>
        public static bool Download(string picUrl, string savePath, int timeOut = 10000)
        {
            bool value = false;
            WebResponse response = null;
            Stream stream = null;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(picUrl);
                if (timeOut != -1) request.Timeout = timeOut;
                response = request.GetResponse();
                stream = response.GetResponseStream();
                if (!response.ContentType.ToLower().StartsWith("text/"))
                    value = SaveBinaryFile(response, savePath);
            }
            finally
            {
                if (stream != null) stream.Close();
                if (response != null) response.Close();
            }
            return value;
        }

        /// <summary>
        /// 保存二进制文件
        /// </summary>
        /// <param name="response"></param>
        /// <param name="savePath"></param>
        /// <returns></returns>
        private static bool SaveBinaryFile(WebResponse response, string savePath)
        {
            bool value = false;
            byte[] buffer = new byte[1024];
            Stream outStream = null;
            Stream inStream = null;
            try
            {
                if (File.Exists(savePath)) File.Delete(savePath);
                outStream = System.IO.File.Create(savePath);
                inStream = response.GetResponseStream();
                int l;
                do
                {
                    l = inStream.Read(buffer, 0, buffer.Length);
                    if (l > 0) outStream.Write(buffer, 0, l);
                } while (l > 0);
                value = true;
            }
            finally
            {
                if (outStream != null) outStream.Close();
                if (inStream != null) inStream.Close();
            }
            return value;
        }
    }
}
