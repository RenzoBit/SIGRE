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
	[Table("rol")]
	[DisplayColumn("nombre")]
	public partial class Rol
	{
		public Rol()
        {
			lusuario = new HashSet<Usuario>();
			lrolopcion = new HashSet<RolOpcion>();
        }

		[Key]
		public int idrol { get; set; }

		[Display(Name = "Nombre")]
		[Required(ErrorMessage = "Ingrese un nombre")]
		[StringLength(50)]
		public string nombre { get; set; }

		[Display(Name = "Descripción")]
		[Required(ErrorMessage = "Ingrese una descripción")]
		[StringLength(100)]
		public string descripcion { get; set; }

		[Display(Name = "Dinámico")]
		[Required(ErrorMessage = "Seleccione un valor para dinámico")]
		[DefaultValue(false)]
		public bool dinamico { get; set; }

		public const short CONSULTOR = 1;
		public const short FACILITADOR = 2;
		public const short ADMINISTRADOR = 3;
		public const short DASHBOARD = 4;
		public const short DIRECCION = 5;
		public const short APROBADOR_C = 6;
		public const short PROPIETARIO = 7;
		public const short SUPER = 8;
		public const short APROBADOR_P = 9;
		public const short APROBADOR_A = 10;
		public const short APROBADOR_R = 11;
		
		public virtual ICollection<Usuario> lusuario { get; set; }

		public virtual ICollection<RolOpcion> lrolopcion { get; set; }

		[NotMapped]
		public virtual IList<Opcion> lopcion { get; set; }
	}
}
