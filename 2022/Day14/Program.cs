using System.Diagnostics;

namespace Day14
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Day 14!");

            //string inputFile = @"..\..\..\sample_input.txt";
            string inputFile = @"..\..\..\final_input.txt";
            var input = File.ReadAllLines(inputFile).ToList();

            Run(input);
        }

        private static void Run(List<string> input)
        {
            var timer = new Stopwatch();
            timer.Start();

            var linesGroup = input.Select(CreateCords);
            var occupiedPoints = CreateDictionaryWithLinePoints(linesGroup);
            var maxBottom = occupiedPoints.Keys.Select(x => x.Item2).Max();

            var answer1 = GetTotalUnitsOfSandAtRest(occupiedPoints, maxBottom);
            var answer1Time = timer.ElapsedMilliseconds;
            Console.WriteLine($"Answer1 = {answer1}; Time Taken = {answer1Time} ms");

            timer.Restart();

            occupiedPoints = CreateDictionaryWithLinePoints(linesGroup);
            maxBottom = occupiedPoints.Keys.Select(x => x.Item2).Max() + 2;

            var answer2 = GetTotalUnitsOfSandAtRest(occupiedPoints, maxBottom, true) + 1;
            var answer2Time = timer.ElapsedMilliseconds;
            Console.WriteLine($"Answer2 = {answer2}; Time Taken = {answer2Time} ms");
        }

        static int GetTotalUnitsOfSandAtRest(Dictionary<(int, int), bool> occupuiedPoints, int maxBottom, bool isWallAtBottom = false)
        {
            (int, int) sandPourStartPos = new(500, 0);

            //Simulate Sand Falling
            var maxLimitReached = false;
            var sandUnitCount = 0;

            while(!maxLimitReached)
            {
                maxLimitReached = !SimulateSandFall(sandPourStartPos, occupuiedPoints, maxBottom, isWallAtBottom);
                sandUnitCount++;
            }

            return --sandUnitCount;
        }

        static bool SimulateSandFall((int, int) startPos, Dictionary<(int, int), bool> occupuiedPoints, int maxBottom, bool isWallAtBottom)
        {
            var currentPos = startPos;

            while(true)
            {
                var currentIterationPos = currentPos;

                // All the way Down
                currentPos = MoveSteps(currentPos, occupuiedPoints, -1, GetNextStepDown, maxBottom, isWallAtBottom, out bool isFlowingToAbyss);
                if (isFlowingToAbyss) return false;

                // Diagnal Left
                var positionAfterMove = MoveSteps(currentPos, occupuiedPoints, 1, GetNextStepDiagLeft, maxBottom, isWallAtBottom, out isFlowingToAbyss);
                if (isFlowingToAbyss) return false;
                if (currentPos != positionAfterMove)
                {
                    currentPos = positionAfterMove;
                    continue;
                }

                // DiagnoalRight
                positionAfterMove = MoveSteps(currentPos, occupuiedPoints, 1, GetNextStepDiagRight, maxBottom, isWallAtBottom, out isFlowingToAbyss); ;
                if (isFlowingToAbyss) return false;
                if (currentPos != positionAfterMove)
                {
                    currentPos = positionAfterMove;
                    continue;
                }

                if (positionAfterMove == currentIterationPos) break; // no more moves possible
            }

            occupuiedPoints[currentPos] = true;

            return startPos != currentPos;
        }

        static (int, int) MoveSteps((int, int) startPos, Dictionary<(int, int), bool> occupuiedPoints, int stepsToMove, Func<(int, int), (int, int)> getNextStep, int maxBottom, bool isWallAtBottom, out bool isFlowingToAbyss)
        {
            var canMove = true;
            var currentPos = startPos;
            isFlowingToAbyss = false;
            var stepCount = 1;

            while (canMove)
            {
                (int, int) nextStep = getNextStep(currentPos);
                if (occupuiedPoints.ContainsKey(nextStep)) break;

                if (nextStep.Item2 >= maxBottom)
                {
                    if (!isWallAtBottom)
                    {
                        isFlowingToAbyss = true;
                        currentPos = startPos;
                    }
                    break;
                }
                currentPos = nextStep;
                stepCount++;
                if (stepsToMove != -1 && stepsToMove < stepCount) canMove = false;
            }
            return currentPos;
        }

        static (int, int) GetNextStepDown((int, int) currentStep) => new(currentStep.Item1, currentStep.Item2 + 1);
        static (int, int) GetNextStepDiagLeft((int, int) currentStep) => new(currentStep.Item1 - 1, currentStep.Item2 + 1);
        static (int, int) GetNextStepDiagRight((int, int) currentStep) => new(currentStep.Item1 + 1, currentStep.Item2 + 1);

        static (int, int)[] CreateCords(string source)
        {
            List<(int, int)> result = new List<(int, int)>();
            source.Split(" -> ").Select(x => x.Split(",")).ToList().ForEach(y => result.Add(new(int.Parse(y[0]), int.Parse(y[1]))));
            return result.ToArray();
        }

        static Dictionary<(int, int), bool> CreateDictionaryWithLinePoints(IEnumerable<(int, int)[]> linesGroup)
        {
            var result = new Dictionary<(int, int), bool>();

            linesGroup.ToList().ForEach(g => {
                for(int i = 1; i < g.Length; i++)
                {
                    GetAllLinePoints(g[i - 1], g[i]).ForEach(x => result[x] = true);
                }
            });
            return result;
        }

        static List<(int, int)> GetAllLinePoints((int, int) start, (int, int) end)
        {
            var result = new List<(int, int)>();
            var lower = Math.Min(start.Item1, end.Item1);
            for (int i = 0; i <= Math.Abs(start.Item1 - end.Item1); i++)
            {
                result.Add(new(lower + i, start.Item2));
            }
            lower = Math.Min(start.Item2, end.Item2);
            for (int i = 0; i <= Math.Abs(start.Item2 - end.Item2); i++)
            {
                result.Add(new(start.Item1, lower + i));
            }
            return result;
        }
    }
}