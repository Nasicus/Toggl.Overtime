using System;
using System.Collections.Generic;
using System.Linq;

namespace Nasicus.Toggl.Overtime.Model
{
	public class WeekSummary : ITimeSummary
	{
		public string DisplayName;

		public double Overtime { get; set; }

		public double Worktime { get; set; }

		public double RegularWorkingSeconds
		{
			get
			{
				return Days.Count*workDayInSeconds;
			}
		}

		public List<DaySummary> Days = new List<DaySummary>();

		internal DateTime EarliestDay
		{
			get
			{
				return Days.Select(d => d.Date).Min();
			}
		}

		internal DateTime LatestDay
		{
			get
			{
				return Days.Select(d => d.Date).Max();
			}
		}

		private readonly double workDayInSeconds;

		public WeekSummary(string displayName, double workDayInSeconds)
		{
			DisplayName = displayName;
			this.workDayInSeconds = workDayInSeconds;
		}

		public void AddDay(DaySummary daySummary)
		{
			Days.Add(daySummary);
		}
	}
}