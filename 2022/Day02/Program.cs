using System.Diagnostics;

namespace Day02
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Day 02!");

            //string inputFile = @"..\..\..\sample_input.txt";
            string inputFile = @"..\..\..\final_input.txt";
            var input = File.ReadAllLines(inputFile).ToList();

            var timer = new Stopwatch();
            timer.Start();

            var numberMap = new Dictionary<char, int>
            {
                {'A', 0},
                {'B', 1},
                {'C', 2},
                {'X', 0},
                {'Y', 1},
                {'Z', 2}
            };

            var pointsMap = new Dictionary<char, int>
            {
                {'X', 1},
                {'Y', 2},
                {'Z', 3}
            };

            var resultMap = new int[3, 3];
            resultMap[0, 0] = 3;
            resultMap[0, 1] = 6;
            resultMap[0, 2] = 0;
            resultMap[1, 0] = 0;
            resultMap[1, 1] = 3;
            resultMap[1, 2] = 6;
            resultMap[2, 0] = 6;
            resultMap[2, 1] = 0;
            resultMap[2, 2] = 3;

            var pointsMap2 = new Dictionary<char, int>
            {
                {'A', 1},
                {'B', 2},
                {'C', 3}
            };

            var resultMap2 = new Dictionary<char, int>
            {
                {'X', 0},
                {'Y', 3},
                {'Z', 6}
            };

            var strategyMap2 = new char[3, 3];
            strategyMap2[0, 0] = 'C';
            strategyMap2[0, 1] = 'A';
            strategyMap2[0, 2] = 'B';
            strategyMap2[1, 0] = 'A';
            strategyMap2[1, 1] = 'B';
            strategyMap2[1, 2] = 'C';
            strategyMap2[2, 0] = 'B';
            strategyMap2[2, 1] = 'C';
            strategyMap2[2, 2] = 'A';

            var answer1 = 0;
            var answer2 = 0;

            input.ForEach(x =>
            {
                var arr = x.ToCharArray();
                var o = arr[0];
                var m = arr[2];

                answer1 += pointsMap[m];
                answer1 += resultMap[numberMap[o], numberMap[m]];

                var m2 = strategyMap2[numberMap[o], numberMap[m]];
                answer2 += pointsMap2[m2];
                answer2 += resultMap2[m];
            });

            var answer1Time = timer.ElapsedMilliseconds;
            timer.Restart();

            var answer2Time = timer.ElapsedMilliseconds;

            Console.WriteLine($"Answer1 = {answer1}; Time Taken = {answer1Time}ms");
            Console.WriteLine($"Answer2 = {answer2}; Time Taken = {answer2Time}ms");
        }
    }
}