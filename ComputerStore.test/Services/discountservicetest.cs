using ComputerStore.data.entities;
using ComputerStore.data.Interfaces;
using ComputerStore.services.DTOs;
using ComputerStore.services.Interfaces;
using ComputerStore.services.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerStore.test.Services
{
    public class Discountservicetest
    {
        private readonly Mock<IProductRepository> _mockProductRepo;
        private readonly Discountservice _discountService;

        public Discountservicetest()
        {
            _mockProductRepo = new Mock<IProductRepository>();
            _discountService = new Discountservice(_mockProductRepo.Object);
        }

        [Fact]
        public async Task CalculateDiscountAsync_ShouldApply5Percent_WhenMultipleItemsInSameCategory()
        {
            // Arrange
            var cart = new ShoppingbasketDTO
            {
                Items = new List<BasketDTO>
                {
                    new BasketDTO { ProductId = 1, Quantity = 1 },
                    new BasketDTO { ProductId = 2, Quantity = 1 }
                }
            };

            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Laptop", Price = 1000, Stock = 10, CategoryId = 1 },
                new Product { Id = 2, Name = "Monitor", Price = 500, Stock = 10, CategoryId = 1 }
            };

            _mockProductRepo
                .Setup(repo => repo.GetProductsByIdsAsync(It.IsAny<IEnumerable<int>>()))
                .ReturnsAsync(products);

            // Act
            var discount = await _discountService.CalculateDiscountAsync(cart);

            // Assert
            var expectedDiscount = (1000 * 0.05m) + (500 * 0.05m); // 50 + 25 = 75
            Assert.Equal(expectedDiscount, discount);
        }

        [Fact]
        public async Task CalculateDiscountAsync_ShouldNotApplyDiscount_WhenOnlyOneItemInCategory()
        {
            var cart = new ShoppingbasketDTO
            {
                Items = new List<BasketDTO>
                {
                    new BasketDTO { ProductId = 1, Quantity = 1 },
                    new BasketDTO { ProductId = 2, Quantity = 1 }
                }
            };

            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Laptop", Price = 1000, Stock = 10, CategoryId = 1 },
                new Product { Id = 2, Name = "Mouse", Price = 100, Stock = 10, CategoryId = 2 }
            };

            _mockProductRepo
                .Setup(repo => repo.GetProductsByIdsAsync(It.IsAny<IEnumerable<int>>()))
                .ReturnsAsync(products);

            var discount = await _discountService.CalculateDiscountAsync(cart);

            Assert.Equal(0, discount);
        }

        [Fact]
        public async Task CalculateDiscountAsync_ShouldThrowException_IfProductNotFound()
        {
            var cart = new ShoppingbasketDTO
            {
                Items = new List<BasketDTO>
                {
                    new BasketDTO { ProductId = 99, Quantity = 1 }
                }
            };

            _mockProductRepo
                .Setup(repo => repo.GetProductsByIdsAsync(It.IsAny<IEnumerable<int>>()))
                .ReturnsAsync(new List<Product>());

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _discountService.CalculateDiscountAsync(cart));
        }

        [Fact]
        public async Task CalculateDiscountAsync_ShouldThrowException_IfInsufficientStock()
        {
            var cart = new ShoppingbasketDTO
            {
                Items = new List<BasketDTO>
                {
                    new BasketDTO { ProductId = 1, Quantity = 5 }
                }
            };

            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Laptop", Price = 1000, Stock = 3, CategoryId = 1 }
            };

            _mockProductRepo
                .Setup(repo => repo.GetProductsByIdsAsync(It.IsAny<IEnumerable<int>>()))
                .ReturnsAsync(products);

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _discountService.CalculateDiscountAsync(cart));
        }
    }
}
