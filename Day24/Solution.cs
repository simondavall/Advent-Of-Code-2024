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

  private static long PartOne(string input)
  {
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
    List<string> swappedWires = [];

    var repeat = 0;
    while (repeat++ < 4) {
      var foundSwap = false;
      var errorLine = FindError();
      foreach (var x in _connections.Keys) {
        foreach (var y in _connections.Keys) {
          if (x == y)
            continue;
          (_connections[x], _connections[y]) = (_connections[y], _connections[x]);
          if (FindError() > errorLine) {
            swappedWires.Add(x);
            swappedWires.Add(y);
            foundSwap = true;
            break;
          }
          (_connections[x], _connections[y]) = (_connections[y], _connections[x]);
        }

        if (foundSwap)
          break;
      }
    }

    swappedWires.Sort();
    return string.Join(",", swappedWires);
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
    var i = 0;

    while (true) {
      if (!CheckGraph(i))
        break;
      i++;
    }

    return i;
  }

  private static bool CheckGraph(int n)
  {
    return CheckZWires(Wire('z', n), n);
  }

  private static bool CheckZWires(string wire, int n)
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
