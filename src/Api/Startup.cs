using Api.Utils;
using Logic.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandler>();

            app.UseRouting();

            app.UseCors(builder =>
            {
                builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });

            app.UseEndpoints(endPoints =>
            {
                endPoints.MapControllers();
            });
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddCors()
                .AddControllers();

            services.AddSingleton(new SessionFactory(@"Server=(local);Database=Cqrs;Trusted_Connection=true"));
            services.AddScoped<UnitOfWork>();
        }
    }
}