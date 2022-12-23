using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Tangy_DataAccess.Data;
using Tangy_DataAccess.ViewModels;
using Tangy_Models;

namespace Tangy_DataAccess.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public OrderRepository(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<OrderHeaderDto> CancelOrder(int id)
        {
            var orderHeader = await _db.OrderHeaders.FindAsync(id);
            if (orderHeader == null)
            {
                return new OrderHeaderDto();
            }

            if (orderHeader.Status == "Pending")
            {
                orderHeader.Status = "Cancelled";
                await _db.SaveChangesAsync();
            }

            if (orderHeader.Status == "Confirmed")
            {
                //refund
                // var options = new Stripe.RefundCreateOptions
                // {
                //     Reason= Stripe.RefundReasons.RequestedByCustomer,
                //     PaymentIntent = orderHeader.PaymentIntentId
                // };
                //
                // var service = new Stripe.RefundService();
                // Stripe.Refund refund = service.Create(options);

                orderHeader.Status = "Refunded";
                await _db.SaveChangesAsync();
            }

            return _mapper.Map<OrderHeader, OrderHeaderDto>(orderHeader);
        }

        public async Task<OrderDto> Create(OrderDto objDto)
        {
            try
            {
                var obj = _mapper.Map<OrderDto, Order>(objDto);
                _db.OrderHeaders.Add(obj.OrderHeader);
                await _db.SaveChangesAsync();

                foreach (var details in obj.OrderDetails)
                {
                    details.OrderHeaderId = obj.OrderHeader.Id;
                }

                _db.OrderDetails.AddRange(obj.OrderDetails);
                await _db.SaveChangesAsync();

                return new OrderDto()
                {
                    OrderHeader = _mapper.Map<OrderHeader, OrderHeaderDto>(obj.OrderHeader),
                    OrderDetails = _mapper.Map<IEnumerable<OrderDetail>, IEnumerable<OrderDetailDto>>(obj.OrderDetails)
                        .ToList()
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return objDto;
        }

        public async Task<int> Delete(int id)
        {
            var objHeader = await _db.OrderHeaders.FirstOrDefaultAsync(u => u.Id == id);
            if (objHeader != null)
            {
                IEnumerable<OrderDetail> objDetail = _db.OrderDetails.Where(u => u.OrderHeaderId == id);

                _db.OrderDetails.RemoveRange(objDetail);
                _db.OrderHeaders.Remove(objHeader);
                return _db.SaveChanges();
            }

            return 0;
        }

        public async Task<OrderDto> Get(int id)
        {
            Order order = new()
            {
                OrderHeader = _db.OrderHeaders.FirstOrDefault(u => u.Id == id),
                OrderDetails = _db.OrderDetails.Where(u => u.OrderHeaderId == id),
            };
            if (order != null)
            {
                return _mapper.Map<Order, OrderDto>(order);
            }

            return new OrderDto();
        }

        public async Task<IEnumerable<OrderDto>> GetAll(string? userId = null, string? status = null)
        {
            List<Order> OrderFromDb = new List<Order>();
            IEnumerable<OrderHeader> orderHeaderList = _db.OrderHeaders;
            IEnumerable<OrderDetail> orderDetailList = _db.OrderDetails;

            foreach (OrderHeader header in orderHeaderList)
            {
                Order order = new()
                {
                    OrderHeader = header,
                    OrderDetails = orderDetailList.Where(u => u.OrderHeaderId == header.Id),
                };
                OrderFromDb.Add(order);
            }
            //do some filtering #TODO

            return _mapper.Map<IEnumerable<Order>, IEnumerable<OrderDto>>(OrderFromDb);
        }

        public async Task<OrderHeaderDto> MarkPaymentSuccessful(int id)
        {
            var data = await _db.OrderHeaders.FindAsync(id);
            if (data == null)
            {
                return new OrderHeaderDto();
            }

            if (data.Status == "Pending")
            {
                data.Status = "Confirmed";
                await _db.SaveChangesAsync();
                return _mapper.Map<OrderHeader, OrderHeaderDto>(data);
            }

            return new OrderHeaderDto();
        }

        public async Task<OrderHeaderDto> UpdateHeader(OrderHeaderDto objDto)
        {
            if (objDto != null)
            {
                var orderHeaderFromDb = _db.OrderHeaders.FirstOrDefault(u => u.Id == objDto.Id);
                orderHeaderFromDb.Name = objDto.Name;
                orderHeaderFromDb.PhoneNumber = objDto.PhoneNumber;
                orderHeaderFromDb.Carrier = objDto.Carrier;
                orderHeaderFromDb.Tracking = objDto.Tracking;
                orderHeaderFromDb.StreetAddress = objDto.StreetAddress;
                orderHeaderFromDb.City = objDto.City;
                orderHeaderFromDb.State = objDto.State;
                orderHeaderFromDb.PostalCode = objDto.PostalCode;
                orderHeaderFromDb.Status = objDto.Status;

                await _db.SaveChangesAsync();
                return _mapper.Map<OrderHeader, OrderHeaderDto>(orderHeaderFromDb);
            }

            return new OrderHeaderDto();
        }

        public async Task<bool> UpdateOrderStatus(int orderId, string status)
        {
            var data = await _db.OrderHeaders.FindAsync(orderId);
            if (data == null)
            {
                return false;
            }

            data.Status = status;
            if (status == "Shipped")
            {
                data.ShippingDate = DateTime.Now;
            }

            await _db.SaveChangesAsync();
            return true;
        }
    }
}