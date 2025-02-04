using AocHelper;

namespace Day07;

internal static partial class Program
{
  private const string Title = "\n## Day 7: Bridge Repair ##";
  private const string AdventOfCode = "https://adventofcode.com/2024/day/7";
  private const long ExpectedPartOne = 5837374519342;
  private const long ExpectedPartTwo = 492383931650959;

  private static List<(long result, long[] operands)> FailedEquations = [];
  private static long _partOneResult;

  private static long PartOne(string input)
  {
    FailedEquations = [];
    var (results, operands) = ProcessData(input);

    long tally = 0;
    for (var i = 0; i < operands.Length; i++) {
      var accumulatedResults = new List<long> { results[i] };
      for (var j = operands[i].Length - 1; j >= 0; j--) {
        var currentOperand = operands[i][j];
        var calculatedResults = new List<long>();

        foreach (var accumulatedResult in accumulatedResults) {
          if (accumulatedResult.IsDivisibleBy(currentOperand)) {
            calculatedResults.Add(accumulatedResult / currentOperand);
          }
          calculatedResults.Add(accumulatedResult - currentOperand);
        }

        accumulatedResults = calculatedResults;
      }

      if (accumulatedResults.Contains(0))
        tally += results[i];
      else
        FailedEquations.Add((results[i], operands[i]));
    }

    _partOneResult = tally;
    return tally;
  }

  private static long PartTwo(string input)
  {
    long tally = 0;

    for (var i = 0; i < FailedEquations.Count; i++) {
      var accumulatedResults = new List<long> { FailedEquations[i].operands[0] };

      for (var j = 1; j < FailedEquations[i].operands.Length; j++) {
        var currentOperand = FailedEquations[i].operands[j];
        var calculatedResults = new List<long>();

        foreach (var accumulatedResult in accumulatedResults) {
          if (accumulatedResult + currentOperand <= FailedEquations[i].result)
            calculatedResults.Add(accumulatedResult + currentOperand);

          if (accumulatedResult * currentOperand <= FailedEquations[i].result)
            calculatedResults.Add(accumulatedResult * currentOperand);

          if (ConcatNumbers(accumulatedResult, currentOperand) <= FailedEquations[i].result) {
            calculatedResults.Add(ConcatNumbers(accumulatedResult, currentOperand));
          }
        }
        accumulatedResults = calculatedResults;
      }

      if (accumulatedResults.Contains(FailedEquations[i].result))
        tally += FailedEquations[i].result;
    }
    return tally + _partOneResult;
  }

  private static long ConcatNumbers(long first, long second)
  {
    var temp = second;
    while (temp / 10 > 0) {
      temp /= 10;
      first *= 10;
    }

    return (first * 10) + second;
  }

  private static bool IsDivisibleBy(this long result, long operand)
  {
    return result % operand == 0;
  }

  private static (long[] results, long[][] operands) ProcessData(string input)
  {
    var lines = File.ReadAllText("input.txt").Split('\n', StringSplitOptions.RemoveEmptyEntries);

    var results = new List<long>();
    var operands = new List<long[]>();
    foreach (var line in lines) {
      var (left, right) = line.Split(':').ToTuplePair();
      results.Add(left.ToLong());
      operands.Add(right.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToLongArray());
    }
    return (results.ToArray(), operands.ToArray());
  }
}
