using AocHelper;

namespace Day12;

internal static partial class Program
{
  private const string Title = "\n## Day 12: Garden Groups ##";
  private const string AdventOfCode = "https://adventofcode.com/2024/day/12";
  private const long ExpectedPartOne = 1461752;
  private const long ExpectedPartTwo = 904114;

  private static readonly (int dx, int dy)[] _directions = [(0, -1), (1, 0), (0, 1), (-1, 0)];
  private static List<(int, int)> _visited = [];

  private static long PartOne(string input)
  {
    var (map, height, width) = GetMap(input);
    _visited = [];
    long tally = 0;
    for (var y = 0; y < height; y++) {
      for (var x = 0; x < width; x++) {
        if (!_visited.Contains((x, y))) {
          var ch = map[y][x];
          var (area, perimeter) = CalcPerimeter(ch, x, y, map);
          tally += area * perimeter;
        }
      }
    }

    return tally;
  }

  private static long PartTwo(string input)
  {
    var (map, height, width) = GetMap(input);
    _visited = [];
    long tally = 0;
    for (var y = 0; y < height; y++) {
      for (var x = 0; x < width; x++) {
        if (!_visited.Contains((x, y))) {
          var ch = map[y][x];
          var (area, corners) = CalcBulkPerimeter(ch, x, y, map);
          tally += area * corners;
        }
      }
    }

    return tally;
  }

  private static (int area, int corners) CalcBulkPerimeter(char plant, int x, int y, char[][] map)
  {
    var area = 1;
    var corners = 0;

    if (_visited.Contains((x, y)))
      return (0, 0);
    _visited.Add((x, y));

    corners += CountCorners(plant, x, y, map);

    foreach (var (dx, dy) in _directions){
      var (nx, ny) = (x + dx, y + dy);
      if (IsInBounds(nx, ny, map) && map[ny][nx] == plant){
        var (a, c) = CalcBulkPerimeter(plant, nx, ny, map);
        area += a;
        corners += c;
      }
    }

    return (area, corners);
  }

  private static (int area, int perimeter) CalcPerimeter(char plant, int x, int y, char[][] map)
  {
    var area = 1;
    var perimeter = 0;
    if (_visited.Contains((x, y)))
      return (0, 0);
    _visited.Add((x, y));

    foreach (var (dx, dy) in _directions){
      var (nx, ny) = (x + dx, y + dy);
      if (IsInBounds(nx, ny, map) && map[ny][nx] == plant){
        var (a, p) = CalcPerimeter(plant, nx, ny, map);
        area += a;
        perimeter += p;
      } else {
        perimeter++;
      }
    }

    return (area, perimeter);
  }

  private static int CountCorners(char c, int x, int y, char[][] map)
  {
    var corners = 0;

    //check left and up
    if ((x == 0 || map[y][x - 1] != c) && (y == 0 || map[y - 1][x] != c)
        || (x > 0 && map[y][x - 1] == c && y > 0 && map[y - 1][x] == c && map[y - 1][x - 1] != c)) {
      corners++;
    }

    //check right and up
    if ((x == map[0].Length - 1 || map[y][x + 1] != c) && (y == 0 || map[y - 1][x] != c)
        || (x < map[0].Length - 1 && map[y][x + 1] == c && y > 0 && map[y - 1][x] == c && map[y - 1][x + 1] != c)) {
      corners++;
    }

    //check right and down
    if ((x == map[0].Length - 1 || map[y][x + 1] != c) && (y == map.Length - 1 || map[y + 1][x] != c)
        || (x < map[0].Length - 1 && map[y][x + 1] == c && y < map.Length - 1 && map[y + 1][x] == c && map[y + 1][x + 1] != c)) {
      corners++;
    }

    //check left and down
    if ((x == 0 || map[y][x - 1] != c) && (y == map.Length - 1 || map[y + 1][x] != c)
        || (x > 0 && map[y][x - 1] == c && y < map.Length - 1 && map[y + 1][x] == c && map[y + 1][x - 1] != c)) {
      corners++;
    }

    return corners;
  }

  private static bool IsInBounds(int x, int y, char[][] map){
    return 0 <= x && x < map[0].Length && 0 <= y && y < map.Length;
  }

  private static (char[][] map, int height, int width) GetMap(string input)
  {
    var map = input.To2DCharArray();
    return (map, map.Length, map[0].Length);
  }
}
