using AocHelper;

namespace Day16;

internal static partial class Program
{
  private const string Title = "\n## Day 16: Reindeer Maze ##";
  private const string AdventOfCode = "https://adventofcode.com/2024/day/16";
  private const long ExpectedPartOne = 101492;
  private const long ExpectedPartTwo = 543;

  private static HashSet<(int, int)> OnShortestPath = [];

  private static long PartOne(string input)
  {
    OnShortestPath = [];

    var turnPenalty = 1000;
    var shorestPath = int.MaxValue;
    var (map, (sx, sy)) = ProcessInput(input);
    var (sdx, sdy) = (1, 0); // start heading east

    var seen = new Dictionary<(int, int, int, int), int>();
    var q = new PriorityQueue<(int, int, int, int, List<(int, int)>), int>();
    q.Enqueue((sx, sy, sdx, sdy, [(sx, sy)]), 0);
    seen.Add((sx, sy, sdx, sdy), 0);

    while (q.Count > 0) {
      if (!q.TryDequeue(out var item, out int depth))
        continue;
      var (x, y, dx, dy, path) = item;

      var (nx, ny) = (x + dx, y + dy);
      if (map[ny][nx] == 'E' && shorestPath >= depth + 1) {
        shorestPath = depth + 1;
        var newPath = path.ToList();
        newPath.Add((nx, ny));
        AddToTilesOnShortestPaths(newPath);
        continue;
      }

      if (map[ny][nx] != '#')
        if (!seen.TryGetValue((nx, ny, dx, dy), out int d) || d >= depth + 1) {
          seen[(nx, ny, dx, dy)] = depth + 1;
          var newPath = path.ToList();
          newPath.Add((nx, ny));
          q.Enqueue((nx, ny, dx, dy, newPath), depth + 1);
        }

      // turn clockwise
      var (dx1, dy1) = (-dy, dx);
      if (map[y + dy1][x + dx1] != '#' && (!seen.TryGetValue((x, y, dx1, dy1), out int d1) || d1 >= depth + turnPenalty)) {
        seen[(x, y, dx1, dy1)] = depth + turnPenalty;
        q.Enqueue((x, y, dx1, dy1, path), depth + turnPenalty);
      }

      // turn anti-clockwise
      var (dx2, dy2) = (dy, -dx);
      if (map[y + dy2][x + dx2] != '#' && (!seen.TryGetValue((x, y, dx2, dy2), out int d2) || d2 >= depth + turnPenalty)) {
        seen[(x, y, dx2, dy2)] = depth + turnPenalty;
        q.Enqueue((x, y, dx2, dy2, path), depth + turnPenalty);
      }
    }
    return shorestPath;
  }

  private static long PartTwo(string input)
  {
    return OnShortestPath.Count;
  }

  private static void AddToTilesOnShortestPaths(List<(int, int)> path)
  {
    foreach (var tile in path)
      OnShortestPath.Add(tile);
  }

  private static (char[][] map, (int x, int y) start) ProcessInput(string input)
  {
    var map = input.To2DCharArray();
    var start = (-1, -1);
    for (var y = 0; y < map.Length; y++) {
      for (var x = 0; x < map[0].Length; x++) {
        var ch = map[y][x];
        if (ch == 'S')
          start = (x, y);
      }
    }
    return (map, start);
  }
}
