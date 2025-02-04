using AocHelper;

namespace Day02;

internal static partial class Program
{
  private const string Title = "\n##  Day 2:Red-Nosed Reports ##";
  private const string AdventOfCode = "https://adventofcode.com/2024/day/2";
  private const long ExpectedPartOne = 269;
  private const long ExpectedPartTwo = 337;

  private static long PartOne(string input)
  {
    var reports = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);
    long tally = 0;

    foreach (var report in reports) {
      var levels = report.Split(' ').ToIntArray();
      tally += IsReportSafe(levels) ? 1 : 0;
    }

    return tally;
  }

  private static long PartTwo(string input)
  {
    var reports = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);
    var tally = 0;

    foreach (var report in reports) {
      var levels = report.Split(' ').ToIntArray();
      if (IsReportSafe(levels)) {
        tally++;
        continue;
      }

      for (var level = 0; level < levels.Length; level++) {
        int[] amendedLevels = levels.Where((_, l) => level != l).ToArray();
        if (IsReportSafe(amendedLevels)){
          tally++;
          break;
        }
      }
    }

    return tally;
  }

  private static bool IsReportSafe(int[] levels)
  {
    bool isLevelIncreasing = levels[0] > levels[1];
    foreach (var _ in levels) {
      for (var j = 1; j < levels.Length; j++) {
        if (levels[j - 1] > levels[j] != isLevelIncreasing
            || levels[j - 1] == levels[j]
            || int.Abs(levels[j - 1] - levels[j]) > 3)
          return false;
      }
    }

    return true;
  }
}
