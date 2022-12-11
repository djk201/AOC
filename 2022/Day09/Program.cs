using System.Diagnostics;
namespace Day09
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Day 09!");

            //string inputFile = @"..\..\..\sample_input.txt";
            string inputFile = @"..\..\..\final_input.txt";
            var input = File.ReadAllLines(inputFile);

            //Run(input);
            new Solution2().Run(input);
        }

        private static void Run(IEnumerable<string> input)
        {
            var timer = new Stopwatch();
            timer.Start();

            var motions = input.Select(x =>
            {
                var parts = x.Split(' ');
                var motion = new Motion { Direction = parts[0], Steps = int.Parse(parts[1]) };
                return motion;
            }).ToList();

            var map = CreateMap(motions, out int x, out int y);

            var answer1 = GetTailPositionsVisitedCount(map, motions, x, y);

            var answer1Time = timer.ElapsedMilliseconds;
            Console.WriteLine($"Answer1 = {answer1}; Time Taken = {answer1Time} ms");

            timer.Restart();
            var answer2 = 0;
            var answer2Time = timer.ElapsedMilliseconds;
            Console.WriteLine($"Answer2 = {answer2}; Time Taken = {answer2Time} ms");
        }

        private static int GetTailPositionsVisitedCount(int[,] map, List<Motion> motions, int startX, int startY)
        {
            map[startX, startY] = 1;
            int xH = startX;
            int yH = startY;
            int xT = startX;
            int yT = startY;

            motions.ForEach(m =>
            {
                for (var i = 0; i < m.Steps; i++)
                {
                    switch (m.Direction)
                    {
                        case "U":
                            xH += 1;
                            break;
                        case "D":
                            xH -= 1;
                            break;
                        case "R":
                            yH += 1;
                            break;
                        default: //"L
                            yH -= 1;
                            break;
                    }
                }
            });

            var result = 1;
            return result;
        }

        private static int[,] CreateMap(List<Motion> motions, out int x, out int y)
        {
            int maxUpDown = 0;
            int minUpDown = 0;
            int maxLeftRight = 0;
            int minLeftRight = 0;

            int upDown = 0;
            int leftRight = 0;

            motions.ForEach(m =>
            {
                switch(m.Direction)
                {
                    case "U":
                        upDown += m.Steps;
                        maxUpDown = Math.Max(maxUpDown, upDown);
                        break;
                    case "D":
                        upDown -= m.Steps;
                        minUpDown = Math.Min(minUpDown, upDown);
                        break;
                    case "L":
                        leftRight += m.Steps;
                        maxLeftRight = Math.Max(maxLeftRight, upDown);
                        break;
                    default: //"R
                        leftRight -= m.Steps;
                        minLeftRight = Math.Min(minLeftRight, upDown);
                        break;
                }
            });

            x = 0 - minUpDown;
            y = 0 - minLeftRight;

            var map = new int[maxUpDown - minUpDown, maxLeftRight - minLeftRight];
            return map;
        }
    }

    class Motion
    {
        public string Direction { get; set; }
        public int Steps { get; set; }
    }
}