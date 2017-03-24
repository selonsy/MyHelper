using NUnit.Framework;
using Devin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devin.Tests
{
    [TestFixture()]
    public class BaseTests
    {
        [OneTimeSetUp()]
        public void Init()
        {
            Base.Init_Config("shenjinlong", 1);
        }

        [Test()]
        public void IsDebugTest()
        {
            
            bool a = true;
            bool b = Base.IsDebug;
            Assert.AreEqual(a, b);
        }
        [Test()]
        public void JVersionTest()
        {
            string a = "1000";
            string b = Base.JVersion;
            Assert.AreEqual(a, b);
        }
        [Test()]
        public void DataBaseTypeTest()
        {
            string a = "sqlserver";
            string b = Base.DataBaseType;
            Assert.AreEqual(a, b);
        }
        [Test()]
        public void LogDefaultPathTest()
        {
            string a = "";
            string b = Base.LogDefaultPath;
            Assert.AreNotEqual(a, b);
        }
        [Test]
        public void ConnStr()
        {
            string connstr = Base.ConnStr;
            bool a = true;
            bool b = SQLHelper.IsLegalConnStr(connstr);
            Assert.AreEqual(a, b);
        }
    }
}