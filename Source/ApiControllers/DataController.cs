using System;
using System.Globalization;
using System.Linq;
using System.Web.Http;
using Nasicus.Toggl.Overtime.Model;
using Nasicus.Toggl.Overtime.Utility;
using Toggl;
using Toggl.QueryObjects;
using Toggl.Services;

namespace Nasicus.Toggl.Overtime.ApiControllers
{
	public class DataController : ApiController
	{
		public const string DateFormat = "MM-dd-yyyy";

		[HttpGet]
		public TogglTimeSummary Get(string apiToken, string regularWorkingHoursString, string startDateString, string endDateString)
		{
			int regularWorkingHours = Int32.Parse(regularWorkingHoursString);
			DateTime startDate = DateTime.ParseExact(startDateString, DateFormat, CultureInfo.InvariantCulture);
			DateTime endDate = DateTime.ParseExact(endDateString, DateFormat, CultureInfo.InvariantCulture);

			double workDayInSeconds = (regularWorkingHours/ 5.0 * 3600);

			var togglTimeSummary = new TogglTimeSummary();

			var timeService = new TimeEntryService(apiToken);
			var timeParams = new TimeEntryParams
				{
					StartDate = startDate,
					EndDate = endDate
				};

			double overTime = 0;
			double workTime = 0;
			WeekSummary weekSummary = null;

			foreach (IGrouping<DateTime, TimeEntry> dayEntry in timeService.List(timeParams)
			                                                               .GroupBy(h => DateTime.Parse(h.Start).Date)
			                                                               .OrderByDescending(d => d.Key))
			{
				var daySummary = new DaySummary(dayEntry.Key);
				long currentDuration = 0;

				string currentYearAndWeek = String.Format("{0}-{1}",
				                                          daySummary.Date.Year,
				                                          DateTimeUtility.GetIso8601WeekOfYear(daySummary.Date));

				if (weekSummary == null || currentYearAndWeek != weekSummary.DisplayName)
				{
					weekSummary = new WeekSummary(currentYearAndWeek);
					togglTimeSummary.AddWeek(weekSummary);
				}

				weekSummary.AddDay(daySummary);

				foreach (TimeEntry timeEntry in dayEntry.Where(t => t.Duration != null))
				{
					long duration = (long)timeEntry.Duration;

					daySummary.AddTimeEntry(duration);
					currentDuration += duration;
				}

				daySummary.Worktime = currentDuration;

				if (currentDuration > 0)
				{
					daySummary.Overtime = currentDuration - workDayInSeconds;
					weekSummary.Worktime += currentDuration;
					overTime += daySummary.Overtime;
					workTime += daySummary.Worktime;
					weekSummary.Overtime += daySummary.Overtime;
				}
			}

			togglTimeSummary.Overtime = overTime;
			togglTimeSummary.Worktime = workTime;

			return togglTimeSummary;
		}
	}

	public static class Parameters
	{
		public const double WorkDayInSeconds = (42.0/5)*3600;
		public const string StartDate = "01.04.2014";
	}
}