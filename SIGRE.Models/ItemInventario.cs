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
	[Table("iteminventario")]
	[DisplayColumn("descripcion")]
	public partial class ItemInventario
	{
		[Key]
		public int iditeminventario { get; set; }

		[Display(Name = "Categoría de inventario")]
		[Required(ErrorMessage = "Seleccione un categoría de inventario")]
		public int idcategoriainventario { get; set; }

		[Display(Name = "Colaborador")]
		public int? idcolaborador { get; set; }

		[Display(Name = "Descripción")]
		[Required(ErrorMessage = "Ingrese una descripción")]
		[StringLength(50)]
		public string descripcion { get; set; }

		[Display(Name = "Fecha de modificación")]
		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
		public DateTime? fechamodificacion { get; set; }

		[Display(Name = "Detalle de modificación")]
		[StringLength(100)]
		public string detallemodificacion { get; set; }

		[Display(Name = "Estado")]
		[Required(ErrorMessage = "Seleccione un estado")]
        [StringLength(1)]
		[DefaultValue("D")]
		public string tipooperacion { get; set; }

		[NotMapped]
		[Display(Name = "Préstamo")]
		[DefaultValue(false)]
		public bool prestamo { set; get; }

		[NotMapped]
		public string colaborador_nombre { set; get; }

		public virtual CategoriaInventario categoriainventario { get; set; }

		public virtual Colaborador colaborador { get; set; }

		public virtual IList<ItemInventarioDetalle> liteminventariodetalle { get; set; }

		public const string DISPONIBLE = "D";
		public const string PRESTADO = "P";
		public const string ASIGNADO = "A";
		public const string BAJA = "B";

		public string muestraOperacion
		{
			get
			{
				switch (this.tipooperacion)
				{
					case "D":
						return "Disponible";
					case "P":
						return "Prestado";
					case "A":
						return "Asignado";
					case "B":
						return "De baja";
					default:
						return "Otro";
				}
			}
		}

		public string muestraTieneDatos
		{
			get
			{
				string res = "No";
				foreach (ItemInventarioDetalle o in liteminventariodetalle)
				{
					if (o.valorbusqueda != null && o.valorbusqueda.Trim() != "")
					{
						res = "Si";
						break;
					}
				}
				return res;
			}
		}

	}
}
