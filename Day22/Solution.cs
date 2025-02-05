using AocHelper;

namespace Day22;

internal static partial class Program
{
  private const string Title = "\n## Day 22: Monkey Market ##";
  private const string AdventOfCode = "https://adventofcode.com/2024/day/22";
  private const long ExpectedPartOne = 19822877190;
  private const long ExpectedPartTwo = 2277;

  private static Dictionary<(int, int, int, int), long> _sequences = [];
  private static Dictionary<(int, int, int, int), int> _contributed = [];

  private static long PartOne(string input)
  {
    _sequences = [];
    _contributed = [];

    var findIndex = 2000;
    long[] initialNumbers = input.Split("\n", StringSplitOptions.RemoveEmptyEntries).ToLongArray();

    long tally = 0;
    for (var index = 0; index < initialNumbers.Length; index++) {
      var counter = 0;
      while (counter++ < findIndex) {
        initialNumbers[index] = Next(initialNumbers[index]);
      }

      tally += initialNumbers[index];
    }

    return tally;
  }

  private static long PartTwo(string input)
  {
    var findIndex = 2000;
    long[] initialNumbers = input.Split("\n", StringSplitOptions.RemoveEmptyEntries).ToLongArray();
    var secretNumbers = new List<SecretNumber>();

    foreach (var n in initialNumbers) {
      _contributed = [];
      secretNumbers.Clear();
      var initial = new SecretNumber {
        Number = n,
        Price = (int)n % 10
      };
      secretNumbers.Add(initial);

      var counter = 0;
      while (counter++ < findIndex) {
        secretNumbers = NextSecret(secretNumbers);
      }
    }

    return _sequences.Values.Max();
  }

  private static long Next(long current)
  {
    const long secretNumberModulo = 16777216;
    current = (current * 64 ^ current) % secretNumberModulo;
    current = (current / 32 ^ current) % secretNumberModulo;
    current = (current * 2048 ^ current) % secretNumberModulo;
    return current;
  }

  private static List<SecretNumber> NextSecret(List<SecretNumber> secrets)
  {
    if (secrets.Count > 3) {
      secrets.RemoveAt(0);
    }

    var next = new SecretNumber(Next(secrets.Last().Number));
    next.Price = (int)(next.Number % 10);
    next.Diff = next.Price - secrets.Last().Price;
    secrets.Add(next);

    if (secrets.Count > 3) {
      if (_contributed.TryAdd((secrets[0].Diff, secrets[1].Diff, secrets[2].Diff, secrets[3].Diff), next.Price)) {
        if (!_sequences.TryAdd((secrets[0].Diff, secrets[1].Diff, secrets[2].Diff, secrets[3].Diff), next.Price))
          _sequences[(secrets[0].Diff, secrets[1].Diff, secrets[2].Diff, secrets[3].Diff)] += next.Price;
      }
    }

    return secrets;
  }

  private struct SecretNumber(long number)
  {
    public long Number { get; init; } = number;
    public int Price { get; set; }
    public int Diff { get; set; }
  }
}
