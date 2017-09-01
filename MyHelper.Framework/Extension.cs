// <summary>  
// Copyright：Sichen International Co. Ltd.
// Author：Devin
// Date：2016-10-20
// Modifyed：selonsy  
// ModifyTime：2016-10-20  
// Desc：
// 扩展类
// </summary> 
using Devin.MongoDB;
using MongoDB.Bson;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Devin
{
    /// <summary>
    /// object(默认)
    /// </summary>
    public static partial class Extension
    {

        #region Is/NotNull

        public static bool IsNotNull(this object obj)
        {
            return obj != null;
        }

        public static bool IsNull(this object obj)
        {
            return obj == null;
        }

        #endregion

        #region GetAttribute

        public static T GetAttribute<T>(this object obj) where T : class
        {
            return obj.GetType().GetAttribute<T>();
        }

        public static T GetAttribute<T>(this Type type) where T : class
        {
            T t;
            Attribute customAttribute = type.GetCustomAttribute(typeof(T));
            t = (!customAttribute.IsNotNull() ? default(T) : (T)(customAttribute as T));
            return t;
        }

        #endregion

    }

    /// <summary>
    /// DateTime
    /// </summary>
    public static partial class Extension
    {

        /// <summary>
        /// 日期的最小值(1900-01-01 00:00:00)
        /// </summary>
        /// <param name="sqlDateTime"></param>
        /// <returns></returns>
        public static DateTime MinValue(this DateTime sqlDateTime)
        {            
            return new DateTime(1900, 01, 01, 00, 00, 00);
        }

        /// <summary>
        /// 将JS时间戳(毫秒)转换成C#的DateTime
        /// </summary>
        /// <param name="datetime"></param>
        /// <param name="jsTimeStamp"></param>
        /// <returns></returns>
        public static DateTime ToLocalDateTime(this long jsTimeStamp, bool is_unix = false)
        {
            DateTime startTime = new DateTime(1970, 1, 1).ToLocalTime();
            DateTime dt = is_unix ? startTime.AddSeconds(jsTimeStamp) : startTime.AddMilliseconds(jsTimeStamp);
            return dt;
        }

        /// <summary>
        /// 获取当前时间的时间戳表示
        /// </summary>
        /// <param name="datetime"></param>
        /// <param name="is_unix"></param>
        /// <returns></returns>
        public static long GetTimeStamp(this DateTime datetime, bool is_unix = false)
        {
            DateTime startTime = new DateTime(1970, 1, 1).ToLocalTime();
            long timeStamp = is_unix ? (long)(datetime - startTime).TotalSeconds : (long)(datetime - startTime).TotalMilliseconds;
            return timeStamp;
        }

        /// <summary>
        /// 将当前时间转化成发布时间(几个月前,几周前,几天前,几小时前,几分钟前,几秒前或刚刚)
        /// 备注:一个月按30天计算
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ToPublishTime(this DateTime datetime)
        {
            TimeSpan span = DateTime.Now - datetime;
            if (span.TotalDays > 60) return datetime.ToLongDateString();
            else
            {
                if (span.TotalDays > 30) return "1个月前";
                else
                {
                    if (span.TotalDays > 14) return "2周前";
                    else
                    {
                        if (span.TotalDays > 7) return "1周前";
                        else
                        {
                            if (span.TotalDays > 1) return string.Format("{0}天前", (int)Math.Floor(span.TotalDays));
                            else
                            {
                                if (span.TotalHours > 1) return string.Format("{0}小时前", (int)Math.Floor(span.TotalHours));
                                else
                                {
                                    if (span.TotalMinutes > 1) return string.Format("{0}分钟前", (int)Math.Floor(span.TotalMinutes));
                                    else
                                    {
                                        if (span.TotalSeconds >= 1) return string.Format("{0}秒前", (int)Math.Floor(span.TotalSeconds));
                                        else return "刚刚";
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Mongo
    /// </summary>
    public static partial class Extension
    {
        /// <summary>
        /// 格式化Json字符串
        /// </summary>
        /// <param name="bson"></param>
        /// <returns></returns>
        private static object ToCommonJsonObj(this BsonValue bson)
        {
            StringBuilder sb = new StringBuilder();
            switch (bson.BsonType)
            {
                case BsonType.Array:
                    {
                        var arr = bson as BsonArray;
                        if (arr.IsEmptyValue()) { sb.Append(arr.ToString()); break; }

                        sb.Append("[");
                        foreach (var item in arr)
                        {
                            sb.Append(item.ToCommonJsonObj());
                            sb.Append(",");
                        }
                        sb.Remove(sb.Length - 1, 1);
                        sb.Append("]");
                    }; break;
                case BsonType.Document:
                    {
                        var doc = bson as BsonDocument;
                        if (doc.IsEmptyValue()) { sb.Append(doc.ToString()); break; }

                        sb.Append("{");
                        foreach (var kv in doc)
                        {
                            sb.Append($"\"{kv.Name}\":{kv.Value.ToCommonJsonObj()},");
                        }
                        sb.Remove(sb.Length - 1, 1);
                        sb.Append("}");

                    }; break;
                case BsonType.Boolean:
                    {
                        return (bson as BsonBoolean).Value ? "true" : "false";
                    }
                case BsonType.Double:
                    return bson.AsDouble;
                case BsonType.Int32:
                    return bson.AsInt32;
                case BsonType.Int64:
                    return bson.AsInt64;
                case BsonType.DateTime:
                    return $"\"{bson.ToLocalTime().ToString()}\"";
                case BsonType.Null:
                    return "";
                case BsonType.ObjectId:
                case BsonType.String:
                    return $"\"{bson.ToString()}\"";
                default:
                    {
                        return $"\"{bson.ToString()}\"";
                    };
            }
            return sb.ToString();
        }

        public static string ToCommonJson(this BsonValue bson)
        {
            return bson.ToCommonJsonObj().ToString();
        }

        public static BsonDocument ParseBson(this string doc, bool is_throw_exception = false)
        {
            BsonDocument bson = null;
            if (is_throw_exception) return BsonDocument.Parse(doc);

            bool is_success = BsonDocument.TryParse(doc, out bson);
            if (is_success) return bson;
            else return null;
        }
    }

    /// <summary>
    /// Enum
    /// </summary>
    public static partial class Extension
    {
        /// <summary>
        /// ToInt
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ToInt(this Enum value)
        {
            return Convert.ToInt32(value);
        }
    }

    /// <summary>
    /// String
    /// </summary>
    public static partial class Extension
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

        #region Substring

        /// <summary>
        /// 字符串正则截取
        /// </summary>
        /// <param name="str"></param>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static string SubReg(this string str, string begin, string end)
        {
            Regex rg = new Regex("(?<=(" + begin + "))[.\\s\\S]*?(?=(" + end + "))", RegexOptions.Multiline | RegexOptions.Singleline);
            return rg.Match(str).Value;
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

        #region Camel/Pascal

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

        #region Int

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
        /// 字符串转整型
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static int ToInt(this string s, bool is_exception = true)
        {
            if (is_exception) { return int.Parse(s); }
            try
            {
                return int.Parse(s);
            }
            catch
            {
                return 0;
            }
        }

        #endregion
    }

    /// <summary>
    /// IEnumerable<T>
    /// </summary>
    public static partial class Extension
    {
        /// <summary>
        /// 数组或list随机选出几个
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="collection">数组或list</param>
        /// <param name="count">选出数量</param>
        /// <returns></returns>
        public static IEnumerable<T> Random<T>(this IEnumerable<T> collection, int count)
        {
            var rd = new Random();
            return collection.OrderBy(c => rd.Next()).Take(count);
        }

        /// <summary>
        /// 数组或list随机选出一个
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static T Random<T>(this IEnumerable<T> collection)
        {
            return collection.Random<T>(1).SingleOrDefault();
        }
    }
}
