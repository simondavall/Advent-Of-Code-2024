namespace Day10;

internal static partial class Program
{
  private const string Title = "\n## Day 10: Hoof It ##";
  private const string AdventOfCode = "https://adventofcode.com/2024/day/10";
  private const long ExpectedPartOne = 820;
  private const long ExpectedPartTwo = 1786;

  private static Dictionary<(int, int), HashSet<(int, int)>> _cachedTrailHeads = [];
  private static Dictionary<(int, int), int> _cachedTrails = [];
  private static readonly (int dx, int dy)[] _directions = [(0, -1), (1, 0), (0, 1), (-1, 0)];

  private static long PartOne(string input)
  {
    _cachedTrails = [];
    _cachedTrailHeads = [];
    var (map, startingPoints) = ProcessData(input);

    long tally = 0;
    foreach (var (x, y) in startingPoints) {
      var headsReached = FindTrailHeads(x, y, 0, map);
      tally += headsReached.Length;
    }

    return tally;
  }

  private static long PartTwo(string input)
  {
    var (map, startingPoints) = ProcessData(input);

    long tally = 0;
    foreach (var (x, y) in startingPoints) {
      var trails = FindDistinctTrails(x, y, 0, map);
      tally += trails;
    }

    return tally;
  }

  private static int FindDistinctTrails(int x, int y, int number, int[][] map)
  {
    if (number == 9)
      return 1;

    if (_cachedTrails.TryGetValue((x, y), out var score))
      return score;

    var count = 0;
    foreach (var (dx, dy) in _directions) {
      var (nx, ny) = (x + dx, y + dy);
      if (IsInBounds(nx, ny, map) && map[ny][nx] == number + 1)
        count += FindDistinctTrails(nx, ny, number + 1, map);
    }

    _cachedTrails.Add((x, y), count);

    return count;
  }

  private static (int x, int y)[] FindTrailHeads(int x, int y, int number, int[][] map)
  {
    HashSet<(int, int)> heads = [];

    if (number == 9)
      return [(x, y)];

    if (_cachedTrailHeads.TryGetValue((x, y), out var cachedHeads))
      return cachedHeads.ToArray();

    foreach (var (dx, dy) in _directions) {
      var (nx, ny) = (x + dx, y + dy);
      if (IsInBounds(nx, ny, map) && map[ny][nx] == number + 1) {
        var reachableHeads = FindTrailHeads(nx, ny, number + 1, map);
        foreach (var head in reachableHeads)
          heads.Add(head);
      }
    }

    _cachedTrailHeads.Add((x, y), heads);

    return heads.ToArray();
  }

  private static bool IsInBounds(int x, int y, int[][] map)
  {
    return 0 <= x && x < map[0].Length && 0 <= y && y < map.Length;
  }

  private static (int[][] map, (int x, int y)[] startingPoints) ProcessData(string input)
  {
    var data = input.Split("\n", StringSplitOptions.RemoveEmptyEntries);

    var startingPoints = new List<(int, int)>();
    var height = data.Length;
    var width = data[0].Length;
    var map = new int[height][];
    for (var y = 0; y < height; y++) {
      map[y] = new int[width];
      for (var x = 0; x < width; x++) {
        if (data[y][x] == '0')
          startingPoints.Add((x, y));
        map[y][x] = data[y][x] - '0';
      }
    }

    return (map, startingPoints.ToArray());
  }
}
