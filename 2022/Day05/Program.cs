using System.Diagnostics;

namespace Day05
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Day 05!");

            string inputFile = @"..\..\..\sample_input.txt";
            //string inputFile = @"..\..\..\final_input.txt";
            var input = File.ReadAllLines(inputFile).ToList();

            Run(input);
        }
        private static void Run(List<string> input)
        {
            var timer = new Stopwatch();
            timer.Start();

            var answer1 = 0;
            var answer2 = 0;

            input.ForEach(x =>
            {

            });

            var answer1Time = timer.ElapsedMilliseconds;
            timer.Restart();

            var answer2Time = timer.ElapsedMilliseconds;

            Console.WriteLine($"Answer1 = {answer1}; Time Taken = {answer1Time}ms");
            Console.WriteLine($"Answer2 = {answer2}; Time Taken = {answer2Time}ms");
        }

    }
}