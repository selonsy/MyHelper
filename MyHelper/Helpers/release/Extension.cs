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

        #region string#FormatWith

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

        #region string#IsNullOrEmpty/IsNotNullOrEmpty

        /// <summary>
        /// IsNullOrEmpty
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string s)
        {
            return string.IsNullOrEmpty(s);
        }
        /// <summary>
        /// IsNotNullOrEmpty
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsNotNullOrEmpty(this string s)
        {
            return !string.IsNullOrEmpty(s);
        }

        #endregion

        #region string#Match|IsMatch

        /// <summary>
        /// Match
        /// </summary>
        /// <param name="s"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static bool IsMatch(this string s, string pattern)
        {
            if (s == null) return false;
            else return Regex.IsMatch(s, pattern);
        }
        /// <summary>
        /// Match
        /// </summary>
        /// <param name="s"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static string Match(this string s, string pattern)
        {
            if (s == null) return "";
            return Regex.Match(s, pattern).Value;
        }

        #endregion

        #region string#ToCamel|ToPascal

        /// <summary>
        /// ToCamel
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToCamel(this string s)
        {
            if (s.IsNullOrEmpty()) return s;
            return s[0].ToString().ToLower() + s.Substring(1);
        }
        /// <summary>
        /// ToPascal
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToPascal(this string s)
        {
            if (s.IsNullOrEmpty()) return s;
            return s[0].ToString().ToUpper() + s.Substring(1);
        }

        #endregion

        #region string#IsInt|ToInt

        /// <summary>
        /// IsInt
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsInt(this string s)
        {
            int i;
            return int.TryParse(s, out i);
        }
        /// <summary>
        /// ToInt
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static int ToInt(this string s)
        {
            return int.Parse(s);
        }

        #endregion

        #region Int#sInRange

        /// <summary>
        /// IsInRange
        /// </summary>
        /// <param name="i">i</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <param name="except">过滤值</param>
        /// <param name="isInclude">是否包含边界</param>
        /// <returns></returns>
        public static bool IsInRange(this int i, int min, int max, int[] except = null, bool isInclude = false)
        {
            if (isInclude)
            {
                if (except == null)
                {
                    if (i >= min && i <= max) return true;
                }
                else
                {
                    if (i >= min && i <= max && !except.Contains(i)) return true;
                }
            }
            else
            {
                if (except == null)
                {
                    if (i > min && i < max) return true;
                }
                else
                {
                    if (i > min && i < max && !except.Contains(i)) return true;
                }
            }
            return false;
        }

        #endregion

        #region DateTime#MinValue

        /// <summary>
        /// 日期的最小值(1900-01-01 00:00:00)
        /// </summary>
        /// <param name="sqlDateTime"></param>
        /// <returns></returns>
        public static DateTime MinValue(this DateTime sqlDateTime)
        {
            return new DateTime(1900, 01, 01, 00, 00, 00);
        }

        #endregion

        #region Enum#ToInt

        /// <summary>
        /// ToInt
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ToInt(this Enum value)
        {
            return Convert.ToInt32(value);
        }

        #endregion
    }
}
