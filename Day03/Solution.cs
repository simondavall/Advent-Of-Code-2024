using System.Text.RegularExpressions;
using AocHelper;

namespace Day03;

internal static partial class Program
{
  private const string Title = "\n## Day 3: Mull It Over ##";
  private const string AdventOfCode = "https://adventofcode.com/2024/day/3";
  private const long ExpectedPartOne = 181345830;
  private const long ExpectedPartTwo = 98729041;

  private static long PartOne(string input)
  {
    long tally = 0;

    var multiplicationDigits = ParseForMultiplicationDigits().Matches(input);
    foreach (Match digits in multiplicationDigits) {
      var first = digits.Groups[1].Value.ToLong();
      var second = digits.Groups[2].Value.ToLong();
      tally += first * second;
    }

    return tally;
  }

  private static long PartTwo(string input)
  {
    long tally = 0;
    var instructions = input.Split("do()");
    foreach (var instructionSet in instructions) {
      var activeInstructions = instructionSet.Split("don't()").FirstOrDefault();
      if (activeInstructions == null)
        continue;

      var multiplicationDigits = ParseForMultiplicationDigits().Matches(activeInstructions);
      foreach (Match digits in multiplicationDigits) {
        var first = digits.Groups[1].Value.ToLong();
        var second = digits.Groups[2].Value.ToLong();
        tally += first * second;
      }
    }

    return tally;
  }

  [GeneratedRegex(@"mul\((\d{1,3}),(\d{1,3})\)")]
  private static partial Regex ParseForMultiplicationDigits();
}
