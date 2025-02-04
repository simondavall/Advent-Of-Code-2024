namespace Day13;

internal static partial class Program
{
  private const string Title = "\n## Day 13: Claw Contraption ##";
  private const string AdventOfCode = "https://adventofcode.com/2024/day/13";
  private const long ExpectedPartOne = 38839;
  private const long ExpectedPartTwo = 75200131617108;

  private static long PartOne(string input)
  {
    Machine[] machines = GetMachines(input);

    long tally = 0;
    foreach (var m in machines) {
      var (a, b) = CalcAB(m);
      tally += a * 3 + b * 1;
    }

    return tally;
  }

  private static long PartTwo(string input)
  {
    Machine[] machines = GetMachines(input);

    long tally = 0;
    foreach (var m in machines) {
      var nx = m.Prize.x + 10_000_000_000_000;
      var ny = m.Prize.y + 10_000_000_000_000;
      m.Prize = (nx, ny);

      var (a, b) = CalcAB(m);
      tally += a * 3 + b * 1;
    }

    return tally;
  }

  private static (long a, long b) CalcAB(Machine m)
  {
    var top = (m.Prize.x * m.B.y) - (m.B.x * m.Prize.y);
    var bottom = m.A.x * m.B.y - m.A.y * m.B.x;
    if (bottom == 0)
      throw new DivideByZeroException();

    var a = (double)top / bottom;

    if (a != (long)a)
      return (0, 0);

    var b = (m.Prize.x - m.A.x * a) / m.B.x;
    return b == (long)b ? ((long)a, (long)b) : (0, 0);
  }

  private static Machine[] GetMachines(string input)
  {
    var data = input.Split("\n\n", StringSplitOptions.RemoveEmptyEntries);
    return data.Select(Machine.CreateMachine).ToArray();
  }

  internal class Machine
  {
    public (long x, long y) A { get; set; }
    public (long x, long y) B { get; set; }
    public (long x, long y) Prize { get; set; }

    internal static Machine CreateMachine(string input)
    {
      var machine = new Machine();
      var lines = input.Split("\n");

      machine.A = (GetX(lines[0]), GetY(lines[0]));
      machine.B = (GetX(lines[1]), GetY(lines[1]));
      machine.Prize = (GetX(lines[2]), GetY(lines[2]));

      return machine;
    }

    private static long GetX(string input)
    {
      var i = input.IndexOf('X') + 2;
      var j = input.IndexOf(',');
      return long.Parse(input.Substring(i, j - i));
    }

    private static long GetY(string input)
    {
      var i = input.IndexOf('Y') + 2;
      return long.Parse(input[i..]);
    }
  }


}
