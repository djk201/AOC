using Shared;
using System.Diagnostics;
namespace Day07
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Day 07!");

            //string inputFile = @"..\..\..\sample_input.txt";
            string inputFile = @"..\..\..\final_input.txt";
            var input = File.ReadAllLines(inputFile).ToList();

            Run(input);
        }

        private static void Run(List<string> input)
        {
            var timer = new Stopwatch();
            timer.Start();

            var chunks = input.ChunkBy("$ ls");

            var rootNode = new Node { Type = Type.Directory, Level = 1 };
            var directories = new List<Node> { rootNode };
            var currentNode = rootNode;

            for(var i = 1; i < chunks.Count; i++)
            {
                chunks[i].ToList().ForEach(x =>
                {
                    var parts = x.Split(' ');

                    switch(parts[0])
                    {
                        case "$": // cd
                            if (parts[2] == "..") currentNode = currentNode.Parent;
                            else
                            {
                                var dirNode = new Node { Type = Type.Directory, Parent = currentNode, Level = currentNode.Level + 1 };
                                currentNode.ChildNodes.Add(dirNode);
                                currentNode = dirNode;
                                directories.Add(dirNode);
                            }
                            break;
                        case "dir":
                            // do nothing
                            break;
                        default:
                            var fileNode = new Node { Size = int.Parse(parts[0]), Type = Type.File, Parent = currentNode };
                            currentNode.ChildNodes.Add(fileNode);
                            break;
                    }
                });
            }

            // Calculate Size of all directories
            directories.OrderByDescending(x => x.Level).ToList().ForEach(o => o.Size = o.ChildNodes.Sum(a => a.Size));

            var answer1 = directories.Where(x => x.Size <= 100000).Sum(y => y.Size);
            var answer1Time = timer.ElapsedMilliseconds;
            Console.WriteLine($"Answer1 = {answer1}; Time Taken = {answer1Time} ms");

            timer.Restart();

            var requiredSpace = 30000000 - (70000000 - rootNode.Size);
            var answer2 = directories.OrderBy(x => x.Size).First(y => y.Size >= requiredSpace).Size;

            var answer2Time = timer.ElapsedMilliseconds;
            Console.WriteLine($"Answer2 = {answer2}; Time Taken = {answer2Time} ms");
        }

    }

    class Node
    {
        public Type Type { get; set; }
        public int Size { get; set; }
        public List<Node> ChildNodes { get; set; }
        public Node Parent { get; set; }
        public int Level { get; set; }

        public Node()
        {
            ChildNodes = new List<Node>();
        }
    }

    public enum Type { Directory, File }

}