using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CopyLog
{
    class Program
    {
        static void Main(string[] args)
        {
            var i = 0;

            var file = File.Open(@"C:\Temp\log.txt", FileMode.Append, FileAccess.Write, FileShare.Read);
            var streamWriter = new StreamWriter(file);
            var random = new Random();

            var logToCopy = File.ReadAllLines(@"C:\Temp\DE_u_ex16121914.log");

            streamWriter.AutoFlush = true;

            while (true)
            {
               
            foreach (var line in logToCopy)
            {
                streamWriter.WriteLine(line);

                    //Console.WriteLine(line);

                if (random.Next(100) == 1)
                    {
                        Thread.Sleep(10);

                    }
                }
            }
        }
    }
}
