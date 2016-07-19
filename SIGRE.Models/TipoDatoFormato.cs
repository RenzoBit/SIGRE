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
	[Table("tipodatoformato")]
	[DisplayColumn("descripcion")]
	public partial class TipoDatoFormato
	{
		public TipoDatoFormato()
        {
			lcategoriadetalle = new HashSet<CategoriaDetalle>();
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
		[Display(Name = "Formato")]
		[Required(ErrorMessage = "Seleccione un formato")]
		public int idformato { get; set; }

		[Display(Name = "Descripción de formato")]
		[Required(ErrorMessage = "Ingrese una descripción de formato")]
		[StringLength(30)]
		public string formato { get; set; }

		[Display(Name = "Longitud")]
		[Required(ErrorMessage = "Ingrese una longitud")]
		public int longitud { get; set; }

		[Display(Name = "Escala")]
		[Required(ErrorMessage = "Ingrese una escala")]
		public int escala { get; set; }

		[Display(Name = "Descripción")]
		[Required(ErrorMessage = "Descripción requerida")]
		[StringLength(60)]
		public string descripcion { get; set; }

		[Display(Name = "Tipo")]
		[Required(ErrorMessage = "Seleccione un tipo")]
		[StringLength(1)]
		public string tipo { get; set; }

		[Display(Name = "Activo")]
		[Required(ErrorMessage = "Seleccione un valor para activo")]
		[DefaultValue(false)]
		public bool activo { get; set; }

		public virtual TipoDato tipodato { get; set; }

		public virtual ICollection<CategoriaDetalle> lcategoriadetalle { get; set; }

		public const string CADENA = "C";
		public const string FECHA = "F";
		public const string ENTERO = "E";
		public const string DECIMAL = "D";
	}
}
