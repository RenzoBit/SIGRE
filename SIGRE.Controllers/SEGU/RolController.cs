using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIGRE.Models;

namespace SIGRE.Controllers.SEGU
{
	[Authorize]
	public class RolController : Controller
    {
        private BDSIGRE db = new BDSIGRE();

        public ActionResult Index()
        {
            return View(db.Rol.Where(x => !x.dinamico).OrderBy(x => x.nombre).ToList());
        }

		public ActionResult Create()
        {
			Rol rol = new Rol();
			rol.lopcion = db.Opcion.Where(x => x.idsuperior != null && !x.dinamico).OrderBy(x => x.opcionsuperior.link).ThenBy(x => x.link).ToList();
            return View(rol);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Rol rol)
        {
			if (!string.IsNullOrEmpty(rol.nombre))
			{
				rol.nombre = rol.nombre.Replace("  ", " ").Trim();
				if (0 < db.Rol.Where(c => c.nombre.Equals(rol.nombre)).Count())
					ModelState.AddModelError("nombre", "El nombre del rol debe ser único");
			}
			bool hay = false;
			foreach (Opcion opcion in rol.lopcion)
				if (opcion.acceso)
				{
					hay = true;
					break;
				}
			if (!hay)
				ModelState.AddModelError("idrol", "Debe seleccionar al menos una opción");
            if (ModelState.IsValid)
            {
                db.Rol.Add(rol);
                db.SaveChanges();
				int? idsuperior = 0;
				foreach (Opcion opcion in rol.lopcion)
					if (opcion.acceso)
					{
						if (opcion.idsuperior != idsuperior)
						{
							db.RolOpcion.Add(new RolOpcion() { idrol = rol.idrol, idopcion = (int)opcion.idsuperior, idsuperior = null });
							db.SaveChanges();
						}
						db.RolOpcion.Add(new RolOpcion() { idrol = rol.idrol, idopcion = opcion.idopcion, idsuperior = opcion.idsuperior });
						db.SaveChanges();
						idsuperior = opcion.idsuperior;
					}
				TempData["alerta"] = "Rol registrado satisfactoriamente";
                return RedirectToAction("Index");
            }
            return View(rol);
        }

        public ActionResult Edit(int id = 0)
        {
			Rol rol = db.Rol.Find(id);
			List<Opcion> lopcion = db.Opcion.Where(x => x.idsuperior != null && !x.dinamico).OrderBy(x => x.opcionsuperior.link).ThenBy(x => x.link).ToList();
			foreach (Opcion opcion in lopcion)
				foreach (RolOpcion rolopcion in rol.lrolopcion)
					if (opcion.idopcion == rolopcion.idopcion)
					{
						opcion.acceso = true;
						break;
					}
			rol.lopcion = lopcion;
            return View(rol);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Rol rol)
        {
			if (!string.IsNullOrEmpty(rol.nombre))
			{
				rol.nombre = rol.nombre.Replace("  ", " ").Trim();
				if (0 < db.Rol.Where(c => c.nombre.Equals(rol.nombre) && c.idrol != rol.idrol).Count())
					ModelState.AddModelError("nombre", "El nombre del rol debe ser único");
			}
			bool hay = false;
			foreach (Opcion opcion in rol.lopcion)
				if (opcion.acceso)
				{
					hay = true;
					break;
				}
			if (!hay)
				ModelState.AddModelError("idrol", "Debe seleccionar al menos una opción");
            if (ModelState.IsValid)
            {
				List<RolOpcion> lrolopcion = db.RolOpcion.Where(x => x.idrol == rol.idrol).ToList();
				foreach (RolOpcion rolopcion in lrolopcion)
				{
					db.RolOpcion.Remove(rolopcion);
					db.SaveChanges();
				}
                db.Entry(rol).State = EntityState.Modified;
                db.SaveChanges();
				int? idsuperior = 0;
				foreach (Opcion opcion in rol.lopcion)
					if (opcion.acceso)
					{
						if (opcion.idsuperior != idsuperior)
						{
							db.RolOpcion.Add(new RolOpcion() { idrol = rol.idrol, idopcion = (int)opcion.idsuperior, idsuperior = null });
							db.SaveChanges();
						}
						db.RolOpcion.Add(new RolOpcion() { idrol = rol.idrol, idopcion = opcion.idopcion, idsuperior = opcion.idsuperior });
						db.SaveChanges();
						idsuperior = opcion.idsuperior;
					}
				TempData["alerta"] = "Rol actualizado satisfactoriamente";
                return RedirectToAction("Index");
            }
            return View(rol);
        }

        public ActionResult Delete(int id = 0)
        {
			List<RolOpcion> lrolopcion = db.RolOpcion.Where(x => x.idrol == id).ToList();
			foreach (RolOpcion rolopcion in lrolopcion)
			{
				db.RolOpcion.Remove(rolopcion);
				db.SaveChanges();
			}
			Rol rol = db.Rol.Find(id);
			db.Rol.Remove(rol);
			db.SaveChanges();
			TempData["alerta"] = "Rol eliminado satisfactoriamente";
			return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}