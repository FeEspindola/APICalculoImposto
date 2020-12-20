using System.Threading.Tasks;
using CalculadoraImposto.API;
using CalculadoraImposto.API.Dados;
using CalculadoraImposto.Test.Suporte;
using FluentAssertions;
using NUnit.Framework;

namespace CalculadoraImposto.Test.Specs
{
    public class HistoricoCalculoRepositorioTestes : TesteBaseBanco
    {
        [Test]
        public async Task ObterTodosDeveRetornarTodosHistoricosDoBanco()
        {
            // Arrange
            var historico = new HistoricoCalculo
            {
                ValorCalculado = 3000,
                ValorSalario = 2550
            };
            _contextParaTestes.Add(historico);
            _contextParaTestes.SaveChanges();
            var repositorio = GetService<HistoricoCalculoRepositorio>();

            // Act
            var historicosDoBanco = await repositorio.ObterTodos();

            // Assert
            historicosDoBanco.Should().BeEquivalentTo(historico);
        }
    }
}