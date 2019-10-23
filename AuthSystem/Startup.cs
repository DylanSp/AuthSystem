using AuthSystem.Adapters;
using AuthSystem.Interfaces;
using AuthSystem.Interfaces.Adapters;
using AuthSystem.Interfaces.Managers;
using AuthSystem.Managers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AuthSystem
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApiVersioning();

            // TODO - register other services
            services.AddTransient<IPermissionGrantManager, PermissionGrantManager>();
            services.AddTransient<IPermissionGrantAdapter, PostgresPermissionGrantAdapter>();
            services.AddTransient<IPostgresConnectionContext>(sp =>
            {
                // TODO - load connection string from config file
                var connectionString = "User ID=postgres;Password=adminpass;Host=localhost;Port=5432;Database=AuthSystem;";
                var connectionContext = new PostgresConnectionContext(connectionString);
                return connectionContext;
            });

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
