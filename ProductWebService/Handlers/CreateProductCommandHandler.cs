using AutoMapper;
using Domain;
using Entity;
using ProductWebService.Command;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace ProductWebService.Handlers
{
    public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand>
    {
        private readonly IProductContext _productContext;
        private readonly IMapper _mapper;
        public CreateProductCommandHandler(
          IProductContext productContext, IMapper mapper)
        {
            _productContext = productContext;
            _mapper = mapper;
        }

        public async Task ExecuteAsync(CreateProductCommand command)
        {
            var productEntity = await _productContext.Products
                .Where(x => x.Id == command.Id)
                .FirstOrDefaultAsync();
            if (productEntity != null)
            {
                throw new Exception($"Product with id {command.Id} already exists.");
            }

            productEntity = _mapper.Map<ProductEntity>(command);

            _productContext.Products.Add(productEntity);
            await _productContext.SaveChangesAsync();
        }
    }
}
