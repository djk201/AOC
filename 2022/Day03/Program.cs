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

            //object answer2 = null;

            var timer = new Stopwatch();
            timer.Start();

            var matchingItems = new List<char>();

            input.ForEach(x =>
            {
                var foundItem = false;
                for (var i = 0; i < x.Length / 2; i++)
                {
                    for(var j = x.Length / 2; j < x.Length; j++)
                    {
                        if (x[i] == x[j])
                        {
                            matchingItems.Add(x[i]);
                            foundItem = true;
                            break;
                        }
                    }
                    if (foundItem) break;
                }
            });

            var answer1 = CalculatePriorities(matchingItems);

            var answer1Time = timer.ElapsedMilliseconds;
            
            timer.Restart();
            var groups = input.Chunk(3);
            var badges = new List<char>();

            foreach (var group in groups)
            {
                foreach(var item1 in group[0])
                {
                    var foundBadge = false;
                    foreach (var item2 in group[1])
                    {
                        if (item1 == item2)
                        {
                            foreach (var item3 in group[2])
                            {
                                if (item2 == item3)
                                {
                                    badges.Add(item3);
                                    foundBadge = true;
                                    break;
                                }
                            }
                            if (foundBadge) break;
                        }
                    }
                    if (foundBadge) break;
                }
            }

            var answer2 = CalculatePriorities(badges);

            var answer2Time = timer.ElapsedMilliseconds;

            Console.WriteLine($"Answer1 = {answer1}; Time Taken = {answer1Time}ms");
            Console.WriteLine($"Answer2 = {answer2}; Time Taken = {answer2Time}ms");

        }

        static int CalculatePriorities(List<Char> items)
        {
            var priorities = 0;

            items.ForEach(x =>
            {
                var asciiInt = (int)x;
                if (asciiInt >= 97)
                    priorities += asciiInt - 96;
                else
                    priorities += asciiInt - 38;
            });

            return priorities;
        }
    }
}