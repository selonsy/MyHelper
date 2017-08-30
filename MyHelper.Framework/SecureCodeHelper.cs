using System;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Drawing.Text;
using System.Drawing.Imaging;
using System.Collections;

namespace Devin
{
    /// <summary>
    /// 随机字符类
    /// </summary>
    public class Rand
    {
        #region private

        /// <summary>
        /// 组合规则
        /// </summary>
        private enum 组合规则
        {
            数字 = 1,
            小写字母 = 2,
            大写字母 = 3,
            大小写字母混合 = 4,
            小写字母和数字混合 = 5,
            大写字母和数字混合 = 6,
            大小写字母和数字混合 = 7
        }

        /// <summary>
        /// 随机种子
        /// </summary>
        private Random seed;

        /// <summary>
        /// 原始字符串
        /// </summary>
        private string original_str = "0123456789abcdefghigklmnopqrstuvwxyzABCDEFGHIGKLMNOPQRSTUVWXYZ";

        /// <summary>
        /// 获取一个随机字符
        /// </summary>
        /// <param name="rnd"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private string getRandomChar(Random rnd, 组合规则 type)
        {
            string strChar = string.Empty;
            switch (type)
            {
                case 组合规则.数字:
                    strChar = rnd.Next(10).ToString();
                    break;
                case 组合规则.小写字母:
                    strChar = original_str.Substring(10 + rnd.Next(26), 1);
                    break;
                case 组合规则.大写字母:
                    strChar = original_str.Substring(36 + rnd.Next(26), 1);
                    break;
                case 组合规则.大小写字母混合:
                    strChar = original_str.Substring(10 + rnd.Next(52), 1);
                    break;
                case 组合规则.小写字母和数字混合:
                    strChar = original_str.Substring(0 + rnd.Next(36), 1);
                    break;
                case 组合规则.大写字母和数字混合:
                    strChar = original_str.Substring(0 + rnd.Next(36), 1).ToUpper();
                    break;
                case 组合规则.大小写字母和数字混合:
                    strChar = original_str.Substring(0 + rnd.Next(61), 1);
                    break;
            }
            return strChar;
        }

        #endregion

        #region 构造函数

        public Rand(int rand)
        {
            seed = new Random(rand);
        }
        public Rand()
        {
            seed = new Random(new Random().Next(100000));
        }

        #endregion

