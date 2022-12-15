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

        static int GetTuningFrequencyForDistressBeacon(List<string> input, int minCord, int maxCord)
        {
            List<Sensor> sensors = GetSensorAndRespectiveBeaconLocations(input);
            Console.WriteLine("Initilized Sensors");
            (int, int) distressBeaconPos = GetDistressBeaconPosition(sensors, minCord, maxCord);
            return distressBeaconPos.Item1 * 4000000 + distressBeaconPos.Item2;
        }

        private static (int, int) GetDistressBeaconPosition(List<Sensor> sensors, int minCord, int maxCord)
        {
            //Dictionary<(int, int), bool> emptyLocations = new Dictionary<(int, int), bool>();

            //sensors.ForEach(s =>
            //{
            //    for (int x = s.SensorPos.Item1 - s.DistanceToBeacon; x <= s.SensorPos.Item1 + s.DistanceToBeacon; x++)
            //    {
            //        var rangeY = s.DistanceToBeacon - Math.Abs(s.SensorPos.Item1 - x);
            //        for (int y = s.SensorPos.Item2 - rangeY; y <= s.SensorPos.Item2 + rangeY; y++)
            //        {
            //            emptyLocations[new(x, y)] = true;
            //        }
            //    }
            //});

            bool isPointInSensorRange = false; ;
            (int, int) distressBeacon = new (0,0);
            for(int x = minCord; x <= maxCord; x++)
            {
                for (int y = minCord; y <= maxCord; y++)
                {
                    isPointInSensorRange = false;
                    foreach (var s in sensors)
                    {
                        if (IsPointWithinSensorRange(s, x, y))
                        {
                            isPointInSensorRange = true;
                            break;
                        }
                    };
                    if (isPointInSensorRange) continue;
                    distressBeacon = new (x, y);
                    break;
                }
                if (!isPointInSensorRange) break;
            }

            Console.WriteLine("Identified DistressBeacon");

            //int beaconX = Enumerable.Range(minCord, maxCord).Except(emptyLocations.Keys.Select(c => c.Item1)).FirstOrDefault();
            //int beaconY = Enumerable.Range(minCord, maxCord).Except(emptyLocations.Keys.Select(c => c.Item2)).FirstOrDefault();
            return distressBeacon;
        }

        static bool IsPointWithinSensorRange(Sensor sensor, int pointX, int pointY)
        {
            var distanceToPoint = Math.Abs(sensor.SensorPos.Item1 - pointX) + Math.Abs(sensor.SensorPos.Item2 - pointY);
            return distanceToPoint <= sensor.DistanceToBeacon;
        }

        static int GetConfirmedEmptyLocationsForY(List<string> input, int y)
        {
            List<Sensor> sensors = GetSensorAndRespectiveBeaconLocations(input);
            int emptyLocations = GetConfirmedEmptyBeaconLocationsFor(sensors, y);
            return emptyLocations;
        }

        private static int GetConfirmedEmptyBeaconLocationsFor(List<Sensor> sensors, int y)
        {
            Dictionary<(int, int), bool> emptyLocations = new Dictionary<(int, int), bool>();
            var allBeacons = sensors.Select(s => s.NearestBeaconPos).ToList();

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
    }
}