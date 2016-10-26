// <summary>  
// Copyright：Sichen International Co. Ltd.
// Author：Devin
// Date：2016-10-24
// Modifyed：selonsy  
// ModifyTime：2016-10-24  
// Desc：
// 反射帮助类
// </summary> 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Devin;

namespace Devin
{
    /// <summary>
    /// 反射帮助类
    /// </summary>
    public class ReflectionHelper
    {

        #region 创建对象实例

        /// <summary>
        /// 创建对象实例
        /// </summary>
        /// <param name="fullName">命名空间名称.类型名称,程序集名称(如:MyNameSpace.MyClassName,MyAssemblyName)</param>        
        /// <returns></returns>
        public static object CreateClassInstance(string fullName)
        {
            return createClassInstance(fullName);
        }

        /// <summary>
        /// 创建对象实例
        /// </summary>
        /// <param name="nameSpace">命名空间名称</param>
        /// <param name="className">类型名称</param>
        /// <param name="assemblyName">程序集名称</param>
        /// <returns></returns>
        public static object CreateClassInstance(string nameSpace, string className, string assemblyName)
        {
            string path = nameSpace + "." + className + "," + assemblyName;
            return createClassInstance(path);
        }

        private static object createClassInstance(string path)
        {
            if (path.IsNullOrEmpty()) return null;
            try
            {
                Type type = Type.GetType(path);                                      //加载类型
                object obj = Activator.CreateInstance(type, true);                   //根据类型创建实例
                return obj;
            }
            catch (Exception ex)
            {
                LogHelper.WriteException(ex, "反射创建类实例失败,path:{0}", path);
                return null;
            }
        }

        #endregion

        #region 创建方法实例

        /// <summary>
        /// 创建方法实例
        /// </summary>
        /// <param name="service">对象实例</param>
        /// <param name="methodName">方法名称</param>
        /// <returns></returns>
        public static MethodInfo CreateMethodInstance(object service, string methodName)
        {
            return createMethodInstance(service, methodName);
        }

        /// <summary>
        /// 创建方法实例
        /// </summary>
        /// <param name="fullName">命名空间名称.类型名称,程序集名称(如:MyNameSpace.MyClassName,MyAssemblyName)</param>
        /// <param name="methodName">方法名称</param>
        /// <returns></returns>
        public static MethodInfo CreateMethodInstance(string fullName, string methodName)
        {
            object service = CreateClassInstance(fullName);
            return createMethodInstance(service, methodName);
        }

        /// <summary>
        /// 创建方法实例
        /// </summary>
        /// <param name="fullName">命名空间名称.类型名称,程序集名称(如:MyNameSpace.MyClassName,MyAssemblyName)</param>
        /// <param name="methodName">方法名称</param>
        /// <param name="service">对象实例</param>
        /// <returns></returns>
        public static MethodInfo CreateMethodInstance(string fullName, string methodName,ref object service)
        {
            service = CreateClassInstance(fullName);
            return createMethodInstance(service, methodName);
        }

        /// <summary>
        /// 创建方法实例
        /// </summary>
        /// <param name="nameSpace">命名空间名称</param>
        /// <param name="className">类型名称</param>
        /// <param name="assemblyName">程序集名称</param>
        /// <param name="methodName">方法名称</param>
        /// <returns></returns>
        public static MethodInfo CreateMethodInstance(string nameSpace, string className, string assemblyName, string methodName)
        {
            string path = nameSpace + "." + className + "," + assemblyName;
            object service = CreateClassInstance(path);
            return createMethodInstance(service, methodName);
        }

        private static MethodInfo createMethodInstance(object service, string methodName)
        {
            if (service == null || methodName.IsNullOrEmpty()) return null;
            try
            {
                Type serviceType = service.GetType();
                MethodInfo methodinfo = serviceType.GetMethod(methodName);
                return methodinfo;
            }
            catch (Exception ex)
            {
                LogHelper.WriteException(ex, "反射创建方法实例失败,servicetype:{0},methodName:{1}", service.GetType(), methodName);
                return null;
            }
        }

        #endregion

        #region 执行反射方法

        /// <summary>
        /// 执行反射方法
        /// </summary>
        /// <param name="service">类实例</param>
        /// <param name="methodinfo">方法实例</param>        
        /// <param name="args">参数数组</param>
        /// <returns></returns>
        public static T DoInvoke<T>(object service, MethodInfo methodinfo, params object[] args)
        {
            return doInvoke<T>(service, methodinfo, args);
        }

        /// <summary>
        /// 执行反射方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fullName">命名空间.类型名,程序集(如:MyNameSpace.MyClassName,MyAssemblyName)</param>
        /// <param name="methodName">方法名称</param>
        /// <param name="args">参数数组</param>
        /// <returns></returns>
        public static T DoInvoke<T>(string fullName, string methodName, params object[] args)
        {
            object service = null;
            MethodInfo methodinfo = CreateMethodInstance(fullName, methodName, ref service);
            return doInvoke<T>(service, methodinfo, args);
        }

        private static T doInvoke<T>(object service, MethodInfo methodinfo, params object[] args)
        {
            if (service == null || methodinfo == null) return default(T);
            try
            {
                T returnData = (T)methodinfo.Invoke(service, args);
                return returnData;
            }
            catch (Exception ex)
            {
                LogHelper.WriteException(ex, "反射方法执行失败,servicetype:{0},methodtype:{1},args:{2}", service.GetType(), methodinfo.GetType(), args.Length > 0 ? string.Join(";", args) : "");
                return default(T);
            }
        }

        #endregion

    }
}
