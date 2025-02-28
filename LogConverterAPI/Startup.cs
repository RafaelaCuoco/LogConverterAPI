using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using LogConverterAPI.Data;
using LogConverterAPI.Services;

namespace LogConverterAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // Este método é chamado pelo runtime para adicionar serviços ao contêiner.
        public void ConfigureServices(IServiceCollection services)
        {
            // Configuração do DbContext com SQL Server
            services.AddDbContext<LogContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // Registrar o serviço LogTransformerService
            services.AddScoped<LogTransformerService>();

            // Adiciona os controladores da API (usando AddMvc no .NET Core 2.1)
            services.AddMvc();

            // Configuração do Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Log Converter API",
                    Version = "v1",
                    Description = "API para transformar logs do formato 'MINHA CDN' para o formato 'Agora'.",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact
                    {
                        Name = "iTaaS Solution",
                        Email = "rafaela.cuoco@gmail.com",
                        Url = new System.Uri("https://www.itaas.c")
                    }
                });

                // Habilita os comentários XML para documentação detalhada
                var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = System.IO.Path.Combine(System.AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        // Este método é chamado pelo runtime para configurar o pipeline de requisições HTTP.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Habilita o middleware do Swagger
            app.UseSwagger();

            // Habilita o middleware do Swagger UI
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Log Converter API V1");
                c.RoutePrefix = string.Empty; // Define a página inicial como a UI do Swagger
            });

            app.UseHttpsRedirection();
            app.UseMvc(); 
        }
    }
}