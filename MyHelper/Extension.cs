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
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Devin
{
    /// <summary>
    /// 扩展类
    /// </summary>
    public static class Extension
    {
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
        /// ToInt
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

        #region DateTime

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

        #region Enum

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

        #region NewtonSoft

        public static JObject ToCommon(this JObject value)
        {
            if (value.Property("_id") != null)
            {
                value["id"] = value["_id"]["$oid"];
                value.Remove("_id");
            }
            return value;
        }

        public static JArray ToCommon(this JArray array)
        {
            foreach (JObject item in array)
            {
                if (item.Property("_id") != null)
                {
                    item["id"] = item["_id"]["$oid"];
                    item.Remove("_id");
                }
            }
            return array;
        }

        #endregion

        #region MongoDb

        /// <summary>
        /// 格式化Json字符串
        /// </summary>
        /// <param name="bson"></param>
        /// <returns></returns>
        public static object ToCommonJsonObj(this BsonValue bson)
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
        #endregion
    }
}
