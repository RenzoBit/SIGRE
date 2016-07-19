using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIGRE.Models;
using System.Text;

namespace SIGRE.Controllers.INVEN
{
	[Authorize]
	public class ItemInventarioController : Controller
    {
        private BDSIGRE db = new BDSIGRE();

        //
        // GET: /INVEN/ItemInventario/

		public ActionResult Index()
        {
			List<SelectListItem> ltipooperacion = new List<SelectListItem>();
			ltipooperacion.Add(new SelectListItem { Text = "Disponible", Value = "D" });
			ltipooperacion.Add(new SelectListItem { Text = "Prestado", Value = "P" });
			ltipooperacion.Add(new SelectListItem { Text = "Asignado", Value = "A" });
			ltipooperacion.Add(new SelectListItem { Text = "De baja", Value = "B" });

			ViewBag.idcategoriainventariotipo = new SelectList(db.CategoriaInventarioTipo, "idcategoriainventariotipo", "descripcion");
			ViewBag.idcategoriainventario = new SelectList(db.CategoriaInventario.Where(c => c.idcategoriainventariotipo == 0), "idcategoriainventario", "nombre");
			ViewBag.idcategoriadetalle = new SelectList(db.CategoriaDetalle.Where(c => c.idcategoriainventario == 0), "idcategoriadetalle", "nombre");
			ViewBag.tipooperacion = new SelectList(ltipooperacion, "Value", "Text");
			/*
			List<ItemInventario> l = new List<ItemInventario>();
			return View(l);
			*/
			var iteminventario = db.ItemInventario.Include(i => i.categoriainventario).Include(i => i.colaborador);
			return View(iteminventario.ToList());
        }

		[HttpPost]
        public ActionResult Index(int idcategoriainventariotipo = 0, int idcategoriainventario = 0, string tipooperacion = "", int idcategoriadetalle = 0, string valorbusqueda = "")
        {
			List<SelectListItem> ltipooperacion = new List<SelectListItem>();
			ltipooperacion.Add(new SelectListItem { Text = "Disponible", Value = "D" });
			ltipooperacion.Add(new SelectListItem { Text = "Prestado", Value = "P" });
			ltipooperacion.Add(new SelectListItem { Text = "Asignado", Value = "A" });
			ltipooperacion.Add(new SelectListItem { Text = "De baja", Value = "B" });

			ViewBag.idcategoriainventariotipo = new SelectList(db.CategoriaInventarioTipo, "idcategoriainventariotipo", "descripcion", idcategoriainventariotipo);
			ViewBag.idcategoriainventario = new SelectList(db.CategoriaInventario.Where(c => c.idcategoriainventariotipo.Equals(idcategoriainventariotipo)), "idcategoriainventario", "nombre", idcategoriainventario);
			ViewBag.idcategoriadetalle = new SelectList(db.CategoriaDetalle.Where(c => c.idcategoriainventario.Equals(idcategoriainventario)), "idcategoriadetalle", "nombre", idcategoriadetalle);
			ViewBag.tipooperacion = new SelectList(ltipooperacion, "Value", "Text", tipooperacion);
            var iteminventario = db.ItemInventario.Where(
				s => (idcategoriainventariotipo.Equals(0) || s.categoriainventario.idcategoriainventariotipo.Equals(idcategoriainventariotipo))
					&& (idcategoriainventario.Equals(0) || s.idcategoriainventario.Equals(idcategoriainventario))
					&& (tipooperacion.Equals("") || s.tipooperacion.Equals(tipooperacion))
					&& (idcategoriadetalle.Equals(0) || s.liteminventariodetalle.Any(t => t.idcategoriadetalle.Equals(idcategoriadetalle)))
					&& (valorbusqueda.Equals("") || s.liteminventariodetalle.Any(t => t.valorbusqueda.Contains(valorbusqueda.Trim())))
					//&& (valor.Equals("") || (s.categoriainventario.lcategoriadetalle.Any(t => t.nombre.Equals(nombre))))
				).Include(i => i.categoriainventario).Include(i => i.colaborador);
			List<ItemInventario> l = iteminventario.ToList();
			if (l.Count() == 0)
				TempData["alerta"] = "No se encontraron resultados para la búsqueda";
            return View(l);
        }

        //
        // GET: /INVEN/ItemInventario/Details/5

        public ActionResult Details(int id = 0)
        {
            ItemInventario iteminventario = db.ItemInventario.Find(id);
            if (iteminventario == null)
            {
                return HttpNotFound();
            }
            return View(iteminventario);
        }

        //
        // GET: /INVEN/ItemInventario/Create

