using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Text.RegularExpressions;

namespace Devin.MongoDB
{
    /// <summary>
    /// mongodb扩展方法
    /// </summary>
    public static partial class MongoDbExtension
    {
        /// <summary>
        /// 获取更新信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        //public static UpdateDefinition<T> GetUpdateDefinition<T>(this T entity)
        //{
        //    var properties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);

        //    var updateDefinitionList = GetUpdateDefinitionList<T>(properties, entity);

        //    var updateDefinitionBuilder = new UpdateDefinitionBuilder<T>().Combine(updateDefinitionList);

        //    return updateDefinitionBuilder;
        //}

        /// <summary>
        /// 获取更新信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyInfos"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static List<UpdateDefinition<T>> GetUpdateDefinitionList<T>(PropertyInfo[] propertyInfos, object entity)
        {
            var updateDefinitionList = new List<UpdateDefinition<T>>();

            propertyInfos = propertyInfos.Where(a => a.Name != "_id").ToArray();

            //foreach (var propertyInfo in propertyInfos)
            //{
            //    if (propertyInfo.PropertyType.IsArray || typeof(IList).IsAssignableFrom(propertyInfo.PropertyType))
            //    {
            //        var value = propertyInfo.GetValue(entity) as IList;

            //        var filedName = propertyInfo.Name;

            //        updateDefinitionList.Add(Builders<T>.Update.Set(filedName, value));
            //    }
            //    else
            //    {
            //        var value = propertyInfo.GetValue(entity);
            //        if (propertyInfo.PropertyType == typeof(decimal))
            //            value = value.ToString();

            //        var filedName = propertyInfo.Name;

            //        updateDefinitionList.Add(Builders<T>.Update.Set(filedName, value));
            //    }
            //}

            return updateDefinitionList;
        }
    }

    /// <summary>
    /// mongodb扩展方法
    /// </summary>
    public static partial class MongoDbExtension
    {
        /// <summary>
        /// Mongo路径查找
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static BsonValue PathGet(this BsonDocument doc, string path)
        {
            string[] pathArr = null;
            try
            {
                pathArr = MongoPath.ParsePath(path);
            }
            catch (Exception ex)
            {

            }
            //路径表达式错误
            if (pathArr == null || pathArr.Length < 1) return null;
            BsonValue cursor = doc;
            int index = -1;
            foreach (var node in pathArr)
            {
                if (cursor.IsBsonDocument && cursor.AsBsonDocument.Contains(node))
                    cursor = cursor[node];
                else if (cursor.IsBsonArray && int.TryParse(node, out index))
                    cursor = cursor[index];
                else
                    cursor = string.Empty;
            }
            return cursor;
        }

        /// <summary>
        /// Mongo路径更新
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="path"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static BsonDocument PathSet(this BsonDocument doc, string path, BsonValue value)
        {
            string[] pathArr = null;
            try
            {
                pathArr = MongoPath.ParsePath(path);
            }
            catch (Exception)
            {
            }
            if (pathArr == null || pathArr.Length < 1) return doc;
            BsonValue cursor = doc;
            int index = -1;
            for (var i = 0; i < pathArr.Length - 1; i++)
            {
                var node = pathArr[i];
                if (cursor.IsBsonDocument && cursor.AsBsonDocument.Contains(node))
                    cursor = cursor[node];
                else if (cursor.IsBsonArray && int.TryParse(node, out index))
                    cursor = cursor[index];
                else
                    return doc;
            }
            var lastNode = pathArr[pathArr.Length - 1];
            if (cursor.IsBsonDocument && cursor.AsBsonDocument.Contains(lastNode))
                cursor[lastNode] = value;
            else if (cursor.IsBsonArray && int.TryParse(lastNode, out index) && cursor.AsBsonArray.Count > index)
                cursor[index] = value;
            return doc;
        }

