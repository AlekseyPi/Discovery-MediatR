namespace Discovery.Extensions;

public static class EnumExtensions
{
    public static string ValuesAsString<TEnum>() where TEnum : struct, Enum
    {
        return string.Join(", ", Enum.GetNames<TEnum>());
    }
}