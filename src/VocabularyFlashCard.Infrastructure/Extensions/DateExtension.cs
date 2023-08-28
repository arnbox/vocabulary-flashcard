namespace WorkTimeReport.Infrastructure.Extensions
{
	public static class DateExtension
	{
		private const string _timeZoneName = "W. Europe Standard Time";
		/// <summary>
		/// Get DateTime.Now according to your time zone
		/// </summary>
		/// <param name="timeZoneName">Optional time zone name</param>
		/// <returns>Current DateTime according to time zone</returns>
		public static DateTime ToCustomeTimeZone(this DateTime dateTime, string timeZoneName = _timeZoneName) =>
			TimeZoneInfo.ConvertTimeBySystemTimeZoneId(dateTime, TimeZoneInfo.Local.Id, timeZoneName);
	}
}
