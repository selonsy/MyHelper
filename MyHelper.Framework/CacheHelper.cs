﻿// <summary>  
// Copyright：Sichen International Co. Ltd.
// Author：Devin
// Date：2016-10-25
// Modifyed：selonsy  
// ModifyTime：2016-10-25  
// Desc：
// 本地缓存帮助类
// </summary> 

using System;
using System.Configuration;
using System.Web;
using System.Web.Caching;

namespace Devin
{
    /// <summary>
    /// 本地缓存帮助类
    /// </summary>
    public class CacheHelper
    {
        /// <summary>
        /// 本地缓存获取
        /// </summary>
        /// <param name="name">key</param>
        /// <returns></returns>
        public static object Get(string name)
        {
            return HttpRuntime.Cache.Get(name);
        }

        /// <summary>
        /// 本地缓存移除
        /// </summary>
        /// <param name="name">key</param>
        public static void Remove(string name)
        {
            if (HttpRuntime.Cache[name] != null)
                HttpRuntime.Cache.Remove(name);
        }

        /// <summary>
        /// 本地缓存写入（默认缓存20min）
        /// </summary>
        /// <param name="name">key</param>
        /// <param name="value">value</param>
        public static void Set(string name, object value)
        {
            Set(name, value, null);
        }

        /// <summary>
        /// 本地缓存写入（默认缓存20min）,依赖项
        /// </summary>
        /// <param name="name">key</param>
        /// <param name="value">value</param>
        /// <param name="cacheDependency">依赖项</param>
        public static void Set(string name, object value, CacheDependency cacheDependency)
        {
            HttpRuntime.Cache.Insert(name, value, cacheDependency, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(20 * 60));
        }

        /// <summary>
        /// 本地缓存写入
        /// </summary>
        /// <param name="name">key</param>
        /// <param name="value">value</param>
        /// <param name="seconds">缓存秒数</param>
        public static void Set(string name, object value, int seconds)
        {
            HttpRuntime.Cache.Insert(name, value, null, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(seconds));
        }

        /// <summary>
        /// 本地缓存写入，包括分钟，是否绝对过期及缓存过期的回调
        /// </summary>
        /// <param name="name">key</param>
        /// <param name="value">value</param>
        /// <param name="seconds">缓存秒数</param>
        /// <param name="isAbsoluteExpiration">是否绝对过期</param>
        /// <param name="onRemoveCallback">缓存过期回调</param>
        public static void Set(string name, object value, int seconds, bool isAbsoluteExpiration, CacheItemRemovedCallback onRemoveCallback)
        {
            if (isAbsoluteExpiration)
                HttpRuntime.Cache.Insert(name, value, null, DateTime.Now.AddSeconds(seconds), Cache.NoSlidingExpiration, CacheItemPriority.Normal, onRemoveCallback);
            else
                HttpRuntime.Cache.Insert(name, value, null, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(seconds), CacheItemPriority.Normal, onRemoveCallback);
        }
    }
}
