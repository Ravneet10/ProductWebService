using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public interface IProductContext : System.IDisposable
    {
        IDbSet<ProductEntity> Products { get; set; }
        Task<int> SaveChangesAsync();
        List<ProductEntity> getProducts();
    }
}
