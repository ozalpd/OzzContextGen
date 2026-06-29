using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Resources;

namespace OzzContextGen.Core.Helpers;

/// <summary>
/// Represents a strongly typed enumeration value and its associated display text for use in UI selection or display
/// scenarios.
/// </summary>
/// <remarks>This class is commonly used to bind enumeration values to user interface elements, such as
/// dropdown lists or combo boxes, where both the underlying value and a user-friendly display string are
/// needed.</remarks>
/// <typeparam name="T">The enumeration type represented by this item.</typeparam>
public class EnumValueItem<T> where T : Enum
{
    public required T Value { get; set; }
    public string DisplayValue { get; set; } = string.Empty;
}

/// <summary>
/// Provides extension methods for enum types, including attribute retrieval and localized display value resolution.
/// </summary>
public static class EnumExtensions
{
    /// <summary>
    /// A generic extension method that aids in reflecting and retrieving any attribute that is applied to an `Enum`.
    /// </summary>
    public static TAttribute? GetAttribute<TAttribute>(this Enum enumValue) where TAttribute : Attribute
    {
        var attribute = enumValue.GetType()
                        .GetMember(enumValue.ToString())
                        .First()
                        .GetCustomAttribute<TAttribute>();

        return attribute;
    }

    /// <summary>
    /// Returns the <see cref="DisplayAttribute.Order"/> value for an enum member,
    /// or <see cref="int.MaxValue"/> when no <c>[Display(Order = ...)]</c> attribute is present.
    /// </summary>
    public static int GetDisplayOrder(this Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        if (field == null)
            return int.MaxValue;

        return field.GetCustomAttribute<DisplayAttribute>()?.GetOrder() ?? int.MaxValue;
    }

    /// <summary>
    /// Gets the display value for an enum member from its DisplayAttribute.
    /// </summary>
    /// <param name="value">The enum value.</param>
    /// <returns>The display name from DisplayAttribute resources if available, the DisplayAttribute Name, or the enum's
    /// string representation.</returns>
    public static string GetDisplayValue(this Enum value)
    {
        var fieldInfo = value.GetType().GetField(value.ToString());

        if (fieldInfo == null)
            return value.ToString();

        DisplayAttribute[] descriptionAttributes = fieldInfo.GetCustomAttributes(
                                                             typeof(DisplayAttribute),
                                                             false) as DisplayAttribute[] ?? [];
        if (descriptionAttributes.Length > 0 && descriptionAttributes[0].ResourceType != null)
        {
            return LookupResource(descriptionAttributes[0].ResourceType!, descriptionAttributes[0].Name ?? string.Empty);
        }

        return (descriptionAttributes.Length > 0) ? descriptionAttributes[0].Name ?? value.ToString() : value.ToString();
    }

    /// <summary>
    /// Gets all values of the specified enum type with their display values, sorted by
    /// <see cref="DisplayAttribute.Order"/> when present on the enum members.
    /// </summary>
    /// <remarks>
    /// Members without a <c>[Display(Order = ...)]</c> attribute default to <c>int.MaxValue</c>
    /// so they appear after explicitly ordered members. Members with the same order value retain
    /// their declaration order (stable sort).
    /// Use this instead of <see cref="GetValues{T}"/> when enum members carry
    /// <c>[Display(Order = ...)]</c> and the UI should respect that order.
    /// </remarks>
    /// <typeparam name="T">The enum type whose values to retrieve.</typeparam>
    /// <returns>
    /// A collection of <see cref="EnumValueItem{T}"/> sorted by <see cref="DisplayAttribute.Order"/>.
    /// </returns>
    public static IEnumerable<EnumValueItem<T>> GetOrderedValues<T>() where T : Enum
    {
        return GetValues<T>().OrderBy(x => x.Value.GetDisplayOrder());
    }

    /// <summary>
    /// Gets all values of the specified enum type with their display values.
    /// </summary>
    /// <typeparam name="T">The enum type whose values to retrieve.</typeparam>
    /// <returns>A collection of enum value items containing each enum value and its display representation.</returns>
    public static IEnumerable<EnumValueItem<T>> GetValues<T>() where T : Enum
    {
        return Enum.GetValues(typeof(T)).Cast<T>().Select(x => new EnumValueItem<T>
        {
            Value = x,
            DisplayValue = x.GetDisplayValue()
        });
    }

    private static string LookupResource(Type? resourceManagerProvider, string resourceKey)
    {
        if (resourceManagerProvider == null)
            return resourceKey;

        foreach (PropertyInfo staticProperty in resourceManagerProvider.GetProperties(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public))
        {
            if (staticProperty.PropertyType == typeof(ResourceManager))
            {
                var resourceManager = (ResourceManager?)staticProperty.GetValue(null, null);
                return resourceManager?.GetString(resourceKey) ?? resourceKey;
            }
        }

        return resourceKey; // Fallback with the key name
    }
}