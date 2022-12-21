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
            var answer2 = WhatNumberToYell(input, "humn");
            var answer2Time = timer.ElapsedMilliseconds;
            Console.WriteLine($"Answer2 = {answer2}; Time Taken = {answer2Time} ms");
        }

        // Part 2
        static long WhatNumberToYell(List<string> input, string you)
        {
            MonkeyNumbers = InitializeMonkeys(input);
            return GetYourNumber(MonkeyNumbers["root"].Item1, you, 0);
        }

        static long GetYourNumber(Monkey m, string you, long balance)
        {
            var m1 = MonkeyNumbers[m.Monkey1].Item1;
            var m2 = MonkeyNumbers[m.Monkey2].Item1;

            long resolvedNumber;
            Monkey dependentMonkey;
            bool foundYou = false;
            Monkey monkeyToResolve = null;

            if (m1.Name == you)
            {
                foundYou = true;
                dependentMonkey = m1;
                monkeyToResolve = m2;
            }
            else if (m2.Name == you)
            {
                foundYou = true;
                dependentMonkey = m2;
                monkeyToResolve = m1;
            }
            else if (DependsOn(m1, you))
            {
                dependentMonkey = m1;
                monkeyToResolve = m2;
            }
            else
            {
                dependentMonkey = m2;
                monkeyToResolve = m1;
            }

            resolvedNumber = monkeyToResolve.DependsOnOthers ? ResolveNumberForMonkey(monkeyToResolve) : monkeyToResolve.Number;

            balance = balance == 0 ? resolvedNumber : CalculateBalance(m, monkeyToResolve.Name, resolvedNumber, balance);

            if (foundYou)
            {
                return balance;
            }

            return GetYourNumber(dependentMonkey, you, balance);
        }

        private static long CalculateBalance(Monkey m, string resolvedMonkey, long resolvedNumber, long forceNumber)
        {
            bool m1Resolved = m.Monkey1 == resolvedMonkey;
            long balanceNumber = 0;

            Func<long, long, long>? reverseOperation;

            switch(m.OperationType)
            {
                case OperationType.Add:
                    balanceNumber = forceNumber - resolvedNumber;
                    break;
                case OperationType.Subtract:
                    balanceNumber = m1Resolved ? resolvedNumber - forceNumber : resolvedNumber + forceNumber;
                    break;
                case OperationType.Multiply:
                    balanceNumber = forceNumber / resolvedNumber;
                    break;
                case OperationType.Divide:
                default:
                    balanceNumber = m1Resolved ? forceNumber / resolvedNumber : forceNumber * resolvedNumber;
                    break;
            }

            return balanceNumber;
        }

        // Part 1
        static long GetRootNumber(List<string> input)
        {
            MonkeyNumbers = InitializeMonkeys(input);
            return ResolveNumberForMonkey(MonkeyNumbers["root"].Item1);
        }

        static bool DependsOn(Monkey monkey, string other)
        {
            if (!monkey.DependsOnOthers) return false;
            if (monkey.Monkey1 == other || monkey.Monkey2 == other) return true;
            return DependsOn(MonkeyNumbers[monkey.Monkey1].Item1, other) || DependsOn(MonkeyNumbers[monkey.Monkey2].Item1, other);
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
                OperationType operationType = OperationType.Add;
                if (dependsOnOthers)
                {
                    switch (parts[2])
                    {
                        case "+":
                            operation = Add;
                            operationType = OperationType.Add;
                            break;
                        case "-":
                            operation = Subtract;
                            operationType = OperationType.Subtract;
                            break;
                        case "*":
                            operation = Multiply;
                            operationType = OperationType.Multiply;
                            break;
                        case "/":
                            operation = Divide;
                            operationType = OperationType.Divide;                                                                    
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
                    Operation = operation,
                    OperationType = operationType
                };
                monkeys.Add(monkey.Name, (monkey, !dependsOnOthers));
            });
            return monkeys;
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
        public OperationType OperationType {get; set;}
        public Func<long, long, long>? Operation { get; set; }

    }

    enum OperationType { Add, Subtract, Multiply, Divide }
}