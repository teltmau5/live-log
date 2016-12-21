using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Tail
{
    class Program
    {


        private static void OnChanged(object source, FileSystemEventArgs e)
        {
            StartWatcherInBackground(e.FullPath);
        }

        private static void StartWatcherInBackground(string filename)
        {
            Console.WriteLine($"Watching {filename}");

            Task.Run(() =>
            {
                Thread.Sleep(500);

                var watcher = new LogFileWatcher(_store);

                watcher.StartReading(filename);
            });
        }

        static BufferedStore _store = new BufferedStore();

        static void Main(string[] args)
        {
            var rootPath = @"C:\temp";

            if (args.Length > 0) rootPath = args[0];

            if (!Directory.Exists(rootPath))
            {
                Console.WriteLine("Directory does not exist: " + rootPath);
                return;
            }

            Console.WriteLine($"Watching root path {rootPath}");

            FileSystemWatcher watcher;

            watcher = new FileSystemWatcher();
            watcher.Path = rootPath;
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
                                   | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            watcher.Filter = "*.log";
            watcher.Created += new FileSystemEventHandler(OnChanged);
            watcher.EnableRaisingEvents = true;


            var files = Directory.EnumerateFiles(rootPath, "*.log");
            foreach (var file in files)
            {
                StartWatcherInBackground(file);
            }




            while (true)
            {
                try
                {

                    var samples = _store.RetrieveAndResetSamples();

                    new SamplePersist().Save(samples);
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                    Console.WriteLine(exception.StackTrace);
                }

                Thread.Sleep(2000);
            }

        }
    }
}
