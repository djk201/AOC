using Shared;
using System.Diagnostics;

namespace Day05
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Day 05!");

            //string inputFile = @"..\..\..\sample_input.txt";
            string inputFile = @"..\..\..\final_input.txt";
            var input = File.ReadAllLines(inputFile).ToList();

            Run(input);
        }
        private static void Run(List<string> input)
        {
            var timer = new Stopwatch();
            timer.Start();

            var chunks = input.ChunkBy(string.Empty);
            var stackInput = chunks[0].ToList();
            var movementsInput = chunks[1].ToList();

            var lastLine = stackInput[stackInput.Count - 1].Trim();
            var maxStacks = int.Parse((lastLine[lastLine.Length - 1]).ToString());

            // Part 1
            List<Stack<string>> stacks = CreateStackFromInput(stackInput, maxStacks);
            List<List<int>> movements = CreateMovementsFromInput(movementsInput);
            string answer1 = ProcessMovementsForPart1(stacks, movements);
            var answer1Time = timer.ElapsedMilliseconds;
            Console.WriteLine($"Answer1 = {answer1}; Time Taken = {answer1Time} ms");

            // Part 2
            timer.Restart();
            stacks = CreateStackFromInput(stackInput, maxStacks);
            string answer2 = ProcessMovementsForPart2(stacks, movements);
            var answer2Time = timer.ElapsedMilliseconds;
            Console.WriteLine($"Answer2 = {answer2}; Time Taken = {answer2Time} ms");
        }

        private static string ProcessMovementsForPart2(List<Stack<string>> stacks, List<List<int>> movements)
        {
            movements.ForEach(x =>
            {
                var cratesToMove = new Stack<string>();
                for (var i = 0; i < x[0]; i++)
                {
                    cratesToMove.Push(stacks[x[1] - 1].Pop());
                }
                for (var i = 0; i < x[0]; i++)
                {
                    stacks[x[2] - 1].Push(cratesToMove.Pop());
                }
            });

            var result = string.Empty;

            stacks.ForEach(x => result += x.Peek());
            return result;
        }

        private static string ProcessMovementsForPart1(List<Stack<string>> stacks, List<List<int>> movements)
        {
            movements.ForEach(x =>
            {
                for (var i = 0; i < x[0]; i++)
                {
                    var crateToMove = stacks[x[1] - 1].Pop();
                    stacks[x[2] - 1].Push(crateToMove);
                }
            });

            var answer1 = string.Empty;

            stacks.ForEach(x => answer1 += x.Peek());
            return answer1;
        }

        private static List<List<int>> CreateMovementsFromInput(List<string> movementsInput)
        {
            var movements = new List<List<int>>();

            movementsInput.ForEach(x =>
            {
                x = x.Replace("move ", string.Empty);
                x = x.Replace("from ", string.Empty);
                x = x.Replace("to ", string.Empty);

                var movement = x.Split(' ').ToListOfTypeInt();
                movements.Add(movement);
            });
            return movements;
        }

        private static List<Stack<string>> CreateStackFromInput(List<string> stackInput, int maxStacks)
        {
            var stacks = new List<Stack<string>>();

            // initialize stacks
            for (var i = 0; i < maxStacks; i++)
            {
                stacks.Add(new Stack<string>());
            }

            // create stacks
            for (var i = stackInput.Count() - 2; i >= 0; i--)
            {
                var crateIndex = 1;
                for (var j = 0; j < maxStacks; j++)
                {
                    var crate = stackInput[i].Substring(crateIndex, 1);
                    if (!string.IsNullOrWhiteSpace(crate)) stacks[j].Push(crate);
                    crateIndex += 4;
                }
            }

            return stacks;
        }
    }
}