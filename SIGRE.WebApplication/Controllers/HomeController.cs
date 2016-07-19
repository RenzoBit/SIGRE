using SIGRE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGRE.WebApplication.Controllers
{
	public class HomeController : Controller
	{
		private BDSIGRE db = new BDSIGRE();

		public ActionResult Index()
		{
			if (Request.IsAuthenticated && Session["lrolopcion"] == null)
			{
				Usuario usuario = db.Usuario.Where(x => x.username == User.Identity.Name).FirstOrDefault();
				Session["lrolopcion"] = db.RolOpcion.Where(x => x.idrol == usuario.idrol && x.idsuperior == null).ToList();
			}
			ViewBag.Message = "Living Innovation";
			return View();
		}

		public ActionResult About()
		{
			ViewBag.Message = "Your app description page.";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}
	}
}
