using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CalculadoraImposto.API.Dados;
using CalculadoraImposto.API.ImpostoRenda;
using Microsoft.EntityFrameworkCore;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace CalculadoraImposto.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private readonly IHostEnvironment _hostEnvironment;
        public Startup(IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            Configuration = configuration;
            _hostEnvironment = hostEnvironment;
        }

        
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();

            services.AddDbContext<ImpostoRendaContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("ImpostoRendaDatabase")));

            services.AddScoped<HistoricoCalculoRepositorio>();
            services.AddScoped<ImpostoRenda.CalculadoraImposto>();

            var tabelaJson = @"{""faixas"": [
                            {
                                ""valorInicial"": 0,
                                ""valorFinal"": 1903.98,
                                ""aliquota"": 0
                            },
                            {
                                ""valorInicial"": 1903.99,
                                ""valorFinal"": 2826.65,
                                ""aliquota"": 7.5
                            },
                            {
                                ""valorInicial"": 2826.66,
                                ""valorFinal"": 3751.05,
                                ""aliquota"": 15
                            },
                            {
                                ""valorInicial"": 3751.06,
                                ""aliquota"": 27.5
                            }
                            ]
                        }";

            if (!_hostEnvironment.IsEnvironment("Test"))
            {
                var server = WireMockServer.Start(7070);
                server.Given(Request.Create().UsingAnyMethod())
                    .RespondWith(Response.Create().WithBody(tabelaJson));
            }
            services.AddHttpClient<IServicoImpostoRenda, ServicoImpostoRenda>(x => x.BaseAddress = new Uri("http://localhost:7070/"));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CalculadoraImposto.API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CalculadoraImposto.API v1"));
            }

           // app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
