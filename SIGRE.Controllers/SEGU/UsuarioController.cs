using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIGRE.Models;
using System.Web.Security;
using WebMatrix.WebData;

namespace SIGRE.Controllers.SEGU
{
	[Authorize]
	public class UsuarioController : Controller
	{
		private BDSIGRE db = new BDSIGRE();

		public ActionResult Index()
		{
			return View(db.Colaborador.Include(x => x.usuario).ToList());
		}

		public ActionResult Create()
		{
			ViewBag.lcolaborador = db.Colaborador.OrderBy(x => x.nombre).Where(x => x.idusuario != null).Include(x => x.usuario).ToList();
			ViewBag.idrol = new SelectList(db.Rol.Where(x => !x.dinamico).OrderBy(x => x.nombre), "idrol", "nombre");
			return View(new Usuario() { password = "1" });
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(Usuario usuario)
		{
			if (string.IsNullOrEmpty(usuario.colaborador_nombre) && string.IsNullOrEmpty(usuario.username))
				ModelState.AddModelError("colaborador_nombre", "Colaborador no existe, ingrese un código o nombre");
			if (!string.IsNullOrEmpty(usuario.colaborador_nombre) && string.IsNullOrEmpty(usuario.username))
				ModelState.AddModelError("colaborador_nombre", "Colaborador no existe, ingrese un código o nombre");
			if (!string.IsNullOrEmpty(usuario.colaborador_nombre) && !string.IsNullOrEmpty(usuario.username) && db.Usuario.Where(x => x.username == usuario.username).Count() > 0)
				ModelState.AddModelError("colaborador_nombre", "El colaborador ingresado ya tiene un usuario creado");
			if (string.IsNullOrEmpty(usuario.passwordn))
				ModelState.AddModelError("passwordn", "Genere una contraseña");
			if (ModelState.IsValid)
			{
				usuario.password = Halp.GetMD5(usuario.passwordn);
				db.Usuario.Add(usuario);
				db.SaveChanges();
				Colaborador colaborador = db.Colaborador.Where(x => x.codigo == usuario.username).FirstOrDefault();
				colaborador.idusuario = usuario.idusuario;
				db.Entry(colaborador).State = EntityState.Modified;
				db.SaveChanges();
				if (Correo.sepuede)
				{
					string titulo = "Creación de usuario";
					string mensaje = "Su cuenta de usuario fue creada<br/><br/>Código de usuario: " + usuario.username + "<br/>Contraseña: " + usuario.passwordn + "<br/>Rol: " + db.Rol.Find(usuario.idrol).nombre + "<br/>Bloqueado: " + usuario.muestraBloqueado;
					Correo.enviar2(colaborador.correo, titulo, mensaje);
					TempData["alerta"] = "Usuario registrado satisfactoriamente";
				}
				else
					TempData["alerta"] = "Usuario registrado con éxito. Notifique la clave al usuario con la opción del mismo nombre cuando la conexión a Internet haya sido restablecida";
				return RedirectToAction("Create");
			}
			ViewBag.lcolaborador = db.Colaborador.OrderBy(x => x.nombre).Where(x => x.idusuario != null).Include(x => x.usuario).ToList();
			ViewBag.idrol = new SelectList(db.Rol.Where(x => !x.dinamico).OrderBy(x => x.nombre), "idrol", "nombre", usuario.idrol);
			return View(usuario);
		}

		public ActionResult Edit(int id = 0)
		{
			Usuario usuario = db.Usuario.Find(id);
			usuario.colaborador_nombre = db.Colaborador.Where(x => x.idusuario == usuario.idusuario).FirstOrDefault().nombre;
			if (usuario == null)
			{
				return HttpNotFound();
			}
			ViewBag.lcolaborador = db.Colaborador.OrderBy(x => x.nombre).Where(x => x.idusuario != null).Include(x => x.usuario).ToList();
			ViewBag.idrol = new SelectList(db.Rol.Where(x => !x.dinamico).OrderBy(x => x.nombre), "idrol", "nombre", usuario.idrol);
			return View(usuario);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(Usuario usuario)
		{
			if (ModelState.IsValid)
			{
				db.Entry(usuario).State = EntityState.Modified;
				if (string.IsNullOrEmpty(usuario.passwordn))
					db.Entry(usuario).Property("password").IsModified = false;
				else
					usuario.password = Halp.GetMD5(usuario.passwordn);
				db.SaveChanges();
				if (Correo.sepuede)
				{
					string titulo = "Actualización de usuario";
					string mensaje = "Su cuenta de usuario fue actualizada<br/><br/>Código de usuario: " + usuario.username + "<br/>Contraseña: " + (string.IsNullOrEmpty(usuario.passwordn) ? "Sin cambio" : usuario.passwordn) + "<br/>Rol: " + db.Rol.Find(usuario.idrol).nombre + "<br/>Bloqueado: " + usuario.muestraBloqueado;
					Correo.enviar2(db.Colaborador.Where(x => x.idusuario == usuario.idusuario).FirstOrDefault().correo, titulo, mensaje);
					TempData["alerta"] = "Usuario actualizado satisfactoriamente";
				}
				else
					TempData["alerta"] = "Usuario actualizado con éxito. Notifique la clave al usuario con la opción del mismo nombre cuando la conexión a Internet haya sido restablecida";
				return RedirectToAction("Create");
			}
			ViewBag.lcolaborador = db.Colaborador.OrderBy(x => x.nombre).Where(x => x.idusuario != null).Include(x => x.usuario).ToList();
			ViewBag.idrol = new SelectList(db.Rol.Where(x => !x.dinamico).OrderBy(x => x.nombre), "idrol", "nombre", usuario.idrol);
			return View(usuario);
		}

		public ActionResult Delete(int id = 0)
		{
			Colaborador colaborador = db.Colaborador.Where(x => x.idusuario == id).FirstOrDefault();
			colaborador.idusuario = null;
			db.Entry(colaborador).State = EntityState.Modified;
			db.SaveChanges();
			Usuario usuario = db.Usuario.Find(id);
			db.Usuario.Remove(usuario);
			db.SaveChanges();
			TempData["alerta"] = "Usuario eliminado satisfactoriamente";
			return RedirectToAction("Create");
		}

		public ActionResult Clave(int id = 0)
		{
			if (Correo.sepuede)
			{
				Usuario usuario = db.Usuario.Find(id);
				usuario.passwordn = Halp.GetPassword();
				usuario.password = Halp.GetMD5(usuario.passwordn);
				db.SaveChanges();
				string titulo = "Cuenta de usuario";
				string mensaje = "Los datos de su cuenta de usuario son<br/><br/>Código de usuario: " + usuario.username + "<br/>Contraseña: " + usuario.passwordn + "<br/>Rol: " + db.Rol.Find(usuario.idrol).nombre + "<br/>Bloqueado: " + usuario.muestraBloqueado;
				Correo.enviar2(db.Colaborador.Where(x => x.idusuario == usuario.idusuario).FirstOrDefault().correo, titulo, mensaje);
				TempData["alerta"] = "Notificación enviada con éxito";
			}
			else
				TempData["alerta"] = "No es posible notificar al colaborador porque no hay conexión a internet";
			return RedirectToAction("Create");
		}

		protected override void Dispose(bool disposing)
		{
			db.Dispose();
			base.Dispose(disposing);
		}

		public ActionResult EditPassword()
		{
			Usuario usuario = db.Usuario.Where(x => x.username == User.Identity.Name).FirstOrDefault();
			return View(usuario);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult EditPassword(Usuario usuario)
		{
			ViewBag.password = usuario.password;
			ViewBag.passwordn = usuario.passwordn;
			ViewBag.passwordr = usuario.passwordr;
			bool ok = false;
			Usuario o = db.Usuario.Find(usuario.idusuario);
			if (usuario.password != null && Halp.GetMD5(usuario.password) == o.password)
				ok = true;
			else
			{
				o.intentospass++;
				if (o.intentospass < 5)
					ModelState.AddModelError("password", "La contraseña ingresada es incorrecta");
				else if (o.intentospass == 5)
				{
					ModelState.Remove("password");
					ModelState.AddModelError("password", "La contraseña ingresada es incorrecta. Si falla en el próximo intento su cuenta será bloqueada");
				}
				else
				{
					o.bloqueado = true;
					db.Entry(o).State = EntityState.Modified;
					db.SaveChanges();
					TempData["alerta"] = "Su cuenta ha sido bloqueada. Comuníquese con el área de soporte";
					return RedirectToAction("GTFO", "Account", new { Area = "" });
				}
				db.Entry(o).State = EntityState.Modified;
				db.SaveChanges();
			}
			if (string.IsNullOrEmpty(usuario.passwordn) || !Halp.esPassword(usuario.passwordn))
				ModelState.AddModelError("passwordn", "✘");
			else
				ModelState.AddModelError("passwordn", "✔");
			if (string.IsNullOrEmpty(usuario.passwordr) || !Halp.esPassword(usuario.passwordr) || usuario.passwordn != usuario.passwordr)
				ModelState.AddModelError("passwordr", "✘");
			if (!string.IsNullOrEmpty(usuario.passwordn) && !string.IsNullOrEmpty(usuario.passwordr) && Halp.esPassword(usuario.passwordn) && Halp.esPassword(usuario.passwordr) && usuario.passwordn == usuario.passwordr)
			{
				ModelState.AddModelError("passwordn", "✔");
				ModelState.AddModelError("passwordr", "✔");
				if (ok)
					if (o.password == Halp.GetMD5(usuario.passwordn))
						ModelState.AddModelError("password", "Nueva contraseña debe ser diferente a la actual");
					else
					{
						o.password = Halp.GetMD5(usuario.passwordn);
						o.intentospass = 0;
						db.Entry(o).State = EntityState.Modified;
						db.SaveChanges();
						TempData["alerta"] = "Contraseña cambiada con éxito";
						return RedirectToAction("Index", "Home", new { Area = "" });
					}
			}

			/*
			if (!string.IsNullOrEmpty(usuario.passwordn) && !string.IsNullOrEmpty(usuario.passwordr) && usuario.passwordn != usuario.passwordr)
			{
				ModelState.AddModelError("passwordn", "✘");
				ModelState.AddModelError("passwordr", "✘");
			}
			else if (!string.IsNullOrEmpty(usuario.passwordn) && !string.IsNullOrEmpty(usuario.passwordr) && usuario.passwordn == usuario.passwordr)
			{
				if (Halp.esPassword(usuario.passwordn))
				{
					ModelState.AddModelError("passwordn", "✔");
					ModelState.AddModelError("passwordr", "✔");
					if (ok)
						if (o.password == Halp.GetMD5(usuario.passwordn))
							ModelState.AddModelError("password", "Nueva contraseña debe ser diferente a la actual");
						else
						{
							o.password = Halp.GetMD5(usuario.passwordn);
							o.intentospass = 0;
							db.Entry(o).State = EntityState.Modified;
							db.SaveChanges();
							TempData["alerta"] = "Contraseña cambiada con éxito";
							return RedirectToAction("Index", "Home", new { Area = "" });
						}
				}
				else
				{
					ModelState.AddModelError("passwordn", "✘");
					ModelState.AddModelError("passwordr", "✘");
				}
			}
			*/
			return View(usuario);
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
				aprobado = x.aprobado,
				idusuario = x.idusuario
			}).Where(x => ((x.codigo.Contains(term) || x.nombre.Contains(term)) && !x.desactivado && x.aprobado == true && x.idusuario == null));
			return Json(data, JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		public ContentResult Password()
		{
			return Content(Halp.GetPassword(), "text");
		}
	}
}