namespace AocHelper;

public static class Helper
{
  public static List<T> List<T>(List<T> list)
  {
    return list;
  }

  public static int ToInt(this string str)
  {
    if (int.TryParse(str, out var value))
      return value;

    throw new InvalidCastException($"Not a valid integer: {str}");
  }

  public static long ToLong(this string str)
  {
    if (long.TryParse(str, out var value))
      return value;

    throw new InvalidCastException($"Not a valid integer: {str}");
  }

  public static T[] CreateArray<T>(int size, T defaultValue)
  {
    T[] array = new T[size];
    for (var i = 0; i < size; i++) array[i] = defaultValue;
    return array;
  }

  public static string Print<T>(this T[] arr, int max = 10)
  {
    if (arr.Length > max) {
      return $"[{string.Join(", ", arr[..max])} ... ]";
    } else {
      return $"[{string.Join(", ", arr)}]";
    }
  }
}
