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
using System.IO;

namespace SIGRE.Controllers.ADMIN
{
	[Authorize]
	public class ColaboradorController : Controller
    {
        private BDSIGRE db = new BDSIGRE();

		public ActionResult Index(int sesion = 1)
		{
			IQueryable<Colaborador> colaborador;
			bool ignora = false;
			if (sesion == 0 || (Session["p_idcentrocosto"] == null && Session["p_nombre"] == null))
				ignora = true;
			if (ignora)
			{
				Session["p_idcentrocosto"] = null;
				Session["p_nombre"] = null;
				ViewBag.idcentrocosto = new SelectList(db.CentroCosto.Where(x => x.centrocosto_idcentrocosto == null), "idcentrocosto", "nombre");
				colaborador = db.Colaborador.Include(c => c.aprobador).Include(c => c.colaboradortipo).Include(c => c.perfil).Include(c => c.usuario).Include(c => c.centrocosto);
			}
			else
			{
				int idcentrocosto = (int)Session["p_idcentrocosto"];
				string nombre = (string)Session["p_nombre"];
				ViewBag.idcentrocosto = new SelectList(db.CentroCosto.Where(x => x.centrocosto_idcentrocosto == null), "idcentrocosto", "nombre", idcentrocosto);
				colaborador = db.Colaborador.Where(
				s => (idcentrocosto == 0 || s.centrocosto.centrocosto_idcentrocosto == idcentrocosto)
					&& (nombre.Trim().Equals("") || s.nombre.Contains(nombre.Trim()))
				).Include(c => c.aprobador).Include(c => c.colaboradortipo).Include(c => c.perfil).Include(c => c.usuario).Include(c => c.centrocosto);
			}
			return View(colaborador.ToList());
		}

		[HttpPost]
		public ActionResult Index(int idcentrocosto = 0, string nombre = "")
		{
			Session["p_idcentrocosto"] = idcentrocosto;
			Session["p_nombre"] = nombre;
			ViewBag.idcentrocosto = new SelectList(db.CentroCosto.Where(x => x.centrocosto_idcentrocosto == null), "idcentrocosto", "nombre", idcentrocosto);
			var o = db.Colaborador.Where(
				s => (idcentrocosto == 0 || s.centrocosto.centrocosto_idcentrocosto == idcentrocosto)
					&& (nombre.Trim().Equals("") || s.nombre.Contains(nombre.Trim()))
				).Include(c => c.aprobador).Include(c => c.colaboradortipo).Include(c => c.perfil).Include(c => c.usuario).Include(c => c.centrocosto);
			List<Colaborador> l = o.ToList();
			if (l.Count() == 0)
				TempData["alerta"] = "No se encontraron resultados para la búsqueda";
			return View(l);
		}

		public ActionResult IndexAprobar()
		{
			Colaborador colaborador = db.Colaborador.Where(x => x.codigo == User.Identity.Name).FirstOrDefault();
			List<Colaborador> l = db.Colaborador.Where(x => x.colaborador_idcolaborador == colaborador.idcolaborador && !x.desactivado && x.aprobado == null).Include(c => c.aprobador).Include(c => c.colaboradortipo).Include(c => c.perfil).Include(c => c.usuario).Include(c => c.centrocosto).ToList();
			return View(l);
		}

