using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Tail
{

    class Sample
    {
        public string Identifier { get; set; }
        public int HttpCode { get; set; }
        public HttpMethod Method { get; set; }
        public string ServerName { get; set; }
        public int DurationInMilliseconds { get; set; }
        public DateTime Timestamp { get; set; }
        public string Domain { get; set; }
    }


    interface ISampleStore
    {
        void AddSample(Sample sample);
    }

    class BufferedStore : ISampleStore
    {
        private List<Sample> _samples = new List<Sample>();
        private object _locker = new object();


        public void AddSample(Sample sample)
        {
            lock (_locker)
            {
                this._samples.Add(sample);
            }
        }

        public List<Sample> RetrieveAndResetSamples()
        { 

            List<Sample> result;

            lock (_locker)
            {
                result = _samples;
                _samples = new List<Sample>();
            }

            return result;
        }
    }
}
