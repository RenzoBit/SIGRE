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
	[Table("categoriainventario")]
	[DisplayColumn("nombre")]
	public partial class CategoriaInventario
    {
        public CategoriaInventario()
        {
            lcategoriadetalle = new HashSet<CategoriaDetalle>();
			liteminventario = new HashSet<ItemInventario>();
			lcategoriareporte = new HashSet<CategoriaReporte>();
        }

		[Key]
		[Display(Name = "ID autogenerado")]
		public int idcategoriainventario { get; set; }

		[Display(Name = "Tipo de categoría")]
		[Required(ErrorMessage = "Seleccione un tipo")]
		public int idcategoriainventariotipo { get; set; }

		[Display(Name = "Nombre")]
		[Required(ErrorMessage = "Ingrese un nombre")]
		[StringLength(50)]
		public string nombre { get; set; }

		[Display(Name = "Descripción")]
		[StringLength(200)]
		public string descripcion { get; set; }

		[Display(Name = "Desactivado")]
		[Required(ErrorMessage = "Seleccione un valor para desactivado")]
		[DefaultValue(false)]
		public bool desactivado { get; set; }

		[Display(Name = "Múltiple")]
		[Required(ErrorMessage = "Seleccione un valor para múltiple")]
		[DefaultValue(false)]
		public bool multiple { get; set; }

		[Display(Name = "Utilizada")]
		[Required(ErrorMessage = "Seleccione un valor para utilizada")]
		[DefaultValue(false)]
		public bool utilizada { get; set; }

		public virtual CategoriaInventarioTipo categoriainventariotipo { get; set; }

		public virtual ICollection<CategoriaDetalle> lcategoriadetalle { get; set; }

		public virtual ICollection<ItemInventario> liteminventario { get; set; }

		public ICollection<CategoriaReporte> lcategoriareporte { get; set; }

		public string muestraAsignacion
		{
			get
			{
				return multiple ? "Si" : "No";
			}
		}

		public string muestraEstado
		{
			get
			{
				return (desactivado ? "Inactiva" : "Activa") + " - " + (utilizada ? "Utilizada" : "No utilizada");
			}
		}

		public int muestraCantidad
		{
			get
			{
				return lcategoriareporte.Count();
			}
		}
    }
}
