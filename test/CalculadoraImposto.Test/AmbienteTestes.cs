using CalculadoraImposto.API;
using CalculadoraImposto.API.Dados;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace CalculadoraImposto.Test
{
    [SetUpFixture]
    public class AmbienteTestes
    {

        public static WebApplicationFactory<Startup> Factory;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            Factory = new WebApplicationFactory<Startup>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {


                }).UseEnvironment("Test");
            });

            using var scopeMigration = Factory.Services.CreateScope();
            var impostoRendaContext = scopeMigration.ServiceProvider.GetService<ImpostoRendaContext>();

            impostoRendaContext.Database.EnsureDeleted();
            impostoRendaContext.Database.Migrate();

        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            Factory.Dispose();
        }
    }
}