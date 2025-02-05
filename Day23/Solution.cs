using AocHelper;

namespace Day23;

internal static partial class Program
{
  private const string Title = "\n## Day 23: LAN Party ##";
  private const string AdventOfCode = "https://adventofcode.com/2024/day/23";
  private const long ExpectedPartOne = 1467;
  private const string ExpectedPartTwo = "di,gs,jw,kz,md,nc,qp,rp,sa,ss,uk,xk,yn";

  private static Dictionary<string, List<string>> _connections = [];
  private static List<string> LanParty = [];

  private static long PartOne(string input)
  {
    var connectionsData = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);
    _connections = GetConnections(connectionsData);

    var connected = (from x in _connections.Keys
        .Where(x => x[0] == 't')
                     from y in _connections[x]
                     from z in _connections[y]
                     where x != z && _connections[z]
        .Contains(x)
                     select (x, y, z)
        .SortedTriple()).ToList();

    return connected.ToList().Distinct().Count();
  }

  private static string PartTwo(string input)
  {
    LanParty = [];
    foreach (var computer in _connections.Keys) {
      FindLongestChain(computer, [computer]);
    }

    var max = 0;
    var lanParty = string.Empty;
    foreach (var party in LanParty.Where(party => party.Length > max)) {
      max = party.Length;
      lanParty = party;
    }

    return lanParty;
  }

  private static void FindLongestChain(string computer, List<string> connectedComputers)
  {
    connectedComputers.Sort();
    var key = string.Join(',', connectedComputers);
    if (LanParty.Contains(key))
      return;
    LanParty.Add(key);

    foreach (var neighbour in _connections[computer]) {
      if (connectedComputers.Contains(neighbour))
        continue;

      if (connectedComputers.All(connectedComputer => _connections[connectedComputer].Contains(neighbour))) {
        connectedComputers.Add(neighbour);
        FindLongestChain(neighbour, connectedComputers);
      }
    }
  }

  private static Dictionary<string, List<string>> GetConnections(string[] input)
  {
    Dictionary<string, List<string>> connections = [];

    foreach (var c in input) {
      var (x, y) = c.Split('-').ToTuplePair();

      if (!connections.ContainsKey(x))
        connections.Add(x, []);
      if (!connections.ContainsKey(y))
        connections.Add(y, []);

      connections[x].Add(y);
      connections[y].Add(x);
    }

    return connections;
  }

  private static (T, T, T) SortedTriple<T>(this (T, T, T) tuple) where T : IComparable<T>
  {
    List<T> list = [tuple.Item1, tuple.Item2, tuple.Item3];
    list.Sort();
    return (list[0], list[1], list[2]);
  }
}
