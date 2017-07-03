using System;
using System.IO;
using Devin;

namespace ConsoleAppNetCore
{
    class Program
    {
        static void Main(string[] args)
        {
            Config.Init_Config("netcoretest", 3);
            //LogHelper.WriteDebug("nimza");
            RedisHelper.StringSet(CacheFolderEnum.Folder1, "name", "wangjie");

            string ss = RedisHelper.StringGet(CacheFolderEnum.Folder1, "name");


        }
    }
}