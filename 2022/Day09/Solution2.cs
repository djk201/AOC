using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day09
{
    internal class Solution2
    {
        public void Run(IEnumerable<string> input)
        {
            var timer = new Stopwatch();
            timer.Start();

            var answer1 = GetNumberOfLocationsVisited(input.ToArray(), 2);
            var answer1Time = timer.ElapsedMilliseconds;
            Console.WriteLine($"Answer1 = {answer1}; Time Taken = {answer1Time} ms");
            var answer2 = GetNumberOfLocationsVisited(input.ToArray(), 10);
            var answer2Time = timer.ElapsedMilliseconds;
            Console.WriteLine($"Answer2 = {answer2}; Time Taken = {answer2Time} ms");

        }

        public static int GetNumberOfLocationsVisited(string[] source, int numKnots)
        {
            //string[] source = FileHelper.Instance.GetFileAsStringArray(path);
            (char dir, int dist)[] moves = source.Select(s => s.Split())
                                                .Select(a => (a[0][0], Int32.Parse(a[1])))
                                                .ToArray();

            Dictionary<(int, int), bool> visited = new() { { (0, 0), true } };
            RPoint[] knotpos = GetPointArray(numKnots);
            for (int m = 0; m < moves.Length; m++)
            {
                var move = moves[m];
                for (int i = 0; i < move.dist; i++)
                {
                    knotpos[0].Move(move.dir);
                    for (int n = 1; n < numKnots; n++)
                    {
                        if (Touching(knotpos[n - 1], knotpos[n]))
                        {
                            continue;
                        }
                        knotpos[n].Follow(knotpos[n - 1]);
                    }
                    //visited[knotpos.Last().Cords] = true;
                    visited[knotpos[^1].Cords] = true;
                }
            }

            return visited.Count;
        }

        private static RPoint[] GetPointArray(int num)
        {
            var result = new List<RPoint>();
            for (int i = 0; i < num; i++)
            {
                result.Add(new RPoint());
            }

            return result.ToArray();
        }

        static bool Touching(RPoint head, RPoint tail)
        {
            return Math.Abs(head.X - tail.X) <= 1 && Math.Abs(head.Y - tail.Y) <= 1;
        }
    }

    public class RPoint
    {
        public RPoint()
        {
            this.X = 0;
            this.Y = 0;
        }

        public (int, int) Cords => new(this.X, this.Y);
        public int X { get; set; }
        public int Y { get; set; }

        public void Move(char direction)
        {
            switch (direction)
            {
                case 'D':
                    Y -= 1;
                    break;
                case 'U':
                    Y += 1;
                    break;
                case 'L':
                    X -= 1;
                    break;
                case 'R':
                    X += 1;
                    break;
                default:
                    break;
            }
        }

        public void Follow(RPoint target)
        {
            int xDiff = target.X - X;
            int yDiff = target.Y - Y;

            if (xDiff == 0)
            {
                Y += Math.Sign(yDiff);
            }
            else if (yDiff == 0)
            {
                X += Math.Sign(xDiff);
            }
            else
            {
                X += Math.Sign(xDiff);
                Y += Math.Sign(yDiff);
            }
        }
    }

}
