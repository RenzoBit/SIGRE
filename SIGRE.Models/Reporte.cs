namespace SIGRE.Models
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	[Table("reporte")]
	[DisplayColumn("descripcion")]
	public partial class Reporte
	{
		public Reporte()
		{
			lcategoriareporte = new HashSet<CategoriaReporte>();
		}

		[Key]
		public int idreporte { get; set; }

		[Display(Name = "Descripción")]
		[Required(ErrorMessage = "Ingrese una descripción")]
		[StringLength(30)]
		public string descripcion { get; set; }

		public ICollection<CategoriaReporte> lcategoriareporte { get; set; }
	}
}