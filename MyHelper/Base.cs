// <summary>  
// Copyright：Sichen International Co. Ltd.
// Author：Devin
// Date：2017-3-20
// Modifyed：selonsy  
// ModifyTime：2017-3-20  
// Desc：
// 基类
// </summary> 

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Web;

namespace Devin
{
    /// <summary>
    /// 基类
    /// </summary>
    public class Base
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
            if (config_dic == null || config_dic.Count <= 0)
                return null;
            else if (config_dic.ContainsKey(key.ToLower()))
                return config_dic[key.ToLower()].ToString();
            else
                return null;
        }

        #endregion

        #region 构造函数

        /// <summary>
        /// 静态构造函数
        /// </summary>
        static Base()
        {
            Init_Config();
        }

        /// <summary>
        /// 初始化配置信息,需要在应用程序启动时执行,如global.asax        
        /// </summary>
        /// <param name="project_name"></param>
        public static void Init_Config(string project_name = "default", int config_type = 1)
        {
            string config_path = string.Empty;
            if (config_type == ET.ConfigType.Xml.ToInt())
            {
                config_path = string.Format("C:\\webconfig\\{0}\\config.xml", project_name);
                config_dic = new XmlConfig(config_path).ConfigData;
            }
            else if (config_type == ET.ConfigType.Ini.ToInt())
            {
                config_path = string.Format("C:\\webconfig\\{0}\\config.ini", project_name);
                config_dic = new IniConfig(config_path).ConfigData;
            }
        }

        #endregion

        //项目名称
        public static string ProjectName { get { return ConfigDataValue(); } }

        //数据库连接字符串
        public static string ConnStr { get { return IsDebug ? ConnStr_Debug : ConnStr_Release; } }
        //测试数据库
        public static string ConnStr_Debug { get { return ConfigDataValue().Trim('\''); } }
        //正式数据库
        public static string ConnStr_Release { get { return ConfigDataValue().Trim('\''); } }
        //数据库类型
        public static string DataBaseType { get { return ConfigDataValue(); } }

        //Redis
        public static string ConnStr_Redis { get { return IsDebug ? ConnStr_Redis_Debug : ConnStr_Redis_Release; } }
        public static string ConnStr_Redis_Pwd { get { return IsDebug ? ConnStr_Redis_Debug_Pwd : ConnStr_Redis_Release_Pwd; } }
        public static string ConnStr_Redis_Debug { get { return ConfigDataValue().Trim(); } }
        public static string ConnStr_Redis_Debug_Pwd { get { return ConfigDataValue().Trim(); } }
        public static string ConnStr_Redis_Release { get { return ConfigDataValue().Trim(); } }
        public static string ConnStr_Redis_Release_Pwd { get { return ConfigDataValue().Trim(); } }

        //IsDebug
        public static bool IsDebug { get { return ConfigDataValue().ToLower() == "true"; } }
        //日志文件路径
        public static string LogDefaultPath { get { return ConfigDataValue(); } }
        //JS版本号
        public static string JVersion { get { return ConfigDataValue(); } }
    }
}
