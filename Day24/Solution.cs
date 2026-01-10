using System.Diagnostics;
using AocHelper;

namespace Day24;

internal static partial class Program
{
  private const string Title = "\n## Day 24: Crossed Wires ##";
  private const string AdventOfCode = "https://adventofcode.com/2024/day/24";
  private const long ExpectedPartOne = 57588078076750;
  private const string ExpectedPartTwo = "kcd,pfn,shj,tpk,wkb,z07,z23,z27";

  private static Dictionary<string, int> _wires = [];
  private static Dictionary<string, Connection> _connections = [];
  private static HashSet<string> _cachedZWires = [];

  private static long PartOne(string input)
  {
    _wires = [];
    _cachedZWires = [];
    SetGlobalWiresAndConnections(input);

    var zWires = new List<int>();
    var i = 0;

    while (true) {
      var key = Wire('z', i);
      if (!_connections.ContainsKey(key))
        break;
      zWires.Add(GetOutput(key));
      i++;
    }

    zWires.Reverse();
    return Convert.ToInt64(string.Join("", zWires), 2);
  }

  private static string PartTwo(string input)
  {
    var repititions = 4;
    List<string> swappedWires = [];
    var counter = 0;
    var errorLine = FindError();

    while (counter++ < repititions) {
      var foundSwap = false;
      foreach (var x in _connections.Keys) {
        foreach (var y in _connections.Keys) {
          if (x == y)
            continue;
          SwitchConnections(x, y);
          var error = FindError();
          if (error > errorLine) {
            errorLine = error;
            swappedWires.Add(x);
            swappedWires.Add(y);
            foundSwap = true;
            break;
          }
          SwitchConnections(x, y);
        }

        if (foundSwap)
          break;
      }
    }

    swappedWires.Sort();
    return string.Join(",", swappedWires);
  }

  private static void SwitchConnections(string c1, string c2)
  {
    (_connections[c1], _connections[c2]) = (_connections[c2], _connections[c1]);
  }

  private static int GetOutput(string wire)
  {
    if (_wires.TryGetValue(wire, out var value))
      return value;

    var c = _connections[wire];
    _wires[wire] = GetOutput(c.Gate, GetOutput(c.Input1), GetOutput(c.Input2));

    return _wires[wire];
  }

  private static int GetOutput(string gate, int input1, int input2)
  {
    var output = gate switch {
      "AND" => input1 & input2,
      "OR" => input1 | input2,
      "XOR" => input1 ^ input2,
      _ => throw new UnreachableException($"Oops, unknown gate {gate}")
    };

    return output;
  }

  private static int FindError()
  {
    int i = 0;

    while (true) {
      var zWire = Wire('z', i);
      if (!_cachedZWires.Contains(zWire) && !IsCorrect(zWire, i)){
        break;
      }
      _cachedZWires.Add(zWire);
      i++;
    }

    return i;
  }

  private static bool IsCorrect(string wire, int n)
  {
    if (!_connections.TryGetValue(wire, out var c))
      return false;

    if (c.Gate != "XOR")
      return false;
    if (n == 0)
      return SortedPair(c.Input1, c.Input2) == ("x00", "y00");

    return (CheckXor(c.Input1, n) && CheckCarryBit(c.Input2, n))
           || (CheckXor(c.Input2, n) && CheckCarryBit(c.Input1, n));
  }

  private static bool CheckXor(string wire, int n)
  {
    if (!_connections.TryGetValue(wire, out var c))
      return false;

    if (c.Gate != "XOR")
      return false;

    return SortedPair(c.Input1, c.Input2) == (Wire('x', n), Wire('y', n));
  }

  private static bool CheckCarryBit(string wire, int n)
  {
    if (!_connections.TryGetValue(wire, out var c))
      return false;

    if (n == 1) {
      if (c.Gate != "AND")
        return false;
      return SortedPair(c.Input1, c.Input2) == ("x00", "y00");
    }

    if (c.Gate != "OR")
      return false;

    return (CheckCarry(c.Input1, n - 1) && CheckCarryOver(c.Input2, n - 1))
           || (CheckCarry(c.Input2, n - 1) && CheckCarryOver(c.Input1, n - 1));
  }

  private static bool CheckCarry(string wire, int n)
  {
    if (!_connections.TryGetValue(wire, out var c))
      return false;

    if (c.Gate != "AND")
      return false;

    return SortedPair(c.Input1, c.Input2) == (Wire('x', n), Wire('y', n));
  }

  private static bool CheckCarryOver(string wire, int n)
  {
    if (!_connections.TryGetValue(wire, out var c))
      return false;

    if (c.Gate != "AND")
      return false;

    return (CheckXor(c.Input1, n) && CheckCarryBit(c.Input2, n))
           || (CheckXor(c.Input2, n) && CheckCarryBit(c.Input1, n));
  }

  private static string Wire(char ch, int n)
  {
    return ch + n.ToString("00");
  }

  private static (T, T) SortedPair<T>(T item1, T item2) where T : IComparable<T>
  {
    return item1.CompareTo(item2) <= 0 ? (item1, item2) : (item2, item1);
  }

  private static void SetGlobalWiresAndConnections(string input)
  {
    var (wireData, connectionsData) = input.Split("\n\n", StringSplitOptions.RemoveEmptyEntries).ToTuplePair();

    _wires = [];
    foreach (var wire in wireData.Split('\n', StringSplitOptions.RemoveEmptyEntries)) {
      var items = wire.Split(": ", StringSplitOptions.RemoveEmptyEntries);
      _wires.Add(items[0], int.Parse(items[1]));
    }

    _connections = [];
    var connArray = connectionsData.Split('\n', StringSplitOptions.RemoveEmptyEntries);
    _connections = connArray
        .Select(connection => connection.Split(' '))
        .ToDictionary(output => output[4], formula => new Connection(formula[0], formula[2], formula[1]));
  }

  private class Connection(string input1, string input2, string gate)
  {
    public string Input1 { get; set; } = input1;
    public string Input2 { get; set; } = input2;
    public string Gate { get; set; } = gate;
  }
}
