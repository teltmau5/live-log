using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RandomLog
{
    class Program
    {
        static void Main(string[] args)
        {
            var i = 0;

            var file = File.Open(@"C:\Temp\log.txt", FileMode.Append, FileAccess.ReadWrite, FileShare.Read);
            var streamWriter = new StreamWriter(file);
            var random = new Random();


            while (i < 1000000)
            {
               streamWriter.WriteLine(DateTime.Now.ToString("u") + " W3SVC3 example 130.225.95.160 GET /logout.aspx - 443 - 130.225.95.252 HTTP/1.1 Mozilla/5.0+(Windows+NT+6.1;+Win64;+x64)+AppleWebKit/537.36+(KHTML,+like+Gecko)+Chrome/55.0.2883.87+Safari/537.36 https://example.org/application/view/6388 example.org 302 0 0 469 529 218");
               Thread.Sleep(random.Next(500) + 1);

                i++;
            }
        }
    }
}
