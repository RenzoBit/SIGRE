using System.Web.Mvc;

namespace SIGRE.WebApplication.Areas.SEGU
{
	public class SEGUAreaRegistration : AreaRegistration
	{
		public override string AreaName
		{
			get
			{
				return "SEGU";
			}
		}

		public override void RegisterArea(AreaRegistrationContext context)
		{
			context.MapRoute(
				"SEGU_default",
				"SEGU/{controller}/{action}/{id}",
				new { action = "Index", id = UrlParameter.Optional },
				new[] { "SIGRE.Controllers.SEGU" }
			);
		}
	}
}
