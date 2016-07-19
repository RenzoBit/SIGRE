using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIGRE.Models
{
    [Table("tipodatoreferencia")]
	[DisplayColumn("descripcion")]
	public partial class TipoDatoReferencia
    {
        public TipoDatoReferencia()
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
		[Display(Name = "Referencia")]
		[Required(ErrorMessage = "Seleccione una referencia")]
		public int idreferencia { get; set; }

		[Display(Name = "Descripción")]
		[Required(ErrorMessage = "Ingrese una descripción")]
        [StringLength(30)]
        public string descripcion { get; set; }

		[Display(Name = "Valor entero")]
		public int? valorentero { get; set; }

        public virtual TipoDato tipodato { get; set; }

		public virtual ICollection<TipoDatoAtributo> ltipodatoatributo { get; set; }

		public const short HOY = 1;
		public const short CERO = 2;
		public const short UNO = 3;
    }
}
