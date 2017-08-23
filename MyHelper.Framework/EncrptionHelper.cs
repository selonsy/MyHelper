// <summary>  
// Copyright：Sichen International Co. Ltd.
// Author：Devin
// Date：2017-3-20
// Modifyed：selonsy  
// ModifyTime：2017-3-20  
// Desc：
// 加密解密类
// </summary> 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Globalization;
#if NETFRAMEWORK
using System.Web.Security;
#endif
using System.Collections;

namespace Devin
{
    /// <summary>
    /// Encrption帮助类
    /// </summary>
    public abstract class EncrptionHelper
    {
        #region DES
         
        //示例:
        //加密：EncryptDES("要加密的字符串", "azjmerbv");
        //解密：DecryptDES("要解密的字符串", "azjmerbv");  

        #region Keys
        //默认密钥向量        
        private static byte[] Keys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
        #endregion

        /// DES加密字符串
        /// <summary>        
        /// DES加密字符串        
        /// </summary>        
        /// <param name="encryptString">待加密的字符串</param>       
        /// <param name="encryptKey">加密密钥,要求为8位</param>        
        /// <returns>加密成功返回加密后的字符串，失败返回源串</returns>        
        public static string EncryptDES(string encryptString, string encryptKey)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
                DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Convert.ToBase64String(mStream.ToArray());
            }
            catch
            {
                return encryptString;
            }
        }

        /// DES解密字符串
        /// <summary>        
        /// DES解密字符串        
        /// </summary>        
        /// <param name="decryptString">待解密的字符串</param>        
        /// <param name="decryptKey">解密密钥,要求为8位,和加密密钥相同</param>        
        /// <returns>解密成功返回解密后的字符串，失败返源串</returns>        
        public static string DecryptDES(string decryptString, string decryptKey)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(decryptKey);
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Convert.FromBase64String(decryptString);
                DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch { return decryptString; }
        }

        #endregion

        #region MD5
        
        /// <summary>
        /// MD5加密字符串
        /// </summary>
        /// <param name="encryptString"></param>
        /// <returns></returns>
        public static string EncryptMD5(string encryptString)
        {
            MD5 md5 = MD5.Create();
            byte[] data = md5.ComputeHash(Encoding.Default.GetBytes(encryptString));
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sb.Append(data[i].ToString("x2"));
            }
            return sb.ToString();                       
        }

#if NETFRAEWORK

        /// <summary>
        /// MD5加密字符串(已过时)
        /// </summary>
        /// <param name="encryptString"></param>
        /// <returns></returns>
        public static string EncryptMD5_1(string encryptString)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(encryptString, "md5");
        }

#elif NETSTANDARD

#endif

        /// <summary>
        /// MD5加密字符串
        /// </summary>
        /// <param name="encryptString"></param>
        /// <returns></returns>
        public static string EncryptMD5_2(string encryptString)
        {
            byte[] result = Encoding.Default.GetBytes(encryptString);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] output = md5.ComputeHash(result);
            return BitConverter.ToString(output).Replace("-", "");
        }

        /// <summary>
        /// MD5加密字符串
        /// </summary>
        /// <param name="encryptString"></param>
        /// <returns></returns>
        public static string EncryptMD5_3(string encryptString)
        {
            var md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            var result = BitConverter.ToString(md5.ComputeHash(UnicodeEncoding.UTF8.GetBytes(encryptString.Trim())));
            return result;
        }

#endregion
        
#region RSA

