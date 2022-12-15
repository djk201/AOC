using System.Diagnostics;
namespace Day15
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Day 15!");

            //string inputFile = @"..\..\..\sample_input.txt";
            //int y = 10;
            //int minCord = 0;
            //int maxCord = 20;
            string inputFile = @"..\..\..\final_input.txt";
            int y = 2000000;
            int minCord = 0;
            int maxCord = 4000000;

            var input = File.ReadAllLines(inputFile).ToList();

            Run(input, y, minCord, maxCord);
            //Debug(input);
        }

        private static void Run(List<string> input, int y, int minCord, int maxCord)
        {
            var timer = new Stopwatch();
            timer.Start();

            var answer1 = GetConfirmedEmptyLocationsForY(input, y);
            var answer1Time = timer.ElapsedMilliseconds;
            Console.WriteLine($"Answer1 = {answer1}; Time Taken = {answer1Time} ms");

            timer.Restart();
            var answer2 = GetTuningFrequencyForDistressBeacon(input, minCord, maxCord);
            var answer2Time = timer.ElapsedMilliseconds;
            Console.WriteLine($"Answer2 = {answer2}; Time Taken = {answer2Time} ms");
        }

        static long GetTuningFrequencyForDistressBeacon(List<string> input, int minCord, int maxCord)
        {
            List<Sensor> sensors = GetSensorAndRespectiveBeaconLocations(input);
            (int, int) distressBeaconPos = GetDistressBeaconPosition(sensors, minCord, maxCord);
            return (long) distressBeaconPos.Item1 * 4000000 + distressBeaconPos.Item2;
        }

        private static (int, int) GetDistressBeaconPosition(List<Sensor> sensors, int minCord, int maxCord)
        {
            var posToCheck = sensors.SelectMany(s => GetAdjacentPosForSensor(s, minCord, maxCord)).Distinct();

            foreach (var pos in posToCheck)
            {
                var isPointInRange = false;
                foreach (Sensor sensor in sensors)
                {
                    isPointInRange = sensor.IsPointInRange(pos);
                    if (isPointInRange) break;
                }
                if (!isPointInRange) return pos;
            }
            return (0, 0);
        }

        static List<(int, int)> GetAdjacentPosForSensor(Sensor s, int min, int max)
        {
            var adjacentPos = new List<(int, int)>();
            var radius = s.DistanceToBeacon + 1;
            for (int x = s.SensorPos.Item1 - radius; x <= s.SensorPos.Item1 + radius; x++)
            {
                if ((x < min) || (x > max)) continue;
                var yRange = radius - Math.Abs(s.SensorPos.Item1 - x);
                if (!(s.SensorPos.Item2 - yRange < min) && !(s.SensorPos.Item2 - yRange > max)) adjacentPos.Add(new(x, s.SensorPos.Item2 - yRange));

                if (x == s.SensorPos.Item1 - radius || x == s.SensorPos.Item1 + radius) continue;
                if (!(s.SensorPos.Item2 + yRange < min) && !(s.SensorPos.Item2 + yRange > max)) adjacentPos.Add(new(x, s.SensorPos.Item2 + yRange));
            }
            return adjacentPos;
        }

        static int GetConfirmedEmptyLocationsForY(List<string> input, int y)
        {
            List<Sensor> sensors = GetSensorAndRespectiveBeaconLocations(input);
            var allBeacons = sensors.Select(s => s.NearestBeaconPos).ToList();

            Dictionary<(int, int), bool> emptyLocations = new Dictionary<(int, int), bool>();
            sensors.ForEach(s =>
            {
                var focal = Math.Abs(y - s.SensorPos.Item2);
                var range = s.DistanceToBeacon - focal;
                for (int x = s.SensorPos.Item1 - range; x <= s.SensorPos.Item1 + range; x++)
                {
                    (int, int) loc = new(x, y);
                    if (!allBeacons.Contains(loc)) emptyLocations[loc] = true;
                }
            });

            return emptyLocations.Count();
        }

        private static List<Sensor> GetSensorAndRespectiveBeaconLocations(List<string> input)
        {
            var sensors = new List<Sensor>();
            input.ForEach(l =>
            {
                var parts = l.Replace("Sensor at ", string.Empty).Replace(": closest beacon is at ", ",").Split(",");

                var pos = parts.ToList().Select(s => s.Trim().Split("=")).Select(t => int.Parse(t[1])).ToArray();
                sensors.Add(new Sensor
                {
                    SensorPos = new(pos[0], pos[1]),
                    NearestBeaconPos = new(pos[2], pos[3])
                });
            });
            return sensors;
        }
    }

    class Sensor
    {
        public (int, int) SensorPos { get; set; }
        public (int, int) NearestBeaconPos { get; set; }
        public int DistanceToBeacon => Math.Abs(SensorPos.Item1 - NearestBeaconPos.Item1) + Math.Abs(SensorPos.Item2 - NearestBeaconPos.Item2);
        public bool IsPointInRange((int, int) point) => Math.Abs(SensorPos.Item1 - point.Item1) + Math.Abs(SensorPos.Item2 - point.Item2) <= DistanceToBeacon;
    }
}