        /// <summary>
        /// 检查是否为空
        /// </summary>
        /// <param name="bv"></param>
        /// <returns></returns>
        public static bool IsEmptyValue(this BsonValue bv)
        {
            if (bv == null) return true;
            bool result = false;
            switch (bv.BsonType)
            {
                //永远为空
                case BsonType.Undefined:
                case BsonType.Null:
                    result = true;
                    break;              
                case BsonType.Int32:
                    result = bv.AsInt32 == 0;
                    break;
                case BsonType.Int64:
                    result = bv.AsInt64 == 0;
                    break;
                case BsonType.String:
                    //字符串为空
                    result = string.IsNullOrEmpty(bv.AsString.Trim());
                    break;
                case BsonType.Document:
                    //文档无元素小于1为空
                    result = bv.AsBsonDocument.ElementCount < 1;
                    break;
                case BsonType.Array:
                    //数组的长度小于1表示为空
                    result = bv.AsBsonArray.Count < 1;
                    break;
                //默认以ToString是否为空来进行判断
                default:
                    result = string.IsNullOrEmpty(bv.ToString());
                    break;
                //case BsonType.EndOfDocument:
                //    break;
                //case BsonType.Double:
                //    break;
                //case BsonType.Binary:
                //    break;               
                //case BsonType.ObjectId:
                //    break;
                //case BsonType.Boolean:
                //    break;
                //case BsonType.DateTime:
                //    break;               
                //case BsonType.RegularExpression:
                //    break;
                //case BsonType.JavaScript:
                //    break;
                //case BsonType.Symbol:
                //    break;
                //case BsonType.JavaScriptWithScope:
                //    break;
                //case BsonType.Timestamp:
                //    break;
                //case BsonType.Decimal128:
                //    break;
                //case BsonType.MinKey:
                //    break;
                //case BsonType.MaxKey:
                //    break;
            }
            return result;
        }

        /// <summary>
        /// 检查是否不空
        /// </summary>
        /// <param name="bv"></param>
        /// <returns></returns>
        public static bool IsNotEmptyValue(this BsonValue bv)
        {
            return !IsEmptyValue(bv);
        }
    }

    /// <summary>
    /// mongo路径查找
    /// </summary>
    public class MongoPath
    {
        private enum Actions { APPEND = 0, PUSH = 1, INC_SUB_PATH_DEPTH = 2, PUSH_SUB_PATH = 3 };

        private enum States { BEFORE_PATH = 0, IN_PATH, BEFORE_IDENT, IN_IDENT, IN_SUB_PATH, IN_SINGLE_QUOTE, IN_DOUBLE_QUOTE, AFTER_PATH, ERROR };

        Dictionary<int, Dictionary<string, int[]>> pathStateMachine = new Dictionary<int, Dictionary<string, int[]>>();

