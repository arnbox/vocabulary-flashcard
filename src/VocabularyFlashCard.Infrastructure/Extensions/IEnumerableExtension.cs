using System.Diagnostics.CodeAnalysis;

namespace WorkTimeReport.Infrastructure.Extensions
{
	public static class IEnumerableExtension
	{
		/// <summary>
		/// Check an IEnumerable is null or empty
		/// </summary>
		/// <param name=""></param>
		/// <returns>true if is null or empty otherwise false</returns>
		public static bool IsNullOrEmpty<T>([NotNullWhen(false)] this IEnumerable<T>? enumerable)
		{
			if (enumerable == null)
				return true;

			return !enumerable.Any();
		}


		/// <summary>
		/// Check an IEnumerable is null or empty
		/// </summary>
		/// <param name=""></param>
		/// <returns>true if is null or empty otherwise false</returns>
		public static bool HasValue<T>([NotNullWhen(true)] this IEnumerable<T>? enumerable)
		{
			return (enumerable != null && enumerable.Any());
		}
	}
}
