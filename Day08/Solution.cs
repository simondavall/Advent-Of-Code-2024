using AocHelper;

namespace Day08;

internal static partial class Program
{
  private const string Title = "\n## Day 8: Resonant Collinearity ##";
  private const string AdventOfCode = "https://adventofcode.com/2024/day/8";
  private const long ExpectedPartOne = 376;
  private const long ExpectedPartTwo = 1352;

  private static long PartOne(string input)
  {
    var (antennas, height, width) = ProcessData(input);
    var antiNodeLocations = new List<(int x, int y)>();
    foreach (var antenna in antennas) {
      for (var i = 0; i < antenna.Value.Count - 1; i++) {
        for (var j = i + 1; j < antenna.Value.Count; j++) {
          var (x1, y1) = antenna.Value[i];
          var (x2, y2) = antenna.Value[j];

          var dx = x1 - x2;
          var dy = y1 - y2;

          var (nx, ny) = (x1 + dx, y1 + dy);
          if (IsInBounds(nx, ny, height, width) && !antiNodeLocations.Contains((nx, ny)))
            antiNodeLocations.Add((nx, ny));

          (nx, ny) = (x2 - dx, y2 - dy);
          if (IsInBounds(nx, ny, height, width) && !antiNodeLocations.Contains((nx, ny)))
            antiNodeLocations.Add((nx, ny));
        }
      }
    }

    return antiNodeLocations.Count;
  }

  private static long PartTwo(string input)
  {
    var (antennas, height, width) = ProcessData(input);

    var antiNodeLocations = new List<(int x, int y)>();
    foreach (var antenna in antennas) {
      for (var i = 0; i < antenna.Value.Count - 1; i++) {
        for (var j = i + 1; j < antenna.Value.Count; j++) {
          var (x1, y1) = antenna.Value[i];
          var (x2, y2) = antenna.Value[j];

          if (!antiNodeLocations.Contains((x1, y1)))
            antiNodeLocations.Add((x1, y1));
          if (!antiNodeLocations.Contains((x2, y2)))
            antiNodeLocations.Add((x2, y2));

          var dx = x1 - x2;
          var dy = y1 - y2;

          var (nx, ny) = (x1 + dx, y1 + dy);
          while (IsInBounds(nx, ny, height, width)) {
            if (!antiNodeLocations.Contains((nx, ny)))
              antiNodeLocations.Add((nx, ny));

            nx += dx;
            ny += dy;
          }

          (nx, ny) = (x2 - dx, y2 - dy);
          while (IsInBounds(nx, ny, height, width)) {
            if (!antiNodeLocations.Contains((nx, ny)))
              antiNodeLocations.Add((nx, ny));

            nx -= dx;
            ny -= dy;
          }
        }
      }
    }

    return antiNodeLocations.Count;
  }

  private static (Dictionary<char, List<(int x, int y)>> antennas, int height, int width) ProcessData(string input)
  {
    var map = input.To2DCharArray();

    var height = map.Length;
    var width = map[0].Length;

    Dictionary<char, List<(int x, int y)>> antennas = [];

    for (var y = 0; y < height; y++) {
      for (var x = 0; x < width; x++) {
        var c = map[y][x];
        if (c == '.')
          continue;
        if (antennas.TryGetValue(c, out var antenna)) {
          antenna.Add((x, y));
          continue;
        }
        antennas.Add(c, [(x, y)]);
      }
    }
    return (antennas, height, width);
  }

  private static bool IsInBounds(int x, int y, int height, int width)
  {
    return 0 <= x && x < width && 0 <= y && y < height;
  }
}