        private void InitStateMachine()
        {
            pathStateMachine.Add((int)States.BEFORE_PATH, new Dictionary<string, int[]>()
            {
                {"ws",new[] { (int)States.BEFORE_PATH } },
                {"ident",new[] { (int)States.IN_IDENT,(int)Actions.APPEND } },
                {"[",new[] { (int)States.IN_SUB_PATH } },
                {"eof",new[] { (int)States.AFTER_PATH } }
            });

            pathStateMachine.Add((int)States.IN_PATH, new Dictionary<string, int[]>()
            {
                {"ws",new[] { (int)States.IN_PATH } },
                {".",new[] { (int)States.BEFORE_IDENT} },
                {"[",new[] { (int)States.IN_SUB_PATH } },
                {"eof",new[] { (int)States.AFTER_PATH } }
            });

            pathStateMachine.Add((int)States.BEFORE_IDENT, new Dictionary<string, int[]>()
            {
                {"ws",new[] { (int)States.BEFORE_IDENT } },
                {"ident",new[] { (int)States.IN_IDENT ,(int)Actions.APPEND} }
            });

            pathStateMachine.Add((int)States.IN_IDENT, new Dictionary<string, int[]>()
            {
                {"ident",new[] { (int)States.IN_IDENT, (int)Actions.APPEND} },
                {"0",new[] { (int)States.IN_IDENT ,(int)Actions.APPEND} },
                 {"number",new[] { (int)States.IN_IDENT ,(int)Actions.APPEND} },
                 {"ws",new[] { (int)States.IN_PATH, (int)Actions.PUSH } },
                 {".",new[] { (int)States.BEFORE_IDENT, (int)Actions.PUSH } },
                 {"[",new[] { (int)States.IN_SUB_PATH, (int)Actions.PUSH } },
                 {"eof",new[] { (int)States.AFTER_PATH, (int)Actions.PUSH } }
            });

            pathStateMachine.Add((int)States.IN_SUB_PATH, new Dictionary<string, int[]>()
            {
                {"'",new[] { (int)States.IN_SINGLE_QUOTE, (int)Actions.APPEND} },
                {"\"",new[] { (int)States.IN_DOUBLE_QUOTE, (int)Actions.APPEND} },
                 {"[",new[] { (int)States.IN_SUB_PATH, (int)Actions.INC_SUB_PATH_DEPTH } },
                 {"]",new[] { (int)States.IN_PATH, (int)Actions.PUSH_SUB_PATH } },
                 {"eof",new[] { (int)States.ERROR} },
                 {"else",new[] { (int)States.IN_SUB_PATH, (int)Actions.APPEND } }
            });


            pathStateMachine.Add((int)States.IN_SINGLE_QUOTE, new Dictionary<string, int[]>()
            {
                {"'",new[] { (int)States.IN_SUB_PATH, (int)Actions.APPEND } },
                 {"eof",new[] { (int)States.ERROR} },
                 {"else",new[] { (int)States.IN_SINGLE_QUOTE, (int)Actions.APPEND } }
            });

            pathStateMachine.Add((int)States.IN_DOUBLE_QUOTE, new Dictionary<string, int[]>()
            {
                {"\"",new[] { (int)States.IN_SUB_PATH, (int)Actions.APPEND } },
                 {"eof",new[] { (int)States.ERROR} },
                 {"else",new[] { (int)States.IN_DOUBLE_QUOTE, (int)Actions.APPEND } }
            });
        }

        private MongoPath()
        {
            InitStateMachine();
        }

        /**
         * Determine the type of a character in a keypath.
         *
         * @param {Char} ch
         * @return {String} type
         */
        private static string getPathCharType(char? ch)
        {
            if (ch == null)
            {
                return "eof";
            }

            var code = (int)ch;

            switch (code)
            {
                case 0x5B: // [
                case 0x5D: // ]
                case 0x2E: // .
                case 0x22: // "
                case 0x27: // '
                case 0x30: // 0
                    return ch + "";
                case 0x5F: // _
                case 0x24: // $
                    return "ident";
                case 0x20:     // Space
                case 0x09:     // Tab
                case 0x0A:     // Newline
                case 0x0D:     // Return
                case 0xA0:     // No-break space
                case 0xFEFF:   // Byte Order Mark
                case 0x2028:   // Line Separator
                case 0x2029:   // Paragraph Separator
                    return "ws";
            }
            // a-z, A-Z
            if (
              (code >= 0x61 && code <= 0x7A) ||
              (code >= 0x41 && code <= 0x5A) ||
              (code >= 0x4e00 && code <= 0x9fa5)
            )
            {
                return "ident";
            }
            // 1-9
            if (code >= 0x31 && code <= 0x39)
            {
                return "number";
            }
            return "else";
        }

        /**
         * Format a subPath, return its plain form if it is
         * a literal string or number. Otherwise prepend the
         * dynamic indicator (*).
         *
         * @param {String} path
         * @return {String}
         */
        static string formatSubPath(string path)
        {
            var trimmed = path.Trim();
            // invalid leading 0
            if (path[0] == '0' && isNaN(path))
            {
                return null;
            }
            return isLiteral(trimmed)
              ? stripQuotes(trimmed)
              : '*' + trimmed;
        }

        static bool isNaN(object obj)
        {
            int out_int;
            double out_double;
            if (obj is string)
            {
                return !(int.TryParse(obj as string, out out_int) || double.TryParse(obj as string, out out_double));
            }
            if (obj is int || obj is double || obj is bool) return false;
            if (obj == null) return false;
            return true;
        }