		public ActionResult EditAprobar(int id = 0)
		{
			Colaborador colaborador = db.Colaborador.Find(id);
			colaborador.centrocosto_idcentrocosto = colaborador.centrocosto.centrocosto_idcentrocosto ?? default(int);
			ViewBag.idcolaboradortipo = new SelectList(db.ColaboradorTipo, "idcolaboradortipo", "descripcion", colaborador.idcolaboradortipo);
			ViewBag.centrocosto_idcentrocosto = new SelectList(db.CentroCosto.Where(c => c.centrocosto_idcentrocosto == null), "idcentrocosto", "nombre", colaborador.centrocosto_idcentrocosto);
			ViewBag.idcentrocosto = new SelectList(db.CentroCosto.Where(c => c.centrocosto_idcentrocosto == colaborador.centrocosto_idcentrocosto), "idcentrocosto", "nombre", colaborador.idcentrocosto);
			return View(colaborador);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[HandleError]
		public ActionResult EditAprobar(Colaborador colaborador)
		{
			db.Entry(colaborador).State = EntityState.Modified;
			db.Entry(colaborador).Property("foto").IsModified = false;
			db.SaveChanges();
			Colaborador rolando = db.Colaborador.Where(x => x.usuario.idrol == Rol.CONSULTOR && x.aprobado == true && !x.desactivado).FirstOrDefault();
			string correo = rolando == null ? "" : rolando.correo;
			string titulo = "Petición de aprobación";
			string mensaje = "La petición de aprobación del colaborador " + colaborador.nombre + " fue " + (colaborador.aprobado == true ? "aceptada" : "rechazada");
			if (Correo.sepuede)
				Correo.enviar2(correo, titulo, mensaje);
			TempData["alerta"] = "Petición de aprobación guardada con éxito";
			return RedirectToAction("IndexAprobar");
		}

		[HandleError]
		public ActionResult DeleteAprobar(int id = 0)
		{
			Colaborador colaborador = db.Colaborador.Find(id >= 0 ? id : -id);
			colaborador.aprobado = (id >= 0);
			db.Entry(colaborador).State = EntityState.Modified;
			db.Entry(colaborador).Property("foto").IsModified = false;
			db.SaveChanges();
			Colaborador rolando = db.Colaborador.Where(x => x.usuario.idrol == Rol.CONSULTOR && x.aprobado == true && !x.desactivado).FirstOrDefault();
			string correo = rolando == null ? "" : rolando.correo;
			string titulo = "Petición de aprobación";
			string mensaje = "La petición de aprobación del colaborador " + colaborador.nombre + " fue " + (colaborador.aprobado == true ? "aceptada" : "rechazada");
			if (Correo.sepuede)
				Correo.enviar2(correo, titulo, mensaje);
			TempData["alerta"] = "Petición de aprobación guardada con éxito";
			return RedirectToAction("IndexAprobar");
		}

        public ActionResult Create()
        {
            ViewBag.idcolaboradortipo = new SelectList(db.ColaboradorTipo, "idcolaboradortipo", "descripcion");
			ViewBag.centrocosto_idcentrocosto = new SelectList(db.CentroCosto.Where(c => c.centrocosto_idcentrocosto == null), "idcentrocosto", "nombre");
			ViewBag.idcentrocosto = new SelectList(db.CentroCosto.Where(c => c.centrocosto_idcentrocosto == 0), "idcentrocosto", "nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
		[HandleError]
        public ActionResult Create(Colaborador colaborador)
		{
			colaborador.codigo = colaborador.codigo.Replace(" ", "");
			int i = db.Colaborador.Where(c => c.codigo.Equals(colaborador.codigo)).Count();
			if (i > 0)
			{
				ModelState.AddModelError("codigo", "El código ya fue registrado");
			}
			if (colaborador.idcolaboradortipo != 0)
				if (colaborador.idcolaboradortipo == 1 && !colaborador.codigo[0].Equals('M'))
					ModelState.AddModelError("codigo", "El prefijo del código debería ser M");
				else if (colaborador.idcolaboradortipo == 2 && !colaborador.codigo[0].Equals('X'))
					ModelState.AddModelError("codigo", "El prefijo del código debería ser X");
			if (colaborador.file != null && colaborador.file.ContentLength > 0)
			{
				var validImageTypes = new string[]
				{
					"image/gif",
					"image/jpeg",
					"image/pjpeg",
					"image/png"
				};
				if (!validImageTypes.Contains(colaborador.file.ContentType))
				{
					ModelState.AddModelError("file", "Seleccione una imagen GIF, JPG o PNG");
				}
				else
				{
					MemoryStream target = new MemoryStream();
					colaborador.file.InputStream.CopyTo(target);
					colaborador.foto = target.ToArray();
				}
			}

            if (ModelState.IsValid)
            {
				db.Colaborador.Add(colaborador);
				db.SaveChanges();
				TempData["alerta"] = "Solicitud de confirmación enviada con éxito";
				string correo = db.Colaborador.Find(colaborador.colaborador_idcolaborador).correo;
				string titulo = "Solicitud de aprobación";
				string mensaje = "Solicitud de aprobación de nuevo colaborador<br/><br/><a href=\""
						+ (Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/ADMIN/Colaborador/Edit2/" + colaborador.idcolaborador)
						+ "\" target=\"_blank\">IR</a>";
				if (Correo.sepuede)
					Correo.enviar2(correo, titulo, mensaje);
				return RedirectToAction("Index");
            }

            ViewBag.idcolaboradortipo = new SelectList(db.ColaboradorTipo, "idcolaboradortipo", "descripcion", colaborador.idcolaboradortipo);
			ViewBag.centrocosto_idcentrocosto = new SelectList(db.CentroCosto.Where(c => c.centrocosto_idcentrocosto == null), "idcentrocosto", "nombre", colaborador.centrocosto_idcentrocosto);
			ViewBag.idcentrocosto = new SelectList(db.CentroCosto.Where(c => c.centrocosto_idcentrocosto == colaborador.centrocosto_idcentrocosto), "idcentrocosto", "nombre", colaborador.idcentrocosto);
            return View(colaborador);
        }

        public ActionResult Edit(int id = 0)
        {
            Colaborador colaborador = db.Colaborador.Find(id);
			colaborador.centrocosto_idcentrocosto = colaborador.centrocosto.centrocosto_idcentrocosto ?? default(int);
			colaborador.aprobador_nombre = (colaborador.aprobador == null ? "" : colaborador.aprobador.nombre);
            if (colaborador == null)
            {
                return HttpNotFound();
            }
            ViewBag.idcolaboradortipo = new SelectList(db.ColaboradorTipo, "idcolaboradortipo", "descripcion", colaborador.idcolaboradortipo);
			ViewBag.centrocosto_idcentrocosto = new SelectList(db.CentroCosto.Where(c => c.centrocosto_idcentrocosto == null), "idcentrocosto", "nombre", colaborador.centrocosto_idcentrocosto);
            ViewBag.idcentrocosto = new SelectList(db.CentroCosto.Where(c => c.centrocosto_idcentrocosto == colaborador.centrocosto_idcentrocosto), "idcentrocosto", "nombre", colaborador.idcentrocosto);
            return View(colaborador);
        }

		public ActionResult Edit2(int id = 0)
		{
			Colaborador colaborador = db.Colaborador.Find(id);
			if (colaborador == null)
			{
				TempData["alerta"] = "El colaborador fue eliminado";
				return RedirectToAction("Index", "Home", new { Area = "" });
			}
			else if (colaborador.aprobado == true)
			{
				TempData["alerta"] = "El colaborador ya fue aprobado";
				return RedirectToAction("Index", "Home", new { Area = "" });
			}
			else if (colaborador.aprobado == false)
			{
				TempData["alerta"] = "El colaborador ya fue rechazado";
				return RedirectToAction("Index", "Home", new { Area = "" });
			}
			colaborador.centrocosto_idcentrocosto = colaborador.centrocosto.centrocosto_idcentrocosto ?? default(int);
			ViewBag.idcolaboradortipo = new SelectList(db.ColaboradorTipo, "idcolaboradortipo", "descripcion", colaborador.idcolaboradortipo);
			ViewBag.centrocosto_idcentrocosto = new SelectList(db.CentroCosto.Where(c => c.centrocosto_idcentrocosto == null), "idcentrocosto", "nombre", colaborador.centrocosto_idcentrocosto);
			ViewBag.idcentrocosto = new SelectList(db.CentroCosto.Where(c => c.centrocosto_idcentrocosto == colaborador.centrocosto_idcentrocosto), "idcentrocosto", "nombre", colaborador.idcentrocosto);
			return View(colaborador);
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public ActionResult Edit(Colaborador colaborador)
        {
			bool cargafoto = false;
			colaborador.codigo = colaborador.codigo.Replace(" ", "");
			int i = db.Colaborador.Where(c => c.codigo.Equals(colaborador.codigo) && c.idcolaborador != colaborador.idcolaborador).Count();
			if (i > 0)
			{
				ModelState.AddModelError("codigo", "El código ya fue registrado");
			}
			if (colaborador.idcolaboradortipo != 0)
				if (colaborador.idcolaboradortipo == 1 && !colaborador.codigo[0].Equals('M'))
					ModelState.AddModelError("codigo", "El prefijo del código debería ser M");
				else if (colaborador.idcolaboradortipo == 2 && !colaborador.codigo[0].Equals('X'))
					ModelState.AddModelError("codigo", "El prefijo del código debería ser X");
			if (colaborador.file != null && colaborador.file.ContentLength > 0)
			{
				var validImageTypes = new string[]
				{
					"image/gif",
					"image/jpeg",
					"image/pjpeg",
					"image/png"
				};
				if (!validImageTypes.Contains(colaborador.file.ContentType))
				{
					ModelState.AddModelError("file", "Seleccione una imagen GIF, JPG o PNG");
				}
				else
				{
					MemoryStream target = new MemoryStream();
					colaborador.file.InputStream.CopyTo(target);
					colaborador.foto = target.ToArray();
					cargafoto = true;
				}
			}
            if (ModelState.IsValid)
            {
				db.Entry(colaborador).State = EntityState.Modified;
				db.Entry(colaborador).Property("foto").IsModified = cargafoto;
				db.SaveChanges();

				if (colaborador.aprobado == null)
				{
					TempData["alerta"] = "Solicitud de confirmación enviada con éxito";
					string correo = db.Colaborador.Find(colaborador.colaborador_idcolaborador).correo;
					string titulo = "Solicitud de aprobación";
					string mensaje = "Solicitud de aprobación de nuevo colaborador<br/><br/><a href=\""
						+ (Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/ADMIN/Colaborador/Edit2/" + colaborador.idcolaborador)
						+ "\" target=\"_blank\">ENLACE</a>";
					if (Correo.sepuede)
						Correo.enviar2(correo, titulo, mensaje);
				}
				else
					TempData["alerta"] = "Colaborador actualizado con éxito";
				
				return RedirectToAction("Index");
            }
            ViewBag.idcolaboradortipo = new SelectList(db.ColaboradorTipo, "idcolaboradortipo", "descripcion", colaborador.idcolaboradortipo);
			ViewBag.centrocosto_idcentrocosto = new SelectList(db.CentroCosto.Where(c => c.centrocosto_idcentrocosto == null), "idcentrocosto", "nombre", colaborador.centrocosto_idcentrocosto);
			ViewBag.idcentrocosto = new SelectList(db.CentroCosto.Where(c => c.centrocosto_idcentrocosto == colaborador.centrocosto_idcentrocosto), "idcentrocosto", "nombre", colaborador.idcentrocosto);
            return View(colaborador);
        }

		[HttpPost]
		[ValidateAntiForgeryToken]
		[HandleError]
        public ActionResult Edit2(Colaborador colaborador)
		{
			db.Entry(colaborador).State = EntityState.Modified;
			db.Entry(colaborador).Property("foto").IsModified = false;
			db.SaveChanges();
			Colaborador rolando = db.Colaborador.Where(x => x.usuario.idrol == Rol.CONSULTOR && x.aprobado == true && !x.desactivado).FirstOrDefault();
			string correo = rolando == null ? "" : rolando.correo;
			string titulo = "Petición de aprobación";
			string mensaje = "La petición de aprobación del colaborador " + colaborador.nombre + " fue " + (colaborador.aprobado == true ? "aceptada" : "rechazada");
			if (Correo.sepuede)
				Correo.enviar2(correo, titulo, mensaje);
			TempData["alerta"] = "Petición de aprobación guardada con éxito";
			return RedirectToAction("Index", "Home", new { Area = "" });
		}

        [HandleError]
        public ActionResult Delete(int id = 0)
		{
			Colaborador colaborador = db.Colaborador.Find(id > 0 ? id : -id);
			string mensaje = null;
			if (id > 0)
			{
				mensaje = "El colaborador " + colaborador.nombre + " de su unidad de negocio fue eliminado.";
				TempData["alerta"] = "El colaborador fue eliminado satisfactoriamente";
				db.Colaborador.Remove(colaborador);
			}
			else
			{
				colaborador.desactivado = !colaborador.desactivado;
				mensaje = "El colaborador " + colaborador.nombre + " de su unidad de negocio fue " + (colaborador.desactivado ? "desactivado" : "activado");
				TempData["alerta"] = "El colaborador fue " + (colaborador.desactivado ? "desactivado" : "activado") + " satisfactoriamente";
				db.Entry(colaborador).State = EntityState.Modified;
			}
			db.SaveChanges();

			string correo = db.Colaborador.Find(colaborador.colaborador_idcolaborador).correo;
			string titulo = "Actualización de colaborador";
			if (Correo.sepuede)
				Correo.enviar2(correo, titulo, mensaje);
			return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

		[HttpPost]
		public JsonResult ColaboradorList(string term)
		{
			var data = db.Colaborador.Select(x => new
			{
				idcolaborador = x.idcolaborador,
				codigo = x.codigo,
				nombre = x.nombre
			}).Where(x => (x.codigo.Contains(term) || x.nombre.Contains(term)));
			return Json(data, JsonRequestBehavior.AllowGet);
		}

		public JsonResult CentroCostoList(int id)
		{
			return Json(new SelectList(db.CentroCosto.Where(c => c.centrocosto_idcentrocosto == id), "idcentrocosto", "nombre"), JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		public ContentResult Aprobador(int id)
		{
			Colaborador o = db.CentroCosto.Find(id).propietario;
			return Content(o.idcolaborador + "|" + o.nombre, "text");
		}

		[HttpGet]
		[AllowAnonymous]
		public ActionResult ViewImage(int id)
		{
			byte[] foto = (id == 0 ? null : db.Colaborador.Find(id).foto);
			if (foto == null)
			{
				foto = new byte[7118];
				for (int i = 0; i < 14236; i += 2)
					foto[i / 2] = Convert.ToByte("89504E470D0A1A0A0000000D4948445200000080000000800806000000C33E61CB0000000467414D410000AFC837058AE90000001974455874536F6674776172650041646F626520496D616765526561647971C9653C00001B604944415478DAEC5D798C1BE7757FCBFBBE975CEE7D48BB5AC9922C47B622DB7122C7711DC7496C14E905C3859B204DEBB46812F48F023D81A268D02269DD140DD2382E8A066D63A0B573BA4AE2238E6C4B3E74AE56D76AB507B9BC8F21B9BCC9BE6F969479CCF09BD91DEE61CD5B7C187239E41CEFFD7EEFF8DECCF454AB5590E5D615857C0A640390453600596403904536005964039045360059640390E596911EF9146C5B9D342EB95ED7A5CA336403D8E6CA6D7DCD37142DCB469D556AA38CA3D4F0BE22D40854B24E360DB55C4A6D5570EBEBFA50B67C0E0D8A2FE228E0C8D75E97C4B0409301C8F3021C9A459100B57CCA55F2BC5772BC6EFC5FB5A668A2F02C8E4CEDB3AC5857A09215DB15D476522C97525B878A63A96AF83E417FF1B1C71EB3ECDBB7AFE7D9679F3DEDF3F9223585D799014433808C5AD1CA6D7DDD09B92A1E05AB5ADFBB5C2EFD430F3D3468B1580CA8608FD56A350D0C0CD87169C4CFAC369BCD8C62AA1F2FC330BFF7F4D34F9FAAB142A161BFC531C07677000A7ED4F229592142B9DD44ED4D451F3B76CCE3F57A2D070E1EEC331A8DBAC9C949AF5AAD568E8F8FF7A9542A657F7F7F9F9073D1A82BA7CB35828B8B38566B4359730F3B87016ACAEDA4E0F5A2564941301F72551C0AE6456E1DB50E87C3383935E5AEA316DF9BEC76BB05975654B8A11BE70E0D6A1017461CDADA3E298466782AE8320528145D43ADB243B4BC51D436BD7FF0C107BDA860D3A1437778B55AAD6E6A6AAA8FA0766262A29FA01615E006B1B095501C76870717C4B8341B3200B1FBA754488ADAFA7B29144B532E3B906E8DC7EEBFDF4B503B3539E5264845655A91522D04C14251BBD5AE13F7D55E43BFBAC100D61F042AC5A1562150B90A4A7EAB14A06095004A66C7C71F7EB81F8325C31D35D44E4F4FF7AB10B56363637DA250BB03C4EDF15839D02F8A01D82F34205AC141B96214DB75D4BADD6EF3F8D8782F8A055F5BEAA845AAB6E9516EA594961C37472C23D8005A03262E3FA89400B554E5D6517BF8F09D0388520D412DF1B5A3885A8D46A3F2783CBD5BE86AB7AF01605AD8701E15EB3500558D46882FD1D5969A066A51095030A772474747CDF7DC7B2F497FACC3C323CE3A6A7BDD6E2BE6B3467CEFD0E9745AB9EEB83EC138C568B158B40CC3D475D2E8A2AB3403A8A397289A50A7A9368CB5C852D71060B429F7539FFAF420C9670FDFB986DADB6EBB6D40AD522B8786873D62500B7215BA49D2A52A5C65CA102F54E15EB71A348A4E29B4A267FF81038E13BFFCA572BD0CA0AA299928DE466A0BF8838348C5638854D7E0E0A007D16A27A9509FA7CF6E301AF542512BEBB55D886289822F264A50A8009C8E97A08C27EA3C2EB3F8E252B2B992FBEE27ECB0DBA2ECF89B13E31376348056772D98015435A49B5F3CFED3CF7EF4A30F7C41569378618A55984B952192AFC072A602FED50A04736BAFC9FFC867641DB1E2CF56E806B06B97932308EC11C300AC0B0887433D5519B76D42509943749E8BAFA1F67C6D398328CED4E8BA5B12CC9651239D8BB67DDE3E7B4B9C263A08243E5E77F5EAD5D55B49B1C4C72EA4CB78922BB0826329830846D42E206AE3885AA258A2609A74B3B3663943372E125873646C8218001A33814BB397D2EF97B680BA8F3D132B42A1BCF63EC7FAD8124BC5D753E51D711C01344C9A4E7ADD6C31685D2EA0DE38C07EE1E2CC4C7ABB9F90351F5B8615D6C796590427F2A8D0342AB6B0A660A2E8AD44ADA406B05AA2AE63B7D94D1B61807AC05EBD70E17CBA502C96D46AF596CC149E8F17A1487C2CA29644C49751991944EB35A684745D61957DABC9121A36CD9CAD369BA52506508861807A1709DB5C986218C6E1743AA43C8810F1B1AB6516B904C1F32982D60AAB589212CDB2415577503B6151C12E1CE3661598D53DE0D1BF175193ED1771BBC4F793D7734C69DB1940384B377A9BDD6E6D610041C5A04603B8D95D9A4826D376110670365A6495378BE8250113399144B944C9D15C4550102325259B50C91FF1EAE0DE3E2DECB6AA40D1D3C9389AD32BE23AC8F1BCECCFC1E94851905176DD001038240E5576380E1532F6C8E8A879E1C68D753140B5910122E170726C6CBCE3179FBFB10A5F782D0AE56D14304ED9D4F0D0901EEEF16841AD5C9F39E9F07B47DC1A7610237EEE7A068E2FE5B6D41048001B451670EB3BD702A6A7F7DAD1005AE703C433402C164BD30E97A0AA8227653B04520EAD023EBFD78C4A6B284C4A90CA5891493E376582CF8C1BE17FD0105E5CCC6E9921F891057A29063039B5C7F5E24F7EBC6E06B86900C1503049FBA2D780DBA856B65CF90F0E19E009549251D5BDABDC88213C89DBF8B0570B5F3B9B045F66F3E384908038606070C02E3613A847FA8D0C50F62D2F476976DE8706D0B3850503935A017F7CC806FB9D9B3789386E51C3DF1D75C037671878CD9FDDD4E3F5A58B88523DA518E4B67254037B4433806FD997A4E51D6E1D3180AD61001B52FE9FDE6987318B1A68D34D398C9E2E270A30972CC2751CA16C898DAAD3986BAE7D1F60D8ACC660500D873D3A2ACDEA553DF0A58356D86B53C1B72F2661B33C4298D40228DBF278FA6C2D692095161B19A05C5F5EB97239463B2E1506016E9D42508A22A51005FDC511173290AAE309B9962CC08FE6D37032C01FC05D4F146E2E5FC1E5333300075D5A7860D80877A131287BF8C1F32BB88E118DE1E9B3F14D318240865E0BB0391C269E54501003D43381D299D3EF4684EC148903C2ABC54D44BE12FEE6A80BEC3A252FF2438894676612F04E28B7AE6D9C0D67D9D16754C1DFDFEB019D8AFFFCDDDBAF870C32C9BF5E886F820B28D08361A7D3BC9E18A0D50594530C93CF6432AB064A476C2F32C066B9019275FCD1ED2E70E8F829FA47F329F8CFCB0996F6379A9D3C3E6561E99E260F8D18D1E88AF0FD39A6CB2EA04867009B7D5D3100F0148318BDA1B3010C18310EA86C8E017C66AA16F071049E25FCDFD7DF8EC0A9C0AA2405A547C62D70B44F2F3895FC2D349659648D6B897CD78E3F982A507707F565D46AB59A7C3E2FB816A0E03380782C96A2ED945BAF6233816E0FA2F85FDD6DE1DC8762B90A7FFB6608DE5AC948B2AD69BB061E9FB689528E0A63853F38E404CC16BB760E92B912325B67B029148A9EFD076FB78388F980C628B1A91A582F06751A1E3610AB7475A87AAAF0F983CE5A39B7790F2A7862BE7A3200E7421949B665410D7EE5702F2815C079C4DF3C1D660D8DEBB37E930A3E396EEEEAB9A807829DC6E4D41E519D41BC2E20140824689CB3560BE8AE0BF8E8B005834DEE88FFB9D9189C47E54B518D2406F6953BFBD840936B5B2F2D32F0F24212DEF0A5E0AFEF1BC0D451D3B6CE63BBADF0D28D243085EE6446814C01466AA92B9F0C8F8C38C404828D2EA029100C0403099AB5B98D6AD600BA3594B83B8FEEB6716EFD4278155EB812956C5BBFB1C70E7B9D3ACE6DDD48E6E0D9B32176BD7CB1045F3FE98752A5D2B61E091A1FC6F8A15BE7238899004D27DEFE011B88B83E809701823503E8242E1203743108BC7BC8CC6EA35508F53F7B2608D5724512F47FC06B844727ED9C9FAD16CBF0B537FD50C4657D5B2B4C1E5E46A47F6CBC3D567860D40CFF7B29C2C6265B9109F4BA3D8DC5206A1CD0DA10729301E6AE22BC6879272A47AFECA10627EB95FB86CC9C91F81B4B29F0A312A4503E61B1A73ED0C71BF17FE3D40A841179ADDBFADE4C04EE1E3083B1A561DFA251C221B701DEF24BDF581512540B10570C52F1B880D2D52B57E242B2200FC6018B49E927474C78220FE089ECE140FFF397C292C41E64CAF8CB47BC605473574C5FB81C85332B29CEB397C957E0951B097864B2BD6DE29E4133BCED93BE2EE06772D454D06E778A6A0EE56200D60866672E244AE57259894273038B09E92746F6BB4D0D91FF7B72319C015F3227C9369E3CE885519B963345BE80C1E5F72E043B969B5F9A8FA101B4BB8EDBDC7A3CEB15C94BC4914C414839B85E0C12749F005E062023994C241D8ECE9D41FD26359CEE422630EDD2739EFCB7961949D0FF91311B1C1BB5716E23992FC137DE5CC2CCAB738C1140442EA1310E5974CDECA55662B4AE811B899CA4E72494A6FF9ED96C3101776F20673148C1C700C4009844926D11EF345C0675570A1FA356EE887C369496E0B7B5F0E4ED5ECEDF2FA3D2FFF1F54548E54A827E6B36C45D1718B3E9243F27E1741E68FAD06875DA3E6FBF1104CE0AB61A405331281A8BA6E8A9A006A87BB58ED1CF9167936093A5FF0DFCAE41A5802FDD3DC4DB32F65FE78270399C11FC7B7351EEEB68D8FD97F89C14F1F863D9223515BCEDC04127472AD8232A0660AB81D1283592E93369242F061990424D1A451B630553F90D6FEBA923C36CE4CFC5FD279792F093CB2151D9452895C3F5DB7FCBC996C9A5778D2413B0EB3B1783C6267639D69B0636A582A16030490B3A9CE4644ADC194450CAE59B53F9D286B6F5A969371CF29A397FDB8FC6F5ED53CBA27F3F8188E4FA3D03C92CBAD03115443730D96BECB8CEC0E09043682AD85A65696280C08A3F41BD24C9A895BC358C6F1A9650E07AB7B5CF6382CFECF7707E5628A3DF7FED06640B25D1B5857C91BBECAB572ABAD232C76602B46EAD3E2F5767508F5806282D2F2DC6E885148DE4075A2A7353272186F56C8B50E617EF1E01054F87CF332797C09FCCAEABB0A4E5892572A572570C20C8D0330187D3650681570AB73240531CB078633E4E6D0D434BB7EB9510CF4AD719942B96387D344BAB20CEAF92D6B52FDE3302662D7717D1CFAF46E0F585E806D9AACA710C65D1FB2A3415A4BAE5DE5EABD06250C720F09D5327C342EE20EE346820B15A90EC20D3B9227BE7F29E967D7619C4B3CD6FDE3E00932E23A79FBE1ECBC077DF59DA1052D98092E3EB0C1E433718204A0C80F2BB76BB43706B98A25310984A31F9D54C867ABF803EB356D274A782B97884A3EE6D432A376B94827FE7AE211B3C38C97D3BC054BE08FFF4DAF53577B3817D1DB6E9387F7F055D4A37D2E3708ADE7564B6B41583040781EDC5A01493D6537A039DB5629094B2145F05B7A9BD16B01BD17C7A3941FDBED7A283CFDE35CC99A29112EDB75E9F87587AE3134A932E13E7364895B01B0C104ED15D80112D40A3D56A0AF9BC1228AD610A9A01C4A2B124CD3049262075F7CBE5107709E2F0A095FA5D122AFCE17DE360D0704F633C7FCE07E77C890DEF23B98E708FC7C4616055B8164A75A52B28B19A8342A9D2511F3D3D8A9EC93DD3368E54B0A30B6834829BD5C0443C9616E202A42E7BCEF8939C4777C7A00D3418D875FAEEE73E380A03A43ECFF17DA2F81F9CF74BB28F4746ECECBEB46EE35A38CD368E74AB3F3092A6BB81C93D7B1D1C31400FCD005A19A01C0987185AE9D16ED452D6103F9613AB6CEDBB8DDE10D5F78E3B79BF77FF642F1C1DE59EBF8A620EFDADD7E710A1950DEF1FD1FBC37BB96FEDFF0E9B3D57BB36EA9940A7313432C6D51A06420CA0DA520C8A5127848CDAAE58FAEBD7B9AF4F79747F3FA65F8AB6F5279C4678FCF008E777CAE8F8FFF91757219D2D4AB26FC4C8BC161D27FD9FBA11ED6A977454C0A490C7DB6F0301CDA15403080502D42B857B4DBAAEF8BB5F5C0DB2B373ADF66DC3A0F3917DDEA675499EFFD47DBB783B7ABFFBD63C5C0F3392EC1789311E3DD0CFB99DB716A210CFE4BADA1D1C64E8377273385D826A015CB4D0E4021617E66334BAB1627E4EF3CBEB1931A4BA93F3DC2CF0F1BD5E187318D8F5C8117EFE9E09701A359CEBBE894CF2F2A58064FBF5C45DA3E03673A77FC767FC5DBF4E2292A2BB00779F57D055425406B8323B1315E2995C466D57F2DEEF9F5982329BAB376F50492A7C1F9942E4ABE0D30706607FBF8D73C7FC184B7CE7C435C9F6E7EE71177C68979B735BEF2EC49065525D390F8D232A2006B0391C162131808A270BB8C902972FCE24CA257A6B582F6602816446FAD2672A0BFF87A8FAC4FE81B6CF5C88F83F79681FF459F49CB938E9E8FDC64B9700775F9206D26904D593472738B745BA80BFF7F63CE76792D702880BA06CC66C6E7A8680B018E046B2D4560D648B414C92DA18623768BB66F12F9C59442473F71DF65BF5BC3781FA37447E20B92AC93EDC31E4802F3FB097B791E4DFDFB8062126DB75F443CD35D2F46169BF5094D30814ADD06F61807A6F203515F4101476C9E7954A65F8D6AB971065C227578ECFF8E0EDF9B024DBFFF8BE7E78EAD8143BB1C425AF5F0BC1090C5837E33A49360B4043A3E943A3D369ED4ED258D9F9221105B45B405B6B18E90DA46DD169D27535F75D8CA5E15F5E9EAD65059DE56A3009CF211D6F749B037603FCD92307E1D7EE1CE39D4A2631C67FBC79ADABC7DE3AF2A512A4B205EAAAFB0EDCEEA2F505B4304095B31C1C0E059255E8FCE7A8F50574739C5D8CC2777E710533A1CE14E931EBE1D30787C16D5A5F7D62CC6984DF3EBA0BFEF293B7C398CBC4BB1D42F9FF70FC3CE40BA54D43FFCD06D15416683AD935B5C7452B07AB785C40B5A51A486F0D33E9BBD202D596D2CD0541AF51C2E3A8203EB1E8D5F0C8C121765CF4C5E1D52B01B886ACC037654D328A51971926DC16B86BAC17C67ACDD4FD588CA651F91720992DC056088903467B2D1DD7F17807A83790E67B70641303444321EA4DA3880BE8D9A4E70CBC32EB83442607BFF3A129306A3B3748EE1BB0B3839D02CE15D9148A6DADC2A50BF759A35242BF4DDC033DE7420CFCE34FCF43265FDAB2FB248693F44CC0E9BA79D730055F2A48630036105C5CB84EBD6D9CD3BC390C5097330B11F82B44E1EF1E9B66912B44CC3A158EB5D9BB519789D3F23B09F13C2F9E5B84EF9F5E101590764322A955FA85A29E3E3B509A43F99E1CDAE40296166E505BC3D488248B4EBD169C6C160DA21FFCEA0F4EC3270F8DC0C7F60F814EADECDAB608E53FF3CA2CF8E26BB58EADBE436A5C406B98DDC5FAB28E570A530B4164CC9C3D2DA869CE69D6A101E437F54490C99717DE9D879FCD2CC3C318F87D787A405243086094FFD2451FEB762ADBE8491A61865E74B3586DF5CE20953006A8721A41291E8D64496B18EDA651360399144A6CC909C9E40AF0DCC96BF0E3330B70FFBE01B867B7175C66FDBA8DEAEDF910BC76C90FB3FEF8961CCF90CB025A64D511B70DD44A054C209B936075CC6307954201A3E8DE69F6D8520C52501980FCDEE558B13AE550B73F4320C5A475140370F39464375356F305F821320219037613DC36E480098F15F7CD80EFB92FA820D9C14284C19182853003F33848C0D80DAA37E935E0B19AC08E0128899B9C6603D88C3AF0D88C60C6CFBC7633182881ADD0A8C56032532F145575F8FDA649A16422C1F47A3A3F70D961D66FAB0705FA626976B4A67C838EB50030DD90154821049D0AFCFD5D8856D22E4FD04BB28C6144B34147946BDAD4E327F337BBA7F7D9AECECE0864806ADBB4F07B770D8B4452D5494AD061D4C1767F5424690C2168172304A936A31E7AAD06B0186A6845850E382DA0D3A890AEADACA2A9A8DD825333BDFF76171A806A3D0CD01408C62221EA59EBB518B7F40EE262859477C710AD44794489449944A92654AE07E3278B51CB1ED34E96BEFE013B74B854BC531AD8E40222E110F5A651D66DC4004481C4DFF6A14F25C5A2418252B58AA56512444D78853D1167A73F41CFD33FD8B11620D80002FE656A2D603362803A4A0D5A542EFA72934ECB2ADB86419503032A96AE4DFAF7BD6205BBAF5EB74DB001407B0CF0DE7D037DBE38EDAC113F49E6CB4BEBAC92F5D77CEA48EF5A1035EA595B8E791CEC72D82DF016AEF2936FDF03A58B2D93F2CE08B630409597012E5D381311F24C6162049196CE2042C56EE253318072590DB5D447CFBE26FFEB73084D7D64CD8A159BC369E64805A931405B20889164A28CA2A0B4863DF1C01D502895D0E7DA5834F73B2D3268B750CCCD0F9414E6022E840BD5DB7A356DC5A04C2A95365B6DD64E1BFCE09E6159B3DB484C264BC747CA0A6600620089443C69A218802CDB4B48F5D664B668D32986B31AA8A0B072DD08D8DEC0543291964FE9CE9303878FF4F2B180100628DF2C0645C32999D1779E0C0C8FDA05B980160B68CB04C281405CF6E93B4FBC03FC770D5351E2B54AC3284523F4DE4059B69FB8FAFA6DC0739510CD0534B901FFD2625C36809D278EB5278A723687D29E2CD9E40296E6E7A2F2E9DC796277B82CC282C06AC734B03C7BEE74B42A53C08E138BDDC975DF40510CC0C6008958249BCF65B3F229DD59D272D7B026065070397D32DE0D16B82F144D26529B7711943CA418268BADF121124D71801006688A03D20C231783769890F99B5D7BF6D9A84160EBE56F0DA9E0CD62109B0A5637E52A6879483846774DD567E51A2783C433402C1266644CED3C99D8B3CF52D363B5C110A885A0F609A168442E06ED40F10C0E9396FE524D8F3755289601CA2BCBF45BC8CBB2FDC4BB6600B99A11DC6402BE8EA046697E88847F292177E6EC2C6192F18052AD22C13B69D5CA371841C7C9A05606606B01F39767A3B2FEB78714F2B97C3C1A89655733D96828184FC422E958389408057CB1F08A3F72F1EC3B4BF3576603B82AB95E8F5CE396C241EA38C53A0BA8046CA7C9055CBE70265611D01A26CBC60415192E160BA5806F31582A96CA7397667CD56AB574E1DD934BA8F0DCAB2FFE60B986E4C6517FE6235170A146F944E1E4CE92AD0C50A50681A756F2D5BBBCDAB662503A934A9B2D7267D04650BB9A4E6530A34AC6232106DF3381E585682418489E397522B8B2B490EAA0DCC6D7659E516A3184BA31D45F97B95D406706B81907A41289946C00FCA8F52FCE07086AE7AF5EF297CBA5C2C5336FFBD2C9C4EA899FBFE8E7506CAB428528B8D2F2BAD2F2BAD2620C8D4651690802ABB434903313C0A082F1C2E82DA35832FF918845136926994EC6A24C24B8926412F1946FE17A381E09A7DE3AF1F24A24B09211A15CDAA8702C69A3DAA8D80EFF6F1C203606600F0A292BF57E512ED26E08515AF6DDB8EE2F158BE585B92B8102F2F4E5F3A75712D148FAE4AB3F5BE1401117254B815A9A625B15DCA44C9EF71DB04DCF025A19801C6001AD7EDBB78661A0B48A284DA690AD98783C1D09B1A86516E7AE84709979E3E5E3CB64765384725B955C1189DA720B323B29976F3429F5A43FDFA48523FD5AD1E749880B8006E593A0820431E1EA96A27625502A95CAFE05F4B5A5627971EEEA4A369BC95D9B3917404533EF9C7835C8A3CCF550B210C5563AD06E2705F3A2F6CD16E5F22173A322D6059008321B09FA03DD502C46C6E91483A08D45926986C98403BE38F1BBD7AFCC06332926F3DAF11F2EE1322F223A2E51A898164455042A57306A85287633452570BD3A0390939F092C2F2E8ADD50D0BFBC5246D4E27783E8634BCBF373814C9A59BD7EF96210FF973C7BEA4458A062378A5A3EC50AF1B35CC81585DAED2642B3804A9DFE494161E6F45BD7EBEB222219846D26110DC733A95436E85B8CAE66D2ABD72E9E5FC96557B322512B2688EAA458A950CBBE7E43A062776281542C0390A0498988F5FDF9EF3FF1EB3F7DFEBF4985895C8C4F2EEDAD3FEF5D4CC1A22C808ECB1D22E30DA3F68D1D885A29A5E98641783238573ADACF3E7897180B79260BB90D08995932D65E93FFD51B0DAB22E8B82C22EDE91A6ADF4F72741D5980D0BBA0D51B095535B493A1AD2D1BAF39AF6E066A41BEE6B83B0C4059AFA7C1101A6F3FDAFA48D275A35656ECF635804623687CDDC31307C9A8DD29065095AFF4B8A545219F02D90064910D4016D90064910D4016D90064B9C5E4FF051800AD01AB8D7D9F58A30000000049454E44AE426082".Substring(i, 2), 16);
			}
			return File(foto, "image", string.Format("{0}", id));
		}

    }
}