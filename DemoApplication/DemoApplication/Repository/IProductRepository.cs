using DemoApplication.Models;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

namespace DemoApplication.Repository
{
	public interface IProductRepository
	{
		Task<Product> GetByIdAsync(int id);
		Task<List<Product>> ListAsync();
		Task CreateAsync(Product product);
		Task UpdateAsync(Product product);
		Task DeleteAsync(int id);
	}
}