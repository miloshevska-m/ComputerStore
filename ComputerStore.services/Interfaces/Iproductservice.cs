using ComputerStore.services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerStore.services.Interfaces
{
    public interface Iproductservice
    {
        Task<IEnumerable<ProductDTO>> GetAllAsync();
        Task<ProductDTO> GetByIdAsync(int id);
        Task AddAsync(ProductDTO product);
        Task UpdateAsync(ProductDTO product);
        Task DeleteAsync(int id);
        Task ImportStockAsync(IEnumerable<StockDTO> stockDtos);
        Task<IEnumerable<ProductDTO>> GetProductsByIdsAsync(IEnumerable<int> ids);
    }
}
