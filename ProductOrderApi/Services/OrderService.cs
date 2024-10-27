using ProductOrderApi.Data.Entities;
using ProductOrderApi.Data.Models;
using ProductOrderApi.Data.Repositories;

namespace ProductOrderApi.Services
{
    public class OrderService
    {
        private readonly OrderRepository _orderRepository;
        private readonly ProductRepository _productRepository;
        private readonly OrderProductRepository _orderProductRepository;

        public OrderService(OrderRepository orderRepository, ProductRepository productRepository, OrderProductRepository orderProductRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _orderProductRepository = orderProductRepository;
        }
        public async Task<IEnumerable<Order>> GetOrders()
        {
            return await _orderRepository.GetOrdersAsync();
        }
        public async Task<Order?> GetOrder(int id)
        {
            return await _orderRepository.GetOrderAsync(id);

        }
        public async Task<Order> CreateOrder(CreateOrderModel order)
        {
            var orderNew = new Order
            {
                OrderDate = DateTime.Now,
            };

            var orderEntity = await _orderRepository.CreateOrderAsync(orderNew);

            var orderProductList = new List<OrderProduct>();

            if (order.OrderProducts is not null && order.OrderProducts.Any())
            {
                foreach (var oProduct in order.OrderProducts)
                {
                    orderProductList.Add(
                        new OrderProduct
                        {
                            OrderId = orderEntity.Id,
                            ProductId = oProduct.ProductId,
                            Quantity = oProduct.Quantity,
                        }
                    );
                }
                orderEntity.OrderProducts = orderProductList;
                orderEntity.TotalPrice = await _orderProductRepository.GetPrice(orderEntity.Id);
            }

            await _orderProductRepository.AddOrderProductsRange(orderProductList);

            await _orderRepository.SaveChangesAsync();

            return orderEntity;
        }
        public async Task<Order> UpdateOrder(Order order)
        {
            return await _orderRepository.UpdateOrderAsync(order);
        }
        public async Task DeleteOrder(int id)
        {
            await _orderRepository.DeleteOrderAsync(id);
        }
    }
}
