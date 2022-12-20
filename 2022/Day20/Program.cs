﻿using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;

namespace Day20
{
    internal class Program
    {
        static int TotalNodes = 0;
        static void Main(string[] args)
        {
            Console.WriteLine("Day 20!");

            string inputFile = @"..\..\..\sample_input.txt";
            //string inputFile = @"..\..\..\final_input.txt";
            var input = File.ReadAllLines(inputFile).ToList();

            Run(input);
        }

        private static void Run(List<string> input)
        {
            var timer = new Stopwatch();
            timer.Start();

            input.ForEach(x =>
            {

            });

            var answer1 = PartOne(input);
            var answer1Time = timer.ElapsedMilliseconds;
            Console.WriteLine($"Answer1 = {answer1}; Time Taken = {answer1Time} ms");

            timer.Restart();
            var answer2 = 0;
            var answer2Time = timer.ElapsedMilliseconds;
            Console.WriteLine($"Answer2 = {answer2}; Time Taken = {answer2Time} ms");
        }

        static int PartOne(List<string> input)
        {
            Node first = null, previous = null, current = null, zeroNode = null;
            var all = new List<Node>();

            foreach (var l in input)
            {
                current = new Node { Value = int.Parse(l) };
                all.Add(current);
                if (current.Value == 0) zeroNode = current;
                if (first == null)
                {
                    first = current;
                    previous = current;
                    continue;
                }
                previous.Right = current;
                current.Left = previous;
                previous = current;
            };
            current.Right = first;
            first.Left = current;
            TotalNodes = input.Count;
            all.ForEach(Mix);

            var k1 = GetNthNode(zeroNode, 1000);
            var k2 = GetNthNode(k1, 1000);
            var k3 = GetNthNode(k2, 1000);
            return k1.Value + k2.Value + k3.Value;
        }

        static void Mix(Node node)
        {
            int sign = Math.Sign(node.Value);
            if (sign == 0) return;

            var n = Math.Abs(node.Value) > TotalNodes ? Math.Abs(node.Value) % TotalNodes : Math.Abs(node.Value);

            Node target = node;
            for (int i = 0; i < n; i++)
            {
                target = (sign == -1) ? target.Left : target.Right;
            }

            // Remove the node we want to move
            var left = node.Left;
            var right = node.Right;

            left.Right = right;
            right.Left = left;

            // Insert it after or before target based on direction
            //right = target.Right;

            if (sign == -1)
            {
                left = target.Left;
                left.Right = node;
                target.Left = node;
                node.Left = left;
                node.Right = target;
            }
            else // sign is 1
            {
                right = target.Right;
                right.Left = node;
                target.Right = node;
                node.Left = target;
                node.Right = right;
            }
        }

        static Node GetNthNode(Node from, int n)
        {
            n = n > TotalNodes ? n % TotalNodes : n;

            Node current = from;
            for (int i = 0; i < n; i++)
            {
                current = current.Right;
            }

            return current;
        }
    }

    class Node
    {
        public int Value { get; set; }
        public Node? Left { get; set; }
        public Node? Right { get; set; }
    }
}