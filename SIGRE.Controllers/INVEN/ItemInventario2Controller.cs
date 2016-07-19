using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIGRE.Models;
using System.Text;
using System.Linq.Expressions;

namespace SIGRE.Controllers.INVEN
{
	[Authorize]
	public class ItemInventario2Controller : Controller
	{
		private BDSIGRE db = new BDSIGRE();

		public ActionResult Index(int sesion = 1)
		{
			List<SelectListItem> ltipooperacion = new List<SelectListItem>();
			ltipooperacion.Add(new SelectListItem { Text = "Disponible", Value = "D" });
			ltipooperacion.Add(new SelectListItem { Text = "Prestado", Value = "P" });
			ltipooperacion.Add(new SelectListItem { Text = "Asignado", Value = "A" });
			ltipooperacion.Add(new SelectListItem { Text = "De baja", Value = "B" });
			IQueryable<ItemInventario> iteminventario;
			bool ignora = false;
			if (sesion == 0 || (Session["p_idcategoriainventariotipo"] == null && Session["p_idcategoriainventario"] == null && Session["p_tipooperacion"] == null && Session["p_idcategoriadetalle"] == null && Session["p_valorbusqueda"] == null))
				ignora = true;
			if (ignora)
			{
				Session["p_idcategoriainventariotipo"] = null;
				Session["p_idcategoriainventario"] = null;
				Session["p_tipooperacion"] = null;
				Session["p_idcategoriadetalle"] = null;
				Session["p_valorbusqueda"] = null;
				ViewBag.idcategoriainventariotipo = new SelectList(db.CategoriaInventarioTipo, "idcategoriainventariotipo", "descripcion");
				ViewBag.idcategoriainventario = new SelectList(db.CategoriaInventario.Where(c => c.idcategoriainventariotipo == 0), "idcategoriainventario", "nombre");
				ViewBag.idcategoriadetalle = new SelectList(db.CategoriaDetalle.Where(c => c.idcategoriainventario == 0), "idcategoriadetalle", "nombre");
				ViewBag.tipooperacion = new SelectList(ltipooperacion, "Value", "Text");
				iteminventario = db.ItemInventario.Include(i => i.categoriainventario).Include(i => i.colaborador);
			}
			else
			{
				int idcategoriainventariotipo = (int) Session["p_idcategoriainventariotipo"];
				int idcategoriainventario = (int) Session["p_idcategoriainventario"];
				string tipooperacion = (string) Session["p_tipooperacion"];
				int idcategoriadetalle = (int) Session["p_idcategoriadetalle"];
				string valorbusqueda = (string)Session["p_valorbusqueda"];
				ViewBag.idcategoriainventariotipo = new SelectList(db.CategoriaInventarioTipo, "idcategoriainventariotipo", "descripcion", idcategoriainventariotipo);
				ViewBag.idcategoriainventario = new SelectList(db.CategoriaInventario.Where(c => c.idcategoriainventariotipo.Equals(idcategoriainventariotipo)), "idcategoriainventario", "nombre", idcategoriainventario);
				ViewBag.idcategoriadetalle = new SelectList(db.CategoriaDetalle.Where(c => c.idcategoriainventario.Equals(idcategoriainventario)), "idcategoriadetalle", "nombre", idcategoriadetalle);
				ViewBag.tipooperacion = new SelectList(ltipooperacion, "Value", "Text", tipooperacion);
				iteminventario = db.ItemInventario.Where(
				s => (idcategoriainventariotipo.Equals(0) || s.categoriainventario.idcategoriainventariotipo.Equals(idcategoriainventariotipo))
					&& (idcategoriainventario.Equals(0) || s.idcategoriainventario.Equals(idcategoriainventario))
					&& (tipooperacion.Equals("") || s.tipooperacion.Equals(tipooperacion))
					&& (idcategoriadetalle.Equals(0) || s.liteminventariodetalle.Any(t => t.idcategoriadetalle.Equals(idcategoriadetalle)))
					&& (valorbusqueda.Equals("") || s.liteminventariodetalle.Any(t => t.valorbusqueda.Contains(valorbusqueda.Trim())))
				).Include(i => i.categoriainventario).Include(i => i.colaborador);
			}
			return View(iteminventario.ToList());
		}

		[HttpPost]
		public ActionResult Index(int idcategoriainventariotipo = 0, int idcategoriainventario = 0, string tipooperacion = "", int idcategoriadetalle = 0, string valorbusqueda = "")
		{
			Session["p_idcategoriainventariotipo"] = idcategoriainventariotipo;
			Session["p_idcategoriainventario"] = idcategoriainventario;
			Session["p_tipooperacion"] = tipooperacion;
			Session["p_idcategoriadetalle"] = idcategoriadetalle;
			Session["p_valorbusqueda"] = valorbusqueda;
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
				).Include(i => i.categoriainventario).Include(i => i.colaborador);
			List<ItemInventario> l = iteminventario.ToList();
			if (l.Count() == 0)
				TempData["alerta"] = "No se encontraron resultados para la búsqueda";
			return View(l);
		}

