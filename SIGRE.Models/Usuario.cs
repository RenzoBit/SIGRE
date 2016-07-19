using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGRE.Models
{
	[Table("usuario")]
	[DisplayColumn("username")]
	public partial class Usuario
	{
		public Usuario()
        {
			lcolaborador = new HashSet<Colaborador>();
        }

		[Key]
		public int idusuario { get; set; }

		[Display(Name = "Código de usuario")]
		[Required(ErrorMessage = "No se eligió un colaborador válido")]
		[StringLength(50)]
		public string username { get; set; }

		[Display(Name = "Contraseña")]
		[Required(ErrorMessage = "La contraseña ingresada es incorrecta")]
		[StringLength(32)]
		public string password { get; set; }

		[Display(Name = "Rol")]
		[Required(ErrorMessage = "Seleccione un rol")]
		public int idrol { get; set; }

        [Display(Name = "Intentos")]
		[Required(ErrorMessage = "Ingrese intentos")]
		[DefaultValue(0)]
		public int intentos { get; set; }

		[Display(Name = "Intentos contraseña")]
		[Required(ErrorMessage = "Ingrese intentos contraseña")]
		[DefaultValue(0)]
		public int intentospass { get; set; }

		[Display(Name = "Bloqueado")]
		[Required(ErrorMessage = "Seleccione un valor para bloqueado")]
		[DefaultValue(false)]
		public bool bloqueado { get; set; }

		[NotMapped]
		[Display(Name = "Colaborador")]
		public string colaborador_nombre { set; get; }

		[NotMapped]
		[Display(Name = "Nueva contraseña")]
		public string passwordn { set; get; }

		[NotMapped]
		[Display(Name = "Repetir nueva contraseña")]
		public string passwordr { set; get; }

		public virtual Rol rol { get; set; }

		public virtual ICollection<Colaborador> lcolaborador { get; set; }

		public string muestraBloqueado
		{
			get
			{
				return bloqueado ? "Si" : "No";
			}
		}
	}
}
