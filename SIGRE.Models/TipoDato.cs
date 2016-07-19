using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGRE.Models
{
	[Table("tipodato")]
	[DisplayColumn("descripcion")]
	public partial class TipoDato
	{
		public TipoDato()
        {
			lcategoriadetalle = new HashSet<CategoriaDetalle>();
			ltipodatoformato = new HashSet<TipoDatoFormato>();
			ltipodatooperador = new HashSet<TipoDatoOperador>();
			ltipodatoatributo = new HashSet<TipoDatoAtributo>();
			ltipodatoreferencia = new HashSet<TipoDatoReferencia>();
        }

		[Key]
		public int idtipodato { get; set; }
		
		[Display(Name = "Descripción")]
		[Required(ErrorMessage = "Ingrese una descripción")]
		[StringLength(30)]
		public string descripcion { get; set; }

		public virtual ICollection<CategoriaDetalle> lcategoriadetalle { get; set; }

		public virtual ICollection<TipoDatoFormato> ltipodatoformato { get; set; }

		public virtual ICollection<TipoDatoOperador> ltipodatooperador { get; set; }

		public virtual ICollection<TipoDatoAtributo> ltipodatoatributo { get; set; }

		public virtual ICollection<TipoDatoReferencia> ltipodatoreferencia { get; set; }

		public const short TEXTO = 1;
		public const short ALFANUMERICO = 2;
		public const short FECHA = 3;
		public const short NUMERO = 4;
		public const short MONTO = 5;
	}
}
