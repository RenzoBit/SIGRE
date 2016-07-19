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
	[Table("iteminventariodetalle")]
	[DisplayColumn("iditeminventariodetalle")]
	public partial class ItemInventarioDetalle
	{
		[Key]
		public int iditeminventariodetalle { get; set; }

		[Display(Name = "Ítem de inventario")]
		[Required(ErrorMessage = "Seleccione un ítem de inventario")]
		public int iditeminventario { get; set; }

		[Display(Name = "Detalle de categoría")]
		[Required(ErrorMessage = "Seleccione un detalle de categoría")]
		public int idcategoriadetalle { get; set; }

		[Display(Name = " ")]
		[StringLength(1000)]
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string valorcadena { get; set; }

		[Display(Name = "tipo entero")]
		public int? valorentero { get; set; }

		[Display(Name = "tipo decimal")]
		public decimal? valordecimal { get; set; }

		[Display(Name = "tipo fecha")]
		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
		public DateTime? valorfecha { get; set; }

		[Display(Name = "Valor de búsqueda")]
		[StringLength(1000)]
        public string valorbusqueda { get; set; }

		[Display(Name = "Monto")]
		[Required(ErrorMessage = "Seleccione un valor para monto")]
		[DefaultValue(false)]
		public bool monto { get; set; }

		public virtual ItemInventario iteminventario { get; set; }

		public virtual CategoriaDetalle categoriadetalle { get; set; }

		public string formato
		{
			get
			{
				return "{0:0." + (new String('0', categoriadetalle.tipodatoformato.escala)) + "}";
			}
		}
	}
}
