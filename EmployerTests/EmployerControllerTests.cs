using Employer.Contexts;
using Employer.Controllers;
using Employer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace EmployerTests;

public class EmployerControllerTests
{
    private ApplicationDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;

        var context = new ApplicationDbContext(options);

        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        context.Users.AddRange(new List<User>
        {
            new User { Id = 1, Username = "ezequiel", Name = "Ezequiel Cardoso" },
            new User { Id = 2, Username = "john_doe", Name = "John Doe" }
        });

        context.SaveChanges();
        return context;
    }

    [Fact]
    public async Task GetUser_ShouldReturnOk_WhenUserExists()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var controller = new EmployerController(context);
        var username = "ezequiel";

        // Act
        var result = await controller.GetUser(username) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);

        var json = JsonSerializer.Serialize(result.Value);
        var response = JsonSerializer.Deserialize<Dictionary<string, string>>(json);

        Assert.NotNull(response);
        Assert.True(response.ContainsKey("Message"));
        Assert.Equal("Hello, Ezequiel Cardoso", response["Message"]);
    }

    [Fact]
    public async Task GetUser_ShouldReturnNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var controller = new EmployerController(context);
        var username = "non_existent_user";

        // Act
        var result = await controller.GetUser(username) as NotFoundObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(404, result.StatusCode);
        Assert.Equal("User not found.", result.Value);
    }

    [Fact]
    public async Task GetUser_ShouldReturnBadRequest_WhenUsernameIsEmpty()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var controller = new EmployerController(context);
        var username = "";

        // Act
        var result = await controller.GetUser(username) as BadRequestObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(400, result.StatusCode);
        Assert.Equal("Username is required.", result.Value);
    }
}