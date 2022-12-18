using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;

namespace Day18
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Day 18!");

            //string inputFile = @"..\..\..\sample_input.txt";
            string inputFile = @"..\..\..\final_input.txt";
            var input = File.ReadAllLines(inputFile).ToList();

            Run(input);
        }

        private static void Run(List<string> input)
        {
            var timer = new Stopwatch();
            timer.Start();

            var answer1 = GetTotalSurfaceArea(input);
            var answer1Time = timer.ElapsedMilliseconds;
            Console.WriteLine($"Answer1 = {answer1}; Time Taken = {answer1Time} ms");

            timer.Restart();
            var answer2 = 0;
            var answer2Time = timer.ElapsedMilliseconds;
            Console.WriteLine($"Answer2 = {answer2}; Time Taken = {answer2Time} ms");
        }

        static long GetTotalSurfaceArea(List<string> input)
        {
            var points = input.Select(x => x.Split(",")).Select(y => 
                                    new Point { X = int.Parse(y[0]), Y = int.Parse(y[1]), Z = int.Parse(y[2]) }).ToArray();

            long surfaceArea = 6;
            for (int i = 1; i < points.Length; i++)
            {
                surfaceArea += 6;
                for(int j = 0; j < i; j++)
                {
                    if (points[i].IsAdjacentToPoint(points[j])) surfaceArea -= 2;
                }
            }
            return surfaceArea;
        }

        
    }

    class Point
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public bool IsAdjacentToPoint(Point p) => 
            (p.X == X && p.Y == Y && Math.Abs(p.Z - Z) == 1) || 
            (p.Y == Y && p.Z == Z && Math.Abs(p.X - X) == 1) || 
            (p.X == X && p.Z == Z && Math.Abs(p.Y - Y) == 1);
    }
}