#region 公钥和私钥

        /// <summary>
        /// RSA公钥
        /// </summary>
        private static string RSAPublicKey = @"<RSAKeyValue><Modulus>puajh2nmtmaLPAQfrbtnEjlZylRDNmpgSmSnriPwK3nKrC06ZkjYsPnyw0Z3LXQw3OQyoH1R+d8/k68AKkbxv01elW9Xkg9eCqfLvUoJ46oX3CyRmtmp5jclrIT2upBXbjlAqBiRyEAsyLnwBtiG4qudNV+V1/YgV8dKen/jF8U=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>"
             ;
        /// <summary>
        /// RSA私钥
        /// </summary>
        private static string RSAPrivateKey = @"            <RSAKeyValue><Modulus>puajh2nmtmaLPAQfrbtnEjlZylRDNmpgSmSnriPwK3nKrC06ZkjYsPnyw0Z3LXQw3OQyoH1R+d8/k68AKkbxv01elW9Xkg9eCqfLvUoJ46oX3CyRmtmp5jclrIT2upBXbjlAqBiRyEAsyLnwBtiG4qudNV+V1/YgV8dKen/jF8U=</Modulus><Exponent>AQAB</Exponent><P>6d19htFXzFa0Rof2vlxuF5AuaCQ7m8ieBCHhr7ZRt02blUYireWv6Km2FwXNXn5mwpWaSlBJdTDoG9xLOmJYTw==</P><Q>trKd8DiYEeF8CHKIRGkxK0z7ulCwdB8pHDpCKwJN6w7bClFGfikFQkbbEdBPQmuFfiy9+cH3dQQa1y64yUp1qw==</Q><DP>KVnkjCYP5C2giqgX1Oj6/mecDQX7FDyQ1CI5iZwvLr4mOGrCTwVB6V24uLFp8r9e5CEXuvQfSP1wzRxSoOwquQ==</DP><DQ>nspWLZDy0y+4/xqUx2jqPGlXrRMumm7u60LTedAx6uhphuknLMX4dSdihnQYP3F2gDjhEW/95S2hKn/ISeO2qw==</DQ><InverseQ>q50M0lubWlnW1wX2dEFt8mhFI7E8jtfH0MfTkVqZlky2O7dG075Os+QawBevb0N3Q0npXRJtvEJZFfHUZrgHzA==</InverseQ><D>PqNItEWWhNYvYzLZAMbYhApqHHlLUFzUDpeAJubt+CMZYU0IIeMtEB5+cbM8fx6gSAaMyZa+8VxL7A09rqA5kcoNI9GAsWJopm22+fydLl7ftHn/Eco+xJQ0xsm9zTWLful4j1kMSFkr5F7crv54NjMcOOdel42toISX3Mv5Q/E=</D></RSAKeyValue>"
            ;

        //生成秘钥
        //using (RSACryptoServiceProvider rsa1 = new RSACryptoServiceProvider())
        //{
        //    string publicKey = rsa1.ToXmlString(false); // 公钥
        //    string privateKey = rsa1.ToXmlString(true); // 私钥和公钥
        //}

#endregion

        /// <summary>
        /// RSA加密
        /// </summary>
        /// <param name="content">要加密的文本</param>
        /// <returns>加密后的文本</returns>
        public static string EncryptRSA(string content)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            byte[] cipherbytes;
            rsa.FromXmlString(RSAPublicKey);
            cipherbytes = rsa.Encrypt(Encoding.UTF8.GetBytes(content), false);
            return Convert.ToBase64String(cipherbytes);            
        }

        /// <summary>
        /// RSA加密
        /// </summary>
        /// <param name="publickey">公钥</param>
        /// <param name="content">要加密的文本</param>
        /// <returns>加密后的文本</returns>
        public static string EncryptRSA(string publickey, string content)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            byte[] cipherbytes;
            rsa.FromXmlString(publickey);
            cipherbytes = rsa.Encrypt(Encoding.UTF8.GetBytes(content), false);
            return Convert.ToBase64String(cipherbytes);
        }

        /// <summary>
        /// RSA解密
        /// </summary>
        /// <param name="content">要解密的文本</param>
        /// <returns>解密后的文本</returns>
        public static string DecryptRSA(string content)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            byte[] cipherbytes;
            rsa.FromXmlString(RSAPrivateKey);
            cipherbytes = rsa.Decrypt(Convert.FromBase64String(content), false);
            return Encoding.UTF8.GetString(cipherbytes);
        }

        /// <summary>
        /// RSA解密
        /// </summary>
        /// <param name="privatekey">私钥</param>
        /// <param name="content">要解密的文本</param>
        /// <returns>解密后的文本</returns>
        public static string DecryptRSA(string privatekey, string content)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            byte[] cipherbytes;
            rsa.FromXmlString(privatekey);
            cipherbytes = rsa.Decrypt(Convert.FromBase64String(content), false);
            return Encoding.UTF8.GetString(cipherbytes);
        }

#endregion

