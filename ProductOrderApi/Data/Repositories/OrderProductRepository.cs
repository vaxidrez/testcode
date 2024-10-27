using Microsoft.EntityFrameworkCore;
using ProductOrderApi.Data.Entities;

namespace ProductOrderApi.Data.Repositories
{
    public class OrderProductRepository
    {
        private readonly OrderContext _context;
        public OrderProductRepository(OrderContext context)
        {
            _context = context;
        }
        public async Task AddOrderProductsRange(List<OrderProduct> orderProducts)
        {
            await _context.AddRangeAsync(orderProducts);
        }

        public async Task<decimal> GetPrice(int orderId)
        {
            var result = await _context.Orders
            .Include(o => o.OrderProducts)
            .ThenInclude(o => o.Product)
            .Where(o => o.Id == orderId)
            .Select(o => new
            {
                Price = o.OrderProducts.Sum(x => x.Product.Price * x.Quantity)
            }).FirstAsync();

            return result.Price;
        }

    }
}
