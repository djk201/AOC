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
            var answer2 = GetExternalSurfaceArea(input);
            var answer2Time = timer.ElapsedMilliseconds;
            Console.WriteLine($"Answer2 = {answer2}; Time Taken = {answer2Time} ms");
        }

        static long GetExternalSurfaceArea(List<string> input)
        {
            var points = input.Select(x => x.Split(",")).Select(y => (int.Parse(y[0]), int.Parse(y[1]), int.Parse(y[2])));
            var min = new Point(points.Select(p => p.Item1).Min() - 1, points.Select(p => p.Item2).Min() - 1, points.Select(p => p.Item3).Min() - 1);
            var max = new Point(points.Select(p => p.Item1).Max() + 1, points.Select(p => p.Item2).Max() + 1, points.Select(p => p.Item3).Max() + 1);

            var queue = new Queue<Point>();
            queue.Enqueue(min);

            var result = 0;
            var visited = new List<(int, int, int)> { min.ToMulti };
            while(queue.TryDequeue(out var point))
            {
                foreach(var neighbour in point.AllNeighbours)
                {
                    if (!neighbour.IsInRange(min, max)) continue;
                    if (points.Contains(neighbour.ToMulti)) result++;
                    else if (!visited.Contains(neighbour.ToMulti))
                    {
                        queue.Enqueue(neighbour);
                        visited.Add(neighbour.ToMulti);
                    }
                }
            }
            return result;

        }

        static long GetTotalSurfaceArea(List<string> input)
        {
            var points = input.Select(x => x.Split(",")).Select(y => 
                                    new Point (int.Parse(y[0]), int.Parse(y[1]), int.Parse(y[2]))).ToArray();

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

        public Point (int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public (int, int, int) ToMulti => (X, Y, Z);

        public bool IsAdjacentToPoint(Point p) =>
            (p.X == X && p.Y == Y && Math.Abs(p.Z - Z) == 1) ||
            (p.Y == Y && p.Z == Z && Math.Abs(p.X - X) == 1) ||
            (p.X == X && p.Z == Z && Math.Abs(p.Y - Y) == 1);

        public List<Point> AllNeighbours
        {
            get
            {
                return new List<Point>
                {
                    {new Point (X, Y, Z - 1)},
                    {new Point (X, Y, Z + 1)},
                    {new Point (X - 1, Y, Z)},
                    {new Point (X + 1, Y, Z)},
                    {new Point (X, Y - 1, Z)},
                    {new Point (X, Y + 1, Z)}
                };
            }
        }

        public bool IsInRange(Point rangeMin, Point rangeMax) => 
            X >= rangeMin.X && X <= rangeMax.X && Y >= rangeMin.Y && Y <= rangeMax.Y && Z >= rangeMin.Z && Z <= rangeMax.Z;
    }
}