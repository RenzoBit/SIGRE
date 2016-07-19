using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGRE.Models
{
	[Table("colaboradortipo")]
	[DisplayColumn("descripcion")]
	public class ColaboradorTipo
	{
		[Key]
		public int idcolaboradortipo { get; set; }
		[Display(Name = "Descripción")]
		[Required(ErrorMessage = "Ingrese una descripción")]
		[StringLength(50)]
		public string descripcion { get; set; }

		public virtual ICollection<Colaborador> lcolaborador { get; set; }

		public const short INTERNO = 1;

		public const short EXTERNO = 2;
	}
}
