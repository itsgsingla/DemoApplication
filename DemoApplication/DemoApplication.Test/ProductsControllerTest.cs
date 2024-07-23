using DemoApplication.Controllers;
using DemoApplication.Models;
using DemoApplication.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Moq;

namespace DemoApplication.Test
{
	public class ProductsControllerTest
	{
		[Fact]
		public void Test_Create_GET_ReturnsViewResultNullModel()
		{
			// Arrange
			IProductRepository context = null;
			var controller = new ProductsController(context);

			// Act
			var result = controller.Create();

			// Assert
			var viewResult = Assert.IsType<ViewResult>(result);
			Assert.Null(viewResult.ViewData.Model);
		}


		[Fact]
		public async Task Test_Create_POST_InvalidModelState()
		{
			// Arrange
			var r = new Product()
			{
				Id = 4,
				Name = "Test Product 4",
				Price = 59,
				Description = "This is a test product"
			};
			var mockRepo = new Mock<IProductRepository>();
			mockRepo.Setup(repo => repo.CreateAsync(It.IsAny<Product>()));
			var controller = new ProductsController(mockRepo.Object);
			controller.ModelState.AddModelError("Name", "Name is required");

			// Act
			var result = await controller.Create(r);

			// Assert
			var viewResult = Assert.IsType<ViewResult>(result);
			Assert.Null(viewResult.ViewData.Model);
			mockRepo.Verify();
		}

		[Fact]
		public async Task Test_Create_POST_ValidModelState()
		{
			// Arrange
			var r = new Product()
			{
				Id = 4,
				Name = "Test Product 4",
				Price = 59,
				Description = "This is a test product"
			};

			var mockRepo = new Mock<IProductRepository>();
			mockRepo.Setup(repo => repo.CreateAsync(It.IsAny<Product>()))
				.Returns(Task.CompletedTask)
				.Verifiable();
			var controller = new ProductsController(mockRepo.Object);

			// Act
			var result = await controller.Create(r);

			// Assert
			var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
			Assert.Null(redirectToActionResult.ControllerName);
			Assert.Equal("Index", redirectToActionResult.ActionName);
			mockRepo.Verify();
		}

		[Fact]
		public async Task Test_Read_GET_ReturnsViewResult_WithAListOfProducts()
		{
			// Arrange
			var mockRepo = new Mock<IProductRepository>();
			mockRepo.Setup(repo => repo.ListAsync()).ReturnsAsync(GetTestProducts());
			var controller = new ProductsController(mockRepo.Object);

			// Act
			var result = await controller.Index();

			// Assert
			var viewResult = Assert.IsType<ViewResult>(result);
			var model = Assert.IsAssignableFrom<IEnumerable<Product>>(viewResult.ViewData.Model);
			Assert.Equal(3, model.Count());
		}

		private static List<Product> GetTestProducts()
		{
			var products = new List<Product>
			{
				new Product()
				{
					Id = 1,
					Name = "Product One",
					Price = 45,
					Description = "This is product 1"
				},
				new Product()
				{
					Id = 2,
					Name = "Product Two",
					Price = 49,
					Description = "This is product 2"
				},
				new Product()
				{
					Id = 3,
					Name = "Product Three",
					Price = 64,
					Description = "This is product 3"
				}
			};
			return products;
		}

		[Fact]
		public async Task Test_Update_GET_ReturnsViewResult_WithSingleProduct()
		{
			// Arrange
			int testId = 2;
			string testName = "test product";
			int testPrice = 60;
			string testDescription = "This is a test product";

			var mockRepo = new Mock<IProductRepository>();
			mockRepo.Setup(repo => repo.GetByIdAsync(testId)).ReturnsAsync(GetTestProduct());
			var controller = new ProductsController(mockRepo.Object);

			// Act
			var result = await controller.Edit(testId);

			// Assert
			var viewResult = Assert.IsType<ViewResult>(result);
			var model = Assert.IsAssignableFrom<Product>(viewResult.ViewData.Model);
			Assert.Equal(testId, model.Id);
			Assert.Equal(testName, model.Name);
			Assert.Equal(testPrice, model.Price);
			Assert.Equal(testDescription, model.Description);
		}

		private Product GetTestProduct()
		{
			var r = new Product()
			{
				Id = 2,
				Name = "test product",
				Price = 60,
				Description = "This is a test product"
			};
			return r;
		}

		[Fact]
		public async Task Test_Update_POST_ReturnsViewResult_InValidModelState()
		{
			// Arrange
			int testId = 2;
			Product r = GetTestProduct();

			var mockRepo = new Mock<IProductRepository>();
			mockRepo.Setup(repo => repo.GetByIdAsync(testId)).ReturnsAsync(GetTestProduct());

			var controller = new ProductsController(mockRepo.Object);
			controller.ModelState.AddModelError("Name", "Name is required");

			// Act
			var result = await controller.Edit(testId, r);

			// Assert
			var viewResult = Assert.IsType<ViewResult>(result);
			var model = Assert.IsAssignableFrom<Product>(viewResult.ViewData.Model);
			Assert.Equal(testId, model.Id);
		}

		[Fact]
		public async Task Test_Update_POST_ReturnsViewResult_ValidModelState()
		{
			// Arrange
			int testId = 2;
			var r = new Product()
			{
				Id = 2,
				Name = "Test Two",
				Price = 55,
				Description = "This is product 2"
			};
			var mockRepo = new Mock<IProductRepository>();
			mockRepo.Setup(repo => repo.GetByIdAsync(testId)).ReturnsAsync(GetTestProduct());
			var controller = new ProductsController(mockRepo.Object);

			mockRepo.Setup(repo => repo.UpdateAsync(It.IsAny<Product>()))
				   .Returns(Task.CompletedTask)
				   .Verifiable();

			// Act
			var result = await controller.Edit(testId, r);

			// Assert
			var viewResult = Assert.IsType<ViewResult>(result);
			var model = Assert.IsAssignableFrom<Product>(viewResult.ViewData.Model);
			Assert.Equal(testId, model.Id);
			Assert.Equal(r.Name, model.Name);
			Assert.Equal(r.Price, model.Price);
			Assert.Equal(r.Description, model.Description);

			mockRepo.Verify();
		}

		[Fact]
		public async Task Test_Delete_POST_ReturnsViewResult_InValidModelState()
		{
			// Arrange
			int testId = 2;

			var mockRepo = new Mock<IProductRepository>();
			mockRepo.Setup(repo => repo.GetByIdAsync(testId)).ReturnsAsync(GetTestProduct());
			mockRepo.Setup(repo => repo.DeleteAsync(It.IsAny<int>()))
				   .Returns(Task.CompletedTask)
				   .Verifiable();

			var controller = new ProductsController(mockRepo.Object);

			// Act
			var result = await controller.DeleteConfirmed(testId);

			// Assert
			var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
			Assert.Null(redirectToActionResult.ControllerName);
			Assert.Equal("Index", redirectToActionResult.ActionName);
			mockRepo.Verify();
		}
	}
}