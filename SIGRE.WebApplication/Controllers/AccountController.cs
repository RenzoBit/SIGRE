using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using SIGRE.WebApplication.Filters;
using SIGRE.WebApplication.Models;
using SIGRE.Models;
using SIGRE.Controllers;

namespace SIGRE.WebApplication.Controllers
{
	[Authorize]
	[InitializeSimpleMembership]
	public class AccountController : Controller
	{
		private BDSIGRE db = new BDSIGRE();

		[AllowAnonymous]
		public ActionResult Login(string returnUrl)
		{
			ViewBag.ReturnUrl = returnUrl;
			return View();
		}

		//
		// POST: /Account/Login

		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public ActionResult Login(LoginModel model, string returnUrl)
		{
			Usuario usuario = db.Usuario.Where(x => x.username == model.UserName.ToUpper()).FirstOrDefault();
			if (usuario == null)
			{
				TempData["alerta"] = "Nombre de usuario o clave incorrecta";
				TempData["borra"] = "si";
			}
			else if (usuario.bloqueado)
				TempData["alerta"] = "Su cuenta ha sido bloqueada. Comuníquese con el área de soporte";
			else
			{
				if (usuario.password == Halp.GetMD5(model.Password))
				{
					usuario.intentos = 0;
					usuario.intentospass = 0;
					db.Entry(usuario).State = System.Data.EntityState.Modified;
					db.SaveChanges();
					FormsAuthentication.SetAuthCookie(usuario.username, true);
					List<RolOpcion> l1 = db.RolOpcion.Where(x => x.idrol == usuario.idrol && x.idsuperior == null).ToList();
					if (usuario.idrol != Rol.SUPER)
					{
						int idcolaborador = db.Colaborador.Where(x => x.codigo == usuario.username).FirstOrDefault().idcolaborador;
						List<RolOpcion> l2 = null;
						bool e1 = false;
						bool e2 = false;
						if (0 < db.Colaborador.Where(x => x.colaborador_idcolaborador == idcolaborador && !x.desactivado && x.aprobado == null).Count())
						{
							l2 = db.RolOpcion.Where(x => x.idrol == Rol.APROBADOR_C && x.idsuperior == null).ToList();
							foreach (RolOpcion o2 in l2)
							{
								e1 = false;
								foreach (RolOpcion o1 in l1)
									if (o2.idopcion == o1.idopcion)
									{
										e1 = true;
										foreach (RolOpcion s2 in o2.lrolopcion)
										{
											e2 = false;
											foreach (RolOpcion s1 in o1.lrolopcion)
												if (s2.idopcion == s1.idopcion)
												{
													e2 = true;
													break;
												}
											if (!e2)
												o1.lrolopcion.Add(s2);
										}
										break;
									}
								if (!e1)
									l1.Add(o2);
							}
						}
						if (0 < db.Recurso.Where(x => x.idcolaborador == idcolaborador && !x.desactivado && x.aprobado == null).Count())
						{
							l2 = db.RolOpcion.Where(x => x.idrol == Rol.PROPIETARIO && x.idsuperior == null).ToList();
							foreach (RolOpcion o2 in l2)
							{
								e1 = false;
								foreach (RolOpcion o1 in l1)
									if (o2.idopcion == o1.idopcion)
									{
										e1 = true;
										foreach (RolOpcion s2 in o2.lrolopcion)
										{
											e2 = false;
											foreach (RolOpcion s1 in o1.lrolopcion)
												if (s2.idopcion == s1.idopcion)
												{
													e2 = true;
													break;
												}
											if (!e2)
												o1.lrolopcion.Add(s2);
										}
										break;
									}
								if (!e1)
									l1.Add(o2);
							}
						}
						if (0 < db.Perfil.Where(x => x.idcolaborador == idcolaborador && !x.desactivado && x.aprobado == null).Count())
						{
							l2 = db.RolOpcion.Where(x => x.idrol == Rol.APROBADOR_P && x.idsuperior == null).ToList();
							foreach (RolOpcion o2 in l2)
							{
								e1 = false;
								foreach (RolOpcion o1 in l1)
									if (o2.idopcion == o1.idopcion)
									{
										e1 = true;
										foreach (RolOpcion s2 in o2.lrolopcion)
										{
											e2 = false;
											foreach (RolOpcion s1 in o1.lrolopcion)
												if (s2.idopcion == s1.idopcion)
												{
													e2 = true;
													break;
												}
											if (!e2)
												o1.lrolopcion.Add(s2);
										}
										break;
									}
								if (!e1)
									l1.Add(o2);
							}
						}
						if (0 < db.ColaboradorPerfil.Where(x => x.perfil.idcolaborador == idcolaborador && x.aprobado == null && !x.revocacion).Count())
						{
							l2 = db.RolOpcion.Where(x => x.idrol == Rol.APROBADOR_A && x.idsuperior == null).ToList();
							foreach (RolOpcion o2 in l2)
							{
								e1 = false;
								foreach (RolOpcion o1 in l1)
									if (o2.idopcion == o1.idopcion)
									{
										e1 = true;
										foreach (RolOpcion s2 in o2.lrolopcion)
										{
											e2 = false;
											foreach (RolOpcion s1 in o1.lrolopcion)
												if (s2.idopcion == s1.idopcion)
												{
													e2 = true;
													break;
												}
											if (!e2)
												o1.lrolopcion.Add(s2);
										}
										break;
									}
								if (!e1)
									l1.Add(o2);
							}
						}
						if (0 < db.ColaboradorPerfil.Where(x => x.perfil.idcolaborador == idcolaborador && x.aprobado == null && x.revocacion).Count())
						{
							l2 = db.RolOpcion.Where(x => x.idrol == Rol.APROBADOR_R && x.idsuperior == null).ToList();
							foreach (RolOpcion o2 in l2)
							{
								e1 = false;
								foreach (RolOpcion o1 in l1)
									if (o2.idopcion == o1.idopcion)
									{
										e1 = true;
										foreach (RolOpcion s2 in o2.lrolopcion)
										{
											e2 = false;
											foreach (RolOpcion s1 in o1.lrolopcion)
												if (s2.idopcion == s1.idopcion)
												{
													e2 = true;
													break;
												}
											if (!e2)
												o1.lrolopcion.Add(s2);
										}
										break;
									}
								if (!e1)
									l1.Add(o2);
							}
						}
					}
					Session["lrolopcion"] = l1;
					return RedirectToLocal(returnUrl);
				}
				else
				{
					usuario.intentos += 1;
					if (usuario.intentos == 5)
						TempData["alerta"] = "Nombre de usuario o clave incorrecta. Si falla en el próximo intento su cuenta será bloqueada";
					else if (usuario.intentos == 6)
					{
						TempData["alerta"] = "Su cuenta ha sido bloqueada. Comuníquese con el área de soporte";
						usuario.bloqueado = true;
					}
					else
						TempData["alerta"] = "Nombre de usuario o clave incorrecta";
					db.Entry(usuario).State = System.Data.EntityState.Modified;
					db.SaveChanges();
				}
			}
			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult LogOff()
		{
			Session["lrolopcion"] = null;
			FormsAuthentication.SignOut();
			WebSecurity.Logout();
			return RedirectToAction("Index", "Home");
		}

		[HttpGet]
		public ActionResult GTFO()
		{
			Session["lrolopcion"] = null;
			FormsAuthentication.SignOut();
			WebSecurity.Logout();
			return RedirectToAction("Index", "Home");
		}

		[HttpGet]
		[AllowAnonymous]
		public ContentResult Sesion()
		{
			return Content((Request.IsAuthenticated && Session["lrolopcion"] != null) ? "1" : "0", "text");
		}

		//
		// GET: /Account/Register

		[AllowAnonymous]
		public ActionResult Register()
		{
			return View();
		}

		//
		// POST: /Account/Register

		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public ActionResult Register(RegisterModel model)
		{
			if (ModelState.IsValid)
			{
				// Intento de registrar al usuario
				try
				{
					WebSecurity.CreateUserAndAccount(model.UserName, model.Password);
					WebSecurity.Login(model.UserName, model.Password);
					return RedirectToAction("Index", "Home");
				}
				catch (MembershipCreateUserException e)
				{
					ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
				}
			}

			// Si llegamos a este punto, es que se ha producido un error y volvemos a mostrar el formulario
			return View(model);
		}

		//
		// POST: /Account/Disassociate

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Disassociate(string provider, string providerUserId)
		{
			string ownerAccount = OAuthWebSecurity.GetUserName(provider, providerUserId);
			ManageMessageId? message = null;

			// Desasociar la cuenta solo si el usuario que ha iniciado sesión es el propietario
			if (ownerAccount == User.Identity.Name)
			{
				// Usar una transacción para evitar que el usuario elimine su última credencial de inicio de sesión
				using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
				{
					bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
					if (hasLocalAccount || OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name).Count > 1)
					{
						OAuthWebSecurity.DeleteAccount(provider, providerUserId);
						scope.Complete();
						message = ManageMessageId.RemoveLoginSuccess;
					}
				}
			}

			return RedirectToAction("Manage", new { Message = message });
		}

