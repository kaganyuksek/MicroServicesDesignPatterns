using EventSourcing.API.DTOs;
using EventSourcing.API.Models;
using EventSourcing.API.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EventSourcing.API.Handlers
{
    public class GetProductByIdHandler : IRequestHandler<GetProductById, ProductDto>
    {
        private readonly AppDbContext Context;

        public GetProductByIdHandler(AppDbContext context)
        {
            Context = context;
        }

        async Task<ProductDto> IRequestHandler<GetProductById, ProductDto>.Handle(GetProductById request, CancellationToken cancellationToken)
        {
            var product = await Context.Products.FirstOrDefaultAsync(p => p.Id == request.Id);

            if (product is null)
            {
                return null;
            }

            return new ProductDto(product.Name, product.Price, product.Stock);
        }
    }
}
