using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIGRE.Models;

namespace SIGRE.Controllers.ADMIN
{
	[Authorize]
	public class CentroCostoController : Controller
	{
		private BDSIGRE db = new BDSIGRE();

		public ActionResult Index()
		{
			List<SelectListItem> ltipo = new List<SelectListItem>();
			ltipo.Add(new SelectListItem { Text = "Unidad de negocio", Value = "U" });
			ltipo.Add(new SelectListItem { Text = "Centro de costo", Value = "C" });
			ViewBag.tipo = new SelectList(ltipo, "Value", "Text");
			var centrocosto = db.CentroCosto.Include(c => c.unidadnegocio).Include(c => c.propietario);
			return View(centrocosto.ToList());
		}

		[HttpPost]
		public ActionResult Index(string tipo = "", string codigo = "", string nombre = "")
		{
			List<SelectListItem> ltipo = new List<SelectListItem>();
			ltipo.Add(new SelectListItem { Text = "Unidad de negocio", Value = "U" });
			ltipo.Add(new SelectListItem { Text = "Centro de costo", Value = "C" });
			ViewBag.tipo = new SelectList(ltipo, "Value", "Text", tipo);
			var centrocosto = db.CentroCosto.Where(
				s => (tipo == "" || (tipo == "U" && s.centrocosto_idcentrocosto == null) || (tipo == "C" && s.centrocosto_idcentrocosto != null))
				&& (codigo == "" || s.codigo.Contains(codigo.Trim()))
				&& (nombre == "" || s.nombre.Contains(nombre.Trim()))).Include(c => c.unidadnegocio).Include(c => c.propietario);
			List<CentroCosto> l = centrocosto.ToList();
			return View(l);
		}

		public ActionResult Create()
		{
			ViewBag.centrocosto_idcentrocosto = new SelectList(db.CentroCosto.Where(x => x.centrocosto_idcentrocosto == null && !x.desactivado).OrderBy(x => x.nombre), "idcentrocosto", "nombre");
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(CentroCosto centrocosto)
		{
			if (!String.IsNullOrEmpty(centrocosto.codigo) && db.CentroCosto.Where(x => x.codigo == centrocosto.codigo).Count() > 0)
				ModelState.AddModelError("codigo", "El código del centro de costo debe ser único");
			if (!String.IsNullOrEmpty(centrocosto.nombre) && db.CentroCosto.Where(x => x.nombre == centrocosto.nombre).Count() > 0)
				ModelState.AddModelError("nombre", "El nombre del centro de costo debe ser único");
			if (ModelState.IsValid)
			{
				centrocosto.codigo = centrocosto.codigo.ToUpper();
				db.CentroCosto.Add(centrocosto);
				db.SaveChanges();
				return RedirectToAction("Index");
			}
			ViewBag.centrocosto_idcentrocosto = new SelectList(db.CentroCosto.Where(x => x.centrocosto_idcentrocosto == null && !x.desactivado).OrderBy(x => x.nombre), "idcentrocosto", "nombre", centrocosto.centrocosto_idcentrocosto);
			return View(centrocosto);
		}

		public ActionResult Edit(int id = 0)
		{
			CentroCosto centrocosto = db.CentroCosto.Find(id);
			centrocosto.colaborador_nombre = centrocosto.propietario.nombre;
			if (centrocosto == null)
			{
				return HttpNotFound();
			}
			ViewBag.centrocosto_idcentrocosto = new SelectList(db.CentroCosto.Where(x => x.centrocosto_idcentrocosto == null && x.idcentrocosto != id).OrderBy(x => x.nombre), "idcentrocosto", "nombre", centrocosto.centrocosto_idcentrocosto);
			return View(centrocosto);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(CentroCosto centrocosto)
		{
			if (!String.IsNullOrEmpty(centrocosto.codigo) && db.CentroCosto.Where(x => x.codigo == centrocosto.codigo && x.idcentrocosto != centrocosto.idcentrocosto).Count() > 0)
				ModelState.AddModelError("codigo", "El código del centro de costo debe ser único");
			if (!String.IsNullOrEmpty(centrocosto.nombre) && db.CentroCosto.Where(x => x.nombre == centrocosto.nombre && x.idcentrocosto != centrocosto.idcentrocosto).Count() > 0)
				ModelState.AddModelError("nombre", "El nombre del centro de costo debe ser único");
			if (ModelState.IsValid)
			{
				centrocosto.codigo = centrocosto.codigo.ToUpper();
				db.Entry(centrocosto).State = EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("Index");
			}
			ViewBag.centrocosto_idcentrocosto = new SelectList(db.CentroCosto.Where(x => x.centrocosto_idcentrocosto == null && x.idcentrocosto != centrocosto.idcentrocosto).OrderBy(x => x.nombre), "idcentrocosto", "nombre", centrocosto.centrocosto_idcentrocosto);
			return View(centrocosto);
		}

		public ActionResult Delete(int id = 0)
		{
			CentroCosto centrocosto = db.CentroCosto.Find(id >= 0 ? id : -id);
			if (id >= 0)
			{
				db.CentroCosto.Remove(centrocosto);
				db.SaveChanges();
				TempData["alerta"] = "Centro de costo eliminado satisfactoriamente";
			}
			else
			{
				centrocosto.desactivado = !centrocosto.desactivado;
				db.Entry(centrocosto).State = EntityState.Modified;
				db.SaveChanges();
				if (centrocosto.desactivado)
					TempData["alerta"] = "Centro de costo desactivado satisfactoriamente";
				else
					TempData["alerta"] = "Centro de costo activado satisfactoriamente";
			}
			return RedirectToAction("Index");
		}

		protected override void Dispose(bool disposing)
		{
			db.Dispose();
			base.Dispose(disposing);
		}

		[HttpPost]
		public JsonResult PropietarioList(string term)
		{
			var data = db.Colaborador.Select(x => new
			{
				idcolaborador = x.idcolaborador,
				codigo = x.codigo,
				nombre = x.nombre,
				desactivado = x.desactivado,
				aprobado = x.aprobado,
				idcolaboradortipo = x.idcolaboradortipo
			}).Where(x => ((x.codigo.Contains(term) || x.nombre.Contains(term)) && !x.desactivado && x.aprobado == true && x.idcolaboradortipo == ColaboradorTipo.INTERNO));
			return Json(data, JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		public ContentResult Propietario(string term)
		{
			string mensaje = "0|";
			if (term.Length == 7 && (term[0] == 'M' || term[0] == 'X'))
			{
				Colaborador c = db.Colaborador.Where(x => x.codigo == term && !x.desactivado && x.aprobado == true && x.idcolaboradortipo == ColaboradorTipo.INTERNO).FirstOrDefault();
				if (c != null)
					mensaje = c.idcolaborador + "|" + c.nombre + "|" + c.codigo;
			}
			else
			{
				Colaborador c = db.Colaborador.Where(x => x.nombre == term && !x.desactivado && x.aprobado == true && x.idcolaboradortipo == ColaboradorTipo.INTERNO).FirstOrDefault();
				if (c != null)
					mensaje = c.idcolaborador + "|" + c.nombre + "|" + c.codigo;
			}
			return Content(mensaje, "text");
		}
	}
}