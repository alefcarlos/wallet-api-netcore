using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.AspNetCore.Swagger;

namespace Wallet.Api.Swagger
{
    public static class SwaggerServiceExtensions
    {
        /// <summary>
        /// Add Swagger for documentation
        /// </summary>
        /// <param name="services"></param>
        public static void AddSwagger(this IServiceCollection services)
        {

            // Integrate with api versioning
            // Ref: https://github.com/Microsoft/aspnet-api-versioning/wiki/API-Documentation#aspnet-core
            services.AddSwaggerGen(options =>
            {

                // resolve the IApiVersionDescriptionProvider service
                // note: that we have to build a temporary service provider here because one has not been created yet
                var provider = services.BuildServiceProvider()
                       .GetRequiredService<IApiVersionDescriptionProvider>();

                // add a swagger document for each discovered API version
                // note: you might choose to skip or document deprecated API versions differently
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
                }

                // add a custom operation filter which sets default values
                options.OperationFilter<SwaggerDefaultValues>();

                // integrate xml comments
                options.IncludeXmlComments(GetXmlCommentsPath(PlatformServices.Default.Application));
            });
        }

        private static string GetXmlCommentsPath(ApplicationEnvironment appEnvironment)
        {
            return System.IO.Path.Combine(appEnvironment.ApplicationBasePath, "Wallet.Api.xml");
        }

        private static Info CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var info = new Info()
            {
                Title = $"Wallet API {description.ApiVersion}",
                Version = description.ApiVersion.ToString(),
                Description = "Handle wallets with this API.",
                Contact = new Contact() { Name = "Alef Carlos", Email = "alef.carlos@gmail.com" },
            };

            if (description.IsDeprecated)
            {
                info.Description += " This API version has been deprecated.";
            }

            return info;
        }
    }
}