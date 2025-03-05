using Application.Abstractions.Data;
using Application.Features.Customers.Delete;
using Domain.Entities.Customers;
using Moq;
using Moq.EntityFrameworkCore;

namespace UnitTests.Application.Features.Customers
{
    public class DeleteCustomerCommandHandlerTests
    {
        private readonly Mock<IApplicationDbContext> _mockContext;
        private readonly DeleteCustomerCommandHandler _handler;

        public DeleteCustomerCommandHandlerTests()
        {
            _mockContext = new Mock<IApplicationDbContext>();
            _handler = new DeleteCustomerCommandHandler(_mockContext.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenCustomerNotFound()
        {
            // Arrange
            var customers = new List<Customer>(); // No customers in DB
            _mockContext.Setup(c => c.Customers).ReturnsDbSet(customers);

            var command = new DeleteCustomerCommand(Guid.NewGuid());

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal("Customer.NotFound", result.Error.Code);
        }

        [Fact]
        public async Task Handle_ShouldRemoveCustomer_WhenCustomerExists()
        {
            // Arrange
            var customer = new Customer { Id = Guid.NewGuid(), Email = "test@example.com" };
            var customers = new List<Customer> { customer };

            _mockContext.Setup(c => c.Customers).ReturnsDbSet(customers);

            // ✅ Ensure the customer is removed properly
            _mockContext.Setup(m => m.Customers.Remove(It.IsAny<Customer>()))
                .Callback<Customer>(c => customers.Remove(c)); // ✅ Manually remove from list

            _mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var command = new DeleteCustomerCommand(customer.Id);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.DoesNotContain(customers, c => c.Id == customer.Id); // ✅ Customer should be gone
        }


        [Fact]
        public async Task Handle_ShouldRaiseDomainEvent_WhenCustomerIsDeleted()
        {
            // Arrange
            var customer = new Customer { Id = Guid.NewGuid(), Email = "test@example.com" };
            var customers = new List<Customer> { customer };

            _mockContext.Setup(c => c.Customers).ReturnsDbSet(customers);
            _mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var command = new DeleteCustomerCommand(customer.Id);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Single(customer.DomainEvents);
            Assert.IsType<CustomerDeletedDomainEvent>(customer.DomainEvents.First());
        }


    }
}
