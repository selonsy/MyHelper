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
    public class IniConfigTests
    {
        [Test()]
        public void IniConfigTest()
        {
            IniConfig config = new IniConfig(@"D:\00MyWorkSpace\99MyGitHub\MyHelper\MyHelperTests\bin\Debug\ufjnls.dll.ini");
            Assert.Fail();
        }
    }
}