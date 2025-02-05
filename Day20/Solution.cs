using System.Diagnostics;
using AocHelper;

namespace Day20;

internal static partial class Program
{
  private const string Title = "\n## Day 20: Race Condition ##";
  private const string AdventOfCode = "https://adventofcode.com/2024/day/20";
  private const long ExpectedPartOne = 1415;
  private const long ExpectedPartTwo = 1022577;

  private static readonly (int dx, int dy)[] _directions = [(-1, 0),(0, 1),(1, 0),(0, -1)];
  private static Dictionary<(int x, int y), int> _racePath = [];
  private static Dictionary<int, int> Cheats = [];

  private static long PartOne(string input)
  {
    _racePath = [];
    Cheats = [];
    var (map, start, end, settings) = ProcessInput(input);

    // run race in reverse
    var currentPoint = end;
    _racePath.Add(currentPoint, 0);
    while (currentPoint != start) {
      currentPoint = FindNext(currentPoint.x, currentPoint.y, map);
    }
    // correct the direction of the results
    _racePath = _racePath.Reverse().ToDictionary();

    foreach (var index in Enumerable.Range(0, _racePath.Count - 1)) {
      FindCheats(index, settings.MaxCheatLength1, settings.MinSaving);
    }

    return Cheats.Sum(x => x.Value);
  }

  private static long PartTwo(string input)
  {
    var (map, start, end, settings) = ProcessInput(input);
    Cheats.Clear();

    foreach (var index in Enumerable.Range(0, _racePath.Count - 1)) {
      FindCheats(index, settings.MaxCheatLength2, settings.MinSaving);
    }

    return Cheats.Sum(x => x.Value);
  }

  private static void FindCheats(int index, int maxCheatLength, int minSaving)
  {
    var currentPoint = _racePath.Keys.Skip(index).First();
    foreach (var destPoint in _racePath.Skip(index + 1)) {
      var dx = currentPoint.x - destPoint.Key.x;
      var dy = currentPoint.y - destPoint.Key.y;

      var cheatLength = int.Abs(dx) + int.Abs(dy);
      if (cheatLength > maxCheatLength)
        continue;

      var saving = _racePath[currentPoint] - (destPoint.Value + cheatLength);
      if (saving < minSaving)
        continue;

      if (!Cheats.TryAdd(saving, 1))
        Cheats[saving]++;
    }
  }

  private static (int x, int y) FindNext(int x, int y, char[][] map)
  {
    foreach (var (dx, dy) in _directions){
      var (nx, ny) = (x + dx, y + dy);
      if (IsNextPoint((nx, ny), map)){
        return (nx, ny);
      }
    }
    throw new UnreachableException("Did not find next point in race");
  }

  private static bool IsNextPoint((int x, int y) p, char[][] map)
  {
    if ((map[p.y][p.x] == '.' || map[p.y][p.x] == 'S') && !_racePath.ContainsKey(p)) {
      _racePath.Add(p, _racePath.Count);
      return true;
    }

    return false;
  }

  private static (char[][] map, (int x, int y) start, (int x, int y) end, Settings settings) ProcessInput(string input)
  {
    var (settingsData, mapData) = input.Split("\n\n", StringSplitOptions.RemoveEmptyEntries).ToTuplePair();

    var (psSave, maxCheat1, maxCheat2) = settingsData.Split(',').ToIntTupleTriple();
    var settings = new Settings(psSave, maxCheat1, maxCheat2);

    var (start, end) = ((-1, -1), (-1, -1));
    var map = mapData.To2DCharArray();
    for (var y = 0; y < map.Length; y++) {
      for (var x = 0; x < map[0].Length; x++) {
        var ch = map[y][x];
        if (ch == 'S')
          start = (x, y);
        else if (ch == 'E')
          end = (x, y);
      }
    }
    return (map, start, end, settings);
  }

  private class Settings(int minSaving, int maxCheat1, int maxCheat2)
  {
    public int MinSaving { get; set; } = minSaving;
    public int MaxCheatLength1 { get; set; } = maxCheat1;
    public int MaxCheatLength2 { get; set; } = maxCheat2;
  }
}
