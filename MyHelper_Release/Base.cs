using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devin
{
    /// <summary>
    /// 基类
    /// </summary>
    public static class Base
    {
        /// <summary>
        /// sa模式连接字符串
        /// </summary>
        public static string Conn4Sa = AppConfig.GetConnectionString("connstrforsa");

        /// <summary>
        /// 集成模式连接字符串
        /// </summary>
        public static string Conn4Window = AppConfig.GetConnectionString("connstrforwindows");

        /// <summary>
        /// 日志文件路径
        /// </summary>
        public static string LogDefaultPath = AppConfig.GetAppConfig("LogPath");              
    }
}
