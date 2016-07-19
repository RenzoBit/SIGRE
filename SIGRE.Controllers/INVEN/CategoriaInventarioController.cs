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
	public class CategoriaInventarioController : Controller
    {
        private BDSIGRE db = new BDSIGRE();

        //
        // GET: /INVEN/CategoriaInventario/

        public ActionResult Index()
        {
			ViewBag.idcategoriainventariotipo = new SelectList(db.CategoriaInventarioTipo, "idcategoriainventariotipo", "descripcion");
			var categoriainventario = db.CategoriaInventario.Include(c => c.categoriainventariotipo);
            return View(categoriainventario.ToList());
        }

		[HttpPost]
		public ActionResult Index(int idcategoriainventariotipo = 0, string nombre = "")
		{
			/*
			if (idcategoriainventariotipo == 0 && Session["idcategoriainventariotipo"] != null)
				idcategoriainventariotipo = (int) Session["idcategoriainventariotipo"];
			else
				Session["idcategoriainventariotipo"] = idcategoriainventariotipo;
			if (nombre == "" && Session["nombre"] != null)
				nombre = (string) Session["nombre"];
			else
				Session["nombre"] = nombre;
			*/
			ViewBag.idcategoriainventariotipo = new SelectList(db.CategoriaInventarioTipo, "idcategoriainventariotipo", "descripcion", idcategoriainventariotipo);
			var o = db.CategoriaInventario.Where(
				s => (idcategoriainventariotipo == 0 || s.idcategoriainventariotipo == idcategoriainventariotipo)
					&& (nombre.Trim().Equals("") || s.nombre.Contains(nombre.Trim()))
				).Include(c => c.categoriainventariotipo);
			List<CategoriaInventario> l = o.ToList();
			if (l.Count() == 0)
				TempData["alerta"] = "No se encontraron resultados para la búsqueda";
			return View(l);
		}

        //
        // GET: /INVEN/CategoriaInventario/Details/5

        public ActionResult Details(int id = 0)
        {
            CategoriaInventario categoriainventario = db.CategoriaInventario.Find(id);
            if (categoriainventario == null)
            {
                return HttpNotFound();
            }
            return View(categoriainventario);
        }

        //
        // GET: /INVEN/CategoriaInventario/Create

        public ActionResult Create()
        {
            ViewBag.idcategoriainventariotipo = new SelectList(db.CategoriaInventarioTipo, "idcategoriainventariotipo", "descripcion");
			CategoriaInventario categoriainventario = new CategoriaInventario();
			categoriainventario.utilizada = false;
            return View(categoriainventario);
        }

        //
        // POST: /INVEN/CategoriaInventario/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CategoriaInventario categoriainventario)
		{
			if (categoriainventario.nombre != null)
			{
				categoriainventario.nombre = categoriainventario.nombre.Replace("  ", " ").Trim();
				if (0 < db.CategoriaInventario.Where(c => c.nombre.Equals(categoriainventario.nombre)).Count())
					ModelState.AddModelError("nombre", "El nombre de la categoría debe ser único");
			}
            if (ModelState.IsValid)
            {
				db.CategoriaInventario.Add(categoriainventario);
				db.SaveChanges();
				TempData["alerta"] = "Categoría registrada con éxito";
				return RedirectToAction("Index");
            }

            ViewBag.idcategoriainventariotipo = new SelectList(db.CategoriaInventarioTipo, "idcategoriainventariotipo", "descripcion", categoriainventario.idcategoriainventariotipo);
            return View(categoriainventario);
        }

        //
        // GET: /INVEN/CategoriaInventario/Edit/5

        public ActionResult Edit(int id = 0)
        {
            CategoriaInventario categoriainventario = db.CategoriaInventario.Find(id);
            if (categoriainventario == null)
            {
                return HttpNotFound();
            }
            ViewBag.idcategoriainventariotipo = new SelectList(db.CategoriaInventarioTipo, "idcategoriainventariotipo", "descripcion", categoriainventario.idcategoriainventariotipo);
            return View(categoriainventario);
        }

        //
        // POST: /INVEN/CategoriaInventario/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CategoriaInventario categoriainventario)
		{
			if (categoriainventario.nombre != null)
			{
				categoriainventario.nombre = categoriainventario.nombre.Replace("  ", " ").Trim();
				if (0 < db.CategoriaInventario.Where(c => c.nombre.Equals(categoriainventario.nombre) && c.idcategoriainventario != categoriainventario.idcategoriainventario).Count())
					ModelState.AddModelError("nombre", "El nombre de la categoría debe ser único");
			}
            if (ModelState.IsValid)
			{
				db.Entry(categoriainventario).State = EntityState.Modified;
				db.SaveChanges();
				TempData["alerta"] = "Categoría actualizada con éxito";
				return RedirectToAction("Index");
            }
            ViewBag.idcategoriainventariotipo = new SelectList(db.CategoriaInventarioTipo, "idcategoriainventariotipo", "descripcion", categoriainventario.idcategoriainventariotipo);
            return View(categoriainventario);
        }

        //
        // GET: /INVEN/CategoriaInventario/Delete/5

        public ActionResult Delete(int id = 0)
        {
			CategoriaInventario categoriainventario = db.CategoriaInventario.Find(id < 0 ? -id : id);
			if (id < 0)
			{
				categoriainventario.desactivado = !categoriainventario.desactivado;
				db.Entry(categoriainventario).State = EntityState.Modified;
				TempData["alerta"] = "Categoría " + (categoriainventario.desactivado ? "desactivada" : "activada") + " con éxito";
			}
			else
			{
				List<CategoriaDetalle> lcategoriadetalle = db.CategoriaDetalle.Where(c => c.idcategoriainventario == id).ToList();
				foreach (CategoriaDetalle categoriadetalle in lcategoriadetalle)
				{
					db.CategoriaDetalle.Remove(categoriadetalle);
					db.SaveChanges();
				}
				db.CategoriaInventario.Remove(categoriainventario);
				TempData["alerta"] = "Categoría eliminada con éxito";
			}
			db.SaveChanges();
            return RedirectToAction("Index");
        }

        //
        // POST: /INVEN/CategoriaInventario/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CategoriaInventario categoriainventario = db.CategoriaInventario.Find(id);
            db.CategoriaInventario.Remove(categoriainventario);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}