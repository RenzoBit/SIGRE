namespace SIGRE.Models
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	[Table("categoriareporte")]
	[DisplayColumn("idcategoriainventario")]
	public partial class CategoriaReporte
	{
		[Key]
		public int idcategoriareporte { get; set; }

		[Display(Name = "Categoría")]
		[Required(ErrorMessage = "Seleccione una categoría de inventario")]
		public int idcategoriainventario { get; set; }

		[Display(Name = "Reporte")]
		[Required(ErrorMessage = "Seleccione un reporte")]
		public int idreporte { get; set; }

		public CategoriaInventario categoriainventario { get; set; }

		public Reporte reporte { get; set; }
	}
}