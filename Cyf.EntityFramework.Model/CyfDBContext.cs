namespace Cyf.EntityFramework.Model
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class CyfDBContext : DbContext
    {
        public CyfDBContext()
            : base("name=CyfDBContext")
        {
        }

        public virtual DbSet<company> companies { get; set; }
        public virtual DbSet<employee> employees { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<company>()
                .Property(e => e.company_name)
                .IsUnicode(false);

            modelBuilder.Entity<company>()
                .Property(e => e.company_position)
                .IsUnicode(false);

            modelBuilder.Entity<employee>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<employee>()
                .Property(e => e.position)
                .IsUnicode(false);
        }
    }
}
