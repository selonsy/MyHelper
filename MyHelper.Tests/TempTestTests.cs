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
    public class TempTestTests
    {
        [TestMethod()]
        public void getCurrentDirTest()
        {
            var result = new TempTest().getCurrentDir();                        
        }
    }
}