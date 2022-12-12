using System.Diagnostics;

namespace Day12
{
    internal class Solution3
    {
        const int EndValue = (int)'E';
        const int StartValue = (int)'S';
        const int FirstValidStep = (int)'a';
        const int LastValidStep = (int)'z';
        static (int, int) EndPos = new(0, 0);
        //static List<List<(int, int)>> ValidPaths = new List<List<(int, int)>>();
        //static List<(int, int)> Unreacheable = new List<(int, int)>();

        public static void Run(IEnumerable<string> input)
        {
            var timer = new Stopwatch();
            timer.Start();

            // Create Map
            int[,] map = CreateMap(input.ToList(), out (int, int) startPos);

            var answer1 = CalculateShortestPath(map, startPos);
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
                    else if (map[i, j] == EndValue)
                    {
                        EndPos = new(i, j);
                    }
                }
            }
            return map;
        }

        static int CalculateShortestPath(int[,] map, (int, int) startPos)
        {
            
            Dictionary<(int, int), bool> visitedDict = new Dictionary<(int, int), bool> { { startPos, true } };
            var isPathFound = false;

            List<(int, int)> currentSteps = new List<(int, int)> { startPos };
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
                if (IsValidNextStep(map, currentStep, x, visitedDict))
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

}
