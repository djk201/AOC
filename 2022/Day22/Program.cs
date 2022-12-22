using Shared;
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

            //string inputFile = @"..\..\..\sample_input.txt";
            string inputFile = @"..\..\..\final_input.txt";
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
            var instructions = new List<Instruction>();
            int[,] map = ReadInput(input, instructions, cursor);
            cursor.FacingSide =  Facing.Right;

            instructions.ForEach(i =>
            {
                cursor = Move(map, i, cursor);
            });

            return 1000 * (cursor.Row + 1) + 4 * (cursor.Col + 1) + (long)cursor.FacingSide;
        }

        private static Cursor Move(int[,] map, Instruction i, Cursor cursor)
        {
            if (i.Direction != ' ') cursor.Turn(i.Direction);

            for(int x=0; x< i.Steps; x++)
            {
                Cursor next = GetNextStep(map, cursor);

                if (map[next.Row, next.Col] == 0)
                {
                    while (map[next.Row, next.Col] == 0)
                    {
                        next = GetNextStep(map, next);
                    }
                }
                if (map[next.Row, next.Col] == 2) break; // Wall

                if (map[next.Row, next.Col] == 1)
                {
                    cursor = next;
                    continue;
                }
            }
            return cursor;
        }

        private static Cursor GetNextStep(int[,] map, Cursor cursor)
        {
            int rows = map.GetLength(0);
            int cols = map.GetLength(1);

            var next = new Cursor { Row = cursor.Row, Col = cursor.Col, FacingSide = cursor.FacingSide};

            switch (next.FacingSide)
            {
                case Facing.Right:
                    next.Col = next.Col == cols -1 ? 0 : next.Col + 1;
                    break;
                case Facing.Left:
                    next.Col = next.Col == 0 ? cols - 1 : next.Col - 1;
                    break;
                case Facing.Down:
                    next.Row = next.Row == rows - 1 ? 0 : next.Row + 1;
                    break;
                case Facing.Up:
                    next.Row = next.Row == 0 ? rows - 1 : next.Row - 1;
                    break;
            }
            return next;
        }

        private static int[,] ReadInput(List<string> input, List<Instruction> instructions, Cursor cursor)
        {
            var chunks = input.ChunkBy(string.Empty);
            var maxColumns = chunks.First().Select(l => l.Length).Max();
            int[,] map = new int[chunks[0].Length, maxColumns];

            cursor.Col = -1;

            for (int r = 0; r < chunks[0].Length; r++)
            {
                for(int c = 0; c < chunks[0][r].Length; c++)
                {
                    var val = chunks[0][r][c];
                    if (cursor.Col == -1 && val != ' ') cursor.Col = c - 1;
                    map[r, c] = val == ' ' ? 0 : val == '.' ? 1 : 2;
                }
            }

            char direction = ' ';
            string currentNum = string.Empty;
            for (int i = 0; i < chunks[1][0].Length; i++)
            {
                if (chunks[1][0][i] == 'R' || chunks[1][0][i] == 'L')
                {
                    instructions.Add(new Instruction { Direction = direction, Steps = int.Parse(currentNum) });
                    direction = chunks[1][0][i];
                    currentNum = string.Empty;
                    continue;
                }
                currentNum = currentNum + chunks[1][0][i];
            }

            instructions.Add(new Instruction { Direction = direction, Steps = int.Parse(currentNum) });

            return map;
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