        /**
         * Check if an expression is a literal value.
         *
         * @param {String} exp
         * @return {Boolean}
         */
        static string literalValueRE = "^\\s?(true|false|-?[\\d\\.]+|'[^']*'|\"[^\"]*\")\\s?$";
        static bool isLiteral(string exp)
        {
            return new Regex(literalValueRE).IsMatch(exp);
        }

        /**
         * Strip quotes from a string
         *
         * @param {String} str
         * @return {String | false}
         */
        static string stripQuotes(string str)
        {
            var a = str[0];
            var b = str[str.Length - 1];
            return a == b && (a == 0x22 || a == 0x27) ? str.Substring(1, str.Length - 2) : str;
        }

        public static string[] ParsePath(string path)
        {
            return new Parse().parse(path).ToArray();
        }

        class Parse
        {
            List<string> keys = new List<string>();
            int index = -1;
            int? mode = (int)States.BEFORE_PATH;
            int subPathDepth = 0;
            int[] transition;
            Dictionary<string, int[]> typeMap;
            char? c;
            string newChar, type;
            string key;
            string path;

            MongoPath _path = new MongoPath();

            Func<bool> action;
            Dictionary<int, Func<bool>> actions = new Dictionary<int, Func<bool>>();

            public Parse()
            {
                InitActions();
            }

            private void InitActions()
            {
                actions.Add((int)Actions.PUSH, () =>
                {
                    if (!string.IsNullOrEmpty(key))
                    {
                        keys.Add(key);
                        key = null;
                    }
                    return true;
                });

                actions.Add((int)Actions.APPEND, () =>
                {
                    if (string.IsNullOrEmpty(key))
                    {
                        key = newChar;
                    }
                    else
                    {
                        key += newChar;
                    }
                    return true;
                });

                actions.Add((int)Actions.INC_SUB_PATH_DEPTH, () =>
                {
                    actions[(int)Actions.APPEND]();
                    subPathDepth++;
                    return true;
                });

                actions.Add((int)Actions.PUSH_SUB_PATH, () =>
                {
                    if (subPathDepth > 0)
                    {
                        subPathDepth--;
                        mode = (int)States.IN_SUB_PATH;
                        actions[(int)Actions.APPEND]();
                    }
                    else
                    {
                        subPathDepth = 0;
                        key = formatSubPath(key);
                        if (string.IsNullOrEmpty(key))
                        {
                            return false;
                        }
                        else
                        {
                            actions[(int)Actions.PUSH]();
                        }
                    }
                    return true;
                });
            }

            bool maybeUnescapeQuote()
            {
                var nextChar = path[index + 1];
                if ((mode == (int)States.IN_SINGLE_QUOTE && nextChar == '\'') || (mode == (int)States.IN_DOUBLE_QUOTE && nextChar == '"'))
                {
                    index++;
                    newChar = "\\" + nextChar;
                    actions[(int)Actions.APPEND]();
                    return true;
                }
                return false;
            }

            /**
             * Parse a string path into an array of segments
             *
             * @param {String} path
             * @return {Array|undefined}
             */
            public List<string> parse(string path)
            {
                this.path = path;
                while (mode != null)
                {
                    index++;
                    c = index < path.Length ? path[index] : (char?)null;
                    if (c == '\\' && maybeUnescapeQuote())
                    {
                        continue;
                    }
                    type = getPathCharType(c);
                    typeMap = _path.pathStateMachine[mode.Value];
                    transition = typeMap.ContainsKey(type) ? typeMap[type] : typeMap.ContainsKey("else") ? typeMap["else"] : new[] { (int)States.ERROR };
                    if (transition[0] == (int)States.ERROR)
                    {
                        // parse error
                        return null; 
                    }
                    mode = transition[0];
                    action = transition.Length > 1 ? actions[transition[1]] : null;
                    if (action != null)
                    {
                        //transition[2];
                        newChar = null;
                        newChar = newChar == null ? c + "" : newChar;
                        if (action() == false)
                        {
                            return null;
                        }
                    }
                    if (mode == (int)States.AFTER_PATH)
                    {
                        return keys;
                    }
                }
                return null;
            }
        }
    }
}
