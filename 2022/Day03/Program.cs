using System.Diagnostics;

namespace Day03
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Day 03!");

            //string inputFile = @"..\..\..\sample_input.txt";
            string inputFile = @"..\..\..\final_input.txt";
            var input = File.ReadAllLines(inputFile).ToList();

            var timer = new Stopwatch();
            timer.Start();


            var rucksacksByCompartments = input.Select(x => x.Chunk(x.Length / 2).Select(x => new string(x)));

            var matchingItems = new List<char>();
            rucksacksByCompartments.ToList().ForEach(x => matchingItems.Add(FindCommonChar(x.ToList())));

            var answer1 = CalculatePriorities(matchingItems);
            var answer1Time = timer.ElapsedMilliseconds;
            Console.WriteLine($"Answer1 = {answer1}; Time Taken = {answer1Time}ms");

            timer.Restart();
            var groups = input.Chunk(3).ToList();
            var badges = new List<char>();
            groups.ForEach(x => badges.Add(FindCommonChar(x.ToList())));

            var answer2 = CalculatePriorities(badges);
            var answer2Time = timer.ElapsedMilliseconds;
            Console.WriteLine($"Answer2 = {answer2}; Time Taken = {answer2Time}ms");
        }

        static char FindCommonChar(List<string> items)
        {
            var common = items[0].ToCharArray();
            items.ForEach(x => common = common.Intersect(x).ToArray());
            return common[0];
        }

        static int CalculatePriorities(List<Char> items)
        {
            return items.Select(x => (int)x >= (int)'a' ? (int)x - 96 : (int)x - 38).Sum();
        }
    }
}