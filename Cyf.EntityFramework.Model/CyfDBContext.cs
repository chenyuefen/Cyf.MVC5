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

        public virtual DbSet<Acount> acounts { get; set; }
        public virtual DbSet<Company> companies { get; set; }
        public virtual DbSet<Employee> employees { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Acount>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<Acount>()
                .Property(e => e.account)
                .IsUnicode(false);

            modelBuilder.Entity<Acount>()
                .Property(e => e.password)
                .IsUnicode(false);

            modelBuilder.Entity<Acount>()
                .Property(e => e.email)
                .IsUnicode(false);

            modelBuilder.Entity<Company>()
                .Property(e => e.company_name)
                .IsUnicode(false);

            modelBuilder.Entity<Company>()
                .Property(e => e.company_position)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.position)
                .IsUnicode(false);
        }
    }
}
