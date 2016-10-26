using NUnit.Framework;
using Devin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Devin.Tests
{
    [TestFixture()]
    public class ReflectionHelperTests
    {
        [Test()]
        public void CreateClassInstanceTest()
        {
            object obj = ReflectionHelper.CreateClassInstance("Devin.Services.apiService,Devin_aliyun");
            MethodInfo methodInfo = ReflectionHelper.CreateMethodInstance(obj, "gettoken");
            string value = ReflectionHelper.DoInvoke<string>(obj, methodInfo, new object[] { "nihao" });
            Assert.IsNotNull(value);
        }
    }
}