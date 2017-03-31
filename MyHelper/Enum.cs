// <summary>  
// Copyright：Sichen International Co. Ltd.
// Author：Devin
// Date：2017-3-23  
// Modifyed：selonsy  
// ModifyTime：2017-3-23
// Desc：
// 枚举类
// </summary> 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devin
{
    /// <summary>
    /// 枚举类
    /// </summary>
    public static class ET
    {
        /// <summary>
        /// 配置类型
        /// </summary>
        public enum ConfigType
        {
            Xml = 1,
            Ini = 2
        }

        /// <summary>
        /// 有效状态
        /// </summary>
        public enum 有效状态
        {
            有效 = 1,
            无效 = 0
        }

        /// <summary>
        /// 数据库类型
        /// </summary>
        public enum DataBaseType
        {
            sqlserver = 1001,
            access = 1002,
            oracle = 1003,
            mysql = 1004,
            mongo = 1005,
        }

        /// <summary>
        /// 用户类型
        /// </summary>
        public enum UserType
        {
            /// <summary>
            /// 超级管理员
            /// </summary>
            supervisor = 1,
            /// <summary>
            /// 普通管理员
            /// </summary>
            admin = 2,
            /// <summary>
            /// 普通用户
            /// </summary>
            user = 3,
        }

        /// <summary>
        /// 系统请求状态码
        /// </summary>
        public enum ReturnCode
        {
            //系统的返回值(1-99)
            //[Text("成功")]
            Success = 1,

            //[Text("失败")]
            Failed = 2,

            //[Text("无效请求")]
            InvalidRequest = 3,

            //[Text("数据验证不通过")]
            InvalidArgs = 4,

            //[Text("文件下载")]
            File = 5,

            //[Text("HTML页面")]
            HTML = 6,

            //[Text("未授权访问")]
            NotAuthorized = 7,

            //[Text("系统异常")]
            Exception = 99,


            //用户的返回值(10000-?)
            //[Text("账号错误")]
            InvalidAccount = 10000,

            //[Text("密码错误")]
            InvalidPassword = 10001,

            //[Text("账号权限不够")]
            AuthForbid = 10002,

            //[Text("账号被禁用")]
            AccountDisable = 10003,

            //[Text("账号未激活")]
            AccountNoActive = 10004,
        }

        /// <summary>
        /// Api请求状态码
        /// </summary>
        public enum ApiStatusCode
        {
            //[Text("请求/处理成功")]
            Success = 200,

            //[Text("内部请求出错")]
            Error = 500,

            //[Text("未授权标识")]
            Unauthorized = 401,

            //[Text("请求参数不完整或不正确")]
            ParameterError = 400,

            //[Text("请求TOKEN失效")]
            TokenInvalid = 403,

            //[Text("HTTP请求类型不合法")]
            HttpMehtodError = 405,

            //[Text("HTTP请求不合法,请求参数可能被篡改")]
            HttpRequestError = 406,

            //[Text("该URL已经失效")]
            URLExpireError = 407,
        }
    }
}
