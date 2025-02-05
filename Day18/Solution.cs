using AocHelper;

namespace Day18;

internal static partial class Program
{
  private const string Title = "\n## Day 18: RAM Run ##";
  private const string AdventOfCode = "https://adventofcode.com/2024/day/18";
  private const long ExpectedPartOne = 264;
  private static readonly (int, int) ExpectedPartTwo = (41, 26);

  private static readonly (int dx, int dy)[] _directions = [(0, -1), (1, 0), (0, 1), (-1, 0)];
  private static readonly Dictionary<(int x, int y), int> Visited = [];
  private static readonly List<List<(int x, int y)>> Paths = [];

  private static long PartOne(string input)
  {
    var (map, _) = ProcessInput(input);

    var start = (0, 0);
    var shortestPath = FindShortestPath(start, map);

    return shortestPath.Length - 1;
  }

  private static (int, int) PartTwo(string input)
  {
    var (map, bytes) = ProcessInput(input);
    var start = (0, 0);
    var shortestPath = FindShortestPath(start, map);

    foreach (var corruptedByte in bytes) {
      map[corruptedByte.y][corruptedByte.x] = '#';

      if (shortestPath.Contains(corruptedByte))
        shortestPath = FindShortestPath(start, map);

      if (shortestPath.Length == 0)
        return corruptedByte;
    }

    return (-1, -1);
  }

  private static (int x, int y)[] FindShortestPath((int x, int y) p, char[][] map)
  {
    (int x, int y)[] shortestPath = [];
    Visited.Clear();
    Paths.Clear();

    var finishPoint = (map[0].Length - 1, map.Length - 1);

    Visited.Add((p.x, p.y), 0);
    Paths.Add([(p.x, p.y)]);

    while (Paths.Count > 0) {
      var path = Paths[0];
      Paths.RemoveAt(0);

      foreach (var (dx, dy) in _directions) {
        var (nx, ny) = (path[0].x + dx, path[0].y + dy);
        if (IsValid(nx, ny, path.Count, map)) {
          List<(int, int)> newPath = [(nx, ny)];
          newPath.AddRange(path);
          Paths.Add(newPath);

          if ((nx, ny) == finishPoint)
            shortestPath = newPath.ToArray();
        }
      }
    }

    return shortestPath;
  }

  private static bool IsValid(int x, int y, int n, char[][] map)
  {
    if (!IsInBounds(x, y, map) || map[y][x] == '#')
      return false;

    if (Visited.TryGetValue((x, y), out var value)) {
      if (value <= n)
        return false;

      // remove existing paths with point p in
      for (var index = Paths.Count - 1; index >= 0; index--) {
        var path = Paths[index];
        if (path.Contains((x, y)))
          Paths.RemoveAt(index--);
      }

      Visited[(x, y)] = n;
    } else {
      Visited.Add((x, y), n);
    }

    return true;
  }

  private static bool IsInBounds(int x, int y, char[][] map)
  {
    return 0 <= x && x < map[0].Length && 0 <= y && y < map.Length;
  }

  private static (char[][] map, (int x, int y)[] corruptedBytes) ProcessInput(string input)
  {
    var (settings, pointsData) = input.Split("\n\n", StringSplitOptions.RemoveEmptyEntries).ToTuplePair();
    var (mapSize, corruptBytesCount) = settings.Split(',').ToIntTuplePair();

    var points = pointsData.Split('\n', StringSplitOptions.RemoveEmptyEntries);
    var bytes = new List<(int x, int y)>();
    Array.ForEach(points, x => bytes.Add(x.Split(',').ToIntTuplePair()));

    var map = new char[mapSize][];
    for (var y = 0; y < mapSize; y++) {
      map[y] = Helper.CreateArray(mapSize, '.');
    }

    for (var i = 0; i < corruptBytesCount; i++)
      map[bytes[i].y][bytes[i].x] = '#';

    return (map, bytes.ToArray());
  }
}
