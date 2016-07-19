using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SIGRE.Models
{
	[Table("perfil")]
	[DisplayColumn("nombre")]
	public class Perfil
	{
		[Key]
		public int idperfil { get; set; }
		[Display(Name = "Aprobador")]
		[Required(ErrorMessage = "Seleccione un propietario")]
		public int idcolaborador { get; set; }
		[Display(Name = "Unidad de negocio")]
		[Required(ErrorMessage = "Seleccione una unidad de negocio")]
		public int idcentrocosto { get; set; }
		[Display(Name = "Nombre")]
		[Required(ErrorMessage = "Ingrese un nombre")]
		[StringLength(50)]
		public string nombre { get; set; }
		[Display(Name = "Descripción")]
		[Required(ErrorMessage = "Ingrese una descripción")]
		[StringLength(100)]
		public string descripcion { get; set; }
		[Display(Name = "Costo mensual")]
		[Required(ErrorMessage = "Ingrese un costo")]
		[DefaultValue(0)]
		public decimal costo { get; set; }
		[Display(Name = "Aprobado")]
		public bool? aprobado { get; set; }
		[Display(Name = "Desactivado")]
		[Required(ErrorMessage = "Seleccione un valor para desactivado")]
		[DefaultValue(false)]
		public bool desactivado { get; set; }
		[Display(Name = "Asignado")]
		[Required(ErrorMessage = "Seleccione un valor para asignado")]
		[DefaultValue(false)]
		public bool asignado { get; set; }
		[NotMapped]
		public string propietario_nombre { set; get; }

		//falta ver que tiene un colaborador con su idcolaborador
		public virtual Colaborador propietario { get; set; }
		public virtual CentroCosto unidadnegocio { get; set; }
		public virtual ICollection<Colaborador> lcolaborador { get; set; }
		public virtual ICollection<PerfilRecurso> lperfilrecurso { get; set; }
		public virtual ICollection<ColaboradorPerfil> lcolaboradorperfil { get; set; }

		public string muestraEstado
		{
			get
			{
				return (this.desactivado ? "Inactivo" : (this.aprobado == true ? "Aprobado" : (this.aprobado == false ? "Rechazado" : "En aprobación"))) + " - " + (this.lcolaborador.Count() == 0 ? "No asignado" : "Asignado");
			}
		}

		public string muestraUnidadNombre
		{
			get
			{
				return this.unidadnegocio.codigo + " - " + this.nombre;
			}
		}
	}
}
