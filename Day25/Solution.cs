using System.Text;

namespace Day25;

internal static partial class Program
{
  private const string Title = "\n## Day 25: Code Chronicle ##";
  private const string AdventOfCode = "https://adventofcode.com/2024/day/25";
  private const long ExpectedPartOne = 3395;
  private const long ExpectedPartTwo = 0;


  private static long PartOne(string input)
  {
    var (locks, keys) = ProcessInput(input);
    long tally = 0;
    foreach (var k in keys) {
      foreach (var l in locks) {
        if (k.Size + l.Size > 25)
          continue;
        tally += MatchKey(l, k);
      }
    }
    return tally;
  }

  private static int MatchKey(Schematic l, Schematic k)
  {
    return l.Tumblers.Where((t, i) => t + k.Tumblers[i] > 5).Any() ? 0 : 1;
  }

  private static long PartTwo(string input)
  {
    return 0;
  }

  private static (Schematic[] locks, Schematic[] keys) ProcessInput(string input1)
  {
    var input = input1.Split("\n\n", StringSplitOptions.RemoveEmptyEntries);

    var locks = new List<Schematic>();
    var keys = new List<Schematic>();

    foreach (var schematic in input) {
      var lines = schematic.Split('\n', StringSplitOptions.RemoveEmptyEntries);

      var item = new Schematic();
      foreach (var row in lines) {
        item.AddRow(row);
      }

      if (lines[0][0] == '#')
        locks.Add(item);
      else
        keys.Add(item);
    }

    return (locks.ToArray(), keys.ToArray());
  }

  private struct Schematic()
  {
    public int Size { get; private set; } = -5;
    public readonly int[] Tumblers = [-1, -1, -1, -1, -1];

    public void AddRow(string row)
    {
      for (var i = 0; i < Tumblers.Length; i++)
        Tumblers[i] += row[i] == '#' ? 1 : 0;

      Size += row.Sum(ch => ch == '#' ? 1 : 0);
    }

    public override string ToString()
    {
      var str = new StringBuilder($"Size: {Size:00}: (");
      foreach (var t in Tumblers)
        str.Append($"{t},");
      str.Replace(',', ')', str.Length - 1, 1);
      return str.ToString();
    }
  }
}
