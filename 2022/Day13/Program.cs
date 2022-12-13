using System.Diagnostics;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Shared;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Day13
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Day 13!");

            //string inputFile = @"..\..\..\sample_input.txt";
            string inputFile = @"..\..\..\final_input.txt";
            var input = File.ReadAllLines(inputFile).ToList();

            Run(input);
        }

        private static void Run(List<string> input)
        {
            var timer = new Stopwatch();
            timer.Start();

            var pairs = input.ChunkBy(string.Empty).ToList();

            var totalScore = 0;
            for (int i = 0; i < pairs.Count; i++)
            {
                //Console.WriteLine(i);
                dynamic left = JsonConvert.DeserializeObject(pairs[i][0]);
                dynamic right = JsonConvert.DeserializeObject(pairs[i][1]);

                if (IsRightOrder(left, right)) totalScore += (i + 1);
            }

            var answer1 = totalScore;
            var answer1Time = timer.ElapsedMilliseconds;
            Console.WriteLine($"Answer1 = {answer1}; Time Taken = {answer1Time} ms");

            timer.Restart();
            var answer2 = 0;
            var answer2Time = timer.ElapsedMilliseconds;
            Console.WriteLine($"Answer2 = {answer2}; Time Taken = {answer2Time} ms");
        }

        static bool? IsRightOrder(JArray left, JArray right)
        {
            bool? result = null; ;
            if (!left.Any() && !right.Any()) // both are empty
                return null;

            for (int i=0; i < left.Count; i++)
            {
                if (right.Count <= i) return false; // right ran out of items

                if (left[i].GetType() == typeof(JValue) && right[i].GetType() == typeof(JValue))
                {
                    if (left[i].ToObject<int>() == right[i].ToObject<int>())
                    {
                        continue;
                    }
                    else
                    {
                        return left[i].ToObject<int>() < right[i].ToObject<int>();
                    }
                }

                var leftArray = left[i].GetType() == typeof(JArray) ? left[i] as JArray : new JArray(left[i]);
                var rightArray = right[i].GetType() == typeof(JArray) ? right[i] as JArray : new JArray(right[i]);

                result = IsRightOrder(leftArray, rightArray);

                if (result == null) continue;
                return result;
            }

            if (result == null && right.Count > left.Count) // left ran out of items
            {
                return true;
            }
            
            return result;
        }
    }
}