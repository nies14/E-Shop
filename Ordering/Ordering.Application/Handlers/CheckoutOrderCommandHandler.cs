using MediatR;
using Ordering.Application.Commands;
using Ordering.Application.Mappers;
using Ordering.Core.Repositories;

namespace Ordering.Application.Handlers
{
    public class CheckoutOrderCommandHandler : IRequestHandler<CheckoutOrderCommand, int>
    {
        private readonly IOrderRepository _orderRepository;

        public CheckoutOrderCommandHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public async Task<int> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var orderEntity = request.Convert();
            var generatedOrder = await _orderRepository.AddAsync(orderEntity);
            return generatedOrder.Id;
        }
    }
}
