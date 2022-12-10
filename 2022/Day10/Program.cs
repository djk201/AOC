using System.Diagnostics;
using System.Text;

namespace Day10
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Day 10!");

            //string inputFile = @"..\..\..\sample_input.txt";
            string inputFile = @"..\..\..\final_input.txt";
            var input = File.ReadAllLines(inputFile).ToList();

            Run(input);
        }

        private static void Run(List<string> input)
        {
            var timer = new Stopwatch();
            timer.Start();

            var cycle = 0;
            var X = 1;
            var signalStrength = 0;
            var sb = new StringBuilder();
            input.ForEach(x =>
            {
                cycle += 1;
                signalStrength += GetSignalStrength(cycle, X);
                sb.Append(GetCrtPixel(cycle, X));
                var parts = x.Split(' ');
                if (parts[0] == "addx")
                {
                    cycle += 1;
                    signalStrength += GetSignalStrength(cycle, X);
                    sb.Append(GetCrtPixel(cycle, X));
                    X += int.Parse(parts[1]);
                }
            });

            var answer1 = signalStrength;
            var answer1Time = timer.ElapsedMilliseconds;
            Console.WriteLine($"Answer1 = {answer1}; Time Taken = {answer1Time} ms");

            timer.Restart();
            
            var answer2 = sb.ToString();
            var answer2Time = timer.ElapsedMilliseconds;
            Console.WriteLine(answer2);
            Console.WriteLine($"Answer2 Time Taken = {answer2Time} ms");
        }

        static string GetCrtPixel(int cycle, int X)
        {
            var pos = cycle % 40;
            var result = Math.Abs(pos - (X+1)) <= 1 ? "#" : ".";
            if (cycle % 40 == 0) result += Environment.NewLine;
            return result;
        }

        static int GetSignalStrength(int cycle, int X)
        {
            var result = 0;

            if (cycle == 20 || cycle > 20 && (cycle - 20) % 40 == 0)
            {
                result = cycle * X;
            }
            return result;
        }
    }
}