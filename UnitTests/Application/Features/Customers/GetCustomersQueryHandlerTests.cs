using Application.Abstractions.Data;
using Application.Features.Customers.Get;
using Domain.Entities.Customers;
using Moq;
using Moq.EntityFrameworkCore;

namespace UnitTests.Application.Features.Customers
{
    public class GetCustomersQueryHandlerTests
    {
        private readonly Mock<IApplicationDbContext> _mockContext;
        private readonly GetCustomersQueryHandler _handler;

        public GetCustomersQueryHandlerTests()
        {
            _mockContext = new Mock<IApplicationDbContext>();
            _handler = new GetCustomersQueryHandler(_mockContext.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnCustomers_WhenCustomersExist()
        {
            var customers = new List<Customer>
            {
                new Customer { Id = Guid.NewGuid(), Email = "john@example.com", Name = "John Doe", Phone = "+1234567890", Address = "123 Main St" },
                new Customer { Id = Guid.NewGuid(), Email = "jane@example.com", Name = "Jane Doe", Phone = "+0987654321", Address = "456 Elm St" }
            };

            _mockContext.Setup(c => c.Customers).ReturnsDbSet(customers);

            var query = new GetCustomersQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(2, result.Value.Count);

            Assert.Contains(result.Value, c => c.Email == "john@example.com");
            Assert.Contains(result.Value, c => c.Email == "jane@example.com");
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoCustomersExist()
        {
            // Arrange
            var customers = new List<Customer>(); // No customers
            _mockContext.Setup(c => c.Customers).ReturnsDbSet(customers);

            var query = new GetCustomersQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Empty(result.Value);
        }
    }
}
