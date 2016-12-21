using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tail
{
    internal class SamplePersist
    {
        internal void Save(List<Sample> samples)
        {
            string query =
                @"INSERT INTO Log  (ServerName, Timestamp, Count, AvgDurationInMilliseconds, MaxDurationInMilliseconds, Identifier, Domain)
                  VALUES(@ServerName, @Timestamp, @Count, @AvgDurationInMilliseconds, @MaxDurationInMilliseconds, @Identifier, @Domain)
";

            using (var connection = new SqlConnection("Server=morfe;Database=LogTail;Trusted_Connection=True;"))
            {
                connection.Open();
                foreach (var sample in samples.GroupBy(p => new { p.Identifier, Timestamp = RemoveSeconds(p.Timestamp), p.Domain } ))
                {
                    Console.WriteLine(sample.Key.Timestamp + " " + sample.Key.Domain + " " + sample.Key.Identifier + " " + sample.Average(p => p.DurationInMilliseconds));

                    SqlCommand myCommand = new SqlCommand(query, connection);
                    myCommand.Parameters.AddWithValue("@ServerName", "Example");
                    myCommand.Parameters.AddWithValue("@Timestamp", sample.Key.Timestamp);
                    myCommand.Parameters.AddWithValue("@Identifier", sample.Key.Identifier);
                    myCommand.Parameters.AddWithValue("@Count", sample.Count());
                    myCommand.Parameters.AddWithValue("@AvgDurationInMilliseconds", sample.Average(p => p.DurationInMilliseconds));
                    myCommand.Parameters.AddWithValue("@MaxDurationInMilliseconds", sample.Max(p => p.DurationInMilliseconds));
                    myCommand.Parameters.AddWithValue("@Domain", sample.Key.Domain);
                    // ... other parameters
                    myCommand.ExecuteNonQuery();
                }
            }

        }


        private static DateTime RemoveSeconds(DateTime time)
        {
            return new DateTime(time.Year, time.Month, time.Day, time.Hour, time.Minute, 0);
        }
    }
}
