// <summary>  
// Copyright：Sichen International Co. Ltd.
// Author：Devin
// Date：2017-3-20
// Modifyed：selonsy  
// ModifyTime：2017-3-20  
// Desc：
// 配置管理类
// </summary> 
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devin
{
    /// <summary>
    /// 配置管理类(xxx.ini)
    /// </summary>
    public class IniConfig
    {
        #region private

        /// <summary>
        /// 配置项字典
        /// </summary>
        private Dictionary<string, string> config_data;
        /// <summary>
        /// 配置文件路径
        /// </summary>
        private string fullFileName;

        #endregion

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="_fileName">.ini文件的绝对路径</param>
        public IniConfig(string _fileName)
        {
            config_data = new Dictionary<string, string>();
            fullFileName = _fileName;
            bool hasCfgFile = File.Exists(fullFileName);
            if (hasCfgFile == false)
            {
                StreamWriter writer = new StreamWriter(File.Create(fullFileName), Encoding.Default);
                writer.Close();
            }
            StreamReader reader = new StreamReader(fullFileName, Encoding.Default);
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (line.StartsWith(";") || line.StartsWith("[") || string.IsNullOrEmpty(line))
                    continue;
                else
                {
                    int index = line.IndexOf('=');
                    if (index != -1)
                    {
                        string key = line.Substring(0, index).Trim();
                        string value = line.Substring(index + 1, line.Length - index - 1).Trim();
                        if (config_data.ContainsKey(key))
                        {
                            //同名的键值对,后面的有效
                            config_data[key] = value;
                        }
                        else
                        {
                            config_data.Add(key, value);
                        }                        
                    }
                }
            }
            reader.Close();
        }

        /// <summary>
        /// 获取配置项
        /// </summary>
        /// <param name="key">配置项名称</param>
        /// <returns></returns>
        public string get(string key)
        {
            if (config_data.Count <= 0)
                return null;
            else if (config_data.ContainsKey(key))
                return config_data[key].ToString();
            else
                return null;
        }

        /// <summary>
        /// 设置配置项
        /// </summary>
        /// <param name="key">配置项名称</param>
        /// <param name="value">配置项内容</param>
        public void set(string key, string value)
        {
            if (config_data.ContainsKey(key))
                config_data[key] = value;
            else
                config_data.Add(key, value);
        }

        /// <summary>
        /// 保存配置项
        /// </summary>
        public void save()
        {
            StreamWriter writer = new StreamWriter(fullFileName, false, Encoding.Default);
            IDictionaryEnumerator enu = config_data.GetEnumerator();
            while (enu.MoveNext())
            {
                if (enu.Key.ToString().StartsWith(";"))
                    writer.WriteLine(enu.Value);
                else
                    writer.WriteLine(enu.Key + "=" + enu.Value);
            }
            writer.Close();
        }

        #region 属性

        /// <summary>
        /// 配置项字典
        /// </summary>
        public Dictionary<string, string> ConfigData
        {
            get
            {
                return config_data;
            }
        }

        #endregion
    }
}
