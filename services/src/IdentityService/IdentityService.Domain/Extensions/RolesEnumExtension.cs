using System.Reflection;
using IdentityService.Domain.Enums;

namespace IdentityService.Domain.Extensions;

/// <summary>
/// Методы расширения для получения строки из роли и роли из строки
/// </summary>
public static class RolesEnumExtension
{
    public static string GetString(this Role role)
    {
        return role.ToString().ToLowerInvariant();
    }

    public static Role? GetRolesEnum(this string role)
    {
        FieldInfo[] fields = typeof(Role).GetFields();

        foreach (FieldInfo field in fields)
        {
            if (field?.ToString()?.ToLowerInvariant() == role.ToLowerInvariant())
            {
                object? res = field.GetValue(null) ?? null;
                return res == null ? null : (Role)res;
            }
        }
        return null;
    }
}
