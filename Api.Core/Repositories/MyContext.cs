using Microsoft.EntityFrameworkCore;
using Api.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Core.Repositories
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions<MyContext> options)
           : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        //    {
        //        modelBuilder.Entity(entityType.ClrType).ToTable(entityType.ClrType.Name);
        //    }

        //}

        #region DBSets
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        
        #endregion
    }
}
