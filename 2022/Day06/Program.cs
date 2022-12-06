using System.Diagnostics;
namespace Day06
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Day 06!");

            //string inputFile = @"..\..\..\sample_input.txt";
            string inputFile = @"..\..\..\final_input.txt";
            var input = File.ReadAllLines(inputFile).ToList();

            Run(input);
        }

        private static void Run(List<string> input)
        {
            var timer = new Stopwatch();
            timer.Start();

            int answer1 = FindMarker(input[0], 4);
            var answer1Time = timer.ElapsedMilliseconds;
            Console.WriteLine($"Answer1 = {answer1}; Time Taken = {answer1Time} ms");

            timer.Restart();

            var answer2 = FindMarker(input[0], 14);
            var answer2Time = timer.ElapsedMilliseconds;
            Console.WriteLine($"Answer2 = {answer2}; Time Taken = {answer2Time} ms");
        }

        private static int FindMarker(string line, int queueSize)
        {
            var answer1 = 0;
            var queue = new Queue<char>();
            for (var i = 0; i < line.Length; i++)
            {
                if (queue.Contains(line[i]))
                {
                    char dequeuedItem = ' ';
                    while (dequeuedItem != line[i])
                    {
                        dequeuedItem = queue.Dequeue();
                    }
                }
                else if (queue.Count == queueSize-1)
                {
                    answer1 = i + 1;
                    break;
                }
                queue.Enqueue(line[i]);
            }

            return answer1;
        }
    }
}