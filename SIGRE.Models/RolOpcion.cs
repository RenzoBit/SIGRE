using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIGRE.Models
{
    [Table("rolopcion")]
	[DisplayColumn("idrol")]
	public partial class RolOpcion
    {
		public RolOpcion()
        {
            lrolopcion = new HashSet<RolOpcion>();
        }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
		[Display(Name = "Rol")]
		[Required(ErrorMessage = "Seleccione un rol")]
		public int idrol { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
		[Display(Name = "Página")]
		[Required(ErrorMessage = "Seleccione una opción")]
		public int idopcion { get; set; }

        [Display(Name = "Opción superior")]
		public int? idsuperior { get; set; }

        public virtual Opcion opcion { get; set; }

		public virtual Rol rol { get; set; }

		public virtual RolOpcion rolopcionsuperior { get; set; }

		public virtual ICollection<RolOpcion> lrolopcion { get; set; }
    }
}
