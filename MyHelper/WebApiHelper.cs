// <summary>  
// Copyright：Sichen International Co. Ltd.
// Author：Devin
// Date：2016-10-25
// Modifyed：selonsy  
// ModifyTime：2016-10-25  
// Desc：
// WebApi帮助类
// </summary> 

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Devin
{
    /// <summary>
    /// WebApi帮助类
    /// </summary>
    public class WebApiHelper
    {
        /// <summary>
        /// Post请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="apiurl">WebApi的Url地址(不带参数)</param>
        /// <param name="tokenurl">WebApi的请求Token的Url地址(不带参数)</param> 
        /// <param name="data">请求参数("keyname"模式,按字典顺序拼接)</param>
        /// <param name="tokenid">请求TokenId,唯一标识符</param>
        /// <returns></returns>
        public static T Post<T>(string apiurl, string tokenurl, string data, int tokenid)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(data);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiurl);

            string timeStamp = GetTimeStamp();      //获取时间戳  
            string nonce = GetRandom();             //获取随机数

            //加入头信息
            request.Headers.Add("tokenid", tokenid.ToString());     //当前请求ID
            request.Headers.Add("timestamp", timeStamp);            //发起请求时的时间戳（单位：毫秒）
            request.Headers.Add("nonce", nonce);                    //发起请求时的随机数
            request.Headers.Add("signature", GetSignature(tokenurl, timeStamp, nonce, tokenid, data)); //当前请求内容的数字签名

            //写数据
            request.Method = "POST";
            request.ContentLength = bytes.Length;
            request.ContentType = "application/json";
            request.Timeout = 300000;
            request.Headers.Set("Pragma", "no-cache");
            Stream reqstream = request.GetRequestStream();
            reqstream.Write(bytes, 0, bytes.Length);
            
            //读数据            
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream streamReceive = response.GetResponseStream();
            StreamReader streamReader = new StreamReader(streamReceive, Encoding.UTF8);
            string strResult = streamReader.ReadToEnd();

            //关闭流
            reqstream.Close();
            streamReader.Close();
            streamReceive.Close();
            request.Abort();
            response.Close();

            HttpResponseMsg resultObj = JsonConvert.DeserializeObject<HttpResponseMsg>(strResult);
            return JsonConvert.DeserializeObject<T>(resultObj.data.ToString());
        }

        /// <summary>
        /// Get请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="apiurl">WebApi的Url地址(不带参数)</param>        
        /// <param name="tokenurl">WebApi的请求Token的Url地址(不带参数)</param>  
        /// <param name="query">请求参数("keyname"模式,按字典顺序拼接)</param>
        /// <param name="queryStr">请求参数("key=name"模式,按字典顺序拼接)</param>
        /// <param name="tokenid">请求TokenId,唯一标识符</param>
        /// <param name="sign">是否启动签名验证</param>
        /// <returns></returns>
        public static T Get<T>(string apiurl, string tokenurl, string query, string queryStr, int tokenid, bool sign = true)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiurl + "?" + queryStr);
            string timeStamp = GetTimeStamp();  //获取时间戳
            string nonce = GetRandom();         //获取随机数

            //加入头信息
            request.Headers.Add("tokenid", tokenid.ToString());     //当前请求ID
            request.Headers.Add("timestamp", timeStamp);            //发起请求时的时间戳（单位：毫秒）
            request.Headers.Add("nonce", nonce);                    //发起请求时的时间戳（单位：毫秒）

            //是否启用签名
            if (sign)
            {
                request.Headers.Add("signature", GetSignature(tokenurl, timeStamp, nonce, tokenid, query));   //当前请求内容的数字签名
            }

            request.Method = "GET";
            request.ContentType = "application/json";
            request.Timeout = 90000;
            request.Headers.Set("Pragma", "no-cache");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream streamReceive = response.GetResponseStream();
            StreamReader streamReader = new StreamReader(streamReceive, Encoding.UTF8);
            string strResult = streamReader.ReadToEnd();

            streamReader.Close();
            streamReceive.Close();
            request.Abort();
            response.Close();

            HttpResponseMsg resultObj = JsonConvert.DeserializeObject<HttpResponseMsg>(strResult);
            return JsonConvert.DeserializeObject<T>(resultObj.data.ToString());
        }

        /// <summary>
        /// 获取token
        /// </summary>
        /// <param name="apiurl">WebApi的Url地址(不带参数)</param>
        /// <param name="tokenid">请求TokenId,唯一标识符</param>
        /// <returns></returns>
        public static TokenEntity GetSignToken(string apiurl, int tokenid)
        {
            Dictionary<string, string> parames = new Dictionary<string, string>();
            parames.Add("tokenid", tokenid.ToString());
            Tuple<string, string> parameters = GetQueryString(parames);
            TokenEntity token = WebApiHelper.Get<TokenEntity>(apiurl, apiurl, parameters.Item1, parameters.Item2, tokenid, false);
            return token;
        }

        /// <summary>
        /// 拼接get参数
        /// </summary>
        /// <param name="parames"></param>
        /// <returns></returns>
        public static Tuple<string, string> GetQueryString(Dictionary<string, string> parames)
        {
            //第一步：把字典按Key的字母顺序排序
            IDictionary<string, string> sortedParams = new SortedDictionary<string, string>(parames);
            IEnumerator<KeyValuePair<string, string>> dem = sortedParams.GetEnumerator();

            //第二步：把所有参数名和参数值串在一起
            StringBuilder query = new StringBuilder("");    //签名字符串
            StringBuilder queryStr = new StringBuilder(""); //url参数
            if (parames == null || parames.Count == 0)
                return new Tuple<string, string>("", "");

            while (dem.MoveNext())
            {
                string key = dem.Current.Key;
                string value = dem.Current.Value;
                if (!string.IsNullOrEmpty(key))
                {
                    query.Append(key).Append(value);
                    queryStr.Append("&").Append(key).Append("=").Append(value);
                }
            }

            return new Tuple<string, string>(query.ToString(), queryStr.ToString().Substring(1, queryStr.Length - 1));
        }

        #region private

        /// <summary>
        /// 计算签名
        /// </summary>
        /// <param name="tokenurl">WebApi的请求Token的Url地址(不带参数)</param>
        /// <param name="timestamp">时间戳</param>
        /// <param name="nonce">随机数</param>
        /// <param name="tokenid">请求TokenId,唯一标识符</param>
        /// <param name="data">请求参数("keyname"模式,按字典顺序拼接)</param>
        /// <returns></returns>
        private static string GetSignature(string tokenurl, string timestamp, string nonce, int tokenid, string data)
        {
            TokenEntity token = null;
            token = GetSignToken(tokenurl, tokenid);
            if (token == null)
            {
                throw new Exception("token为null,请求ID为：" + tokenid);
            }
            var hash = System.Security.Cryptography.MD5.Create();
            //拼接签名数据
            var signStr = timestamp + nonce + tokenid + token.TokenValue + data;
            //将字符串中字符按升序排序
            var sortStr = string.Concat(signStr.OrderBy(c => c));
            var bytes = Encoding.UTF8.GetBytes(sortStr);
            //使用MD5加密
            var md5Val = hash.ComputeHash(bytes);
            //把二进制转化为大写的十六进制
            StringBuilder result = new StringBuilder();
            foreach (var c in md5Val)
            {
                result.Append(c.ToString("X2"));
            }
            return result.ToString().ToUpper();
        }

        /// <summary>  
        /// 获取时间戳  
        /// </summary>  
        /// <returns></returns>  
        private static string GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalMilliseconds).ToString();
        }

        /// <summary>  
        /// 获取随机数
        /// </summary>  
        /// <returns></returns>  
        private static string GetRandom()
        {
            Random rd = new Random(DateTime.Now.Millisecond);
            int i = rd.Next(0, int.MaxValue);
            return i.ToString();
        }

        #endregion

        /// <summary>
        /// 返回类
        /// </summary>
        public class HttpResponseMsg
        {
            /// <summary>
            /// 状态码
            /// </summary>
            public int code { get; set; }
            /// <summary>
            /// 操作信息
            /// </summary>
            public string msg { get; set; }
            /// <summary>
            /// 返回数据
            /// </summary>
            public object data { get; set; }

        }

        /// <summary>
        /// Token实体类
        /// </summary>
        public class TokenEntity
        {

            /// <summary>
            /// TokenId
            /// </summary>
            public int TokenId { get; set; }

            /// <summary>
            /// TokenValue
            /// </summary>
            public string TokenValue { get; set; }

            /// <summary>
            /// ExpireTime
            /// </summary>
            public DateTime ExpireTime { get; set; }

        }

        /// <summary>
        /// 返回码类
        /// </summary>
        public enum StatusCode
        {
            Success = 200,          //请求(或处理)成功
            Error = 500,            //内部请求出错
            Unauthorized = 401,     //未授权标识
            ParameterError = 400,   //请求参数不完整或不正确
            TokenInvalid = 403,     //请求TOKEN失效
            HttpMehtodError = 405,  //HTTP请求类型不合法
            HttpRequestError = 406, //HTTP请求不合法,请求参数可能被篡改
            URLExpireError = 407,   //该URL已经失效
        }
    }
}
