using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MyHelper.Framework.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateFuzzyDir("D:\\00MyTest\\");
        }

        /// <summary>
        /// 在指定文件夹下面创建100*100的干扰文件夹
        /// </summary>
        /// <param name="root_path"></param>
        public static void CreateFuzzyDir(string root_path)
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
    }
}
