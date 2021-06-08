using Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using ProductWebService.Handlers;
using ProductWebService.Queries;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;

namespace ProductServiceTest
{
    [TestClass]
    public class GetProductQueryHandlerTest
    {
        protected IProductContext _productContext { get; set; }
        private IDbSet<ProductEntity> Product { get; set; }

        private readonly GetProductQueryHandler _getProductQueryHandler;
        public List<ProductEntity> getProducts() => Product.Local.ToList<ProductEntity>();

        public GetProductQueryHandlerTest()
        {
            _productContext = Substitute.For<IProductContext>();
            CreateProducts();
            _getProductQueryHandler = CreateQueryHandler();
        }
        protected void CreateProducts()
        {
            var product = new List<ProductEntity>();
            product.Add(new ProductEntity { Id = Guid.Parse("B662E56A-067D-4BF6-AB78-A97C005C373A"), Name = "Tom", Description = "test" });
            product.Add(new ProductEntity { Id = Guid.Parse("1466881F-24B0-4378-992E-A97C0066445C"), Name = "Arthur", Description = "test" });
            _productContext.Products = (IDbSet<ProductEntity>)product;
        }

            private GetProductQueryHandler CreateQueryHandler()
        {
            return new GetProductQueryHandler(_productContext);
        }
        [TestMethod]
        public async System.Threading.Tasks.Task QueryingAllProducts_ReturnsCorrectResults()
        {

            var query = new GetProductQuery();

            var results = await _getProductQueryHandler.Handle(query,CancellationToken.None);

            Assert.Equals(2, results.Count());
        }


    }
}
