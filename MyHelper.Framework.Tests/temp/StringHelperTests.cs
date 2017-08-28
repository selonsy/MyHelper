using Xunit;
using Devin.Temp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devin.Temp.Tests
{
    public class StringHelperTests
    {
        [Fact()]
        public void GetQuanPinTest()
        {
            string str = StringHelper.GetQuanPin("陈玉曌");
            Assert.Equal("chenyuzhao", str);            
        }
    }
}