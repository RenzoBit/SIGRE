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
	[Table("colaborador")]
	[DisplayColumn("nombre")]
	public partial class Colaborador
	{
		public Colaborador()
        {
			lcentrocosto = new HashSet<CentroCosto>();
			lcolaborador = new HashSet<Colaborador>();
			liteminventario = new HashSet<ItemInventario>();
			lperfil = new HashSet<Perfil>();
			lrecursop = new HashSet<Recurso>();
			lcolaboradorperfil = new HashSet<ColaboradorPerfil>();
        }

		[Key]
		public int idcolaborador { get; set; }
		
		[Display(Name = "MUID / XMUID")]
		[Required(ErrorMessage = "Ingrese un MUID o XMUID")]
		[StringLength(7)]
		public string codigo { get; set; }
		
		[Display(Name = "Nombre")]
		[Required(ErrorMessage = "Ingrese un nombre")]
		[StringLength(100)]
		public string nombre { get; set; }
		
		[Display(Name = "Desactivado")]
		[Required(ErrorMessage = "Seleccione un valor para desactivado")]
		[DefaultValue(false)]
		public bool desactivado { get; set; }

		public virtual ICollection<ItemInventario> liteminventario { get; set; }

		[Display(Name = "NDA")]
		[Required(ErrorMessage = "Seleccione un valor para NDA")]
		[DefaultValue(false)]
		public bool nda { get; set; }
		
		[Display(Name = "Foto")]
		public byte[] foto { get; set; }
		
		[Display(Name = "COL_1")]
		public int? col_1 { get; set; }
		
		[Display(Name = "Propietario")]
		[Required(ErrorMessage = "Seleccione un propietario")]
		public int colaborador_idcolaborador { get; set; }
		
		[Display(Name = "Tipo de colaborador")]
		[Required(ErrorMessage = "Seleccione un tipo")]
		public int idcolaboradortipo { get; set; }
		
		[Display(Name = "Perfil")]
		public int? idperfil { get; set; }
		
		[Display(Name = "Usuario")]
		public int? idusuario { get; set; }
		
		[Display(Name = "Centro de costo")]
		[Required(ErrorMessage = "Seleccione un centro de costo")]
		public int idcentrocosto { get; set; }
		
		[Display(Name = "Aprobado")]
		public bool? aprobado { get; set; }
		
		[Display(Name = "Correo electrónico")]
		[StringLength(50)]
		[EmailAddress]
		public string correo { get; set; }
		
		[NotMapped]
		[Display(Name = "Unidad de negocio")]
		[Required(ErrorMessage = "Seleccione una unidad de negocio")]
		public int centrocosto_idcentrocosto { set; get; }
		
		[NotMapped]
		public string aprobador_nombre { set; get; }
		
		[NotMapped]
		[Display(Name = "Foto")]
		[DataType(DataType.Upload)]
		public HttpPostedFileBase file { set; get; }

		public virtual Colaborador aprobador { get; set; }
		public virtual ColaboradorTipo colaboradortipo { get; set; }
		public virtual Perfil perfil { get; set; }
		public virtual Usuario usuario { get; set; }
		public virtual CentroCosto centrocosto { get; set; }
		public virtual ICollection<Colaborador> lcolaborador { get; set; }
		public virtual ICollection<CentroCosto> lcentrocosto { get; set; }
		public virtual ICollection<Perfil> lperfil { get; set; }
		public virtual ICollection<Recurso> lrecursop { get; set; }
		//public virtual ICollection<Recurso> lrecursoa { get; set; }
		public virtual ICollection<ColaboradorPerfil> lcolaboradorperfil { get; set; }

		public string muestraCodigoNombre
		{
			get
			{
				return this.codigo + " - " + this.nombre;
			}
		}

		public string muestraEstado
		{
			get
			{
				return (this.aprobado == null ? "En aprobación" : (this.aprobado == true ? "Aprobado" : "Rechazado")) + " - " + (this.desactivado ? "Inactivo" : "Activo");
			}
		}

	}
}
