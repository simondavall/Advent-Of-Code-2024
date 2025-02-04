using AocHelper;

namespace Day11;

internal static partial class Program
{
  private const string Title = "\n## Day 11: Plutonian Pebbles ##";
  private const string AdventOfCode = "https://adventofcode.com/2024/day/11";
  private const long ExpectedPartOne = 198089;
  private const long ExpectedPartTwo = 236302670835517;

  private static Dictionary<long, long[]> _cachedRulesResult = [];

  private static long PartOne(string input)
  {
    _cachedRulesResult = [];
    var repititions = 25;
    Dictionary<long, long> numbersList = input.ToLongDictionary();

    long tally = 0;
    while (repititions-- > 0) {
      numbersList = Blink(numbersList);
    }

    foreach (var item in numbersList) {
      tally += item.Value;
    }

    return tally;
  }

  private static long PartTwo(string input)
  {
    var repititions = 75;
    Dictionary<long, long> numbersList = input.ToLongDictionary();
    long tally = 0;
    while (repititions-- > 0) {
      numbersList = Blink(numbersList);
    }

    foreach (var item in numbersList) {
      tally += item.Value;
    }

    return tally;
  }
  private static Dictionary<long, long> Blink(Dictionary<long, long> list)
  {
    Dictionary<long, long> nextResults = [];

    foreach (var listItem in list) {
      long[] results;
      if (_cachedRulesResult.TryGetValue(listItem.Key, out var value)) {
        results = value;
      } else {
        results = ApplyRules(listItem.Key);
        _cachedRulesResult.Add(listItem.Key, results);
      }

      foreach (var result in results) {
        if (!nextResults.TryAdd(result, 1 * listItem.Value)) {
          nextResults[result] += listItem.Value;
        }
      }
    }

    return nextResults;
  }

  private static long[] ApplyRules(long i)
  {
    if (i == 0)
      return [1];

    var digitsCount = i.Length();
    if (digitsCount % 2 == 0) {
      var multiplier = 10;
      for (var j = 1; j < digitsCount / 2; j++)
        multiplier *= 10;

      return [i / multiplier, i % multiplier];
    }

    return [i * 2024];
  }

  private static int Length(this long n)
  {
    var count = 0;

    while (n > 0) {
      count++;
      n /= 10;
    }

    return count;
  }

  private static Dictionary<long, long> ToLongDictionary(this string map)
  {
    var dict = new Dictionary<long, long>();

    var numbers = map.Split(" ", StringSplitOptions.RemoveEmptyEntries).ToLongArray();
    foreach (var i in numbers) {
      if (!dict.TryAdd(i, 1)) {
        dict[i]++;
      }
    }

    return dict;
  }
}
