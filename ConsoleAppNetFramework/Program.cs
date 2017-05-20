using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ConsoleAppNetFramework
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(HttpRuntime.AppDomainAppPath);
        }
    }
}
