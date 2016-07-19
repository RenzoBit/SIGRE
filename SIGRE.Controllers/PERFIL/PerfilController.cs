using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIGRE.Models;
using System.Net.Mail;
using System.Net;
using System.Threading;

namespace SIGRE.Controllers.PERFIL
{
	[Authorize]
	public class PerfilController : Controller
    {
        private BDSIGRE db = new BDSIGRE();

		public ActionResult Index(int sesion = 1)
		{
			IQueryable<Perfil> perfil;
			bool ignora = false;
			if (sesion == 0 || (Session["p_idcentrocosto"] == null && Session["p_nombre"] == null))
				ignora = true;
			if (ignora)
			{
				Session["p_idcentrocosto"] = null;
				Session["p_nombre"] = null;
				ViewBag.idcentrocosto = new SelectList(db.CentroCosto.Where(x => x.centrocosto_idcentrocosto == null), "idcentrocosto", "nombre");
				perfil = db.Perfil.Include(p => p.propietario).Include(p => p.unidadnegocio).OrderBy(p => p.unidadnegocio.codigo).ThenBy(p => p.nombre);
			}
			else
			{
				int idcentrocosto = (int)Session["p_idcentrocosto"];
				string nombre = (string)Session["p_nombre"];
				ViewBag.idcentrocosto = new SelectList(db.CentroCosto.Where(x => x.centrocosto_idcentrocosto == null), "idcentrocosto", "nombre", idcentrocosto);
				perfil = db.Perfil.Where(
				s => (idcentrocosto == 0 || s.idcentrocosto == idcentrocosto)
					&& (nombre.Trim().Equals("") || s.nombre.Contains(nombre.Trim()))
				).Include(p => p.propietario).Include(p => p.unidadnegocio).OrderBy(p => p.unidadnegocio.codigo).ThenBy(p => p.nombre); ;
			}
            return View(perfil.ToList());
        }

		[HttpPost]
		public ActionResult Index(int idcentrocosto = 0, string nombre = "")
		{
			Session["p_idcentrocosto"] = idcentrocosto;
			Session["p_nombre"] = nombre;
			ViewBag.idcentrocosto = new SelectList(db.CentroCosto.Where(x => x.centrocosto_idcentrocosto == null), "idcentrocosto", "nombre", idcentrocosto);
			var o = db.Perfil.Where(
				s => (idcentrocosto == 0 || s.idcentrocosto == idcentrocosto)
					&& (nombre.Trim().Equals("") || s.nombre.Contains(nombre.Trim()))
				).Include(p => p.propietario).Include(p => p.unidadnegocio).OrderBy(p => p.unidadnegocio.codigo).ThenBy(p => p.nombre);
			List<Perfil> l = o.ToList();
			if (l.Count() == 0)
				TempData["alerta"] = "No se encontraron resultados para la búsqueda";
			return View(l);
		}

		public ActionResult IndexAprobar()
		{
			Colaborador colaborador = db.Colaborador.Where(x => x.codigo == User.Identity.Name).FirstOrDefault();
			List<Perfil> l = db.Perfil.Where(x => x.idcolaborador == colaborador.idcolaborador && !x.desactivado && x.aprobado == null).Include(p => p.propietario).Include(p => p.unidadnegocio).ToList();
			return View(l);
		}

