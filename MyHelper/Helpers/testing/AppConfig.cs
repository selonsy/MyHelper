// <summary>  
// Copyright：Sichen International Co. Ltd.
// Author：Devin
// Date：2016-08-24  
// Modifyed：selonsy  
// ModifyTime：2016-08-24  
// Desc：
// 配置管理类
// </summary> 

using System;
using System.Configuration;
using System.Collections.Specialized;
using System.ServiceModel.Configuration;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Devin
{

    /// <summary>
    /// 配置管理类(machine.config)
    /// </summary>
    public static partial class AppConfig
    {
        //注意：
        //默认的配置文件路径为：
        //"C:\\Windows\\Microsoft.NET\\Framework\\v4.0.30319\\Config\\machine.config"

        private static readonly NameValueCollection appSettings;
        private static readonly ConnectionStringSettingsCollection connectionstringsettings;

        /// <summary>
        /// 构造函数
        /// </summary>
        static AppConfig()
        {        
            appSettings = ConfigurationManager.AppSettings;
            connectionstringsettings = ConfigurationManager.ConnectionStrings;
        }

        /// <summary>
        /// 获取连接字符串
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetConnStr(string key)
        {
            ConnectionStringSettings value = connectionstringsettings[key];

            if (value != null)
                return value.ConnectionString;

            throw new ApplicationException("在配置文件的connectionStrings节点集合中找不到key为" + key + "的子节点");
        }

        /// <summary>
        /// 获取Bool值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool GetBoolValue(string key)
        {
            bool value = false;
            if (appSettings[key] != null)
                bool.TryParse(appSettings[key], out value);

            return value;
        }

        /// <summary>
        /// 获取int值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static int GetIntValue(string key)
        {
            int value = 0;
            if (appSettings[key] != null)
                int.TryParse(appSettings[key], out value);

            return value;
        }

        #region GetString

        /// <summary>
        /// 获取配置文件中appSettings节点下指定索引键的字符串类型的的值
        /// </summary>
        /// <param name="key">索引键</param>
        /// <returns>字符串</returns>
        public static string GetString(string key)
        {
            return getValue(key, true, null);
        }

        /// <summary>
        /// 获取配置文件中appSettings节点下指定索引键的字符串类型的的值
        /// </summary>
        /// <param name="key">索引键</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>字符串</returns>
        public static string GetString(string key, string defaultValue)
        {
            return getValue(key, false, defaultValue);
        }

        #endregion

        #region GetStringArray

        /// <summary>
        /// 获取配置文件中appSettings节点下指定索引键的string[]类型的的值
        /// </summary>
        /// <param name="key">索引键</param>
        /// <param name="separator">分隔符</param>
        /// <returns>字符串数组</returns>
        public static string[] GetStringArray(string key, string separator)
        {
            return getStringArray(key, separator, true, null);
        }

        /// <summary>
        /// 获取配置文件中appSettings节点下指定索引键的string[]类型的的值
        /// </summary>
        /// <param name="key">索引键</param>
        /// <param name="separator">分隔符</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>字符串数组</returns>
        public static string[] GetStringArray(string key, string separator, string[] defaultValue)
        {
            return getStringArray(key, separator, false, defaultValue);
        }

        /// <summary>
        /// 获取配置文件中appSettings节点下指定索引键的string[]类型的的值
        /// </summary>
        /// <param name="key">索引键</param>
        /// <param name="separator">分隔符</param>
        /// <param name="valueRequired">指定配置文件中是否必须需要配置有该名称的元素，传入False则方法返回默认值，反之抛出异常</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>字符串数组</returns>
        private static string[] getStringArray(string key, string separator, bool valueRequired, string[] defaultValue)
        {
            string value = getValue(key, valueRequired, null);

            if (!string.IsNullOrEmpty(value))
                return value.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries);
            else if (!valueRequired)
                return defaultValue;

            throw new ApplicationException("在配置文件的appSettings节点集合中找不到key为" + key + "的子节点，且没有指定默认值");
        }

        #endregion

        #region GetInt32

        /// <summary>
        /// 获取配置文件中appSettings节点下指定索引键的Int类型的的值
        /// </summary>
        /// <param name="key">索引键</param>
        /// <returns>Int</returns>
        public static int GetInt32(string key)
        {
            return getInt32(key, null);
        }

        /// <summary>
        /// 获取配置文件中appSettings节点下指定索引键的Int类型的的值
        /// </summary>
        /// <param name="key">索引键</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>Int</returns>
        public static int GetInt32(string key, int defaultValue)
        {
            return getInt32(key, defaultValue);
        }

        /// <summary>
        /// 获取配置文件中appSettings节点下指定索引键的Int类型的的值
        /// </summary>
        /// <param name="key">索引键</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>Int</returns>
        private static int getInt32(string key, int? defaultValue)
        {
            return getValue<int>(key, (v, pv) => int.TryParse(v, out pv), defaultValue);
        }

        #endregion

        #region GetBoolean

        /// <summary>
        /// 获取配置文件中appSettings节点下指定索引键的布尔类型的的值
        /// </summary>
        /// <param name="key">索引键</param>
        /// <returns>布尔值</returns>
        public static bool GetBoolean(string key)
        {
            return getBoolean(key, null);
        }

        /// <summary>
        /// 获取配置文件中appSettings节点下指定索引键的布尔类型的的值
        /// </summary>
        /// <param name="key">索引键</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>布尔值</returns>
        public static bool GetBoolean(string key, bool defaultValue)
        {
            return getBoolean(key, defaultValue);
        }

        /// <summary>
        /// 获取配置文件中appSettings节点下指定索引键的布尔类型的的值
        /// </summary>
        /// <param name="key">索引键</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>布尔值</returns>
        private static bool getBoolean(string key, bool? defaultValue)
        {
            return getValue<bool>(key, (v, pv) => bool.TryParse(v, out pv), defaultValue);
        }

        #endregion

        #region GetTimeSpan

        /// <summary>
        /// 获取配置文件中appSettings节点下指定索引键的时间间隔类型的的值
        /// </summary>
        /// <param name="key">索引键</param>
        /// <returns>时间间隔</returns>
        public static TimeSpan GetTimeSpan(string key)
        {
            return TimeSpan.Parse(getValue(key, true, null));
        }

        /// <summary>
        /// 获取配置文件中appSettings节点下指定索引键的时间间隔类型的的值
        /// </summary>
        /// <param name="key">索引键</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>时间间隔</returns>
        public static TimeSpan GetTimeSpan(string key, TimeSpan defaultValue)
        {
            string val = getValue(key, false, null);

            if (val == null)
                return defaultValue;

            return TimeSpan.Parse(val);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 获取配置文件appSettings集合中指定索引的值
        /// </summary>
        /// <typeparam name="T">返回值类型参数</typeparam>
        /// <param name="key">索引键</param>
        /// <param name="parseValue">将指定索引键的值转化为返回类型的值的委托方法</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        private static T getValue<T>(string key, Func<string, T, bool> parseValue, T? defaultValue) where T : struct
        {
            string value = appSettings[key];

            if (value != null)
            {
                T parsedValue = default(T);

                if (parseValue(value, parsedValue))
                    return parsedValue;
                else
                    throw new ApplicationException(string.Format("Setting '{0}' was not a valid {1}", key, typeof(T).FullName));
            }

            if (!defaultValue.HasValue)
                throw new ApplicationException("在配置文件的appSettings节点集合中找不到key为" + key + "的子节点，且没有指定默认值");
            else
                return defaultValue.Value;
        }

        /// <summary>
        /// 获取配置文件appSettings集合中指定索引的值
        /// </summary>
        /// <param name="key">索引</param>
        /// <param name="valueRequired">指定配置文件中是否必须需要配置有该名称的元素，传入False则方法返回默认值，反之抛出异常</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>字符串</returns>
        private static string getValue(string key, bool valueRequired, string defaultValue)
        {
            string value = appSettings[key];

            if (value != null)
                return value;
            else if (!valueRequired)
                return defaultValue;

            throw new ApplicationException("在配置文件的appSettings节点集合中找不到key为" + key + "的子节点");
        }

        #endregion
    }

    /// <summary>
    /// 配置管理类(app.config[*.dll.config])
    /// </summary>
    public static partial class AppConfig
    {
        /// <summary>
        /// 配置文件路径
        /// </summary>
        /// <returns></returns>
        private static string configpath
        {
            get
            {
                //如果在外部的程序里面引用的话,获取的是当前执行程序的bin目录下面的dll的路径,而不是其引用的位置的dll
                return Assembly.GetExecutingAssembly().Location;                
            }                       
        }

        /// <summary>
        /// 配置文件类的实例
        /// </summary>
        private static Configuration config
        {
            get
            {
                System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(configpath);
                return config;
            }
        }

        /// <summary>
        /// 获取连接字符串
        /// </summary>
        /// <param name="connectionName">连接字符串名称</param>
        /// <returns></returns>
        public static string GetConnectionString(string connectionName)
        {
            string connectionString =
                config.ConnectionStrings.ConnectionStrings[connectionName].ConnectionString.ToString();
            return connectionString;
        }

        /// <summary> 
        /// 更新连接字符串(存在先删除，不存在即新建)
        /// </summary> 
        /// <param name="newName">连接字符串名称</param> 
        /// <param name="newConString">连接字符串内容</param> 
        /// <param name="newProviderName">数据提供程序名称</param> 
        public static void UpdateConnectionString(string newName, string newConString, string newProviderName)
        {
            bool exist = false;                                 
            if (config.ConnectionStrings.ConnectionStrings[newName] != null)
            {
                exist = true;
            }            
            if (exist)
            {
                // 如果连接串已存在，首先删除它  
                config.ConnectionStrings.ConnectionStrings.Remove(newName);
            }
            // 新建一个连接字符串实例  
            ConnectionStringSettings mySettings =
                new ConnectionStringSettings(newName, newConString, newProviderName);
            // 将新的连接串添加到配置文件中. 
            config.ConnectionStrings.ConnectionStrings.Add(mySettings);
            // 保存对配置文件所作的更改  
            config.Save(ConfigurationSaveMode.Modified);
            // 强制重新载入配置文件的ConnectionStrings配置节  
            ConfigurationManager.RefreshSection("ConnectionStrings");
        }

        ///<summary> 
        ///获取appSettings配置节的value项  
        ///</summary> 
        ///<param name="strKey"></param> 
        ///<returns></returns> 
        public static string GetAppConfig(string strKey)
        {
            foreach (string key in config.AppSettings.Settings.AllKeys)
            {
                if (key == strKey)
                {
                    return config.AppSettings.Settings[strKey].Value.ToString();
                }
            }
            return null;
        }

        ///<summary>  
        ///更新appSettings配置节的value项(存在先删除，不存在即新建)
        ///</summary>  
        ///<param name="newKey"></param>  
        ///<param name="newValue"></param>  
        public static void UpdateAppConfig(string newKey, string newValue)
        {
            bool exist = false;
            foreach (string key in config.AppSettings.Settings.AllKeys)
            {
                if (key == newKey)
                {
                    exist = true;
                }
            }
            if (exist)
            {
                //如果配置项存在的话，首先删除它
                config.AppSettings.Settings.Remove(newKey);
            }
            config.AppSettings.Settings.Add(newKey, newValue);
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        /// <summary>
        /// 修改system.serviceModel配置节所有服务终结点的IP地址(待完善) 
        /// </summary>        
        /// <param name="serverIP"></param>
        public static void UpdateServiceModelConfig(string serverIP)
        {
            ConfigurationSectionGroup sec = config.SectionGroups["system.serviceModel"];
            ServiceModelSectionGroup serviceModelSectionGroup = sec as ServiceModelSectionGroup;
            ClientSection clientSection = serviceModelSectionGroup.Client;
            foreach (ChannelEndpointElement item in clientSection.Endpoints)
            {
                string pattern = @"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b";
                string address = item.Address.ToString();
                string replacement = string.Format("{0}", serverIP);
                address = Regex.Replace(address, pattern, replacement);
                item.Address = new Uri(address);
            }
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("system.serviceModel");
        }
        
        /// <summary>
        /// 修改applicationSettings中App.Properties.Settings中服务的IP地址(待完善) 
        /// </summary>
        /// <param name="configPath"></param>
        /// <param name="serverIP"></param>
        public static void UpdateApplicationSettingsConfig(string configPath, string serverIP)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(configPath);
            ConfigurationSectionGroup sec = config.SectionGroups["applicationSettings"];
            ConfigurationSection configSection = sec.Sections["DataService.Properties.Settings"];
            ClientSettingsSection clientSettingsSection = configSection as ClientSettingsSection;
            if (clientSettingsSection != null)
            {
                SettingElement element1 = clientSettingsSection.Settings.Get("DataService_SystemManagerWS_SystemManagerWS");
                if (element1 != null)
                {
                    clientSettingsSection.Settings.Remove(element1);
                    string oldValue = element1.Value.ValueXml.InnerXml;
                    element1.Value.ValueXml.InnerXml = GetNewIP(oldValue, serverIP);
                    clientSettingsSection.Settings.Add(element1);
                }

                SettingElement element2 = clientSettingsSection.Settings.Get("DataService_EquipManagerWS_EquipManagerWS");
                if (element2 != null)
                {
                    clientSettingsSection.Settings.Remove(element2);
                    string oldValue = element2.Value.ValueXml.InnerXml;
                    element2.Value.ValueXml.InnerXml = GetNewIP(oldValue, serverIP);
                    clientSettingsSection.Settings.Add(element2);
                }
            }
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("applicationSettings");
        }

#region 私有函数
        private static string GetNewIP(string oldValue, string serverIP)
        {
            string pattern = @"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b";
            string replacement = string.Format("{0}", serverIP);
            string newvalue = Regex.Replace(oldValue, pattern, replacement);
            return newvalue;
        }

#endregion

    }

    /// <summary>
    /// 配置管理类(可自由扩充,新建同名属性即可,调用:AppConfig.YourKeyName)
    /// </summary>
    public static partial class AppConfig
    {
        /// <summary>
        /// 测试配置项目名称
        /// </summary>
        public static string TestConfigName
        {
            get
            {
                return AppSettingValue();
            }
        }

        private static string AppSettingValue([CallerMemberName] string key = null)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }

}
