using System.Collections;
using System.Reflection;

namespace VocabularyFlashCard.Infrastructure.Extensions;
public static class ObjectExtensions
{
    /// <summary>
    /// Trims all string values in the Object
    /// Arrays are ignored
    /// </summary>
    /// <typeparam name="TSelf"></typeparam>
    /// <param name="obj"></param>
    public static void TrimAllStrings<T>(this T obj)
    {
        if (obj is null)
        {
            return;
        }

        BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy;

        var properties = obj.GetType().GetProperties(flags).Where(p => p is not null && p.CanWrite);

        foreach (PropertyInfo p in properties)
        {
            Type nodeType = p.PropertyType;
            if (nodeType == typeof(string))
            {
                string value = (string?)p.GetValue(obj) ?? string.Empty;
                if (!string.IsNullOrEmpty(value))
                {
                    p.SetValue(obj, value.Trim());
                }
            }
            else if (nodeType != typeof(object) && Type.GetTypeCode(nodeType) == TypeCode.Object)
            {
                if (!nodeType.Name.Contains(nameof(IEnumerable)))
                {
                    p.GetValue(obj)?.TrimAllStrings();
                }
            }
            else if (p is IEnumerable pEnumerable)
            {
                foreach (var item in pEnumerable)
                {
                    item.TrimAllStrings();
                }
            }
        }
    }

    //private static void trimProperty(PropertyInfo prop, Object obj)
    //{
    //    Type nodeType = prop.PropertyType;
    //    if (nodeType == typeof(string))
    //    {
    //        string value = (string?)prop.GetValue(obj) ?? string.Empty;
    //        if (!string.IsNullOrEmpty(value))
    //        {
    //            prop.SetValue(obj, value.Trim());
    //        }
    //    }
    //}
}
