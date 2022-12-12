using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Day12
{
    internal class Program
    {
        const int EndValue = (int)'E';
        const int StartValue = (int)'S';
        const int FirstValidStep = (int)'a';
        const int LastValidStep = (int)'z';
        static List<List<(int, int)>> ValidPaths = new List<List<(int, int)>>();
        static List<(int, int)> Unreacheable = new List<(int, int)>();

        static void Main(string[] args)
        {
            Console.WriteLine("Day 12!");

            string inputFile = @"..\..\..\sample_input.txt";
            //string inputFile = @"..\..\..\final_input.txt";
            var input = File.ReadAllLines(inputFile);

            //Run(input);
            Solution2.Run(input);
        }

        private static void Run(IEnumerable<string> input)
        {
            var timer = new Stopwatch();
            timer.Start();

            // Create Map
            int[,] map = CreateMap(input.ToList(), out (int, int) startPos);

            CalculateAllValidPaths(map, startPos);

            var answer1 = ValidPaths.Select(p => p.Count).Min();
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
                for(int j = 0; j < input[0].Length; j++)
                {
                    map[i, j] = input[i][j];
                    if (map[i, j] == StartValue)
                    {
                        startPos = new (i, j);
                    }
                }
            }
            return map;
        }

        static void CalculateAllValidPaths(int[,] map, (int, int) startPos)
        {
            var currentPath = new List<(int, int)>();
            currentPath.Add(startPos);

            CalculatePaths(map, currentPath, startPos);
        }

        private static bool CalculatePaths(int[,] map, List<(int, int)> currentPath, (int, int) currentStep)
        {
            List<(int, int)> validNextSteps = GetValidNextSteps(map, currentPath, currentStep);

            foreach(var step in validNextSteps)
            {
                var newPathArray = new (int, int)[currentPath.Count];
                currentPath.CopyTo(newPathArray);
                var newPath = newPathArray.ToList();
                newPath.Add(step);
                if (map[step.Item1, step.Item2] == EndValue)
                {
                    return true;
                }
                if (CalculatePaths(map, newPath, step))
                {
                    ValidPaths.Add(newPath);
                    continue;
                }
                else
                {
                    Unreacheable.Add(step);
                }
            }
            return false;
        }

        private static List<(int, int)> GetValidNextSteps(int[,] map, List<(int, int)> currentPath, (int, int) currentStep)
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
                if (IsValidNextStep(map, currentPath, currentStep, x))
                {
                    result.Add(x);
                }
            });

            return result;
        }

        static bool IsValidNextStep(int[,] map, List<(int, int)> currentPath, (int, int) currentStep, (int, int) nextStep)
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
            if (currentPath.Contains(nextStep) || Unreacheable.Contains(nextStep)) return false;

            return true;
        }
    }
}