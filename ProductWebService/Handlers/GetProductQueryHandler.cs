using Domain;
using Entity;
using MediatR;
using ProductWebService.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProductWebService.Handlers
{
    public class GetProductQueryHandler : IRequestHandler<GetProductQuery, List<ProductDto>>
    {
        private readonly IProductContext _productContext;
        public GetProductQueryHandler(IProductContext productContext)
        {
            _productContext = productContext;
        }
        public Task<List<ProductDto>> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            var products = _productContext.getProducts().ToList();
            return Task.FromResult(products.Select(i => new ProductDto()
            {
                Id = i.Id,
                Name = i.Name,
                Description = i.Description,
                Price = i.Price
            }).ToList());
        }
    }
}
