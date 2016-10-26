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
    public class UtilsTests
    {
        [Test()]
        public void GetNewGuidTest()
        {
            string a = Utils.GetNewGuid();
            Assert.AreEqual(32,a.Length);
        }
    }
}