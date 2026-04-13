using System.ComponentModel;
using System.Reflection;

namespace AionOverdose58.Shared.Extensions;

public static class EnumExtensions
{
    public static string GetDisplayName(this Enum value)
    {
        FieldInfo? field = value.GetType().GetField(value.ToString());
        if (field?.GetCustomAttribute<DescriptionAttribute>() is { } attr)
            return attr.Description;
        return value.ToString();
    }
}
