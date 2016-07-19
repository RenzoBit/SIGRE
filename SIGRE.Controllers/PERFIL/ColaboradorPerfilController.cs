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
	public class ColaboradorPerfilController : Controller
	{
		private BDSIGRE db = new BDSIGRE();
		private static Perfil perfil = null;

		public ActionResult Index(int id = 0)
		{
			perfil = db.Perfil.Find(id);
			ViewBag.perfil = perfil;
			var colaboradorperfil = db.ColaboradorPerfil.Where(c => c.idperfil == id).Include(c => c.colaborador).Include(c => c.perfil);
			return View(colaboradorperfil.ToList());
		}

		public ActionResult Create()
		{
			ViewBag.perfil = perfil;
			ViewBag.idcolaborador = new SelectList(db.Colaborador.Where(x => x.centrocosto.centrocosto_idcentrocosto == perfil.idcentrocosto && x.idperfil == null && !x.lcolaboradorperfil.Any(y => y.aprobado == null)), "idcolaborador", "nombre");
			ColaboradorPerfil o = new ColaboradorPerfil() { idperfil = perfil.idperfil };
			o.perfil = db.Perfil.Find(o.idperfil);
			ViewBag.idrecurso = new MultiSelectList(o.perfil.lperfilrecurso, "idrecurso", "recurso.muestraNombrePrecio");
			return View(o);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[HandleError]
		public ActionResult Create(ColaboradorPerfil o1, FormCollection collection)
		{
			if (o1.idcolaborador == 0)
			{
				ModelState.AddModelError("idcolaborador", "Seleccione un colaborador");
			}
			else
			{
				if (Correo.sepuede || collection["forzar"] == "1")
				{
					ColaboradorPerfil o2 = new ColaboradorPerfil() { idcolaborador = o1.idcolaborador, idperfil = o1.idperfil };
					db.ColaboradorPerfil.Add(o2);
					db.SaveChanges();
					TempData["alerta"] = collection["forzar"] == "1" ? "Se guardo la asignación de perfil sin envío de solicitud de aprobación" : "Solicitud de aprobación enviada con éxito";
					string correo = db.Colaborador.Find(perfil.unidadnegocio.idcolaborador).correo;
					string titulo = "Solicitud de aprobación";
					string mensaje = "Solicitud de aprobación de nueva asignación de perfil<br/><br/><a href=\""
						+ (Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/PERFIL/ColaboradorPerfil/Edit2/" + o2.idcolaboradorperfil)
						+ "\" target=\"_blank\">IR</a>";
					if (Correo.sepuede)
						Correo.enviar2(correo, titulo, mensaje);
					return RedirectToAction("Index", new { id = perfil.idperfil });
				}
				else
					TempData["confirma"] = "No se podrá enviar la solicitud de aprobación de perfil ¿desea continuar de todos modos?|$('#forzar').val('1'); $('#form1').submit();";
			}

			ViewBag.perfil = perfil;
			ViewBag.idcolaborador = new SelectList(db.Colaborador.Where(x => x.centrocosto.centrocosto_idcentrocosto == perfil.idcentrocosto && x.idperfil == null && !x.lcolaboradorperfil.Any(y => y.aprobado == null)), "idcolaborador", "nombre", o1.idcolaborador);
			o1.perfil = db.Perfil.Find(o1.idperfil);
			ViewBag.idrecurso = new MultiSelectList(o1.perfil.lperfilrecurso, "idrecurso", "recurso.muestraNombrePrecio");
			return View(o1);
		}

		public ActionResult Edit(int id = 0)
		{
			ColaboradorPerfil colaboradorperfil = db.ColaboradorPerfil.Find(id);
			if (colaboradorperfil == null)
			{
				return HttpNotFound();
			}
			ViewBag.perfil = perfil;
			ViewBag.idcolaborador = new SelectList(db.Colaborador.Where(x => x.centrocosto.centrocosto_idcentrocosto == perfil.idcentrocosto), "idcolaborador", "nombre", colaboradorperfil.idcolaborador);
			return View(colaboradorperfil);
		}

		public ActionResult Edit2(int id = 0)
		{
			ColaboradorPerfil o = db.ColaboradorPerfil.Find(id);
			if (o == null)
			{
				TempData["alerta"] = "La solicitud ya fue eliminada";
				return RedirectToAction("Index", "Home", new { Area = "" });
			}
			else if (o.aprobado == true)
			{
				TempData["alerta"] = "La solicitud ya fue aprobada";
				return RedirectToAction("Index", "Home", new { Area = "" });
			}
			else if (o.aprobado == false)
			{
				TempData["alerta"] = "La solicitud ya fue rechazada";
				return RedirectToAction("Index", "Home", new { Area = "" });
			}
			Perfil perfil = db.Perfil.Find(o.idperfil);
			ViewBag.perfil = perfil;
			ViewBag.idcolaborador = new SelectList(db.Colaborador.Where(x => x.centrocosto.centrocosto_idcentrocosto == perfil.idcentrocosto), "idcolaborador", "nombre", o.idcolaborador);
			o.perfil = db.Perfil.Find(o.idperfil);
			ViewBag.idrecurso = new MultiSelectList(o.perfil.lperfilrecurso, "idrecurso", "recurso.muestraNombrePrecio");
			return View(o);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[HandleError]
		public ActionResult Edit(ColaboradorPerfil o)
		{
			if (ModelState.IsValid)
			{
				o.fecha = DateTime.Now;
				db.Entry(o).State = EntityState.Modified;
				db.SaveChanges();
				if (o.aprobado == null)
				{
					TempData["alerta"] = "Solicitud de aprobación enviada con éxito";
					string correo = db.Colaborador.Find(perfil.idcolaborador).correo;
					string titulo = "Solicitud de aprobación";
					string mensaje = "Solicitud de aprobación de nueva asignación de perfil<br/><br/><a href=\""
						+ (Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/PERFIL/ColaboradorPerfil/Edit2/" + o.idcolaboradorperfil)
						+ "\" target=\"_blank\">IR</a>";
					if (Correo.sepuede)
						Correo.enviar2(correo, titulo, mensaje);
				}
				else
					TempData["alerta"] = "Asignación de perfil actualizada con éxito";
				return RedirectToAction("Index", new { id = perfil.idperfil });
			}
			ViewBag.perfil = perfil;
			ViewBag.idcolaborador = new SelectList(db.Colaborador.Where(x => x.centrocosto.centrocosto_idcentrocosto == perfil.idcentrocosto), "idcolaborador", "nombre", o.idcolaborador);
			return View(o);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[HandleError]
		public ActionResult Edit2(ColaboradorPerfil o1)
		{
			ColaboradorPerfil o2 = new ColaboradorPerfil() { idcolaboradorperfil = o1.idcolaboradorperfil, idcolaborador = o1.idcolaborador, idperfil = o1.idperfil, fecha = o1.fecha, comentario = o1.comentario, aprobado = o1.aprobado, revocacion = o1.revocacion };
			Colaborador colaborador = db.Colaborador.Find(o2.idcolaborador);
			string titulo = null;
			string mensaje = null;
			int idcorreo = 0;
			if (o2.revocacion)
			{
				titulo = "Solicitud de revocación";
				mensaje = "La solicitud de revocación de perfil para " + colaborador.nombre;
				TempData["alerta"] = "Solicitud de revocación guardada con éxito";
				if (o2.aprobado == true)
				{
					idcorreo = Rol.FACILITADOR;
					mensaje += " fue aceptada, puede proceder a actualizar el perfil asignado al colaborador";
					colaborador.idperfil = null;
					db.Entry(colaborador).State = EntityState.Modified;
					db.SaveChanges();
					//db.ColaboradorPerfil.Remove(db.ColaboradorPerfil.Find(o2.idcolaboradorperfil));
					//db.SaveChanges();
				}
				else if (o2.aprobado == false)
				{
					idcorreo = Rol.CONSULTOR;
					mensaje += " fue rechazada";
					o2.revocacion = false;
					o2.aprobado = true;
				}
				db.Entry(o2).State = EntityState.Modified;
				db.SaveChanges();
			}
			else
			{
				titulo = "Solicitud de asignación";
				mensaje = "La solicitud de asignación de perfil para " + colaborador.nombre;
				TempData["alerta"] = "Solicitud de asignación guardada con éxito";
				if (o2.aprobado == true)
				{
					idcorreo = Rol.FACILITADOR;
					mensaje += " fue aceptada, puede proceder a aplicar el perfil asignado al colaborador";
					colaborador.idperfil = o2.idperfil;
					db.Entry(colaborador).State = EntityState.Modified;
					db.SaveChanges();
				}
				else if (o2.aprobado == false)
				{
					idcorreo = Rol.CONSULTOR;
					mensaje += " fue rechazada";
					//db.ColaboradorPerfil.Remove(db.ColaboradorPerfil.Find(o2.idcolaboradorperfil));
					//db.SaveChanges();
				}
				db.Entry(o2).State = EntityState.Modified;
				db.SaveChanges();
			}
			Colaborador rolando = db.Colaborador.Where(x => x.usuario.idrol == idcorreo && x.aprobado == true && !x.desactivado).FirstOrDefault();
			string correo = rolando == null ? "" : rolando.correo;
			if (Correo.sepuede)
				Correo.enviar2(correo, titulo, mensaje);
			return RedirectToAction("Index", "Home", new { Area = "" });
		}

		[HandleError]
		public ActionResult Delete(int id = 0)
		{
			ColaboradorPerfil o = db.ColaboradorPerfil.Find(id > 0 ? id : -id);
			string mensaje = null;
			string titulo = null;
			if (id > 0)
			{
				titulo = "Eliminación de solicitud";
				mensaje = "La solicitud de asignación del perfil " + perfil.nombre + " a " + o.colaborador.nombre + " fue eliminada";
				TempData["alerta"] = "La solicitud fue eliminada satisfactoriamente";
				db.ColaboradorPerfil.Remove(o);
			}
			else
			{
				titulo = "Solicitud de revocación";
				mensaje = "Solicitud de revocación de asignación de perfil<br/><br/><a href=\""
					+ (Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/PERFIL/ColaboradorPerfil/Edit2/" + o.idcolaboradorperfil)
					+ "\" target=\"_blank\">IR</a>";
				o.revocacion = true;
				o.aprobado = null;
				db.Entry(o).State = EntityState.Modified;
				TempData["alerta"] = (Correo.sepuede ? "Solicitud de revocación enviada con éxito" : "Solicitud de revocación registrada con éxito");
			}
			db.SaveChanges();
			string correo = db.Colaborador.Find(perfil.unidadnegocio.idcolaborador).correo;
			if (Correo.sepuede)
				Correo.enviar2(correo, titulo, mensaje);
			return RedirectToAction("Index", new { id = perfil.idperfil });
		}

		public ActionResult IndexAsignar()
		{
			Colaborador colaborador = db.Colaborador.Where(x => x.codigo == User.Identity.Name).FirstOrDefault();
			List<ColaboradorPerfil> l = db.ColaboradorPerfil.Where(x => x.perfil.idcolaborador == colaborador.idcolaborador && x.aprobado == null && !x.revocacion).Include(x => x.colaborador).Include(x => x.perfil).ToList();
			return View(l);
		}

		public ActionResult IndexRevocar()
		{
			Colaborador colaborador = db.Colaborador.Where(x => x.codigo == User.Identity.Name).FirstOrDefault();
			List<ColaboradorPerfil> l = db.ColaboradorPerfil.Where(x => x.perfil.idcolaborador == colaborador.idcolaborador && x.aprobado == null && x.revocacion).Include(x => x.colaborador).Include(x => x.perfil).ToList();
			return View(l);
		}

		public ActionResult EditAprobar(int id = 0)
		{
			ColaboradorPerfil o = db.ColaboradorPerfil.Find(id);
			Perfil perfil = db.Perfil.Find(o.idperfil);
			ViewBag.perfil = perfil;
			ViewBag.idcolaborador = new SelectList(db.Colaborador.Where(x => x.centrocosto.centrocosto_idcentrocosto == perfil.idcentrocosto), "idcolaborador", "nombre", o.idcolaborador);
			o.perfil = db.Perfil.Find(o.idperfil);
			ViewBag.idrecurso = new MultiSelectList(o.perfil.lperfilrecurso, "idrecurso", "recurso.muestraNombrePrecio");
			return View(o);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[HandleError]
		public ActionResult EditAprobar(ColaboradorPerfil ox)
		{
			ColaboradorPerfil o1 = db.ColaboradorPerfil.Find(ox.idcolaboradorperfil);
			o1.aprobado = ox.aprobado;
			Colaborador colaborador = db.Colaborador.Find(o1.idcolaborador);
			string titulo = null;
			string mensaje = null;
			int idcorreo = 0;
			string redirect = (o1.revocacion ? "IndexRevocar" : "IndexAsignar");
			if (o1.revocacion)
			{
				titulo = "Solicitud de revocación";
				mensaje = "La solicitud de revocación de perfil para " + colaborador.nombre;
				TempData["alerta"] = "Solicitud de revocación guardada con éxito";
				if (o1.aprobado == true)
				{
					idcorreo = Rol.FACILITADOR;
					mensaje += " fue aceptada, puede proceder a actualizar el perfil asignado al colaborador";
					colaborador.idperfil = null;
					db.Entry(colaborador).State = EntityState.Modified;
					db.SaveChanges();
					//db.ColaboradorPerfil.Remove(o1);
					//db.SaveChanges();
				}
				else if (o1.aprobado == false)
				{
					idcorreo = Rol.CONSULTOR;
					mensaje += " fue rechazada";
					o1.revocacion = false;
					o1.aprobado = true;
				}
				db.Entry(o1).State = EntityState.Modified;
				db.SaveChanges();
			}
			else
			{
				titulo = "Solicitud de asignación";
				mensaje = "La solicitud de asignación de perfil para " + colaborador.nombre;
				TempData["alerta"] = "Solicitud de asignación guardada con éxito";
				if (o1.aprobado == true)
				{
					idcorreo = Rol.FACILITADOR;
					mensaje += " fue aceptada, puede proceder a aplicar el perfil asignado al colaborador";
					colaborador.idperfil = o1.idperfil;
					db.Entry(colaborador).State = EntityState.Modified;
					db.SaveChanges();
				}
				else if (o1.aprobado == false)
				{
					idcorreo = Rol.CONSULTOR;
					mensaje += " fue rechazada";
					//db.ColaboradorPerfil.Remove(o1);
					//db.SaveChanges();
				}
				db.Entry(o1).State = EntityState.Modified;
				db.SaveChanges();
			}
			Colaborador rolando = db.Colaborador.Where(x => x.usuario.idrol == idcorreo && x.aprobado == true && !x.desactivado).FirstOrDefault();
			string correo = rolando == null ? "" : rolando.correo;
			if (Correo.sepuede)
				Correo.enviar2(correo, titulo, mensaje);
			return RedirectToAction(redirect);
		}

		[HandleError]
		public ActionResult DeleteAprobar(int id = 0)
		{
			ColaboradorPerfil o1 = db.ColaboradorPerfil.Find(id >= 0 ? id : -id);
			o1.aprobado = (id >= 0);
			Colaborador colaborador = db.Colaborador.Find(o1.idcolaborador);
			string titulo = null;
			string mensaje = null;
			int idcorreo = 0;
			string redirect = (o1.revocacion ? "IndexRevocar" : "IndexAsignar");
			if (o1.revocacion)
			{
				titulo = "Solicitud de revocación";
				mensaje = "La solicitud de revocación de perfil para " + colaborador.nombre;
				TempData["alerta"] = "Solicitud de revocación guardada con éxito";
				if (o1.aprobado == true)
				{
					idcorreo = Rol.FACILITADOR;
					mensaje += " fue aceptada, puede proceder a actualizar el perfil asignado al colaborador";
					colaborador.idperfil = null;
					db.Entry(colaborador).State = EntityState.Modified;
					db.SaveChanges();
					//db.ColaboradorPerfil.Remove(o1);
					//db.SaveChanges();
				}
				else if (o1.aprobado == false)
				{
					idcorreo = Rol.CONSULTOR;
					mensaje += " fue rechazada";
					o1.revocacion = false;
					o1.aprobado = true;
				}
				db.Entry(o1).State = EntityState.Modified;
				db.SaveChanges();
			}
			else
			{
				titulo = "Solicitud de asignación";
				mensaje = "La solicitud de asignación de perfil para " + colaborador.nombre;
				TempData["alerta"] = "Solicitud de asignación guardada con éxito";
				if (o1.aprobado == true)
				{
					idcorreo = Rol.FACILITADOR;
					mensaje += " fue aceptada, puede proceder a aplicar el perfil asignado al colaborador";
					colaborador.idperfil = o1.idperfil;
					db.Entry(colaborador).State = EntityState.Modified;
					db.SaveChanges();
				}
				else if (o1.aprobado == false)
				{
					idcorreo = Rol.CONSULTOR;
					mensaje += " fue rechazada";
					//db.ColaboradorPerfil.Remove(o1);
					//db.SaveChanges();
				}
				db.Entry(o1).State = EntityState.Modified;
				db.SaveChanges();
			}
			Colaborador rolando = db.Colaborador.Where(x => x.usuario.idrol == idcorreo && x.aprobado == true && !x.desactivado).FirstOrDefault();
			string correo = rolando == null ? "" : rolando.correo;
			if (Correo.sepuede)
				Correo.enviar2(correo, titulo, mensaje);
			return RedirectToAction(redirect);
		}

		public ActionResult IndexConsultar()
		{
			return View(new List<ColaboradorPerfil>());
		}

		[HttpPost]
		public ActionResult IndexConsultar(string codigo = "")
		{
			Colaborador o = db.Colaborador.Where(x => x.codigo == codigo).FirstOrDefault();
			List<ColaboradorPerfil> l = db.ColaboradorPerfil.Where(x => x.colaborador.codigo == codigo && (x.aprobado == null || x.aprobado == true)).ToList();
			if (l.Count() == 0)
				if (o == null)
					TempData["alerta"] = "El X/MUID ingresado no existe en el sistema. Por favor, intente nuevamente";
				else
					TempData["alerta"] = "El colaborador indicado no tiene un perfil asignado o revocado";
			return View(l);
		}

		public ActionResult EditConsultar(int id = 0)
		{
			ColaboradorPerfil o = db.ColaboradorPerfil.Find(id);
			Perfil perfil = db.Perfil.Find(o.idperfil);
			ViewBag.perfil = perfil;
			ViewBag.idcolaborador = new SelectList(db.Colaborador.Where(x => x.centrocosto.centrocosto_idcentrocosto == perfil.idcentrocosto), "idcolaborador", "nombre", o.idcolaborador);
			o.perfil = db.Perfil.Find(o.idperfil);
			ViewBag.idrecurso = new MultiSelectList(o.perfil.lperfilrecurso, "idrecurso", "recurso.muestraNombrePrecio");
			return View(o);
		}

		protected override void Dispose(bool disposing)
		{
			db.Dispose();
			base.Dispose(disposing);
		}
	}
}