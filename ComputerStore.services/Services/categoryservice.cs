using AutoMapper;
using ComputerStore.data.Interfaces;
using ComputerStore.services.DTOs;
using ComputerStore.services.Interfaces;

namespace ComputerStore.services.Services
{
    public class Categoryservice : Icategoryservice
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public Categoryservice(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryDTO>> GetAllAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<CategoryDTO>>(categories);
        }

        public async Task<CategoryDTO> GetByIdAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            return _mapper.Map<CategoryDTO>(category);
        }

        public async Task AddAsync(CategoryDTO categoryDto)
        {
            var category = _mapper.Map<data.entities.Category>(categoryDto);
            await _categoryRepository.AddAsync(category);
        }

        public async Task UpdateAsync(CategoryDTO categoryDto)
        {
            var category = _mapper.Map<data.entities.Category>(categoryDto);
            await _categoryRepository.UpdateAsync(category);
        }

        public async Task DeleteAsync(int id)
        {
            await _categoryRepository.DeleteAsync(id);
        }
    }
}