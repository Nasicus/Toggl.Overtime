using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Nasicus.Toggl.Overtime.Model
{
  public class DaySummary : ITimeSummary
  {
    private readonly double targetWorkTimeInSeconds;

    [JsonProperty(ItemConverterType = typeof(JavaScriptDateTimeConverter))]
    public DateTime Date { get; private set; }

    public double Worktime { get; private set; }

    public double Overtime => Worktime - targetWorkTimeInSeconds;

    public IEnumerable<long> TimeEntries => _timeEntries;
    private readonly List<long> _timeEntries = new List<long>();

    public DaySummary(DateTime date, double targetWorkTimeInSeconds)
    {
      Date = date;
      this.targetWorkTimeInSeconds = targetWorkTimeInSeconds;
    }

    public void AddTimeEntry(long timeSpanHours)
    {
      _timeEntries.Add(timeSpanHours);
      Worktime += timeSpanHours;
    }
  }
}