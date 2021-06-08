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
        public DbSet<ProductEntity> Product { get; set; }

        public ProductContext(DbContextOptions<ProductContext> options) : base(options)
        {
            LoadDefaultProducts();
        }

        public List<ProductEntity> getProducts() => Product.Local.ToList<ProductEntity>();
        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }
        private void LoadDefaultProducts()
        {
            Product.Add(new ProductEntity { Id = Guid.Parse("B662E56A-067D-4BF6-AB78-A97C005C373A"), Name = "Tom", Description="test" });
            Product.Add(new ProductEntity { Id = Guid.Parse("1466881F-24B0-4378-992E-A97C0066445C"), Name = "Arthur", Description="test" });
        }
	}
}
