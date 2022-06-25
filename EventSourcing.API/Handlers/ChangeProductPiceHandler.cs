using EventSourcing.API.Commands;
using EventSourcing.API.EventStores;
using MediatR;

namespace EventSourcing.API.Handlers
{
    public class ChangeProductPiceHandler : IRequestHandler<ChangeProductPriceCommand>
    {
        private readonly ProductStream Stream;

        public ChangeProductPiceHandler(ProductStream stream)
        {
            Stream = stream;
        }

        public async Task<Unit> Handle(ChangeProductPriceCommand request, CancellationToken cancellationToken)
        {
            Stream.PriceChanged(request.ChangeProductPriceDto);

            await Stream.SaveAsync();

            return Unit.Value;
        }
    }
}
