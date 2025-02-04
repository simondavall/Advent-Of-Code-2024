using System.Drawing;

using AocHelper;

namespace Day15;

internal static partial class Program
{
  private const string Title = "\n## Day 15: Warehouse Woes ##";
  private const string AdventOfCode = "https://adventofcode.com/2024/day/15";
  private const long ExpectedPartOne = 1414416;
  private const long ExpectedPartTwo = 1386070;

  private static int _mapHeight;
  private static int _mapWidth;

  private static long PartOne(string input)
  {
    var (map, instructions) = ProcessData(input);

    var charMap = map.To2DCharArray();
    var currentPosition = FindStartingPosition(charMap);
    var index = 0;

    while (index < instructions.Length) {
      var nextInstruction = instructions[index];
      currentPosition = Robot.Move(charMap, nextInstruction, currentPosition);
      index++;
    }

    return CalcTally(charMap);
  }

  private static long PartTwo(string input)
  {
    var (map, instructions) = ProcessData(input);
    var charMap = map.ToExpanded2DCharArray();
    var currentPosition = FindStartingPosition(charMap);
    var index = 0;

    //PrintMap(charMap, 0, ' ');
    while (index < instructions.Length) {
      var nextInstruction = instructions[index];
      var nextPosition = Robot.Move2(charMap, nextInstruction, currentPosition);

      currentPosition = nextPosition;
      index++;
    }

    //PrintMap(charMap, index, ' ');
    return CalcTally(charMap);
  }

  private static long CalcTally(char[,] map)
  {
    long tally = 0;
    for (var y = 0; y < _mapHeight; y++)
      for (var x = 0; x < _mapWidth; x++)
        if (map[x, y] is 'O' or '[') {
          tally += 100 * y + x;
        }

    return tally;
  }

  private static Point FindStartingPosition(char[,] map)
  {
    for (var y = 0; y < _mapHeight; y++) {
      for (var x = 0; x < _mapWidth; x++)
        if (map[x, y] == '@')
          return new Point(x, y);
    }

    return new Point(-1, -1);
  }

  private static char[] GetInstructionSet(string input)
  {
    List<char> instructions = [];
    foreach (var line in input.Split('\n')) {
      instructions.AddRange(line.ToCharArray());
    }
    return instructions.ToArray();
  }

  private static char[,] To2DCharArray(this string[] input)
  {
    var charArray = new char[input[0].Length, input.Length];
    for (var y = 0; y < input.Length; y++) {
      for (var x = 0; x < input[0].Length; x++) {
        charArray[x, y] = input[y][x];
      }
    }

    return charArray;
  }

  private static char[,] ToExpanded2DCharArray(this string[] input)
  {
    _mapWidth = input[0].Length * 2;
    var charArray = new char[_mapWidth, input.Length];
    for (var y = 0; y < input.Length; y++) {
      var newString = input[y].Replace("#", "##").Replace("O", "[]").Replace(".", "..").Replace("@", "@.");
      for (var x = 0; x < _mapWidth; x++) {
        charArray[x, y] = newString[x];
      }
    }

    return charArray;
  }

  private static (string[] map, char[] instructions) ProcessData(string input)
  {
    var (mapData, instructionData) = input.Split("\n\n", StringSplitOptions.RemoveEmptyEntries).ToTuplePair();

    var map = mapData.Split('\n', StringSplitOptions.RemoveEmptyEntries);
    _mapHeight = map.Length;
    _mapWidth = map[0].Length;

    var instructions = GetInstructionSet(instructionData);

    return (map, instructions);
  }

  private static void PrintMap(char[,] map, int count, char c)
  {
    Console.WriteLine($"Map at {count} secs with instruction {c}");
    for (var y = 0; y < _mapHeight; y++) {
      Console.Write($"{y}: ");
      for (var x = 0; x < _mapWidth; x++) {
        Console.Write(map[x, y]);
      }
      Console.WriteLine();
    }
    Console.WriteLine();
  }
}
