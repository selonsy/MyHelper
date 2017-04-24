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
using System.Xml;

namespace Devin
{
    /// <summary>
    /// 配置管理类(xxx.config)
    /// </summary>
    public class XmlConfig
    {
        #region private

        /// <summary>
        /// 配置项字典
        /// </summary>
        private Dictionary<string, string> configdata;
        /// <summary>
        /// 配置文件路径
        /// </summary>
        private string fullFileName;
        /// <summary>
        /// Xml文档
        /// </summary>
        private XmlDocument _xml;
        /// <summary>
        /// XML的根节点
        /// </summary>
        private XmlElement _element;

        #endregion

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="_fileName">.ini文件的绝对路径</param>
        public XmlConfig(string _fileName)
        {
            configdata = new Dictionary<string, string>();
            fullFileName = _fileName;
            bool hasCfgFile = File.Exists(fullFileName);
            if (hasCfgFile == false)
            {
                StreamWriter writer = new StreamWriter(File.Create(fullFileName), Encoding.Default);
                writer.Close();
            }
            else
            {
                //创建一个XML对象
                _xml = new XmlDocument();
                //加载XML文件
                _xml.Load(fullFileName);
                //为XML的根节点赋值
                _element = _xml.DocumentElement;
                foreach (XmlNode node in _element.ChildNodes)
                {
                    if (node.NodeType == XmlNodeType.Element)
                    {
                        string key = node.Name;
                        string value = node.InnerText;
                        if (configdata.ContainsKey(key))
                        {
                            //同名的键值对,后面的有效
                            configdata[key] = value;
                        }
                        else
                        {
                            configdata.Add(key, value);
                        }                        
                    }
                }
            }
        }
       
        /// <summary>
        /// 获取配置项
        /// </summary>
        /// <param name="key">配置项名称</param>
        /// <returns></returns>
        public string get(string key)
        {
            if (configdata.Count <= 0)
                return null;
            else if (configdata.ContainsKey(key))
                return configdata[key].ToString();
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
            if (configdata.ContainsKey(key))
                configdata[key] = value;
            else
                configdata.Add(key, value);
        }

        #region 属性

        /// <summary>
        /// 配置项字典
        /// </summary>
        public Dictionary<string, string> ConfigData
        {
            get { return configdata; }
        }

        #endregion
    }
}
