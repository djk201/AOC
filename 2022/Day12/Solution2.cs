using System.Diagnostics;

namespace Day12
{
    internal class Solution2
    {
        const int EndValue = (int)'E';
        const int StartValue = (int)'S';
        const int FirstValidStep = (int)'a';
        const int LastValidStep = (int)'z';
        //static List<List<(int, int)>> ValidPaths = new List<List<(int, int)>>();
        //static List<(int, int)> Unreacheable = new List<(int, int)>();
        static long ShortestPath = 0;

        public static void Run(IEnumerable<string> input)
        {
            var timer = new Stopwatch();
            timer.Start();

            // Create Map
            int[,] map = CreateMap(input.ToList(), out (int, int) startPos);

            CalculateAllValidPaths(map, startPos);

            var answer1 = ShortestPath - 1;
            var answer1Time = timer.ElapsedMilliseconds;
            Console.WriteLine($"Answer1 = {answer1}; Time Taken = {answer1Time} ms");

            timer.Restart();
            var answer2 = 0;
            var answer2Time = timer.ElapsedMilliseconds;
            Console.WriteLine($"Answer2 = {answer2}; Time Taken = {answer2Time} ms");
        }

        static int[,] CreateMap(List<string> input, out (int, int) startPos)
        {
            startPos = new(0, 0);
            //var map = input.Select(x => x.ToArray().Select(c => (int)c).ToArray()).ToArray();
            int[,] map = new int[input.Count(), input[0].Length];

            for (int i = 0; i < input.Count(); i++)
            {
                for (int j = 0; j < input[0].Length; j++)
                {
                    map[i, j] = input[i][j];
                    if (map[i, j] == StartValue)
                    {
                        startPos = new(i, j);
                    }
                }
            }
            return map;
        }

        static void CalculateAllValidPaths(int[,] map, (int, int) startPos)
        {
            Dictionary<(int, int), bool> nodeDict = new Dictionary<(int, int), bool>();
            nodeDict.Add(startPos, true);

            CalculatePaths(map, startPos, 1, nodeDict);
        }

        private static void CalculatePaths(int[,] map, (int, int) currentStep, int stepCount, Dictionary<(int, int), bool> nodeDict)
        {
            //var currentStep = currentNode.Position;
            List<(int, int)> validNextSteps = GetValidNextSteps(map, currentStep, nodeDict);

            validNextSteps.ForEach(s => nodeDict[s] = true);
            stepCount++;

            foreach (var step in validNextSteps)
            {
                if (map[step.Item1, step.Item2] == EndValue)
                {
                    if (ShortestPath == 0 || ShortestPath > stepCount)
                    {
                        ShortestPath = stepCount;
                    }
                    break;
                }
                CalculatePaths(map, step, stepCount, nodeDict);
            }
            stepCount--;

            validNextSteps.ForEach(s => nodeDict[s] = false);
        }

        private static List<(int, int)> GetValidNextSteps(int[,] map, (int, int) currentStep, Dictionary<(int, int), bool> nodeDict)
        {
            var result = new List<(int, int)>();

            var stepsToCheck = new List<(int, int)>
            {
                (currentStep.Item1, currentStep.Item2 - 1),
                (currentStep.Item1 - 1, currentStep.Item2),
                (currentStep.Item1, currentStep.Item2 + 1),
                (currentStep.Item1 + 1, currentStep.Item2)
            };

            stepsToCheck.ForEach(x =>
            {
                if (IsValidNextStep(map, currentStep, x, nodeDict))
                {
                    result.Add(x);
                }
            });

            return result;
        }

        static bool IsValidNextStep(int[,] map, (int, int) currentStep, (int, int) nextStep, Dictionary<(int, int), bool> nodeDict)
        {
            if (nextStep.Item1 < 0 || nextStep.Item2 < 0
                || nextStep.Item1 >= map.GetLength(0) || nextStep.Item2 >= map.GetLength(1)) return false;
            if (nextStep.Item1 == currentStep.Item1 && nextStep.Item2 == currentStep.Item2) return false;

            if (map[currentStep.Item1, currentStep.Item2] == StartValue)
            {
                if (map[nextStep.Item1, nextStep.Item2] == FirstValidStep)
                {
                    return true;
                }
                return false;
            }
            if (map[nextStep.Item1, nextStep.Item2] == EndValue)
            {
                if (map[currentStep.Item1, currentStep.Item2] == LastValidStep)
                {
                    return true;
                }
                return false;
            }

            if (map[currentStep.Item1, currentStep.Item2] < map[nextStep.Item1, nextStep.Item2] - 1) return false;
            if (nodeDict.ContainsKey(nextStep)) return false;

            return true;
        }


    }

    class Node
    {
        public int X { get; set; }
        public int Y { get; set; }
        public (int, int) Position => new (X, Y);
        public int Value { get; set; }
        public int Level { get; set; }
        public List<Node> ChildNodes { get; set; }
        public Node Parent { get; set; }
        public Node()
        {
            ChildNodes = new List<Node>();
        }
        public bool IsVisited { get; set; }
    }

}
