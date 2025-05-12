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
    public class Categoryservicetest
    {
        private readonly Mock<ICategoryRepository> _mockCategoryRepository;
        private readonly IMapper _mapper;
        private readonly Categoryservice _categoryService;

        public Categoryservicetest()
        {
            _mockCategoryRepository = new Mock<ICategoryRepository>();

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Category, CategoryDTO>().ReverseMap();
            });

            _mapper = mapperConfig.CreateMapper();
            _categoryService = new Categoryservice(_mockCategoryRepository.Object, _mapper);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllCategories()
        {
            var categories = new List<Category>
        {
            new Category { Id = 1, Name = "Laptop" },
            new Category { Id = 2, Name = "Desktop" }
        };
            _mockCategoryRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(categories);

            var result = await _categoryService.GetAllAsync();

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCorrectCategory()
        {
            var category = new Category { Id = 1, Name = "Laptop" };
            _mockCategoryRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(category);

            var result = await _categoryService.GetByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal("Laptop", result.Name);
        }

        [Fact]
        public async Task AddAsync_ShouldMapAndCallRepository()
        {
            var categoryDto = new CategoryDTO { Id = 3, Name = "Tablet", Description = "Mobile device" };

            await _categoryService.AddAsync(categoryDto);

            _mockCategoryRepository.Verify(repo => repo.AddAsync(It.Is<Category>(
                c => c.Name == "Tablet" && c.Description == "Mobile device"
            )), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldMapAndCallRepository()
        {
            var categoryDto = new CategoryDTO { Id = 1, Name = "UpdatedName", Description = "UpdatedDescription" };

            await _categoryService.UpdateAsync(categoryDto);

            _mockCategoryRepository.Verify(repo => repo.UpdateAsync(It.Is<Category>(
                c => c.Id == 1 && c.Name == "UpdatedName" && c.Description == "UpdatedDescription"
            )), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldCallRepositoryWithCorrectId()
        {
            await _categoryService.DeleteAsync(2);

            _mockCategoryRepository.Verify(repo => repo.DeleteAsync(2), Times.Once);
        }
    }
}
