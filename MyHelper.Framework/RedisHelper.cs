// <summary>  
// Copyright：Sichen International Co. Ltd.
// Author：Devin
// Date：2017-1-21
// Modifyed：selonsy  
// ModifyTime：2017-1-21  
// Desc：
// Redis帮助类
// </summary> 
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.ComponentModel;
using System.Reflection;

namespace Devin
{
    #region 操作示例

    /*
         1.向redis服务器第一个库的fd1目录里，添加一个键为name，值为wangjie的记录：
         RedisHelper.StringSet(CacheFolderEnum.Folder1, "name", "wangjie");

         2.获取这条记录：
         string key = RedisHelper.StringGet(CacheFolderEnum.Folder1, "name");
         Console.WriteLine("键为name的记录对应的值：" + key);

         3.删除这条记录：
         bool result = RedisHelper.StringRemove(CacheFolderEnum.Folder1, "name");
         if (result)
         {
            Console.WriteLine("键为name的记录删除成功");
         }
         else
         {
            Console.WriteLine("键为name的记录删除失败");
         } 

         4.查询这条记录是否存在：
         bool ifExist = RedisHelper.KeyExists(CacheFolderEnum.Folder1, "name");
         if (ifExist)
         {
            Console.WriteLine("键为name的记录存在");
         }
         else
         {
            Console.WriteLine("键为name的记录不存在");
         }

         5.向redis服务器第二个库的fd2目录里，添加一个键为sd1，值为一个对象的记录：
         Student student = new Student() { Id = 1, Name = "张三", Class = "三年二班" };
         RedisHelper.Set<Student>(CacheFolderEnum.Folder2, "sd1", student, 10, 1);

         6.获取这个对象：
         Student sdGet = RedisHelper.Get<Student>(CacheFolderEnum.Folder2, "sd1", 1);
         if (sdGet != null)
         {
            Console.WriteLine("Id：" + sdGet.Id + " Name：" + sdGet.Name + " Class：" + sdGet.Class);
         }
         else
         {
            Console.WriteLine("找不到键为sd1的记录");
         }
    */

    #endregion

    /// <summary>
    /// Redis帮助类
    /// </summary>
    public static class RedisHelper
    {
        private static string _conn = Config.DBRedisStr;
        private static string _pwd = Config.DBRedisPwd;

        static ConnectionMultiplexer _redis;
        static readonly object _locker = new object();

        #region 单例模式
        public static ConnectionMultiplexer Manager
        {
            get
            {
                if (_redis == null)
                {
                    lock (_locker)
                    {
                        if (_redis != null) return _redis;

                        _redis = GetManager();
                        return _redis;
                    }
                }
                return _redis;
            }
        }

        private static ConnectionMultiplexer GetManager(string connectionString = null)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = _conn;
            }
            var options = ConfigurationOptions.Parse(connectionString);
            options.Password = _pwd;
            return ConnectionMultiplexer.Connect(options);
        }
        #endregion

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="folder">目录</param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expireMinutes">过期时间，单位：分钟。默认600分钟</param>
        /// <param name="db">库，默认第一个。0~15共16个库</param>
        /// <returns></returns>
        public static bool StringSet(CacheFolderEnum folder, string key, string value, int expireMinutes = 600, int db = -1)
        {
            string fd = GetDescription(folder);
            return Manager.GetDatabase(db).StringSet(string.IsNullOrEmpty(fd) ? key : fd + ":" + key, value, TimeSpan.FromMinutes(expireMinutes));
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="folder">目录</param>
        /// <param name="key">键</param>
        /// <param name="db">库，默认第一个。0~15共16个库</param>
        /// <returns></returns>
        public static string StringGet(CacheFolderEnum folder, string key, int db = -1)
        {
            try
            {
                string fd = GetDescription(folder);
                return Manager.GetDatabase(db).StringGet(string.IsNullOrEmpty(fd) ? key : fd + ":" + key);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="folder">目录</param>
        /// <param name="key">键</param>
        /// <param name="db">库，默认第一个。0~15共16个库</param>
        /// <returns></returns>
        public static bool StringRemove(CacheFolderEnum folder, string key, int db = -1)
        {
            try
            {
                string fd = GetDescription(folder);
                return Manager.GetDatabase(db).KeyDelete(string.IsNullOrEmpty(fd) ? key : fd + ":" + key);
            }
            catch (Exception)
            {
                return false;
            }
        }
        
        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="db">库，默认第一个。0~15共16个库</param>
        public static bool KeyExists(CacheFolderEnum folder, string key, int db = -1)
        {
            try
            {
                string fd = GetDescription(folder);
                return Manager.GetDatabase(db).KeyExists(string.IsNullOrEmpty(fd) ? key : fd + ":" + key);
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 延期
        /// </summary>
        /// <param name="folder">目录</param>
        /// <param name="key">键</param>
        /// <param name="min">延长时间，单位：分钟，默认600分钟</param>
        /// <param name="db">库，默认第一个。0~15共16个库</param>
        public static bool AddExpire(CacheFolderEnum folder, string key, int min = 600, int db = -1)
        {
            try
            {
                string fd = GetDescription(folder);
                return Manager.GetDatabase(db).KeyExpire(string.IsNullOrEmpty(fd) ? key : fd + ":" + key, DateTime.Now.AddMinutes(min));
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="folder">目录</param>
        /// <param name="key">键</param>
        /// <param name="t">实体</param>
        /// <param name="ts">延长时间，单位：分钟，默认600分钟</param>
        /// <param name="db">库，默认第一个。0~15共16个库</param>
        public static bool Set<T>(CacheFolderEnum folder, string key, T t, int expireMinutes = 600, int db = -1)
        {
            string fd = GetDescription(folder);
            var str = JsonConvert.SerializeObject(t);
            return Manager.GetDatabase(db).StringSet(string.IsNullOrEmpty(fd) ? key : fd + ":" + key, str, TimeSpan.FromMinutes(expireMinutes));
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="folder">目录</param>
        /// <param name="key">键</param>
        /// <param name="db">库，默认第一个。0~15共16个库</param>
        public static T Get<T>(CacheFolderEnum folder, string key, int db = -1) where T : class
        {
            string fd = GetDescription(folder);
            var strValue = Manager.GetDatabase(db).StringGet(string.IsNullOrEmpty(fd) ? key : fd + ":" + key);
            return string.IsNullOrEmpty(strValue) ? null : JsonConvert.DeserializeObject<T>(strValue);
        }

        /// <summary>
        /// 获得枚举的Description
        /// </summary>
        /// <param name="value">枚举值</param>
        /// <param name="nameInstead">当枚举值没有定义DescriptionAttribute，是否使用枚举名代替，默认是使用</param>
        /// <returns>枚举的Description</returns>
        private static string GetDescription(this Enum value, Boolean nameInstead = true)
        {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);
            if (name == null)
            {
                return null;
            }

            FieldInfo field = type.GetField(name);

            DescriptionAttribute attribute = field.GetCustomAttribute<DescriptionAttribute>();

            //DescriptionAttribute attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;

            if (attribute == null && nameInstead == true)
            {
                return name;
            }
            return attribute == null ? null : attribute.Description;
        }
    }

    /// <summary>
    /// Redis缓存文件夹
    /// </summary>
    public enum CacheFolderEnum
    {
        /// <summary>
        /// 根目录
        /// </summary>
        [Description("")]
        Root = 0,

        /// <summary>
        /// 测试目录1
        /// </summary>
        [Description("fd1")]
        Folder1 = 1,

        /// <summary>
        /// 测试目录2
        /// </summary>
        [Description("fd2")]
        Folder2 = 2
    }
}