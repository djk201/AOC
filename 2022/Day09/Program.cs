using System.Diagnostics;
namespace Day09
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Day 09!");

            string inputFile = @"..\..\..\sample_input.txt";
            //string inputFile = @"..\..\..\final_input.txt";
            var input = File.ReadAllLines(inputFile);

            Run(input);
        }

        private static void Run(IEnumerable<string> input)
        {
            var timer = new Stopwatch();
            timer.Start();

            var answer1 = 0;

            var motions = input.Select(x =>
            {
                var parts = x.Split(' ');
                var motion = new Motion { Direction = parts[0], Steps = int.Parse(parts[1]) };
                return motion;
            }).ToList();

            var answer1Time = timer.ElapsedMilliseconds;
            Console.WriteLine($"Answer1 = {answer1}; Time Taken = {answer1Time} ms");

            timer.Restart();
            var answer2 = 0;
            var answer2Time = timer.ElapsedMilliseconds;
            Console.WriteLine($"Answer2 = {answer2}; Time Taken = {answer2Time} ms");
        }

        private static int[][] CreateMap(IEnumerable<>)
        {

        }
    }

    class Motion
    {
        public string Direction { get; set; }
        public int Steps { get; set; }
    }
}