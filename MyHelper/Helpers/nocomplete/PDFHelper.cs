// <summary>  
// Copyright：Sichen International Co. Ltd.
// Author：Devin
// Date：2016-09-02  
// Modifyed：selonsy  
// ModifyTime：2016-09-02  
// Desc：
// PDF帮助类
// </summary> 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace Devin
{
    /// <summary>
    /// PDF帮助类
    /// </summary>
    public class PDFHelper
    {
        /// <summary>
        /// 给PDF增加图片
        /// </summary>
        /// <param name="inputFile">原始pdf路径</param>
        /// <param name="outputFile">生成pdf路径</param>
        /// <param name="map">将要增加的图片</param>
        /// <param name="x">x轴偏移量</param>
        /// <param name="y">y轴偏移量</param>
        public static void AddImgToPdf(string inputFile, string outputFile,System.Drawing.Bitmap map,float x,float y)
        {
            Image img = Image.GetInstance(map, BaseColor.WHITE);
            img.SetAbsolutePosition(x, y);
            PdfReader pdfReader = new PdfReader(inputFile);
            using (FileStream fs = new FileStream(outputFile, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                using (PdfStamper stamper = new PdfStamper(pdfReader, fs))
                {
                    int PageCount = pdfReader.NumberOfPages;
                    for (int i = 1; i <= PageCount; i++)
                    {
                        PdfContentByte cb = stamper.GetUnderContent(i);
                        cb.AddImage(img);
                    }
                }
            }
        }
    }
}
