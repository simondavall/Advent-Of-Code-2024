using AocHelper;

namespace Day19;

internal static partial class Program
{
  private const string Title = "\n## Day 19: Linen Layout ##";
  private const string AdventOfCode = "https://adventofcode.com/2024/day/19";
  private const long ExpectedPartOne = 347;
  private const long ExpectedPartTwo = 919219286602165;

  private static Dictionary<string, bool> CachedMatches = [];
  private static Dictionary<(string design, string towel), long> CachedMatchCounts = [];
  private static Dictionary<string, long> CachedCounts = [];

  private static long PartOne(string input)
  {
    ClearCache();
    var (towels, designs) = ProcessInput(input);

    long tally = 0;
    foreach (var design in designs) {
      CachedMatches.Clear();
      tally += HasMatch(towels, design) ? 1 : 0;
    }

    return tally;
  }

  private static long PartTwo(string input)
  {
    var (towels, designs) = ProcessInput(input);
    long tally = 0;

    foreach (var design in designs) {
      CachedMatchCounts.Clear();
      var count = CountMatches(towels, design);
      tally += count;
    }

    return tally;
  }

  private static bool HasMatch(string[] towels, string design)
  {
    if (towels.Contains(design))
      return true;

    if (CachedMatches.TryGetValue(design, out var match))
      return match;

    var hasMatch = false;

    foreach (var towel in towels.Where(design.StartsWith)) {
      if (design.StartsWith(towel)) {
        hasMatch = HasMatch(towels, design[towel.Length..]);
      }

      if (hasMatch) {
        CachedMatches.TryAdd(design, true);
        return true;
      }
    }

    CachedMatches.Add(design, false);
    return false;
  }

  private static long CountMatches(string[] towels, string design)
  {
    if (design.Length == 0)
      return 1;

    if (CachedCounts.TryGetValue(design, out var cachedMatches))
      return cachedMatches;

    long count = 0;

    foreach (var towel in towels.Where(design.StartsWith)) {
      if (CachedMatchCounts.TryGetValue((design, towel), out var cachedCount)) {
        count += cachedCount;
        continue;
      }

      var newDesign = design[towel.Length..];
      var matches = CountMatches(towels, newDesign);
      CachedMatchCounts.Add((newDesign, towel), matches);
      count += matches;

      CachedMatchCounts.TryAdd((newDesign, towel), count);
    }

    CachedCounts.Add(design, count);
    return count;
  }

  private static void ClearCache(){
    CachedCounts = [];
    CachedMatchCounts = [];
    CachedMatches = [];
  }

  private static (string[] towels, string[] designs) ProcessInput(string input)
  {
    var (towelData, designData) = input.Split("\n\n", StringSplitOptions.RemoveEmptyEntries).ToTuplePair();

    var towels = towelData.Split(", ");
    var designs = designData.Split('\n', StringSplitOptions.RemoveEmptyEntries);

    return (towels, designs);
  }
}
