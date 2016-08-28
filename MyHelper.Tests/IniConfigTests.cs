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
    public class IniConfigTests
    {
        [TestMethod()]
        public void IniConfigTest()
        {
            IniConfig config = new IniConfig("C:\\Users\\admin\\Desktop\\MyOpenCV\\OpenCVTest\\CsharpDiaoYong\\bin\\Debug\\config\\my.ini");
            config.get("undone");
            Assert.Fail();
        }
    }
}