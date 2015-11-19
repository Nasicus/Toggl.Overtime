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
    public HttpResponseMessage Get(string apiToken, string regularWorkingHoursString, string startDateString, string endDateString)
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
        return Request.CreateErrorResponse(HttpStatusCode.PreconditionFailed,
                                           "At least one of the passed parameters was not in the correct format!");
      }

      Dictionary<DateTime, List<TimeEntry>> workTimeByDays;
      try
      {
        var timeService = new TimeEntryService(apiToken);
        workTimeByDays = timeService.List(new TimeEntryParams {StartDate = startDate, EndDate = endDate.AddDays(1)})
                                    .GroupBy(h => DateTime.Parse(h.Start).Date)
                                    .OrderBy(d => d.Key)
                                    .ToDictionary(timeEntriesByDay => timeEntriesByDay.Key,
                                                  timeEntriesByDay => timeEntriesByDay.ToList());
      }
      catch (WebException webEx)
      {
        var httpWebResponse = ((HttpWebResponse) webEx.Response);
        return Request.CreateErrorResponse(HttpStatusCode.PreconditionFailed,
                                           $"The toggl server (toggl.com) threw an exception ({(int) httpWebResponse.StatusCode}:" +
                                           $" {httpWebResponse.StatusDescription}) - either your API token is wrong or the servers are down.");
      }

      double regularWorkDayInSeconds = (regularWorkingHours / 5.0 * 3600);
      var togglTimeSummary = new TogglTimeSummary();

      foreach (var daysByWeek in workTimeByDays.GroupBy(timeEntriesByDay => GetWeekGroupKey(timeEntriesByDay.Key),
                                                        timeEntriesByDay => new {Day = timeEntriesByDay.Key, TimeEntries = timeEntriesByDay.Value}))
      {
        var weekSummary = new WeekSummary(daysByWeek.Key, regularWorkDayInSeconds);

        foreach (var timeEntriesByDay in daysByWeek)
        {
          var daySummary = new DaySummary(timeEntriesByDay.Day, weekSummary.IsRegularWorkDayLimitReached ? 0 : regularWorkDayInSeconds);

          foreach (long timeEntry in timeEntriesByDay.TimeEntries
                                                     .Where(t => t.Duration.HasValue && t.Duration > 0)
                                                     .Select(t => (long) t.Duration))
          {
            daySummary.AddTimeEntry(timeEntry);
          }

          weekSummary.AddDay(daySummary);
        }

        togglTimeSummary.AddWeek(weekSummary);
      }

      return Request.CreateResponse(HttpStatusCode.OK, togglTimeSummary);
    }

    private static string GetWeekGroupKey(DateTime day)
    {
      return $"{day.Year}-{DateTimeUtility.GetIso8601WeekOfYear(day)}";
    }
  }
}