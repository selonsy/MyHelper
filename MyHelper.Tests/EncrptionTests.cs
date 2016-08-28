using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Devin;
namespace Devin.Tests
{
    /// <summary>
    /// 
    /// </summary>
    [TestClass()]
    public class EncrptionTests
    {     
        /// <summary>
        /// RSA加解密
        /// </summary>
        [TestMethod()]
        public void EncryptRSATest()
        {
            string content = "沈金龙";            
            string jiami = EncrptionHelper.EncryptRSA("", content);
            string jiemi = EncrptionHelper.DecryptRSA("", jiami);
            Assert.AreEqual(jiemi, content);            
        }

        /// <summary>
        /// Mysoft加解密
        /// </summary>
        [TestMethod]
        public void EncryptMyTest()
        {
            string content = "沈金龙";
            string jiami = EncrptionHelper.MyEncode(content);
            string jiemi = EncrptionHelper.MyDecode(jiami);
            Assert.AreEqual(jiemi, content);

            string content1 = "Mysoft95938";
            string expected1 = "50626qclpvJ";
            string actual1 = EncrptionHelper.MyEncode(content1);
            Assert.AreEqual(expected1, actual1);

            string content2 = "6.707";
            string expected2 = "95938";
            string actual2 = EncrptionHelper.MyDecode(content2);
            Assert.AreEqual(expected2, actual2);           
        }
    }
}

