using System.Diagnostics;
namespace Day08
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Day 08!");

            string inputFile = @"..\..\..\sample_input.txt";
            //string inputFile = @"..\..\..\final_input.txt";
            var input = File.ReadAllLines(inputFile).ToList();

            Run(input);
        }

        private static void Run(List<string> input)
        {
            var timer = new Stopwatch();
            timer.Start();

            int totalRows = input.Count;
            int totalColumns = input[0].Length;

            var map = new Node[totalRows, totalColumns];
            var scoreUpGrid = new int[10];
            var scoreLeftGrid = new int[10];
            var scoreDownGrid = new int[10];
            var scoreRightGrid = new int[10];

            for (var i = 0; i < totalRows; i++)
            {
                for (var j = 0; j < totalColumns; j++)
                {
                    var height = int.Parse(input[i][j].ToString());
                    var newNode = new Node { Height =  height};
                    UpdateScoreGrid(scoreUpGrid, height);
                    if (i > 0)
                    {
                        newNode.MaxUp = Math.Max(map[i - 1, j].Height, map[i - 1, j].MaxUp);
                        newNode.ScenicScoreUp = scoreUpGrid[height] + 1;
                    }
                    UpdateScoreGrid(scoreLeftGrid, height);
                    if (j > 0)
                    {
                        newNode.MaxLeft = Math.Max(map[i, j - 1].Height, map[i, j - 1].MaxLeft);
                        newNode.ScenicScoreLeft = scoreLeftGrid[height] + 1;
                    }
                    map[i, j] = newNode;
                }
            }

            for (var i = totalRows - 1; i >= 0; i--)
            {
                for (var j = totalColumns - 1; j >= 0; j--)
                {
                    var height = map[i, j].Height;
                    UpdateScoreGrid(scoreDownGrid, height);
                    if (i < totalRows - 1)
                    {
                        map[i, j].MaxDown = Math.Max(map[i + 1, j].Height, map[i + 1, j].MaxDown);
                        map[i, j].ScenicScoreDown = scoreDownGrid[height] + 1;
                    }
                    UpdateScoreGrid(scoreRightGrid, height);
                    if (j < totalColumns - 1)
                    {
                        map[i, j].MaxRight = Math.Max(map[i, j + 1].Height, map[i, j + 1].MaxRight);
                        map[i, j].ScenicScoreRight = scoreRightGrid[height] + 1;
                    }
                }
            }

            var hiddenCount = 0;
            for (var i = 1; i < totalRows - 1; i++)
            {
                for (var j = 1; j < totalColumns - 1; j++)
                {
                    var node = map[i, j];
                    if (map[i, j].Height <= node.MaxLeft && map[i, j].Height <= node.MaxRight 
                        && map[i, j].Height <= node.MaxUp && map[i, j].Height <= node.MaxDown)
                    {
                        hiddenCount++;
                    }
                }
            }

            var answer1 = (totalColumns * totalRows) - hiddenCount;

            var answer1Time = timer.ElapsedMilliseconds;
            Console.WriteLine($"Answer1 = {answer1}; Time Taken = {answer1Time} ms");

            timer.Restart();

            var highestScenicScore = 0;
            for (var i = 1; i < totalRows - 1; i++)
            {
                for (var j = 1; j < totalColumns - 1; j++)
                {
                    var node = map[i, j];
                    var score = node.ScenicScoreLeft * node.ScenicScoreRight * node.ScenicScoreUp * node.ScenicScoreDown;
                    highestScenicScore = Math.Max(highestScenicScore, score);
                }
            }

            var answer2 = highestScenicScore;

            var answer2Time = timer.ElapsedMilliseconds;
            Console.WriteLine($"Answer2 = {answer2}; Time Taken = {answer2Time} ms");
        }

        static void UpdateScoreGrid(int[] scoreGrid, int height)
        {
            for (var i = 0; i < scoreGrid.Length; i++)
            {
                if (i > height)
                {
                    scoreGrid[i] += 1;
                }
                else
                {
                    scoreGrid[i] = 0;
                }
            }
        }
    }

    class Node
    {
        public int Height { get; set; }
        public int MaxUp { get; set; }
        public int MaxLeft { get; set; }
        public int MaxRight { get; set; }
        public int MaxDown { get; set; }

        public int ScenicScoreUp { get; set; }
        public int ScenicScoreLeft { get; set; }
        public int ScenicScoreRight { get; set; }
        public int ScenicScoreDown { get; set; }
    }
}