        /// <summary>
        /// 获取指定长度字符串(大小写字母和数字混合)
        /// </summary>
        /// <param name="length">字符串长度</param>
        /// <returns></returns>
        public string GetRandomStr(int strLength)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < strLength; i++)
            {
                sb.Append(getRandomChar(seed, 组合规则.大小写字母和数字混合));
            }
            return sb.ToString();
        }

        /// <summary>
        /// 获取指定长度字符串
        /// </summary>
        /// <param name="intLength">数字长度</param>
        /// <param name="lowerLength">小写字母长度</param>
        /// <param name="upperLength">大写字母长度</param>
        /// <returns></returns>
        public string GetRandomStr(int intLength, int lowerLength, int upperLength)
        {
            List<string> str_list = new List<string>();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < intLength; i++)
            {
                str_list.Add(getRandomChar(seed, 组合规则.数字));
            }
            for (int i = 0; i < lowerLength; i++)
            {
                str_list.Add(getRandomChar(seed, 组合规则.小写字母));
            }
            for (int i = 0; i < upperLength; i++)
            {
                str_list.Add(getRandomChar(seed, 组合规则.大写字母));
            }
            return string.Join("", str_list.OrderBy(r => Guid.NewGuid()).ToList());
        }

    }

    /// <summary>
    /// 验证图片类
    /// </summary>
    public class SecureCodeHelper
    {
        public string Text { get; set; }

        public byte[] Image_Bytes { get; set; }

        public Image Image
        {
            get
            {
                if (this.Image_Bytes.Length > 0)
                {
                    MemoryStream ms = new MemoryStream(this.Image_Bytes);
                    return Image.FromStream(ms);
                }
                else return null;
            }
        }

        public SecureCodeHelper()
        {
            string code = new Rand().GetRandomStr(4, 0, 0);
            this.Text = code;
            this.Image_Bytes = new ValidateCode_Style0().CreateImage(code);
        }

        public SecureCodeHelper(string text)
        {
            this.Text = text;
            this.Image_Bytes = new ValidateCode_Style0().CreateImage(text);
        }

        public SecureCodeHelper(ValidateCodeStyle type)
        {
            string text = string.Empty;
            var validatecode_style_obj = ReflectionHelper.CreateClassInstance("Devin", $"ValidateCode_Style{type.ToInt()}", this.GetType().Assembly.FullName);
            if (validatecode_style_obj.IsNotNull())
            {
                ValidateCodeType obj = validatecode_style_obj as ValidateCodeType;
                this.Image_Bytes = obj.CreateImage(out text);
                this.Text = text;
            }
        }
    }

    #region 验证码

    /// <summary>
    /// 图片验证码抽象类
    /// </summary>
    public abstract class ValidateCodeType
    {
        protected ValidateCodeType()
        {
        }

        public abstract byte[] CreateImage(out string resultCode);

        public abstract string Name { get; }

        public virtual string Tip
        {
            get
            {
                return "请输入图片中的字符";
            }
        }

        public string Type
        {
            get
            {
                return base.GetType().Name;
            }
        }
    }

    /// <summary>
    /// 验证码样式
    /// </summary>
    public enum ValidateCodeStyle
    {
        线条干扰_蓝色 = 1,
        噪点干扰_蓝色 = 2,
        GIF颠簸动画 = 3,
        GIF闪烁动画_蓝色 = 4,
        噪点干扰_扭曲 = 5,
        中文_蓝色 = 6,
        二年级算术_蓝色 = 7,
        噪点干扰_彩色 = 8,
        线条干扰_彩色 = 9,
        GIF闪烁动画_彩色 = 10,
        中文_彩色 = 11,
        字体旋转_简单 = 12,
        二年级算术_彩色 = 13
    }

    /// <summary>
    /// 默认(支持自定义验证码文字)
    /// </summary>
    public class ValidateCode_Style0
    {
        #region 私有字段
        private int letterWidth = 16;  //单个字体的宽度范围
        private int letterHeight = 20; //单个字体的高度范围
        private static byte[] randb = new byte[4];
        private static RNGCryptoServiceProvider rand = new RNGCryptoServiceProvider();
        private Font[] fonts =
        {
           new Font(new FontFamily("Times New Roman"),10 +Next(1),System.Drawing.FontStyle.Regular),
           new Font(new FontFamily("Georgia"), 10 + Next(1),System.Drawing.FontStyle.Regular),
           new Font(new FontFamily("Arial"), 10 + Next(1),System.Drawing.FontStyle.Regular),
           new Font(new FontFamily("Comic Sans MS"), 10 + Next(1),System.Drawing.FontStyle.Regular)
        };
        #endregion

        public byte[] CreateImage(string validataCode)
        {
            int int_ImageWidth = validataCode.Length * letterWidth;
            Bitmap image = new Bitmap(int_ImageWidth, letterHeight);
            Graphics g = Graphics.FromImage(image);
            g.Clear(Color.White);
            for (int i = 0; i < 2; i++)
            {
                int x1 = Next(image.Width - 1);
                int x2 = Next(image.Width - 1);
                int y1 = Next(image.Height - 1);
                int y2 = Next(image.Height - 1);
                g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
            }
            int _x = -12, _y = 0;
            for (int int_index = 0; int_index < validataCode.Length; int_index++)
            {
                _x += Next(12, 16);
                _y = Next(-2, 2);
                string str_char = validataCode.Substring(int_index, 1);
                str_char = Next(1) == 1 ? str_char.ToLower() : str_char.ToUpper();
                Brush newBrush = new SolidBrush(GetRandomColor());
                Point thePos = new Point(_x, _y);
                g.DrawString(str_char, fonts[Next(fonts.Length - 1)], newBrush, thePos);
            }
            for (int i = 0; i < 10; i++)
            {
                int x = Next(image.Width - 1);
                int y = Next(image.Height - 1);
                image.SetPixel(x, y, Color.FromArgb(Next(0, 255), Next(0, 255), Next(0, 255)));
            }
            image = TwistImage(image, true, Next(1, 3), Next(4, 6));
            g.DrawRectangle(new Pen(Color.LightGray, 1), 0, 0, int_ImageWidth - 1, (letterHeight - 1));

            MemoryStream stream = new MemoryStream();
            image.Save(stream, ImageFormat.Png);
            image.Dispose();
            image = null;
            stream.Close();
            stream.Dispose();
            return stream.GetBuffer();
        }

        #region 私有方法

        /// <summary>
        /// 字体随机颜色
        /// </summary>
        private Color GetRandomColor()
        {
            Random RandomNum_First = new Random((int)DateTime.Now.Ticks);
            System.Threading.Thread.Sleep(RandomNum_First.Next(50));
            Random RandomNum_Sencond = new Random((int)DateTime.Now.Ticks);
            int int_Red = RandomNum_First.Next(180);
            int int_Green = RandomNum_Sencond.Next(180);
            int int_Blue = (int_Red + int_Green > 300) ? 0 : 400 - int_Red - int_Green;
            int_Blue = (int_Blue > 255) ? 255 : int_Blue;
            return Color.FromArgb(int_Red, int_Green, int_Blue);
        }

        /// <summary>
        /// 正弦曲线Wave扭曲图片
        /// </summary>
        /// <param name="srcBmp">图片路径</param>
        /// <param name="bXDir">如果扭曲则选择为True</param>
        /// <param name="nMultValue">波形的幅度倍数，越大扭曲的程度越高,一般为3</param>
        /// <param name="dPhase">波形的起始相位,取值区间[0-2*PI)</param>
        private Bitmap TwistImage(Bitmap srcBmp, bool bXDir, double dMultValue, double dPhase)
        {
            double PI = 6.283185307179586476925286766559;
            Bitmap destBmp = new Bitmap(srcBmp.Width, srcBmp.Height);
            Graphics graph = Graphics.FromImage(destBmp);
            graph.FillRectangle(new SolidBrush(Color.White), 0, 0, destBmp.Width, destBmp.Height);
            graph.Dispose();
            double dBaseAxisLen = bXDir ? (double)destBmp.Height : (double)destBmp.Width;
            for (int i = 0; i < destBmp.Width; i++)
            {
                for (int j = 0; j < destBmp.Height; j++)
                {
                    double dx = 0;
                    dx = bXDir ? (PI * (double)j) / dBaseAxisLen : (PI * (double)i) / dBaseAxisLen;
                    dx += dPhase;
                    double dy = Math.Sin(dx);
                    int nOldX = 0, nOldY = 0;
                    nOldX = bXDir ? i + (int)(dy * dMultValue) : i;
                    nOldY = bXDir ? j : j + (int)(dy * dMultValue);

                    Color color = srcBmp.GetPixel(i, j);
                    if (nOldX >= 0 && nOldX < destBmp.Width
                     && nOldY >= 0 && nOldY < destBmp.Height)
                    {
                        destBmp.SetPixel(nOldX, nOldY, color);
                    }
                }
            }
            srcBmp.Dispose();
            return destBmp;
        }

        /// <summary>
        /// 获得下一个随机数
        /// </summary>
        /// <param name="max">最大值</param>
        private static int Next(int max)
        {
            rand.GetBytes(randb);
            int value = BitConverter.ToInt32(randb, 0);
            value = value % (max + 1);
            if (value < 0) value = -value;
            return value;
        }

        /// <summary>
        /// 获得下一个随机数
        /// </summary>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        private static int Next(int min, int max)
        {
            int value = Next(max - min) + min;
            return value;
        }

        #endregion
    }

    /// <summary>
    /// 线条干扰(蓝色)
    /// </summary>
    public class ValidateCode_Style1 : ValidateCodeType
    {
        private Color backgroundColor = Color.White;
        private bool chaos = true;
        private Color chaosColor = Color.FromArgb(170, 170, 0x33);
        private Color drawColor = Color.FromArgb(50, 0x99, 0xcc);
        private bool fontTextRenderingHint;
        private int imageHeight = 30;
        private int padding = 1;
        private int validataCodeLength = 4;
        private int validataCodeSize = 0x10;
        private string validateCodeFont = "Arial";

        public override byte[] CreateImage(out string validataCode)
        {
            Bitmap bitmap;
            string formatString = "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z";
            GetRandom(formatString, this.ValidataCodeLength, out validataCode);
            MemoryStream stream = new MemoryStream();
            this.ImageBmp(out bitmap, validataCode);
            bitmap.Save(stream, ImageFormat.Png);
            bitmap.Dispose();
            bitmap = null;
            stream.Close();
            stream.Dispose();
            return stream.GetBuffer();
        }

        private void CreateImageBmp(ref Bitmap bitMap, string validateCode)
        {
            Graphics graphics = Graphics.FromImage(bitMap);
            if (this.fontTextRenderingHint)
            {
                graphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixel;
            }
            else
            {
                graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            }
            Font font = new Font(this.validateCodeFont, (float)this.validataCodeSize, FontStyle.Regular);
            Brush brush = new SolidBrush(this.drawColor);
            int maxValue = Math.Max((this.ImageHeight - this.validataCodeSize) - 5, 0);
            Random random = new Random();
            for (int i = 0; i < this.validataCodeLength; i++)
            {
                int[] numArray = new int[] { ((i * this.validataCodeSize) + random.Next(1)) + 3, random.Next(maxValue) - 4 };
                Point point = new Point(numArray[0], numArray[1]);
                graphics.DrawString(validateCode[i].ToString(), font, brush, (PointF)point);
            }
            graphics.Dispose();
        }

        private void DisposeImageBmp(ref Bitmap bitmap)
        {
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.Clear(Color.White);
            Pen pen = new Pen(this.DrawColor, 1f);
            new Random();
            Point[] pointArray = new Point[2];
            Random random = new Random();
            if (this.Chaos)
            {
                pen = new Pen(this.ChaosColor, 1f);
                for (int i = 0; i < (this.validataCodeLength * 2); i++)
                {
                    pointArray[0] = new Point(random.Next(bitmap.Width), random.Next(bitmap.Height));
                    pointArray[1] = new Point(random.Next(bitmap.Width), random.Next(bitmap.Height));
                    graphics.DrawLine(pen, pointArray[0], pointArray[1]);
                }
            }
            graphics.Dispose();
        }

        private static void GetRandom(string formatString, int len, out string codeString)
        {
            codeString = string.Empty;
            string[] strArray = formatString.Split(new char[] { ',' });
            Random random = new Random();
            for (int i = 0; i < len; i++)
            {
                int index = random.Next(0x186a0) % strArray.Length;
                codeString = codeString + strArray[index].ToString();
            }
        }

        private void ImageBmp(out Bitmap bitMap, string validataCode)
        {
            int width = (int)(((this.validataCodeLength * this.validataCodeSize) * 1.3) + 4.0);
            bitMap = new Bitmap(width, this.ImageHeight);
            this.DisposeImageBmp(ref bitMap);
            this.CreateImageBmp(ref bitMap, validataCode);
        }

        public Color BackgroundColor
        {
            get
            {
                return this.backgroundColor;
            }
            set
            {
                this.backgroundColor = value;
            }
        }

        public bool Chaos
        {
            get
            {
                return this.chaos;
            }
            set
            {
                this.chaos = value;
            }
        }

        public Color ChaosColor
        {
            get
            {
                return this.chaosColor;
            }
            set
            {
                this.chaosColor = value;
            }
        }

        public Color DrawColor
        {
            get
            {
                return this.drawColor;
            }
            set
            {
                this.drawColor = value;
            }
        }

        private bool FontTextRenderingHint
        {
            get
            {
                return this.fontTextRenderingHint;
            }
            set
            {
                this.fontTextRenderingHint = value;
            }
        }

        public int ImageHeight
        {
            get
            {
                return this.imageHeight;
            }
            set
            {
                this.imageHeight = value;
            }
        }

        public override string Name
        {
            get
            {
                return "线条干扰(蓝色)";
            }
        }

        public int Padding
        {
            get
            {
                return this.padding;
            }
            set
            {
                this.padding = value;
            }
        }

        public int ValidataCodeLength
        {
            get
            {
                return this.validataCodeLength;
            }
            set
            {
                this.validataCodeLength = value;
            }
        }

        public int ValidataCodeSize
        {
            get
            {
                return this.validataCodeSize;
            }
            set
            {
                this.validataCodeSize = value;
            }
        }

        public string ValidateCodeFont
        {
            get
            {
                return this.validateCodeFont;
            }
            set
            {
                this.validateCodeFont = value;
            }
        }
    }

    /// <summary>
    /// 噪点干扰(蓝色)
    /// </summary>
    public class ValidateCode_Style2 : ValidateCodeType
    {
        private Color backgroundColor = Color.White;
        private Color chaosColor = Color.FromArgb(170, 170, 0x33);
        private Color drawColor = Color.FromArgb(50, 0x99, 0xcc);
        private bool fontTextRenderingHint;
        private int imageHeight = 30;
        private int padding = 1;
        private int validataCodeLength = 4;
        private int validataCodeSize = 0x10;
        private string validateCodeFont = "Arial";

        public override byte[] CreateImage(out string validataCode)
        {
            Bitmap bitmap;
            string formatString = "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z";
            GetRandom(formatString, this.ValidataCodeLength, out validataCode);
            MemoryStream stream = new MemoryStream();
            this.ImageBmp(out bitmap, validataCode);
            bitmap.Save(stream, ImageFormat.Png);
            bitmap.Dispose();
            bitmap = null;
            stream.Close();
            stream.Dispose();
            return stream.GetBuffer();
        }

        private void CreateImageBmp(ref Bitmap bitMap, string validateCode)
        {
            Graphics graphics = Graphics.FromImage(bitMap);
            if (this.fontTextRenderingHint)
            {
                graphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixel;
            }
            else
            {
                graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            }
            Font font = new Font(this.validateCodeFont, (float)this.validataCodeSize, FontStyle.Regular);
            Brush brush = new SolidBrush(this.drawColor);
            int maxValue = Math.Max((this.ImageHeight - this.validataCodeSize) - 5, 0);
            Random random = new Random();
            for (int i = 0; i < this.validataCodeLength; i++)
            {
                int[] numArray = new int[] { ((i * this.validataCodeSize) + random.Next(1)) + 3, random.Next(maxValue) - 4 };
                Point point = new Point(numArray[0], numArray[1]);
                graphics.DrawString(validateCode[i].ToString(), font, brush, (PointF)point);
            }
            graphics.Dispose();
        }

        private void DisposeImageBmp(ref Bitmap bitmap)
        {
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.Clear(Color.White);
            Pen pen = new Pen(this.DrawColor, 1f);
            Random random = new Random();
            pen = new Pen(this.ChaosColor, 1f);
            for (int i = 0; i < (this.validataCodeLength * 10); i++)
            {
                int x = random.Next(bitmap.Width);
                int y = random.Next(bitmap.Height);
                graphics.DrawRectangle(pen, x, y, 1, 1);
            }
            graphics.Dispose();
        }

        private static void GetRandom(string formatString, int len, out string codeString)
        {
            codeString = string.Empty;
            string[] strArray = formatString.Split(new char[] { ',' });
            Random random = new Random();
            for (int i = 0; i < len; i++)
            {
                int index = random.Next(0x186a0) % strArray.Length;
                codeString = codeString + strArray[index].ToString();
            }
        }

        private void ImageBmp(out Bitmap bitMap, string validataCode)
        {
            int width = (int)(((this.validataCodeLength * this.validataCodeSize) * 1.3) + 4.0);
            bitMap = new Bitmap(width, this.ImageHeight);
            this.DisposeImageBmp(ref bitMap);
            this.CreateImageBmp(ref bitMap, validataCode);
        }

        public Color BackgroundColor
        {
            get
            {
                return this.backgroundColor;
            }
            set
            {
                this.backgroundColor = value;
            }
        }

        public Color ChaosColor
        {
            get
            {
                return this.chaosColor;
            }
            set
            {
                this.chaosColor = value;
            }
        }

        public Color DrawColor
        {
            get
            {
                return this.drawColor;
            }
            set
            {
                this.drawColor = value;
            }
        }

        private bool FontTextRenderingHint
        {
            get
            {
                return this.fontTextRenderingHint;
            }
            set
            {
                this.fontTextRenderingHint = value;
            }
        }

        public int ImageHeight
        {
            get
            {
                return this.imageHeight;
            }
            set
            {
                this.imageHeight = value;
            }
        }

        public override string Name
        {
            get
            {
                return "噪点干扰(蓝色)";
            }
        }

        public int Padding
        {
            get
            {
                return this.padding;
            }
            set
            {
                this.padding = value;
            }
        }

        public int ValidataCodeLength
        {
            get
            {
                return this.validataCodeLength;
            }
            set
            {
                this.validataCodeLength = value;
            }
        }

        public int ValidataCodeSize
        {
            get
            {
                return this.validataCodeSize;
            }
            set
            {
                this.validataCodeSize = value;
            }
        }

        public string ValidateCodeFont
        {
            get
            {
                return this.validateCodeFont;
            }
            set
            {
                this.validateCodeFont = value;
            }
        }
    }

    /// <summary>
    /// GIF颠簸动画
    /// </summary>
    public class ValidateCode_Style3 : ValidateCodeType
    {
        private Color backgroundColor = Color.White;
        private bool chaos = true;
        private Color chaosColor = Color.FromArgb(170, 170, 0x33);
        private int chaosMode = 1;
        private int contortRange = 4;
        private Color drawColor = Color.FromArgb(50, 0x99, 0xcc);
        private bool fontTextRenderingHint;
        private int imageHeight = 30;
        private int padding = 1;
        private const double PI = 3.1415926535897931;
        private const double PI2 = 6.2831853071795862;
        private int validataCodeLength = 4;
        private int validataCodeSize = 0x10;
        private string validateCodeFont = "Arial";

        public override byte[] CreateImage(out string validataCode)
        {
            Bitmap bitmap;
            string formatString = "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z";
            GetRandom(formatString, this.ValidataCodeLength, out validataCode);
            MemoryStream stream = new MemoryStream();
            AnimatedGifEncoder encoder = new AnimatedGifEncoder();
            encoder.Start();
            encoder.SetDelay(1);
            encoder.SetRepeat(0);
            for (int i = 0; i < 3; i++)
            {
                this.SplitCode(validataCode);
                this.ImageBmp(out bitmap, validataCode);
                bitmap.Save(stream, ImageFormat.Png);
                encoder.AddFrame(Image.FromStream(stream));
                stream = new MemoryStream();
                bitmap.Dispose();
            }
            encoder.OutPut(ref stream);
            bitmap = null;
            stream.Close();
            stream.Dispose();
            return stream.GetBuffer();
        }

        private void CreateImageBmp(ref Bitmap bitMap, string validateCode)
        {
            Graphics graphics = Graphics.FromImage(bitMap);
            if (this.fontTextRenderingHint)
            {
                graphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixel;
            }
            else
            {
                graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            }
            Font font = new Font(this.validateCodeFont, (float)this.validataCodeSize, FontStyle.Regular);
            Brush brush = new SolidBrush(this.drawColor);
            int maxValue = Math.Max((this.ImageHeight - this.validataCodeSize) - 5, 0);
            Random random = new Random();
            for (int i = 0; i < this.validataCodeLength; i++)
            {
                int[] numArray = new int[] { ((i * this.validataCodeSize) + random.Next(1)) + 3, random.Next(maxValue) - 4 };
                Point point = new Point(numArray[0], numArray[1]);
                graphics.DrawString(validateCode[i].ToString(), font, brush, (PointF)point);
            }
            graphics.Dispose();
        }

        private void DisposeImageBmp(ref Bitmap bitmap)
        {
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.Clear(Color.White);
            Pen pen = new Pen(this.DrawColor, 1f);
            Random random = new Random();
            Point[] pointArray = new Point[2];
            if (this.Chaos)
            {
                switch (this.chaosMode)
                {
                    case 1:
                        pen = new Pen(this.ChaosColor, 1f);
                        for (int i = 0; i < (this.validataCodeLength * 10); i++)
                        {
                            int x = random.Next(bitmap.Width);
                            int y = random.Next(bitmap.Height);
                            graphics.DrawRectangle(pen, x, y, 1, 1);
                        }
                        break;

                    case 2:
                        pen = new Pen(this.ChaosColor, (float)(this.validataCodeLength * 4));
                        for (int j = 0; j < (this.validataCodeLength * 10); j++)
                        {
                            int num5 = random.Next(bitmap.Width);
                            int num6 = random.Next(bitmap.Height);
                            graphics.DrawRectangle(pen, num5, num6, 1, 1);
                        }
                        break;

                    case 3:
                        pen = new Pen(this.ChaosColor, 1f);
                        for (int k = 0; k < (this.validataCodeLength * 2); k++)
                        {
                            pointArray[0] = new Point(random.Next(bitmap.Width), random.Next(bitmap.Height));
                            pointArray[1] = new Point(random.Next(bitmap.Width), random.Next(bitmap.Height));
                            graphics.DrawLine(pen, pointArray[0], pointArray[1]);
                        }
                        break;

                    default:
                        pen = new Pen(this.ChaosColor, 1f);
                        for (int m = 0; m < (this.validataCodeLength * 10); m++)
                        {
                            int num9 = random.Next(bitmap.Width);
                            int num10 = random.Next(bitmap.Height);
                            graphics.DrawRectangle(pen, num9, num10, 1, 1);
                        }
                        break;
                }
            }
            graphics.Dispose();
        }

        private static void GetRandom(string formatString, int len, out string codeString)
        {
            codeString = string.Empty;
            string[] strArray = formatString.Split(new char[] { ',' });
            Random random = new Random();
            for (int i = 0; i < len; i++)
            {
                int index = random.Next(0x186a0) % strArray.Length;
                codeString = codeString + strArray[index].ToString();
            }
        }

        private void ImageBmp(out Bitmap bitMap, string validataCode)
        {
            int width = (int)(((this.validataCodeLength * this.validataCodeSize) * 1.3) + 4.0);
            bitMap = new Bitmap(width, this.ImageHeight);
            this.DisposeImageBmp(ref bitMap);
            this.CreateImageBmp(ref bitMap, validataCode);
        }

        private string[] SplitCode(string srcCode)
        {
            Random random = new Random();
            string[] strArray = new string[2];
            foreach (char ch in srcCode)
            {
                if ((random.Next(Math.Abs((int)DateTime.Now.Ticks)) % 2) == 0)
                {
                    string[] strArray2;
                    string[] strArray3;
                    (strArray2 = strArray)[0] = strArray2[0] + ch.ToString();
                    (strArray3 = strArray)[1] = strArray3[1] + " ";
                }
                else
                {
                    string[] strArray4;
                    string[] strArray5;
                    (strArray4 = strArray)[1] = strArray4[1] + ch.ToString();
                    (strArray5 = strArray)[0] = strArray5[0] + " ";
                }
            }
            return strArray;
        }

        public Bitmap TwistImage(Bitmap srcBmp, bool bXDir, double dMultValue, double dPhase)
        {
            Bitmap image = new Bitmap(srcBmp.Width, srcBmp.Height);
            Graphics graphics = Graphics.FromImage(image);
            graphics.FillRectangle(new SolidBrush(Color.White), 0, 0, image.Width, image.Height);
            graphics.Dispose();
            double num = bXDir ? ((double)image.Height) : ((double)image.Width);
            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    double a = 0.0;
                    a = bXDir ? ((6.2831853071795862 * j) / num) : ((6.2831853071795862 * i) / num);
                    a += dPhase;
                    double num5 = Math.Sin(a);
                    int x = 0;
                    int y = 0;
                    x = bXDir ? (i + ((int)(num5 * dMultValue))) : i;
                    y = bXDir ? j : (j + ((int)(num5 * dMultValue)));
                    Color pixel = srcBmp.GetPixel(i, j);
                    if (((x >= 0) && (x < image.Width)) && ((y >= 0) && (y < image.Height)))
                    {
                        image.SetPixel(x, y, pixel);
                    }
                }
            }
            return image;
        }

        public Color BackgroundColor
        {
            get
            {
                return this.backgroundColor;
            }
            set
            {
                this.backgroundColor = value;
            }
        }

        public bool Chaos
        {
            get
            {
                return this.chaos;
            }
            set
            {
                this.chaos = value;
            }
        }

        public Color ChaosColor
        {
            get
            {
                return this.chaosColor;
            }
            set
            {
                this.chaosColor = value;
            }
        }

        public int ChaosMode
        {
            get
            {
                return this.chaosMode;
            }
            set
            {
                this.chaosMode = value;
            }
        }

        public int ContortRange
        {
            get
            {
                return this.contortRange;
            }
            set
            {
                this.contortRange = value;
            }
        }

        public Color DrawColor
        {
            get
            {
                return this.drawColor;
            }
            set
            {
                this.drawColor = value;
            }
        }

        private bool FontTextRenderingHint
        {
            get
            {
                return this.fontTextRenderingHint;
            }
            set
            {
                this.fontTextRenderingHint = value;
            }
        }

        public int ImageHeight
        {
            get
            {
                return this.imageHeight;
            }
            set
            {
                this.imageHeight = value;
            }
        }

        public override string Name
        {
            get
            {
                return "GIF颠簸动画";
            }
        }

        public int Padding
        {
            get
            {
                return this.padding;
            }
            set
            {
                this.padding = value;
            }
        }

        public int ValidataCodeLength
        {
            get
            {
                return this.validataCodeLength;
            }
            set
            {
                this.validataCodeLength = value;
            }
        }

        public int ValidataCodeSize
        {
            get
            {
                return this.validataCodeSize;
            }
            set
            {
                this.validataCodeSize = value;
            }
        }

        public string ValidateCodeFont
        {
            get
            {
                return this.validateCodeFont;
            }
            set
            {
                this.validateCodeFont = value;
            }
        }
    }

    /// <summary>
    /// GIF闪烁动画(蓝色)   
    /// </summary>
    public class ValidateCode_Style4 : ValidateCodeType
    {
        private Color backgroundColor = Color.White;
        private bool chaos = true;
        private Color chaosColor = Color.FromArgb(170, 170, 0x33);
        private int chaosMode = 1;
        private int contortRange = 4;
        private Color drawColor = Color.FromArgb(50, 0x99, 0xcc);
        private bool fontTextRenderingHint;
        private int imageHeight = 30;
        private int padding = 1;
        private const double PI = 3.1415926535897931;
        private const double PI2 = 6.2831853071795862;
        private int validataCodeLength = 4;
        private int validataCodeSize = 0x10;
        private string validateCodeFont = "Arial";

        public override byte[] CreateImage(out string validataCode)
        {
            Bitmap bitmap;
            string formatString = "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z";
            GetRandom(formatString, this.ValidataCodeLength, out validataCode);
            MemoryStream stream = new MemoryStream();
            AnimatedGifEncoder encoder = new AnimatedGifEncoder();
            encoder.Start();
            encoder.SetDelay(1);
            encoder.SetRepeat(0);
            for (int i = 0; i < 3; i++)
            {
                string[] strArray = this.SplitCode(validataCode);
                for (int j = 0; j < 2; j++)
                {
                    if (j == 0)
                    {
                        this.ImageBmp(out bitmap, strArray[0]);
                    }
                    else
                    {
                        this.ImageBmp(out bitmap, strArray[1]);
                    }
                    bitmap.Save(stream, ImageFormat.Png);
                    encoder.AddFrame(Image.FromStream(stream));
                    stream = new MemoryStream();
                    bitmap.Dispose();
                }
            }
            encoder.OutPut(ref stream);
            bitmap = null;
            stream.Close();
            stream.Dispose();
            return stream.GetBuffer();
        }

        private void CreateImageBmp(ref Bitmap bitMap, string validateCode)
        {
            Graphics graphics = Graphics.FromImage(bitMap);
            if (this.fontTextRenderingHint)
            {
                graphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixel;
            }
            else
            {
                graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            }
            Font font = new Font(this.validateCodeFont, (float)this.validataCodeSize, FontStyle.Regular);
            Brush brush = new SolidBrush(this.drawColor);
            int maxValue = Math.Max((this.ImageHeight - this.validataCodeSize) - 4, 0);
            Random random = new Random();
            for (int i = 0; i < this.validataCodeLength; i++)
            {
                int[] numArray = new int[] { ((i * this.validataCodeSize) + random.Next(1)) + 3, random.Next(maxValue) - 4 };
                Point point = new Point(numArray[0], numArray[1]);
                graphics.DrawString(validateCode[i].ToString(), font, brush, (PointF)point);
            }
            graphics.Dispose();
        }

        private void DisposeImageBmp(ref Bitmap bitmap)
        {
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.Clear(Color.White);
            Pen pen = new Pen(this.DrawColor, 1f);
            Random random = new Random();
            Point[] pointArray = new Point[2];
            if (this.Chaos)
            {
                switch (this.chaosMode)
                {
                    case 1:
                        pen = new Pen(this.ChaosColor, 1f);
                        for (int i = 0; i < (this.validataCodeLength * 10); i++)
                        {
                            int x = random.Next(bitmap.Width);
                            int y = random.Next(bitmap.Height);
                            graphics.DrawRectangle(pen, x, y, 1, 1);
                        }
                        break;

                    case 2:
                        pen = new Pen(this.ChaosColor, (float)(this.validataCodeLength * 4));
                        for (int j = 0; j < (this.validataCodeLength * 10); j++)
                        {
                            int num5 = random.Next(bitmap.Width);
                            int num6 = random.Next(bitmap.Height);
                            graphics.DrawRectangle(pen, num5, num6, 1, 1);
                        }
                        break;

                    case 3:
                        pen = new Pen(this.ChaosColor, 1f);
                        for (int k = 0; k < (this.validataCodeLength * 2); k++)
                        {
                            pointArray[0] = new Point(random.Next(bitmap.Width), random.Next(bitmap.Height));
                            pointArray[1] = new Point(random.Next(bitmap.Width), random.Next(bitmap.Height));
                            graphics.DrawLine(pen, pointArray[0], pointArray[1]);
                        }
                        break;

                    default:
                        pen = new Pen(this.ChaosColor, 1f);
                        for (int m = 0; m < (this.validataCodeLength * 10); m++)
                        {
                            int num9 = random.Next(bitmap.Width);
                            int num10 = random.Next(bitmap.Height);
                            graphics.DrawRectangle(pen, num9, num10, 1, 1);
                        }
                        break;
                }
            }
            bitmap = this.TwistImage(bitmap, true, (double)this.contortRange, 6.0);
            graphics.Dispose();
        }

        private static void GetRandom(string formatString, int len, out string codeString)
        {
            codeString = string.Empty;
            string[] strArray = formatString.Split(new char[] { ',' });
            Random random = new Random();
            for (int i = 0; i < len; i++)
            {
                int index = random.Next(0x186a0) % strArray.Length;
                codeString = codeString + strArray[index].ToString();
            }
        }

        private void ImageBmp(out Bitmap bitMap, string validataCode)
        {
            int width = (int)(((this.validataCodeLength * this.validataCodeSize) * 1.3) + 4.0);
            bitMap = new Bitmap(width, this.ImageHeight);
            this.DisposeImageBmp(ref bitMap);
            this.CreateImageBmp(ref bitMap, validataCode);
        }

        private string[] SplitCode(string srcCode)
        {
            Random random = new Random();
            string[] strArray = new string[2];
            foreach (char ch in srcCode)
            {
                if ((random.Next(Math.Abs((int)DateTime.Now.Ticks)) % 2) == 0)
                {
                    string[] strArray2;
                    string[] strArray3;
                    (strArray2 = strArray)[0] = strArray2[0] + ch.ToString();
                    (strArray3 = strArray)[1] = strArray3[1] + " ";
                }
                else
                {
                    string[] strArray4;
                    string[] strArray5;
                    (strArray4 = strArray)[1] = strArray4[1] + ch.ToString();
                    (strArray5 = strArray)[0] = strArray5[0] + " ";
                }
            }
            return strArray;
        }

        public Bitmap TwistImage(Bitmap srcBmp, bool bXDir, double dMultValue, double dPhase)
        {
            Bitmap image = new Bitmap(srcBmp.Width, srcBmp.Height);
            Graphics graphics = Graphics.FromImage(image);
            graphics.FillRectangle(new SolidBrush(Color.White), 0, 0, image.Width, image.Height);
            graphics.Dispose();
            double num = bXDir ? ((double)image.Height) : ((double)image.Width);
            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    double a = 0.0;
                    a = bXDir ? ((6.2831853071795862 * j) / num) : ((6.2831853071795862 * i) / num);
                    a += dPhase;
                    double num5 = Math.Sin(a);
                    int x = 0;
                    int y = 0;
                    x = bXDir ? (i + ((int)(num5 * dMultValue))) : i;
                    y = bXDir ? j : (j + ((int)(num5 * dMultValue)));
                    Color pixel = srcBmp.GetPixel(i, j);
                    if (((x >= 0) && (x < image.Width)) && ((y >= 0) && (y < image.Height)))
                    {
                        image.SetPixel(x, y, pixel);
                    }
                }
            }
            return image;
        }

        public Color BackgroundColor
        {
            get
            {
                return this.backgroundColor;
            }
            set
            {
                this.backgroundColor = value;
            }
        }

        public bool Chaos
        {
            get
            {
                return this.chaos;
            }
            set
            {
                this.chaos = value;
            }
        }

        public Color ChaosColor
        {
            get
            {
                return this.chaosColor;
            }
            set
            {
                this.chaosColor = value;
            }
        }

        public int ChaosMode
        {
            get
            {
                return this.chaosMode;
            }
            set
            {
                this.chaosMode = value;
            }
        }

        public int ContortRange
        {
            get
            {
                return this.contortRange;
            }
            set
            {
                this.contortRange = value;
            }
        }

        public Color DrawColor
        {
            get
            {
                return this.drawColor;
            }
            set
            {
                this.drawColor = value;
            }
        }

        private bool FontTextRenderingHint
        {
            get
            {
                return this.fontTextRenderingHint;
            }
            set
            {
                this.fontTextRenderingHint = value;
            }
        }

        public int ImageHeight
        {
            get
            {
                return this.imageHeight;
            }
            set
            {
                this.imageHeight = value;
            }
        }

        public override string Name
        {
            get
            {
                return "GIF闪烁动画(蓝色)";
            }
        }

        public int Padding
        {
            get
            {
                return this.padding;
            }
            set
            {
                this.padding = value;
            }
        }

        public int ValidataCodeLength
        {
            get
            {
                return this.validataCodeLength;
            }
            set
            {
                this.validataCodeLength = value;
            }
        }

        public int ValidataCodeSize
        {
            get
            {
                return this.validataCodeSize;
            }
            set
            {
                this.validataCodeSize = value;
            }
        }

        public string ValidateCodeFont
        {
            get
            {
                return this.validateCodeFont;
            }
            set
            {
                this.validateCodeFont = value;
            }
        }
    }

    /// <summary>
    /// 噪点干扰(扭曲) 
    /// </summary>
    public class ValidateCode_Style5 : ValidateCodeType
    {
        private Color backgroundColor = Color.White;
        private bool chaos = true;
        private Color chaosColor = Color.FromArgb(170, 170, 0x33);
        private int chaosMode = 1;
        private int contortRange = 4;
        private Color drawColor = Color.FromArgb(50, 0x99, 0xcc);
        private bool fontTextRenderingHint;
        private int imageHeight = 30;
        private int padding = 1;
        private const double PI = 3.1415926535897931;
        private const double PI2 = 6.2831853071795862;
        private int validataCodeLength = 4;
        private int validataCodeSize = 0x10;
        private string validateCodeFont = "Arial";

        public override byte[] CreateImage(out string validataCode)
        {
            Bitmap bitmap;
            string formatString = "1,2,3,4,5,6,7,8,9";
            GetRandom(formatString, this.ValidataCodeLength, out validataCode);
            MemoryStream stream = new MemoryStream();
            this.ImageBmp(out bitmap, validataCode);
            bitmap.Save(stream, ImageFormat.Png);
            bitmap.Dispose();
            bitmap = null;
            stream.Close();
            stream.Dispose();
            return stream.GetBuffer();
        }

        private void CreateImageBmp(ref Bitmap bitMap, string validateCode)
        {
            Graphics graphics = Graphics.FromImage(bitMap);
            if (this.fontTextRenderingHint)
            {
                graphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixel;
            }
            else
            {
                graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            }
            Font font = new Font(this.validateCodeFont, (float)this.validataCodeSize, FontStyle.Regular);
            Brush brush = new SolidBrush(this.drawColor);
            int maxValue = Math.Max((this.ImageHeight - this.validataCodeSize) - 5, 0);
            Random random = new Random();
            for (int i = 0; i < this.validataCodeLength; i++)
            {
                int[] numArray = new int[] { ((i * this.validataCodeSize) + random.Next(1)) + 3, random.Next(maxValue) - 4 };
                Point point = new Point(numArray[0], numArray[1]);
                graphics.DrawString(validateCode[i].ToString(), font, brush, (PointF)point);
            }
            graphics.Dispose();
        }

        private void DisposeImageBmp(ref Bitmap bitmap)
        {
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.Clear(Color.White);
            Pen pen = new Pen(this.DrawColor, 1f);
            Random random = new Random();
            Point[] pointArray = new Point[2];
            if (this.Chaos)
            {
                switch (this.chaosMode)
                {
                    case 1:
                        pen = new Pen(this.ChaosColor, 1f);
                        for (int i = 0; i < (this.validataCodeLength * 10); i++)
                        {
                            int x = random.Next(bitmap.Width);
                            int y = random.Next(bitmap.Height);
                            graphics.DrawRectangle(pen, x, y, 1, 1);
                        }
                        break;

                    case 2:
                        pen = new Pen(this.ChaosColor, (float)(this.validataCodeLength * 4));
                        for (int j = 0; j < (this.validataCodeLength * 10); j++)
                        {
                            int num5 = random.Next(bitmap.Width);
                            int num6 = random.Next(bitmap.Height);
                            graphics.DrawRectangle(pen, num5, num6, 1, 1);
                        }
                        break;

                    case 3:
                        pen = new Pen(this.ChaosColor, 1f);
                        for (int k = 0; k < (this.validataCodeLength * 2); k++)
                        {
                            pointArray[0] = new Point(random.Next(bitmap.Width), random.Next(bitmap.Height));
                            pointArray[1] = new Point(random.Next(bitmap.Width), random.Next(bitmap.Height));
                            graphics.DrawLine(pen, pointArray[0], pointArray[1]);
                        }
                        break;

                    default:
                        pen = new Pen(this.ChaosColor, 1f);
                        for (int m = 0; m < (this.validataCodeLength * 10); m++)
                        {
                            int num9 = random.Next(bitmap.Width);
                            int num10 = random.Next(bitmap.Height);
                            graphics.DrawRectangle(pen, num9, num10, 1, 1);
                        }
                        break;
                }
            }
            graphics.Dispose();
        }

        private static void GetRandom(string formatString, int len, out string codeString)
        {
            codeString = string.Empty;
            string[] strArray = formatString.Split(new char[] { ',' });
            Random random = new Random();
            for (int i = 0; i < len; i++)
            {
                int index = random.Next(0x186a0) % strArray.Length;
                codeString = codeString + strArray[index].ToString();
            }
        }

        private void ImageBmp(out Bitmap bitMap, string validataCode)
        {
            int width = (int)(((this.validataCodeLength * this.validataCodeSize) * 1.3) + 4.0);
            bitMap = new Bitmap(width, this.ImageHeight);
            this.DisposeImageBmp(ref bitMap);
            this.CreateImageBmp(ref bitMap, validataCode);
            bitMap = this.TwistImage(bitMap, true, (double)this.contortRange, 6.0);
        }

        private string[] SplitCode(string srcCode)
        {
            Random random = new Random();
            string[] strArray = new string[2];
            foreach (char ch in srcCode)
            {
                if ((random.Next(Math.Abs((int)DateTime.Now.Ticks)) % 2) == 0)
                {
                    string[] strArray2;
                    string[] strArray3;
                    (strArray2 = strArray)[0] = strArray2[0] + ch.ToString();
                    (strArray3 = strArray)[1] = strArray3[1] + " ";
                }
                else
                {
                    string[] strArray4;
                    string[] strArray5;
                    (strArray4 = strArray)[1] = strArray4[1] + ch.ToString();
                    (strArray5 = strArray)[0] = strArray5[0] + " ";
                }
            }
            return strArray;
        }

        public Bitmap TwistImage(Bitmap srcBmp, bool bXDir, double dMultValue, double dPhase)
        {
            Bitmap image = new Bitmap(srcBmp.Width, srcBmp.Height);
            Graphics graphics = Graphics.FromImage(image);
            graphics.FillRectangle(new SolidBrush(Color.White), 0, 0, image.Width, image.Height);
            graphics.Dispose();
            double num = bXDir ? ((double)image.Height) : ((double)image.Width);
            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    double a = 0.0;
                    a = bXDir ? ((6.2831853071795862 * j) / num) : ((6.2831853071795862 * i) / num);
                    a += dPhase;
                    double num5 = Math.Sin(a);
                    int x = 0;
                    int y = 0;
                    x = bXDir ? (i + ((int)(num5 * dMultValue))) : i;
                    y = bXDir ? j : (j + ((int)(num5 * dMultValue)));
                    Color pixel = srcBmp.GetPixel(i, j);
                    if (((x >= 0) && (x < image.Width)) && ((y >= 0) && (y < image.Height)))
                    {
                        image.SetPixel(x, y, pixel);
                    }
                }
            }
            return image;
        }

        public Color BackgroundColor
        {
            get
            {
                return this.backgroundColor;
            }
            set
            {
                this.backgroundColor = value;
            }
        }

        public bool Chaos
        {
            get
            {
                return this.chaos;
            }
            set
            {
                this.chaos = value;
            }
        }

        public Color ChaosColor
        {
            get
            {
                return this.chaosColor;
            }
            set
            {
                this.chaosColor = value;
            }
        }

        public int ChaosMode
        {
            get
            {
                return this.chaosMode;
            }
            set
            {
                this.chaosMode = value;
            }
        }

        public int ContortRange
        {
            get
            {
                return this.contortRange;
            }
            set
            {
                this.contortRange = value;
            }
        }

        public Color DrawColor
        {
            get
            {
                return this.drawColor;
            }
            set
            {
                this.drawColor = value;
            }
        }

        private bool FontTextRenderingHint
        {
            get
            {
                return this.fontTextRenderingHint;
            }
            set
            {
                this.fontTextRenderingHint = value;
            }
        }

        public int ImageHeight
        {
            get
            {
                return this.imageHeight;
            }
            set
            {
                this.imageHeight = value;
            }
        }

        public override string Name
        {
            get
            {
                return "噪点干扰(扭曲)";
            }
        }

        public int Padding
        {
            get
            {
                return this.padding;
            }
            set
            {
                this.padding = value;
            }
        }

        public int ValidataCodeLength
        {
            get
            {
                return this.validataCodeLength;
            }
            set
            {
                this.validataCodeLength = value;
            }
        }

        public int ValidataCodeSize
        {
            get
            {
                return this.validataCodeSize;
            }
            set
            {
                this.validataCodeSize = value;
            }
        }

        public string ValidateCodeFont
        {
            get
            {
                return this.validateCodeFont;
            }
            set
            {
                this.validateCodeFont = value;
            }
        }
    }

    /// <summary>
    /// 中文(蓝色)
    /// </summary>
    public class ValidateCode_Style6 : ValidateCodeType
    {
        private Color backgroundColor = Color.White;
        private Color chaosColor = Color.FromArgb(170, 170, 0x33);
        private Color drawColor = Color.FromArgb(50, 0x99, 0xcc);
        private bool fontTextRenderingHint;
        private int imageHeight = 30;
        private int padding = 1;
        private int validataCodeLength = 4;
        private int validataCodeSize = 0x10;
        private string validateCodeFont = "Arial";

        public override byte[] CreateImage(out string validataCode)
        {
            Bitmap bitmap;
            string formatString = "丰,王,井,开,夫,天,无,元,专,云,扎,艺,木,五,支,厅,不,太,犬,区,历,尤,友,匹,车,巨,牙,屯,比,互,切,瓦,止,少,日,中,冈,贝,内,水,见,午,牛,手,毛,气,升,长,仁,什,片,仆,化,仇,币,仍,仅,斤,爪,反,介,父,从,今,凶,分,乏,公,仓,月,氏,勿,欠,风,丹,匀,乌,凤,勾,文,六,方,火,为,斗,忆,订,计,户,认,心,尺,引,丑,巴,孔,队,办,以,允,予,劝,双,书,幻,玉,刊,示,末,未,击,打,巧,正,扑,扒,功,扔,去,甘,世,古,节,本,术,可,丙,左,厉,右,石,布,龙,平,灭,轧,东,卡,北,占,业,旧,帅,归,且,旦,奏,春,帮,珍,玻,毒,型,挂,封,持,项,垮,挎,城,挠,政,赴,赵,挡,挺,括,拴,拾,挑,指,垫,挣,挤,拼,挖,按,挥,挪,某,甚,革,荐,巷,带,草,茧,茶,荒,茫,荡,荣,故,胡,南,药,标,枯,柄,栋,相,查,柏,柳,柱,柿,栏,树,要,咸,威,歪,研,砖,厘,厚,砌,砍,面,耐,耍,牵,残,殃,轻,鸦,皆,背,战,点,临,览,竖,省,削,尝,是,盼,眨,哄,显,哑,冒,映,星,昨,畏,趴,胃,贵,界,虹,虾,蚁,思,蚂,虽,品,咽,骂,哗,咱,响,哈,咬,咳,哪,炭,峡,罚,贱,贴,骨,钞,钟,钢,钥,钩,卸,缸,拜,看,矩,怎,牲,选,适,秒,香,种,秋,科,重,复,竿,段,便,俩,贷,顺,修,保,促,侮,俭,俗,俘,信,皇,泉,鬼,侵,追,俊,盾,待,律,很,须,叙,剑,逃,食,盆,胆,胜,胞,胖,脉,勉,狭,狮,独,狡,狱,狠,贸,怨,急,饶,蚀,饺,饼,弯,将,奖,哀,亭,亮,度,迹,庭,疮,疯,疫,疤,姿,亲,音,帝,施,闻,阀,阁,差,养,美,姜,叛,送,类,迷,前,首,逆,总,炼,炸,炮,烂,剃,洁,洪,洒,浇,浊,洞,测,洗,活,派,洽,染,济,洋,洲,浑,浓,津,恒,恢,恰,恼,恨,举,觉,宣,室,宫,宪,突,穿,窃,客,冠,语,扁,袄,祖,神,祝,误,诱,说,诵,垦,退,既,屋,昼,费,陡,眉,孩,除,险,院,娃,姥,姨,姻,娇,怒,架,贺,盈,勇,怠,柔,垒,绑,绒,结,绕,骄,绘,给,络,骆,绝,绞,统";
            GetRandom(formatString, this.ValidataCodeLength, out validataCode);
            MemoryStream stream = new MemoryStream();
            this.ImageBmp(out bitmap, validataCode);
            bitmap.Save(stream, ImageFormat.Png);
            bitmap.Dispose();
            bitmap = null;
            stream.Close();
            stream.Dispose();
            return stream.GetBuffer();
        }

        private void CreateImageBmp(ref Bitmap bitMap, string validateCode)
        {
            Graphics graphics = Graphics.FromImage(bitMap);
            if (this.fontTextRenderingHint)
            {
                graphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixel;
            }
            else
            {
                graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            }
            Font font = new Font(this.validateCodeFont, (float)this.validataCodeSize, FontStyle.Regular);
            Brush brush = new SolidBrush(this.drawColor);
            int maxValue = Math.Max((this.ImageHeight - this.validataCodeSize) - 5, 0);
            Random random = new Random();
            for (int i = 0; i < this.validataCodeLength; i++)
            {
                int[] numArray = new int[] { (i * this.validataCodeSize) + (i * 5), random.Next(maxValue) };
                Point point = new Point(numArray[0], numArray[1]);
                graphics.DrawString(validateCode[i].ToString(), font, brush, (PointF)point);
            }
            graphics.Dispose();
        }

        private void DisposeImageBmp(ref Bitmap bitmap)
        {
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.Clear(Color.White);
            Pen pen = new Pen(this.DrawColor, 1f);
            Random random = new Random();
            pen = new Pen(this.ChaosColor, 1f);
            for (int i = 0; i < (this.validataCodeLength * 10); i++)
            {
                int x = random.Next(bitmap.Width);
                int y = random.Next(bitmap.Height);
                graphics.DrawRectangle(pen, x, y, 1, 1);
            }
            graphics.Dispose();
        }

        private static void GetRandom(string formatString, int len, out string codeString)
        {
            codeString = string.Empty;
            string[] strArray = formatString.Split(new char[] { ',' });
            Random random = new Random();
            for (int i = 0; i < len; i++)
            {
                int index = random.Next(0x186a0) % strArray.Length;
                codeString = codeString + strArray[index].ToString();
            }
        }

        private void ImageBmp(out Bitmap bitMap, string validataCode)
        {
            int width = (int)(((this.validataCodeLength * this.validataCodeSize) * 1.3) + 10.0);
            bitMap = new Bitmap(width, this.ImageHeight);
            this.DisposeImageBmp(ref bitMap);
            this.CreateImageBmp(ref bitMap, validataCode);
        }

        public Color BackgroundColor
        {
            get
            {
                return this.backgroundColor;
            }
            set
            {
                this.backgroundColor = value;
            }
        }

        public Color ChaosColor
        {
            get
            {
                return this.chaosColor;
            }
            set
            {
                this.chaosColor = value;
            }
        }

        public Color DrawColor
        {
            get
            {
                return this.drawColor;
            }
            set
            {
                this.drawColor = value;
            }
        }

        private bool FontTextRenderingHint
        {
            get
            {
                return this.fontTextRenderingHint;
            }
            set
            {
                this.fontTextRenderingHint = value;
            }
        }

        public int ImageHeight
        {
            get
            {
                return this.imageHeight;
            }
            set
            {
                this.imageHeight = value;
            }
        }

        public override string Name
        {
            get
            {
                return "中文(蓝色)";
            }
        }

        public int Padding
        {
            get
            {
                return this.padding;
            }
            set
            {
                this.padding = value;
            }
        }

        public int ValidataCodeLength
        {
            get
            {
                return this.validataCodeLength;
            }
            set
            {
                this.validataCodeLength = value;
            }
        }

        public int ValidataCodeSize
        {
            get
            {
                return this.validataCodeSize;
            }
            set
            {
                this.validataCodeSize = value;
            }
        }

        public string ValidateCodeFont
        {
            get
            {
                return this.validateCodeFont;
            }
            set
            {
                this.validateCodeFont = value;
            }
        }
    }

    /// <summary>
    /// 2年级算术(蓝色) 
    /// </summary>
    public class ValidateCode_Style7 : ValidateCodeType
    {
        private Color backgroundColor = Color.White;
        private Color chaosColor = Color.FromArgb(170, 170, 0x33);
        private Color drawColor = Color.FromArgb(50, 0x99, 0xcc);
        private bool fontTextRenderingHint;
        private int imageHeight = 30;
        private int padding = 1;
        private int validataCodeLength = 5;
        private int validataCodeSize = 0x10;
        private string validateCodeFont = "Arial";

        public override byte[] CreateImage(out string resultCode)
        {
            string str2;
            Bitmap bitmap;
            string formatString = "1,2,3,4,5,6,7,8,9,0";
            GetRandom(formatString, out str2, out resultCode);
            MemoryStream stream = new MemoryStream();
            this.ImageBmp(out bitmap, str2);
            bitmap.Save(stream, ImageFormat.Png);
            bitmap.Dispose();
            bitmap = null;
            stream.Close();
            stream.Dispose();
            return stream.GetBuffer();
        }

        private void CreateImageBmp(ref Bitmap bitMap, string validateCode)
        {
            Graphics graphics = Graphics.FromImage(bitMap);
            if (this.fontTextRenderingHint)
            {
                graphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixel;
            }
            else
            {
                graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            }
            Font font = new Font(this.validateCodeFont, (float)this.validataCodeSize, FontStyle.Regular);
            Brush brush = new SolidBrush(this.drawColor);
            int maxValue = Math.Max((this.ImageHeight - this.validataCodeSize) - 5, 0);
            Random random = new Random();
            for (int i = 0; i < this.validataCodeLength; i++)
            {
                int[] numArray = new int[] { (i * this.validataCodeSize) + (i * 5), random.Next(maxValue) };
                Point point = new Point(numArray[0], numArray[1]);
                graphics.DrawString(validateCode[i].ToString(), font, brush, (PointF)point);
            }
            graphics.Dispose();
        }

        private void DisposeImageBmp(ref Bitmap bitmap)
        {
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.Clear(Color.White);
            Pen pen = new Pen(this.DrawColor, 1f);
            Random random = new Random();
            pen = new Pen(this.ChaosColor, 1f);
            for (int i = 0; i < (this.validataCodeLength * 10); i++)
            {
                int x = random.Next(bitmap.Width);
                int y = random.Next(bitmap.Height);
                graphics.DrawRectangle(pen, x, y, 1, 1);
            }
            graphics.Dispose();
        }

        private static void GetRandom(string formatString, out string codeString, out string resultCode)
        {
            Random random = new Random();
            string s = string.Empty;
            string str2 = string.Empty;
            string[] strArray = formatString.Split(new char[] { ',' });
            for (int i = 0; i < 2; i++)
            {
                int index = random.Next(strArray.Length);
                if ((i == 0) && (strArray[index] == "0"))
                {
                    i--;
                }
                else
                {
                    s = s + strArray[index].ToString();
                }
            }
            for (int j = 0; j < 2; j++)
            {
                int num4 = random.Next(strArray.Length);
                if ((j == 0) && (strArray[num4] == "0"))
                {
                    j--;
                }
                else
                {
                    str2 = str2 + strArray[num4].ToString();
                }
            }
            if ((random.Next(100) % 2) == 1)
            {
                codeString = s + "+" + str2;
                resultCode = (int.Parse(s) + int.Parse(str2)).ToString();
            }
            else
            {
                if (int.Parse(s) > int.Parse(str2))
                {
                    codeString = s + "─" + str2;
                }
                else
                {
                    codeString = str2 + "─" + s;
                }
                resultCode = Math.Abs((int)(int.Parse(s) - int.Parse(str2))).ToString();
            }
        }

        private void ImageBmp(out Bitmap bitMap, string validataCode)
        {
            int width = (int)(((this.validataCodeLength * this.validataCodeSize) * 1.3) + 10.0);
            bitMap = new Bitmap(width, this.ImageHeight);
            this.DisposeImageBmp(ref bitMap);
            this.CreateImageBmp(ref bitMap, validataCode);
        }

        public Color BackgroundColor
        {
            get
            {
                return this.backgroundColor;
            }
            set
            {
                this.backgroundColor = value;
            }
        }

        public Color ChaosColor
        {
            get
            {
                return this.chaosColor;
            }
            set
            {
                this.chaosColor = value;
            }
        }

        public Color DrawColor
        {
            get
            {
                return this.drawColor;
            }
            set
            {
                this.drawColor = value;
            }
        }

        private bool FontTextRenderingHint
        {
            get
            {
                return this.fontTextRenderingHint;
            }
            set
            {
                this.fontTextRenderingHint = value;
            }
        }

        public int ImageHeight
        {
            get
            {
                return this.imageHeight;
            }
            set
            {
                this.imageHeight = value;
            }
        }

        public override string Name
        {
            get
            {
                return "2年级算术(蓝色)";
            }
        }

        public int Padding
        {
            get
            {
                return this.padding;
            }
            set
            {
                this.padding = value;
            }
        }

        public override string Tip
        {
            get
            {
                return "输入计算结果";
            }
        }

        public int ValidataCodeLength
        {
            get
            {
                return this.validataCodeLength;
            }
            set
            {
                this.validataCodeLength = value;
            }
        }

        public int ValidataCodeSize
        {
            get
            {
                return this.validataCodeSize;
            }
            set
            {
                this.validataCodeSize = value;
            }
        }

        public string ValidateCodeFont
        {
            get
            {
                return this.validateCodeFont;
            }
            set
            {
                this.validateCodeFont = value;
            }
        }
    }

    /// <summary>
    /// 噪点干扰(彩色)
    /// </summary>
    public class ValidateCode_Style8 : ValidateCodeType
    {
        private Color backgroundColor = Color.White;
        private Color chaosColor = Color.FromArgb(170, 170, 0x33);
        private Color[] drawColors = new Color[] { Color.FromArgb(0x6b, 0x42, 0x26), Color.FromArgb(0x4f, 0x2f, 0x4f), Color.FromArgb(50, 0x99, 0xcc), Color.FromArgb(0xcd, 0x7f, 50), Color.FromArgb(0x23, 0x23, 0x8e), Color.FromArgb(0x70, 0xdb, 0x93), Color.Red, Color.FromArgb(0xbc, 0x8f, 0x8e) };
        private bool fontTextRenderingHint;
        private int imageHeight = 30;
        private int padding = 1;
        private int validataCodeLength = 4;
        private int validataCodeSize = 0x10;
        private string validateCodeFont = "Arial";

        public override byte[] CreateImage(out string validataCode)
        {
            Bitmap bitmap;
            string formatString = "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z";
            GetRandom(formatString, this.ValidataCodeLength, out validataCode);
            MemoryStream stream = new MemoryStream();
            this.ImageBmp(out bitmap, validataCode);
            bitmap.Save(stream, ImageFormat.Png);
            bitmap.Dispose();
            bitmap = null;
            stream.Close();
            stream.Dispose();
            return stream.GetBuffer();
        }

        private void CreateImageBmp(ref Bitmap bitMap, string validateCode)
        {
            Graphics graphics = Graphics.FromImage(bitMap);
            Random random = new Random();
            if (this.fontTextRenderingHint)
            {
                graphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixel;
            }
            else
            {
                graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            }
            Font font = new Font(this.validateCodeFont, (float)this.validataCodeSize, FontStyle.Regular);
            int maxValue = Math.Max((this.ImageHeight - this.validataCodeSize) - 5, 0);
            for (int i = 0; i < this.validataCodeLength; i++)
            {
                Color color = this.DrawColors[random.Next(this.DrawColors.Length)];
                Brush brush = new SolidBrush(color);
                int[] numArray = new int[] { ((i * this.validataCodeSize) + random.Next(1)) + 3, random.Next(maxValue) - 4 };
                Point point = new Point(numArray[0], numArray[1]);
                graphics.DrawString(validateCode[i].ToString(), font, brush, (PointF)point);
            }
            graphics.Dispose();
        }

        private void DisposeImageBmp(ref Bitmap bitmap)
        {
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.Clear(Color.White);
            Random random = new Random();
            Pen pen = new Pen(this.ChaosColor, 1f);
            for (int i = 0; i < (this.validataCodeLength * 10); i++)
            {
                int x = random.Next(bitmap.Width);
                int y = random.Next(bitmap.Height);
                graphics.DrawRectangle(pen, x, y, 1, 1);
            }
            graphics.Dispose();
        }

        private static void GetRandom(string formatString, int len, out string codeString)
        {
            codeString = string.Empty;
            string[] strArray = formatString.Split(new char[] { ',' });
            Random random = new Random();
            for (int i = 0; i < len; i++)
            {
                int index = random.Next(0x186a0) % strArray.Length;
                codeString = codeString + strArray[index].ToString();
            }
        }

        private void ImageBmp(out Bitmap bitMap, string validataCode)
        {
            int width = (int)((this.validataCodeLength * this.validataCodeSize) * 1.2);
            bitMap = new Bitmap(width, this.ImageHeight);
            this.DisposeImageBmp(ref bitMap);
            this.CreateImageBmp(ref bitMap, validataCode);
        }

        public Color BackgroundColor
        {
            get
            {
                return this.backgroundColor;
            }
            set
            {
                this.backgroundColor = value;
            }
        }

        public Color ChaosColor
        {
            get
            {
                return this.chaosColor;
            }
            set
            {
                this.chaosColor = value;
            }
        }

        public Color[] DrawColors
        {
            get
            {
                return this.drawColors;
            }
            set
            {
                this.drawColors = value;
            }
        }

        private bool FontTextRenderingHint
        {
            get
            {
                return this.fontTextRenderingHint;
            }
            set
            {
                this.fontTextRenderingHint = value;
            }
        }

        public int ImageHeight
        {
            get
            {
                return this.imageHeight;
            }
            set
            {
                this.imageHeight = value;
            }
        }

        public override string Name
        {
            get
            {
                return "噪点干扰(彩色)";
            }
        }

        public int Padding
        {
            get
            {
                return this.padding;
            }
            set
            {
                this.padding = value;
            }
        }

        public int ValidataCodeLength
        {
            get
            {
                return this.validataCodeLength;
            }
            set
            {
                this.validataCodeLength = value;
            }
        }

        public int ValidataCodeSize
        {
            get
            {
                return this.validataCodeSize;
            }
            set
            {
                this.validataCodeSize = value;
            }
        }

        public string ValidateCodeFont
        {
            get
            {
                return this.validateCodeFont;
            }
            set
            {
                this.validateCodeFont = value;
            }
        }
    }

    /// <summary>
    /// 线条干扰(彩色) 
    /// </summary>
    public class ValidateCode_Style9 : ValidateCodeType
    {
        private Color backgroundColor = Color.White;
        private Color chaosColor = Color.FromArgb(170, 170, 0x33);
        private Color[] drawColors = new Color[] { Color.FromArgb(0x6b, 0x42, 0x26), Color.FromArgb(0x4f, 0x2f, 0x4f), Color.FromArgb(50, 0x99, 0xcc), Color.FromArgb(0xcd, 0x7f, 50), Color.FromArgb(0x23, 0x23, 0x8e), Color.FromArgb(0x70, 0xdb, 0x93), Color.Red, Color.FromArgb(0xbc, 0x8f, 0x8e) };
        private bool fontTextRenderingHint;
        private int imageHeight = 30;
        private int padding = 1;
        private int validataCodeLength = 4;
        private int validataCodeSize = 0x10;
        private string validateCodeFont = "Arial";

        public override byte[] CreateImage(out string validataCode)
        {
            Bitmap bitmap;
            string formatString = "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z";
            GetRandom(formatString, this.ValidataCodeLength, out validataCode);
            MemoryStream stream = new MemoryStream();
            this.ImageBmp(out bitmap, validataCode);
            bitmap.Save(stream, ImageFormat.Png);
            bitmap.Dispose();
            bitmap = null;
            stream.Close();
            stream.Dispose();
            return stream.GetBuffer();
        }

        private void CreateImageBmp(ref Bitmap bitMap, string validateCode)
        {
            Graphics graphics = Graphics.FromImage(bitMap);
            Random random = new Random();
            if (this.fontTextRenderingHint)
            {
                graphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixel;
            }
            else
            {
                graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            }
            Font font = new Font(this.validateCodeFont, (float)this.validataCodeSize, FontStyle.Regular);
            int maxValue = Math.Max((this.ImageHeight - this.validataCodeSize) - 5, 0);
            for (int i = 0; i < this.validataCodeLength; i++)
            {
                Color color = this.DrawColors[random.Next(this.DrawColors.Length)];
                Brush brush = new SolidBrush(color);
                int[] numArray = new int[] { ((i * this.validataCodeSize) + random.Next(1)) + 3, random.Next(maxValue) - 4 };
                Point point = new Point(numArray[0], numArray[1]);
                graphics.DrawString(validateCode[i].ToString(), font, brush, (PointF)point);
            }
            graphics.Dispose();
        }

        private void DisposeImageBmp(ref Bitmap bitmap)
        {
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.Clear(Color.White);
            new Random();
            Point[] pointArray = new Point[2];
            Random random = new Random();
            for (int i = 0; i < (this.validataCodeLength * 2); i++)
            {
                Pen pen = new Pen(this.DrawColors[random.Next(this.DrawColors.Length)], 1f);
                pointArray[0] = new Point(random.Next(bitmap.Width), random.Next(bitmap.Height));
                pointArray[1] = new Point(random.Next(bitmap.Width), random.Next(bitmap.Height));
                graphics.DrawLine(pen, pointArray[0], pointArray[1]);
            }
            graphics.Dispose();
        }

        private static void GetRandom(string formatString, int len, out string codeString)
        {
            codeString = string.Empty;
            string[] strArray = formatString.Split(new char[] { ',' });
            Random random = new Random();
            for (int i = 0; i < len; i++)
            {
                int index = random.Next(0x186a0) % strArray.Length;
                codeString = codeString + strArray[index].ToString();
            }
        }

        private void ImageBmp(out Bitmap bitMap, string validataCode)
        {
            int width = (int)((this.validataCodeLength * this.validataCodeSize) * 1.2);
            bitMap = new Bitmap(width, this.ImageHeight);
            this.DisposeImageBmp(ref bitMap);
            this.CreateImageBmp(ref bitMap, validataCode);
        }

        public Color BackgroundColor
        {
            get
            {
                return this.backgroundColor;
            }
            set
            {
                this.backgroundColor = value;
            }
        }

        public Color ChaosColor
        {
            get
            {
                return this.chaosColor;
            }
            set
            {
                this.chaosColor = value;
            }
        }

        public Color[] DrawColors
        {
            get
            {
                return this.drawColors;
            }
            set
            {
                this.drawColors = value;
            }
        }

        private bool FontTextRenderingHint
        {
            get
            {
                return this.fontTextRenderingHint;
            }
            set
            {
                this.fontTextRenderingHint = value;
            }
        }

        public int ImageHeight
        {
            get
            {
                return this.imageHeight;
            }
            set
            {
                this.imageHeight = value;
            }
        }

        public override string Name
        {
            get
            {
                return "线条干扰(彩色)";
            }
        }

        public int Padding
        {
            get
            {
                return this.padding;
            }
            set
            {
                this.padding = value;
            }
        }

        public int ValidataCodeLength
        {
            get
            {
                return this.validataCodeLength;
            }
            set
            {
                this.validataCodeLength = value;
            }
        }

        public int ValidataCodeSize
        {
            get
            {
                return this.validataCodeSize;
            }
            set
            {
                this.validataCodeSize = value;
            }
        }

        public string ValidateCodeFont
        {
            get
            {
                return this.validateCodeFont;
            }
            set
            {
                this.validateCodeFont = value;
            }
        }
    }

    /// <summary>
    /// GIF闪烁动画(彩色)
    /// </summary>
    public class ValidateCode_Style10 : ValidateCodeType
    {
        private Color backgroundColor = Color.White;
        private bool chaos = true;
        private Color chaosColor = Color.FromArgb(170, 170, 0x33);
        private int chaosMode = 1;
        private List<Color> colors = new List<Color>();
        private Color[] drawColors = new Color[] { Color.FromArgb(0x6b, 0x42, 0x26), Color.FromArgb(0x4f, 0x2f, 0x4f), Color.FromArgb(50, 0x99, 0xcc), Color.FromArgb(0xcd, 0x7f, 50), Color.FromArgb(0x23, 0x23, 0x8e), Color.FromArgb(0x70, 0xdb, 0x93), Color.Red, Color.FromArgb(0xbc, 0x8f, 0x8e) };
        private bool fontTextRenderingHint;
        private int imageHeight = 30;
        private int padding = 1;
        private int validataCodeLength = 4;
        private int validataCodeSize = 0x10;
        private string validateCodeFont = "Arial";

        public override byte[] CreateImage(out string validataCode)
        {
            Bitmap bitmap;
            string formatString = "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z";
            GetRandom(formatString, this.ValidataCodeLength, out validataCode);
            MemoryStream stream = new MemoryStream();
            AnimatedGifEncoder encoder = new AnimatedGifEncoder();
            encoder.Start();
            encoder.SetDelay(1);
            encoder.SetRepeat(0);
            Random random = new Random();
            for (int i = 0; i < validataCode.Length; i++)
            {
                this.colors.Add(this.DrawColors[random.Next(this.DrawColors.Length)]);
            }
            for (int j = 0; j < 3; j++)
            {
                string[] strArray = this.SplitCode(validataCode);
                for (int k = 0; k < 2; k++)
                {
                    if (k == 0)
                    {
                        this.ImageBmp(out bitmap, strArray[0]);
                    }
                    else
                    {
                        this.ImageBmp(out bitmap, strArray[1]);
                    }
                    bitmap.Save(stream, ImageFormat.Png);
                    encoder.AddFrame(Image.FromStream(stream));
                    stream = new MemoryStream();
                    bitmap.Dispose();
                }
            }
            encoder.OutPut(ref stream);
            bitmap = null;
            stream.Close();
            stream.Dispose();
            return stream.GetBuffer();
        }

        private void CreateImageBmp(ref Bitmap bitMap, string validateCode)
        {
            Graphics graphics = Graphics.FromImage(bitMap);
            if (this.fontTextRenderingHint)
            {
                graphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixel;
            }
            else
            {
                graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            }
            Font font = new Font(this.validateCodeFont, (float)this.validataCodeSize, FontStyle.Regular);
            int maxValue = Math.Max((this.ImageHeight - this.validataCodeSize) - 4, 0);
            Random random = new Random();
            for (int i = 0; i < this.validataCodeLength; i++)
            {
                Brush brush = new SolidBrush(this.colors[i]);
                int[] numArray = new int[] { ((i * this.validataCodeSize) + random.Next(1)) + 3, random.Next(maxValue) - 4 };
                Point point = new Point(numArray[0], numArray[1]);
                graphics.DrawString(validateCode[i].ToString(), font, brush, (PointF)point);
            }
            graphics.Dispose();
        }

        private void DisposeImageBmp(ref Bitmap bitmap)
        {
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.Clear(Color.White);
            Random random = new Random();
            Point[] pointArray = new Point[2];
            if (this.Chaos)
            {
                Pen pen;
                switch (this.chaosMode)
                {
                    case 1:
                        pen = new Pen(this.ChaosColor, 1f);
                        for (int i = 0; i < (this.validataCodeLength * 10); i++)
                        {
                            int x = random.Next(bitmap.Width);
                            int y = random.Next(bitmap.Height);
                            graphics.DrawRectangle(pen, x, y, 1, 1);
                        }
                        break;

                    case 2:
                        pen = new Pen(this.ChaosColor, (float)(this.validataCodeLength * 4));
                        for (int j = 0; j < (this.validataCodeLength * 10); j++)
                        {
                            int num5 = random.Next(bitmap.Width);
                            int num6 = random.Next(bitmap.Height);
                            graphics.DrawRectangle(pen, num5, num6, 1, 1);
                        }
                        break;

                    case 3:
                        pen = new Pen(this.ChaosColor, 1f);
                        for (int k = 0; k < (this.validataCodeLength * 2); k++)
                        {
                            pointArray[0] = new Point(random.Next(bitmap.Width), random.Next(bitmap.Height));
                            pointArray[1] = new Point(random.Next(bitmap.Width), random.Next(bitmap.Height));
                            graphics.DrawLine(pen, pointArray[0], pointArray[1]);
                        }
                        break;

                    default:
                        pen = new Pen(this.ChaosColor, 1f);
                        for (int m = 0; m < (this.validataCodeLength * 10); m++)
                        {
                            int num9 = random.Next(bitmap.Width);
                            int num10 = random.Next(bitmap.Height);
                            graphics.DrawRectangle(pen, num9, num10, 1, 1);
                        }
                        break;
                }
            }
            graphics.Dispose();
        }

        private static void GetRandom(string formatString, int len, out string codeString)
        {
            codeString = string.Empty;
            string[] strArray = formatString.Split(new char[] { ',' });
            Random random = new Random();
            for (int i = 0; i < len; i++)
            {
                int index = random.Next(0x186a0) % strArray.Length;
                codeString = codeString + strArray[index].ToString();
            }
        }

        private void ImageBmp(out Bitmap bitMap, string validataCode)
        {
            int width = (int)(((this.validataCodeLength * this.validataCodeSize) * 1.3) + 4.0);
            bitMap = new Bitmap(width, this.ImageHeight);
            this.DisposeImageBmp(ref bitMap);
            this.CreateImageBmp(ref bitMap, validataCode);
        }

        private string[] SplitCode(string srcCode)
        {
            Random random = new Random();
            string[] strArray = new string[2];
            foreach (char ch in srcCode)
            {
                if ((random.Next(Math.Abs((int)DateTime.Now.Ticks)) % 2) == 0)
                {
                    string[] strArray2;
                    string[] strArray3;
                    (strArray2 = strArray)[0] = strArray2[0] + ch.ToString();
                    (strArray3 = strArray)[1] = strArray3[1] + " ";
                }
                else
                {
                    string[] strArray4;
                    string[] strArray5;
                    (strArray4 = strArray)[1] = strArray4[1] + ch.ToString();
                    (strArray5 = strArray)[0] = strArray5[0] + " ";
                }
            }
            return strArray;
        }

        public Color BackgroundColor
        {
            get
            {
                return this.backgroundColor;
            }
            set
            {
                this.backgroundColor = value;
            }
        }

        public bool Chaos
        {
            get
            {
                return this.chaos;
            }
            set
            {
                this.chaos = value;
            }
        }

        public Color ChaosColor
        {
            get
            {
                return this.chaosColor;
            }
            set
            {
                this.chaosColor = value;
            }
        }

        public int ChaosMode
        {
            get
            {
                return this.chaosMode;
            }
            set
            {
                this.chaosMode = value;
            }
        }

        public Color[] DrawColors
        {
            get
            {
                return this.drawColors;
            }
            set
            {
                this.drawColors = value;
            }
        }

        private bool FontTextRenderingHint
        {
            get
            {
                return this.fontTextRenderingHint;
            }
            set
            {
                this.fontTextRenderingHint = value;
            }
        }

        public int ImageHeight
        {
            get
            {
                return this.imageHeight;
            }
            set
            {
                this.imageHeight = value;
            }
        }

        public override string Name
        {
            get
            {
                return "GIF闪烁动画(彩色)";
            }
        }

        public int Padding
        {
            get
            {
                return this.padding;
            }
            set
            {
                this.padding = value;
            }
        }

        public int ValidataCodeLength
        {
            get
            {
                return this.validataCodeLength;
            }
            set
            {
                this.validataCodeLength = value;
            }
        }

        public int ValidataCodeSize
        {
            get
            {
                return this.validataCodeSize;
            }
            set
            {
                this.validataCodeSize = value;
            }
        }

        public string ValidateCodeFont
        {
            get
            {
                return this.validateCodeFont;
            }
            set
            {
                this.validateCodeFont = value;
            }
        }
    }

    /// <summary>
    /// 中文(彩色)  
    /// </summary>
    public class ValidateCode_Style11 : ValidateCodeType
    {
        private Color backgroundColor = Color.White;
        private Color chaosColor = Color.FromArgb(170, 170, 0x33);
        private Color[] drawColors = new Color[] { Color.FromArgb(0x6b, 0x42, 0x26), Color.FromArgb(0x4f, 0x2f, 0x4f), Color.FromArgb(50, 0x99, 0xcc), Color.FromArgb(0xcd, 0x7f, 50), Color.FromArgb(0x23, 0x23, 0x8e), Color.FromArgb(0x70, 0xdb, 0x93), Color.Red, Color.FromArgb(0xbc, 0x8f, 0x8e) };
        private bool fontTextRenderingHint;
        private int imageHeight = 30;
        private int padding = 1;
        private int validataCodeLength = 4;
        private int validataCodeSize = 0x10;
        private string validateCodeFont = "Arial";

        public override byte[] CreateImage(out string validataCode)
        {
            Bitmap bitmap;
            string formatString = "丰,王,井,开,夫,天,无,元,专,云,扎,艺,木,五,支,厅,不,太,犬,区,历,尤,友,匹,车,巨,牙,屯,比,互,切,瓦,止,少,日,中,冈,贝,内,水,见,午,牛,手,毛,气,升,长,仁,什,片,仆,化,仇,币,仍,仅,斤,爪,反,介,父,从,今,凶,分,乏,公,仓,月,氏,勿,欠,风,丹,匀,乌,凤,勾,文,六,方,火,为,斗,忆,订,计,户,认,心,尺,引,丑,巴,孔,队,办,以,允,予,劝,双,书,幻,玉,刊,示,末,未,击,打,巧,正,扑,扒,功,扔,去,甘,世,古,节,本,术,可,丙,左,厉,右,石,布,龙,平,灭,轧,东,卡,北,占,业,旧,帅,归,且,旦,奏,春,帮,珍,玻,毒,型,挂,封,持,项,垮,挎,城,挠,政,赴,赵,挡,挺,括,拴,拾,挑,指,垫,挣,挤,拼,挖,按,挥,挪,某,甚,革,荐,巷,带,草,茧,茶,荒,茫,荡,荣,故,胡,南,药,标,枯,柄,栋,相,查,柏,柳,柱,柿,栏,树,要,咸,威,歪,研,砖,厘,厚,砌,砍,面,耐,耍,牵,残,殃,轻,鸦,皆,背,战,点,临,览,竖,省,削,尝,是,盼,眨,哄,显,哑,冒,映,星,昨,畏,趴,胃,贵,界,虹,虾,蚁,思,蚂,虽,品,咽,骂,哗,咱,响,哈,咬,咳,哪,炭,峡,罚,贱,贴,骨,钞,钟,钢,钥,钩,卸,缸,拜,看,矩,怎,牲,选,适,秒,香,种,秋,科,重,复,竿,段,便,俩,贷,顺,修,保,促,侮,俭,俗,俘,信,皇,泉,鬼,侵,追,俊,盾,待,律,很,须,叙,剑,逃,食,盆,胆,胜,胞,胖,脉,勉,狭,狮,独,狡,狱,狠,贸,怨,急,饶,蚀,饺,饼,弯,将,奖,哀,亭,亮,度,迹,庭,疮,疯,疫,疤,姿,亲,音,帝,施,闻,阀,阁,差,养,美,姜,叛,送,类,迷,前,首,逆,总,炼,炸,炮,烂,剃,洁,洪,洒,浇,浊,洞,测,洗,活,派,洽,染,济,洋,洲,浑,浓,津,恒,恢,恰,恼,恨,举,觉,宣,室,宫,宪,突,穿,窃,客,冠,语,扁,袄,祖,神,祝,误,诱,说,诵,垦,退,既,屋,昼,费,陡,眉,孩,除,险,院,娃,姥,姨,姻,娇,怒,架,贺,盈,勇,怠,柔,垒,绑,绒,结,绕,骄,绘,给,络,骆,绝,绞,统";
            GetRandom(formatString, this.ValidataCodeLength, out validataCode);
            MemoryStream stream = new MemoryStream();
            this.ImageBmp(out bitmap, validataCode);
            bitmap.Save(stream, ImageFormat.Png);
            bitmap.Dispose();
            bitmap = null;
            stream.Close();
            stream.Dispose();
            return stream.GetBuffer();
        }

        private void CreateImageBmp(ref Bitmap bitMap, string validateCode)
        {
            Graphics graphics = Graphics.FromImage(bitMap);
            if (this.fontTextRenderingHint)
            {
                graphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixel;
            }
            else
            {
                graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            }
            Font font = new Font(this.validateCodeFont, (float)this.validataCodeSize, FontStyle.Regular);
            int maxValue = Math.Max((this.ImageHeight - this.validataCodeSize) - 5, 0);
            Random random = new Random();
            for (int i = 0; i < this.validataCodeLength; i++)
            {
                Brush brush = new SolidBrush(this.drawColors[random.Next(this.drawColors.Length)]);
                int[] numArray = new int[] { (i * this.validataCodeSize) + (i * 5), random.Next(maxValue) };
                Point point = new Point(numArray[0], numArray[1]);
                graphics.DrawString(validateCode[i].ToString(), font, brush, (PointF)point);
            }
            graphics.Dispose();
        }

        private void DisposeImageBmp(ref Bitmap bitmap)
        {
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.Clear(Color.White);
            Random random = new Random();
            Pen pen = new Pen(this.ChaosColor, 1f);
            for (int i = 0; i < (this.validataCodeLength * 10); i++)
            {
                int x = random.Next(bitmap.Width);
                int y = random.Next(bitmap.Height);
                graphics.DrawRectangle(pen, x, y, 1, 1);
            }
            graphics.Dispose();
        }

        private static void GetRandom(string formatString, int len, out string codeString)
        {
            codeString = string.Empty;
            string[] strArray = formatString.Split(new char[] { ',' });
            Random random = new Random();
            for (int i = 0; i < len; i++)
            {
                int index = random.Next(0x186a0) % strArray.Length;
                codeString = codeString + strArray[index].ToString();
            }
        }

        private void ImageBmp(out Bitmap bitMap, string validataCode)
        {
            int width = (int)(((this.validataCodeLength * this.validataCodeSize) * 1.3) + 10.0);
            bitMap = new Bitmap(width, this.ImageHeight);
            this.DisposeImageBmp(ref bitMap);
            this.CreateImageBmp(ref bitMap, validataCode);
        }

        public Color BackgroundColor
        {
            get
            {
                return this.backgroundColor;
            }
            set
            {
                this.backgroundColor = value;
            }
        }

        public Color ChaosColor
        {
            get
            {
                return this.chaosColor;
            }
            set
            {
                this.chaosColor = value;
            }
        }

        public Color[] DrawColors
        {
            get
            {
                return this.drawColors;
            }
            set
            {
                this.drawColors = value;
            }
        }

        private bool FontTextRenderingHint
        {
            get
            {
                return this.fontTextRenderingHint;
            }
            set
            {
                this.fontTextRenderingHint = value;
            }
        }

        public int ImageHeight
        {
            get
            {
                return this.imageHeight;
            }
            set
            {
                this.imageHeight = value;
            }
        }

        public override string Name
        {
            get
            {
                return "中文(彩色)";
            }
        }

        public int Padding
        {
            get
            {
                return this.padding;
            }
            set
            {
                this.padding = value;
            }
        }

        public int ValidataCodeLength
        {
            get
            {
                return this.validataCodeLength;
            }
            set
            {
                this.validataCodeLength = value;
            }
        }

        public int ValidataCodeSize
        {
            get
            {
                return this.validataCodeSize;
            }
            set
            {
                this.validataCodeSize = value;
            }
        }

        public string ValidateCodeFont
        {
            get
            {
                return this.validateCodeFont;
            }
            set
            {
                this.validateCodeFont = value;
            }
        }
    }

    /// <summary>
    /// 字体旋转(简单) 
    /// </summary>
    public class ValidateCode_Style12 : ValidateCodeType
    {
        private Color backgroundColor = Color.White;
        private Color chaosColor = Color.FromArgb(170, 170, 0x33);
        private Color drawColor = Color.FromArgb(50, 0x99, 0xcc);
        private bool fontTextRenderingHint = true;
        private int validataCodeSize = 0x10;
        private string validateCodeFont = "Arial";

        public override byte[] CreateImage(out string resultCode)
        {
            Bitmap bitmap;
            string formatString = "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z";
            GetRandom(formatString, 4, out resultCode);
            MemoryStream stream = new MemoryStream();
            this.ImageBmp(out bitmap, resultCode);
            bitmap.Save(stream, ImageFormat.Png);
            bitmap.Dispose();
            bitmap = null;
            stream.Close();
            stream.Dispose();
            return stream.GetBuffer();
        }

        private void CreateImageBmp(ref Bitmap bitMap, string validateCode)
        {
            Graphics graphics = Graphics.FromImage(bitMap);
            if (this.fontTextRenderingHint)
            {
                graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            }
            else
            {
                graphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixel;
            }
            Font font = new Font(this.validateCodeFont, (float)this.validataCodeSize, FontStyle.Regular);
            Brush brush = new SolidBrush(this.drawColor);
            Random random = new Random();
            for (int i = 0; i < 4; i++)
            {
                Bitmap image = new Bitmap(30, 30);
                Graphics graphics2 = Graphics.FromImage(image);
                graphics2.TranslateTransform(4f, 0f);
                graphics2.RotateTransform((float)random.Next(20));
                Point point = new Point(4, -2);
                graphics2.DrawString(validateCode[i].ToString(), font, brush, (PointF)point);
                graphics.DrawImage(image, i * 30, 0);
                graphics2.Dispose();
            }
            graphics.Dispose();
        }

        private void DisposeImageBmp(ref Bitmap bitmap)
        {
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.Clear(Color.White);
            Pen pen = new Pen(this.DrawColor, 1f);
            Random random = new Random();
            pen = new Pen(this.ChaosColor, 1f);
            for (int i = 0; i < 40; i++)
            {
                int x = random.Next(bitmap.Width);
                int y = random.Next(bitmap.Height);
                graphics.DrawRectangle(pen, x, y, 1, 1);
            }
            graphics.Dispose();
        }

        private static void GetRandom(string formatString, int len, out string codeString)
        {
            codeString = string.Empty;
            string[] strArray = formatString.Split(new char[] { ',' });
            Random random = new Random();
            for (int i = 0; i < len; i++)
            {
                int index = random.Next(0x186a0) % strArray.Length;
                codeString = codeString + strArray[index].ToString();
            }
        }

        private void ImageBmp(out Bitmap bitMap, string validataCode)
        {
            bitMap = new Bitmap(120, 30);
            this.DisposeImageBmp(ref bitMap);
            this.CreateImageBmp(ref bitMap, validataCode);
        }

        public Color BackgroundColor
        {
            get
            {
                return this.backgroundColor;
            }
            set
            {
                this.backgroundColor = value;
            }
        }

        public Color ChaosColor
        {
            get
            {
                return this.chaosColor;
            }
            set
            {
                this.chaosColor = value;
            }
        }

        public Color DrawColor
        {
            get
            {
                return this.drawColor;
            }
            set
            {
                this.drawColor = value;
            }
        }

        private bool FontTextRenderingHint
        {
            get
            {
                return this.fontTextRenderingHint;
            }
            set
            {
                this.fontTextRenderingHint = value;
            }
        }

        public override string Name
        {
            get
            {
                return "字体旋转(简单)";
            }
        }

        public int ValidataCodeSize
        {
            get
            {
                return this.validataCodeSize;
            }
            set
            {
                this.validataCodeSize = value;
            }
        }

        public string ValidateCodeFont
        {
            get
            {
                return this.validateCodeFont;
            }
            set
            {
                this.validateCodeFont = value;
            }
        }
    }

    /// <summary>
    /// 2年级算术(彩色)
    /// </summary>
    public class ValidateCode_Style13 : ValidateCodeType
    {
        private Color backgroundColor = Color.White;
        private Color chaosColor = Color.FromArgb(170, 170, 0x33);
        private Color[] drawColors = new Color[] { Color.FromArgb(0x6b, 0x42, 0x26), Color.FromArgb(0x4f, 0x2f, 0x4f), Color.FromArgb(50, 0x99, 0xcc), Color.FromArgb(0xcd, 0x7f, 50), Color.FromArgb(0x23, 0x23, 0x8e), Color.FromArgb(0x70, 0xdb, 0x93), Color.Red, Color.FromArgb(0xbc, 0x8f, 0x8e) };
        private bool fontTextRenderingHint;
        private int imageHeight = 30;
        private int padding = 1;
        private int validataCodeLength = 5;
        private int validataCodeSize = 0x10;
        private string validateCodeFont = "Arial";

        public override byte[] CreateImage(out string resultCode)
        {
            string str2;
            Bitmap bitmap;
            string formatString = "1,2,3,4,5,6,7,8,9,0";
            GetRandom(formatString, out str2, out resultCode);
            MemoryStream stream = new MemoryStream();
            this.ImageBmp(out bitmap, str2);
            bitmap.Save(stream, ImageFormat.Png);
            bitmap.Dispose();
            bitmap = null;
            stream.Close();
            stream.Dispose();
            return stream.GetBuffer();
        }

        private void CreateImageBmp(ref Bitmap bitMap, string validateCode)
        {
            Graphics graphics = Graphics.FromImage(bitMap);
            if (this.fontTextRenderingHint)
            {
                graphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixel;
            }
            else
            {
                graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            }
            Font font = new Font(this.validateCodeFont, (float)this.validataCodeSize, FontStyle.Regular);
            int maxValue = Math.Max((this.ImageHeight - this.validataCodeSize) - 5, 0);
            Random random = new Random();
            for (int i = 0; i < this.validataCodeLength; i++)
            {
                Brush brush = new SolidBrush(this.drawColors[random.Next(this.drawColors.Length)]);
                int[] numArray = new int[] { (i * this.validataCodeSize) + (i * 5), random.Next(maxValue) };
                Point point = new Point(numArray[0], numArray[1]);
                graphics.DrawString(validateCode[i].ToString(), font, brush, (PointF)point);
            }
            graphics.Dispose();
        }

        private void DisposeImageBmp(ref Bitmap bitmap)
        {
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.Clear(Color.White);
            Random random = new Random();
            Pen pen = new Pen(this.ChaosColor, 1f);
            for (int i = 0; i < (this.validataCodeLength * 10); i++)
            {
                int x = random.Next(bitmap.Width);
                int y = random.Next(bitmap.Height);
                graphics.DrawRectangle(pen, x, y, 1, 1);
            }
            graphics.Dispose();
        }

        private static void GetRandom(string formatString, out string codeString, out string resultCode)
        {
            Random random = new Random();
            string s = string.Empty;
            string str2 = string.Empty;
            string[] strArray = formatString.Split(new char[] { ',' });
            for (int i = 0; i < 2; i++)
            {
                int index = random.Next(strArray.Length);
                if ((i == 0) && (strArray[index] == "0"))
                {
                    i--;
                }
                else
                {
                    s = s + strArray[index].ToString();
                }
            }
            for (int j = 0; j < 2; j++)
            {
                int num4 = random.Next(strArray.Length);
                if ((j == 0) && (strArray[num4] == "0"))
                {
                    j--;
                }
                else
                {
                    str2 = str2 + strArray[num4].ToString();
                }
            }
            if ((random.Next(100) % 2) == 1)
            {
                codeString = s + "+" + str2;
                resultCode = (int.Parse(s) + int.Parse(str2)).ToString();
            }
            else
            {
                if (int.Parse(s) > int.Parse(str2))
                {
                    codeString = s + "─" + str2;
                }
                else
                {
                    codeString = str2 + "─" + s;
                }
                resultCode = Math.Abs((int)(int.Parse(s) - int.Parse(str2))).ToString();
            }
        }

        private void ImageBmp(out Bitmap bitMap, string validataCode)
        {
            int width = (int)(((this.validataCodeLength * this.validataCodeSize) * 1.3) + 10.0);
            bitMap = new Bitmap(width, this.ImageHeight);
            this.DisposeImageBmp(ref bitMap);
            this.CreateImageBmp(ref bitMap, validataCode);
        }

        public Color BackgroundColor
        {
            get
            {
                return this.backgroundColor;
            }
            set
            {
                this.backgroundColor = value;
            }
        }

        public Color ChaosColor
        {
            get
            {
                return this.chaosColor;
            }
            set
            {
                this.chaosColor = value;
            }
        }

        public Color[] DrawColors
        {
            get
            {
                return this.drawColors;
            }
            set
            {
                this.drawColors = value;
            }
        }

        private bool FontTextRenderingHint
        {
            get
            {
                return this.fontTextRenderingHint;
            }
            set
            {
                this.fontTextRenderingHint = value;
            }
        }

        public int ImageHeight
        {
            get
            {
                return this.imageHeight;
            }
            set
            {
                this.imageHeight = value;
            }
        }

        public override string Name
        {
            get
            {
                return "2年级算术(彩色)";
            }
        }

        public int Padding
        {
            get
            {
                return this.padding;
            }
            set
            {
                this.padding = value;
            }
        }

        public override string Tip
        {
            get
            {
                return "请输入计算结果";
            }
        }

        public int ValidataCodeLength
        {
            get
            {
                return this.validataCodeLength;
            }
            set
            {
                this.validataCodeLength = value;
            }
        }

        public int ValidataCodeSize
        {
            get
            {
                return this.validataCodeSize;
            }
            set
            {
                this.validataCodeSize = value;
            }
        }

        public string ValidateCodeFont
        {
            get
            {
                return this.validateCodeFont;
            }
            set
            {
                this.validateCodeFont = value;
            }
        }
    }

    public class AnimatedGifEncoder
    {
        protected int colorDepth;
        protected byte[] colorTab;
        protected int delay;
        protected int dispose = -1;
        protected bool firstFrame = true;
        protected int height;
        protected Image image;
        protected byte[] indexedPixels;
        protected MemoryStream Memory;
        protected int palSize = 7;
        protected byte[] pixels;
        protected int repeat = -1;
        protected int sample = 10;
        protected bool sizeSet;
        protected bool started;
        protected int transIndex;
        protected Color transparent = Color.Empty;
        protected bool[] usedEntry = new bool[0x100];
        protected int width;

        public bool AddFrame(Image im)
        {
            if ((im == null) || !this.started)
            {
                return false;
            }
            bool flag = true;
            try
            {
                if (!this.sizeSet)
                {
                    this.SetSize(im.Width, im.Height);
                }
                this.image = im;
                this.GetImagePixels();
                this.AnalyzePixels();
                if (this.firstFrame)
                {
                    this.WriteLSD();
                    this.WritePalette();
                    if (this.repeat >= 0)
                    {
                        this.WriteNetscapeExt();
                    }
                }
                this.WriteGraphicCtrlExt();
                this.WriteImageDesc();
                if (!this.firstFrame)
                {
                    this.WritePalette();
                }
                this.WritePixels();
                this.firstFrame = false;
            }
            catch (IOException)
            {
                flag = false;
            }
            return flag;
        }

        protected void AnalyzePixels()
        {
            int length = this.pixels.Length;
            int num2 = length / 3;
            this.indexedPixels = new byte[num2];
            NeuQuant quant = new NeuQuant(this.pixels, length, this.sample);
            this.colorTab = quant.Process();
            int num3 = 0;
            for (int i = 0; i < num2; i++)
            {
                int index = quant.Map(this.pixels[num3++] & 0xff, this.pixels[num3++] & 0xff, this.pixels[num3++] & 0xff);
                this.usedEntry[index] = true;
                this.indexedPixels[i] = (byte)index;
            }
            this.pixels = null;
            this.colorDepth = 8;
            this.palSize = 7;
            if (this.transparent != Color.Empty)
            {
                this.transIndex = this.FindClosest(this.transparent);
            }
        }

        protected int FindClosest(Color c)
        {
            if (this.colorTab == null)
            {
                return -1;
            }
            int r = c.R;
            int g = c.G;
            int b = c.B;
            int num4 = 0;
            int num5 = 0x1000000;
            int length = this.colorTab.Length;
            for (int i = 0; i < length; i++)
            {
                int num8 = r - (this.colorTab[i++] & 0xff);
                int num9 = g - (this.colorTab[i++] & 0xff);
                int num10 = b - (this.colorTab[i] & 0xff);
                int num11 = ((num8 * num8) + (num9 * num9)) + (num10 * num10);
                int index = i / 3;
                if (this.usedEntry[index] && (num11 < num5))
                {
                    num5 = num11;
                    num4 = index;
                }
            }
            return num4;
        }

        protected void GetImagePixels()
        {
            int width = this.image.Width;
            int height = this.image.Height;
            if ((width != this.width) || (height != this.height))
            {
                Image image = new Bitmap(this.width, this.height);
                Graphics graphics = Graphics.FromImage(image);
                graphics.DrawImage(this.image, 0, 0);
                this.image = image;
                graphics.Dispose();
            }
            this.pixels = new byte[(3 * this.image.Width) * this.image.Height];
            int index = 0;
            Bitmap bitmap = new Bitmap(this.image);
            for (int i = 0; i < this.image.Height; i++)
            {
                for (int j = 0; j < this.image.Width; j++)
                {
                    Color pixel = bitmap.GetPixel(j, i);
                    this.pixels[index] = pixel.R;
                    index++;
                    this.pixels[index] = pixel.G;
                    index++;
                    this.pixels[index] = pixel.B;
                    index++;
                }
            }
        }

        public void OutPut(ref MemoryStream MemoryResult)
        {
            this.started = false;
            this.Memory.WriteByte(0x3b);
            this.Memory.Flush();
            MemoryResult = this.Memory;
            this.Memory.Close();
            this.Memory.Dispose();
            this.transIndex = 0;
            this.Memory = null;
            this.image = null;
            this.pixels = null;
            this.indexedPixels = null;
            this.colorTab = null;
            this.firstFrame = true;
        }

        public void SetDelay(int ms)
        {
            this.delay = (int)Math.Round((double)(((float)ms) / 10f));
        }

        public void SetDispose(int code)
        {
            if (code >= 0)
            {
                this.dispose = code;
            }
        }

        public void SetFrameRate(float fps)
        {
            if (fps != 0f)
            {
                this.delay = (int)Math.Round((double)(100f / fps));
            }
        }

        public void SetQuality(int quality)
        {
            if (quality < 1)
            {
                quality = 1;
            }
            this.sample = quality;
        }

        public void SetRepeat(int iter)
        {
            if (iter >= 0)
            {
                this.repeat = iter;
            }
        }

        public void SetSize(int w, int h)
        {
            if (!this.started || this.firstFrame)
            {
                this.width = w;
                this.height = h;
                if (this.width < 1)
                {
                    this.width = 320;
                }
                if (this.height < 1)
                {
                    this.height = 240;
                }
                this.sizeSet = true;
            }
        }

        public void SetTransparent(Color c)
        {
            this.transparent = c;
        }

        public void Start()
        {
            this.Memory = new MemoryStream();
            this.WriteString("GIF89a");
            this.started = true;
        }

        protected void WriteGraphicCtrlExt()
        {
            int num;
            int num2;
            this.Memory.WriteByte(0x21);
            this.Memory.WriteByte(0xf9);
            this.Memory.WriteByte(4);
            if (this.transparent == Color.Empty)
            {
                num = 0;
                num2 = 0;
            }
            else
            {
                num = 1;
                num2 = 2;
            }
            if (this.dispose >= 0)
            {
                num2 = this.dispose & 7;
            }
            num2 = num2 << 2;
            this.Memory.WriteByte(Convert.ToByte((int)(num2 | num)));
            this.WriteShort(this.delay);
            this.Memory.WriteByte(Convert.ToByte(this.transIndex));
            this.Memory.WriteByte(0);
        }

        protected void WriteImageDesc()
        {
            this.Memory.WriteByte(0x2c);
            this.WriteShort(0);
            this.WriteShort(0);
            this.WriteShort(this.width);
            this.WriteShort(this.height);
            if (this.firstFrame)
            {
                this.Memory.WriteByte(0);
            }
            else
            {
                this.Memory.WriteByte(Convert.ToByte((int)(0x80 | this.palSize)));
            }
        }

        protected void WriteLSD()
        {
            this.WriteShort(this.width);
            this.WriteShort(this.height);
            this.Memory.WriteByte(Convert.ToByte((int)(240 | this.palSize)));
            this.Memory.WriteByte(0);
            this.Memory.WriteByte(0);
        }

        protected void WriteNetscapeExt()
        {
            this.Memory.WriteByte(0x21);
            this.Memory.WriteByte(0xff);
            this.Memory.WriteByte(11);
            this.WriteString("NETSCAPE2.0");
            this.Memory.WriteByte(3);
            this.Memory.WriteByte(1);
            this.WriteShort(this.repeat);
            this.Memory.WriteByte(0);
        }

        protected void WritePalette()
        {
            this.Memory.Write(this.colorTab, 0, this.colorTab.Length);
            int num = 0x300 - this.colorTab.Length;
            for (int i = 0; i < num; i++)
            {
                this.Memory.WriteByte(0);
            }
        }

        protected void WritePixels()
        {
            new LZWEncoder(this.width, this.height, this.indexedPixels, this.colorDepth).Encode(this.Memory);
        }

        protected void WriteShort(int value)
        {
            this.Memory.WriteByte(Convert.ToByte((int)(value & 0xff)));
            this.Memory.WriteByte(Convert.ToByte((int)((value >> 8) & 0xff)));
        }

        protected void WriteString(string s)
        {
            char[] chArray = s.ToCharArray();
            for (int i = 0; i < chArray.Length; i++)
            {
                this.Memory.WriteByte((byte)chArray[i]);
            }
        }
    }

    public class GifDecoder
    {
        protected int[] act;
        protected int bgColor;
        protected int bgIndex;
        protected Bitmap bitmap;
        protected byte[] block = new byte[0x100];
        protected int blockSize;
        protected int delay;
        protected int dispose;
        protected int frameCount;
        protected ArrayList frames;
        protected int[] gct;
        protected bool gctFlag;
        protected int gctSize;
        protected int height;
        protected int ih;
        protected Image image;
        protected Stream inStream;
        protected bool interlace;
        protected int iw;
        protected int ix;
        protected int iy;
        protected int lastBgColor;
        protected int lastDispose;
        protected Image lastImage;
        protected Rectangle lastRect;
        protected int[] lct;
        protected bool lctFlag;
        protected int lctSize;
        protected int loopCount = 1;
        protected static readonly int MaxStackSize = 0x1000;
        protected int pixelAspect;
        protected byte[] pixels;
        protected byte[] pixelStack;
        protected short[] prefix;
        protected int status;
        public static readonly int STATUS_FORMAT_ERROR = 1;
        public static readonly int STATUS_OK = 0;
        public static readonly int STATUS_OPEN_ERROR = 2;
        protected byte[] suffix;
        protected int transIndex;
        protected bool transparency;
        protected int width;

        protected void DecodeImageData()
        {
            int num2;
            int num3;
            int num4;
            int num5;
            int num6;
            int num7;
            int num8 = -1;
            int num9 = this.iw * this.ih;
            if ((this.pixels == null) || (this.pixels.Length < num9))
            {
                this.pixels = new byte[num9];
            }
            if (this.prefix == null)
            {
                this.prefix = new short[MaxStackSize];
            }
            if (this.suffix == null)
            {
                this.suffix = new byte[MaxStackSize];
            }
            if (this.pixelStack == null)
            {
                this.pixelStack = new byte[MaxStackSize + 1];
            }
            int num10 = this.Read();
            int num11 = ((int)1) << num10;
            int num12 = num11 + 1;
            int index = num11 + 2;
            int num14 = num8;
            int num15 = num10 + 1;
            int num16 = (((int)1) << num15) - 1;
            int num = 0;
            while (num < num11)
            {
                this.prefix[num] = 0;
                this.suffix[num] = (byte)num;
                num++;
            }
            int num17 = num7 = num6 = num5 = num4 = num3 = num2 = 0;
            int num18 = 0;
            while (num18 < num9)
            {
                if (num4 == 0)
                {
                    if (num7 < num15)
                    {
                        if (num6 == 0)
                        {
                            num6 = this.ReadBlock();
                            if (num6 <= 0)
                            {
                                break;
                            }
                            num2 = 0;
                        }
                        num17 += (this.block[num2] & 0xff) << num7;
                        num7 += 8;
                        num2++;
                        num6--;
                        continue;
                    }
                    num = num17 & num16;
                    num17 = num17 >> num15;
                    num7 -= num15;
                    if ((num > index) || (num == num12))
                    {
                        break;
                    }
                    if (num == num11)
                    {
                        num15 = num10 + 1;
                        num16 = (((int)1) << num15) - 1;
                        index = num11 + 2;
                        num14 = num8;
                        continue;
                    }
                    if (num14 == num8)
                    {
                        this.pixelStack[num4++] = this.suffix[num];
                        num14 = num;
                        num5 = num;
                        continue;
                    }
                    int num19 = num;
                    if (num == index)
                    {
                        this.pixelStack[num4++] = (byte)num5;
                        num = num14;
                    }
                    while (num > num11)
                    {
                        this.pixelStack[num4++] = this.suffix[num];
                        num = this.prefix[num];
                    }
                    num5 = this.suffix[num] & 0xff;
                    if (index >= MaxStackSize)
                    {
                        break;
                    }
                    this.pixelStack[num4++] = (byte)num5;
                    this.prefix[index] = (short)num14;
                    this.suffix[index] = (byte)num5;
                    index++;
                    if (((index & num16) == 0) && (index < MaxStackSize))
                    {
                        num15++;
                        num16 += index;
                    }
                    num14 = num19;
                }
                num4--;
                this.pixels[num3++] = this.pixelStack[num4];
                num18++;
            }
            for (num18 = num3; num18 < num9; num18++)
            {
                this.pixels[num18] = 0;
            }
        }

        protected bool Error()
        {
            return (this.status != STATUS_OK);
        }

        public int GetDelay(int n)
        {
            this.delay = -1;
            if ((n >= 0) && (n < this.frameCount))
            {
                this.delay = ((GifFrame)this.frames[n]).delay;
            }
            return this.delay;
        }

        public Image GetFrame(int n)
        {
            Image image = null;
            if ((n >= 0) && (n < this.frameCount))
            {
                image = ((GifFrame)this.frames[n]).image;
            }
            return image;
        }

        public int GetFrameCount()
        {
            return this.frameCount;
        }

        public Size GetFrameSize()
        {
            return new Size(this.width, this.height);
        }

        public Image GetImage()
        {
            return this.GetFrame(0);
        }

        public int GetLoopCount()
        {
            return this.loopCount;
        }

        private int[] GetPixels(Bitmap bitmap)
        {
            int[] numArray = new int[(3 * this.image.Width) * this.image.Height];
            int index = 0;
            for (int i = 0; i < this.image.Height; i++)
            {
                for (int j = 0; j < this.image.Width; j++)
                {
                    Color pixel = bitmap.GetPixel(j, i);
                    numArray[index] = pixel.R;
                    index++;
                    numArray[index] = pixel.G;
                    index++;
                    numArray[index] = pixel.B;
                    index++;
                }
            }
            return numArray;
        }

        protected void Init()
        {
            this.status = STATUS_OK;
            this.frameCount = 0;
            this.frames = new ArrayList();
            this.gct = null;
            this.lct = null;
        }

        protected int Read()
        {
            int num = 0;
            try
            {
                num = this.inStream.ReadByte();
            }
            catch (IOException)
            {
                this.status = STATUS_FORMAT_ERROR;
            }
            return num;
        }

        public int Read(Stream inStream)
        {
            this.Init();
            if (inStream != null)
            {
                this.inStream = inStream;
                this.ReadHeader();
                if (!this.Error())
                {
                    this.ReadContents();
                    if (this.frameCount < 0)
                    {
                        this.status = STATUS_FORMAT_ERROR;
                    }
                }
                inStream.Close();
            }
            else
            {
                this.status = STATUS_OPEN_ERROR;
            }
            return this.status;
        }

        public int Read(string name)
        {
            this.status = STATUS_OK;
            try
            {
                name = name.Trim().ToLower();
                this.status = this.Read(new FileInfo(name).OpenRead());
            }
            catch (IOException)
            {
                this.status = STATUS_OPEN_ERROR;
            }
            return this.status;
        }

        protected int ReadBlock()
        {
            this.blockSize = this.Read();
            int offset = 0;
            if (this.blockSize <= 0)
            {
                return offset;
            }
            try
            {
                int num2 = 0;
                while (offset < this.blockSize)
                {
                    num2 = this.inStream.Read(this.block, offset, this.blockSize - offset);
                    if (num2 == -1)
                    {
                        goto Label_0050;
                    }
                    offset += num2;
                }
            }
            catch (IOException)
            {
            }
            Label_0050:
            if (offset < this.blockSize)
            {
                this.status = STATUS_FORMAT_ERROR;
            }
            return offset;
        }

        protected int[] ReadColorTable(int ncolors)
        {
            int num = 3 * ncolors;
            int[] numArray = null;
            byte[] buffer = new byte[num];
            int num2 = 0;
            try
            {
                num2 = this.inStream.Read(buffer, 0, buffer.Length);
            }
            catch (IOException)
            {
            }
            if (num2 < num)
            {
                this.status = STATUS_FORMAT_ERROR;
                return numArray;
            }
            numArray = new int[0x100];
            int num3 = 0;
            int num4 = 0;
            while (num3 < ncolors)
            {
                int num5 = buffer[num4++] & 0xff;
                int num6 = buffer[num4++] & 0xff;
                int num7 = buffer[num4++] & 0xff;
                numArray[num3++] = ((Convert.ToInt32((uint)0xff000000) | (num5 << 0x10)) | (num6 << 8)) | num7;
            }
            return numArray;
        }

        protected void ReadContents()
        {
            bool flag = false;
            while (!flag && !this.Error())
            {
                switch (this.Read())
                {
                    case 0x2c:
                        {
                            this.ReadImage();
                            continue;
                        }
                    case 0x3b:
                        {
                            flag = true;
                            continue;
                        }
                    case 0:
                        {
                            continue;
                        }
                    case 0x21:
                        break;

                    default:
                        goto Label_00BA;
                }
                switch (this.Read())
                {
                    case 0xf9:
                        this.ReadGraphicControlExt();
                        break;

                    case 0xff:
                        {
                            this.ReadBlock();
                            string str = "";
                            for (int i = 0; i < 11; i++)
                            {
                                str = str + ((char)this.block[i]);
                            }
                            if (str.Equals("NETSCAPE2.0"))
                            {
                                this.ReadNetscapeExt();
                            }
                            else
                            {
                                this.Skip();
                            }
                            break;
                        }
                }
                this.Skip();
                continue;
                Label_00BA:
                this.status = STATUS_FORMAT_ERROR;
            }
        }

        protected void ReadGraphicControlExt()
        {
            this.Read();
            int num = this.Read();
            this.dispose = (num & 0x1c) >> 2;
            if (this.dispose == 0)
            {
                this.dispose = 1;
            }
            this.transparency = (num & 1) != 0;
            this.delay = this.ReadShort() * 10;
            this.transIndex = this.Read();
            this.Read();
        }

        protected void ReadHeader()
        {
            string str = "";
            for (int i = 0; i < 6; i++)
            {
                str = str + ((char)this.Read());
            }
            if (!str.StartsWith("GIF"))
            {
                this.status = STATUS_FORMAT_ERROR;
            }
            else
            {
                this.ReadLSD();
                if (this.gctFlag && !this.Error())
                {
                    this.gct = this.ReadColorTable(this.gctSize);
                    this.bgColor = this.gct[this.bgIndex];
                }
            }
        }

        protected void ReadImage()
        {
            this.ix = this.ReadShort();
            this.iy = this.ReadShort();
            this.iw = this.ReadShort();
            this.ih = this.ReadShort();
            int num = this.Read();
            this.lctFlag = (num & 0x80) != 0;
            this.interlace = (num & 0x40) != 0;
            this.lctSize = ((int)2) << (num & 7);
            if (this.lctFlag)
            {
                this.lct = this.ReadColorTable(this.lctSize);
                this.act = this.lct;
            }
            else
            {
                this.act = this.gct;
                if (this.bgIndex == this.transIndex)
                {
                    this.bgColor = 0;
                }
            }
            int num2 = 0;
            if (this.transparency)
            {
                num2 = this.act[this.transIndex];
                this.act[this.transIndex] = 0;
            }
            if (this.act == null)
            {
                this.status = STATUS_FORMAT_ERROR;
            }
            if (!this.Error())
            {
                this.DecodeImageData();
                this.Skip();
                if (!this.Error())
                {
                    this.frameCount++;
                    this.bitmap = new Bitmap(this.width, this.height);
                    this.image = this.bitmap;
                    this.SetPixels();
                    this.frames.Add(new GifFrame(this.bitmap, this.delay));
                    if (this.transparency)
                    {
                        this.act[this.transIndex] = num2;
                    }
                    this.ResetFrame();
                }
            }
        }

        protected void ReadLSD()
        {
            this.width = this.ReadShort();
            this.height = this.ReadShort();
            int num = this.Read();
            this.gctFlag = (num & 0x80) != 0;
            this.gctSize = ((int)2) << (num & 7);
            this.bgIndex = this.Read();
            this.pixelAspect = this.Read();
        }

        protected void ReadNetscapeExt()
        {
            do
            {
                this.ReadBlock();
                if (this.block[0] == 1)
                {
                    int num = this.block[1] & 0xff;
                    int num2 = this.block[2] & 0xff;
                    this.loopCount = (num2 << 8) | num;
                }
            }
            while ((this.blockSize > 0) && !this.Error());
        }

        protected int ReadShort()
        {
            return (this.Read() | (this.Read() << 8));
        }

        protected void ResetFrame()
        {
            this.lastDispose = this.dispose;
            this.lastRect = new Rectangle(this.ix, this.iy, this.iw, this.ih);
            this.lastImage = this.image;
            this.lastBgColor = this.bgColor;
            this.lct = null;
        }

        protected void SetPixels()
        {
            int[] pixels = this.GetPixels(this.bitmap);
            if (this.lastDispose > 0)
            {
                if (this.lastDispose == 3)
                {
                    int num = this.frameCount - 2;
                    if (num > 0)
                    {
                        this.lastImage = this.GetFrame(num - 1);
                    }
                    else
                    {
                        this.lastImage = null;
                    }
                }
                if (this.lastImage != null)
                {
                    Array.Copy(this.GetPixels(new Bitmap(this.lastImage)), 0, pixels, 0, this.width * this.height);
                    if (this.lastDispose == 2)
                    {
                        Graphics graphics = Graphics.FromImage(this.image);
                        Color empty = Color.Empty;
                        if (this.transparency)
                        {
                            empty = Color.FromArgb(0, 0, 0, 0);
                        }
                        else
                        {
                            empty = Color.FromArgb(this.lastBgColor);
                        }
                        Brush brush = new SolidBrush(empty);
                        graphics.FillRectangle(brush, this.lastRect);
                        brush.Dispose();
                        graphics.Dispose();
                    }
                }
            }
            int num2 = 1;
            int num3 = 8;
            int num4 = 0;
            for (int i = 0; i < this.ih; i++)
            {
                int num6 = i;
                if (this.interlace)
                {
                    if (num4 >= this.ih)
                    {
                        num2++;
                        switch (num2)
                        {
                            case 2:
                                num4 = 4;
                                break;

                            case 3:
                                num4 = 2;
                                num3 = 4;
                                break;

                            case 4:
                                num4 = 1;
                                num3 = 2;
                                break;
                        }
                    }
                    num6 = num4;
                    num4 += num3;
                }
                num6 += this.iy;
                if (num6 < this.height)
                {
                    int num7 = num6 * this.width;
                    int index = num7 + this.ix;
                    int num9 = index + this.iw;
                    if ((num7 + this.width) < num9)
                    {
                        num9 = num7 + this.width;
                    }
                    int num10 = i * this.iw;
                    while (index < num9)
                    {
                        int num11 = this.pixels[num10++] & 0xff;
                        int num12 = this.act[num11];
                        if (num12 != 0)
                        {
                            pixels[index] = num12;
                        }
                        index++;
                    }
                }
            }
            this.SetPixels(pixels);
        }

        private void SetPixels(int[] pixels)
        {
            int num = 0;
            for (int i = 0; i < this.image.Height; i++)
            {
                for (int j = 0; j < this.image.Width; j++)
                {
                    Color color = Color.FromArgb(pixels[num++]);
                    this.bitmap.SetPixel(j, i, color);
                }
            }
        }

        protected void Skip()
        {
            do
            {
                this.ReadBlock();
            }
            while ((this.blockSize > 0) && !this.Error());
        }

        public class GifFrame
        {
            public int delay;
            public Image image;

            public GifFrame(Image im, int del)
            {
                this.image = im;
                this.delay = del;
            }
        }
    }

    public class LZWEncoder
    {
        private int a_count;
        private byte[] accum = new byte[0x100];
        private static readonly int BITS = 12;
        private bool clear_flg;
        private int ClearCode;
        private int[] codetab = new int[HSIZE];
        private int cur_accum;
        private int cur_bits;
        private int curPixel;
        private static readonly int EOF = -1;
        private int EOFCode;
        private int free_ent;
        private int g_init_bits;
        private int hsize = HSIZE;
        private static readonly int HSIZE = 0x138b;
        private int[] htab = new int[HSIZE];
        private int imgH;
        private int imgW;
        private int initCodeSize;
        private int[] masks = new int[] {
            0, 1, 3, 7, 15, 0x1f, 0x3f, 0x7f, 0xff, 0x1ff, 0x3ff, 0x7ff, 0xfff, 0x1fff, 0x3fff, 0x7fff,
            0xffff
         };
        private int maxbits = BITS;
        private int maxcode;
        private int maxmaxcode = (((int)1) << BITS);
        private int n_bits;
        private byte[] pixAry;
        private int remaining;

        public LZWEncoder(int width, int height, byte[] pixels, int color_depth)
        {
            this.imgW = width;
            this.imgH = height;
            this.pixAry = pixels;
            this.initCodeSize = Math.Max(2, color_depth);
        }

        private void Add(byte c, Stream outs)
        {
            this.accum[this.a_count++] = c;
            if (this.a_count >= 0xfe)
            {
                this.Flush(outs);
            }
        }

        private void ClearTable(Stream outs)
        {
            this.ResetCodeTable(this.hsize);
            this.free_ent = this.ClearCode + 2;
            this.clear_flg = true;
            this.Output(this.ClearCode, outs);
        }

        private void Compress(int init_bits, Stream outs)
        {
            int num2;
            this.g_init_bits = init_bits;
            this.clear_flg = false;
            this.n_bits = this.g_init_bits;
            this.maxcode = this.MaxCode(this.n_bits);
            this.ClearCode = ((int)1) << (init_bits - 1);
            this.EOFCode = this.ClearCode + 1;
            this.free_ent = this.ClearCode + 2;
            this.a_count = 0;
            int code = this.NextPixel();
            int num4 = 0;
            int hsize = this.hsize;
            while (hsize < 0x10000)
            {
                num4++;
                hsize *= 2;
            }
            num4 = 8 - num4;
            int num5 = this.hsize;
            this.ResetCodeTable(num5);
            this.Output(this.ClearCode, outs);
            Label_0170:
            while ((num2 = this.NextPixel()) != EOF)
            {
                hsize = (num2 << this.maxbits) + code;
                int index = (num2 << num4) ^ code;
                if (this.htab[index] == hsize)
                {
                    code = this.codetab[index];
                }
                else
                {
                    if (this.htab[index] >= 0)
                    {
                        int num7 = num5 - index;
                        if (index == 0)
                        {
                            num7 = 1;
                        }
                        do
                        {
                            index -= num7;
                            if (index < 0)
                            {
                                index += num5;
                            }
                            if (this.htab[index] == hsize)
                            {
                                code = this.codetab[index];
                                goto Label_0170;
                            }
                        }
                        while (this.htab[index] >= 0);
                    }
                    this.Output(code, outs);
                    code = num2;
                    if (this.free_ent < this.maxmaxcode)
                    {
                        this.codetab[index] = this.free_ent++;
                        this.htab[index] = hsize;
                    }
                    else
                    {
                        this.ClearTable(outs);
                    }
                }
            }
            this.Output(code, outs);
            this.Output(this.EOFCode, outs);
        }

        public void Encode(Stream os)
        {
            os.WriteByte(Convert.ToByte(this.initCodeSize));
            this.remaining = this.imgW * this.imgH;
            this.curPixel = 0;
            this.Compress(this.initCodeSize + 1, os);
            os.WriteByte(0);
        }

        private void Flush(Stream outs)
        {
            if (this.a_count > 0)
            {
                outs.WriteByte(Convert.ToByte(this.a_count));
                outs.Write(this.accum, 0, this.a_count);
                this.a_count = 0;
            }
        }

        private int MaxCode(int n_bits)
        {
            return ((((int)1) << n_bits) - 1);
        }

        private int NextPixel()
        {
            if (this.remaining == 0)
            {
                return EOF;
            }
            this.remaining--;
            int num = this.curPixel + 1;
            if (num < this.pixAry.GetUpperBound(0))
            {
                byte num2 = this.pixAry[this.curPixel++];
                return (num2 & 0xff);
            }
            return 0xff;
        }

        private void Output(int code, Stream outs)
        {
            this.cur_accum &= this.masks[this.cur_bits];
            if (this.cur_bits > 0)
            {
                this.cur_accum |= code << this.cur_bits;
            }
            else
            {
                this.cur_accum = code;
            }
            this.cur_bits += this.n_bits;
            while (this.cur_bits >= 8)
            {
                this.Add((byte)(this.cur_accum & 0xff), outs);
                this.cur_accum = this.cur_accum >> 8;
                this.cur_bits -= 8;
            }
            if ((this.free_ent > this.maxcode) || this.clear_flg)
            {
                if (this.clear_flg)
                {
                    this.maxcode = this.MaxCode(this.n_bits = this.g_init_bits);
                    this.clear_flg = false;
                }
                else
                {
                    this.n_bits++;
                    if (this.n_bits == this.maxbits)
                    {
                        this.maxcode = this.maxmaxcode;
                    }
                    else
                    {
                        this.maxcode = this.MaxCode(this.n_bits);
                    }
                }
            }
            if (code == this.EOFCode)
            {
                while (this.cur_bits > 0)
                {
                    this.Add((byte)(this.cur_accum & 0xff), outs);
                    this.cur_accum = this.cur_accum >> 8;
                    this.cur_bits -= 8;
                }
                this.Flush(outs);
            }
        }

        private void ResetCodeTable(int hsize)
        {
            for (int i = 0; i < hsize; i++)
            {
                this.htab[i] = -1;
            }
        }
    }

    public class NeuQuant
    {
        protected static readonly int alphabiasshift = 10;
        protected int alphadec;
        protected static readonly int alpharadbias = (((int)1) << alpharadbshift);
        protected static readonly int alpharadbshift = (alphabiasshift + radbiasshift);
        protected static readonly int beta = (intbias >> betashift);
        protected static readonly int betagamma = (intbias << (gammashift - betashift));
        protected static readonly int betashift = 10;
        protected int[] bias = new int[netsize];
        protected int[] freq = new int[netsize];
        protected static readonly int gamma = (((int)1) << gammashift);
        protected static readonly int gammashift = 10;
        protected static readonly int initalpha = (((int)1) << alphabiasshift);
        protected static readonly int initrad = (netsize >> 3);
        protected static readonly int initradius = (initrad * radiusbias);
        protected static readonly int intbias = (((int)1) << intbiasshift);
        protected static readonly int intbiasshift = 0x10;
        protected int lengthcount;
        protected static readonly int maxnetpos = (netsize - 1);
        protected static readonly int minpicturebytes = (3 * prime4);
        protected static readonly int ncycles = 100;
        protected static readonly int netbiasshift = 4;
        protected int[] netindex = new int[0x100];
        protected static readonly int netsize = 0x100;
        protected int[][] network;
        protected static readonly int prime1 = 0x1f3;
        protected static readonly int prime2 = 0x1eb;
        protected static readonly int prime3 = 0x1e7;
        protected static readonly int prime4 = 0x1f7;
        protected static readonly int radbias = (((int)1) << radbiasshift);
        protected static readonly int radbiasshift = 8;
        protected static readonly int radiusbias = (((int)1) << radiusbiasshift);
        protected static readonly int radiusbiasshift = 6;
        protected static readonly int radiusdec = 30;
        protected int[] radpower = new int[initrad];
        protected int samplefac;
        protected byte[] thepicture;

        public NeuQuant(byte[] thepic, int len, int sample)
        {
            this.thepicture = thepic;
            this.lengthcount = len;
            this.samplefac = sample;
            this.network = new int[netsize][];
            for (int i = 0; i < netsize; i++)
            {
                int num2;
                this.network[i] = new int[4];
                int[] numArray = this.network[i];
                numArray[2] = num2 = (i << (netbiasshift + 8)) / netsize;
                numArray[0] = numArray[1] = num2;
                this.freq[i] = intbias / netsize;
                this.bias[i] = 0;
            }
        }

        protected void Alterneigh(int rad, int i, int b, int g, int r)
        {
            int num = i - rad;
            if (num < -1)
            {
                num = -1;
            }
            int netsize = i + rad;
            if (netsize > NeuQuant.netsize)
            {
                netsize = NeuQuant.netsize;
            }
            int num3 = i + 1;
            int num4 = i - 1;
            int num5 = 1;
            while ((num3 < netsize) || (num4 > num))
            {
                int[] numArray;
                int num6 = this.radpower[num5++];
                if (num3 < netsize)
                {
                    numArray = this.network[num3++];
                    try
                    {
                        numArray[0] -= (num6 * (numArray[0] - b)) / alpharadbias;
                        numArray[1] -= (num6 * (numArray[1] - g)) / alpharadbias;
                        numArray[2] -= (num6 * (numArray[2] - r)) / alpharadbias;
                    }
                    catch (Exception)
                    {
                    }
                }
                if (num4 > num)
                {
                    numArray = this.network[num4--];
                    try
                    {
                        numArray[0] -= (num6 * (numArray[0] - b)) / alpharadbias;
                        numArray[1] -= (num6 * (numArray[1] - g)) / alpharadbias;
                        numArray[2] -= (num6 * (numArray[2] - r)) / alpharadbias;
                        continue;
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
            }
        }

        protected void Altersingle(int alpha, int i, int b, int g, int r)
        {
            int[] numArray = this.network[i];
            numArray[0] -= (alpha * (numArray[0] - b)) / initalpha;
            numArray[1] -= (alpha * (numArray[1] - g)) / initalpha;
            numArray[2] -= (alpha * (numArray[2] - r)) / initalpha;
        }

        public byte[] ColorMap()
        {
            int num;
            byte[] buffer = new byte[3 * netsize];
            int[] numArray = new int[netsize];
            for (num = 0; num < netsize; num++)
            {
                numArray[this.network[num][3]] = num;
            }
            int num2 = 0;
            for (num = 0; num < netsize; num++)
            {
                int index = numArray[num];
                buffer[num2++] = (byte)this.network[index][0];
                buffer[num2++] = (byte)this.network[index][1];
                buffer[num2++] = (byte)this.network[index][2];
            }
            return buffer;
        }

        protected int Contest(int b, int g, int r)
        {
            int num = 0x7fffffff;
            int num2 = num;
            int index = -1;
            int num4 = index;
            for (int i = 0; i < netsize; i++)
            {
                int[] numArray = this.network[i];
                int num6 = numArray[0] - b;
                if (num6 < 0)
                {
                    num6 = -num6;
                }
                int num7 = numArray[1] - g;
                if (num7 < 0)
                {
                    num7 = -num7;
                }
                num6 += num7;
                num7 = numArray[2] - r;
                if (num7 < 0)
                {
                    num7 = -num7;
                }
                num6 += num7;
                if (num6 < num)
                {
                    num = num6;
                    index = i;
                }
                int num8 = num6 - (this.bias[i] >> (intbiasshift - netbiasshift));
                if (num8 < num2)
                {
                    num2 = num8;
                    num4 = i;
                }
                int num9 = this.freq[i] >> betashift;
                this.freq[i] -= num9;
                this.bias[i] += num9 << gammashift;
            }
            this.freq[index] += beta;
            this.bias[index] -= betagamma;
            return num4;
        }

        public void Inxbuild()
        {
            int num;
            int index = 0;
            int num3 = 0;
            for (int i = 0; i < netsize; i++)
            {
                int[] numArray;
                int[] numArray2 = this.network[i];
                int num5 = i;
                int num6 = numArray2[1];
                num = i + 1;
                while (num < netsize)
                {
                    numArray = this.network[num];
                    if (numArray[1] < num6)
                    {
                        num5 = num;
                        num6 = numArray[1];
                    }
                    num++;
                }
                numArray = this.network[num5];
                if (i != num5)
                {
                    num = numArray[0];
                    numArray[0] = numArray2[0];
                    numArray2[0] = num;
                    num = numArray[1];
                    numArray[1] = numArray2[1];
                    numArray2[1] = num;
                    num = numArray[2];
                    numArray[2] = numArray2[2];
                    numArray2[2] = num;
                    num = numArray[3];
                    numArray[3] = numArray2[3];
                    numArray2[3] = num;
                }
                if (num6 != index)
                {
                    this.netindex[index] = (num3 + i) >> 1;
                    num = index + 1;
                    while (num < num6)
                    {
                        this.netindex[num] = i;
                        num++;
                    }
                    index = num6;
                    num3 = i;
                }
            }
            this.netindex[index] = (num3 + maxnetpos) >> 1;
            for (num = index + 1; num < 0x100; num++)
            {
                this.netindex[num] = maxnetpos;
            }
        }

        public void Learn()
        {
            int num;
            int num2;
            if (this.lengthcount < minpicturebytes)
            {
                this.samplefac = 1;
            }
            this.alphadec = 30 + ((this.samplefac - 1) / 3);
            byte[] thepicture = this.thepicture;
            int index = 0;
            int lengthcount = this.lengthcount;
            int num5 = this.lengthcount / (3 * this.samplefac);
            int num6 = num5 / ncycles;
            int initalpha = NeuQuant.initalpha;
            int initradius = NeuQuant.initradius;
            int rad = initradius >> radiusbiasshift;
            if (rad <= 1)
            {
                rad = 0;
            }
            for (num = 0; num < rad; num++)
            {
                this.radpower[num] = initalpha * ((((rad * rad) - (num * num)) * radbias) / (rad * rad));
            }
            if (this.lengthcount < minpicturebytes)
            {
                num2 = 3;
            }
            else if ((this.lengthcount % prime1) != 0)
            {
                num2 = 3 * prime1;
            }
            else if ((this.lengthcount % prime2) != 0)
            {
                num2 = 3 * prime2;
            }
            else if ((this.lengthcount % prime3) != 0)
            {
                num2 = 3 * prime3;
            }
            else
            {
                num2 = 3 * prime4;
            }
            num = 0;
            while (num < num5)
            {
                int b = (thepicture[index] & 0xff) << netbiasshift;
                int g = (thepicture[index + 1] & 0xff) << netbiasshift;
                int r = (thepicture[index + 2] & 0xff) << netbiasshift;
                int i = this.Contest(b, g, r);
                this.Altersingle(initalpha, i, b, g, r);
                if (rad != 0)
                {
                    this.Alterneigh(rad, i, b, g, r);
                }
                index += num2;
                if (index >= lengthcount)
                {
                    index -= this.lengthcount;
                }
                num++;
                if (num6 == 0)
                {
                    num6 = 1;
                }
                if ((num % num6) == 0)
                {
                    initalpha -= initalpha / this.alphadec;
                    initradius -= initradius / radiusdec;
                    rad = initradius >> radiusbiasshift;
                    if (rad <= 1)
                    {
                        rad = 0;
                    }
                    for (i = 0; i < rad; i++)
                    {
                        this.radpower[i] = initalpha * ((((rad * rad) - (i * i)) * radbias) / (rad * rad));
                    }
                }
            }
        }

        public int Map(int b, int g, int r)
        {
            int num = 0x3e8;
            int num2 = -1;
            int index = this.netindex[g];
            int num4 = index - 1;
            while ((index < netsize) || (num4 >= 0))
            {
                int[] numArray;
                int num5;
                int num6;
                if (index < netsize)
                {
                    numArray = this.network[index];
                    num5 = numArray[1] - g;
                    if (num5 >= num)
                    {
                        index = netsize;
                    }
                    else
                    {
                        index++;
                        if (num5 < 0)
                        {
                            num5 = -num5;
                        }
                        num6 = numArray[0] - b;
                        if (num6 < 0)
                        {
                            num6 = -num6;
                        }
                        num5 += num6;
                        if (num5 < num)
                        {
                            num6 = numArray[2] - r;
                            if (num6 < 0)
                            {
                                num6 = -num6;
                            }
                            num5 += num6;
                            if (num5 < num)
                            {
                                num = num5;
                                num2 = numArray[3];
                            }
                        }
                    }
                }
                if (num4 >= 0)
                {
                    numArray = this.network[num4];
                    num5 = g - numArray[1];
                    if (num5 >= num)
                    {
                        num4 = -1;
                    }
                    else
                    {
                        num4--;
                        if (num5 < 0)
                        {
                            num5 = -num5;
                        }
                        num6 = numArray[0] - b;
                        if (num6 < 0)
                        {
                            num6 = -num6;
                        }
                        num5 += num6;
                        if (num5 < num)
                        {
                            num6 = numArray[2] - r;
                            if (num6 < 0)
                            {
                                num6 = -num6;
                            }
                            num5 += num6;
                            if (num5 < num)
                            {
                                num = num5;
                                num2 = numArray[3];
                            }
                        }
                    }
                }
            }
            return num2;
        }

        public byte[] Process()
        {
            this.Learn();
            this.Unbiasnet();
            this.Inxbuild();
            return this.ColorMap();
        }

        public void Unbiasnet()
        {
            for (int i = 0; i < netsize; i++)
            {
                this.network[i][0] = this.network[i][0] >> netbiasshift;
                this.network[i][1] = this.network[i][1] >> netbiasshift;
                this.network[i][2] = this.network[i][2] >> netbiasshift;
                this.network[i][3] = i;
            }
        }
    }

    #endregion
}