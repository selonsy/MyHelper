// <summary>  
// Copyright：Sichen International Co. Ltd.
// Author：Devin
// Date：2017-3-20
// Modifyed：selonsy  
// ModifyTime：2017年5月20日13:52:48  
// Desc：
// 配置管理类
// </summary> 

using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Devin
{
    /// <summary>
    /// 配置抽象基类
    /// </summary>
    public abstract class MyConfig
    {
        /// <summary>
        /// 配置项字典
        /// </summary>
        public Dictionary<string, string> ConfigDic;
        /// <summary>
        /// 配置文件路径
        /// </summary>
        public string FileName;
        /// <summary>
        /// 初始化配置
        /// </summary>
        /// <param name="_filename"></param>
        /// <returns></returns>
        public abstract Dictionary<string, string> InitConfig(string _filename);
        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Set(string key, string value)
        {
            if (ConfigDic.ContainsKey(key))
                ConfigDic[key] = value;
            else
                ConfigDic.Add(key, value);
        }
        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string Get(string key)
        {
            if (ConfigDic.Count <= 0)
                return string.Empty;
            else if (ConfigDic.ContainsKey(key))
                return ConfigDic[key].ToString();
            else
                return string.Empty;
        }
        /// <summary>
        /// 保存配置
        /// </summary>
        public abstract void Save();
    }

    /// <summary>
    /// 配置管理类(xxx.json)
    /// </summary>
    public class JsonConfig : MyConfig
    {
        public override Dictionary<string, string> InitConfig(string _filename)
        {
            FileName = _filename;
            ConfigDic = new Dictionary<string, string>();

            bool hasCfgFile = File.Exists(FileName);
            if (hasCfgFile)
            {
                string config_str = File.ReadAllText(FileName, Encoding.UTF8);
                JObject config_obj = JObject.Parse(config_str);

                foreach (var item in config_obj)
                {
                    string key = item.Key.ToLower().Replace("_", "");
                    string value = item.Value.ToString();
                    if (ConfigDic.ContainsKey(key))
                    {
                        //同名的键值对,后面的有效
                        ConfigDic[key] = value;
                    }
                    else
                    {
                        ConfigDic.Add(key, value);
                    }
                }
            }
            else
            {
                File.Create(FileName);
            }

            return ConfigDic;
        }

        public override void Save()
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 配置管理类(xxx.xml)
    /// </summary>
    public class XmlConfig : MyConfig
    {
        public override Dictionary<string, string> InitConfig(string _filename)
        {
            FileName = _filename;
            ConfigDic = new Dictionary<string, string>();

            bool hasCfgFile = File.Exists(FileName);
            if (hasCfgFile)
            {
                XDocument xd = XDocument.Load(FileName);
                XElement xe = xd.Root;
                foreach (XElement item in xe.Elements())
                {
                    string key = item.Name.ToString().ToLower().Replace("_", "");
                    string value = item.Value.ToString();

                    if (ConfigDic.ContainsKey(key))
                    {
                        //同名的键值对,后面的有效
                        ConfigDic[key] = value;
                    }
                    else
                    {
                        ConfigDic.Add(key, value);
                    }
                }                
            }
            else
            {
                File.Create(FileName);
            }

            return ConfigDic;
        }

        public override void Save()
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 配置管理类(xxx.ini)
    /// </summary>
    public class IniConfig : MyConfig
    {
        public override Dictionary<string, string> InitConfig(string _filename)
        {
            FileName = _filename;
            ConfigDic = new Dictionary<string, string>();

            FileStream fs = new FileStream(FileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            StreamReader sr = new StreamReader(fs);
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                if (line.StartsWith(";") || line.StartsWith("[") || string.IsNullOrEmpty(line))
                    continue;
                else
                {
                    int index = line.IndexOf('=');
                    if (index != -1)
                    {
                        string key = line.Substring(0, index).Trim().ToLower().Replace("_", "");
                        string value = line.Substring(index + 1, line.Length - index - 1).Trim();
                        if (ConfigDic.ContainsKey(key))
                        {
                            //同名的键值对,后面的有效
                            ConfigDic[key] = value;
                        }
                        else
                        {
                            ConfigDic.Add(key, value);
                        }
                    }
                }
            }

            return ConfigDic;
        }

        public override void Save()
        {
            throw new NotImplementedException();
        }
    }
}
