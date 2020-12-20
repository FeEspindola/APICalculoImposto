using System.Net.Http;
using NUnit.Framework;

namespace CalculadoraImposto.Test.Suporte
{
    public class TesteBaseApi : TesteBaseBanco
    {
        protected HttpClient _httpClient;

        [SetUp]
        public void SetUpHttpClient()
        {
            _httpClient = AmbienteTestes.Factory.CreateClient();
        }
    
        
    }
}