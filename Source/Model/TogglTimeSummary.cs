using System.Collections.Generic;
using System.Linq;

namespace Nasicus.Toggl.Overtime.Model
{
  public class TogglTimeSummary : ITimeSummary
  {
    public double Overtime { get; private set; }

    public double Worktime { get; private set; }

    public IEnumerable<WeekSummary> Weeks => _weeks.OrderByDescending(w => w.EarliestDay);
    private readonly List<WeekSummary> _weeks = new List<WeekSummary>();

    public void AddWeek(WeekSummary weekSummary)
    {
      _weeks.Add(weekSummary);
      Worktime += weekSummary.Worktime;
      Overtime += weekSummary.Overtime;
    }
  }
}