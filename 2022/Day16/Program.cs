using System.Diagnostics;
namespace Day16
{
    internal class Program
    {
        static List<Valve> AllValves;
        static Dictionary<string, (Valve, bool)> ValvesState = new Dictionary<string, (Valve, bool)>();
        static Dictionary<(string, string), long> ValveDistance = new Dictionary<(string, string), long>();
        static int MaxMins = 0;
        static int Actors = 1;
        static void Main(string[] args)
        {
            Console.WriteLine("Day 16!");

            string inputFile = @"..\..\..\sample_input.txt";
            //string inputFile = @"..\..\..\final_input.txt";
            var input = File.ReadAllLines(inputFile).ToList();

            Run(input);
            //new Solution2().Run(input);
        }

        private static void Run(List<string> input)
        {
            var timer = new Stopwatch();
            timer.Start();
            var answer1 = GetMostPressurePossible(input, 30, 1);
            var answer1Time = timer.ElapsedMilliseconds;
            Console.WriteLine($"Answer1 = {answer1}; Time Taken = {answer1Time} ms");

            timer.Restart();
            var answer2 = GetMostPressurePossibleP2(input, 30, 2);
            var answer2Time = timer.ElapsedMilliseconds;
            Console.WriteLine($"Answer2 = {answer2}; Time Taken = {answer2Time} ms");
        }
        static long GetMostPressurePossibleP2(List<string> input, int maxMins, int numOfActors)
        {
            AllValves = InitializeValves(input);
            ValveDistance = CalculateDistances(AllValves.Where(x => x.FlowRate > 0 || x.Name == "AA").ToList(), AllValves.Where(x => x.FlowRate > 0).ToList());
            ValvesState = AllValves.Where(x => x.FlowRate > 0).ToDictionary(v => v.Name, v => (v, false));
            MaxMins = maxMins;

            var actors = new int[numOfActors];
            var result = CalculateMaxPressureP2("AA", 0, 0, 0, 0);
            return result;
        }


        static long CalculateMaxPressureP2(string current, long mins, long flowRate, long pressure, long maxPressure)
        {
            if (mins > MaxMins)
            {
                return 0;
            }
            if (mins == MaxMins)
            {
                maxPressure = Math.Max(maxPressure, pressure);
                //Console.WriteLine($"MaxPressure = {maxPressure}");
                return maxPressure;
            }

            var openedValves = ValvesState.Values.Where(x => x.Item2);
            var newPressure = pressure + openedValves.Sum(x => x.Item1.FlowRate);

            if (ValvesState.Values.All(x => x.Item2)) // All Valves are already open
            {
                var remainingMins = MaxMins - mins;
                return CalculateMaxPressureP2(current, mins + remainingMins, flowRate, newPressure + (flowRate * (remainingMins - 1)), maxPressure);
            }

            // Lets open current valve and calculate new flowRate
            if (current != "AA" && !ValvesState[current].Item2)
            {
                var currentValve = ValvesState[current].Item1;
                ValvesState[current] = (currentValve, true);
                //Console.WriteLine($"Valve {current.Name} OPENED");
                return CalculateMaxPressureP2(current, mins + 1, flowRate + currentValve.FlowRate, newPressure, maxPressure);
            }

            //var currentValve = ValvesState[current].Item1;

            long max = 0;

            foreach (var valve in ValvesState.Where(x => !x.Value.Item2))
            {
                //var minsToReach = valve.Value.Item1.ValvesToOpen[valve.Key];
                var minsToReach = ValveDistance[(current, valve.Key)];
                //Console.WriteLine($"Going from Valve {current.Name} to {valve.Key}");
                max = Math.Max(max, CalculateMaxPressure(valve.Value.Item1, mins + minsToReach, flowRate, newPressure + (flowRate * (minsToReach - 1)), maxPressure));

                // Close the valve so other paths can use it
                ValvesState[valve.Key] = (valve.Value.Item1, false);
                //Console.WriteLine($"Valve {valve.Key} CLOSED");
            }
            return max;
        }

