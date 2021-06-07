using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Entity
{
	public class ProductContext :DbContext,IProductContext
	{
        public virtual System.Data.Entity.IDbSet<ProductEntity> Products { get; set; }
        public DbSet<ProductEntity> Users { get; set; }

        public ProductContext(DbContextOptions<ProductContext> options) : base(options)
        {
            LoadDefaultUsers();
        }

        public List<ProductEntity> getUsers() => Users.Local.ToList<ProductEntity>();
        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }
        private void LoadDefaultUsers()
        {
            Users.Add(new ProductEntity { Id = Guid.Parse("100L"), Name = "Tom" });
            Users.Add(new ProductEntity { Id = Guid.Parse("200L"), Name = "Arthur" });
        }
	}
}
