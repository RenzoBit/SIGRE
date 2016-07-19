using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIGRE.Models
{
    [Table("tipodatooperador")]
	[DisplayColumn("descripcion")]
	public partial class TipoDatoOperador
    {
        public TipoDatoOperador()
        {
			ltipodatoatributo = new HashSet<TipoDatoAtributo>();
        }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
		[Display(Name = "Tipo de dato")]
		[Required(ErrorMessage = "Seleccione un tipo de dato")]
		public int idtipodato { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
		[Display(Name = "Operador")]
		[Required(ErrorMessage = "Seleccione un operador")]
		public int idoperador { get; set; }

		[Display(Name = "Descripción")]
		[Required(ErrorMessage = "Ingrese una descripción")]
		[StringLength(30)]
        public string descripcion { get; set; }

        public virtual TipoDato tipodato { get; set; }

		public virtual ICollection<TipoDatoAtributo> ltipodatoatributo { get; set; }

		public const short DIFERENTE = 1;
		public const short IGUAL = 2;
		public const short MAYOR = 3;
		public const short MENOR = 4;
		public const short MAYOR_IGUAL = 5;
		public const short MENOR_IGUAL = 6;
		public const short LIKE = 7;
    }
}
