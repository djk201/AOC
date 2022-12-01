using Shared;
using System.Diagnostics;

namespace Day01
{
    internal class Solution2
    {
        public static void Run()
        {
            //string inputFile = @"..\..\..\sample_input.txt";
            //string inputFile = @"..\..\..\final_input.txt";
            Utility.GetFile("1");
            string inputFile = @"input.txt";
            var input = File.ReadAllLines(inputFile).ToList();

            var timer = new Stopwatch();
            timer.Start();

            var elves = input.ChunkBy(string.Empty).Select(x => x.ToListOfTypeInt()).Aggregate();

            var answer1 = elves.Max();
            var answer1Time = timer.ElapsedMilliseconds;
            timer.Restart();
            var answer2 = elves.OrderByDescending(z => z).Take(3).Sum();
            var answer2Time = timer.ElapsedMilliseconds;

            Console.WriteLine($"Answer1 = {answer1}; Time Taken = {answer1Time}ms");
            Console.WriteLine($"Answer2 = {answer2}; Time Taken = {answer2Time}ms");
        }
    }
}
