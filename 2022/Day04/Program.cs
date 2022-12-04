using System.Diagnostics;

namespace Day04
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Day 04!");

            //string inputFile = @"..\..\..\sample_input.txt";
            string inputFile = @"..\..\..\final_input.txt";
            var input = File.ReadAllLines(inputFile).ToList();

            Run(input);
        }

        private static void Run(List<string> input)
        {
            var timer = new Stopwatch();
            timer.Start();

            var sectionsAssignments = new List<int[]>();
            input.ForEach(x =>
            {
                var pairs = x.Split(',');
                var assignment1 = pairs[0].Split('-');
                var assignment2 = pairs[1].Split('-');
                var assignmentPair = new int[4];
                assignmentPair[0] = int.Parse(assignment1[0]);
                assignmentPair[1] = int.Parse(assignment1[1]);
                assignmentPair[2] = int.Parse(assignment2[0]);
                assignmentPair[3] = int.Parse(assignment2[1]);
                sectionsAssignments.Add(assignmentPair);
            });

            var answer1 = sectionsAssignments.Count(x => (x[2] >= x[0] && x[3] <= x[1]) || (x[0] >= x[2] && x[1] <= x[3]));

            var answer1Time = timer.ElapsedMilliseconds;
            timer.Restart();

            var answer2 = sectionsAssignments.Count(x => 
                (x[0] >= x[2] && x[0] <= x[3]) 
                || (x[1] >= x[2] && x[1] <= x[3])
                || (x[2] >= x[0] && x[2] <= x[1])
                || (x[3] >= x[0] && x[3] <= x[1]));
            var answer2Time = timer.ElapsedMilliseconds;

            Console.WriteLine($"Answer1 = {answer1}; Time Taken = {answer1Time}ms");
            Console.WriteLine($"Answer2 = {answer2}; Time Taken = {answer2Time}ms");
        }
    }
}