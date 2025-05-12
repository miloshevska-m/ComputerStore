using AutoMapper;
using ComputerStore.data.Interfaces;
using ComputerStore.services.DTOs;
using ComputerStore.services.Interfaces;

namespace ComputerStore.services.Services
{
    public class Discountservice : Idiscountservice
    {
        private readonly IProductRepository _productRepository;

        public Discountservice(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<decimal> CalculateDiscountAsync(ShoppingbasketDTO shoppingCart)
        {
            decimal totalDiscount = 0;
            var productIds = shoppingCart.Items.Select(item => item.ProductId).Distinct();
            var products = await _productRepository.GetProductsByIdsAsync(productIds);

            var categoryCounts = new Dictionary<int, int>();

            foreach (var item in shoppingCart.Items)
            {
                var product = products.FirstOrDefault(p => p.Id == item.ProductId);
                if (product == null)
                {
                    throw new InvalidOperationException($"Product with ID {item.ProductId} not found.");
                }

                if (item.Quantity > product.Stock)
                {
                    throw new InvalidOperationException($"Not enough stock for product {product.Name}. Available: {product.Stock}");
                }

                if (categoryCounts.ContainsKey(product.CategoryId))
                {
                    categoryCounts[product.CategoryId] += item.Quantity;
                }
                else
                {
                    categoryCounts[product.CategoryId] = item.Quantity;
                }
            }

            foreach (var item in shoppingCart.Items)
            {
                var product = products.FirstOrDefault(p => p.Id == item.ProductId);
                if (product != null && categoryCounts[product.CategoryId] > 1)
                {
                    totalDiscount += product.Price * 0.05m;
                }
            }

            return totalDiscount;
        }


    }
}
