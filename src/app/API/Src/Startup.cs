using API.Src.Ioc;
using Autofac.Extensions.DependencyInjection;
using Business.Src;
using Domain.Src;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json.Converters;
using System;
using System.IO;

namespace API.Src
{
    internal class Startup
    {
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            //services.AddSingleton(t => ConfigurationReader.ReadConfig<ApplicationConfiguration>());

            services.AddDbContext<ApplicationContext>();

            services.AddCors();

            services.AddMvcCore(options => options.EnableEndpointRouting = false)
                .AddNewtonsoftJson(o =>
                {
                    o.SerializerSettings.Converters.Add(new StringEnumConverter());
                })
                .AddApiExplorer();

            services.AddSwaggerDocument(settings =>
            {
                settings.SchemaType = NJsonSchema.SchemaType.OpenApi3;
                settings.AllowReferencesWithProperties = true;
                settings.Title = "Language-Service";
            });

            var builder = AutofacBuilder.Build();

            // mediator 
            var businessAssembly = AppDomain.CurrentDomain.Load(BusinessAssembly.Value);
            services.AddMediatR(businessAssembly);
            services.AddHostedService<HostedService>();
            builder.Populate(services);
            return new AutofacServiceProvider(builder.Build());
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseCors(_ => _.AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin());

            app.UseMvcWithDefaultRoute();

            app.UseOpenApi().UseSwaggerUi3();

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            app.UseDefaultFiles(new DefaultFilesOptions()
            {
                FileProvider = new PhysicalFileProvider(path),
                RequestPath = new PathString("")
            });

            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(path),
                RequestPath = new PathString("")
            });
        }
    }
}
