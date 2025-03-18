using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Moq;
using TemplateMVC.Core.Models.Products;
using TemplateMVC.Core.Services.Products;
using TemplateMVC.Core.Repositories.Products;
using Microsoft.AspNetCore.Http;
using TemplateMVC.Core.Models;

public class CategoryServicesTests
{
    private readonly Mock<AppDbContext> _mockDbContext;
    private readonly Mock<CategoryRepository> _mockRepo; 
    private readonly Mock<IHttpContextAccessor> _mockHttpContext;
    private readonly CategoryService _service;

    public CategoryServicesTests()
    {
        _mockDbContext = new Mock<AppDbContext>(); // Mock do DbContext
        _mockRepo = new Mock<CategoryRepository>(_mockDbContext.Object);
        _mockHttpContext = new Mock<IHttpContextAccessor>();
        _service = new CategoryService(_mockRepo.Object, _mockHttpContext.Object);
    }

    [Fact]
    public async Task GetAllCategoriesNotPaginated_ShouldReturnCategoryList()
    {
        // Arrange
        var categories = new List<Category>()
        {
            new Category { UniqueId = Guid.NewGuid(), CategoryName = "HR", Description = "HR Department" },
            new Category { UniqueId = Guid.NewGuid(), CategoryName = "IT", Description = "IT Department" }
        };

        _mockRepo.Setup(r => r.GetAllNotPaginatedAsync()).ReturnsAsync(categories);

        // Act
        var result = await _service.GetAllNotPaginated();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }
}