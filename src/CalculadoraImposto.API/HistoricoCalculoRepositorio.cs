using System.Collections.Generic;
using System.Threading.Tasks;
using CalculadoraImposto.API.Dados;
using Microsoft.EntityFrameworkCore;

namespace CalculadoraImposto.API
{
    public class HistoricoCalculoRepositorio
    {
        private readonly ImpostoRendaContext _context;

        public HistoricoCalculoRepositorio(ImpostoRendaContext context)
        {
            _context = context;
        }

        public async Task Inserir(HistoricoCalculo historicoCalculo)
        {
            await _context.AddAsync(historicoCalculo);
            await _context.SaveChangesAsync();
        }

        public async Task<IReadOnlyCollection<HistoricoCalculo>> ObterTodos() =>
            await _context.HistoricoCalculos.ToListAsync();
    }
}