		public ActionResult Create(int id = 0)
		{
			ViewBag.idcategoriainventario = new SelectList(db.CategoriaInventario.Where(c => !c.desactivado && c.lcategoriadetalle.Count() > 0), "idcategoriainventario", "nombre", id);
			ItemInventario iteminventario = new ItemInventario();
			iteminventario.tipooperacion = ItemInventario.DISPONIBLE;
			iteminventario.idcategoriainventario = id;
			iteminventario.liteminventariodetalle = new List<ItemInventarioDetalle>();
			List<CategoriaDetalle> l = db.CategoriaDetalle.Where(c => c.idcategoriainventario.Equals(iteminventario.idcategoriainventario)).ToList();
			foreach (CategoriaDetalle o in l)
				iteminventario.liteminventariodetalle.Add(new ItemInventarioDetalle() { idcategoriadetalle = o.idcategoriadetalle, categoriadetalle = o });
			return View(iteminventario);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(ItemInventario iteminventario, FormCollection collection)
		{
			if (iteminventario.descripcion != null)
			{
				iteminventario.descripcion = iteminventario.descripcion.Replace("  ", " ").Trim();
				if (db.ItemInventario.Where(c => c.descripcion.Equals(iteminventario.descripcion)).Count() > 0)
					ModelState.AddModelError("descripcion", "La descripción del ítem debe ser única");
				else if (!Halp.tieneLetra(iteminventario.descripcion))
					ModelState.AddModelError("descripcion", "La descripción debería tener al menos una letra");
			}
			if (iteminventario.idcolaborador == null && iteminventario.colaborador_nombre != null)
				ModelState.AddModelError("colaborador_nombre", "El colaborador no existe, elija uno de la lista");

			string key = null;
			int idcd = 0;
			string vc = "";
			DateTime? vf = null;
			int? ve = 0;
			decimal? vd = 0;
			for (int i = 0; i < iteminventario.liteminventariodetalle.Count(); i++)
			{
				idcd = iteminventario.liteminventariodetalle[i].idcategoriadetalle;
				if (iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoformato.tipo == TipoDatoFormato.CADENA)
				{
					vc = iteminventario.liteminventariodetalle[i].valorcadena;
					iteminventario.liteminventariodetalle[i].valorbusqueda = vc;
					Expression<Func<ItemInventario, string>> expression = x => x.liteminventariodetalle[i].valorcadena;
					key = ExpressionHelper.GetExpressionText(expression);
					if (iteminventario.liteminventariodetalle[i].categoriadetalle.obligatorio && (vc == null || vc == ""))
						ModelState.AddModelError(key, "Ingrese " + iteminventario.liteminventariodetalle[i].categoriadetalle.nombre);
					else if (vc != null && vc.Length > iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoformato.longitud)
						ModelState.AddModelError(key, "Longitud de " + iteminventario.liteminventariodetalle[i].categoriadetalle.nombre + " no válida");
					else if (vc != null && vc != "" && iteminventario.liteminventariodetalle[i].categoriadetalle.idtipodato == TipoDato.TEXTO && !Halp.esTexto(vc))
						ModelState.AddModelError(key, "El campo no respeta el formato para textos");
					else if (vc != null && vc != "" && iteminventario.liteminventariodetalle[i].categoriadetalle.idtipodato == TipoDato.ALFANUMERICO && !Halp.esAlfanumerico(vc))
						ModelState.AddModelError(key, "El campo no respeta el formato para alfanuméricos");
					else if (iteminventario.liteminventariodetalle[i].categoriadetalle.identificador && 0 < db.ItemInventarioDetalle.Where(c => c.idcategoriadetalle == idcd && c.categoriadetalle.identificador && c.valorcadena == vc).Count())
						ModelState.AddModelError(key, "El campo " + iteminventario.liteminventariodetalle[i].categoriadetalle.nombre + " debe ser único");
					else if (iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo != null && iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo.idoperador != null && vc != null)
						switch (iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo.idoperador)
						{
							case TipoDatoOperador.DIFERENTE:
								if (vc.ToUpper() == iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo.valorcadena.ToUpper())
									ModelState.AddModelError(key, "El campo " + iteminventario.liteminventariodetalle[i].categoriadetalle.nombre + " debe ser " + iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo.etiqueta);
								break;
							case TipoDatoOperador.IGUAL:
								if (vc.ToUpper() != iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo.valorcadena.ToUpper())
									ModelState.AddModelError(key, "El campo " + iteminventario.liteminventariodetalle[i].categoriadetalle.nombre + " debe ser " + iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo.etiqueta);
								break;
							case TipoDatoOperador.LIKE:
								if (!((vc.ToUpper()).Contains(iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo.valorcadena.ToUpper())))
									ModelState.AddModelError(key, "El campo " + iteminventario.liteminventariodetalle[i].categoriadetalle.nombre + " debe ser " + iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo.etiqueta);
								break;
						}
				}
				else if (iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoformato.tipo == TipoDatoFormato.FECHA)
				{
					vf = iteminventario.liteminventariodetalle[i].valorfecha;
					if (vf != null)
						iteminventario.liteminventariodetalle[i].valorbusqueda = vf.ToString().Substring(0, 10);
					Expression<Func<ItemInventario, DateTime?>> expression = x => x.liteminventariodetalle[i].valorfecha;
					key = ExpressionHelper.GetExpressionText(expression);
					if (iteminventario.liteminventariodetalle[i].categoriadetalle.obligatorio && vf == null)
						ModelState.AddModelError(key, "Ingrese " + iteminventario.liteminventariodetalle[i].categoriadetalle.nombre);
					else if (iteminventario.liteminventariodetalle[i].categoriadetalle.identificador && 0 < db.ItemInventarioDetalle.Where(c => c.idcategoriadetalle == idcd && c.categoriadetalle.identificador && c.valorfecha == vf).Count())
						ModelState.AddModelError(key, "El campo " + iteminventario.liteminventariodetalle[i].categoriadetalle.nombre + " debe ser único");
					else if (iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo != null && iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo.idoperador != null && vf != null)
						switch (iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo.idoperador)
						{
							case TipoDatoOperador.DIFERENTE:
								if (vf == DateTime.Today)
									ModelState.AddModelError(key, "El campo " + iteminventario.liteminventariodetalle[i].categoriadetalle.nombre + " debe ser " + iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo.etiqueta);
								break;
							case TipoDatoOperador.IGUAL:
								if (vf != DateTime.Today)
									ModelState.AddModelError(key, "El campo " + iteminventario.liteminventariodetalle[i].categoriadetalle.nombre + " debe ser " + iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo.etiqueta);
								break;
							case TipoDatoOperador.MAYOR:
								if (vf <= DateTime.Today)
									ModelState.AddModelError(key, "El campo " + iteminventario.liteminventariodetalle[i].categoriadetalle.nombre + " debe ser " + iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo.etiqueta);
								break;
							case TipoDatoOperador.MENOR:
								if (vf >= DateTime.Today)
									ModelState.AddModelError(key, "El campo " + iteminventario.liteminventariodetalle[i].categoriadetalle.nombre + " debe ser " + iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo.etiqueta);
								break;
							case TipoDatoOperador.MAYOR_IGUAL:
								if (vf < DateTime.Today)
									ModelState.AddModelError(key, "El campo " + iteminventario.liteminventariodetalle[i].categoriadetalle.nombre + " debe ser " + iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo.etiqueta);
								break;
							case TipoDatoOperador.MENOR_IGUAL:
								if (vf > DateTime.Today)
									ModelState.AddModelError(key, "El campo " + iteminventario.liteminventariodetalle[i].categoriadetalle.nombre + " debe ser " + iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo.etiqueta);
								break;
						}
				}
				else if (iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoformato.tipo == TipoDatoFormato.ENTERO)
				{
					ve = iteminventario.liteminventariodetalle[i].valorentero;
					iteminventario.liteminventariodetalle[i].valorbusqueda = ve.ToString();
					Expression<Func<ItemInventario, int?>> expression = x => x.liteminventariodetalle[i].valorentero;
					key = ExpressionHelper.GetExpressionText(expression);
					if (iteminventario.liteminventariodetalle[i].categoriadetalle.obligatorio && ve == null)
						ModelState.AddModelError(key, "Ingrese " + iteminventario.liteminventariodetalle[i].categoriadetalle.nombre);
					else if (iteminventario.liteminventariodetalle[i].categoriadetalle.identificador && 0 < db.ItemInventarioDetalle.Where(c => c.idcategoriadetalle == idcd && c.categoriadetalle.identificador && c.valorentero == ve).Count())
						ModelState.AddModelError(key, "El campo " + iteminventario.liteminventariodetalle[i].categoriadetalle.nombre + " debe ser único");
					else if (iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo != null && iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo.idoperador != null && ve != null)
						switch (iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo.idoperador)
						{
							case TipoDatoOperador.DIFERENTE:
								if (ve == iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo.valorentero)
									ModelState.AddModelError(key, "El campo " + iteminventario.liteminventariodetalle[i].categoriadetalle.nombre + " debe ser " + iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo.etiqueta);
								break;
							case TipoDatoOperador.IGUAL:
								if (ve != iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo.valorentero)
									ModelState.AddModelError(key, "El campo " + iteminventario.liteminventariodetalle[i].categoriadetalle.nombre + " debe ser " + iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo.etiqueta);
								break;
							case TipoDatoOperador.MAYOR:
								if (ve <= iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo.valorentero)
									ModelState.AddModelError(key, "El campo " + iteminventario.liteminventariodetalle[i].categoriadetalle.nombre + " debe ser " + iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo.etiqueta);
								break;
							case TipoDatoOperador.MENOR:
								if (ve >= iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo.valorentero)
									ModelState.AddModelError(key, "El campo " + iteminventario.liteminventariodetalle[i].categoriadetalle.nombre + " debe ser " + iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo.etiqueta);
								break;
							case TipoDatoOperador.MAYOR_IGUAL:
								if (ve < iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo.valorentero)
									ModelState.AddModelError(key, "El campo " + iteminventario.liteminventariodetalle[i].categoriadetalle.nombre + " debe ser " + iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo.etiqueta);
								break;
							case TipoDatoOperador.MENOR_IGUAL:
								if (ve > iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo.valorentero)
									ModelState.AddModelError(key, "El campo " + iteminventario.liteminventariodetalle[i].categoriadetalle.nombre + " debe ser " + iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo.etiqueta);
								break;
						}
				}
				else if (iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoformato.tipo == TipoDatoFormato.DECIMAL)
				{
					vd = iteminventario.liteminventariodetalle[i].valordecimal;
					iteminventario.liteminventariodetalle[i].valorbusqueda = vd.ToString();
					Expression<Func<ItemInventario, decimal?>> expression = x => x.liteminventariodetalle[i].valordecimal;
					key = ExpressionHelper.GetExpressionText(expression);
					if (iteminventario.liteminventariodetalle[i].categoriadetalle.obligatorio && vd == null)
						ModelState.AddModelError(key, "Ingrese " + iteminventario.liteminventariodetalle[i].categoriadetalle.nombre);
					else if (iteminventario.liteminventariodetalle[i].categoriadetalle.identificador && 0 < db.ItemInventarioDetalle.Where(c => c.idcategoriadetalle == idcd && c.categoriadetalle.identificador && c.valordecimal == vd).Count())
						ModelState.AddModelError(key, "El campo " + iteminventario.liteminventariodetalle[i].categoriadetalle.nombre + " debe ser único");
					else if (vd != null && BitConverter.GetBytes(decimal.GetBits((decimal)vd)[3])[2] > iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoformato.escala)
						ModelState.AddModelError(key, "El campo no debe tener más de " + iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoformato.escala + " decimal(es)");
					else if (iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo != null && iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo.idoperador != null && vd != null)
						switch (iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo.idoperador)
						{
							case TipoDatoOperador.DIFERENTE:
								if (vd == iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo.valorentero)
									ModelState.AddModelError(key, "El campo " + iteminventario.liteminventariodetalle[i].categoriadetalle.nombre + " debe ser " + iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo.etiqueta);
								break;
							case TipoDatoOperador.IGUAL:
								if (vd != iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo.valorentero)
									ModelState.AddModelError(key, "El campo " + iteminventario.liteminventariodetalle[i].categoriadetalle.nombre + " debe ser " + iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo.etiqueta);
								break;
							case TipoDatoOperador.MAYOR:
								if (vd <= iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo.valorentero)
									ModelState.AddModelError(key, "El campo " + iteminventario.liteminventariodetalle[i].categoriadetalle.nombre + " debe ser " + iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo.etiqueta);
								break;
							case TipoDatoOperador.MENOR:
								if (vd >= iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo.valorentero)
									ModelState.AddModelError(key, "El campo " + iteminventario.liteminventariodetalle[i].categoriadetalle.nombre + " debe ser " + iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo.etiqueta);
								break;
							case TipoDatoOperador.MAYOR_IGUAL:
								if (vd < iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo.valorentero)
									ModelState.AddModelError(key, "El campo " + iteminventario.liteminventariodetalle[i].categoriadetalle.nombre + " debe ser " + iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo.etiqueta);
								break;
							case TipoDatoOperador.MENOR_IGUAL:
								if (vd > iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo.valorentero)
									ModelState.AddModelError(key, "El campo " + iteminventario.liteminventariodetalle[i].categoriadetalle.nombre + " debe ser " + iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo.etiqueta);
								break;
						}
				}
			}

			if (ModelState.IsValid)
			{
				bool ok = true;
				if (iteminventario.tipooperacion == "D" && iteminventario.idcolaborador != null && iteminventario.idcolaborador != 0)
				{
					string mensaje = mensajea2(iteminventario.iditeminventario, iteminventario.idcategoriainventario, iteminventario.idcolaborador, iteminventario.prestamo);
					if (mensaje.Split('|')[0] == "0")
					{
						TempData["alerta"] = mensaje.Split('|')[1];
						ok = false;
					}
					else if (mensaje.Split('|')[0] == "2" && collection["forzar"] == "0")
					{
						TempData["confirma"] = mensaje.Split('|')[1] + "|$('#forzar').val('1'); $('#form1').submit();";
						ok = false;
					}
				}
				if (ok)
				{
					if (iteminventario.idcolaborador == null)
						iteminventario.tipooperacion = "D";
					else
						if (iteminventario.prestamo)
							iteminventario.tipooperacion = "P";
						else
							iteminventario.tipooperacion = "A";
					ItemInventario o = new ItemInventario() { idcategoriainventario = iteminventario.idcategoriainventario, idcolaborador = iteminventario.idcolaborador, descripcion = iteminventario.descripcion, tipooperacion = iteminventario.tipooperacion };
					db.ItemInventario.Add(o);
					db.SaveChanges();
					foreach (ItemInventarioDetalle p in iteminventario.liteminventariodetalle)
					{
						db.ItemInventarioDetalle.Add(new ItemInventarioDetalle() { iditeminventario = o.iditeminventario, idcategoriadetalle = p.idcategoriadetalle, valorcadena = p.valorcadena, valorentero = p.valorentero, valordecimal = p.valordecimal, valorfecha = p.valorfecha, valorbusqueda = p.valorbusqueda });
						db.SaveChanges();
					}
					CategoriaInventario categoriainventario = db.CategoriaInventario.Find(iteminventario.idcategoriainventario);
					categoriainventario.utilizada = true;
					db.Entry(categoriainventario).State = EntityState.Modified;
					db.SaveChanges();
					TempData["alerta"] = "Ítem de inventario registrado con éxito";
					return RedirectToAction("Index");
				}
			}
			ViewBag.idcategoriainventario = new SelectList(db.CategoriaInventario.Where(c => !c.desactivado && c.lcategoriadetalle.Count() > 0), "idcategoriainventario", "nombre", iteminventario.idcategoriainventario);
			return View(iteminventario);
		}

		public ActionResult Edit(int id = 0)
		{
			ItemInventario iteminventario = db.ItemInventario.Find(id);
			iteminventario.colaborador_nombre = (iteminventario.colaborador == null ? "" : iteminventario.colaborador.nombre);
			if (iteminventario.tipooperacion == ItemInventario.PRESTADO)
				iteminventario.prestamo = true;
			ViewBag.idcategoriainventario = new SelectList(db.CategoriaInventario, "idcategoriainventario", "nombre", iteminventario.idcategoriainventario);
			return View(iteminventario);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(ItemInventario iteminventario, FormCollection collection)
		{
			if (iteminventario.descripcion != null)
			{
				iteminventario.descripcion = iteminventario.descripcion.Replace("  ", " ").Trim();
				if (db.ItemInventario.Where(c => c.descripcion.Equals(iteminventario.descripcion) && c.iditeminventario != iteminventario.iditeminventario).Count() > 0)
					ModelState.AddModelError("descripcion", "La descripción del ítem debe ser única");
				else if (!Halp.tieneLetra(iteminventario.descripcion))
					ModelState.AddModelError("descripcion", "La descripción debería tener al menos una letra");
			}
			if (iteminventario.idcolaborador == null && iteminventario.colaborador_nombre != null)
				ModelState.AddModelError("colaborador_nombre", "El colaborador no existe, elija uno de la lista");

			string key = null;
			int idcd = 0;
			string vc = "";
			DateTime? vf = null;
			int? ve = 0;
			decimal? vd = 0;
			for (int i = 0; i < iteminventario.liteminventariodetalle.Count(); i++)
			{
				idcd = iteminventario.liteminventariodetalle[i].idcategoriadetalle;
				if (iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoformato.tipo == TipoDatoFormato.CADENA)
				{
					vc = iteminventario.liteminventariodetalle[i].valorcadena;
					iteminventario.liteminventariodetalle[i].valorbusqueda = vc;
					Expression<Func<ItemInventario, string>> expression = x => x.liteminventariodetalle[i].valorcadena;
					key = ExpressionHelper.GetExpressionText(expression);
					if (iteminventario.liteminventariodetalle[i].categoriadetalle.obligatorio && (vc == null || vc == ""))
						ModelState.AddModelError(key, "Ingrese " + iteminventario.liteminventariodetalle[i].categoriadetalle.nombre);
					else if (vc != null && vc != "" && iteminventario.liteminventariodetalle[i].categoriadetalle.idtipodato == TipoDato.TEXTO && !Halp.esTexto(vc))
						ModelState.AddModelError(key, "El campo no respeta el formato para textos");
					else if (vc != null && vc != "" && iteminventario.liteminventariodetalle[i].categoriadetalle.idtipodato == TipoDato.ALFANUMERICO && !Halp.esAlfanumerico(vc))
						ModelState.AddModelError(key, "El campo no respeta el formato para alfanuméricos");
					else if (iteminventario.liteminventariodetalle[i].categoriadetalle.identificador && 0 < db.ItemInventarioDetalle.Where(c => c.idcategoriadetalle == idcd && c.iditeminventario != iteminventario.iditeminventario && c.categoriadetalle.identificador && c.valorcadena == vc).Count())
						ModelState.AddModelError(key, "El campo " + iteminventario.liteminventariodetalle[i].categoriadetalle.nombre + " debe ser único");
					else if (vc != null && vc.Length > iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoformato.longitud)
						ModelState.AddModelError(key, "Longitud de " + iteminventario.liteminventariodetalle[i].categoriadetalle.nombre + " no válida");
					else if (iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo != null && iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo.idoperador != null && vc != null)
						switch (iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo.idoperador)
						{
							case TipoDatoOperador.DIFERENTE:
								if (vc.ToUpper() == iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo.valorcadena.ToUpper())
									ModelState.AddModelError(key, "El campo " + iteminventario.liteminventariodetalle[i].categoriadetalle.nombre + " debe ser " + iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo.etiqueta);
								break;
							case TipoDatoOperador.IGUAL:
								if (vc.ToUpper() != iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo.valorcadena.ToUpper())
									ModelState.AddModelError(key, "El campo " + iteminventario.liteminventariodetalle[i].categoriadetalle.nombre + " debe ser " + iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo.etiqueta);
								break;
							case TipoDatoOperador.LIKE:
								if (!((vc.ToUpper()).Contains(iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo.valorcadena.ToUpper())))
									ModelState.AddModelError(key, "El campo " + iteminventario.liteminventariodetalle[i].categoriadetalle.nombre + " debe ser " + iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo.etiqueta);
								break;
						}
				}
				else if (iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoformato.tipo == TipoDatoFormato.FECHA)
				{
					vf = iteminventario.liteminventariodetalle[i].valorfecha;
					if (vf != null)
						iteminventario.liteminventariodetalle[i].valorbusqueda = vf.ToString().Substring(0, 10);
					Expression<Func<ItemInventario, DateTime?>> expression = x => x.liteminventariodetalle[i].valorfecha;
					key = ExpressionHelper.GetExpressionText(expression);
					if (iteminventario.liteminventariodetalle[i].categoriadetalle.obligatorio && vf == null)
						ModelState.AddModelError(key, "Ingrese " + iteminventario.liteminventariodetalle[i].categoriadetalle.nombre);
					else if (iteminventario.liteminventariodetalle[i].categoriadetalle.identificador && 0 < db.ItemInventarioDetalle.Where(c => c.idcategoriadetalle == idcd && c.iditeminventario != iteminventario.iditeminventario && c.categoriadetalle.identificador && c.valorfecha == vf).Count())
						ModelState.AddModelError(key, "El campo " + iteminventario.liteminventariodetalle[i].categoriadetalle.nombre + " debe ser único");
					else if (iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo != null && iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo.idoperador != null && vf != null)
						switch (iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo.idoperador)
						{
							case TipoDatoOperador.DIFERENTE:
								if (vf == DateTime.Today)
									ModelState.AddModelError(key, "El campo " + iteminventario.liteminventariodetalle[i].categoriadetalle.nombre + " debe ser " + iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo.etiqueta);
								break;
							case TipoDatoOperador.IGUAL:
								if (vf != DateTime.Today)
									ModelState.AddModelError(key, "El campo " + iteminventario.liteminventariodetalle[i].categoriadetalle.nombre + " debe ser " + iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo.etiqueta);
								break;
							case TipoDatoOperador.MAYOR:
								if (vf <= DateTime.Today)
									ModelState.AddModelError(key, "El campo " + iteminventario.liteminventariodetalle[i].categoriadetalle.nombre + " debe ser " + iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo.etiqueta);
								break;
							case TipoDatoOperador.MENOR:
								if (vf >= DateTime.Today)
									ModelState.AddModelError(key, "El campo " + iteminventario.liteminventariodetalle[i].categoriadetalle.nombre + " debe ser " + iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo.etiqueta);
								break;
							case TipoDatoOperador.MAYOR_IGUAL:
								if (vf < DateTime.Today)
									ModelState.AddModelError(key, "El campo " + iteminventario.liteminventariodetalle[i].categoriadetalle.nombre + " debe ser " + iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo.etiqueta);
								break;
							case TipoDatoOperador.MENOR_IGUAL:
								if (vf > DateTime.Today)
									ModelState.AddModelError(key, "El campo " + iteminventario.liteminventariodetalle[i].categoriadetalle.nombre + " debe ser " + iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo.etiqueta);
								break;
						}
				}
				else if (iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoformato.tipo == TipoDatoFormato.ENTERO)
				{
					ve = iteminventario.liteminventariodetalle[i].valorentero;
					iteminventario.liteminventariodetalle[i].valorbusqueda = ve.ToString();
					Expression<Func<ItemInventario, int?>> expression = x => x.liteminventariodetalle[i].valorentero;
					key = ExpressionHelper.GetExpressionText(expression);
					if (iteminventario.liteminventariodetalle[i].categoriadetalle.obligatorio && ve == null)
						ModelState.AddModelError(key, "Ingrese " + iteminventario.liteminventariodetalle[i].categoriadetalle.nombre);
					else if (iteminventario.liteminventariodetalle[i].categoriadetalle.identificador && 0 < db.ItemInventarioDetalle.Where(c => c.idcategoriadetalle == idcd && c.iditeminventario != iteminventario.iditeminventario && c.categoriadetalle.identificador && c.valorentero == ve).Count())
						ModelState.AddModelError(key, "El campo " + iteminventario.liteminventariodetalle[i].categoriadetalle.nombre + " debe ser único");
					else if (iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo != null && iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo.idoperador != null && ve != null)
						switch (iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo.idoperador)
						{
							case TipoDatoOperador.DIFERENTE:
								if (ve == iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo.valorentero)
									ModelState.AddModelError(key, "El campo " + iteminventario.liteminventariodetalle[i].categoriadetalle.nombre + " debe ser " + iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo.etiqueta);
								break;
							case TipoDatoOperador.IGUAL:
								if (ve != iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo.valorentero)
									ModelState.AddModelError(key, "El campo " + iteminventario.liteminventariodetalle[i].categoriadetalle.nombre + " debe ser " + iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo.etiqueta);
								break;
							case TipoDatoOperador.MAYOR:
								if (ve <= iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo.valorentero)
									ModelState.AddModelError(key, "El campo " + iteminventario.liteminventariodetalle[i].categoriadetalle.nombre + " debe ser " + iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo.etiqueta);
								break;
							case TipoDatoOperador.MENOR:
								if (ve >= iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo.valorentero)
									ModelState.AddModelError(key, "El campo " + iteminventario.liteminventariodetalle[i].categoriadetalle.nombre + " debe ser " + iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo.etiqueta);
								break;
							case TipoDatoOperador.MAYOR_IGUAL:
								if (ve < iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo.valorentero)
									ModelState.AddModelError(key, "El campo " + iteminventario.liteminventariodetalle[i].categoriadetalle.nombre + " debe ser " + iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo.etiqueta);
								break;
							case TipoDatoOperador.MENOR_IGUAL:
								if (ve > iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo.valorentero)
									ModelState.AddModelError(key, "El campo " + iteminventario.liteminventariodetalle[i].categoriadetalle.nombre + " debe ser " + iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo.etiqueta);
								break;
						}
				}
				else if (iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoformato.tipo == TipoDatoFormato.DECIMAL)
				{
					vd = iteminventario.liteminventariodetalle[i].valordecimal;
					iteminventario.liteminventariodetalle[i].valorbusqueda = vd.ToString();
					Expression<Func<ItemInventario, decimal?>> expression = x => x.liteminventariodetalle[i].valordecimal;
					key = ExpressionHelper.GetExpressionText(expression);
					if (iteminventario.liteminventariodetalle[i].categoriadetalle.obligatorio && vd == null)
						ModelState.AddModelError(key, "Ingrese " + iteminventario.liteminventariodetalle[i].categoriadetalle.nombre);
					else if (iteminventario.liteminventariodetalle[i].categoriadetalle.identificador && 0 < db.ItemInventarioDetalle.Where(c => c.idcategoriadetalle == idcd && c.iditeminventario != iteminventario.iditeminventario && c.categoriadetalle.identificador && c.valordecimal == vd).Count())
						ModelState.AddModelError(key, "El campo " + iteminventario.liteminventariodetalle[i].categoriadetalle.nombre + " debe ser único");
					else if (vd != null && BitConverter.GetBytes(decimal.GetBits((decimal)vd)[3])[2] > iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoformato.escala)
						ModelState.AddModelError(key, "El campo no debe tener más de " + iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoformato.escala + " decimal(es)");
					else if (iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo != null && iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo.idoperador != null && vd != null)
						switch (iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo.idoperador)
						{
							case TipoDatoOperador.DIFERENTE:
								if (vd == iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo.valorentero)
									ModelState.AddModelError(key, "El campo " + iteminventario.liteminventariodetalle[i].categoriadetalle.nombre + " debe ser " + iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo.etiqueta);
								break;
							case TipoDatoOperador.IGUAL:
								if (vd != iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo.valorentero)
									ModelState.AddModelError(key, "El campo " + iteminventario.liteminventariodetalle[i].categoriadetalle.nombre + " debe ser " + iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo.etiqueta);
								break;
							case TipoDatoOperador.MAYOR:
								if (vd <= iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo.valorentero)
									ModelState.AddModelError(key, "El campo " + iteminventario.liteminventariodetalle[i].categoriadetalle.nombre + " debe ser " + iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo.etiqueta);
								break;
							case TipoDatoOperador.MENOR:
								if (vd >= iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo.valorentero)
									ModelState.AddModelError(key, "El campo " + iteminventario.liteminventariodetalle[i].categoriadetalle.nombre + " debe ser " + iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo.etiqueta);
								break;
							case TipoDatoOperador.MAYOR_IGUAL:
								if (vd < iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo.valorentero)
									ModelState.AddModelError(key, "El campo " + iteminventario.liteminventariodetalle[i].categoriadetalle.nombre + " debe ser " + iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo.etiqueta);
								break;
							case TipoDatoOperador.MENOR_IGUAL:
								if (vd > iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo.valorentero)
									ModelState.AddModelError(key, "El campo " + iteminventario.liteminventariodetalle[i].categoriadetalle.nombre + " debe ser " + iteminventario.liteminventariodetalle[i].categoriadetalle.tipodatoatributo.etiqueta);
								break;
						}
				}

			}

			if (ModelState.IsValid)
			{
				bool ok = true;
				if (iteminventario.tipooperacion == "D" && iteminventario.idcolaborador != null && iteminventario.idcolaborador != 0)
				{
					string mensaje = mensajea2(iteminventario.iditeminventario, iteminventario.idcategoriainventario, iteminventario.idcolaborador, iteminventario.prestamo);
					if (mensaje.Split('|')[0] == "0")
					{
						TempData["alerta"] = mensaje.Split('|')[1];
						ok = false;
					}
					else if (mensaje.Split('|')[0] == "2" && collection["forzar"] == "0")
					{
						TempData["confirma"] = mensaje.Split('|')[1] + "|$('#forzar').val('1'); $('#form1').submit();";
						ok = false;
					}
				}
				if (ok)
				{
					if (iteminventario.idcolaborador == null)
						iteminventario.tipooperacion = "D";
					else
						if (iteminventario.prestamo)
							iteminventario.tipooperacion = "P";
						else
							iteminventario.tipooperacion = "A";
					db.Entry(new ItemInventario() { iditeminventario = iteminventario.iditeminventario, idcategoriainventario = iteminventario.idcategoriainventario, idcolaborador = iteminventario.idcolaborador, descripcion = iteminventario.descripcion, tipooperacion = iteminventario.tipooperacion }).State = EntityState.Modified;
					db.SaveChanges();
					foreach (ItemInventarioDetalle p in iteminventario.liteminventariodetalle)
					{
						db.Entry(new ItemInventarioDetalle() { iditeminventariodetalle = p.iditeminventariodetalle, iditeminventario = p.iditeminventario, idcategoriadetalle = p.idcategoriadetalle, valorcadena = p.valorcadena, valorentero = p.valorentero, valordecimal = p.valordecimal, valorfecha = p.valorfecha, valorbusqueda = p.valorbusqueda }).State = EntityState.Modified;
						db.SaveChanges();
					}
					TempData["alerta"] = "Ítem de inventario actualizado con éxito";
					return RedirectToAction("Index");
				}
			}
			ViewBag.idcategoriainventario = new SelectList(db.CategoriaInventario, "idcategoriainventario", "nombre", iteminventario.idcategoriainventario);
			return View(iteminventario);
		}

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

		public string mensajea2(int iditeminventario, int idcategoriainventario, int? idcolaborador, bool prestamo)
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
			return mensaje;
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
			StringBuilder html = new StringBuilder("");
			if (l.Count() > 0) {
				html.Append("<table class=\"rvtabla\"><tr><th>Descripción</th><th>Categoría</th><th>Estado</th><th>Colaborador</th><th>Opciones</th></tr>");
				foreach (ItemInventario o in l)
				{
					html.Append("<tr><td>" + o.descripcion + "</td>");
					html.Append("<td>" + o.categoriainventario.nombre + "</td>");
					html.Append("<td>" + o.muestraOperacion + "</td>");
					html.Append("<td>" + o.colaborador.nombre + "</td>");
					html.Append("<td><a href=\"/INVEN/ItemInventario2/Edit/" + o.iditeminventario + "\">Editar</a> | <a href=\"#\" onclick=\"libera('" + o.iditeminventario + "');\">Liberar</a></td></tr>");
				}
				html.Append("</table>");
			}
			return Content(html.ToString(), "text");
		}

		[HttpGet]
		public ContentResult Colabora(string term)
		{
			string mensaje = "0|";
			if (term.Length == 7 && (term[0] == 'M' || term[0] == 'X'))
			{
				Colaborador c = db.Colaborador.Where(x => x.codigo == term).FirstOrDefault();
				if (c != null)
					mensaje = c.idcolaborador + "|" + c.nombre + "|" + c.codigo;
			}
			else
			{
				Colaborador c = db.Colaborador.Where(x => x.nombre == term).FirstOrDefault();
				if (c != null)
					mensaje = c.idcolaborador + "|" + c.nombre + "|" + c.codigo;
			}
			return Content(mensaje, "text");
		}

		[HttpGet]
		public ContentResult Estado(int id = 0)
		{
			CategoriaInventario o = db.CategoriaInventario.Find(id);
			if (o.desactivado)
				return Content("", "text");
			else
				return Content(db.CategoriaDetalle.Where(x => x.idcategoriainventario == id).Count() + "", "text");
		}

		[HttpGet]
		public JsonResult CategoriaInventarioList(int id)
		{
			return Json(new SelectList(db.CategoriaInventario.Where(c => c.idcategoriainventariotipo.Equals(id)).OrderBy(c => c.nombre), "idcategoriainventario", "nombre"), JsonRequestBehavior.AllowGet);
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
			}).Where(x => ((x.codigo.Contains(term) || x.nombre.Contains(term)) && !x.desactivado && x.aprobado == true));
			return Json(data, JsonRequestBehavior.AllowGet);
		}

	}
}