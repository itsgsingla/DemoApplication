using DemoApplication.Data;
using DemoApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoApplication.Repository
{
	public class ProductRepository: IProductRepository
	{
		private readonly ApplicationDbContext context;

		public ProductRepository(ApplicationDbContext dbContext)
		{
			context = dbContext;
		}

		public Task<Product> GetByIdAsync(int id)
		{
			return context.Product.FirstOrDefaultAsync(s => s.Id == id);
		}

		public Task<List<Product>> ListAsync()
		{
			return context.Product.ToListAsync();
		}

		public Task CreateAsync(Product product)
		{
			context.Product.Add(product);
			return context.SaveChangesAsync();
		}

		public Task UpdateAsync(Product product)
		{
			context.Entry(product).State = EntityState.Modified;
			return context.SaveChangesAsync();
		}
		public async Task DeleteAsync(int id)
		{
			var r = await GetByIdAsync(id);
			context.Remove(r);
			await context.SaveChangesAsync();
		}
	}
}
