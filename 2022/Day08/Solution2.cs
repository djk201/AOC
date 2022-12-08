using System.Diagnostics;

namespace Day08
{
    internal class Solution2
    {
        public static void Run(IEnumerable<string> input)
        {
            var timer = new Stopwatch();
            timer.Start();

            var map = input.Select(x => x.Select(y => int.Parse(y.ToString())).ToArray()).ToArray();

            int answer1 = 0;
            for (int x = 0; x < map.Length; x++)
            {
                for (int y = 0; y < map[x].Length; y++)
                {
                    int[][] views = GetViews(map, x, y);
                    bool visible = views.Any(i => i.All(j => j < map[x][y]));
                    answer1 += visible ? 1 : 0;
                }
            }

            var answer1Time = timer.ElapsedMilliseconds;
            Console.WriteLine($"Answer1 = {answer1}; Time Taken = {answer1Time} ms");
            timer.Restart();

            int answer2 = 0;
            for (int i = 0; i < map.Length; i++)
            {
                for (int j = 0; j < map[i].Length; j++)
                {
                    int score = CalculateScenicScore(map, i, j);
                    answer2 = Math.Max(answer2, score);
                }
            }
            var answer2Time = timer.ElapsedMilliseconds;
            Console.WriteLine($"Answer2 = {answer2}; Time Taken = {answer2Time} ms");
        }

        internal static int CalculateScenicScore(int[][] map, int x, int y)
        {
            int[][] views = GetViews(map, x, y);
            int result = 0;
            int cumulative = 1;
            foreach (int[] path in views)
            {
                int score = 1;
                for (int i = 0; i < path.Length; i++)
                {
                    if (path[i] >= map[x][y])
                    {
                        break;
                    }
                    score++;
                    if (i == path.Length - 1)
                    {
                        score--;
                    }
                }
                cumulative *= score;
            }
            result = Math.Max(result, cumulative);
            return result;
        }

        internal static int[][] GetViews(int[][] map, int x, int y)
        {
            int[][] views =
            {
                // View from top to bottom
                map.Take(x).Reverse().Select(i => i[y]).ToArray(),
                // View from bottom to top
                map.Skip(x + 1).Select(i => i[y]).ToArray(),
                // View from left to right
                map[x].Take(y).Reverse().ToArray(),
                // View from right to left
                map[x].Skip(y + 1).ToArray()
            };
            return views;
        }

    }
}
