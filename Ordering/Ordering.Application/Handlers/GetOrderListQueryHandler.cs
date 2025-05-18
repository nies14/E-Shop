using System.Text;
using MediatR;
using Ordering.Application.Mappers;
using Ordering.Application.Queries;
using Ordering.Application.Responses;
using Ordering.Core.Repositories;

namespace Ordering.Application.Handlers;

public class GetOrderListQueryHandler : IRequestHandler<GetOrderListQuery, List<OrderResponse>>
{
    private IOrderRepository _orderRepository;

    public GetOrderListQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<List<OrderResponse>> Handle(GetOrderListQuery request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetOrdersByUserName(request.UserName);

        return order?.Convert()?.ToList();
    }
}
