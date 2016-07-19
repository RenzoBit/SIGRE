using System.Web.Mvc;

namespace SIGRE.WebApplication.Areas.INVEN
{
	public class INVENAreaRegistration : AreaRegistration
	{
		public override string AreaName
		{
			get
			{
				return "INVEN";
			}
		}

		public override void RegisterArea(AreaRegistrationContext context)
		{
			context.MapRoute(
				"INVEN_default",
				"INVEN/{controller}/{action}/{id}",
				new { action = "Index", id = UrlParameter.Optional },
				new[] { "SIGRE.Controllers.INVEN" }
			);
		}
	}
}
