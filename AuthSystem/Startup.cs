using AuthSystem.Adapters;
using AuthSystem.Authentication;
using AuthSystem.Data;
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
using Sodium;

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
            services.AddTransient<ISessionCookieManager, SessionCookieManager>();

            services.AddTransient<IPasswordService>(sp =>
            {
                // TODO - load from config
                var hashStrength = PasswordHash.StrengthArgon.Sensitive;
                return new PasswordService(hashStrength);
            });

            services.AddTransient<IPermissionGrantAdapter, PostgresPermissionGrantAdapter>();
            services.AddTransient<IResourceAdapter, PostgresResourceAdapter>();
            services.AddTransient<IUserAdapter, PostgresUserAdapter>();
            services.AddTransient<ISessionCookieAdapter, PostgresSessionCookieAdapter>();

            services.AddTransient<IPostgresConnectionContext>(sp =>
            {
                var connectionString = Configuration["ConnectionString"];
                var connectionContext = new PostgresConnectionContext(connectionString);
                return connectionContext;
            });

            services.AddControllers();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "Custom Scheme";
                options.DefaultChallengeScheme = "Custom Scheme";
            }).AddCustomAuth(o => { });
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

            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
