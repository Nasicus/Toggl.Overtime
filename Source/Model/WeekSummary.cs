using System;
using System.Collections.Generic;
using System.Linq;
using Nasicus.Toggl.Overtime.Utility;

namespace Nasicus.Toggl.Overtime.Model
{
	public class WeekSummary : ITimeSummary
	{
		public string DisplayName;

		public double Overtime { get; set; }

		public double Worktime { get; set; }

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
		
		public WeekSummary(string displayName)
		{
			DisplayName = displayName;
		}

		public void AddDay(DaySummary daySummary)
		{
			Days.Add(daySummary);
		}
	}
}