namespace Nasicus.Toggl.Overtime.Model
{
	public interface ITimeSummary
	{
		double Overtime { get; }
		double Worktime { get; }
	}
}