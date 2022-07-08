namespace Dogstagram.WebApi.IntegrationTests
{
    using Dogstagram.WebApi.Data;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

                var descriptor = services
                .SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<DogstagramDbContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<DogstagramDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                    options.UseInternalServiceProvider(serviceProvider);
                });

                var sp = services.BuildServiceProvider();

                using var scope = sp.CreateScope();
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<DogstagramDbContext>();
                db.Database.EnsureCreated();

                try
                {
                    db.InitializeDbForTests();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
            });

        }
    }
}
