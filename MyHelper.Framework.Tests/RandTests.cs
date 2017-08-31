using Xunit;
using Devin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devin.Tests
{
    public class RandTests
    {
        [Fact()]
        public void NumberTest()
        {
            string str = new Rand().GetRandomStr(6);
            Assert.True(false, "This test needs an implementation");
        }
    }
}