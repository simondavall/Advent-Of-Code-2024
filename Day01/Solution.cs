using AocHelper;

namespace Day01;

internal static partial class Program
{
  private const string Title = "\n## Day 1: Historian Hysteria ##";
  private const string AdventOfCode = "https://adventofcode.com/2024/day/1";
  private const long ExpectedPartOne = 1590491;
  private const long ExpectedPartTwo = 22588371;

  private static long PartOne(string input)
  {
    string[] lines = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);
    List<int> firstList = [];
    List<int> secondList = [];

    foreach (var line in lines) {
      (int first, int second) = line.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToIntTuplePair();
      firstList.Add(first);
      secondList.Add(second);
    }

    firstList.Sort();
    secondList.Sort();

    return firstList.Select((t, i) => long.Abs(t - secondList[i])).Sum();

  }

  private static long PartTwo(string input)
  {
    string[] lines = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);
    List<int> firstList = [];
    List<int> secondList = [];

    foreach (var line in lines) {
      (int first, int second) = line.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToIntTuplePair();
      firstList.Add(first);
      secondList.Add(second);
    }

     long tally = 0;
    foreach (var locationId in firstList) {
      tally += locationId * secondList.Count(x => x == locationId);
    }

    return tally;
  }
}
