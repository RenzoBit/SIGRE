using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIGRE.Models
{
	[Table("opcion")]
	[DisplayColumn("link")]
	public partial class Opcion
    {
		public Opcion()
        {
			lopcion = new HashSet<Opcion>();
			lrolopcion = new HashSet<RolOpcion>();
        }

        [Key]
        public int idopcion { get; set; }

		[Display(Name = "Opci�n superior")]
		public int? idsuperior { get; set; }

		[Display(Name = "Link")]
		[Required(ErrorMessage = "Ingrese un link")]
        [StringLength(100)]
        public string link { get; set; }

		[Display(Name = "Action")]
		[Required(ErrorMessage = "Ingrese un action")]
        [StringLength(100)]
        public string action { get; set; }

		[Display(Name = "Controller")]
		[StringLength(100)]
        public string controller { get; set; }

		[Display(Name = "�rea")]
		[StringLength(100)]
        public string area { get; set; }

		[Display(Name = "A")]
		[Required(ErrorMessage = "Seleccione un valor para a")]
		[DefaultValue(false)]
		public bool a { get; set; }

		[Display(Name = "Sesi�n")]
		[Required(ErrorMessage = "Seleccione un valor para sesi�n")]
		[DefaultValue(0)]
		public int sesion { get; set; }

		[Display(Name = "Din�mico")]
		[Required(ErrorMessage = "Seleccione un valor para din�mico")]
		[DefaultValue(false)]
		public bool dinamico { get; set; }

		[NotMapped]
		[Display(Name = "Acceso")]
		[DefaultValue(false)]
		public bool acceso { set; get; }

		public virtual Opcion opcionsuperior { get; set; }

        public virtual ICollection<Opcion> lopcion { get; set; }

        public virtual ICollection<RolOpcion> lrolopcion { get; set; }
    }
}
