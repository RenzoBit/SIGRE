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
	public class CategoriaDetalleController : Controller
    {
		private BDSIGRE db = new BDSIGRE();
		private static CategoriaInventario categoriainventario = null;

        //
        // GET: /INVEN/CategoriaDetalle/

		public ActionResult Index(int id = 0)
		{
			categoriainventario = db.CategoriaInventario.Find(id);
			ViewBag.categoriainventario = categoriainventario;
			var categoriadetalle = db.CategoriaDetalle.Where(c => c.idcategoriainventario.Equals(id)).Include(c => c.categoriainventario).Include(c => c.tipodato).Include(c => c.tipodatoatributo).Include(c => c.tipodatoformato);
            return View(categoriadetalle.ToList());
        }

        //
        // GET: /INVEN/CategoriaDetalle/Details/5

        public ActionResult Details(int id = 0)
        {
            CategoriaDetalle categoriadetalle = db.CategoriaDetalle.Find(id);
            if (categoriadetalle == null)
            {
                return HttpNotFound();
            }
            return View(categoriadetalle);
        }

        //
        // GET: /INVEN/CategoriaDetalle/Create

        public ActionResult Create()
        {
			ViewBag.categoriainventario = categoriainventario;
            ViewBag.idtipodato = new SelectList(db.TipoDato, "idtipodato", "descripcion");
			ViewBag.idtipodatoatributo = new SelectList(db.TipoDatoAtributo.Where(c => c.idtipodato.Equals(0)), "idtipodatoatributo", "descripcion");
			ViewBag.idtipodatoformato = new SelectList(db.TipoDatoFormato.Where(c => c.idtipodato.Equals(0)), "idtipodatoformato", "descripcion");
			CategoriaDetalle categoriadetalle = new CategoriaDetalle();
			categoriadetalle.idcategoriainventario = categoriainventario.idcategoriainventario;
			return View(categoriadetalle);
        }

        //
        // POST: /INVEN/CategoriaDetalle/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CategoriaDetalle categoriadetalle)
		{
			//REVISAR
			return null;
			/*
			ViewBag.categoriainventario = categoriainventario;
			int i = db.CategoriaDetalle.Where(c => c.idcategoriainventario.Equals(categoriadetalle.idcategoriainventario)).Count();

			if (i == 0 && !categoriadetalle.identificador)
				ModelState.AddModelError("identificador", "El primer detalle registrado debe ser el identificador");
			if (ModelState.IsValid)
			{
				if (categoriadetalle.identificador)
					categoriadetalle.obligatorio = true;
				db.CategoriaDetalle.Add(categoriadetalle);
				db.SaveChanges();
				TempData["alerta"] = "Detalle registrado con éxito";
				return RedirectToAction("Index", new { id = categoriainventario.idcategoriainventario });
			}
            ViewBag.idtipodato = new SelectList(db.TipoDato, "idtipodato", "descripcion", categoriadetalle.idtipodato);
			ViewBag.idtipodatoatributo = new SelectList(db.TipoDatoAtributo.Where(c => c.idtipodato.Equals(categoriadetalle.idtipodato)), "idtipodatoatributo", "descripcion", categoriadetalle.idtipodatoatributo);
			ViewBag.idtipodatoformato = new SelectList(db.TipoDatoFormato.Where(c => c.idtipodato.Equals(categoriadetalle.idtipodato)), "idtipodatoformato", "descripcion", categoriadetalle.idtipodatoformato);
            return View(categoriadetalle);
			*/
        }

        //
        // GET: /INVEN/CategoriaDetalle/Edit/5

        public ActionResult Edit(int id = 0)
        {
			//REVISAR
			return null;
			/*
			ViewBag.categoriainventario = categoriainventario;
            CategoriaDetalle categoriadetalle = db.CategoriaDetalle.Find(id);
            if (categoriadetalle == null)
            {
                return HttpNotFound();
            }
            ViewBag.idtipodato = new SelectList(db.TipoDato, "idtipodato", "descripcion", categoriadetalle.idtipodato);
			ViewBag.idtipodatoatributo = new SelectList(db.TipoDatoAtributo.Where(c => c.idtipodato.Equals(categoriadetalle.idtipodato)), "idtipodatoatributo", "descripcion", categoriadetalle.idtipodatoatributo);
			ViewBag.idtipodatoformato = new SelectList(db.TipoDatoFormato.Where(c => c.idtipodato.Equals(categoriadetalle.idtipodato)), "idtipodatoformato", "descripcion", categoriadetalle.idtipodatoformato);
            return View(categoriadetalle);
			*/
        }

        //
        // POST: /INVEN/CategoriaDetalle/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CategoriaDetalle categoriadetalle)
        {
			//REVISAR
			return null;
			/*
			ViewBag.categoriainventario = categoriainventario;
            if (ModelState.IsValid)
			{
				if (categoriadetalle.identificador)
					categoriadetalle.obligatorio = true;
                db.Entry(categoriadetalle).State = EntityState.Modified;
				db.SaveChanges();
				TempData["alerta"] = "Detalle actualizado con éxito";
				return RedirectToAction("Index", new { id = categoriainventario.idcategoriainventario });
            }
            ViewBag.idtipodato = new SelectList(db.TipoDato, "idtipodato", "descripcion", categoriadetalle.idtipodato);
			ViewBag.idtipodatoatributo = new SelectList(db.TipoDatoAtributo.Where(c => c.idtipodato.Equals(categoriadetalle.idtipodato)), "idtipodatoatributo", "descripcion", categoriadetalle.idtipodatoatributo);
			ViewBag.idtipodatoformato = new SelectList(db.TipoDatoFormato.Where(c => c.idtipodato.Equals(categoriadetalle.idtipodato)), "idtipodatoformato", "descripcion", categoriadetalle.idtipodatoformato);
            return View(categoriadetalle);
			*/
        }

        //
        // GET: /INVEN/CategoriaDetalle/Delete/5

        public ActionResult Delete(int id = 0)
        {
			CategoriaDetalle categoriadetalle = db.CategoriaDetalle.Find(id);
			db.CategoriaDetalle.Remove(categoriadetalle);
			db.SaveChanges();
			return RedirectToAction("Index", new { id = categoriainventario.idcategoriainventario });
			/*
            CategoriaDetalle categoriadetalle = db.CategoriaDetalle.Find(id);
            if (categoriadetalle == null)
            {
                return HttpNotFound();
            }
            return View(categoriadetalle);
			*/
        }

        //
        // POST: /INVEN/CategoriaDetalle/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CategoriaDetalle categoriadetalle = db.CategoriaDetalle.Find(id);
            db.CategoriaDetalle.Remove(categoriadetalle);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

		public JsonResult AtributoList(int id)
		{
			return Json(new SelectList(db.TipoDatoAtributo.Where(c => c.idtipodato.Equals(id)).Include(c => c.tipodatoreferencia), "idtipodatoatributo", "descripcion"), JsonRequestBehavior.AllowGet);
		}

		public JsonResult FormatoList(int id)
		{
			return Json(new SelectList(db.TipoDatoFormato.Where(c => c.idtipodato.Equals(id)), "idtipodatoformato", "descripcion"), JsonRequestBehavior.AllowGet);
		}

    }
}