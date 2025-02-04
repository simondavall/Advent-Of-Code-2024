namespace Day14;

internal static partial class Program
{
  private const string Title = "\n## Day 14: Restroom Redoubt ##";
  private const string AdventOfCode = "https://adventofcode.com/2024/day/14";
  private const long ExpectedPartOne = 236628054;
  private const long ExpectedPartTwo = 7584;

  private static int _mapHeight = 7; // values for sample.txt
  private static int _mapWidth = 11;

  private static long PartOne(string input)
  {
    _mapHeight = 103; // comment these out when running sample.txt
    _mapWidth = 101;

    var robots = GetRobots(input);
    var quads = new int[4];

    foreach (var robot in robots) {
      var x = (robot.Position.x + (robot.Velocity.x + _mapWidth) * 100) % _mapWidth;
      var y = (robot.Position.y + (robot.Velocity.y + _mapHeight) * 100) % _mapHeight;
      robot.Position = (x, y);

      var index = GetQuad(x, y);
      if (index > -1)
        quads[index]++;
    }

    return quads.Aggregate(1, (current, quad) => current * quad);
  }

  private static long PartTwo(string input)
  {
    var robots = GetRobots(input);
    var secs = 0;
    var points = new (int x, int y)[500];

    while (secs++ < 10403) {
      var index = 0;

      foreach (var robot in robots) {
        Move(robot);
        points[index++] = robot.Position;
      }

      // check to see whether there is a concentrated group
      // of robots. Use PrintMap to inspect for a pattern
      if (HasUnusualSpread(points) > 300) {
        //PrintMap(robots, secs);
        //Xmas
        return secs;
      }
    }

    return -1;
  }
  private static int HasUnusualSpread((int x, int y)[] points)
  {
    const int weight = 20;
    var midHeight = _mapHeight / 2;
    var midWidth = _mapWidth / 2;

    var count = 0;

    foreach (var (x, y) in points) {
      if (x > midWidth - weight && x < midWidth + weight
      && y > midHeight - weight && y < midHeight + weight)
        count++;
    }

    return count;
  }

  private static int GetQuad(int x, int y)
  {
    var halfWidth = _mapWidth / 2;
    var halfHeight = _mapHeight / 2;

    if (x < halfWidth && y < halfHeight)
      return 0;
    if (x > halfWidth && y < halfHeight)
      return 1;
    if (x < halfWidth && y > halfHeight)
      return 2;
    if (x > halfWidth && y > halfHeight)
      return 3;

    return -1;
  }

  private static void Move(Robot robot)
  {
    var x = (robot.Position.x + robot.Velocity.x + _mapWidth) % _mapWidth;
    var y = (robot.Position.y + robot.Velocity.y + _mapHeight) % _mapHeight;
    robot.Position = (x, y);
  }

  // ReSharper disable once UnusedMember.Local
  private static void PrintMap(List<Robot> robots, int secs)
  {
    Console.WriteLine($"Map at: {secs} secs");
    var spaceMap = new int[_mapWidth, _mapHeight];

    foreach (var robot in robots) {
      spaceMap[robot.Position.x, robot.Position.y]++;
    }

    for (var y = 0; y < _mapHeight; y++) {
      for (var x = 0; x < _mapWidth; x++) {
        Console.Write(spaceMap[x, y]);
      }
      Console.WriteLine();
    }
    Console.WriteLine();
  }

  private static Robot[] GetRobots(string input)
  {
    var lines = input.Split("\n", StringSplitOptions.RemoveEmptyEntries);
    return lines.Select(line => line.Split(" "))
        .Select(point => new Robot(GetPoint(point[0]), GetPoint(point[1]))).ToArray();
  }

  private static (int x, int y) GetPoint(string input)
  {
    var p = input.Split("=");
    var x = int.Parse(p[1].Split(",")[0]);
    var y = int.Parse(p[1].Split(",")[1]);
    return (x, y);
  }

  private class Robot((int x, int y) position, (int x, int y) velocity)
  {
    public (int x, int y) Position { get; set; } = position;
    public (int x, int y) Velocity { get; set; } = velocity;
  }
}