		public ActionResult EditAprobar(int id = 0)
		{
			Perfil perfil = db.Perfil.Find(id);
			perfil.propietario_nombre = perfil.propietario.nombre;
			List<PerfilRecurso> l = db.PerfilRecurso.Where(x => x.idperfil == perfil.idperfil).ToList();
			string[] seleccion = new string[l.Count()];
			for (int i = 0; i < l.Count(); i++)
				seleccion[i] = l[i].idrecurso + "";
			ViewBag.idcentrocosto = new SelectList(db.CentroCosto.Where(x => x.centrocosto_idcentrocosto == null), "idcentrocosto", "nombre", perfil.idcentrocosto);
			ViewBag.idrecurso = new MultiSelectList(db.Recurso.Where(x => x.aprobado == true), "idrecurso", "muestraNombrePrecio", seleccion);
			return View(perfil);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[HandleError]
		public ActionResult EditAprobar(Perfil perfil)
		{
			db.Entry(perfil).State = EntityState.Modified;
			db.SaveChanges();
			Colaborador rolando = db.Colaborador.Where(x => x.usuario.idrol == Rol.CONSULTOR && x.aprobado == true && !x.desactivado).FirstOrDefault();
			string correo = rolando == null ? "" : rolando.correo;
			string titulo = "Petición de aprobación";
			string mensaje = "La petición de aprobación del perfil " + perfil.nombre + " fue " + (perfil.aprobado == true ? "aceptada" : "rechazada");
			if (Correo.sepuede)
				Correo.enviar2(correo, titulo, mensaje);
			TempData["alerta"] = "Petición de aprobación guardada con éxito";
			return RedirectToAction("IndexAprobar");
		}

		[HandleError]
		public ActionResult DeleteAprobar(int id = 0)
		{
			Perfil perfil = db.Perfil.Find(id >= 0 ? id : -id);
			perfil.aprobado = (id >= 0);
			db.Entry(perfil).State = EntityState.Modified;
			db.SaveChanges();
			Colaborador rolando = db.Colaborador.Where(x => x.usuario.idrol == Rol.CONSULTOR && x.aprobado == true && !x.desactivado).FirstOrDefault();
			string correo = rolando == null ? "" : rolando.correo;
			string titulo = "Petición de aprobación";
			string mensaje = "La petición de aprobación del perfil " + perfil.nombre + " fue " + (perfil.aprobado == true ? "aceptada" : "rechazada");
			if (Correo.sepuede)
				Correo.enviar2(correo, titulo, mensaje);
			TempData["alerta"] = "Petición de aprobación guardada con éxito";
			return RedirectToAction("IndexAprobar");
		}

        public ActionResult Create()
        {
			ViewBag.idcentrocosto = new SelectList(db.CentroCosto.Where(x => x.centrocosto_idcentrocosto == null && !x.desactivado), "idcentrocosto", "nombre");
			ViewBag.idrecurso = new MultiSelectList(db.Recurso.Where(x => !x.desactivado && x.aprobado == true), "idrecurso", "muestraNombrePrecio");
			Perfil perfil = new Perfil();
			perfil.costo = 0.00M;
			return View(perfil);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
		[HandleError]
        public ActionResult Create(Perfil perfil, FormCollection collection)
        {
			if (perfil.nombre != null && perfil.nombre != "")
			{
				perfil.nombre = perfil.nombre.Replace("  ", " ").Trim();
				int j = db.Perfil.Where(c => c.nombre.Equals(perfil.nombre) && c.idcentrocosto == perfil.idcentrocosto).Count();
				if (j > 0)
					ModelState.AddModelError("nombre", "El nombre del perfil debe ser único por unidad de negocio");
			}
			string[] seleccion = null;
			if (collection["idrecurso[]"] != null)
				seleccion = collection["idrecurso[]"].Split(',');
			if (seleccion == null)
				ModelState.AddModelError("aprobado", "Seleccione al menos un recurso para el perfil");
            if (ModelState.IsValid)
            {
				if (Correo.sepuede || collection["forzar"] == "1")
				{
					db.Perfil.Add(perfil);
					db.SaveChanges();
					PerfilRecurso p = null;
					if (seleccion != null)
						foreach (string s in seleccion)
						{
							p = new PerfilRecurso() { idperfil = perfil.idperfil, idrecurso = Convert.ToInt32(s), montocalculado = db.Recurso.Find(Convert.ToInt32(s)).costo };
							db.PerfilRecurso.Add(p);
							db.SaveChanges();
						}
					TempData["alerta"] = collection["forzar"] == "1" ? "Se guardo el perfil sin envío de solicitud de aprobación" : "Solicitud de aprobación enviada con éxito";
					string correo = db.Colaborador.Find(perfil.idcolaborador).correo;
					string titulo = "Solicitud de aprobación";
					string mensaje = "Solicitud de aprobación de nuevo perfil<br/><br/><a href=\""
						+ (Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/PERFIL/Perfil/Edit2/" + perfil.idperfil)
						+ "\" target=\"_blank\">IR</a>";
					if (Correo.sepuede)
						Correo.enviar2(correo, titulo, mensaje);
					return RedirectToAction("Index");
				}
				else
					TempData["confirma"] = "No hay Internet para enviar la solicitud de aprobación, ¿desea continuar de todos modos?|$('#forzar').val('1'); $('#form1').submit();";
            }
			ViewBag.idcentrocosto = new SelectList(db.CentroCosto.Where(x => x.centrocosto_idcentrocosto == null && !x.desactivado), "idcentrocosto", "nombre");
			ViewBag.idrecurso = new MultiSelectList(db.Recurso.Where(x => !x.desactivado && x.aprobado == true), "idrecurso", "muestraNombrePrecio", seleccion);
			return View(perfil);
        }

        public ActionResult Edit(int id = 0)
        {
            Perfil perfil = db.Perfil.Find(id);
            if (perfil == null)
            {
                return HttpNotFound();
            }
			perfil.propietario_nombre = perfil.propietario.nombre;
			List<PerfilRecurso> l = db.PerfilRecurso.Where(x => x.idperfil == perfil.idperfil).ToList();
			string[] seleccion = new string[l.Count()];
			for(int i = 0; i < l.Count(); i++)
				seleccion[i] = l[i].idrecurso + "";
			ViewBag.idcentrocosto = new SelectList(db.CentroCosto.Where(x => x.centrocosto_idcentrocosto == null), "idcentrocosto", "nombre", perfil.idcentrocosto);
			ViewBag.idrecurso = new MultiSelectList(db.Recurso.Where(x => x.aprobado == true), "idrecurso", "muestraNombrePrecio", seleccion);
			return View(perfil);
        }

		public ActionResult Edit2(int id = 0)
		{
			Perfil perfil = db.Perfil.Find(id);
			if (perfil == null)
			{
				TempData["alerta"] = "El perfil fue eliminado";
				return RedirectToAction("Index", "Home", new { Area = "" });
			}
			else if (perfil.aprobado == true)
			{
				TempData["alerta"] = "El perfil ya fue aprobado";
				return RedirectToAction("Index", "Home", new { Area = "" });
			}
			else if (perfil.aprobado == false)
			{
				TempData["alerta"] = "El perfil ya fue rechazado";
				return RedirectToAction("Index", "Home", new { Area = "" });
			}
			perfil.propietario_nombre = perfil.propietario.nombre;
			List<PerfilRecurso> l = db.PerfilRecurso.Where(x => x.idperfil == perfil.idperfil).ToList();
			string[] seleccion = new string[l.Count()];
			for (int i = 0; i < l.Count(); i++)
				seleccion[i] = l[i].idrecurso + "";
			ViewBag.idcentrocosto = new SelectList(db.CentroCosto.Where(x => x.centrocosto_idcentrocosto == null), "idcentrocosto", "nombre", perfil.idcentrocosto);
			ViewBag.idrecurso = new MultiSelectList(db.Recurso.Where(x => x.aprobado == true), "idrecurso", "muestraNombrePrecio", seleccion);
			return View(perfil);
		}

		public ActionResult Edit3(int id = 0)
		{
			Perfil perfil = db.Perfil.Find(id);
			if (perfil == null)
			{
				return HttpNotFound();
			}
			perfil.propietario_nombre = perfil.propietario.nombre;
			List<PerfilRecurso> l = db.PerfilRecurso.Where(x => x.idperfil == perfil.idperfil).ToList();
			string[] seleccion = new string[l.Count()];
			for (int i = 0; i < l.Count(); i++)
				seleccion[i] = l[i].idrecurso + "";
			ViewBag.idcentrocosto = new SelectList(db.CentroCosto.Where(x => x.centrocosto_idcentrocosto == null), "idcentrocosto", "nombre", perfil.idcentrocosto);
			ViewBag.idrecurso = new MultiSelectList(db.Recurso.Where(x => x.aprobado == true), "idrecurso", "muestraNombrePrecio", seleccion);
			return View(perfil);
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
		[HandleError]
        public ActionResult Edit(Perfil perfil, FormCollection collection)
		{
			if (perfil.nombre != null && perfil.nombre != "")
			{
				perfil.nombre = perfil.nombre.Replace("  ", " ").Trim();
				int j = db.Perfil.Where(c => c.nombre.Equals(perfil.nombre) && c.idcentrocosto == perfil.idcentrocosto && c.idperfil != perfil.idperfil).Count();
				if (j > 0)
					ModelState.AddModelError("nombre", "El nombre del perfil debe ser único por unidad de negocio");
			}
			string[] seleccion = null;
			if (collection["idrecurso[]"] != null)
				seleccion = collection["idrecurso[]"].Split(',');
			if (seleccion == null)
				ModelState.AddModelError("aprobado", "Seleccione al menos un recurso para el perfil");
            if (ModelState.IsValid)
            {
				if (Correo.sepuede || collection["forzar"] == "1")
				{
					perfil.aprobado = null;
					db.Entry(perfil).State = EntityState.Modified;
					db.SaveChanges();
					List<PerfilRecurso> l = db.PerfilRecurso.Where(x => x.idperfil == perfil.idperfil).ToList();
					for (int i = 0; i < l.Count(); i++)
					{
						db.PerfilRecurso.Remove(l[i]);
						db.SaveChanges();
					}
					PerfilRecurso p = null;
					if (seleccion != null)
						foreach (string s in seleccion)
						{
							p = new PerfilRecurso() { idperfil = perfil.idperfil, idrecurso = Convert.ToInt32(s), montocalculado = db.Recurso.Find(Convert.ToInt32(s)).costo };
							db.PerfilRecurso.Add(p);
							db.SaveChanges();
						}
					//if (perfil.aprobado == null)
					//{
					TempData["alerta"] = collection["forzar"] == "1" ? "Se guardo el perfil sin envío de solicitud de aprobación" : "Solicitud de aprobación enviada con éxito";
					string correo = db.Colaborador.Find(perfil.idcolaborador).correo;
					string titulo = "Solicitud de aprobación";
					string mensaje = "Solicitud de aprobación de nuevo perfil<br/><br/><a href=\""
						+ (Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/PERFIL/Perfil/Edit2/" + perfil.idperfil)
						+ "\" target=\"_blank\">IR</a>";
					if (Correo.sepuede)
						Correo.enviar2(correo, titulo, mensaje);
					//}
					//else
					//	TempData["alerta"] = "Perfil actualizado con éxito";
					return RedirectToAction("Index");
				}
				else
					TempData["confirma"] = "No hay Internet para enviar la solicitud de aprobación, ¿desea continuar de todos modos?|$('#forzar').val('1'); $('#form1').submit();";
            }
			ViewBag.idcentrocosto = new SelectList(db.CentroCosto.Where(x => x.centrocosto_idcentrocosto == null), "idcentrocosto", "nombre", perfil.idcentrocosto);
			ViewBag.idrecurso = new MultiSelectList(db.Recurso.Where(x => x.aprobado == true), "idrecurso", "muestraNombrePrecio", seleccion);
			return View(perfil);
        }

		[HttpPost]
		[ValidateAntiForgeryToken]
		[HandleError]
        public ActionResult Edit2(Perfil perfil)
		{
			/*
			if (perfil.aprobado == false)
				perfil.desactivado = true;
			*/
			db.Entry(perfil).State = EntityState.Modified;
			db.SaveChanges();
			Colaborador rolando = db.Colaborador.Where(x => x.usuario.idrol == Rol.CONSULTOR && x.aprobado == true && !x.desactivado).FirstOrDefault();
			string correo = rolando == null ? "" : rolando.correo;
			string titulo = "Petición de aprobación";
			string mensaje = "La petición de aprobación del perfil " + perfil.nombre + " fue " + (perfil.aprobado == true ? "aceptada" : "rechazada");
			if (Correo.sepuede)
				Correo.enviar2(correo, titulo, mensaje);
			TempData["alerta"] = "Petición de aprobación guardada con éxito";
			return RedirectToAction("Index", "Home", new { Area = "" });
		}

        [HandleError]
        public ActionResult Delete(int id = 0)
        {
			Perfil perfil = db.Perfil.Find(id > 0 ? id : -id);
			string mensaje = null;
			if (id > 0)
			{
				List<PerfilRecurso> l1 = db.PerfilRecurso.Where(c => c.idperfil == id).ToList();
				List<ColaboradorPerfil> l2 = db.ColaboradorPerfil.Where(c => c.idperfil == id).ToList();
				foreach (PerfilRecurso o1 in l1)
				{
					db.PerfilRecurso.Remove(o1);
					db.SaveChanges();
				}
				foreach (ColaboradorPerfil o2 in l2)
				{
					db.ColaboradorPerfil.Remove(o2);
					db.SaveChanges();
				}
				mensaje = "El perfil " + perfil.nombre + " que usted aprobó fue eliminado.";
				TempData["alerta"] = "El perfil fue eliminado satisfactoriamente";
				db.Perfil.Remove(perfil);
			}
			else
			{
				perfil.desactivado = !perfil.desactivado;
				if (perfil.desactivado)
				{
					mensaje = "El perfil " + perfil.nombre + " que usted aprobó fue desactivado.";
					TempData["alerta"] = "El perfil fue desactivado satisfactoriamente";
				}
				else
				{
					mensaje = "El perfil " + perfil.nombre + " que usten aprobó fue activado.";
					TempData["alerta"] = "El perfil fue activado satisfactoriamente";
				}
				db.Entry(perfil).State = EntityState.Modified;
			}
			db.SaveChanges();

			string correo = db.Colaborador.Find(perfil.idcolaborador).correo;
			string titulo = "Actualización de perfil";
			if (Correo.sepuede)
				Correo.enviar2(correo, titulo, mensaje);
			return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

		[HttpGet]
		public ContentResult sepuede()
		{
			if (Correo.sepuede)
				return Content("1", "text");
			else
				return Content("0", "text");
		}

    }
}