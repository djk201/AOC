using System.Diagnostics;
namespace Day17
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Day 17!");

            //string inputFile = @"..\..\..\sample_input.txt";
            string inputFile = @"..\..\..\final_input.txt";
            var input = File.ReadAllLines(inputFile).ToList();

            //Run(input);
            new Solution2().Run(input);
        }

        private static void Run(List<string> input)
        {
            var timer = new Stopwatch();
            timer.Start();

            long answer1 = GetTowerHeight(input.First(), 2022);
            var answer1Time = timer.ElapsedMilliseconds;
            Console.WriteLine($"Answer1 = {answer1}; Time Taken = {answer1Time} ms");

            timer.Restart();
            long answer2 = GetTowerHeight(input.First(), 1000000000000);
            var answer2Time = timer.ElapsedMilliseconds;
            Console.WriteLine($"Answer2 = {answer2}; Time Taken = {answer2Time} ms");
        }

        static long GetTowerHeight(string input, long rocks)
        {
            int[] jetPush = input.Select(c => c == '<' ? 0 : 1).ToArray(); // true = left; false = right

            Dictionary<(long, long), bool> tower = new Dictionary<(long, long), bool>();
            var rockType = 0;
            var jetIndex = 0;
            long towerHeight = 0;

            for (int i = 1; i <= rocks; i++)
            {
                var isStopped = false;
                rockType = (rockType % 5) + 1;
                var rock = GetRock(rockType, towerHeight);

                while (!isStopped)
                {
                    // Jet Push
                    rock = Move(tower, rock, jetPush[jetIndex], out isStopped);
                    jetIndex++;
                    jetIndex = jetIndex % jetPush.Length;

                    // Move Down
                    rock = Move(tower, rock, 2, out isStopped);
                }
                rock.ToList().ForEach(p => tower[p] = true);
                towerHeight = tower.Keys.Select(x => x.Item2).Max() + 1;

                if (jetIndex % 1000 == 0)
                {
                    Console.WriteLine($"JetIndex = {jetIndex}");
                }

                if (jetIndex == 0)
                {
                    Console.WriteLine($"Number of Rocks fell = {i}; Are All points occupied = {AreAllPointsOccupiedOnTOpOfTower(tower, towerHeight)}");
                }
            }
            return towerHeight;
        }

        static bool AreAllPointsOccupiedOnTOpOfTower(Dictionary<(long, long), bool> tower, long height)
        {
            for(int i = 0; i < 6; i++)
            {
                if (!tower.ContainsKey((i, height)))
                    return false;
            }
            return true;
        }

        private static (long, long)[] Move(Dictionary<(long, long), bool> tower, (long, long)[] rock, int direction, out bool isStopped)
        {
            isStopped = false;
            var newRockPosition = rock;
            var newPoints = new List<(long, long)>();

            foreach (var point in rock)
            {
                var newPoint = point;
                switch (direction)
                {
                    case 0: // left
                        newPoint = (point.Item1 - 1, point.Item2);
                        if (newPoint.Item1 < 0)
                            isStopped = true;
                        break;
                    case 1: // right
                        newPoint = (point.Item1 + 1, point.Item2);
                        if (newPoint.Item1 > 6)
                            isStopped = true;
                        break;
                    case 2: // down
                    default:
                        newPoint = (point.Item1, point.Item2 - 1);
                        if (newPoint.Item2 < 0)
                            isStopped = true;
                        break;
                }
                if (isStopped || tower.Keys.Contains(newPoint))
                {
                    isStopped = true;
                    break;
                }

                newPoints.Add(newPoint);
            }

            if (!isStopped)
                newRockPosition = newPoints.ToArray();

            return newRockPosition;
        }

        private static (long, long)[] GetRock(int rockType, long towerHeight)
        {
            var x = 1;
            long y = towerHeight + 3;
            var rock = GetRockTemplate(rockType, x);

            for(int i = 0; i < rock.Length; i++)
            {
                rock[i].Item2 += y;
            }
            return rock;
        }

        static (long, long)[] GetRockTemplate(int rockType, int x)
        {
            List<(long, long)> rock;
            
            switch (rockType)
            {
                case 1:
                    rock = new List<(long, long)> { (x + 1, 0), (x + 2, 0), (x + 3, 0), (x + 4, 0) };
                    break;
                case 2:
                    rock = new List<(long, long)> { (x + 2, 0), (x + 1, 1), (x + 2, 1), (x + 3, 1), (x + 2, 2) };
                    break;
                case 3:
                    rock = new List<(long, long)> { (x + 1, 0), (x + 2, 0), (x + 3, 0), (x + 3, 1), (x + 3, 2) };
                    break;
                case 4:
                    rock = new List<(long, long)> { (x + 1, 0), (x + 1, 1), (x + 1, 2), (x + 1, 3) };
                    break;
                case 5:
                default:
                    rock = new List<(long, long)> { (x + 1, 0), (x + 2, 0), (x + 1, 1), (x + 2, 1) };
                    break;
            }

            return rock.ToArray();
        }
    }

}