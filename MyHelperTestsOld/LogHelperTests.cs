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
    public class LogHelperTests
    {
        [Test()]
        public void WriteExceptionTest()
        {
            LogHelper.WriteException(new Exception(), "hahahah");           
        }
    }
}