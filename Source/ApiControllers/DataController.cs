using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
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
    public HttpResponseMessage Get(string apiToken, string regularWorkingHoursString, string startDateString,
      string endDateString)
    {
      double regularWorkingHours;
      DateTime startDate;
      DateTime endDate;
      try
      {
        regularWorkingHours = double.Parse(regularWorkingHoursString);
        startDate = DateTime.ParseExact(startDateString, WebApiConfig.DateFormat, CultureInfo.InvariantCulture);
        endDate = DateTime.ParseExact(endDateString, WebApiConfig.DateFormat, CultureInfo.InvariantCulture);
      }
      catch (Exception)
      {
        return Request.CreateErrorResponse(HttpStatusCode.PreconditionFailed, "At least one of the passed parameters was not in the correct format!");
      }

      double workDayInSeconds = (regularWorkingHours/5.0*3600);

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

      Dictionary<DateTime, List<TimeEntry>> orderByDescending;
      try
      {
        orderByDescending = timeService.List(timeParams)
                                       .GroupBy(h => DateTime.Parse(h.Start).Date)
                                       .OrderByDescending(d => d.Key).ToDictionary(kvp => kvp.Key, kvp => kvp.ToList());
      }
      catch (WebException webEx)
      {
        HttpWebResponse httpWebResponse = ((HttpWebResponse)webEx.Response);
        HttpStatusCode statusCode = httpWebResponse.StatusCode;
        string statusDescription = httpWebResponse.StatusDescription;
        return Request.CreateErrorResponse(HttpStatusCode.PreconditionFailed,
                                           $"The toggl server (toggl.com) threw an exception ({(int)statusCode}: {statusDescription}) - either your API token is wrong or the servers are down.");
      }

      foreach (KeyValuePair<DateTime, List<TimeEntry>> dayEntry in orderByDescending)
      {
        DaySummary daySummary = new DaySummary(dayEntry.Key);
        long currentDuration = 0;

        string currentYearAndWeek = $"{daySummary.Date.Year}-{DateTimeUtility.GetIso8601WeekOfYear(daySummary.Date)}";

        if (weekSummary == null || currentYearAndWeek != weekSummary.DisplayName)
        {
          weekSummary = new WeekSummary(currentYearAndWeek, workDayInSeconds);
          togglTimeSummary.AddWeek(weekSummary);
        }

        foreach (long duration in dayEntry.Value.Where(t => t.Duration != null).Select(timeEntry => (long) timeEntry.Duration))
        {
          daySummary.AddTimeEntry(duration);
          currentDuration += duration;
        }

        daySummary.Worktime = currentDuration;

        if (currentDuration <= 0)
        {
          continue;
        }

        weekSummary.AddDay(daySummary);

        daySummary.Overtime = currentDuration - workDayInSeconds;
        weekSummary.Worktime += currentDuration;
        overTime += daySummary.Overtime;
        workTime += daySummary.Worktime;
        weekSummary.Overtime += daySummary.Overtime;
      }

      togglTimeSummary.Overtime = overTime;
      togglTimeSummary.Worktime = workTime;

      return Request.CreateResponse(HttpStatusCode.OK, togglTimeSummary);
    }
  }
}