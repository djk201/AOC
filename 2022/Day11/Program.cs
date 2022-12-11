using System.Data.SqlTypes;
using System.Diagnostics;
using Shared;

namespace Day11
{
    internal class Program
    {
        static long LCM;
        static void Main(string[] args)
        {
            Console.WriteLine("Day 11!");

            //string inputFile = @"..\..\..\sample_input.txt";
            string inputFile = @"..\..\..\final_input.txt";
            var input = File.ReadAllLines(inputFile).ToList();

            Run(input);
        }

        private static void Run(List<string> input)
        {
            var timer = new Stopwatch();
            timer.Start();

            var monkeyInput = input.ChunkBy(string.Empty);
            var monkeys = monkeyInput.Select(x => InilializeMonkey(x)).ToArray();

            ProcessRounds(monkeys, 20, ProcessReliefPartOne);

            var topInspected2 = monkeys.Select(m => m.InspectedItems).OrderByDescending(i => i).Take(2).ToArray();

            var answer1 = topInspected2[0] * topInspected2[1];
            var answer1Time = timer.ElapsedMilliseconds;
            Console.WriteLine($"Answer1 = {answer1}; Time Taken = {answer1Time} ms");

            timer.Restart();

            monkeys = monkeyInput.Select(x => InilializeMonkey(x)).ToArray();
            LCM = GetLcm(monkeys.Select(m => m.TestDivisible).ToArray());
            ProcessRounds(monkeys, 10000, ProcessReliefPartTwo);
            topInspected2 = monkeys.Select(m => m.InspectedItems).OrderByDescending(i => i).Take(2).ToArray();
            var answer2 = topInspected2[0] * topInspected2[1];
            var answer2Time = timer.ElapsedMilliseconds;
            Console.WriteLine($"Answer2 = {answer2}; Time Taken = {answer2Time} ms");
        }

        static long ProcessReliefPartOne(long worryLevel) { return worryLevel / 3; }

        static long ProcessReliefPartTwo(long worryLevel) { return worryLevel % LCM; }

        public static long GetLcm(long[] numbers)
        {
            return numbers.Aggregate((S, val) => S * val / GetGcd(S, val));
        }

        private static long GetGcd(long n1, long n2) 
        { 
            return n2 == 0 ? n1 : GetGcd(n2, n1 % n2); 
        }

        static void ProcessRounds(Monkey[] monkeys, int count, Func<long, long> processRelief)
        {
            for (int i = 0; i < count; i++)
            {
                ProcessRound(monkeys, processRelief);
            }
        }

        static void ProcessRound(Monkey[] monkeys, Func<long, long> processRelief)
        {
            for (int m = 0; m < monkeys.Length; m++)
            {
                var monkey = monkeys[m];
                monkey.InspectedItems += monkey.Items.Count();
                while (monkey.Items.Any())
                {
                    var item = monkey.Items.Dequeue();
                    long operationValue = monkey.OperationValue == "old" ? item : long.Parse(monkey.OperationValue);
                    var worryLevel = monkey.OperationType == OperationType.Multiply 
                        ? item * operationValue
                        : item + operationValue;
                    worryLevel = processRelief(worryLevel);
                    var targetMonkey = monkey.ThrowToMonkey[(worryLevel % monkey.TestDivisible == 0)];
                    monkeys[targetMonkey].Items.Enqueue(worryLevel);
                }
            }
        }

        static Monkey InilializeMonkey(string[] monkeyInput)
        {
            var monkey = new Monkey();
            monkey.Items = new Queue<long>(monkeyInput[1].Trim().Split(':')[1].Trim().Split(", ").Select(x => long.Parse(x)));
            monkey.OperationType = monkeyInput[2].Trim().Split(' ')[4] == "*" ? OperationType.Multiply : OperationType.Add;
            monkey.OperationValue = monkeyInput[2].Trim().Split(' ')[5];
            monkey.TestDivisible = long.Parse(monkeyInput[3].Trim().Split(' ')[3]);
            monkey.ThrowToMonkey = new Dictionary<bool, int>
                {
                    {true, int.Parse(monkeyInput[4].Trim().Split(' ')[5]) },
                    {false, int.Parse(monkeyInput[5].Trim().Split(' ')[5]) }
                };
            return monkey;
        }
    }

    class Monkey
    {
        public Queue<long> Items { get; set; }
        public OperationType OperationType { get; set; }
        public string OperationValue { get; set; }
        public long TestDivisible { get; set; }
        public long InspectedItems { get; set; }
        public Dictionary<bool, int> ThrowToMonkey { get; set; }

        public Monkey()
        {
            Items = new Queue<long>();
            ThrowToMonkey = new Dictionary<bool, int>();
        }
    }

    public enum OperationType { Add, Multiply }
}