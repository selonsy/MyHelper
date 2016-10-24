// <summary>  
// Copyright：Sichen International Co. Ltd.
// Author：Devin
// Date：2016-09-07  
// Modifyed：selonsy  
// ModifyTime：2016-09-07  
// Desc：
// 图片处理类
// </summary>  

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devin
{
    /// <summary>
    /// 图片处理类
    /// </summary>
    public class PicHelper
    {
        /// <summary>
        /// 图片转化为字节数组
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public byte[] Image2ByteArray(string filePath)
        {
            //将图片以文件流的形式进行保存
            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read); 
            BinaryReader br = new BinaryReader(fs);
            //将流读入到字节数组中
            byte[] imgBytesIn = br.ReadBytes((int)fs.Length);  
            return imgBytesIn;
        }

        /// <summary>
        /// 图片转化为字符串
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public string Image2ByteString(string filePath)
        {
            //将图片以文件流的形式进行保存
            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            //将流读入到字节数组中
            byte[] imgBytesIn = br.ReadBytes((int)fs.Length);
            string imgString = System.Text.Encoding.UTF8.GetString(imgBytesIn);
            return imgString;
        }

        /// <summary>
        /// 字节数组转化为图片
        /// </summary>
        /// <param name="imgBytesIn"></param>
        /// <returns></returns>
        public Image ByteArray2Image(byte[] imgBytesIn)
        {
            MemoryStream ms = new MemoryStream(imgBytesIn);
            return Image.FromStream(ms);
        }

        /// <summary>
        /// 字节串转化为图片
        /// </summary>
        /// <param name="imgBytesIn"></param>
        /// <returns></returns>
        public Image ByteString2Image(string imgBytesIn)
        {
            byte[] imgByte = System.Text.Encoding.Unicode.GetBytes(imgBytesIn);
            MemoryStream ms = new MemoryStream(imgByte);
            return Image.FromStream(ms);
        }


        //#region 将图片从数据库中读取
        ///// <summary>
        ///// 将图片从数据库中读取
        ///// </summary>
        ///// <param name="xs_ID">要读取图片的学号</param>
        ///// <param name="ph">pictureBox1控件名</param>
        //public void get_photo(string xs_ID, PictureBox ph)//将图片从数据库中读取
        //{
        //    byte[] imagebytes = null;
        //    getcon();
        //    SqlCommand con = new SqlCommand("select * from S_jiben where S_num='" + xs_ID + "'", link);
        //    SqlDataReader dr = con.ExecuteReader();
        //    while (dr.Read())
        //    {
        //        imagebytes = (byte[])dr.GetValue(18);
        //    }
        //    dr.Close();
        //    con_close();
        //    MemoryStream ms = new MemoryStream(imagebytes);
        //    Bitmap bmpt = new Bitmap(ms);
        //    ph.Image = bmpt;
        //}
        //#endregion
        //#region
        //public void SaveImage(string MID, OpenFileDialog openF)//将图片以二进制存入数据库中
        //{
        //    string strimg = openF.FileName.ToString();  //记录图片的所在路径
        //    FileStream fs = new FileStream(strimg, FileMode.Open, FileAccess.Read); //将图片以文件流的形式进行保存
        //    BinaryReader br = new BinaryReader(fs);
        //    byte[] imgBytesIn = br.ReadBytes((int)fs.Length);  //将流读入到字节数组中
        //    getcon();
        //    StringBuilder strSql = new StringBuilder();
        //    strSql.Append("update S_jiben Set xs_photo=@Photo where S_num=" + MID);
        //    SqlCommand cmd = new SqlCommand(strSql.ToString(), link);
        //    cmd.Parameters.Add("@Photo", SqlDbType.Binary).Value = imgBytesIn;
        //    cmd.ExecuteNonQuery();
        //    con_close();
        //}
        //#endregion
    }
}
