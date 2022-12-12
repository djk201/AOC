using System.Diagnostics;

namespace Day12
{
    internal class Program
    {
        const int EndValue = (int)'E';
        const int StartValueS = (int)'S';
        const int aValue = (int)'a';
        const int LastValidStep = (int)'z';
        static (int, int) EndPos = new(0, 0);

        static void Main(string[] args)
        {
            Console.WriteLine("Day 12!");

            //string inputFile = @"..\..\..\sample_input.txt";
            string inputFile = @"..\..\..\final_input.txt";
            var input = File.ReadAllLines(inputFile).ToList();

            Run(input);
        }

        public static void Run(List<string> input)
        {
            var timer = new Stopwatch();
            timer.Start();

            int[,] map = CreateMap(input, out (int, int) startPos, out List<(int, int)> aPos);

            var answer1 = CalculateShortestPathFrom(map, new List<(int, int)> { startPos });
            var answer1Time = timer.ElapsedMilliseconds;
            Console.WriteLine($"Answer1 = {answer1}; Time Taken = {answer1Time} ms");

            timer.Restart();
            aPos.Add(startPos);
            var answer2 = CalculateShortestPathFrom(map, aPos); ;
            var answer2Time = timer.ElapsedMilliseconds;
            Console.WriteLine($"Answer2 = {answer2}; Time Taken = {answer2Time} ms");
        }

        static int CalculateShortestPathFrom(int[,] map, List<(int, int)> startPos)
        {
            Dictionary<(int, int), bool> visitedDict = new Dictionary<(int, int), bool>();
            startPos.ForEach(x => visitedDict[x] = true);
            var isPathFound = false;

            List<(int, int)> currentSteps = startPos;
            int stepCount = 0;

            while (!isPathFound)
            {
                stepCount++;
                var validNextSteps = GetAllValidNextSteps(map, currentSteps, visitedDict);
                if (validNextSteps.Contains(EndPos))
                {
                    isPathFound = true;
                    break;
                }
                validNextSteps.ForEach(x => visitedDict[x] = true);
                currentSteps = validNextSteps;
            }
            return stepCount;
        }

        private static List<(int, int)> GetAllValidNextSteps(int[,] map, List<(int, int)> currentSteps, Dictionary<(int, int), bool> visitedDict)
        {
            var result = new List<(int, int)>();

            foreach (var currentStep in currentSteps)
            {
                var nextSteps = GetValidNextSteps(map, currentStep, visitedDict);
                result = result.Union(nextSteps).ToList();
            }
            return result;
        }

        private static List<(int, int)> GetValidNextSteps(int[,] map, (int, int) currentStep, Dictionary<(int, int), bool> visitedDict)
        {
            var stepsToCheck = new List<(int, int)>
            {
                (currentStep.Item1, currentStep.Item2 - 1),
                (currentStep.Item1 - 1, currentStep.Item2),
                (currentStep.Item1, currentStep.Item2 + 1),
                (currentStep.Item1 + 1, currentStep.Item2)
            };

            return stepsToCheck.Where(x => IsValidNextStep(map, currentStep, x, visitedDict)).ToList();
        }

        static bool IsValidNextStep(int[,] map, (int, int) currentStep, (int, int) nextStep, Dictionary<(int, int), bool> nodeDict)
        {
            if (nextStep.Item1 < 0 || nextStep.Item2 < 0 || nextStep.Item1 >= map.GetLength(0) || nextStep.Item2 >= map.GetLength(1)) return false;

            if (map[currentStep.Item1, currentStep.Item2] == StartValueS)
            {
                return map[nextStep.Item1, nextStep.Item2] == aValue ? true : false;
            }
            if (map[nextStep.Item1, nextStep.Item2] == EndValue)
            {
                return map[currentStep.Item1, currentStep.Item2] == LastValidStep ? true : false;
            }

            if (map[currentStep.Item1, currentStep.Item2] < map[nextStep.Item1, nextStep.Item2] - 1) return false;
            if (nodeDict.ContainsKey(nextStep)) return false;

            return true;
        }

        static int[,] CreateMap(List<string> input, out (int, int) startPos, out List<(int, int)> aPos)
        {
            startPos = new(0, 0);
            aPos = new List<(int, int)>();
            int[,] map = new int[input.Count(), input[0].Length];

            for (int i = 0; i < input.Count(); i++)
            {
                for (int j = 0; j < input[0].Length; j++)
                {
                    map[i, j] = input[i][j];
                    if (map[i, j] == StartValueS)
                    {
                        startPos = new(i, j);
                    }
                    else if (map[i, j] == EndValue)
                    {
                        EndPos = new(i, j);
                    }
                    else if (map[i, j] == aValue)
                    {
                        aPos.Add(new(i, j));
                    }
                }
            }
            return map;
        }
    }
}