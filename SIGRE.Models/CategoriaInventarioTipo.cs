using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGRE.Models
{
	[Table("categoriainventariotipo")]
	[DisplayColumn("descripcion")]
	public partial class CategoriaInventarioTipo
	{
        public CategoriaInventarioTipo()
        {
            lcategoriainventario = new HashSet<CategoriaInventario>();
        }

		[Key]
		public int idcategoriainventariotipo { get; set; }

		[Display(Name = "Descripción")]
		[Required(ErrorMessage = "Ingrese una descripción")]
		[StringLength(50)]
		public string descripcion { get; set; }

		public virtual ICollection<CategoriaInventario> lcategoriainventario { get; set; }
	}
}
