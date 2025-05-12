using AutoMapper;
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
    public class Productservicetest
    {
        private readonly Mock<IProductRepository> _mockProductRepo;
        private readonly Mock<ICategoryRepository> _mockCategoryRepo;
        private readonly IMapper _mapper;
        private readonly Productservice _productService;

        public Productservicetest()
        {
            _mockProductRepo = new Mock<IProductRepository>();
            _mockCategoryRepo = new Mock<ICategoryRepository>();

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Product, ProductDTO>().ReverseMap();
                cfg.CreateMap<Category, CategoryDTO>().ReverseMap();
            });

            _mapper = mapperConfig.CreateMapper();
            _productService = new Productservice(_mockProductRepo.Object, _mockCategoryRepo.Object, _mapper);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllProducts()
        {
            var products = new List<Product>
        {
            new Product { Id = 1, Name = "Laptop" },
            new Product { Id = 2, Name = "Monitor" }
        };
            _mockProductRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(products);

            var result = await _productService.GetAllAsync();

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCorrectProduct()
        {
            var product = new Product { Id = 1, Name = "Laptop" };
            _mockProductRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(product);

            var result = await _productService.GetByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal("Laptop", result.Name);
        }

        [Fact]
        public async Task AddAsync_ShouldMapAndCallRepository()
        {
            var dto = new ProductDTO { Id = 3, Name = "Tablet", Price = 250, Stock = 10, CategoryId = 1 };

            await _productService.AddAsync(dto);

            _mockProductRepo.Verify(repo => repo.AddAsync(It.Is<Product>(
                p => p.Name == "Tablet" && p.Price == 250 && p.Stock == 10 && p.CategoryId == 1
            )), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldMapAndCallRepository()
        {
            var dto = new ProductDTO { Id = 1, Name = "Updated", Price = 500, Stock = 20, CategoryId = 2 };

            await _productService.UpdateAsync(dto);

            _mockProductRepo.Verify(repo => repo.UpdateAsync(It.Is<Product>(
                p => p.Id == 1 && p.Name == "Updated" && p.Price == 500 && p.Stock == 20 && p.CategoryId == 2
            )), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldCallRepositoryWithCorrectId()
        {
            await _productService.DeleteAsync(4);

            _mockProductRepo.Verify(repo => repo.DeleteAsync(4), Times.Once);
        }

        [Fact]
        public async Task GetProductsByIdsAsync_ShouldReturnMappedDtos()
        {
            var ids = new List<int> { 1, 2 };
            var products = new List<Product>
        {
            new Product { Id = 1, Name = "Mouse" },
            new Product { Id = 2, Name = "Keyboard" }
        };
            _mockProductRepo.Setup(repo => repo.GetProductsByIdsAsync(ids)).ReturnsAsync(products);

            var result = await _productService.GetProductsByIdsAsync(ids);

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task ImportStockAsync_ShouldAddNewProductAndCategory()
        {
            var stockDtos = new List<StockDTO>
        {
            new StockDTO { Name = "Webcam", Price = 100, Quantity = 5, Categories = new List<string> { "Accessories" } }
        };

            _mockCategoryRepo.Setup(r => r.GetByNameAsync("Accessories")).ReturnsAsync((Category)null);
            _mockProductRepo.Setup(r => r.GetByNameAsync("Webcam")).ReturnsAsync((Product)null);

            await _productService.ImportStockAsync(stockDtos);

            _mockCategoryRepo.Verify(r => r.AddAsync(It.Is<Category>(c => c.Name == "Accessories")), Times.Once);
            _mockProductRepo.Verify(r => r.AddAsync(It.Is<Product>(p => p.Name == "Webcam" && p.Stock == 5)), Times.Once);
        }

        [Fact]
        public async Task ImportStockAsync_ShouldUpdateExistingProduct()
        {
            var existingCategory = new Category { Id = 10, Name = "Accessories" };
            var existingProduct = new Product { Id = 1, Name = "Webcam", Stock = 3, CategoryId = 10 };

            var stockDtos = new List<StockDTO>
        {
            new StockDTO { Name = "Webcam", Price = 100, Quantity = 2, Categories = new List<string> { "Accessories" } }
        };

            _mockCategoryRepo.Setup(r => r.GetByNameAsync("Accessories")).ReturnsAsync(existingCategory);
            _mockProductRepo.Setup(r => r.GetByNameAsync("Webcam")).ReturnsAsync(existingProduct);

            await _productService.ImportStockAsync(stockDtos);

            _mockProductRepo.Verify(r => r.UpdateAsync(It.Is<Product>(
                p => p.Name == "Webcam" && p.Stock == 5 // 3 + 2
            )), Times.Once);
        }
    }
}


