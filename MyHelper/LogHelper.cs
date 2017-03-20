// <summary>  
// Copyright：Sichen International Co. Ltd.
// Author：Devin
// Date：2016-08-24  
// Modifyed：selonsy  
// ModifyTime：2016-08-24  
// Desc：
// 日志类
// </summary> 


using System;
using System.IO;
using System.Web;

namespace Devin
{
    /// <summary>
    /// 日志类
    /// </summary>
    public static class LogHelper
    {        
      
        #region Exception日志

        /// <summary>
        /// 记录Exception日志
        /// </summary>
        /// <param name="ex">Exception</param>
        public static void WriteException(Exception ex)
        {
            string errMsg = FormatMsg(ex, "", LogType.Exception);
            MyWriteLog(errMsg, defaultPath, LogType.Exception);
        }

        /// <summary>
        /// 记录Exception日志，自定义信息
        /// </summary>
        /// <param name="ex">Exception</param>
        /// <param name="msg">自定义信息</param>
        /// <param name="ps"></param>
        public static void WriteException(Exception ex, string msg, params object[] ps)
        {
            string errMsg = FormatMsg(ex, msg, LogType.Exception, ps);
            MyWriteLog(errMsg, defaultPath, LogType.Exception);
        }

        ///// <summary>
        ///// 记录Exception日志，自定义信息、保存路径
        ///// </summary>
        ///// <param name="ex">Exception</param>
        ///// <param name="msg">自定义信息</param>
        ///// <param name="path">保存路径</param>
        ///// <param name="ps"></param>
        //public static void WriteException(Exception ex,string msg,string path,params object[] ps)
        //{
        //    string errMsg = FormatMsg(ex, msg, LogType.Exception, ps);
        //    MyWriteLog(errMsg, path, LogType.Exception);
        //}

        #endregion

        #region Debug日志

        /// <summary>
        /// 记录Debug日志
        /// </summary>
        /// <param name="msg">自定义信息</param>
        /// <param name="ps"></param>
        public static void WriteDebug(string msg, params object[] ps)
        {
            string errMsg = FormatMsg(null, msg, LogType.Debug, ps);
            MyWriteLog(errMsg, defaultPath, LogType.Debug);
        }

        ///// <summary>
        ///// 记录Debug日志，自定义保存路径
        ///// </summary>
        ///// <param name="msg">自定义信息</param>
        ///// <param name="path">保存路径</param>
        ///// <param name="ps"></param>
        //public static void WriteDebug(string msg, string path, params object[] ps)
        //{
        //    string errMsg = FormatMsg(null, msg, LogType.Debug, ps);
        //    MyWriteLog(errMsg, path, LogType.Debug);
        //}

        #endregion

        #region Error日志

        /// <summary>
        /// 记录Error日志
        /// </summary>
        /// <param name="msg">自定义信息</param>
        /// <param name="ps"></param>
        public static void WriteError(string msg, params object[] ps)
        {
            string errMsg = FormatMsg(null, msg, LogType.Error, ps);
            MyWriteLog(errMsg, defaultPath, LogType.Error);
        }

        ///// <summary>
        ///// 记录Error日志，自定义保存路径
        ///// </summary>
        ///// <param name="msg">自定义信息</param>
        ///// <param name="path">保存路径</param>
        ///// <param name="ps"></param>
        //public static void WriteError(string msg, string path, params object[] ps)
        //{
        //    string errMsg = FormatMsg(null, msg, LogType.Error, ps);
        //    MyWriteLog(errMsg, path, LogType.Error);
        //}

        #endregion

        #region Request日志

        /// <summary>
        /// 记录Request日志
        /// </summary>
        /// <param name="msg">自定义信息</param>
        /// <param name="ps"></param>
        public static void WriteRequest(string msg, params object[] ps)
        {
            string errMsg = FormatMsg(null, msg, LogType.Request, ps);
            MyWriteLog(errMsg, defaultPath, LogType.Request);
        }

