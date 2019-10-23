using AuthSystem.Adapters;
using AuthSystem.Interfaces;
using AuthSystem.Interfaces.Adapters;
using AuthSystem.Interfaces.Managers;
using AuthSystem.Managers;
using AuthSystem.Services;
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

            services.AddTransient<IPermissionGrantManager, PermissionGrantManager>();
            services.AddTransient<IResourceManager, ResourceManager>();
            services.AddTransient<IUserManager, UserManager>();

            services.AddTransient<IPasswordService, PasswordService>();

            services.AddTransient<IPermissionGrantAdapter, PostgresPermissionGrantAdapter>();
            services.AddTransient<IResourceAdapter, PostgresResourceAdapter>();
            services.AddTransient<IUserAdapter, PostgresUserAdapter>();

            services.AddTransient<IPostgresConnectionContext>(sp =>
            {
                var connectionString = Configuration["ConnectionString"];
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
