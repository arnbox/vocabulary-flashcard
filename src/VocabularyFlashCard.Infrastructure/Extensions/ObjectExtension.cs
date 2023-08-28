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
	public static void TrimAllStrings<TSelf>(this TSelf obj)
	{
		if (obj is null)
		{
			return;
		}

		BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;

		foreach (PropertyInfo p in obj.GetType().GetProperties(flags))
		{
			Type currentNodeType = p.PropertyType;
			if (currentNodeType == typeof(string))
			{
				string currentValue = (string?)p.GetValue(obj) ?? string.Empty;
				if (!string.IsNullOrEmpty(currentValue))
				{
					p.SetValue(obj, currentValue.Trim());
				}
			}
			else if (currentNodeType != typeof(object) && Type.GetTypeCode(currentNodeType) == TypeCode.Object)
			{
				if (!currentNodeType.Name.Contains(nameof(IEnumerable)))
				{
					p.GetValue(obj).TrimAllStrings();
				}
			}
		}
	}
}
