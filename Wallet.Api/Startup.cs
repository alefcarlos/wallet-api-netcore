using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.PlatformAbstractions;

namespace Wallet.Api
{
    using IoC;
    using Api.Swagger;
    using AutoMapper;
    using Wallet.Api.Models.VM;
    using Wallet.Api.Mapper.MapperProfiles;
    using Wallet.Api.Models;

    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                // .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)

                //Loads custom options for api
                .AddJsonFile("apisettings.json")
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Required to use the Options<T> pattern
            services.AddOptions();

            // Add settings from configuration
            services.Configure<ApiSettings>(Configuration);

            //Add EF
            services.AddEF();

            // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
            // note: the specified format code will format the version as "'v'major[.minor][-status]"
            services.AddMvcCore().AddVersionedApiExplorer(o => o.GroupNameFormat = "'v'VVV");

            // Add framework services.
            services.AddMvc()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });

            // Add API versioning
            // Ref: https://github.com/Microsoft/aspnet-api-versioning
            services.AddApiVersioning(
                o =>
                {
                    o.AssumeDefaultVersionWhenUnspecified = true;
                    o.DefaultApiVersion = new ApiVersion(1, 0);
                });

            // Add Swagger for documentation
            services.AddSwagger();

            //Add AutoMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MainProfile());
            });
            services.AddSingleton<IMapper>(sp => config.CreateMapper());

            // Add Repositories
            services.AddRepositories();

            //Add user managment
            services.AddUser();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApiVersionDescriptionProvider provider)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            // Configure the HTTP request pipeline.
            app.UseStaticFiles();

            app.UseMvc();

            // Ref: https://github.com/Microsoft/aspnet-api-versioning
            // https://github.com/Microsoft/aspnet-api-versioning/tree/master/samples/aspnetcore/SwaggerSample
            app.UseApiVersioning();

            app.UseSwagger();

            // Enable middleware to serve swagger-ui assets (HTML, JS, CSS etc.)
            app.UseSwaggerUI(options =>
            {
                // build a swagger endpoint for each discovered API version
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                }
            });
        }
    }
}
