using System;
using System.Globalization;
using System.Linq;
using System.Web.Http;
using Nasicus.Toggl.Overtime.App_Start;
using Nasicus.Toggl.Overtime.Model;
using Nasicus.Toggl.Overtime.Utility;
using Toggl;
using Toggl.QueryObjects;
using Toggl.Services;

namespace Nasicus.Toggl.Overtime.ApiControllers
{
	public class DataController : ApiController
	{
		[HttpGet]
		public TogglTimeSummary Get(string apiToken, string regularWorkingHoursString, string startDateString, string endDateString)
		{
			double regularWorkingHours = Double.Parse(regularWorkingHoursString);
			DateTime startDate = DateTime.ParseExact(startDateString, WebApiConfig.DateFormat, CultureInfo.InvariantCulture);
			DateTime endDate = DateTime.ParseExact(endDateString, WebApiConfig.DateFormat, CultureInfo.InvariantCulture);

			double workDayInSeconds = (regularWorkingHours/ 5.0 * 3600);

			var togglTimeSummary = new TogglTimeSummary();

			var timeService = new TimeEntryService(apiToken);
			var timeParams = new TimeEntryParams
				{
					StartDate = startDate,
					EndDate = endDate.AddDays(1)
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
					weekSummary = new WeekSummary(currentYearAndWeek, workDayInSeconds);
					togglTimeSummary.AddWeek(weekSummary);
				}

				weekSummary.AddDay(daySummary);

				foreach (long duration in dayEntry.Where(t => t.Duration != null).Select(timeEntry => (long)timeEntry.Duration))
				{
					daySummary.AddTimeEntry(duration);
					currentDuration += duration;
				}

				daySummary.Worktime = currentDuration;

				if (currentDuration <= 0)
				{
					continue;
				}

				daySummary.Overtime = currentDuration - workDayInSeconds;
				weekSummary.Worktime += currentDuration;
				overTime += daySummary.Overtime;
				workTime += daySummary.Worktime;
				weekSummary.Overtime += daySummary.Overtime;
			}

			togglTimeSummary.Overtime = overTime;
			togglTimeSummary.Worktime = workTime;

			return togglTimeSummary;
		}
	}
}