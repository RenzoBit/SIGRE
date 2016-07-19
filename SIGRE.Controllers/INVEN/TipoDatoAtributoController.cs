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
	public class TipoDatoAtributoController : Controller
    {
        private BDSIGRE db = new BDSIGRE();

        public ActionResult Index()
        {
            return View(db.TipoDatoAtributo.ToList());
        }

        public ActionResult Create()
        {
			ViewBag.idtipodato = new SelectList(db.TipoDato, "idtipodato", "descripcion");
			ViewBag.idoperador = new SelectList(db.TipoDatoOperador.Where(c => c.idtipodato == 0), "idoperador", "descripcion");
			ViewBag.idreferencia = new SelectList(db.TipoDatoReferencia.Where(c => c.idtipodato == 0), "idreferencia", "descripcion");
			ViewBag.ltipodatoatributo = db.TipoDatoAtributo.OrderBy(c => c.idtipodato).ThenBy(c => c.codigo).ToList();
			TipoDatoAtributo tipodatoatributo = new TipoDatoAtributo() { idtipodato = 0 };
			return View(tipodatoatributo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TipoDatoAtributo tipodatoatributo)
        {
			if (tipodatoatributo.codigo != null)
			{
				tipodatoatributo.codigo = tipodatoatributo.codigo.Replace("  ", " ").Trim();
				if (0 < db.TipoDatoAtributo.Where(c => c.codigo == tipodatoatributo.codigo).Count())
					ModelState.AddModelError("codigo", "El código debe ser único");
			}
			if (tipodatoatributo.descripcion != null)
			{
				tipodatoatributo.descripcion = tipodatoatributo.descripcion.Replace("  ", " ").Trim();
				if (0 < db.TipoDatoAtributo.Where(c => c.descripcion == tipodatoatributo.descripcion).Count())
					ModelState.AddModelError("descripcion", "La descripción debe ser única");
			}
			if (tipodatoatributo.idoperador > 0 && tipodatoatributo.idoperador != TipoDatoOperador.DIFERENTE && (tipodatoatributo.idtipodato == TipoDato.TEXTO || tipodatoatributo.idtipodato == TipoDato.ALFANUMERICO) && (tipodatoatributo.valorcadena == null || tipodatoatributo.valorcadena.Trim() == ""))
				ModelState.AddModelError("valorcadena", "Ingrese un valor de referencia");
			if (tipodatoatributo.idoperador > 0 && (tipodatoatributo.idtipodato == TipoDato.FECHA || tipodatoatributo.idtipodato == TipoDato.MONTO || tipodatoatributo.idtipodato == TipoDato.NUMERO) && (tipodatoatributo.idreferencia == null || tipodatoatributo.idreferencia == 0))
				ModelState.AddModelError("idreferencia", "Seleccione un valor de referencia");
            if (ModelState.IsValid)
			{
				if (tipodatoatributo.idreferencia == TipoDatoReferencia.CERO || tipodatoatributo.idreferencia == TipoDatoReferencia.UNO)
					tipodatoatributo.valorentero = db.TipoDatoReferencia.Find(tipodatoatributo.idtipodato, tipodatoatributo.idreferencia).valorentero;
				if (tipodatoatributo.idoperador > 0)
					tipodatoatributo.etiqueta = (db.TipoDatoOperador.Find(tipodatoatributo.idtipodato, tipodatoatributo.idoperador).descripcion + " " + ((tipodatoatributo.idtipodato == TipoDato.TEXTO || tipodatoatributo.idtipodato == TipoDato.ALFANUMERICO) ? ((tipodatoatributo.valorcadena == null || tipodatoatributo.valorcadena.Trim() == "") ? "[vacío]" : tipodatoatributo.valorcadena) : db.TipoDatoReferencia.Find(tipodatoatributo.idtipodato, tipodatoatributo.idreferencia).descripcion));
                db.TipoDatoAtributo.Add(tipodatoatributo);
                db.SaveChanges();
				TempData["alerta"] = "Atributo de tipo de dato registrado con éxito";
				return RedirectToAction("Create");
            }
			ViewBag.idtipodato = new SelectList(db.TipoDato, "idtipodato", "descripcion", tipodatoatributo.idtipodato);
			ViewBag.idoperador = new SelectList(db.TipoDatoOperador.Where(c => c.idtipodato == tipodatoatributo.idtipodato), "idoperador", "descripcion", tipodatoatributo.idoperador);
			ViewBag.idreferencia = new SelectList(db.TipoDatoReferencia.Where(c => c.idtipodato == tipodatoatributo.idtipodato), "idreferencia", "descripcion", tipodatoatributo.idreferencia);
			ViewBag.ltipodatoatributo = db.TipoDatoAtributo.OrderBy(c => c.idtipodato).ThenBy(c => c.codigo).ToList();
            return View(tipodatoatributo);
        }

        public ActionResult Edit(int id = 0)
        {
            TipoDatoAtributo tipodatoatributo = db.TipoDatoAtributo.Find(id);
            if (tipodatoatributo == null)
            {
                return HttpNotFound();
			}
			ViewBag.idtipodato = new SelectList(db.TipoDato, "idtipodato", "descripcion", tipodatoatributo.idtipodato);
			ViewBag.idoperador = new SelectList(db.TipoDatoOperador.Where(c => c.idtipodato == tipodatoatributo.idtipodato), "idoperador", "descripcion", tipodatoatributo.idoperador);
			ViewBag.idreferencia = new SelectList(db.TipoDatoReferencia.Where(c => c.idtipodato == tipodatoatributo.idtipodato), "idreferencia", "descripcion", tipodatoatributo.idreferencia);
			ViewBag.ltipodatoatributo = db.TipoDatoAtributo.OrderBy(c => c.idtipodato).ThenBy(c => c.codigo).ToList();
            return View(tipodatoatributo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TipoDatoAtributo tipodatoatributo)
		{
			if (tipodatoatributo.codigo != null)
			{
				tipodatoatributo.codigo = tipodatoatributo.codigo.Replace("  ", " ").Trim();
				if (0 < db.TipoDatoAtributo.Where(c => c.codigo == tipodatoatributo.codigo && c.idatributo != tipodatoatributo.idatributo).Count())
					ModelState.AddModelError("codigo", "El código debe ser único");
			}
			if (tipodatoatributo.descripcion != null)
			{
				tipodatoatributo.descripcion = tipodatoatributo.descripcion.Replace("  ", " ").Trim();
				if (0 < db.TipoDatoAtributo.Where(c => c.descripcion == tipodatoatributo.descripcion && c.idatributo != tipodatoatributo.idatributo).Count())
					ModelState.AddModelError("descripcion", "La descripción debe ser única");
			}
			if (tipodatoatributo.idoperador > 0 && tipodatoatributo.idoperador != TipoDatoOperador.DIFERENTE && (tipodatoatributo.idtipodato == TipoDato.TEXTO || tipodatoatributo.idtipodato == TipoDato.ALFANUMERICO) && (tipodatoatributo.valorcadena == null || tipodatoatributo.valorcadena.Trim() == ""))
				ModelState.AddModelError("valorcadena", "Ingrese un valor de referencia");
			if (tipodatoatributo.idoperador > 0 && (tipodatoatributo.idtipodato == TipoDato.FECHA || tipodatoatributo.idtipodato == TipoDato.MONTO || tipodatoatributo.idtipodato == TipoDato.NUMERO) && (tipodatoatributo.idreferencia == null || tipodatoatributo.idreferencia == 0))
				ModelState.AddModelError("idreferencia", "Seleccione un valor de referencia");
			if (ModelState.IsValid)
			{
				if (tipodatoatributo.idreferencia == TipoDatoReferencia.CERO || tipodatoatributo.idreferencia == TipoDatoReferencia.UNO)
					tipodatoatributo.valorentero = db.TipoDatoReferencia.Find(tipodatoatributo.idtipodato, tipodatoatributo.idreferencia).valorentero;
				if (tipodatoatributo.idoperador > 0)
					tipodatoatributo.etiqueta = (db.TipoDatoOperador.Find(tipodatoatributo.idtipodato, tipodatoatributo.idoperador).descripcion + " " + ((tipodatoatributo.idtipodato == TipoDato.TEXTO || tipodatoatributo.idtipodato == TipoDato.ALFANUMERICO) ? ((tipodatoatributo.valorcadena == null || tipodatoatributo.valorcadena.Trim() == "") ? "[vacío]" : tipodatoatributo.valorcadena) : db.TipoDatoReferencia.Find(tipodatoatributo.idtipodato, tipodatoatributo.idreferencia).descripcion));
				db.Entry(tipodatoatributo).State = EntityState.Modified;
				db.SaveChanges();
				TempData["alerta"] = "Atributo de tipo de dato actualizado con éxito";
				return RedirectToAction("Create");
			}
			ViewBag.idtipodato = new SelectList(db.TipoDato, "idtipodato", "descripcion", tipodatoatributo.idtipodato);
			ViewBag.idoperador = new SelectList(db.TipoDatoOperador.Where(c => c.idtipodato == tipodatoatributo.idtipodato), "idoperador", "descripcion", tipodatoatributo.idoperador);
			ViewBag.idreferencia = new SelectList(db.TipoDatoReferencia.Where(c => c.idtipodato == tipodatoatributo.idtipodato), "idreferencia", "descripcion", tipodatoatributo.idreferencia);
			ViewBag.ltipodatoatributo = db.TipoDatoAtributo.OrderBy(c => c.idtipodato).ThenBy(c => c.codigo).ToList();
			return View(tipodatoatributo);
        }

        public ActionResult Delete(int id = 0)
        {
			TipoDatoAtributo tipodatoatributo = db.TipoDatoAtributo.Find(id);
			db.TipoDatoAtributo.Remove(tipodatoatributo);
			db.SaveChanges();
			TempData["alerta"] = "Atributo de tipo de dato eliminado con éxito";
			return RedirectToAction("Create");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

		[HttpGet]
		public JsonResult OperadorList(int id)
		{
			return Json(new SelectList(db.TipoDatoOperador.Where(c => c.idtipodato == id), "idoperador", "descripcion"), JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		public JsonResult ReferenciaList(int id)
		{
			return Json(new SelectList(db.TipoDatoReferencia.Where(c => c.idtipodato == id), "idreferencia", "descripcion"), JsonRequestBehavior.AllowGet);
		}
    }
}