#region Mysoft

        /// <summary>
        /// 明源的加密函数
        /// </summary>
        /// <param name="instr">待加密的字符串</param>
        /// <returns>加密后的字符串</returns>
        /// <example>
        /// 95938-6.707
        /// </example>
        public static string MyEncode(string instr)
        {
            string StrBuff = null;
            int IntLen = 0;
            int IntCode = 0;
            int IntCode1 = 0;
            int IntCode2 = 0;
            int IntCode3 = 0;
            int i = 0;

            IntLen = instr.Trim().Length;

            IntCode1 = IntLen % 3;
            IntCode2 = IntLen % 9;
            IntCode3 = IntLen % 5;
            IntCode = IntCode1 + IntCode3;

            for (i = 1; i <= IntLen; i++)
            {
                try
                {
                    System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
                    int intAsciiCode = (int)asciiEncoding.GetBytes(instr.Substring(IntLen - i, 1))[0];
                    StrBuff = StrBuff + Convert.ToChar(intAsciiCode - IntCode);
                    if (IntCode == IntCode1 + IntCode3)
                    {
                        IntCode = IntCode2 + IntCode3;
                    }
                    else
                    {
                        IntCode = IntCode1 + IntCode3;
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.WriteException(ex);
                }
            }
            return StrBuff;
        }

        /// <summary>
        /// 明源的解密函数
        /// </summary>
        /// <param name="inStr">待解密的字符串</param>
        /// <returns>解密后的字符串</returns>
        /// <example>
        /// 6.707-95938
        /// </example>
        public static string MyDecode(string inStr)
        {
            string strBuff;
            int intCode;
            int i;

            strBuff = "";

            var intLen = inStr.Trim(' ').Length;

            var intCode1 = intLen % 3;
            var intCode2 = intLen % 9;
            var intCode3 = intLen % 5;

            if (intLen / 2.0 == (int)Math.Floor(intLen / 2.0))
            {
                intCode = intCode2 + intCode3;
            }
            else
            {
                intCode = intCode1 + intCode3;
            }

            for (i = 1; i <= intLen; i++)
            {

                strBuff = strBuff + ((char)(Convert.ToInt32(inStr[intLen + 1 - i - 1]) + intCode)).ToString(CultureInfo.InvariantCulture);

                if (intCode == intCode1 + intCode3)
                {
                    intCode = intCode2 + intCode3;
                }
                else
                {
                    intCode = intCode1 + intCode3;
                }
            }

            return strBuff + new String(' ', inStr.Length - intLen);
        }

#endregion
    }