        public ActionResult Create()
        {
			ViewBag.idcategoriainventario = new SelectList(db.CategoriaInventario.Where(c => !c.desactivado && c.lcategoriadetalle.Count() > 0), "idcategoriainventario", "nombre");
			ItemInventario iteminventario = new ItemInventario();
			iteminventario.tipooperacion = "D";
			return View(iteminventario);
        }

        //
        // POST: /INVEN/ItemInventario/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ItemInventario iteminventario)
		{
			iteminventario.descripcion = iteminventario.descripcion.Replace("  ", " ").Trim();
			int i = db.ItemInventario.Where(c => c.descripcion.Equals(iteminventario.descripcion)).Count();
			if (i > 0)
			{
				ModelState.AddModelError("descripcion", "La descripción del ítem debe ser única");
			}
            if (ModelState.IsValid)
            {
				if (iteminventario.idcolaborador == null)
					iteminventario.tipooperacion = "D";
				else
					if (iteminventario.prestamo)
						iteminventario.tipooperacion = "P";
					else
						iteminventario.tipooperacion = "A";
				db.ItemInventario.Add(iteminventario);
				db.SaveChanges();

				CategoriaInventario categoriainventario = db.CategoriaInventario.Find(iteminventario.idcategoriainventario);
				categoriainventario.utilizada = true;
				db.Entry(categoriainventario).State = EntityState.Modified;
				db.SaveChanges();

				List<CategoriaDetalle> lcategoriadetalle = db.CategoriaDetalle.Where(c => c.idcategoriainventario.Equals(iteminventario.idcategoriainventario)).ToList();
				ItemInventarioDetalle iteminventariodetalle = new ItemInventarioDetalle();
				iteminventariodetalle.iditeminventario = iteminventario.iditeminventario;

				foreach (CategoriaDetalle categoriadetalle in lcategoriadetalle)
				{
					iteminventariodetalle.idcategoriadetalle = categoriadetalle.idcategoriadetalle;
					db.ItemInventarioDetalle.Add(iteminventariodetalle);
					db.SaveChanges();
				}
				TempData["alerta"] = "Ítem de inventario registrado con éxito";
				return RedirectToAction("Index");
            }
			ViewBag.idcategoriainventario = new SelectList(db.CategoriaInventario.Where(c => !c.desactivado && c.lcategoriadetalle.Count() > 0), "idcategoriainventario", "nombre", iteminventario.idcategoriainventario);
            return View(iteminventario);
        }

        //
        // GET: /INVEN/ItemInventario/Edit/5

        public ActionResult Edit(int id = 0)
        {
			ItemInventario iteminventario = db.ItemInventario.Find(id);
			iteminventario.colaborador_nombre = (iteminventario.colaborador == null ? "" : iteminventario.colaborador.nombre);
			if (iteminventario.tipooperacion == "P")
				iteminventario.prestamo = true;
			ViewBag.idcategoriainventario = new SelectList(db.CategoriaInventario, "idcategoriainventario", "nombre", iteminventario.idcategoriainventario);
            return View(iteminventario);
        }

        //
        // POST: /INVEN/ItemInventario/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ItemInventario iteminventario)
        {
			iteminventario.descripcion = iteminventario.descripcion.Replace("  ", " ").Trim();
			int i = db.ItemInventario.Where(c => c.descripcion.Equals(iteminventario.descripcion) && c.iditeminventario != iteminventario.iditeminventario).Count();
			if (i > 0)
			{
				ModelState.AddModelError("descripcion", "La descripción del ítem debe ser única");
			}
            if (ModelState.IsValid)
			{
				if (iteminventario.idcolaborador == null)
					iteminventario.tipooperacion = "D";
				else
					if (iteminventario.prestamo)
						iteminventario.tipooperacion = "P";
					else
						iteminventario.tipooperacion = "A";
				db.Entry(iteminventario).State = EntityState.Modified;
				db.SaveChanges();

				TempData["alerta"] = "Ítem de inventario actualizado con éxito";
				return RedirectToAction("Index");
            }
			ViewBag.idcategoriainventario = new SelectList(db.CategoriaInventario, "idcategoriainventario", "nombre", iteminventario.idcategoriainventario);
            return View(iteminventario);
        }

        //
        // GET: /INVEN/ItemInventario/Delete/5

        public ActionResult Delete(int id = 0)
        {
			ItemInventario iteminventario = db.ItemInventario.Find(id >= 0 ? id : -id);
			if (id >= 0)
			{
				List<ItemInventarioDetalle> l = db.ItemInventarioDetalle.Where(x => x.iditeminventario == id).ToList();
				foreach (ItemInventarioDetalle o in l)
				{
					db.ItemInventarioDetalle.Remove(o);
					db.SaveChanges();
				}
				db.ItemInventario.Remove(iteminventario);
				db.SaveChanges();
				TempData["alerta"] = "Ítem de inventario eliminado satisfactoriamente";
			}
			else
			{
				iteminventario.tipooperacion = "B";
				db.Entry(iteminventario).State = EntityState.Modified;
				db.SaveChanges();
				TempData["alerta"] = "Ítem de inventario dado de baja satisfactoriamente";
			}
			return RedirectToAction("Index");
        }

