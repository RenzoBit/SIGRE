using System.Web.Mvc;

namespace SIGRE.WebApplication.Areas.PERFIL
{
	public class PERFILAreaRegistration : AreaRegistration
	{
		public override string AreaName
		{
			get
			{
				return "PERFIL";
			}
		}

		public override void RegisterArea(AreaRegistrationContext context)
		{
			context.MapRoute(
				"PERFIL_default",
				"PERFIL/{controller}/{action}/{id}",
				new { action = "Index", id = UrlParameter.Optional },
				new[] { "SIGRE.Controllers.PERFIL" }
			);
		}
	}
}
