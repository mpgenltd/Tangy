using Tangy_Models;

namespace Tangy_DataAccess.Repositories
{
    public interface IOrderRepository
    {
        public Task<OrderDto> Get(int id);
        public Task<IEnumerable<OrderDto>> GetAll(string? userId = null, string? status = null);
        public Task<OrderDto> Create(OrderDto objDto);
        public Task<int> Delete(int id);

        public Task<OrderHeaderDto> UpdateHeader(OrderHeaderDto objDto);

        public Task<OrderHeaderDto> MarkPaymentSuccessful(int id);
        public Task<bool> UpdateOrderStatus(int orderId, string status);

        public Task<OrderHeaderDto> CancelOrder(int id);
    }
}
