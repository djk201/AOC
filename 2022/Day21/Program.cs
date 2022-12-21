using System.Diagnostics;
namespace Day21
{
    internal class Program
    {
        static Dictionary<string, (Monkey, bool)> MonkeyNumbers;
        static void Main(string[] args)
        {
            Console.WriteLine("Day 21!");

            //string inputFile = @"..\..\..\sample_input.txt";
            string inputFile = @"..\..\..\final_input.txt";
            var input = File.ReadAllLines(inputFile).ToList();

            Run(input);
        }

        private static void Run(List<string> input)
        {
            var timer = new Stopwatch();
            timer.Start();

            var answer1 = GetRootNumber(input);
            var answer1Time = timer.ElapsedMilliseconds;
            Console.WriteLine($"Answer1 = {answer1}; Time Taken = {answer1Time} ms");

            timer.Restart();
            var answer2 = 0;
            var answer2Time = timer.ElapsedMilliseconds;
            Console.WriteLine($"Answer2 = {answer2}; Time Taken = {answer2Time} ms");
        }

        static long GetRootNumber(List<string> input)
        {
            MonkeyNumbers = InitializeMonkeys(input);
            return ResolveNumberForMonkey(MonkeyNumbers["root"].Item1);
        }

        private static Dictionary<string, (Monkey, bool)> InitializeMonkeys(List<string> input)
        {
            Dictionary<string, (Monkey, bool)> monkeys = new Dictionary<string, (Monkey, bool)>();
            input.ForEach(l =>
            {
                // root: pppw + sjmn
                // dbpl: 5
                var parts = l.Split(" ");
                bool dependsOnOthers = parts.Length > 2 ? true : false;
                Func<long, long, long> operation = null;
                if (dependsOnOthers)
                {
                    switch (parts[2])
                    {
                        case "+":
                            operation = Add;
                            break;
                        case "-":
                            operation = Subtract;
                            break;
                        case "*":
                            operation = Multiply;
                            break;
                        case "/":
                            operation = Divide;
                            break;
                        default:
                            throw new Exception("Unexpected operand");
                            break;
                    }
                }
                var monkey = new Monkey
                {
                    Name = parts[0].Replace(":", string.Empty),
                    Monkey1 = dependsOnOthers ? parts[1] : string.Empty,
                    Monkey2 = dependsOnOthers ? parts[3] : string.Empty,
                    DependsOnOthers = dependsOnOthers,
                    Number = dependsOnOthers ? -1 : long.Parse(parts[1]),
                    Operation = operation
                };
                monkeys.Add(monkey.Name, (monkey, !dependsOnOthers));
            });
            return monkeys;
        }

        static long ResolveNumberForMonkey(Monkey monkey)
        {
            if (!monkey.DependsOnOthers) return monkey.Number;

            var m1 = MonkeyNumbers[monkey.Monkey1];
            var m2 = MonkeyNumbers[monkey.Monkey2];

            if (!m1.Item2) ResolveNumberForMonkey(m1.Item1);
            if (!m2.Item2) ResolveNumberForMonkey(m2.Item1);

            MonkeyNumbers[monkey.Name] = (monkey, true);
            monkey.Number = monkey.Operation(m1.Item1.Number, m2.Item1.Number);

            return monkey.Number;
       }
        static long Add(long num1, long num2) => num1 + num2;
        static long Subtract(long num1, long num2) => num1 - num2;
        static long Multiply(long num1, long num2) => num1 * num2;
        static long Divide(long num1, long num2) => num1 / num2;
    }

    class Monkey
    {
        public string Name { get; set; }
        public long Number { get; set; }
        public bool DependsOnOthers { get; set; }
        public string Monkey1 { get; set; }
        public string Monkey2 { get; set; }
        public Func<long, long, long>? Operation { get; set; }

    }
}