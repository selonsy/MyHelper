using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Devin.Temp;
using System.Drawing;
using Devin;

namespace MyHelper.Framework.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program().SecureCodeTest();
        }

        public void SecureCodeTest()
        {
            SecureCodeHelper obj = new SecureCodeHelper(ValidateCodeStyle.噪点干扰_扭曲);            
            Image bit = obj.Image;
            bit.Save("c://test1.png");
            System.Console.WriteLine(obj.Text);
        }

        /// <summary>
        /// 在指定文件夹下面创建100*100的干扰文件夹
        /// </summary>
        /// <param name="root_path"></param>
        public void CreateFuzzyDir(string root_path)
        {
            string path = root_path;
            DirectoryInfo dir = new DirectoryInfo(path);
            if (!dir.Exists)
            {
                Directory.CreateDirectory(path);
            }
            else if (dir.GetDirectories().Count() > 0)
            {
                throw new Exception("指定文件夹下面已存在目录~");                
            }
            for (int i = 0; i < 100; i++)
            {
                string new_path = dir + (i < 10 ? "0" + i : i.ToString()) + Guid.NewGuid().ToString().Replace("-", "");
                DirectoryInfo temp_dir = new DirectoryInfo(new_path);
                if (!temp_dir.Exists)
                {
                    Directory.CreateDirectory(new_path);
                }
                for (int j = 0; j < 100; j++)
                {
                    string new_new_path = new_path + "//" + (j < 10 ? "0" + j : j.ToString()) + Guid.NewGuid().ToString().Replace("-", "");
                    DirectoryInfo temp_temp_dir = new DirectoryInfo(new_new_path);
                    if (!temp_temp_dir.Exists)
                    {
                        Directory.CreateDirectory(new_new_path);
                    }
                }
            }
        }
        
        /// <summary>
        /// 获取当前工作目录
        /// </summary>
        /// <returns></returns>
        public string GetCurrentDir()
        {
            //获取模块的完整路径。  
            string path1 = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            //获取和设置当前目录(该进程从中启动的目录)的完全限定目录  
            string path2 = System.Environment.CurrentDirectory;
            //获取应用程序的当前工作目录  
            string path3 = System.IO.Directory.GetCurrentDirectory();
            //获取程序的基目录  
            string path4 = System.AppDomain.CurrentDomain.BaseDirectory;
            //获取和设置包括该应用程序的目录的名称  
            string path5 = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;

            //获取启动了应用程序的可执行文件的路径  
            //string path6 = System.Windows.Forms.Application.StartupPath;
            //获取启动了应用程序的可执行文件的路径及文件名  
            //string path7 = System.Windows.Forms.Application.ExecutablePath;

            StringBuilder str = new StringBuilder();
            str.AppendLine("System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName:" + path1);
            str.AppendLine("System.Environment.CurrentDirectory:" + path2);
            str.AppendLine("System.IO.Directory.GetCurrentDirectory():" + path3);
            str.AppendLine("System.AppDomain.CurrentDomain.BaseDirectory:" + path4);
            str.AppendLine("System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase:" + path5);
            //str.AppendLine("System.Windows.Forms.Application.StartupPath:" + path6);
            //str.AppendLine("System.Windows.Forms.Application.ExecutablePath:" + path7);
            string allPath = str.ToString();

            return allPath;

            //result:
            //System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName:
            //C:\PROGRAM FILES (X86)\MICROSOFT VISUAL STUDIO 14.0\COMMON7\IDE\COMMONEXTENSIONS\MICROSOFT\TESTWINDOW\te.processhost.managed.exe

            //System.Environment.CurrentDirectory:
            //D:\00MyWorkSpace\99MyGitHub\MyHelper\MyHelper.Tests\bin\Debug

            //System.IO.Directory.GetCurrentDirectory():
            //D:\00MyWorkSpace\99MyGitHub\MyHelper\MyHelper.Tests\bin\Debug

            //System.AppDomain.CurrentDomain.BaseDirectory:
            //D:\00MyWorkSpace\99MyGitHub\MyHelper\MyHelper.Tests\bin\Debug

            //System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase:
            //D:\00MyWorkSpace\99MyGitHub\MyHelper\MyHelper.Tests\bin\Debug

        }
    }
}
