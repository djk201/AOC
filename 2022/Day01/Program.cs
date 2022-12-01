using System.Diagnostics;

Console.WriteLine("Day 01!");

//string inputFile = @"..\..\..\sample_input.txt";
string inputFile = @"..\..\..\final_input.txt";
var input = File.ReadAllLines(inputFile).ToList();

var timer = new Stopwatch();
timer.Start();

var elves = new List<int>();
var calories = 0;
var elfCounter = 1;

for (var i = 0; i < input.Count; i++)
{
    if (string.IsNullOrWhiteSpace(input[i]))
    {
        elves.Add(calories);
        calories = 0;
        elfCounter++;
        continue;
    }

    calories += int.Parse(input[i]);
}

var answer1 = elves.Max();
var answer1Time = timer.ElapsedMilliseconds;
timer.Restart();
var answer2 = elves.OrderByDescending(z => z).Take(3).Sum();
var answer2Time = timer.ElapsedMilliseconds;

Console.WriteLine($"Answer1 = {answer1}; Time Taken = {answer1Time}ms");
Console.WriteLine($"Answer2 = { answer2}; Time Taken = {answer2Time}ms");