using ComputerStore.data;
using ComputerStore.data.entities;
using ComputerStore.data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ComputerStore.Core.Repositories
{
    public class Productrepository : IProductRepository
    {
        private readonly ComputerStoreContext _context;

        public Productrepository(ComputerStoreContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products
            .Include(p => p.Category)
                .ToListAsync();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await _context.Products
            .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Product> GetByNameAsync(string name)
        {
            return await _context.Products
                .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Name == name);
        }

        public async Task<IEnumerable<Product>> GetProductsByIdsAsync(IEnumerable<int> ids)
        {
            return await _context.Products
                .Where(p => ids.Contains(p.Id))
                .ToListAsync();
        }
    }
}