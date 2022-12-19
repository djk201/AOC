using System.Diagnostics;
namespace Day19
{
    internal class Program
    {
        static int MaxMins = 24;
        static void Main(string[] args)
        {
            Console.WriteLine("Day 19!");

            string inputFile = @"..\..\..\sample_input.txt";
            //string inputFile = @"..\..\..\final_input.txt";
            var input = File.ReadAllLines(inputFile).ToList();

            Run(input);
        }

        private static void Run(List<string> input)
        {
            var timer = new Stopwatch();
            timer.Start();

            var answer1 = GetQualityLevel(input);
            var answer1Time = timer.ElapsedMilliseconds;
            Console.WriteLine($"Answer1 = {answer1}; Time Taken = {answer1Time} ms");

            timer.Restart();
            long answer2 = Part2(input);
            var answer2Time = timer.ElapsedMilliseconds;
            Console.WriteLine($"Answer2 = {answer2}; Time Taken = {answer2Time} ms");
        }

        static long Part2(List<string> input)
        {
            MaxMins = 32;
            var blueprints = GetBluePrints(input);
            long result = 1;
            int blueprintId = 0;

            foreach (var blueprint in blueprints)
            {
                blueprintId++;
                int maxGeodes = GetMaxGeodesForBlueprint(blueprint);

                result = result * maxGeodes;
            }
            return result;
        }

        static int GetQualityLevel(List<string> input)
        {
            var blueprints = GetBluePrints(input);

            var qualityLevel = 0;
            int blueprintId = 0;

            foreach (var blueprint in blueprints)
            {
                blueprintId++;
                int maxGeodes = GetMaxGeodesForBlueprint(blueprint);

                qualityLevel += (maxGeodes * blueprintId);
            }
            return qualityLevel;
        }

        private static int GetMaxGeodesForBlueprint(Blueprint blueprint)
        {
            var result = CalculateMaxGeodes(blueprint, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0);

            return result;
        }

        static int CalculateMaxGeodes(Blueprint b, int or, int cr, int obr, int gr, int o, int c, int ob, int g, int mins, int maxGeodes)
        {
            if (mins == MaxMins)
            {
                maxGeodes = Math.Max(maxGeodes, g);
                return maxGeodes;
            }

            int minsLeft = MaxMins - mins;
            int maxGeodesPossible = g;
            for (int i = 0; i < minsLeft; i++)
                maxGeodesPossible += g + i;
            if (maxGeodesPossible < maxGeodes)
            {
                Console.WriteLine("maxGeodesPossible breached");
                return 0;
            }

            int no = o + or;
            int nc = c + cr;
            int nob = ob + obr;
            int ng = g + gr;


            if (o >= b.GRCostO && ob >= b.GRCostOb)
            {
                return CalculateMaxGeodes(b, or, cr, obr, gr + 1, no - b.GRCostO, nc, nob - b.GRCostOb, ng, mins + 1, maxGeodes);
            }

            if (cr >= b.ObRCostC && obr < b.GRCostOb && o >= b.ObRCostO && c >= b.ObRCostC)
            {
                return CalculateMaxGeodes(b, or, cr, obr + 1, gr, no - b.ObRCostO, nc - b.ObRCostC, nob, ng, mins + 1, maxGeodes);
            }

            int max = 0;

            //if not too many obsidian bots and enough to make one, make one
            if (obr < b.GRCostOb && o >= b.ObRCostO && c >= b.ObRCostC)
            {
                max = Math.Max(max, CalculateMaxGeodes(b, or, cr, obr + 1, gr, no - b.ObRCostO, nc - b.ObRCostC, nob, ng, mins + 1, maxGeodes));
            }
            //if not too many clay bots and enough to make one, make one
            if (cr < b.ObRCostC && o >= b.CRCostO)
            {
                max = Math.Max(max, CalculateMaxGeodes(b, or, cr + 1, obr, gr, no - b.CRCostO, nc, nob, ng, mins + 1, maxGeodes));
            }
            //if not too many ore bots and enough to make one, make one
            if (or < 4 && o >= b.ORCostO)
            {
                max = Math.Max(max, CalculateMaxGeodes(b, or + 1, cr, obr, gr, no - b.ORCostO, nc, nob, ng, mins + 1, maxGeodes));
            }
            //if not holding on to more ore than maximum bot cost, wait and see if we can make a better bot later
            if (o <= 4)
            {
                max = Math.Max(max, CalculateMaxGeodes(b, or, cr, obr, gr, no, nc, nob, ng, mins + 1, maxGeodes));
            }

            return max;
        }

        static List<Blueprint> GetBluePrints(List<string> input)
        {
            var blueprints = new List<Blueprint>();

            input.ForEach(l =>
            {
                var parts = l.Split(": ")[1].Split(". ").Select(x => x.Split(" ")).ToArray();
                blueprints.Add(new Blueprint
                {
                    ORCostO = int.Parse(parts[0][4]),
                    CRCostO = int.Parse(parts[1][4]),
                    ObRCostO = int.Parse(parts[2][4]),
                    ObRCostC = int.Parse(parts[2][7]),
                    GRCostO = int.Parse(parts[3][4]),
                    GRCostOb = int.Parse(parts[3][7])
                });
            });

            return blueprints;
        }

        class Resources
        {
            public int OR { get; set; }
            public int CR { get; set; }
            public int ObR { get; set; }
            public int GR { get; set; }
            public int O { get; set; }
            public int C { get; set; }
            public int Ob { get; set; }
            public int G { get; set; }

            public Resources() { }
            public Resources(int or, int cr, int obr, int gr, int o, int c, int ob, int g)
            {
                OR = or; CR = cr; ObR = obr; GR = gr; O = o; C = c; Ob = ob; G = g; 
            }

            public Resources GenerateResources => new Resources(OR, CR, ObR, GR, O+OR, C+CR, Ob+ObR, G+GR);
            public Resources Copy => new Resources(OR, CR, ObR, GR, O, C, Ob, G);
            public Resources CreateGR(Blueprint b)
            {
                var nr = Copy;
                nr.O -= b.GRCostO;
                nr.Ob -= b.GRCostOb;
                nr.GR += 1;
                return nr;
            }
            public Resources CreateObR(Blueprint b)
            {
                var nr = Copy;
                nr.O -= b.ObRCostO;
                nr.C -= b.ObRCostC;
                nr.ObR += 1;
                return nr;
            }
            public Resources CreateCR(Blueprint b)
            {
                var nr = Copy;
                nr.O -= b.CRCostO;
                nr.CR += 1;
                return nr;
            }
            public Resources CreateOR(Blueprint b)
            {
                var nr = Copy;
                nr.O -= b.ORCostO;
                nr.OR += 1;
                return nr;
            }
        }


        class Blueprint
        {
            public int ORCostO { get; set; }
            public int CRCostO { get; set; }
            public int ObRCostO { get; set; }
            public int ObRCostC { get; set; }
            public int GRCostO { get; set; }
            public int GRCostOb { get; set; }
        }
    }
}