#region DES加密详解_测试 

    /// <summary>
    /// DES实体类，仅用于DESHelper的返回
    /// </summary>
    class DESEntity
    {
#region 属性定义
        /// <summary>
        /// 加密后的字符串
        /// </summary>
        public string EncrptedString { get; set; }
        /// <summary>
        /// 转化成字符串的Key
        /// </summary>
        public string keyStr { get; set; }
        /// <summary>
        /// 转化成字符串的IV
        /// </summary>
        public string IVstr { get; set; }
#endregion

#region 构造函数

#endregion
    }

    /// <summary>
    /// DES加密，不需自定义KEY值，需保存加密后的字符串以及KEY和IV串
    /// </summary>
    class DESHelper
    {
#region 加密知识
        /*
         加密方式：3重DES
         <生成key和IV>
            1、System.Security.Cryptography. TripleDESCryptoServiceProvider类是dotnet中实现TripleDES算法的主要的类。
            2、TripleDESCryptoServiceProvider类只有一个构造方法TripleDESCryptoServiceProvider（），这个方法把一些属性初始化：
                KeySize（加密密钥长度，以位为单位）= 192（24字节）
                BlockSize（加密处理的数据块大小，以位为单位）= 64（8字节）
                FeedbackSize（加密数据块后返回的数据大小，以位为单位）= 64（8字节）
            3、TripleDESCryptoServiceProvider构造方法同时会初始化一组随机的key和IV。
            默认的TripleDESCryptoServiceProvider的key为24字节，IV为8字节，加密数据块为8字节。
            
            生成key和IV的代码很简单：
            TripleDESCryptoServiceProvider tDESalg = new TripleDESCryptoServiceProvider();
            byte[] keyArray = tDESalg.Key;
            byte[] IVArray = tDESalg.IV;
            4、待加密的数据可能有两种形式，一种是二进制的数据，本身就是一组字节流，这样的数据可以跳过这一步，直接进入加密步骤。
              还有一种情况是字符串数据，字符串中同样的字符使用不同的代码页会生成不同的字节码，
              所以从字符串到字节流的转换是需要指定使用何种编码的。在解密之后，要从字节流转换到字符串就要使用相同的代码页解码，
              否则就会出现乱码。
        */
#endregion

#region 字段定义

#endregion

#region 属性定义
        /// <summary>
        /// 编码方式
        /// </summary>
        public static string EncodingType { get; set; }
        /// <summary>
        /// Key数组
        /// </summary>
        public byte[] keyArray { get; set; }
        /// <summary>
        /// IV数组
        /// </summary>
        public byte[] IVArray { get; set; }
#endregion

#region 构造方法
        /// <summary>
        /// 实例化EncryptionHelper对象
        /// </summary>
        public DESHelper()
        {
            TripleDESCryptoServiceProvider tDESalg = new TripleDESCryptoServiceProvider();
            keyArray = tDESalg.Key;
            IVArray = tDESalg.IV;
            EncodingType = "utf-8";
        }
        /// <summary>
        /// 实例化EncryptionHelper对象
        /// </summary>
        /// <param name="encodeType">编码方式</param>
        public DESHelper(string encodeType)
        {
            TripleDESCryptoServiceProvider tDESalg = new TripleDESCryptoServiceProvider();
            keyArray = tDESalg.Key;
            IVArray = tDESalg.IV;
            EncodingType = encodeType;
        }
#endregion

#region 加密和解密
        /// <summary>
        /// 加密方法
        /// </summary>
        /// <param name="str">待加密字符串</param>
        /// <returns></returns>
        public static DESEntity MyEncryption(string str)
        {
            //实例化EncryptionHelper
            DESHelper encryptionHelper = new DESHelper();

            //为EncryptString加密方法准备参数
            byte[] strArray = StrToBytes(str);
            byte[] keyArray = encryptionHelper.keyArray;
            byte[] ivArray = encryptionHelper.IVArray;

            //获取加密后的数据流
            byte[] getStream = EncryptString(strArray, keyArray, ivArray);

            //实例化EncrptionEntity对象
            DESEntity encrptionEntity = new DESEntity();

            //为encrptionEntity属性赋值
            encrptionEntity.EncrptedString = BytesArrayToString(getStream);
            encrptionEntity.keyStr = BytesArrayToString(encryptionHelper.keyArray);
            encrptionEntity.IVstr = BytesArrayToString(encryptionHelper.IVArray);

            //返回 
            return encrptionEntity;
        }

        /// <summary>
        /// 加密方法
        /// </summary>
        /// <param name="str">待加密字符串</param>
        /// <param name="encodeType">编码方式</param>
        /// <returns></returns>
        public static DESEntity MyEncryption(string str, string encodeType)
        {

            //实例化EncryptionHelper
            DESHelper encryptionHelper = new DESHelper(encodeType);

            //为EncryptString加密方法准备参数
            byte[] strArray = StrToBytes(str);
            byte[] keyArray = encryptionHelper.keyArray;
            byte[] ivArray = encryptionHelper.IVArray;

            //获取加密后的数据流
            byte[] getStream = EncryptString(strArray, keyArray, ivArray);

            //实例化EncrptionEntity对象
            DESEntity encrptionEntity = new DESEntity();

            //为encrptionEntity属性赋值
            encrptionEntity.EncrptedString = BytesArrayToString(getStream);
            encrptionEntity.IVstr = BytesArrayToString(encryptionHelper.keyArray);
            encrptionEntity.keyStr = BytesArrayToString(encryptionHelper.IVArray);

            //返回 
            return encrptionEntity;
        }

        /// <summary>
        /// 解密方法
        /// </summary>
        /// <param name="str">待解密字符串</param>
        /// <param name="key">Key</param>
        /// <param name="iv">IV</param>
        /// <returns></returns>
        public static string MyDecryption(string str, string key, string iv)
        {
            //为DecryptTextFromMemory解密方法准备参数
            byte[] strArray = StringToBytesArray(str);
            byte[] keyArray = StringToBytesArray(key);
            byte[] ivArray = StringToBytesArray(iv);

            //获取解密后的数据流
            byte[] getStream = DecryptTextFromMemory(strArray, keyArray, ivArray);
            //转化为字符串
            string result = BytesToStr(getStream);
            //返回
            return result;
        }

#endregion

#region 编码和解码
        /// <summary>
        /// 将字符串编码为字节流-系统自带
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte[] StrToBytes(string str)
        {
            Encoding sEncoding = Encoding.GetEncoding(EncodingType);
            return sEncoding.GetBytes(str);
        }
        /// <summary>
        /// 将字节流编码为字符串-系统自带
        /// </summary>
        /// <param name="strArray"></param>
        /// <returns></returns>
        public static string BytesToStr(byte[] strArray)
        {
            Encoding sEncoding = Encoding.GetEncoding(EncodingType);
            return sEncoding.GetString(strArray);
        }
        /// <summary>
        /// 将字节流数组编码为字符串-数字
        /// </summary>
        /// <returns></returns>
        public static string BytesArrayToString(byte[] strArray)
        {
            return string.Join(",", strArray);
        }
        /// <summary>
        /// 将字符串转化为字节流数组-数字
        /// </summary>
        /// <returns></returns>
        public static byte[] StringToBytesArray(string str)
        {
            string[] strTemp = str.Split(',');
            byte[] result = new byte[strTemp.Length];
            for (int i = 0; i < strTemp.Length; i++)
            {
                result[i] = Convert.ToByte(strTemp[i]);
            }
            return result;
        }
#endregion

#region 加密底层处理过程

        /// <summary>
        /// 将一个明文的二进制流转换成一个加密的二进制流
        /// </summary>
        /// <param name="strArray">一个明文的二进制数据流，其实也就是你要加密的字符串的二进制形式的数据流</param>
        /// <param name="Key"></param>
        /// <param name="IV"></param>
        /// <returns>返回一个加密后是二进制数据流</returns>
        /// <remarks>
        /// 加密的原料是明文字节流，TripleDES算法对字节流进行加密，返回的是加密后的字节流,同时要给定加密使用的key和IV。
        /// 把字符串明文转换成utf-8编码的字节流。
        /// </remarks>
        public static byte[] EncryptString(byte[] strArray, byte[] Key, byte[] IV)
        {
            //建立一个MemoryStream,这里面存放加密后的数据流
            MemoryStream mStream = new MemoryStream();

            //使用MemoryStream和key,IV新建一个CryptoStream对象  
            CryptoStream cStream = new CryptoStream(mStream, new TripleDESCryptoServiceProvider().CreateEncryptor(Key, IV), CryptoStreamMode.Write);

            //将加密后的字节流写入到MemoryStream
            cStream.Write(strArray, 0, strArray.Length);

            //把缓冲区中的最后状态更新到MemoryStream，并清除cStream的缓存区
            cStream.FlushFinalBlock();

            // 把加密后的数据流转成字节流
            byte[] ret = mStream.ToArray();

            //关闭两个streams
            cStream.Close();
            mStream.Close();

            //返回值
            return ret;
        }
#endregion

#region 解密底层处理过程
        /// <summary>
        /// 将一个加密后的二进制数据流进行解密，产生一个明文的二进制数据流
        /// </summary>
        /// <param name="EncryptedDataArray">加密后的数据流</param>
        /// <param name="Key"></param>
        /// <param name="IV"></param>
        /// <returns>一个已经解密的二进制流</returns>
        /// <remarks>
        /// 解密操作解密上面步骤生成的密文byte[]，需要使用到加密步骤使用的同一组Key和IV。
        /// </remarks>
        public static byte[] DecryptTextFromMemory(byte[] EncryptedDataArray, byte[] Key, byte[] IV)
        {

            // 建立一个MemoryStream，这里面存放加密后的数据流
            MemoryStream msDecrypt = new MemoryStream(EncryptedDataArray);

            // 使用MemoryStream 和key、IV新建一个CryptoStream 对象
            CryptoStream csDecrypt = new CryptoStream(msDecrypt, new TripleDESCryptoServiceProvider().CreateDecryptor(Key, IV), CryptoStreamMode.Read);

            // 根据密文byte[]的长度（可能比加密前的明文长），新建一个存放解密后明文的byte[]
            byte[] DecryptDataArray = new byte[EncryptedDataArray.Length];

            // 把解密后的数据读入到DecryptDataArray
            csDecrypt.Read(DecryptDataArray, 0, DecryptDataArray.Length);

            //关闭两个streams
            msDecrypt.Close();
            csDecrypt.Close();

            //返回值
            return DecryptDataArray;
        }
#endregion
    }

#endregion
}