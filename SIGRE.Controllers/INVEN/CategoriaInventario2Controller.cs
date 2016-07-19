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
	public class CategoriaInventario2Controller : Controller
    {
        private BDSIGRE db = new BDSIGRE();

		public ActionResult Index(int sesion = 1)
		{
			IQueryable<CategoriaInventario> categoriainventario;
			bool ignora = false;
			if (sesion == 0 || (Session["p_idcategoriainventariotipo"] == null && Session["p_nombre"] == null))
				ignora = true;
			if (ignora)
			{
				Session["p_idcategoriainventariotipo"] = null;
				Session["p_nombre"] = null;
				ViewBag.idcategoriainventariotipo = new SelectList(db.CategoriaInventarioTipo, "idcategoriainventariotipo", "descripcion");
				categoriainventario = db.CategoriaInventario.Include(c => c.categoriainventariotipo);
			}
			else
			{
				int idcategoriainventariotipo = (int)Session["p_idcategoriainventariotipo"];
				string nombre = (string)Session["p_nombre"];
				ViewBag.idcategoriainventariotipo = new SelectList(db.CategoriaInventarioTipo, "idcategoriainventariotipo", "descripcion", idcategoriainventariotipo);
				categoriainventario = db.CategoriaInventario.Where(
				s => (0 == idcategoriainventariotipo || s.idcategoriainventariotipo == idcategoriainventariotipo)
					&& ("" == nombre.Trim() || s.nombre.Contains(nombre.Trim()))
				).Include(c => c.categoriainventariotipo);
			}
			return View(categoriainventario.ToList());
        }

		[HttpPost]
		public ActionResult Index(int idcategoriainventariotipo = 0, string nombre = "")
		{
			Session["p_idcategoriainventariotipo"] = idcategoriainventariotipo;
			Session["p_nombre"] = nombre;
			ViewBag.idcategoriainventariotipo = new SelectList(db.CategoriaInventarioTipo, "idcategoriainventariotipo", "descripcion", idcategoriainventariotipo);
			List<CategoriaInventario> l = db.CategoriaInventario.Where(
				s => (0 == idcategoriainventariotipo || s.idcategoriainventariotipo == idcategoriainventariotipo)
					&& ("" == nombre.Trim() || s.nombre.Contains(nombre.Trim()))
				).Include(c => c.categoriainventariotipo).ToList();
			if (l.Count() == 0)
				TempData["alerta"] = "No se encontraron resultados para la búsqueda";
			return View(l);
		}

        public ActionResult Create()
        {
            ViewBag.idcategoriainventariotipo = new SelectList(db.CategoriaInventarioTipo, "idcategoriainventariotipo", "descripcion");
			CategoriaInventario categoriainventario = new CategoriaInventario();
			categoriainventario.utilizada = false;
			Session["lcategoriadetalle"] = new List<CategoriaDetalle>();
            return View(categoriainventario);
        }

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
			if (((List<CategoriaDetalle>)Session["lcategoriadetalle"]).Count() == 0)
				ModelState.AddModelError("utilizada", "Agregue al menos un detalle");
			else
			{
				bool identificador = false;
				foreach (CategoriaDetalle o in ((List<CategoriaDetalle>)Session["lcategoriadetalle"]))
					if (o.identificador)
					{
						identificador = true;
						break;
					}
				if (!identificador)
					ModelState.AddModelError("utilizada", "Agregue al menos un detalle identificador");
			}
            if (ModelState.IsValid)
            {
				db.CategoriaInventario.Add(categoriainventario);
				db.SaveChanges();
				foreach (CategoriaDetalle o in ((List<CategoriaDetalle>)Session["lcategoriadetalle"]))
				{
					db.CategoriaDetalle.Add(new CategoriaDetalle() {
						idcategoriainventario = categoriainventario.idcategoriainventario,
						idtipodato = o.idtipodato,
						idatributo = o.idatributo,
						idformato = o.idformato,
						nombre = o.nombre,
						identificador = o.identificador,
						obligatorio = o.obligatorio
					});
					db.SaveChanges();
				}
				TempData["alerta"] = "Categoría registrada con éxito";
				return RedirectToAction("Index");
            }

            ViewBag.idcategoriainventariotipo = new SelectList(db.CategoriaInventarioTipo, "idcategoriainventariotipo", "descripcion", categoriainventario.idcategoriainventariotipo);
            return View(categoriainventario);
        }

        public ActionResult Edit(int id = 0)
        {
            CategoriaInventario categoriainventario = db.CategoriaInventario.Find(id);
            if (categoriainventario == null)
            {
                return HttpNotFound();
            }
            ViewBag.idcategoriainventariotipo = new SelectList(db.CategoriaInventarioTipo, "idcategoriainventariotipo", "descripcion", categoriainventario.idcategoriainventariotipo);
			Session["lcategoriadetalle"] = db.CategoriaDetalle.Where(x => x.idcategoriainventario == id).ToList();
            return View(categoriainventario);
        }

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
			if (((List<CategoriaDetalle>)Session["lcategoriadetalle"]).Count() == 0)
				ModelState.AddModelError("utilizada", "Agregue al menos un detalle");
			else
			{
				bool identificador = false;
				foreach (CategoriaDetalle o in ((List<CategoriaDetalle>)Session["lcategoriadetalle"]))
					if (o.identificador)
					{
						identificador = true;
						break;
					}
				if (!identificador)
					ModelState.AddModelError("utilizada", "Agregue al menos un detalle identificador");
			}
            if (ModelState.IsValid)
			{
				db.Entry(categoriainventario).State = EntityState.Modified;
				db.SaveChanges();
				if (categoriainventario.utilizada)
				{
					foreach (CategoriaDetalle o in ((List<CategoriaDetalle>)Session["lcategoriadetalle"]))
					{
						if (o.idcategoriadetalle == 0)
						{
							db.CategoriaDetalle.Add(new CategoriaDetalle()
							{
								idcategoriainventario = categoriainventario.idcategoriainventario,
								idtipodato = o.idtipodato,
								idatributo = o.idatributo,
								idformato = o.idformato,
								nombre = o.nombre,
								identificador = o.identificador,
								obligatorio = o.obligatorio
							});
							db.SaveChanges();
						}
					}
				}
				else
				{
					List<CategoriaDetalle> lcategoriadetalle = db.CategoriaDetalle.Where(c => c.idcategoriainventario == categoriainventario.idcategoriainventario).ToList();
					foreach (CategoriaDetalle categoriadetalle in lcategoriadetalle)
					{
						db.CategoriaDetalle.Remove(categoriadetalle);
						db.SaveChanges();
					}
					foreach (CategoriaDetalle o in ((List<CategoriaDetalle>)Session["lcategoriadetalle"]))
					{
						db.CategoriaDetalle.Add(new CategoriaDetalle()
						{
							idcategoriainventario = categoriainventario.idcategoriainventario,
							idtipodato = o.idtipodato,
							idatributo = o.idatributo,
							idformato = o.idformato,
							nombre = o.nombre,
							identificador = o.identificador,
							obligatorio = o.obligatorio
						});
						db.SaveChanges();
					}
				}
				TempData["alerta"] = "Categoría actualizada con éxito";
				return RedirectToAction("Index");
            }
            ViewBag.idcategoriainventariotipo = new SelectList(db.CategoriaInventarioTipo, "idcategoriainventariotipo", "descripcion", categoriainventario.idcategoriainventariotipo);
            return View(categoriainventario);
        }

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

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

		public ActionResult IndexDetalle()
		{
			return PartialView((List<CategoriaDetalle>)Session["lcategoriadetalle"]);
		}

		public ActionResult CreateDetalle(int idcategoriadetalle, int idcategoriainventario, int posicion)
		{
			CategoriaDetalle detalle = posicion == 0 ? (new CategoriaDetalle() { idcategoriadetalle = idcategoriadetalle, idcategoriainventario = idcategoriainventario }) : ((List<CategoriaDetalle>)Session["lcategoriadetalle"])[posicion - 1];
			ViewBag.idtipodato = new SelectList(db.TipoDato, "idtipodato", "descripcion", detalle.idtipodato);
			ViewBag.idatributo = new SelectList(db.TipoDatoAtributo.Where(c => c.idtipodato == detalle.idtipodato), "idatributo", "muestraLabel", detalle.idatributo);
			ViewBag.idformato = new SelectList(db.TipoDatoFormato.Where(c => c.idtipodato == detalle.idtipodato && c.activo), "idformato", "descripcion", detalle.idformato);
			ViewBag.posicion = posicion.ToString();
			return PartialView(detalle);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult CreateDetalle(CategoriaDetalle detalle, FormCollection collection)
		{
			ViewBag.posicion = collection["posicion"];
			int i = 0;
			if (detalle.nombre != null)
			{
				detalle.nombre = detalle.nombre.Trim();
				foreach (CategoriaDetalle o in ((List<CategoriaDetalle>)Session["lcategoriadetalle"]))
				{
					i++;
					if (o.nombre.ToUpper() == detalle.nombre.ToUpper() && i != Convert.ToInt16(collection["posicion"]))
					{
						ModelState.AddModelError("nombre", "El nombre debe ser único para la categoría");
						break;
					}
				}
			}
			if (detalle.identificador && !detalle.obligatorio)
				ModelState.AddModelError("obligatorio", "El identificador debe ser obligatorio");
			if (detalle.identificador && ((List<CategoriaDetalle>)Session["lcategoriadetalle"]).Count() > 0)
			{
				i = 0;
				foreach (CategoriaDetalle o in ((List<CategoriaDetalle>)Session["lcategoriadetalle"]))
				{
					i++;
					if (o.identificador && i != Convert.ToInt16(collection["posicion"]))
					{
						ModelState.AddModelError("identificador", "Ya agregó un detalle identificador");
						break;
					}
				}
			}
			if (ModelState.IsValid)
			{
				ViewBag.OK = "1";
				detalle.categoriainventario = detalle.idcategoriainventario == 0 ? (new CategoriaInventario() { idcategoriainventario = 0, utilizada = false }) : db.CategoriaInventario.Find(detalle.idcategoriainventario);
				detalle.tipodatoformato = db.TipoDatoFormato.Find(detalle.idtipodato, detalle.idformato);
				if (detalle.idatributo != null && detalle.idatributo != 0)
					detalle.tipodatoatributo = db.TipoDatoAtributo.Find(detalle.idatributo);
				if (collection["posicion"] == "0")
					((List<CategoriaDetalle>)Session["lcategoriadetalle"]).Add(detalle);
				else
					((List<CategoriaDetalle>)Session["lcategoriadetalle"])[Convert.ToInt32(collection["posicion"]) - 1] = detalle;
			}
			ViewBag.idtipodato = new SelectList(db.TipoDato, "idtipodato", "descripcion", detalle.idtipodato);
			ViewBag.idatributo = new SelectList(db.TipoDatoAtributo.Where(c => c.idtipodato == detalle.idtipodato), "idatributo", "muestraLabel", detalle.idatributo);
			ViewBag.idformato = new SelectList(db.TipoDatoFormato.Where(c => c.idtipodato == detalle.idtipodato && c.activo), "idformato", "descripcion", detalle.idformato);
			return PartialView(detalle);
		}

		public ActionResult DeleteDetalle(int idcategoriadetalle, int idcategoriainventario, int posicion)
		{
			((List<CategoriaDetalle>)Session["lcategoriadetalle"]).RemoveAt(posicion - 1);
			return RedirectToAction("IndexDetalle");
		}

		public ActionResult IndexParametro()
		{
			List<CategoriaInventario> l = db.CategoriaInventario.Include(x => x.categoriainventariotipo).Include(x => x.lcategoriareporte).ToList();
			ViewBag.idcategoriainventariotipo = new SelectList(db.CategoriaInventarioTipo, "idcategoriainventariotipo", "descripcion");
			return View(l);
		}

		[HttpPost]
		public ActionResult IndexParametro(int idcategoriainventariotipo = 0, string nombre = "")
		{
			List<CategoriaInventario> l = db.CategoriaInventario.Where(
				x => (0 == idcategoriainventariotipo || x.idcategoriainventariotipo == idcategoriainventariotipo)
				&& ("" == nombre.Trim() || x.nombre.Contains(nombre.Trim()))).Include(x => x.categoriainventariotipo).Include(x => x.lcategoriareporte).ToList();
			ViewBag.idcategoriainventariotipo = new SelectList(db.CategoriaInventarioTipo, "idcategoriainventariotipo", "descripcion", idcategoriainventariotipo);
			return View(l);
		}

		public JsonResult AtributoList(int id)
		{
			return Json(new SelectList(db.TipoDatoAtributo.Where(c => c.idtipodato == id), "idatributo", "muestraLabel"), JsonRequestBehavior.AllowGet);
		}

		public JsonResult FormatoList(int id)
		{
			return Json(new SelectList(db.TipoDatoFormato.Where(c => c.idtipodato == id && c.activo), "idformato", "descripcion"), JsonRequestBehavior.AllowGet);
		}
    }
}