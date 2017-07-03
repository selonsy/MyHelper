using System;
using Xunit;
using Devin;

namespace MyHelper.NetStandard.Test
{
    /// <summary>
    /// 
    /// </summary>
    public class ConfigBaseTest : IDisposable
    {
        public void Dispose()
        {
            //throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ConfigTest: IClassFixture<ConfigBaseTest>
    {
        //Tips:
        //Just before the first tests in ConfigTest is run, xUnit.net will create an instance of ConfigBaseTest.
        //For each test, it will create a new instance of ConfigTest, and pass the shared instance of ConfigBaseTest to the constructor.

        ConfigBaseTest configbasetest;

        public ConfigTest(ConfigBaseTest configbasetest)
        {
            this.configbasetest = configbasetest;            
        }

        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void MyConfigTest()
        {
            Config.Init_Config("default", 3);

            Assert.NotEmpty(Config.DBMongoStr);
            Assert.NotEmpty(Config.DBRedisPwd);
            Assert.NotEmpty(Config.DBRedisStr);
            Assert.NotEmpty(Config.DBSqlServerStr);
            Assert.True(Config.IsDebug);
            Assert.NotEmpty(Config.JSVersion);
            Assert.NotEmpty(Config.LogPath);
            Assert.NotEmpty(Config.ProjectName);

        }
    }
}