		//
		// GET: /Account/Manage

		public ActionResult Manage(ManageMessageId? message)
		{
			ViewBag.StatusMessage =
				message == ManageMessageId.ChangePasswordSuccess ? "La contraseña se ha cambiado."
				: message == ManageMessageId.SetPasswordSuccess ? "Su contraseña se ha establecido."
				: message == ManageMessageId.RemoveLoginSuccess ? "El inicio de sesión externo se ha quitado."
				: "";
			ViewBag.HasLocalPassword = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
			ViewBag.ReturnUrl = Url.Action("Manage");
			return View();
		}

		//
		// POST: /Account/Manage

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Manage(LocalPasswordModel model)
		{
			bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
			ViewBag.HasLocalPassword = hasLocalAccount;
			ViewBag.ReturnUrl = Url.Action("Manage");
			if (hasLocalAccount)
			{
				if (ModelState.IsValid)
				{
					// ChangePassword iniciará una excepción en lugar de devolver false en determinados escenarios de error.
					bool changePasswordSucceeded;
					try
					{
						changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
					}
					catch (Exception)
					{
						changePasswordSucceeded = false;
					}

					if (changePasswordSucceeded)
					{
						return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
					}
					else
					{
						ModelState.AddModelError("", "La contraseña actual es incorrecta o la nueva contraseña no es válida.");
					}
				}
			}
			else
			{
				// El usuario no dispone de contraseña local, por lo que debe quitar todos los errores de validación generados por un
				// campo OldPassword
				ModelState state = ModelState["OldPassword"];
				if (state != null)
				{
					state.Errors.Clear();
				}

				if (ModelState.IsValid)
				{
					try
					{
						WebSecurity.CreateAccount(User.Identity.Name, model.NewPassword);
						return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
					}
					catch (Exception)
					{
						ModelState.AddModelError("", String.Format("No se puede crear una cuenta local. Es posible que ya exista una cuenta con el nombre \"{0}\".", User.Identity.Name));
					}
				}
			}

			// Si llegamos a este punto, es que se ha producido un error y volvemos a mostrar el formulario
			return View(model);
		}

