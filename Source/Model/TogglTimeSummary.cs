using System.Collections.Generic;

namespace Nasicus.Toggl.Overtime.Model
{
	public class TogglTimeSummary : ITimeSummary
	{
		public double Overtime { get; set; }

		public double Worktime { get; set; }

		public readonly List<WeekSummary> Weeks = new List<WeekSummary>();

		public void AddWeek(WeekSummary weekSummary)
		{
			Weeks.Add(weekSummary);
		}
	}
}