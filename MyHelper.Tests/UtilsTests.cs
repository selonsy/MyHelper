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
    public class UtilsTests
    {
        [TestMethod()]
        public void compareArrTest()
        {            
            int[] arr1 = { 1, 2, 3, 4 };
            int[] arr2 = { 1, 2, 3, 4 };
            int[] arr3 = { 2, 1, 4, 3 };
            int[] arr4 = { 9, 5, 2, 7 };

            Assert.IsTrue(Utils.compareArr_1(arr1, arr2));            
            Assert.IsTrue(Utils.compareArr_1(arr1, arr3));
            Assert.IsFalse(Utils.compareArr_1(arr1, arr4));

            Assert.IsTrue(Utils.compareArr_2(arr1, arr2, false));
            Assert.IsTrue(Utils.compareArr_2(arr1, arr2, true));
            Assert.IsTrue(Utils.compareArr_2(arr1, arr3, false));
            Assert.IsFalse(Utils.compareArr_2(arr1, arr3, true));
            Assert.IsFalse(Utils.compareArr_2(arr1, arr4, false));
            Assert.IsFalse(Utils.compareArr_2(arr1, arr4, true));

        }
    }
}