		//
		// POST: /Account/ExternalLogin

		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public ActionResult ExternalLogin(string provider, string returnUrl)
		{
			return new ExternalLoginResult(provider, Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
		}

		//
		// GET: /Account/ExternalLoginCallback

		[AllowAnonymous]
		public ActionResult ExternalLoginCallback(string returnUrl)
		{
			AuthenticationResult result = OAuthWebSecurity.VerifyAuthentication(Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
			if (!result.IsSuccessful)
			{
				return RedirectToAction("ExternalLoginFailure");
			}

			if (OAuthWebSecurity.Login(result.Provider, result.ProviderUserId, createPersistentCookie: false))
			{
				return RedirectToLocal(returnUrl);
			}

			if (User.Identity.IsAuthenticated)
			{
				// Si el usuario actual ha iniciado sesión, agregue la cuenta nueva
				OAuthWebSecurity.CreateOrUpdateAccount(result.Provider, result.ProviderUserId, User.Identity.Name);
				return RedirectToLocal(returnUrl);
			}
			else
			{
				// El usuario es nuevo, solicitar nombres de pertenencia deseados
				string loginData = OAuthWebSecurity.SerializeProviderUserId(result.Provider, result.ProviderUserId);
				ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(result.Provider).DisplayName;
				ViewBag.ReturnUrl = returnUrl;
				return View("ExternalLoginConfirmation", new RegisterExternalLoginModel { UserName = result.UserName, ExternalLoginData = loginData });
			}
		}

		//
		// POST: /Account/ExternalLoginConfirmation

		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public ActionResult ExternalLoginConfirmation(RegisterExternalLoginModel model, string returnUrl)
		{
			string provider = null;
			string providerUserId = null;

			if (User.Identity.IsAuthenticated || !OAuthWebSecurity.TryDeserializeProviderUserId(model.ExternalLoginData, out provider, out providerUserId))
			{
				return RedirectToAction("Manage");
			}

			if (ModelState.IsValid)
			{
				// Insertar un nuevo usuario en la base de datos
				using (UsersContext db = new UsersContext())
				{
					UserProfile user = db.UserProfiles.FirstOrDefault(u => u.UserName.ToLower() == model.UserName.ToLower());
					// Comprobar si el usuario ya existe
					if (user == null)
					{
						// Insertar el nombre en la tabla de perfiles
						db.UserProfiles.Add(new UserProfile { UserName = model.UserName });
						db.SaveChanges();

						OAuthWebSecurity.CreateOrUpdateAccount(provider, providerUserId, model.UserName);
						OAuthWebSecurity.Login(provider, providerUserId, createPersistentCookie: false);

						return RedirectToLocal(returnUrl);
					}
					else
					{
						ModelState.AddModelError("UserName", "El nombre de usuario ya existe. Escriba un nombre de usuario diferente.");
					}
				}
			}

			ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(provider).DisplayName;
			ViewBag.ReturnUrl = returnUrl;
			return View(model);
		}

		//
		// GET: /Account/ExternalLoginFailure

		[AllowAnonymous]
		public ActionResult ExternalLoginFailure()
		{
			return View();
		}

		[AllowAnonymous]
		[ChildActionOnly]
		public ActionResult ExternalLoginsList(string returnUrl)
		{
			ViewBag.ReturnUrl = returnUrl;
			return PartialView("_ExternalLoginsListPartial", OAuthWebSecurity.RegisteredClientData);
		}

		[ChildActionOnly]
		public ActionResult RemoveExternalLogins()
		{
			ICollection<OAuthAccount> accounts = OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name);
			List<ExternalLogin> externalLogins = new List<ExternalLogin>();
			foreach (OAuthAccount account in accounts)
			{
				AuthenticationClientData clientData = OAuthWebSecurity.GetOAuthClientData(account.Provider);

				externalLogins.Add(new ExternalLogin
				{
					Provider = account.Provider,
					ProviderDisplayName = clientData.DisplayName,
					ProviderUserId = account.ProviderUserId,
				});
			}

			ViewBag.ShowRemoveButton = externalLogins.Count > 1 || OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
			return PartialView("_RemoveExternalLoginsPartial", externalLogins);
		}

		#region Aplicaciones auxiliares
		private ActionResult RedirectToLocal(string returnUrl)
		{
			if (Url.IsLocalUrl(returnUrl))
			{
				return Redirect(returnUrl);
			}
			else
			{
				return RedirectToAction("Index", "Home");
			}
		}

		public enum ManageMessageId
		{
			ChangePasswordSuccess,
			SetPasswordSuccess,
			RemoveLoginSuccess,
		}

		internal class ExternalLoginResult : ActionResult
		{
			public ExternalLoginResult(string provider, string returnUrl)
			{
				Provider = provider;
				ReturnUrl = returnUrl;
			}

			public string Provider { get; private set; }
			public string ReturnUrl { get; private set; }

			public override void ExecuteResult(ControllerContext context)
			{
				OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
			}
		}

		private static string ErrorCodeToString(MembershipCreateStatus createStatus)
		{
			// Vaya a http://go.microsoft.com/fwlink/?LinkID=177550 para
			// obtener una lista completa de códigos de estado.
			switch (createStatus)
			{
				case MembershipCreateStatus.DuplicateUserName:
					return "El nombre de usuario ya existe. Escriba un nombre de usuario diferente.";

				case MembershipCreateStatus.DuplicateEmail:
					return "Ya existe un nombre de usuario para esa dirección de correo electrónico. Escriba una dirección de correo electrónico diferente.";

				case MembershipCreateStatus.InvalidPassword:
					return "La contraseña especificada no es válida. Escriba un valor de contraseña válido.";

				case MembershipCreateStatus.InvalidEmail:
					return "La dirección de correo electrónico especificada no es válida. Compruebe el valor e inténtelo de nuevo.";

				case MembershipCreateStatus.InvalidAnswer:
					return "La respuesta de recuperación de la contraseña especificada no es válida. Compruebe el valor e inténtelo de nuevo.";

				case MembershipCreateStatus.InvalidQuestion:
					return "La pregunta de recuperación de la contraseña especificada no es válida. Compruebe el valor e inténtelo de nuevo.";

				case MembershipCreateStatus.InvalidUserName:
					return "El nombre de usuario especificado no es válido. Compruebe el valor e inténtelo de nuevo.";

				case MembershipCreateStatus.ProviderError:
					return "El proveedor de autenticación devolvió un error. Compruebe los datos especificados e inténtelo de nuevo. Si el problema continúa, póngase en contacto con el administrador del sistema.";

				case MembershipCreateStatus.UserRejected:
					return "La solicitud de creación de usuario se ha cancelado. Compruebe los datos especificados e inténtelo de nuevo. Si el problema continúa, póngase en contacto con el administrador del sistema.";

				default:
					return "Error desconocido. Compruebe los datos especificados e inténtelo de nuevo. Si el problema continúa, póngase en contacto con el administrador del sistema.";
			}
		}
		#endregion
	}
}
