namespace Linn.ManufacturingEngineering.Service.Host
{
    using System.IdentityModel.Tokens.Jwt;
    using System.IO;

    using Linn.Common.Authentication.Host.Extensions;
    using Linn.Common.Logging;
    using Linn.Common.Service;
    using Linn.Common.Service.Extensions;
    using Linn.ManufacturingEngineering.IoC;
    using Linn.ManufacturingEngineering.Service.Host.Negotiators;
    using Linn.ManufacturingEngineering.Service.Models;

    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Diagnostics;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.FileProviders;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddCors();
            services.AddSingleton<IViewLoader, ViewLoader>();
            services.AddSingleton<IResponseNegotiator, HtmlNegotiator>();
            services.AddSingleton<IResponseNegotiator, UniversalResponseNegotiator>();

            services.AddLogging(builder =>
            {
                builder.ClearProviders();
                builder.AddConsole();
                builder.AddFilter("Microsoft", LogLevel.Warning);
                builder.AddFilter("System", LogLevel.Warning);
                builder.AddFilter("Linn", LogLevel.Information);
            });
            services.AddLog();

            services.AddServices();
            services.AddFacade();
            services.AddPersistence();
            services.AddHandlers();
            services.AddMessageDispatchers();

            var appSettings = ApplicationSettings.Get();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.Authority = appSettings.CognitoHost;
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = appSettings.CognitoHost,
                        ValidateAudience = false,
                        ValidAudience = appSettings.CognitoClientId,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true
                    };
                });

            services.AddAuthorization();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseStaticFiles(new StaticFileOptions
                {
                    RequestPath = "/manufacturing-engineering/build",
                    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "client", "build"))
                });
            }
            else
            {
                app.UseStaticFiles(new StaticFileOptions
                {
                    RequestPath = "/manufacturing-engineering/build",
                    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "app", "client", "build"))
                });
            }

            app.UseAuthentication();

            app.UseBearerTokenAuthentication();
            app.Use(
                (context, next) =>
                {
                    context.Response.Headers.Append("Vary", "Accept");
                    return next.Invoke();
                });
            app.UseExceptionHandler(
                c => c.Run(async context =>
                {
                    var exception = context.Features
                        .Get<IExceptionHandlerPathFeature>()
                        ?.Error;

                    var log = app.ApplicationServices.GetService<ILog>();
                    log.Error(exception?.Message, exception);

                    var response = new { error = $"{exception?.Message}  -  {exception?.StackTrace}" };
                    await context.Response.WriteAsJsonAsync(response);
                }));
            app.UseRouting();
            app.UseEndpoints(builder => { builder.MapEndpoints(); });
        }
    }
}
