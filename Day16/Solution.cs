namespace Day16;

public enum Direction
{
  East,
  South,
  West,
  North
}

public struct Vector
{
  public (int x, int y) Point { get; set; }
  public Direction Direction { get; set; }
  public long Tally { get; set; }
  
  public Vector((int x, int y) point, Direction direction, long tally)
  {
    Point = point;
    Direction = direction;
    Tally = tally;
  }
}

internal static partial class Program
{
  private const string Title = "\n## Day 16: Reindeer Maze ##";
  private const string AdventOfCode = "https://adventofcode.com/2024/day/16";
  private const long ExpectedPartOne = 101492;
  private const long ExpectedPartTwo = 543;

  private static (int x, int y) _startPoint;
  private static (int x, int y) _finishPoint;
  private static Dictionary<long, List<List<Vector>>> Paths = [];
  private static List<List<Vector>> ValidPaths = [];
  private static Dictionary<((int, int), Direction), long> Visited = [];

  private static long PartOne(string input)
  {
    ClearGlobalCache();
    var map = GetMap(input);

    return CalculateShortestRoute(map);
  }

  private static long PartTwo(string input)
  {
    List<(int x, int y)> bestPathPoints = [];
    foreach (var path in ValidPaths) {
      bestPathPoints.AddRange(path.Select(v => v.Point));
    }

    return bestPathPoints.Distinct().Count();
  }

  private static void ClearGlobalCache()
  {
    Paths = [];
    ValidPaths = [];
    Visited = [];
  }

  private static long CalculateShortestRoute(char[][] map)
  {
    long score = 0;
    long index = 0;

    var v = new Vector(_startPoint, Direction.East, 0);
    var startPointExits = GetValidExits(v.Point.x, v.Point.y, v.Direction, map);

    foreach (var ((nx, ny), direction) in startPointExits)
      ProcessNextPoint(nx, ny, direction, [v]);

    while (Paths.Count > 0 && (score > 0 || index >= score)) {
      if (!Paths.Remove(index++, out var activePaths))
        continue;

      foreach (var path in activePaths) {
        if (path.Last().Point == _finishPoint) {
          ValidPaths.Add(path);
          if (score == 0 || score > path.Last().Tally)
            score = path.Last().Tally;
        }
        var last = path.Last();
        var ((lx, ly), ld) = (last.Point, last.Direction);
        var validExits = GetValidExits(lx, ly, ld, map);

        foreach (var ((nx, ny), direction) in validExits)
          ProcessNextPoint(nx, ny, direction, path);
      }
    }

    return score;
  }

  private static void ProcessNextPoint(int x, int y, Direction d, List<Vector> v)
  {
    var turnPenalty = 1000;
    var newVector = new Vector((x, y), d, v.Last().Tally);

    newVector.Tally += 1;
    if (d != v.Last().Direction)
      newVector.Tally += turnPenalty;

    if (Visited.TryGetValue(((x, y), d), out var currentTally)) {
      if (currentTally >= newVector.Tally)
        Visited[((x, y), d)] = newVector.Tally;
      else
        return;
    } else {
      Visited.Add(((x, y), d), newVector.Tally);
    }

    var clone = new List<Vector>(v) { newVector };

    if (!Paths.TryAdd(newVector.Tally, [clone]))
      Paths[newVector.Tally].Add(clone);
  }

  private static ((int x, int y) point, Direction direction)[] GetValidExits(int x, int y, Direction d, char[][] map)
  {
    List<((int x, int y), Direction)> points = [];
    char[] validChars = ['.', 'E'];
    if (d != Direction.West && validChars.Contains(map[y][x + 1])) {
      points.Add(((x + 1, y), Direction.East));
    }

    if (d != Direction.East && validChars.Contains(map[y][x - 1])) {
      points.Add(((x - 1, y), Direction.West));
    }

    if (d != Direction.North && validChars.Contains(map[y + 1][x])) {
      points.Add(((x, y + 1), Direction.South));
    }

    if (d != Direction.South && validChars.Contains(map[y - 1][x])) {
      points.Add(((x, y - 1), Direction.North));
    }

    return points.ToArray();
  }

  private static char[][] GetMap(string input)
  {
    var mapData = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);

    var map = new char[mapData.Length][];
    for (var y = 0; y < mapData.Length; y++) {
      map[y] = mapData[y].ToCharArray();
      if (mapData[y].IndexOf('S') > -1)
        _startPoint = (mapData[y].IndexOf('S'), y);
      if (mapData[y].IndexOf('E') > -1)
        _finishPoint = (mapData[y].IndexOf('E'), y);
    }

    return map;
  }
}
