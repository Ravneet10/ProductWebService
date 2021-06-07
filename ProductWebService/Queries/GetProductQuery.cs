using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductWebService.Queries
{
    public class GetProductQuery : IRequest<List<ProductDto>>
    {
        public string ProductId { get; set; }
    }
}
