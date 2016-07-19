using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGRE.Models
{
	public class BDSIGRE : DbContext
	{
		public BDSIGRE()
			: base("DefaultConnection")
		{
		}

		public DbSet<CategoriaDetalle> CategoriaDetalle { get; set; }
		public DbSet<CategoriaInventario> CategoriaInventario { get; set; }
		public DbSet<CategoriaInventarioTipo> CategoriaInventarioTipo { get; set; }
		public DbSet<CategoriaReporte> CategoriaReporte { get; set; }
		public DbSet<CentroCosto> CentroCosto { get; set; }
		public DbSet<Colaborador> Colaborador { get; set; }
		public DbSet<ColaboradorPerfil> ColaboradorPerfil { get; set; }
		public DbSet<ColaboradorTipo> ColaboradorTipo { get; set; }
		public DbSet<ItemInventario> ItemInventario { get; set; }
		public DbSet<ItemInventarioDetalle> ItemInventarioDetalle { get; set; }
		public DbSet<Opcion> Opcion { get; set; }
		public DbSet<Perfil> Perfil { get; set; }
		public DbSet<PerfilRecurso> PerfilRecurso { get; set; }
		public DbSet<Recurso> Recurso { get; set; }
		public DbSet<Reporte> Reporte { get; set; }
		public DbSet<Rol> Rol { get; set; }
		public DbSet<RolOpcion> RolOpcion { get; set; }
		public DbSet<TipoDato> TipoDato { get; set; }
		public DbSet<TipoDatoAtributo> TipoDatoAtributo { get; set; }
		public DbSet<TipoDatoFormato> TipoDatoFormato { get; set; }
		public DbSet<TipoDatoOperador> TipoDatoOperador { get; set; }
		public DbSet<TipoDatoReferencia> TipoDatoReferencia { get; set; }
		public DbSet<Usuario> Usuario { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<CentroCosto>()
				.HasMany(e => e.lcentrocosto)
				.WithOptional(e => e.unidadnegocio)
				.HasForeignKey(e => e.centrocosto_idcentrocosto);

			modelBuilder.Entity<CentroCosto>()
				.HasMany(e => e.lcolaborador)
				.WithRequired(e => e.centrocosto)
				.HasForeignKey(e => e.idcentrocosto)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<CentroCosto>()
				.HasMany(e => e.lperfil)
				.WithRequired(e => e.unidadnegocio)
				.HasForeignKey(e => e.idcentrocosto)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<Colaborador>()
				.HasMany(e => e.lcentrocosto)
				.WithRequired(e => e.propietario)
				.HasForeignKey(e => e.idcolaborador)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<Colaborador>()
				.HasMany(e => e.lcolaborador)
				.WithRequired(e => e.aprobador)
				.HasForeignKey(e => e.colaborador_idcolaborador)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<Colaborador>()
				.HasMany(e => e.lperfil)
				.WithRequired(e => e.propietario)
				.HasForeignKey(e => e.idcolaborador)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<Colaborador>()
				.HasMany(e => e.lrecursop)
				.WithRequired(e => e.propietario)
				.HasForeignKey(e => e.idcolaborador)
				.WillCascadeOnDelete(false);

			/*
			modelBuilder.Entity<Colaborador>()
				.HasMany(e => e.lrecursoa)
				.WithRequired(e => e.aprobador)
				.HasForeignKey(e => e.colaborador_idcolaborador)
				.WillCascadeOnDelete(false);
			*/

			modelBuilder.Entity<Colaborador>()
				.HasMany(e => e.lcolaboradorperfil)
				.WithRequired(e => e.colaborador)
				.HasForeignKey(e => e.idcolaborador)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<Perfil>()
				.Property(e => e.costo)
				.HasPrecision(11, 2);

			modelBuilder.Entity<Perfil>()
				.HasMany(e => e.lcolaborador)
				.WithOptional(e => e.perfil)
				.HasForeignKey(e => e.idperfil);

			modelBuilder.Entity<Perfil>()
				.HasMany(e => e.lperfilrecurso)
				.WithRequired(e => e.perfil)
				.HasForeignKey(e => e.idperfil)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<Perfil>()
				.HasMany(e => e.lcolaboradorperfil)
				.WithRequired(e => e.perfil)
				.HasForeignKey(e => e.idperfil)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<PerfilRecurso>()
				.Property(e => e.montocalculado)
				.HasPrecision(11, 2);

			modelBuilder.Entity<Recurso>()
				.Property(e => e.costo)
				.HasPrecision(11, 2);

			modelBuilder.Entity<Recurso>()
				.HasMany(e => e.lperfilrecurso)
				.WithRequired(e => e.recurso)
				.HasForeignKey(e => e.idrecurso)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<Reporte>()
				.HasMany(e => e.lcategoriareporte)
				.WithRequired(e => e.reporte)
				.WillCascadeOnDelete(false);




			modelBuilder.Entity<CategoriaDetalle>()
				.HasMany(e => e.liteminventariodetalle)
				.WithRequired(e => e.categoriadetalle)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<CategoriaInventario>()
				.HasMany(e => e.lcategoriadetalle)
				.WithRequired(e => e.categoriainventario)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<CategoriaInventario>()
				.HasMany(e => e.lcategoriareporte)
				.WithRequired(e => e.categoriainventario)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<CategoriaInventario>()
				.HasMany(e => e.liteminventario)
				.WithRequired(e => e.categoriainventario)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<CategoriaInventarioTipo>()
				.HasMany(e => e.lcategoriainventario)
				.WithRequired(e => e.categoriainventariotipo)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<ItemInventario>()
				.HasMany(e => e.liteminventariodetalle)
				.WithRequired(e => e.iteminventario)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<ItemInventarioDetalle>()
				.Property(e => e.valordecimal)
				.HasPrecision(11, 4);

			modelBuilder.Entity<TipoDato>()
				.HasMany(e => e.lcategoriadetalle)
				.WithRequired(e => e.tipodato)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<TipoDato>()
				.HasMany(e => e.ltipodatoformato)
				.WithRequired(e => e.tipodato)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<TipoDato>()
				.HasMany(e => e.ltipodatooperador)
				.WithRequired(e => e.tipodato)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<TipoDato>()
				.HasMany(e => e.ltipodatoatributo)
				.WithRequired(e => e.tipodato)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<TipoDato>()
				.HasMany(e => e.ltipodatoreferencia)
				.WithRequired(e => e.tipodato)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<TipoDatoFormato>()
				.HasMany(e => e.lcategoriadetalle)
				.WithRequired(e => e.tipodatoformato)
				.HasForeignKey(e => new { e.idtipodato, e.idformato })
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<TipoDatoOperador>()
				.HasMany(e => e.ltipodatoatributo)
				.WithOptional(e => e.tipodatooperador)
				.HasForeignKey(e => new { e.idtipodato, e.idoperador });

			modelBuilder.Entity<TipoDatoReferencia>()
				.HasMany(e => e.ltipodatoatributo)
				.WithOptional(e => e.tipodatoreferencia)
				.HasForeignKey(e => new { e.idtipodato, e.idreferencia });





			modelBuilder.Entity<Opcion>()
				.HasMany(e => e.lopcion)
				.WithOptional(e => e.opcionsuperior)
				.HasForeignKey(e => e.idsuperior);

			modelBuilder.Entity<Opcion>()
				.HasMany(e => e.lrolopcion)
				.WithRequired(e => e.opcion)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<Rol>()
				.HasMany(e => e.lrolopcion)
				.WithRequired(e => e.rol)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<Rol>()
				.HasMany(e => e.lusuario)
				.WithRequired(e => e.rol)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<RolOpcion>()
				.HasMany(e => e.lrolopcion)
				.WithOptional(e => e.rolopcionsuperior)
				.HasForeignKey(e => new { e.idrol, e.idsuperior });
		}



	}
}
