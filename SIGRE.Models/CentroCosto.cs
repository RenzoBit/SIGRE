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
	[Table("centrocosto")]
	[DisplayColumn("nombre")]
	public class CentroCosto
	{
		[Key]
		public int idcentrocosto { get; set; }

		[Display(Name = "Unidad de negocio")]
		public int? centrocosto_idcentrocosto { get; set; }

		[Display(Name = "Propietario")]
		[Required(ErrorMessage = "Seleccione un propietario que sea colaborador interno")]
		public int idcolaborador { get; set; }

		[Display(Name = "Código")]
		[Required(ErrorMessage = "Ingrese un código")]
		[StringLength(8)]
		public string codigo { get; set; }

		[Display(Name = "Nombre")]
		[Required(ErrorMessage = "Ingrese un nombre")]
		[StringLength(100)]
		public string nombre { get; set; }

		[Display(Name = "Activo")]
		[Required(ErrorMessage = "Seleccione un valor para desactivado")]
		[DefaultValue(false)]
		public bool desactivado { get; set; }
		
		[NotMapped]
		public string colaborador_nombre { set; get; }

		public virtual CentroCosto unidadnegocio { get; set; }
		public virtual Colaborador propietario { get; set; }
		public virtual ICollection<CentroCosto> lcentrocosto { get; set; }
		public virtual ICollection<Colaborador> lcolaborador { get; set; }
		public virtual ICollection<Perfil> lperfil { get; set; }

		public string muestraTipo
		{
			get
			{
				return (this.centrocosto_idcentrocosto == null ? "UN" : "CC");
			}
		}

		public string muestraEstado
		{
			get
			{
				return (desactivado ? "No" : "Si");
			}
		}
	}
}
