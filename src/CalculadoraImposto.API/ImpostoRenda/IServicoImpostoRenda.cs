using System.Threading.Tasks;

namespace CalculadoraImposto.API.ImpostoRenda
{
    public interface IServicoImpostoRenda
    {
        Task<decimal> ObterAliquota(decimal valor);
    }
}