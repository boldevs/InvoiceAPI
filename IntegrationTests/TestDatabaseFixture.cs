using Application.Abstractions.Data;
using Infrastructure;
using Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace IntegrationTests
{
    public class TestDatabaseFixture : IAsyncLifetime
    {
        private readonly IServiceProvider _serviceProvider;
        public ApplicationDbContext DbContext => _serviceProvider.GetRequiredService<ApplicationDbContext>();
        public IApplicationDbContext ApplicationDbContext => _serviceProvider.GetRequiredService<IApplicationDbContext>();

        public TestDatabaseFixture()
        {
            var services = new ServiceCollection();

            // Configure in-memory database for testing
            services.AddDbContext<ApplicationDbContext>(static options =>
                options.UseInMemoryDatabase("InvoiceAppIntegrationTests"));

            // Add MediatR (mock publisher for testing)
            var mediatorMock = new Mock<IPublisher>();
            services.AddSingleton(mediatorMock.Object);

            // Add infrastructure services (simplified for testing)
            services.AddInfrastructure(new ConfigurationBuilder().Build());

            _serviceProvider = services.BuildServiceProvider();
        }
        public Task DisposeAsync()
        {
            throw new NotImplementedException();
        }

        public Task InitializeAsync()
        {
            throw new NotImplementedException();
        }

    }
}
