// <summary>  
// Copyright：Sichen International Co. Ltd.
// Author：Devin
// Date：2017-3-20
// Modifyed：selonsy  
// ModifyTime：2017-3-20  
// Desc：
// 基类
// </summary> 

//NET452    ,NETFRAMEWORK  :   .NET Framework 4.5.2
//NETSTD14  ,NETSTANDARD   :   .NET Standard  1.4

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

namespace Devin
{
    /// <summary>
    /// 基类
    /// </summary>
    public class Config
    {
        #region private

        /// <summary>
        /// 配置项字典
        /// </summary>
        private static Dictionary<string, string> config_dic;

        /// <summary>
        /// 根据调用者的名称自动的获取配置项
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private static string ConfigDataValue([CallerMemberName] string key = null)
        {
            key = key.ToLower().Replace("_", "");
            if (config_dic == null || config_dic.Count <= 0)
                return string.Empty;
            else if (config_dic.ContainsKey(key))
                return config_dic[key].ToString();
            else
                return string.Empty;
        }

        #endregion

        #region 初始化
        
        /// <summary>
        /// 初始化配置信息
        /// 需要显示的执行或者在应用程序启动时执行,如global.asax 
        /// </summary>
        /// <param name="project_name">项目名称</param>
        /// <param name="config_type">配置文件类型</param>
        /// <param name="config_file_path">指定配置文件绝对路径</param>
        public static void Init_Config(string project_name = "default", int config_type = 3, string config_file_path = "")
        {
            string config_path = string.Format("C:\\webconfig\\{0}\\config.{1}", project_name, ((ET.ConfigType)config_type).ToString().ToLower());
            if (config_file_path.IsNotNullOrEmpty()) config_path = config_file_path;

            if (!File.Exists(config_path)) throw new Exception("配置文件不存在");

            if (config_type == ET.ConfigType.Xml.ToInt())
            {
                config_dic = new XmlConfig().InitConfig(config_path);
            }
            else if (config_type == ET.ConfigType.Ini.ToInt())
            {
                config_dic = new IniConfig().InitConfig(config_path);
            }
            else if (config_type == ET.ConfigType.Json.ToInt())
            {
                config_dic = new JsonConfig().InitConfig(config_path);
            }
            else
            {
                throw new Exception("配置信息参数错误");
            }
        }

        #endregion

        #region 公用属性
        /// <summary>
        /// 项目名称
        /// </summary>
        public static string ProjectName { get { return ConfigDataValue(); } }
        /// <summary>
        /// 是否测试服
        /// </summary>
        public static bool IsDebug { get { return ConfigDataValue().ToLower() == "true"; } }
        /// <summary>
        /// 是否需要权限校验
        /// </summary>
        public static bool IsNeedAuthorize { get { return ConfigDataValue().ToLower() == "true"; } }
        /// <summary>
        /// JS版本号
        /// </summary>
        public static string JSVersion { get { return ConfigDataValue(); } }
        /// <summary>
        /// 数据库类型
        /// </summary>
        public static string DataBaseType { get { return ConfigDataValue(); } }
        /// <summary>
        /// 数据库链接字符串
        /// </summary>
        public static string DBConnStr
        {
            get
            {
                if (DataBaseType == ET.DataBaseType.sqlserver.ToString()) return DBSqlServerStr;
                if (DataBaseType == ET.DataBaseType.mongo.ToString()) return DBMongoStr;
                return DBSqlServerStr;
            }
        }
        /// <summary>
        /// 日志地址
        /// </summary>
        public static string LogPath
        {
            get
            {
                string temp_path = string.Empty;
                if ("remote".Equals(LogType))
                {
                    temp_path = LogPath_Remote;
                }
                else
                {
                    temp_path = LogPath_Local;
                }
                if (temp_path.IsNullOrEmpty()) return "C://webconfig//logs//default//";
                else return temp_path + ProjectName;
            }
        }
        /// <summary>
        /// SqlServer链接字符串
        /// </summary>
        public static string DBSqlServerStr
        {
            get
            {
                if (IsDebug) return DBSqlServer_Debug;
                else return DBSqlServer_Release;
            }
        }
        /// <summary>
        /// MongoDB链接字符串
        /// </summary>
        public static string DBMongoStr
        {
            get
            {
                if (IsDebug) return DBMongo_Debug_Connstr;
                else return DBMongo_Release_Connstr;
            }
        }
        /// <summary>
        /// MongoDB数据库名称
        /// </summary>
        public static string DBMongoDBName
        {
            get
            {
                if (IsDebug) return DBMongo_Debug_DataBase;
                else return DBMongo_Release_DataBase;
            }
        }
        /// <summary>
        /// Redis链接字符串
        /// </summary>
        public static string DBRedisStr
        {
            get
            {
                if (IsDebug) return DBRedis_Debug_Connstr;
                else return DBRedis_Release_Connstr;
            }
        }
        /// <summary>
        /// Redis密码
        /// </summary>
        public static string DBRedisPwd
        {
            get
            {
                if (IsDebug) return DBRedis_Debug_Pwd;
                else return DBRedis_Release_Pwd;
            }
        }
        #endregion

        #region 私有属性
        /// <summary>
        /// 日志类型
        /// </summary>
        private static string LogType { get { return ConfigDataValue(); } }
        /// <summary>
        /// 本地日志
        /// </summary>
        private static string LogPath_Local { get { return ConfigDataValue(); } }
        /// <summary>
        /// 远程日志
        /// </summary>
        private static string LogPath_Remote { get { return ConfigDataValue(); } }        
        /// <summary>
        /// sqlserver数据库链接字符串(debug)
        /// </summary>
        private static string DBSqlServer_Debug { get { return ConfigDataValue(); } }
        /// <summary>
        /// sqlserver数据库链接字符串(release)
        /// </summary>
        private static string DBSqlServer_Release { get { return ConfigDataValue(); } }
        /// <summary>
        /// mongo数据库地址(debug)
        /// </summary>
        private static string DBMongo_Debug_Connstr { get { return ConfigDataValue(); } }
        /// <summary>
        /// mongo数据库名称(debug)
        /// </summary>
        private static string DBMongo_Debug_DataBase { get { return ConfigDataValue(); } }
        /// <summary>
        /// mongo数据库地址(release)
        /// </summary>
        private static string DBMongo_Release_Connstr { get { return ConfigDataValue(); } }
        /// <summary>
        /// mongo数据库名称(release)
        /// </summary>
        private static string DBMongo_Release_DataBase { get { return ConfigDataValue(); } }
        /// <summary>
        /// redis数据库地址(debug)
        /// </summary>
        private static string DBRedis_Debug_Connstr { get { return ConfigDataValue(); } }
        /// <summary>
        /// redis数据库密码(debug)
        /// </summary>
        private static string DBRedis_Debug_Pwd { get { return ConfigDataValue(); } }
        /// <summary>
        /// redis数据库地址(release)
        /// </summary>
        private static string DBRedis_Release_Connstr { get { return ConfigDataValue(); } }
        /// <summary>
        /// redis数据库密码(release)
        /// </summary>
        private static string DBRedis_Release_Pwd { get { return ConfigDataValue(); } }
        #endregion   
    }
}
