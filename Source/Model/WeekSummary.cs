using System;
using System.Collections.Generic;
using System.Linq;

namespace Nasicus.Toggl.Overtime.Model
{
  public class WeekSummary : ITimeSummary
  {
    private const int regularWorkDaysPerWeek = 5;

    private readonly double workDayInSeconds;

    public string DisplayName { get; private set; }

    public double Overtime { get; private set; }

    public double Worktime { get; private set; }

    public IEnumerable<DaySummary> Days => _days;
    private readonly List<DaySummary> _days = new List<DaySummary>();

    public double RegularWorkingSeconds => (_days.Count > regularWorkDaysPerWeek ? regularWorkDaysPerWeek : _days.Count) * workDayInSeconds;

    internal DateTime EarliestDay => Days.Select(d => d.Date).Min();

    internal DateTime LatestDay => Days.Select(d => d.Date).Max();

    internal bool IsRegularWorkDayLimitReached => _days.Count >= regularWorkDaysPerWeek;

    public WeekSummary(string displayName, double workDayInSeconds)
    {
      DisplayName = displayName;
      this.workDayInSeconds = workDayInSeconds;
    }

    public void AddDay(DaySummary daySummary)
    {
      _days.Add(daySummary);
      Worktime += daySummary.Worktime;
      Overtime += daySummary.Overtime;
    }
  }
}