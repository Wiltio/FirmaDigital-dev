using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Owin;
using Utn.FirmaDigital.Backend.Web.Api.Helpers;

namespace Utn.FirmaDigital.Backend.Web.Api
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
            services.AddOptions();

            var corsBuilder = new CorsPolicyBuilder();

            corsBuilder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();

            services.AddCors(options =>
            {
                options.AddPolicy("SiteCorsPolicy", corsBuilder.Build());
            });

            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            //configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
             .AddJwtBearer(options =>
             {
                 options.TokenValidationParameters =
                      new TokenValidationParameters
                      {
                          ValidateIssuer = false,
                          ValidateAudience = false,
                          ValidateLifetime = true,
                          ValidateIssuerSigningKey = true,

                          ValidIssuer = "Sevensoft",
                          ValidAudience = "Sevensoft",
                          IssuerSigningKey = JwtSecurityKey.Create(appSettings.Secret)
                      };
                 options.Events = new JwtBearerEvents
                 {
                     OnAuthenticationFailed = context =>
                      {
                          Console.WriteLine("OnAuthenticationFailed: " +
                              context.Exception.Message);
                          return Task.CompletedTask;
                      },
                     OnTokenValidated = context =>
                      {
                          Console.WriteLine("OnTokenValidated: " +
                              context.SecurityToken);
                          return Task.CompletedTask;
                      }
                 };
             });

            services.AddAntiforgery(options => options.HeaderName = "X-XSRF-TOKEN");

            services.AddMvc();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseCors("SiteCorsPolicy");
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


        }

    }
}

