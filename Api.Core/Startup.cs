using Api.Core.Infrastructure;
using Api.Core.Middleware;
using Api.Core.Repositories;
using Api.Core.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
//using Microsoft.IdentityModel.Tokens;
using System;

namespace Api.Core
{
    public class Startup
    {
        private readonly IHostEnvironment _environment;
        private AppSettings _appSettings;
        public IConfiguration Configuration { get; }
        private const string SecretKey = "iNivDmHLpUA223sqsfhqGbMRdRj1PVkH"; // todo: get this from somewhere secure
        //private readonly SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));


        public Startup(IConfiguration configuration, Microsoft.Extensions.Hosting.IHostEnvironment environment)
        {
            Configuration = configuration;
            _environment = environment;
            _appSettings = new AppSettings();
            Configuration.Bind("App", _appSettings);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var env = _environment.IsDevelopment() ? "Desa" : "Prod";
            services.AddControllers().AddNewtonsoftJson();

            services.AddCors(options =>
            {
                options.AddPolicy("cors",
                builder =>
                {
                    builder.WithOrigins(_appSettings.Cors.Split(new char[] { ',' }))
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                });
            });

            System.Net.ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => { return true; };

            services.AddDbContext<MyContext>(options => options.UseMySql(_appSettings.ConnectionStrings[env], ServerVersion.AutoDetect(_appSettings.ConnectionStrings[env])));

            //services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddSwaggerGen();

            // configure strongly typed settings object
            services.Configure<AppSettings>(Configuration.GetSection("App"));
            services.AddScoped<IEmailService, EmailService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, MyContext context)
        {
            context.Database.Migrate();

            app.UseSwagger();
            app.UseSwaggerUI(x => { x.SwaggerEndpoint("v1/swagger.json", ".NET Sign-up and Verification API");
                                    x.RoutePrefix = string.Empty;  // Set Swagger UI at apps root
            });

            app.UseGlobalExceptionHandler();
            app.UseCors("cors");
            app.UseHttpsRedirection();
            app.UseRouting();

            // global error handler
            //app.UseMiddleware<ErrorHandlerMiddleware>();

            // custom jwt auth middleware
            app.UseMiddleware<JwtMiddleware>();

            //app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            BootStrapper.BootStrap();
        }
    }
}
