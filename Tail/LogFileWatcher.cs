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
    class LogFileWatcher
    {
        private readonly ISampleStore _store;

        public LogFileWatcher(ISampleStore store)
        {
            _store = store;
        }

        public void StartReading(string filename)
        {
            var filter = new CombinedFilter(
                new RegexFilter(@"/administration.*$"),
                new RegexFilter(@"/studerende.*$"));

            var regex = new Regex(@"(.*) W3SVC.* (GET|POST|DELETE|HEAD) (\S+) .* (\S+) (?:\d+) (?:\d+) (?:\d+) (?:\d+) (?:\d+) (\d+)$");

            long lastPosition = 0;
            while (true)
            {

                if (!File.Exists(filename)) return;

                using (FileStream fs = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete))
                {
                    using (var streamReader = new StreamReader(fs, Encoding.UTF8, true, 1024))
                    {

                        string line;
                        string lastLine = null;

                        fs.Seek(lastPosition, SeekOrigin.Begin);

                        while ((line = streamReader.ReadLine()) != null)
                        {
                            //if (lastLine != null)
                            //{
                            var match = regex.Match(line);
                            if (match.Success)
                            {
                                //Console.WriteLine(lastLine);
                                var filterResult = filter.IsMatch(match.Groups[3].Value);

                                if (filterResult.Success)
                                {

                                    _store.AddSample(new Sample
                                    {
                                        Identifier = filterResult.Identifier,
                                        DurationInMilliseconds = int.Parse(match.Groups[5].Value),
                                        Domain = match.Groups[4].Value,
                                        Method = new HttpMethod(match.Groups[2].Value),
                                        ServerName = Environment.MachineName,
                                        Timestamp = DateTime.Parse(match.Groups[1].Value),
                                    });

                                }
                                else
                                {
                                    //Console.WriteLine($"       {match.Groups[2].Value}");
                                }


                            }
                            //}


                            //lastLine = line;

                            lastPosition = fs.Position;
                        }
                        // Process line



                    }
                    //// Seek 1024 bytes from the end of the file
                    //        fs.Seek(-1024, SeekOrigin.End);
                    //// read 1024 bytes
                    //byte[] bytes = new byte[1024];
                    //fs.Read(bytes, 0, 1024);
                    //// Convert bytes to string
                    //string s = Encoding.Default.GetString(bytes);
                    //// or string s = Encoding.UTF8.GetString(bytes);
                    //// and output to console


                }
                Thread.Sleep(1000);
            }
        }
    }
}
