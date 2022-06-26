using EventSourcing.API.DTOs;
using MediatR;

namespace EventSourcing.API.Queries
{
    public class GetProductById : IRequest<ProductDto>
    {
        public Guid Id { get; set; }

        public GetProductById(Guid id)
        {
            Id = id;
        }
    }
}
