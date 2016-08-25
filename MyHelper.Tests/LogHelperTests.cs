using Microsoft.VisualStudio.TestTools.UnitTesting;
using Devin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devin.Tests
{
    [TestClass()]
    public class LogHelperTests
    {
        [TestMethod()]
        public void WriteExceptionTest()
        {
            LogHelper.WriteException(new Exception("error heheda"), "我的第{0}次异常", 1);
            LogHelper.WriteDebug("我的第{0}次Debug", 0);
            LogHelper.WriteError("我的第{0}次Error", 0);
            LogHelper.WriteRequest("我的第{0}次Request", 0);            
            Assert.Fail();
        }
    }
}