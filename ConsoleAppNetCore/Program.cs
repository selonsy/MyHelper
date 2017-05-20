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
            LogHelper.WriteDebug("nimza");
        }
    }
}