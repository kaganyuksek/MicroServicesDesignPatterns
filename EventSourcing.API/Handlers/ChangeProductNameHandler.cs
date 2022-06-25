using EventSourcing.API.Commands;
using EventSourcing.API.EventStores;
using MediatR;

namespace EventSourcing.API.Handlers
{
    public class ChangeProductNameHandler : IRequestHandler<ChangeProductNameCommand>
    {
        private readonly ProductStream Stream;

        public ChangeProductNameHandler(ProductStream stream)
        {
            Stream = stream;
        }

        public async Task<Unit> Handle(ChangeProductNameCommand request, CancellationToken cancellationToken)
        {
            Stream.NameChanged(request.ChangeProductNameDto);

            await Stream.SaveAsync();

            return Unit.Value;
        }
    }
}
