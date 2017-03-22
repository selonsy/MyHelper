// <summary>  
// Copyright：Sichen International Co. Ltd.
// Author：Devin
// Date：2017-3-21
// Modifyed：selonsy  
// ModifyTime：2017-3-21  
// Desc：
// WebConfig帮助类
// </summary> 

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Devin
{
    /// <summary>
    /// web配置获取类
    /// </summary>
    public class WebConfigHelper
    {
        /* 配置项示例:         
           <configuration>
              <appSettings>
                <add key="Connstr" value="user id=sa;password=95938;initial catalog=selonsy;datasource=localhost;connect Timeout=20" />
                <add key="IsDebug" value="false" />
                <add key="LogPath" value="/log/" />
                <add key="JVersion" value="1.0.0.0" />
              </appSettings>
           </configuration>
        */

        /// <summary>
        /// 连接字符串
        /// </summary>
        public static string Connstr
        {
            get
            {
                return AppSettingValue();
            }
        }

        /// <summary>
        /// 是否测试服
        /// </summary>
        public static string IsDebug
        {
            get
            {
                return AppSettingValue();
            }
        }

        /// <summary>
        /// 日志路径
        /// </summary>
        public static string LogPath
        {
            get
            {
                return AppSettingValue();
            }
        }

        /// <summary>
        /// 文件版本号
        /// </summary>
        public static string JVersion
        {
            get
            {
                return AppSettingValue();
            }
        }

        /// <summary>
        /// 数据库类型
        /// </summary>
        public static string DataBaseType
        {
            get
            {
                return AppSettingValue();
            }
        }

        #region private

        /// <summary>
        /// 获取键值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private static string AppSettingValue([CallerMemberName] string key = null)
        {
            return ConfigurationManager.AppSettings[key];
        }

        #endregion
    }
}
