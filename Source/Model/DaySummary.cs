﻿using System;
using System.Collections.Generic;

namespace Nasicus.Toggl.Overtime.Model
{
	public class DaySummary : ITimeSummary
	{
		public DateTime Date;
		public readonly List<long> TimeEntries = new List<long>();

		public double Overtime { get; set; }
		public double Worktime { get; set; }

		public DaySummary(DateTime date)
		{
			Date = date;
		}

		public void AddTimeEntry(long timeSpanHours)
		{
			TimeEntries.Add(timeSpanHours);
		}
	}
}