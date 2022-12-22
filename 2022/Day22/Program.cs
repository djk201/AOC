using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Day22
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Day 22!");

            string inputFile = @"..\..\..\sample_input.txt";
            //string inputFile = @"..\..\..\final_input.txt";
            var input = File.ReadAllLines(inputFile).ToList();

            Run(input);
        }

        private static void Run(List<string> input)
        {
            var timer = new Stopwatch();
            timer.Start();

            long answer1 = GetFinalPassword(input);
            var answer1Time = timer.ElapsedMilliseconds;
            Console.WriteLine($"Answer1 = {answer1}; Time Taken = {answer1Time} ms");

            timer.Restart();
            var answer2 = 0;
            var answer2Time = timer.ElapsedMilliseconds;
            Console.WriteLine($"Answer2 = {answer2}; Time Taken = {answer2Time} ms");
        }

        private static long GetFinalPassword(List<string> input)
        {
            // map values - 0=Empty, 1=Open, 2=Wall
            var cursor = new Cursor();
            int[,] map = ReadInput(input, out List<Instruction> instructions, cursor);
            cursor.FacingSide =  Facing.Right;

            instructions.ForEach(i =>
            {
                Move(map, i, cursor);
            });

            return 1000 * cursor.Row + 4 * cursor.Col + (long)cursor.FacingSide;
        }

        private static void Move(int[,] map, Instruction i, Cursor cursor)
        {
            int rows = map.GetLength(0);
            int cols = map.GetLength(1);
            cursor.Turn(i.Direction);

            for(int x=0; x< i.Steps; x++)
            {
                Cursor next = GetNextStep(map, cursor, i.Direction);

                if (map[next.Row, next.Col] == 0)
                {
                    while (map[next.Row, next.Col] != 0)
                    {
                        next = GetNextStep(map, next, i.Direction);
                    }
                }
                if (map[next.Row, next.Col] == 2) break; // Wall

                if (map[next.Row, next.Col] == 1)
                {
                    cursor = next;
                    continue;
                }
            }
        }

        private static Cursor GetNextStep(int[,] map, Cursor cursor, char direction)
        {
            throw new NotImplementedException();
        }

        private static int[,] ReadInput(List<string> input, out List<Instruction> instructions, Cursor cursor)
        {
            var maxColumns = input.Select(l => l.Length).Max();
            input.ForEach(l =>
            {
                
            });

            throw new NotImplementedException();
        }

        class Instruction
        {
            public char Direction { get; set; }
            public int Steps { get; set; }
        }

        class Cursor
        {
            public int Row { get; set; }
            public int Col { get; set; }
            public Facing FacingSide { get; set; }

            public void Turn(char direction)
            {
                int side = (int)FacingSide;
                switch (direction)
                {
                    case 'R':
                        side = side == 3 ? side = 0 : side + 1;
                        break;
                    case 'L':
                    default:
                        side = side == 0 ? side = 3 : side - 1;
                        break;
                }
                FacingSide = (Facing)side;
            }
        }

        enum Facing { Right = 0, Down = 1, Left = 2, Up = 3}
    }
}