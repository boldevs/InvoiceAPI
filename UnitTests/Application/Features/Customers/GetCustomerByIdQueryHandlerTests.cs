using Application.Abstractions.Data;
using Application.Features.Customers.GetById;
using Domain.Entities.Customers;
using Moq;
using Moq.EntityFrameworkCore;

namespace UnitTests.Application.Features.Customers
{
    public class GetCustomerByIdQueryHandlerTests
    {
        private readonly Mock<IApplicationDbContext> _mockContext;
        private readonly GetUserByIdQueryHandler _handler;

        public GetCustomerByIdQueryHandlerTests()
        {
            _mockContext = new Mock<IApplicationDbContext>();
            _handler = new GetUserByIdQueryHandler(_mockContext.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnCustomer_WhenCustomerExists()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var customers = new List<Customer>
            {
                new Customer { Id = customerId, Email = "john@example.com", Name = "John Doe", Phone = "+1234567890", Address = "123 Main St" }
            };

            _mockContext.Setup(c => c.Customers).ReturnsDbSet(customers);

            var query = new GetCustomerByIdQuery(customerId);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(customerId, result.Value.Id);
            Assert.Equal("john@example.com", result.Value.Email);
            Assert.Equal("John Doe", result.Value.Name);
            Assert.Equal("+1234567890", result.Value.Phone);
            Assert.Equal("123 Main St", result.Value.Address);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenCustomerDoesNotExist()
        {
            // Arrange
            var customers = new List<Customer>(); // No customers exist
            _mockContext.Setup(c => c.Customers).ReturnsDbSet(customers);

            var query = new GetCustomerByIdQuery(Guid.NewGuid());

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal("Customer.NotFound", result.Error.Code);
        }
    }
}