        static long GetMostPressurePossible(List<string> input, int maxMins, int actors)
        {
            Actors = actors;
            AllValves = InitializeValves(input);
            ValveDistance = CalculateDistances(AllValves.Where(x => x.FlowRate > 0 || x.Name == "AA").ToList(), AllValves.Where(x => x.FlowRate > 0).ToList());
            ValvesState = AllValves.Where(x => x.FlowRate > 0).ToDictionary(v => v.Name, v => (v, false));
            //ValvesToOpenList = valves.Where(v => v.FlowRate > 0).ToList();
            MaxMins = maxMins;
            var start = AllValves.First(v => v.Name == "AA");

            var result = CalculateMaxPressure(start, 0, 0, 0, 0);
            return result;
        }

        static long CalculateMaxPressure(Valve current, long mins, long flowRate, long pressure, long maxPressure)
        {
            if (mins > MaxMins)
            {
                return 0;
            }
            if (mins == MaxMins)
            {
                maxPressure = Math.Max(maxPressure, pressure);
                //Console.WriteLine($"MaxPressure = {maxPressure}");
                return maxPressure;
            }

            var openedValves = ValvesState.Values.Where(x => x.Item2);
            var newPressure = pressure + openedValves.Sum(x => x.Item1.FlowRate);

            if (ValvesState.Values.All(x => x.Item2)) // All Valves are already open
            {
                var remainingMins = MaxMins - mins;
                return CalculateMaxPressure(current, mins + remainingMins, flowRate, newPressure + (flowRate * (remainingMins-1)), maxPressure);
            }

            // Lets open current valve and calculate new flowRate
            if (current.FlowRate > 0 && !ValvesState[current.Name].Item2)
            {
                ValvesState[current.Name] = (current, true);
                //Console.WriteLine($"Valve {current.Name} OPENED");
                return CalculateMaxPressure(current, mins + 1, flowRate + current.FlowRate, newPressure, maxPressure);
            }

            long max = 0;

            foreach (var valve in ValvesState.Where(x => !x.Value.Item2))
            {
                //var minsToReach = valve.Value.Item1.ValvesToOpen[valve.Key];
                var minsToReach = ValveDistance[(current.Name, valve.Key)];
                //Console.WriteLine($"Going from Valve {current.Name} to {valve.Key}");
                max = Math.Max(max, CalculateMaxPressure(valve.Value.Item1, mins + minsToReach, flowRate, newPressure + (flowRate * (minsToReach - 1)), maxPressure));

                // Close the valve so other paths can use it
                ValvesState[valve.Key] = (valve.Value.Item1, false);
                //Console.WriteLine($"Valve {valve.Key} CLOSED");
            }
            return max;
        }

        private static Dictionary<(string, string), long> CalculateDistances(List<Valve> fromList, List<Valve> toList)
        {
            var distances = new Dictionary<(string, string), long>();
            foreach(var from in fromList)
            {
                foreach(var to in toList)
                {
                    if (from.Name == to.Name) continue;

                    var distance = CalculateDistance(from.Name, to.Name);
                    distances.Add((from.Name, to.Name), distance);
                }
            }
            return distances;
        }

        private static long CalculateDistance(string from, string to)
        {
            Dictionary<string, bool> visitedDict = new Dictionary<string, bool>();
            var isPathFound = false;

            List<string> currentValves = new List<string> { from };
            int mins = 0;
            while (!isPathFound)
            {
                mins++;
                var nextValves = AllValves.Where(x => currentValves.Contains(x.Name)).SelectMany(v => v.LeadsTo).Distinct().Except(visitedDict.Keys).ToList();
                if (nextValves.Contains(to))
                {
                    isPathFound = true;
                    break;
                }
                nextValves.ForEach(x => visitedDict[x] = true);
                currentValves = nextValves;
            }
            return mins;
        }

        private static List<Valve> InitializeValves(List<string> input)
        {
            var valves = new List<Valve>();

            input.ForEach(l =>
            {
                // Valve DR has flow rate=22; tunnels lead to valves DC, YA
                var parts = l.Split(";");
                valves.Add(new Valve
                {
                    Name = parts[0].Split(" ")[1],
                    FlowRate = long.Parse(parts[0].Split(" ")[4].Split("=")[1]),
                    LeadsTo = parts[1].Replace(" tunnels lead to valves ", string.Empty).Replace(" tunnel leads to valve ", string.Empty).Split(", ").ToList()
                });
            });
            return valves;
        }

        class Valve
        {
            public string Name { get; set; }
            public long FlowRate { get; set; }
            public List<string> LeadsTo { get; set; }
            //public Dictionary<string, long> ValvesToOpen { get; set; }
        }
    }
}