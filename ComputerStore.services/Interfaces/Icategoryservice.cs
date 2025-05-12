using ComputerStore.services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerStore.services.Interfaces
{
    public interface Icategoryservice
    {
        Task<IEnumerable<CategoryDTO>> GetAllAsync();
        Task<CategoryDTO> GetByIdAsync(int id);
        Task AddAsync(CategoryDTO category);
        Task UpdateAsync(CategoryDTO category);
        Task DeleteAsync(int id);

    }
}
