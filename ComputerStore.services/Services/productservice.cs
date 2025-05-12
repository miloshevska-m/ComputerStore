using AutoMapper;
using ComputerStore.data.Interfaces;
using ComputerStore.services.DTOs;
using ComputerStore.services.Interfaces;

namespace ComputerStore.services.Services
{
    public class Productservice : Iproductservice
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public Productservice(IProductRepository productRepository, ICategoryRepository categoryRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDTO>> GetAllAsync()
        {
            var products = await _productRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ProductDTO>>(products);
        }

        public async Task<ProductDTO> GetByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            return _mapper.Map<ProductDTO>(product);
        }

        public async Task AddAsync(ProductDTO productDto)
        {
            var product = _mapper.Map<data.entities.Product>(productDto);
            await _productRepository.AddAsync(product);
        }

        public async Task UpdateAsync(ProductDTO productDto)
        {
            var product = _mapper.Map<data.entities.Product>(productDto);
            await _productRepository.UpdateAsync(product);
        }

        public async Task DeleteAsync(int id)
        {
            await _productRepository.DeleteAsync(id);
        }

        public async Task ImportStockAsync(IEnumerable<StockDTO> stockDtos)
        {
            foreach (var stockDto in stockDtos)
            {
                if (stockDto.Categories == null || !stockDto.Categories.Any())
                    continue;

                var categoryName = stockDto.Categories.First().Trim();

                var category = await _categoryRepository.GetByNameAsync(categoryName);
                if (category == null)
                {
                    category = new data.entities.Category
                    {
                        Name = categoryName,
                        Description = null
                    };
                    await _categoryRepository.AddAsync(category);
                }

                var existingProduct = await _productRepository.GetByNameAsync(stockDto.Name);

                if (existingProduct == null)
                {
                    var newProduct = new data.entities.Product
                    {
                        Name = stockDto.Name,
                        Price = (int)Math.Round(stockDto.Price),
                        Stock = stockDto.Quantity,
                        CategoryId = category.Id
                    };
                    await _productRepository.AddAsync(newProduct);
                }
                else
                {
                    existingProduct.Stock += stockDto.Quantity;
                    await _productRepository.UpdateAsync(existingProduct);
                }
            }
        }

        public async Task<IEnumerable<ProductDTO>> GetProductsByIdsAsync(IEnumerable<int> ids)
        {
            var products = await _productRepository.GetProductsByIdsAsync(ids);
            return _mapper.Map<IEnumerable<ProductDTO>>(products);
        }
    }
}
