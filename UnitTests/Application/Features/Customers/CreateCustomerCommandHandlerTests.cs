using Application.Abstractions.Data;
using Application.Features.Customers.Create;
using Domain.Entities.Customers;
using Moq;
using Moq.EntityFrameworkCore; // Correctly mocks DbSet<T> for async queries

namespace UnitTests.Application.Features.Customers;
public class CreateCustomerCommandHandlerTests
{
    private readonly Mock<IApplicationDbContext> _mockContext;
    private readonly CreateCustomerCommandHandler _handler;

    public CreateCustomerCommandHandlerTests()
    {
        _mockContext = new Mock<IApplicationDbContext>();
        _handler = new CreateCustomerCommandHandler(_mockContext.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenEmailIsNotUnique()
    {
        // Arrange
        var existingCustomer = new Customer { Email = "test@example.com", Phone = "1234567890" };
        var customers = new List<Customer> { existingCustomer };

        _mockContext.Setup(c => c.Customers)
            .ReturnsDbSet(customers); // Properly mock the DbSet

        var command = new CreateCustomerCommand { Email = "test@example.com", Name = "John Doe", Phone = "0987654321", Address = "123 Main St" };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(CustomerError.EmailNotUnique, result.Error);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenPhoneNumberIsDuplicate()
    {
        // Arrange
        var existingCustomer = new Customer { Email = "different@example.com", Phone = "1234567890" };
        var customers = new List<Customer> { existingCustomer };

        _mockContext.Setup(c => c.Customers)
            .ReturnsDbSet(customers);

        var command = new CreateCustomerCommand { Email = "new@example.com", Name = "Jane Doe", Phone = "1234567890", Address = "456 Elm St" };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(CustomerError.DuplicatePhoneNumber, result.Error);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenCustomerIsCreatedSuccessfully()
    {
        // Arrange
        var customers = new List<Customer>(); // No existing customers
        _mockContext.Setup(c => c.Customers)
            .ReturnsDbSet(customers);

        var command = new CreateCustomerCommand { Email = "valid@example.com", Name = "John Doe", Phone = "1234567890", Address = "789 Oak St" };

        _mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotEqual(Guid.Empty, result.Value);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenDatabaseErrorOccurs()
    {
        // Arrange
        var customers = new List<Customer>();
        _mockContext.Setup(c => c.Customers)
            .ReturnsDbSet(customers);

        var command = new CreateCustomerCommand { Email = "valid@example.com", Name = "John Doe", Phone = "1234567890", Address = "789 Oak St" };

        _mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ThrowsAsync(new Exception("Database failure"));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains("Database failure", result.Error.Description);
    }

    [Fact]
    public async Task Handle_ShouldRaiseDomainEvent_WhenCustomerIsCreated()
    {
        // Arrange
        var customers = new List<Customer>(); // Empty customer list

        // ✅ Use ReturnsDbSet() to mock DbSet<Customer>
        _mockContext.Setup(c => c.Customers)
            .ReturnsDbSet(customers);

        // ✅ Capture the added customer but do NOT manually raise domain events
        _mockContext.Setup(m => m.Customers.Add(It.IsAny<Customer>()))
            .Callback<Customer>(customer => customers.Add(customer));

        _mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var command = new CreateCustomerCommand
        {
            Email = "valid@example.com",
            Name = "John Doe",
            Phone = "1234567890",
            Address = "789 Oak St"
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);

        var addedCustomer = customers.FirstOrDefault(c => c.Email == "valid@example.com");
        Assert.NotNull(addedCustomer);

        // ✅ Ensure only ONE event is raised
        Assert.Single(addedCustomer.DomainEvents);
    }


}
