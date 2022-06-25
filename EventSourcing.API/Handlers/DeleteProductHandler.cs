using EventSourcing.API.Commands;
using EventSourcing.API.EventStores;
using MediatR;

namespace EventSourcing.API.Handlers
{
    public class DeleteProductHandler : IRequestHandler<DeleteProductCommand>
    {
        private readonly ProductStream Stream;

        public DeleteProductHandler(ProductStream stream)
        {
            Stream = stream;
        }

        public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            Stream.Deleted(request.Id);

            await Stream.SaveAsync();

            return Unit.Value;
        }
    }
}
