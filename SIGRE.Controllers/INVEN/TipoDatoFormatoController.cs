using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIGRE.Models;

namespace SIGRE.Controllers.INVEN
{
	[Authorize]
	public class TipoDatoFormatoController : Controller
    {
        private BDSIGRE db = new BDSIGRE();

        public ActionResult Index()
        {
			ViewBag.idtipodato = new SelectList(db.TipoDato, "idtipodato", "descripcion");
			ViewBag.idformato = new SelectList(db.TipoDatoFormato.Where(c => c.idtipodato == 0), "idformato", "formato");
			var tipodatoformato = db.TipoDatoFormato.Where(t => t.activo).Include(t => t.tipodato).OrderBy(t => t.idtipodato).ThenBy(t => t.idformato);
            return View(tipodatoformato.ToList());
        }
		public ActionResult Create()
		{
			ViewBag.idtipodato = new SelectList(db.TipoDato, "idtipodato", "descripcion");
			ViewBag.idformato = new SelectList(db.TipoDatoFormato.Where(c => c.idtipodato == 0), "idformato", "formato");
			ViewBag.ltipodatoformato = db.TipoDatoFormato.Where(t => t.activo).Include(t => t.tipodato).OrderBy(t => t.idtipodato).ThenBy(t => t.idformato).ToList();
			return View();
		}

		//
		// POST: /INVEN/TipoDatoFormato2/Create

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(TipoDatoFormato tipodatoformato)
		{
			if (tipodatoformato.idtipodato != 0 && tipodatoformato.idformato != 0)
			{
				TipoDatoFormato o = db.TipoDatoFormato.Find(tipodatoformato.idtipodato, tipodatoformato.idformato);
				if (o.activo)
					ModelState.AddModelError("idtipodato", "El formato de tipo de dato ya existe");
				else
				{
					o.activo = true;
					db.Entry(o).State = EntityState.Modified;
					db.SaveChanges();
					TempData["alerta"] = "Formato de tipo de dato registrado con éxito";
					return RedirectToAction("Create");
				}
			}
			ViewBag.idtipodato = new SelectList(db.TipoDato, "idtipodato", "descripcion", tipodatoformato.idtipodato);
			ViewBag.idformato = new SelectList(db.TipoDatoFormato.Where(c => c.idtipodato == tipodatoformato.idtipodato), "idformato", "formato", tipodatoformato.idformato);
			ViewBag.ltipodatoformato = db.TipoDatoFormato.Where(t => t.activo).Include(t => t.tipodato).OrderBy(t => t.idtipodato).ThenBy(t => t.idformato).ToList();
			return View(tipodatoformato);
		}

		public ActionResult Delete(int idtipodato = 0, int idformato = 0)
        {
			TipoDatoFormato o = db.TipoDatoFormato.Find(idtipodato, idformato);
			o.activo = false;
			db.Entry(o).State = EntityState.Modified;
			db.SaveChanges();
			TempData["alerta"] = "Formato de tipo de dato eliminado con éxito";
            return RedirectToAction("Create");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

		[HttpGet]
		public JsonResult FormatoList(int id)
		{
			return Json(new SelectList(db.TipoDatoFormato.Where(c => c.idtipodato == id), "idformato", "formato"), JsonRequestBehavior.AllowGet);
		}
    }
}