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
        /// <summary>
        /// 配置项字典
        /// </summary>
        private Dictionary<string, string> configData;
        /// <summary>
        /// 配置文件路径
        /// </summary>
        private string fullFileName;
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="_fileName">.ini文件的绝对路径</param>
        public IniConfig(string _fileName)
        {
            configData = new Dictionary<string, string>();
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
                        configData.Add(key, value);
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
            if (configData.Count <= 0)
                return null;
            else if (configData.ContainsKey(key))
                return configData[key].ToString();
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
            if (configData.ContainsKey(key))
                configData[key] = value;
            else
                configData.Add(key, value);
        }
        /// <summary>
        /// 保存配置项
        /// </summary>
        public void save()
        {
            StreamWriter writer = new StreamWriter(fullFileName, false, Encoding.Default);
            IDictionaryEnumerator enu = configData.GetEnumerator();
            while (enu.MoveNext())
            {
                if (enu.Key.ToString().StartsWith(";"))
                    writer.WriteLine(enu.Value);
                else
                    writer.WriteLine(enu.Key + "=" + enu.Value);
            }
            writer.Close();
        }
    }
}
