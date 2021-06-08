using Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProductWebService.Command;
using ProductWebService.Queries;
using ProductWebService.Validator;
using System.Collections.Generic;
using System.Threading.Tasks;
using TFNValidationApp.Validator;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProductWebService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICommandBus _commandBus;
        public ProductsController(IMediator mediator, ICommandBus commandBus)
        {
            _mediator = mediator;
            _commandBus = commandBus;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var clientLeaveReasonsResponse = await _mediator.Send(new GetProductQuery() { });
            return Ok(clientLeaveReasonsResponse);
        }

        // POST api/<ProductsController>
        [HttpPost]
        public async Task<IActionResult> Post(CreateProductCommand createProductCommand)
        {
            var validationResults = new CreateProductCommandValidator().Validate(createProductCommand);
            if (!validationResults.IsValid)
            {
                var errorResponse = new ValidationErrorsResponse(validationResults);
                return BadRequest(errorResponse);
            }
            await _commandBus.ExecuteAsync(createProductCommand);
            return Ok();
        }

    }
}
