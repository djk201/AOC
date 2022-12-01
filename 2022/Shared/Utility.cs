using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class Utility
    {
        public static string GetFile(string day, string sessionToken = "53616c7465645f5ffbf91231a8357eda06c8d1c940be8035403551f517d8f4f8fe0e200408558fea06160c1bdbb9cb2b64b18d9ff58f79c563ede70736388dac")
        {
            string current = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            //string input_location = Path.Join(current, "inputs");
            //if (!Directory.Exists(input_location))
            //{
            //    Directory.CreateDirectory(input_location);
            //}
            var input_location = Path.Join(current, $"input.txt");
            if (!File.Exists(input_location))
            {
                string url = $"https://adventofcode.com/2022/day/{day}/input";
                using (var client = new WebClient())
                {
                    client.Headers.Add(HttpRequestHeader.Cookie, $"session={sessionToken}");
                    client.DownloadFile(url, input_location);
                }
            }
            return input_location;
        }
    }
}
