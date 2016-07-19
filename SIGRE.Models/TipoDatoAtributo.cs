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
	[Table("tipodatoatributo")]
	[DisplayColumn("descripcion")]
	public partial class TipoDatoAtributo
	{
		public TipoDatoAtributo()
        {
			lcategoriadetalle = new HashSet<CategoriaDetalle>();
        }

		[Key]
		public int idatributo { get; set; }
		
		[Display(Name = "Tipo de dato")]
		[Required(ErrorMessage = "Seleccione un tipo de dato")]
		public int idtipodato { get; set; }
		
		[Display(Name = "Código")]
		[Required(ErrorMessage = "Ingrese un código")]
		[StringLength(10)]
		public string codigo { get; set; }
		
		[Display(Name = "Descripción")]
		[Required(ErrorMessage = "Ingrese una descripción")]
		[StringLength(30)]
		public string descripcion { get; set; }

		[Display(Name = "Operador")]
		public int? idoperador { get; set; }

		[Display(Name = "Valor de referencia")]
		public int? idreferencia { get; set; }

		[Display(Name = "Valor cadena")]
		[StringLength(1000)]
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string valorcadena { get; set; }

		[Display(Name = "Valor entero")]
		public int? valorentero { get; set; }

		[Display(Name = "Etiqueta")]
		[StringLength(1000)]
		public string etiqueta { get; set; }

		public virtual TipoDato tipodato { get; set; }

		public virtual TipoDatoOperador tipodatooperador { get; set; }

		public virtual TipoDatoReferencia tipodatoreferencia { get; set; }

		public virtual ICollection<CategoriaDetalle> lcategoriadetalle { get; set; }

		public string muestraLabel
		{
			get
			{
				return descripcion + " " + etiqueta;
			}
		}
	}
}