        //
        // POST: /INVEN/ItemInventario/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ItemInventario iteminventario = db.ItemInventario.Find(id);
            db.ItemInventario.Remove(iteminventario);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

		[HttpGet]
		public ContentResult mensajea(int iditeminventario, int idcategoriainventario, int idcolaborador, bool prestamo)
		{
			string mensaje = "";
			bool multiple = db.CategoriaInventario.Find(idcategoriainventario).multiple;
			int ia = db.ItemInventario.Where(c => !(c.iditeminventario == iditeminventario) && c.idcategoriainventario == idcategoriainventario && c.idcolaborador == idcolaborador && c.tipooperacion.Equals("A")).Count();
			int ip = db.ItemInventario.Where(c => !(c.iditeminventario == iditeminventario) && c.idcategoriainventario == idcategoriainventario && c.idcolaborador == idcolaborador && c.tipooperacion.Equals("P")).Count();
			if (multiple)
				if (ia + ip == 0)
					mensaje = "1|";
				else
					if (ip > 0)
						mensaje = "0|El colaborador ya tiene en préstamo un recurso de esta categoría";
					else
						if (prestamo)
							mensaje = "0|El colaborador ya tiene asignado un recurso de esta categoría";
						else
							mensaje = "2|El colaborador ya tiene un recurso asignado de este tipo, ¿desea continuar?";
			else
				if (ia == 0 && ip == 0)
					mensaje = "1|";
				else
					if (ip > 0)
						mensaje = "0|El colaborador ya tiene en préstamo un recurso de esta categoría que no permite asignación múltiple";
					else
						mensaje = "0|El colaborador ya tiene asignado un recurso de esta categoría que no permite asignación múltiple";
			return Content(mensaje, "text");
		}

		[HttpGet]
		public ContentResult grillea(int iditeminventario, int idcategoriainventario, int idcolaborador)
		{
			if (iditeminventario != 0)
			{
				ItemInventario iteminventario = db.ItemInventario.Find(iditeminventario);
				iteminventario.idcolaborador = null;
				iteminventario.tipooperacion = "D";
				db.Entry(iteminventario).State = EntityState.Modified;
				db.SaveChanges();
			}
			List<ItemInventario> l = db.ItemInventario.Where(c => c.idcategoriainventario == idcategoriainventario && c.idcolaborador == idcolaborador && !c.tipooperacion.Equals("B")).Include(i => i.categoriainventario).Include(i => i.colaborador).ToList();
			StringBuilder html = new StringBuilder("<table class=\"rvtabla\"><tr><th>Descripción</th><th>Categoría</th><th>Estado</th><th>Colaborador</th><th>Opciones</th></tr>");
			foreach (ItemInventario o in l)
			{
				html.Append("<tr><td>" + o.descripcion + "</td>");
				html.Append("<td>" + o.categoriainventario.nombre + "</td>");
				html.Append("<td>" + o.muestraOperacion + "</td>");
				html.Append("<td>" + o.colaborador.nombre + "</td>");
				html.Append("<td><a href=\"/INVEN/ItemInventario/Edit/" + o.iditeminventario + "\">Editar</a> | <a href=\"#\" onclick=\"libera('" + o.iditeminventario + "');\">Liberar</a></td></tr>");
			}
			html.Append("</table>");
			return Content(html.ToString(), "text");
		}

		[HttpGet]
		public JsonResult CategoriaInventarioList(int id)
		{
			return Json(new SelectList(db.CategoriaInventario.Where(c => c.idcategoriainventariotipo.Equals(id)), "idcategoriainventario", "nombre"), JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		public JsonResult CategoriaDetalleList(int id)
		{
			return Json(new SelectList(db.CategoriaDetalle.Where(c => c.idcategoriainventario.Equals(id)), "idcategoriadetalle", "nombre"), JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public JsonResult ColaboradorList(string term)
		{
			var data = db.Colaborador.Select(x => new
			{
				idcolaborador = x.idcolaborador,
				codigo = x.codigo,
				nombre = x.nombre,
				desactivado = x.desactivado,
				aprobado = x.aprobado
			}).Where(x => ((x.codigo.Contains(term) || x.nombre.Contains(term)) && !x.desactivado  && x.aprobado == true));
			return Json(data, JsonRequestBehavior.AllowGet);
		}

    }
}