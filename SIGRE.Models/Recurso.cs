using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SIGRE.Models
{
	[Table("recurso")]
	[DisplayColumn("nombre")]
	public class Recurso
	{
		[Key]
		public int idrecurso { get; set; }
		[Display(Name = "Propietario")]
		[Required(ErrorMessage = "Selecciona un propietario")]
		public int idcolaborador { get; set; }
		/*
		[Display(Name = "Aprobador")]
		[Required(ErrorMessage = "Seleccione un aprobador")]
		public int colaborador_idcolaborador { get; set; }
		*/
		[Display(Name = "Nombre")]
		[Required(ErrorMessage = "Ingrese un nombre")]
		[StringLength(50)]
		public string nombre { get; set; }
		[Display(Name = "Descripción")]
		[Required(ErrorMessage = "Ingrese una descripción")]
		[StringLength(100)]
		public string descripcion { get; set; }
		[Display(Name = "Costo")]
		[Required(ErrorMessage = "Ingrese un costo")]
		[DefaultValue(0)]
		public decimal costo { get; set; }
		[Display(Name = "Foto")]
		public byte[] foto { get; set; }
		[Display(Name = "Desactivado")]
		[Required(ErrorMessage = "Seleccione un valor para desactivado")]
		[DefaultValue(false)]
		public bool desactivado { get; set; }
		[Display(Name = "Estado")]
		public bool? aprobado { get; set; }

		/*
		[NotMapped]
		[Display(Name = "Unidad de negocio")]
		[Required(ErrorMessage = "Seleccione una unidad de negocio")]
		public int idcentrocosto { get; set; }
		*/
		[NotMapped]
		public string propietario_nombre { set; get; }
		[NotMapped]
		[Display(Name = "Foto")]
		[DataType(DataType.Upload)]
		public HttpPostedFileBase file { set; get; }

		public virtual Colaborador propietario { get; set; }
		//public virtual Colaborador aprobador { get; set; }
		public virtual ICollection<PerfilRecurso> lperfilrecurso { get; set; }

		public string muestraNombrePrecio
		{
			get
			{
				return this.nombre + " | S/. " + this.costo;
			}
		}

		public string muestraEstado
		{
			get
			{
				return (this.desactivado ? "Inactivo" : "Activo") + " - " +  (this.aprobado == true ? "Aprobado" : (this.aprobado == false ? "Rechazado" : "Pendiente"));
			}
		}
	}
}
