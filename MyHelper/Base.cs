// <summary>  
// Copyright：Sichen International Co. Ltd.
// Author：Devin
// Date：2017-3-20
// Modifyed：selonsy  
// ModifyTime：2017-3-20  
// Desc：
// 基类
// </summary> 

using System.Web;

namespace Devin
{
    /// <summary>
    /// 基类
    /// </summary>
    public class Base
    {
        #region 私有变量

        private static string _connstr;
        private static string _logdefaultpath;
        private static string _inipath;
        private static string _databasetype;
        private static string _isdebug;
        private static string _jversion;
        private static IniConfig config = new IniConfig(HttpRuntime.AppDomainAppPath.ToString() + "\\bin\\ufjnls.dll.ini");

        #endregion

        /// <summary>
        /// 构造函数(初始化配置信息)
        /// </summary>
        public Base()
        {
            _connstr = WebConfigHelper.Connstr;
            _logdefaultpath = WebConfigHelper.LogPath;
            _isdebug = WebConfigHelper.IsDebug;
            _databasetype = WebConfigHelper.DataBaseType;
            _jversion = WebConfigHelper.JVersion;
        }

        /// <summary>
        /// IsDebug
        /// </summary>
        public static bool IsDebug
        {
            get
            {
                if (_isdebug.IsNullOrEmpty())
                {
                    _isdebug = config.get("isdebug");                    
                }
                return _isdebug == "true";                
            }            
        } 

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public static string ConnStr
        {
            get
            {
                if (_connstr.IsNullOrEmpty())
                {
                    _connstr = config.get("connstr");
                }
                return _connstr;
            }
        }        
     
        /// <summary>
        /// 日志文件路径
        /// </summary>
        public static string LogDefaultPath
        {
            get
            {
                if (_logdefaultpath.IsNullOrEmpty())
                {
                    _logdefaultpath = config.get("logpath");
                }
                return HttpRuntime.AppDomainAppPath.ToString() + _logdefaultpath;
            }
        }
        
        /// <summary>
        /// 数据库类型
        /// </summary>
        public static string DataBaseType
        {
            get
            {
                if (_databasetype.IsNullOrEmpty())
                {
                    _databasetype = config.get("databasetype");
                }
                return _databasetype;
            }
        }
    }
}
