using Xunit;
using Devin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devin.Tests
{
    public class ExtensionTests
    {
        [Fact()]
        public void ToPublishTimeTest()
        {             
            
            Assert.Equal(DateTime.Now.AddDays(-61).ToLongDateString(), DateTime.Now.AddDays(-61).ToPublishTime());            
            Assert.Equal("1个月前", DateTime.Now.AddDays(-59).ToPublishTime());
            Assert.Equal("1个月前", DateTime.Now.AddDays(-31).ToPublishTime());
            Assert.Equal("2周前", DateTime.Now.AddDays(-30).ToPublishTime());
            Assert.Equal("2周前", DateTime.Now.AddDays(-29).ToPublishTime());
            Assert.Equal("2周前", DateTime.Now.AddDays(-15).ToPublishTime());
            Assert.Equal("1周前", DateTime.Now.AddDays(-14).ToPublishTime());
            Assert.Equal("1周前", DateTime.Now.AddDays(-13).ToPublishTime());
            Assert.Equal("1周前", DateTime.Now.AddDays(-8).ToPublishTime());
            Assert.Equal("7天前", DateTime.Now.AddDays(-7).ToPublishTime());
            Assert.Equal("6天前", DateTime.Now.AddDays(-6).ToPublishTime());            
            Assert.Equal("24小时前", DateTime.Now.AddDays(-1).ToPublishTime());

            Assert.Equal("24小时前", DateTime.Now.AddHours(-24).ToPublishTime());
            Assert.Equal("15小时前", DateTime.Now.AddHours(-15).ToPublishTime());
            Assert.Equal("60分钟前", DateTime.Now.AddHours(-1).ToPublishTime());

            Assert.Equal("1小时前", DateTime.Now.AddMinutes(-65).ToPublishTime());
            Assert.Equal("60分钟前", DateTime.Now.AddMinutes(-60).ToPublishTime());
            Assert.Equal("59分钟前", DateTime.Now.AddMinutes(-59).ToPublishTime());
            Assert.Equal("30分钟前", DateTime.Now.AddMinutes(-30).ToPublishTime());
            Assert.Equal("60秒前", DateTime.Now.AddMinutes(-1).ToPublishTime());

            Assert.Equal("15秒前", DateTime.Now.AddSeconds(-15).ToPublishTime());
            Assert.Equal("1秒前", DateTime.Now.AddSeconds(-1).ToPublishTime());
            Assert.Equal("刚刚", DateTime.Now.AddSeconds(-0.5).ToPublishTime());

        }
    }
}