﻿using System.Threading.Tasks;
using CalculadoraImposto.API.Dados;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Respawn;

namespace CalculadoraImposto.Test.Suporte
{
    public class TesteBaseBanco
    {
        private static readonly Checkpoint _apagadorDeDados = new Checkpoint
        {
            TablesToIgnore = new[] {"__EFMigrationsHistory"}
        };

        protected static IServiceScope _scope;
        protected static ImpostoRendaContext _contextParaTestes;
        protected static T GetService<T>() => _scope.ServiceProvider.GetService<T>();
        [SetUp]
        public async Task SetUpScope()
        {
            _scope = AmbienteTestes.Factory.Services.CreateScope();
            _contextParaTestes = AmbienteTestes.Factory.Services.CreateScope().ServiceProvider
                .GetService<ImpostoRendaContext>();

            var configuration =
                (ConfigurationRoot)AmbienteTestes.Factory.Services.GetService(typeof(IConfiguration));
            var connectionString = configuration.GetConnectionString("ImpostoRendaDatabase");

            await _apagadorDeDados.Reset(connectionString);
        }
        // Dispose do scope do request
        [TearDown]
        public void TearDownScope()
        {
            _scope.Dispose();
        }
    }
}