        ///// <summary>
        ///// 记录Request日志，自定义保存路径
        ///// </summary>
        ///// <param name="msg">自定义信息</param>
        ///// <param name="path">保存路径</param>
        ///// <param name="ps"></param>
        //public static void WriteRequest(string msg, string path, params object[] ps)
        //{
        //    string errMsg = FormatMsg(null, msg, LogType.Request, ps);
        //    MyWriteLog(errMsg, path, LogType.Request);
        //}

        #endregion

        #region 私有方法

        /// <summary>
        /// 日志文件目录 
        /// </summary>                
        private static string defaultPath = Base.LogDefaultPath;

        /// <summary>
        /// 日志类型
        /// </summary>
        private enum LogType
        {
            /// <summary>
            /// error
            /// </summary>
            Error,
            /// <summary>
            /// debug
            /// </summary>
            Debug,
            /// <summary>
            /// exception
            /// </summary>
            Exception,
            /// <summary>
            /// Request
            /// </summary>
            Request,
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="msg">日志内容</param>
        /// <param name="path">日志存放路径</param>
        /// <param name="logType">日志类型</param>
        private static void MyWriteLog(string msg, string path, LogType logType)
        {                     
            string fileName = path.Trim('\\') + "\\" + logType + "\\" + CreateFileName(logType);
            WriteFile(msg, fileName);
        }

        /// <summary>
        /// 写日志到文件
        /// </summary>
        /// <param name="logContext">日志内容</param>
        /// <param name="fullName">文件名</param>
        private static void WriteFile(string logContext, string fullName)
        {
            FileStream fs = null;
            StreamWriter sw = null;

            int splitIndex = fullName.LastIndexOf('\\');
            if (splitIndex == -1) return;
            string path = fullName.Substring(0, splitIndex);

            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            try
            {
                if (!File.Exists(fullName)) fs = new FileStream(fullName, FileMode.CreateNew);
                else fs = new FileStream(fullName, FileMode.Append);

                sw = new StreamWriter(fs);
                sw.WriteLine(logContext);
            }
            finally
            {
                if (sw != null)
                {
                    sw.Close();
                    sw.Dispose();
                }
                if (fs != null)
                {
                    fs.Close();
                    fs.Dispose();
                }
            }
        }

        /// <summary>
        /// 从异常类中获取日志内容
        /// </summary>
        /// <param name="ex">异常类</param>
        /// <returns>日志内容</returns>
        private static string CreateErrorMeg(Exception ex)
        {
            string result = string.Empty;
            result += "\r\n堆栈信息:";
            result += "\r\n[GetType]" + ex.GetType() + "\r\n";
            result += "[Message]"+ex.Message + "\r\n";
            result += "[Source]" + ex.Source + "\r\n";
            result += "[TargetSite]" + ex.TargetSite + "\r\n";
            result += "[Data]" + ex.Data + "\r\n";                       
            result += "[StackTrace]\r\n" + ex.StackTrace + "\r\n";
            return result;
        }

        /// <summary>
        /// 格式化日志内容
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="msg"></param>
        /// <param name="logtype"></param>
        /// <param name="ps"></param>
        /// <returns></returns>
        private static string FormatMsg(Exception ex, string msg, LogType logtype, params object[] ps)
        {
            //Result
            string result;
                
            //Header
            string header = string.Format("[{0}][{1} {2}] ", logtype.ToString(), DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString());
            
            //Msg
            msg = string.Format(msg, ps);
            if (ex != null)
            {
                msg += CreateErrorMeg(ex);
            }

            result = header + msg;
            return result;
        }

        /// <summary>
        /// 生成日志文件名
        /// </summary>
        /// <param name="logType">日志类型</param>
        /// <returns>日志文件名</returns>
        private static string CreateFileName(LogType logType)
        {
            string result = DateTime.Now.ToString("yyyyMMdd");
            if (logType == LogType.Error)
            {
                result = logType.ToString() + "-" + result + ".txt";
            }
            else if (logType == LogType.Debug)
            {
                result = logType.ToString() + "-" + result + ".txt";
            }
            else if (logType == LogType.Exception)
            {
                result = logType.ToString() + "-" + result + ".txt";
            }
            else if (logType == LogType.Request)
            {
                result = logType.ToString() + "-" + result + ".txt";
            }
            return result;
        }

        #endregion
    }

}