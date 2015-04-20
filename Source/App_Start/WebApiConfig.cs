using System.Web.Http;

namespace Nasicus.Toggl.Overtime.App_Start
{
	public static class WebApiConfig
	{
		public const string DateFormat = "MM-dd-yyyy";

		public static void Register(HttpConfiguration config)
		{
			config.MapHttpAttributeRoutes();

			config.Routes.MapHttpRoute("DefaultApi",
			                           "api/{controller}/{apiToken}/{regularWorkingHoursString}/{startDateString}/{endDateString}");
		}
	}
}
