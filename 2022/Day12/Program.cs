using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Day12
{
    internal class Program
    {
        const int EndValue = (int)'E';
        const int StartValue = (int)'S';
        const int FirstValidStep = (int)'a';
        List<List<(int, int)>> ValidPaths = new List<List<(int, int)>>();

        static void Main(string[] args)
        {
            Console.WriteLine("Day 12!");

            string inputFile = @"..\..\..\sample_input.txt";
            //string inputFile = @"..\..\..\final_input.txt";
            var input = File.ReadAllLines(inputFile).ToList();

            Run(input);
        }

        private static void Run(List<string> input)
        {
            var timer = new Stopwatch();
            timer.Start();

            // Create Map
            int[,] map = new int[1,1];

            // Find Starting Position
            (int, int) startPos = new (1, 1);

            var allPaths = GetAllValidPaths(map, startPos);

            var answer1 = allPaths.Select(p => p.Count).Min();
            var answer1Time = timer.ElapsedMilliseconds;
            Console.WriteLine($"Answer1 = {answer1}; Time Taken = {answer1Time} ms");

            timer.Restart();
            var answer2 = 0;
            var answer2Time = timer.ElapsedMilliseconds;
            Console.WriteLine($"Answer2 = {answer2}; Time Taken = {answer2Time} ms");
        }


        static List<List<(int, int)>> GetAllValidPaths(int[,] map, (int, int) startPos)
        {
            var result = new List<List<(int, int)>>();

            List<(int, int)> doNotVisit;
            var currentPos = startPos;

            var currentPath = new List<(int, int)>();

            //currentPath.Push(startPos);

            var allPathsExplored = false;
            while (!allPathsExplored)
            {
                // Get valid next steps
                List<(int, int)> validNextSteps = new List<(int, int)>();

                foreach (var step in validNextSteps)
                {
                    var newPathArray = new (int, int)[currentPath.Count];
                    currentPath.CopyTo(newPathArray);
                    var newPath = newPathArray.ToList();
                    newPath.Add(step);
                    if (map[step.Item1, step.Item2] == EndValue)
                    {
                        continue;
                    }

                }

            }

            return result;
        }

        private bool ProcessPath(int[,] map, List<(int, int)> currentPath, (int, int) nextStep)
        {
            List<(int, int)> validNextSteps = new List<(int, int)>();

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
                if (ProcessPath(map, newPath, step))
                {
                    ValidPaths.Add(newPath);
                    continue;
                }
            }
            return false;
        }

        private List<(int, int)> GetValidNextSteps(int[,] map, List<(int, int)> currentPath, (int, int) currentStep)
        {
            for(int i = currentStep.Item1 - 1; i <= currentStep.Item1 + 1; i++)
            {
                if (i < 0) continue;
                for(int j = currentStep.Item2; i <= currentStep.Item2 + 1; i++)
                {
                    if (j < 0) continue;

                    if (map[(i, j)] == EndValue || map[]) { }
                }
            }
        }
    }
}