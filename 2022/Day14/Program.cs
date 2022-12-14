using System.Diagnostics;

namespace Day14
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Day 14!");

            string inputFile = @"..\..\..\sample_input.txt";
            //string inputFile = @"..\..\..\final_input.txt";
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

                currentPos = MoveSteps(currentPos, occupuiedPoints, -1, GetNextStepDown, maxBottom, isWallAtBottom, out NoFurtherMovesReason noFurtherMovesReason);
                if (currentPos == currentIterationPos && noFurtherMovesReason == NoFurtherMovesReason.GoingInAbyss)
                {
                    currentPos = startPos;
                    break;
                }
                var positionAfterMove = MoveSteps(currentPos, occupuiedPoints, 1, GetNextStepDiagLeft, maxBottom, isWallAtBottom, out noFurtherMovesReason);
                if (currentPos != positionAfterMove)
                {
                    currentPos = positionAfterMove;
                    continue;
                }
                else if (noFurtherMovesReason == NoFurtherMovesReason.GoingInAbyss)
                {
                    currentPos = startPos;
                    break;
                }
                positionAfterMove = MoveSteps(currentPos, occupuiedPoints, 1, GetNextStepDiagRight, maxBottom, isWallAtBottom, out noFurtherMovesReason); ;

                if (currentPos != positionAfterMove)
                {
                    currentPos = positionAfterMove;
                    continue;
                }
                else if (noFurtherMovesReason == NoFurtherMovesReason.GoingInAbyss)
                {
                    currentPos = startPos;
                    break;
                }

                if (positionAfterMove == currentIterationPos) break; // no more moves possible
            }

            occupuiedPoints[currentPos] = true;

            return startPos != currentPos;
        }

        static (int, int) MoveSteps((int, int) startPos, Dictionary<(int, int), bool> occupuiedPoints, int stepsToMove, Func<(int, int), (int, int)> getNextStep, int maxBottom, bool isWallAtBottom, out NoFurtherMovesReason noFurtherMovesReason)
        {
            var canMove = true;
            var currentPos = startPos;
            noFurtherMovesReason = NoFurtherMovesReason.ReachedBottom;
            var stepCount = 1;

            while (canMove)
            {
                (int, int) nextStep = getNextStep(currentPos);
                if (occupuiedPoints.ContainsKey(nextStep))
                {
                    break;
                }
                if (nextStep.Item2 >= maxBottom)
                {
                    if (!isWallAtBottom)
                    {
                        noFurtherMovesReason = NoFurtherMovesReason.GoingInAbyss;
                        currentPos = startPos;
                    }
                    break;
                }
                currentPos = nextStep;
                stepCount++;
                if (stepsToMove != -1 && stepsToMove < stepCount)
                {
                    canMove = false;
                }
            }
            return currentPos;
        }

        static (int, int) MoveAllTheWay((int, int) startPos, Dictionary<(int, int), bool> occupuiedPoints, Func<(int, int), (int, int)> getNextStep, int maxBottom, bool isWallAtBottom, out bool goingToAbyss)
        {
            var canMove = true;
            var currentPos = startPos;
            goingToAbyss = false;
            while (canMove)
            {
                (int, int) nextStep = MoveOneStep(currentPos, occupuiedPoints, getNextStep);
                if (nextStep == currentPos)
                {
                    canMove = false;
                    break;
                }
                if (nextStep.Item2 >= maxBottom)
                {
                    canMove = false;
                    if (!isWallAtBottom)
                    {
                        goingToAbyss = true;
                        currentPos = startPos;
                    }
                    break;
                }
                currentPos = nextStep;
            }
            return currentPos;
        }

        static (int, int) MoveOneStep((int, int) currentStep, Dictionary<(int, int), bool> occupuiedPoints, Func<(int, int), (int, int)> getNextStep)
        {
            (int, int) nextStep = getNextStep(currentStep);
            return occupuiedPoints.ContainsKey(nextStep) ? currentStep : nextStep;
        }

        static (int, int) GetNextStepDown((int, int) currentStep) => new(currentStep.Item1, currentStep.Item2 + 1);
        static (int, int) GetNextStepDiagLeft((int, int) currentStep) => new(currentStep.Item1 - 1, currentStep.Item2 + 1);
        static (int, int) GetNextStepDiagRight((int, int) currentStep) => new(currentStep.Item1 + 1, currentStep.Item2 + 1);

        static (int, int)[] CreateCords(string source)
        {
            List<(int, int)> result = new List<(int, int)>();

            var lineGroupsString = source.Split(" -> ").Select(x => x.Split(",")).ToList();

            lineGroupsString.ForEach(x =>
            {
                result.Add(new(int.Parse(x[0]), int.Parse(x[1])));
            });
            return result.ToArray();
        }

        static Dictionary<(int, int), bool> CreateDictionaryWithLinePoints(IEnumerable<(int, int)[]> linesGroup)
        {
            var result = new Dictionary<(int, int), bool>();

            linesGroup.ToList().ForEach(g => {
                for(int i = 1; i < g.Length; i++)
                {
                    var allLinePoints = GetAllLinePoints(g[i - 1], g[i]);
                    allLinePoints.ForEach(x => result[x] = true);
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

    enum NoFurtherMovesReason { ReachedBottom, GoingInAbyss }
}