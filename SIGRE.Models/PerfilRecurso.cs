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
	[Table("perfilrecurso")]
	[DisplayColumn("montocalculado")]
	public class PerfilRecurso
	{
		[Key]
		[Column(Order = 0)]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int idperfil { get; set; }
		[Key]
		[Column(Order = 1)]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int idrecurso { get; set; }
		[Display(Name = "Monto calculado")]
		[Required(ErrorMessage = "Ingrese un monto")]
		[DefaultValue(0)]
		public decimal montocalculado { get; set; }

		public virtual Perfil perfil { get; set; }
		public virtual Recurso recurso { get; set; }
	}
}
