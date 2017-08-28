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
        #region private

        /// <summary>
        /// 地球半径
        /// </summary>
        private const double EARTH_RADIUS = 6378.137;
        private static double Rad(double d)
        {
            return d * Math.PI / 180.0;
        }

        #endregion

        /// <summary>
        /// 根据经纬度获取两点间距离，单位m
        /// </summary>
        /// <param name="lat1"></param>
        /// <param name="lng1"></param>
        /// <param name="lat2"></param>
        /// <param name="lng2"></param>
        /// <returns></returns>
        public static double GetDistance(double lat1, double lng1, double lat2, double lng2)
        {
            double radLat1 = Rad(lat1);
            double radLat2 = Rad(lat2);
            double a = radLat1 - radLat2;
            double b = Rad(lng1) - Rad(lng2);
            double s = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) + Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2)));
            s = s * EARTH_RADIUS;
            s = Math.Round(s * 10000) / 10;
            return s;
        }

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
}
