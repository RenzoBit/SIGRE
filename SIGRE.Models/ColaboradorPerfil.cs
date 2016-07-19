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
	[Table("solicitudcolaboradorperfil")]
	[DisplayColumn("fecha")]
	public class ColaboradorPerfil
	{
		[Key]
		public int idcolaboradorperfil { get; set; }
		[Display(Name = "Colaborador")]
		[Required(ErrorMessage = "Seleccione un colaborador")]
		public int idcolaborador { get; set; }
		[Display(Name = "Perfil")]
		[Required(ErrorMessage = "Seleccione un perfil")]
		public int idperfil { get; set; }
		[Display(Name = "Fecha de revisión")]
		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
		public DateTime? fecha { get; set; }
		[Display(Name = "Comentario")]
		[StringLength(100)]
		public string comentario { get; set; }
		[Display(Name = "Estado")]
		public bool? aprobado { get; set; }
		[Display(Name = "Solicitud")]
		[Required(ErrorMessage = "Seleccione un valor para solicitud")]
		[DefaultValue(false)]
		public bool revocacion { get; set; }

		public virtual Colaborador colaborador { get; set; }
		public virtual Perfil perfil { get; set; }

		public string muestraAprobado
		{
			get
			{
				return (this.aprobado == true ? "Aprobada" : (this.aprobado == false ? "Rechazada" : "Pendiente"));
			}
		}

		public string muestraSolicitud
		{
			get
			{
				return (this.revocacion ? "Revocación" : "Asignación");
			}
		}
	}
}
