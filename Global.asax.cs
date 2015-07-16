using System.Web;
using System.Web.Http;
using Nasicus.Toggl.Overtime.App_Start;

namespace Nasicus.Toggl.Overtime
{
	public class WebApiApplication : HttpApplication
	{
		protected void Application_Start()
		{
			GlobalConfiguration.Configure(WebApiConfig.Register);
		}
	}
}
﻿using System.Web;
using System.Web.Http;
using Nasicus.Toggl.Overtime.App_Start;

namespace Nasicus.Toggl.Overtime
{
	public class WebApiApplication : HttpApplication
	{
		protected void Application_Start()
		{
			GlobalConfiguration.Configure(WebApiConfig.Register);
		}
	}
}
