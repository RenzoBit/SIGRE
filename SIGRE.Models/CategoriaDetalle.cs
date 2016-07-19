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
	[Table("categoriadetalle")]
	[DisplayColumn("nombre")]
	public partial class CategoriaDetalle
	{
        public CategoriaDetalle()
        {
            liteminventariodetalle = new HashSet<ItemInventarioDetalle>();
        }

		[Key]
		public int idcategoriadetalle { get; set; }

		[Display(Name = "Categoría inventario")]
		[Required(ErrorMessage = "Seleccione una categoría")]
		public int idcategoriainventario { get; set; }

		[Display(Name = "Tipo de dato")]
		[Required(ErrorMessage = "Seleccione un tipo de dato")]
		public int idtipodato { get; set; }

		[Display(Name = "Atributo")]
		public int? idatributo { get; set; }

		[Display(Name = "Formato")]
		[Required(ErrorMessage = "Seleccione un formato")]
		public int idformato { get; set; }

		[Display(Name = "Nombre")]
		[Required(ErrorMessage = "Ingrese un nombre")]
		[StringLength(30)]
		public string nombre { get; set; }

		[Display(Name = "Identificador")]
		[Required(ErrorMessage = "Seleccione un valor para identicador")]
		[DefaultValue(false)]
		public bool identificador { get; set; }

		[Display(Name = "Obligatorio")]
		[Required(ErrorMessage = "Seleccione un valor para obligatorio")]
		[DefaultValue(false)]
		public bool obligatorio { get; set; }

		public virtual CategoriaInventario categoriainventario { get; set; }

		public virtual TipoDato tipodato { get; set; }

		public virtual TipoDatoAtributo tipodatoatributo { get; set; }

		public virtual TipoDatoFormato tipodatoformato { get; set; }

        public virtual ICollection<ItemInventarioDetalle> liteminventariodetalle { get; set; }

		public string muestraIdentificador
		{
			get
			{
				return this.identificador ? "Si" : "No";
			}
		}

		public string muestraObligatorio
		{
			get
			{
				return this.obligatorio ? "Si" : "No";
			}
		}
	}
}
