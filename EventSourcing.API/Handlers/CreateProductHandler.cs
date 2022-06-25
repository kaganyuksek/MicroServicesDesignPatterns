using EventSourcing.API.Commands;
using EventSourcing.API.EventStores;
using MediatR;

namespace EventSourcing.API.Handlers
{
    public class CreateProductHandler : IRequestHandler<CreateProductCommand>
    {
        private readonly ProductStream Stream;

        public CreateProductHandler(ProductStream stream)
        {
            Stream = stream;
        }

        public async Task<Unit> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            Stream.Created(request.CreateProductDto);
            
            await Stream.SaveAsync();

            return Unit.Value;
        }
    }
}
