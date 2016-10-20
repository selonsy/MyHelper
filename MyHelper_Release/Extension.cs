// <summary>  
// Copyright：Sichen International Co. Ltd.
// Author：Devin
// Date：2016-10-20
// Modifyed：selonsy  
// ModifyTime：2016-10-20  
// Desc：
// 扩展类
// </summary> 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Devin
{
    /// <summary>
    /// 扩展类
    /// </summary>
    public static class Extension
    {

        #region Format String

        /// <summary>
        /// Format String
        /// </summary>
        /// <param name="format"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string FormatWith(this string format, params object[] param)
        {
            return string.Format(format, param);
        }
        /// <summary>
        /// Format String
        /// </summary>
        /// <param name="format"></param>
        /// <param name="arg0"></param>
        /// <returns></returns>
        public static string FormatWith(this string format, object arg0)
        {
            return string.Format(format, arg0);
        }
        /// <summary>
        /// Format String
        /// </summary>
        /// <param name="format"></param>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        /// <returns></returns>
        public static string FormatWith(this string format, object arg0, object arg1)
        {
            return string.Format(format, arg0, arg1);
        }
        /// <summary>
        /// Format String
        /// </summary>
        /// <param name="format"></param>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <returns></returns>
        public static string FormatWith(this string format, object arg0, object arg1, object arg2)
        {
            return string.Format(format, arg0, arg1, arg2);
        }
        /// <summary>
        /// Format String
        /// </summary>
        /// <param name="format"></param>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="arg3"></param>
        /// <returns></returns>
        public static string FormatWith(this string format, object arg0, object arg1, object arg2, object arg3)
        {
            return string.Format(format, arg0, arg1, arg2, arg3);
        }

        #endregion

        #region Is/NotNullOrEmpty

        /// <summary>
        /// IsNullOrEmpty
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }
        /// <summary>
        /// IsNotNullOrEmpty
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNotNullOrEmpty(this string str)
        {
            return !string.IsNullOrEmpty(str);
        }

        #endregion

        #region Match

        /// <summary>
        /// Match
        /// </summary>
        /// <param name="str"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static bool IsMatch(this string str, string pattern)
        {
            if (str == null) return false;
            else return Regex.IsMatch(str, pattern);
        }
        /// <summary>
        /// Match
        /// </summary>
        /// <param name="str"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static string Match(this string str, string pattern)
        {
            if (str == null) return "";
            return Regex.Match(str, pattern).Value;
        }

        #endregion

    }
}
