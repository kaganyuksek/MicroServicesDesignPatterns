using EventSourcing.API.Commands;
using EventSourcing.API.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EventSourcing.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IMediator Mediator;

        public ProductController(IMediator mediator)
        {
            Mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductDto createProductDto)
        {
            await Mediator.Send(new CreateProductCommand() {  CreateProductDto = createProductDto });

            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> ChangeName(ChangeProductNameDto changeProductNameDto)
        {
            await Mediator.Send(new ChangeProductNameCommand() {  ChangeProductNameDto = changeProductNameDto });

            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> ChangePrice(ChangeProductPriceDto changeProductPriceDto)
        {
            await Mediator.Send(new ChangeProductPriceCommand() { ChangeProductPriceDto = changeProductPriceDto });

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await Mediator.Send(new DeleteProductCommand() { Id = id});

            return NoContent();
        }
    }
}
