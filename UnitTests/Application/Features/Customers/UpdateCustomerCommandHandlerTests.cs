using Application.Abstractions.Data;
using Application.Features.Customers.Update;
using Domain.Entities.Customers;
using Moq;
using Moq.EntityFrameworkCore;

namespace UnitTests.Application.Features.Customers
{
    public class UpdateCustomerCommandHandlerTests
    {
        private readonly Mock<IApplicationDbContext> _mockContext;
        private readonly UpdateCustomerCommandHandler _handler;

        public UpdateCustomerCommandHandlerTests()
        {
            _mockContext = new Mock<IApplicationDbContext>();
            _handler = new UpdateCustomerCommandHandler(_mockContext.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenCustomerNotFound()
        {
            // Arrange
            var customers = new List<Customer>(); // No customers in DB
            _mockContext.Setup(c => c.Customers).ReturnsDbSet(customers);

            var command = new UpdateCustomerCommand(Guid.NewGuid(), "new@example.com", "New Name", "+1234567890", "New Address");

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal("Customer.NotFound", result.Error.Code);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenEmailIsAlreadyTaken()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var customers = new List<Customer>
            {
                new Customer { Id = customerId, Email = "old@example.com", Phone = "+1234567890" },
                new Customer { Id = Guid.NewGuid(), Email = "new@example.com", Phone = "+9876543210" } // Email already taken
            };

            _mockContext.Setup(c => c.Customers).ReturnsDbSet(customers);

            var command = new UpdateCustomerCommand(customerId, "new@example.com", "Updated Name", "+1234567890", "Updated Address");

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal("Customer.EmailNotUnique", result.Error.Code);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenPhoneNumberIsAlreadyTaken()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var customers = new List<Customer>
            {
                new Customer { Id = customerId, Email = "user@example.com", Phone = "+1111111111" },
                new Customer { Id = Guid.NewGuid(), Email = "another@example.com", Phone = "+1234567890" } // Phone already taken
            };

            _mockContext.Setup(c => c.Customers).ReturnsDbSet(customers);

            var command = new UpdateCustomerCommand(customerId, "user@example.com", "Updated Name", "+1234567890", "Updated Address");

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal("Customer.DuplicatePhoneNumber", result.Error.Code);
        }

        [Fact]
        public async Task Handle_ShouldUpdateCustomer_WhenValidDataProvided()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var customers = new List<Customer>
            {
                new Customer { Id = customerId, Email = "old@example.com", Phone = "+1234567890", Name = "Old Name", Address = "Old Address" }
            };

            _mockContext.Setup(c => c.Customers).ReturnsDbSet(customers);
            _mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var command = new UpdateCustomerCommand(customerId, "new@example.com", "Updated Name", "+9876543210", "Updated Address");

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);

            var updatedCustomer = customers.First(c => c.Id == customerId);
            Assert.Equal("new@example.com", updatedCustomer.Email);
            Assert.Equal("Updated Name", updatedCustomer.Name);
            Assert.Equal("+9876543210", updatedCustomer.Phone);
            Assert.Equal("Updated Address", updatedCustomer.Address);
        }

        [Fact]
        public async Task Handle_ShouldRaiseDomainEvent_WhenCustomerIsUpdated()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var customers = new List<Customer>
            {
                new Customer { Id = customerId, Email = "old@example.com", Phone = "+1234567890" }
            };

            _mockContext.Setup(c => c.Customers).ReturnsDbSet(customers);
            _mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var command = new UpdateCustomerCommand(customerId, "new@example.com", "Updated Name", "+9876543210", "Updated Address");

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            var updatedCustomer = customers.First(c => c.Id == customerId);
            Assert.Single(updatedCustomer.DomainEvents);
            Assert.IsType<CustomerUpdatedDomainEvent>(updatedCustomer.DomainEvents.First());
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenDatabaseErrorOccurs()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var customers = new List<Customer>
            {
                new Customer { Id = customerId, Email = "old@example.com", Phone = "+1234567890" }
            };

            _mockContext.Setup(c => c.Customers).ReturnsDbSet(customers);
            _mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Database failure"));

            var command = new UpdateCustomerCommand(customerId, "new@example.com", "Updated Name", "+9876543210", "Updated Address");

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Contains("Database failure", result.Error.Description);
        }
    }
}
