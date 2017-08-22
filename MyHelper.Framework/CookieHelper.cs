// <summary>  
// Copyright：Sichen International Co. Ltd.
// Author：Devin
// Date：2016-10-18
// Modifyed：selonsy  
// ModifyTime：2016-10-18  
// Desc：
// Cookie帮助类
// </summary> 

using System;
using System.Web;

namespace Devin
{
    /// <summary>
    /// Cookie帮助类
    /// </summary>
    public class CookieHelper
    {
        /// <summary>
        /// 获取Cookie
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static HttpCookie Get(string name)
        {
            return HttpContext.Current.Request.Cookies[name];
        }

        /// <summary>
        /// 获取Cookie值
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetValue(string name)
        {
            HttpCookie httpCookie = Get(name);
            if (httpCookie != null)
                return httpCookie.Value;
            else
                return string.Empty;
        }

        /// <summary>
        /// 移除Cookie
        /// </summary>
        /// <param name="name"></param>
        public static void Remove(string name)
        {
            Remove(Get(name));
        }

        /// <summary>
        /// 移除Cookie
        /// </summary>
        /// <param name="cookie"></param>
        public static void Remove(HttpCookie cookie)
        {
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddDays(-1);
                cookie.Domain = HttpContext.Current.Request.Url.Host.ToLower();
                Save(cookie);
            }
        }

        /// <summary>
        /// 保存Cookie
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="expiresSeconds"></param>
        public static void Save(string name, string value, int expiresSeconds = 0)
        {
            var httpCookie = Get(name);
            if (httpCookie == null)
                httpCookie = new HttpCookie(name);
            httpCookie.Value = value;
            httpCookie.Domain = HttpContext.Current.Request.Url.Host.ToLower();
            Save(httpCookie, expiresSeconds);
        }

        /// <summary>
        /// 保存Cookie
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="domain"></param>
        /// <param name="expiresSeconds"></param>
        public static void Save(string name, string value, string domain, int expiresSeconds = 0)
        {
            HttpCookie httpCookie = Get(name);
            if (httpCookie == null)
                httpCookie = new HttpCookie(name);
            httpCookie.Value = value;
            if (domain == "")
            {
                httpCookie.Domain = HttpContext.Current.Request.Url.Host.ToLower();
            }
            else
            {
                httpCookie.Domain = domain;
            }
            Save(httpCookie, expiresSeconds);
        }

        /// <summary>
        /// 保存Cookie
        /// </summary>
        /// <param name="cookie"></param>
        /// <param name="expiresSeconds"></param>
        public static void Save(HttpCookie cookie, int expiresSeconds = 0)
        {
            if (expiresSeconds > 0)
                cookie.Expires = DateTime.Now.AddSeconds(expiresSeconds);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// 修改cookie
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static HttpCookie Set(string name, string value)
        {
            HttpCookie httpCookie = Get(name);
            if (httpCookie == null)
            {
                return null;
            }               
            else
            {
                httpCookie.Value = value;
                Save(httpCookie);
            }
            return httpCookie;
        }
    }
}
