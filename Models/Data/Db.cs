using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MVC_Store.Models.Data
{
    public class Db : DbContext
    {
        public Db() : base("StoreContext")
        {}

        public DbSet<PagesDTO> Pages { get; set; }
        public DbSet<CountryDTO> Countries { get; set; }
        public DbSet<SidebarDTO> Sidebars { get; set; }
        public DbSet<CategoryDTO> Categories { get; set; }
        public DbSet<ProductDTO> Products { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CategoryDTO>()
                .HasMany(c => c.Products)
                .WithMany(p => p.Categories)
                .Map(t =>
                {
                    t.MapLeftKey("CategoryId");
                    t.MapRightKey("ProductId");
                    t.ToTable("tblCategoryProduct");
                });
        }

       // public System.Data.Entity.DbSet<MVC_Store.Models.ViewModels.Pages.PageVM> PageVMs { get; set; }

       // public System.Data.Entity.DbSet<MVC_Store.Models.ViewModels.Pages.SidebarVM> SidebarVMs { get; set; }

       //public System.Data.Entity.DbSet<MVC_Store.Models.ViewModels.Shop.CategoryVM> CategoryVMs { get; set; }
    }
}