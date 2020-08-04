
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;



using GraphQL.Server;
using GraphQL.Types;
//using GraphQL.Server.Ui.Playground;

using Example;
using Api;
using Api.Types;

namespace Host
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
            //services.AddControllers();
            //services.AddSingleton<ISchema, StarWarsSchema>();

            services.AddSingleton<VBQuery>();
            services.AddSingleton<ISchema, VBSchema>();



            services.AddGraphQL((options, provider) =>
            {
                options.EnableMetrics = true;
                options.ExposeExceptions = true;
                // var logger = provider.GetRequiredService<ILogger<Startup>>();
                // options.UnhandledExceptionDelegate = ctx => logger.LogError("{Error} occured", ctx.OriginalException.Message);
            })
             .AddSystemTextJson(deserializerSettings => { }, serializerSettings => { }) // For .NET Core 3+
             //.AddNewtonsoftJson(deserializerSettings => { }, serializerSettings => { }) // For everything else
             .AddWebSockets() // Add required services for web socket support
             .AddDataLoader() // Add required services for DataLoader support
             .AddGraphTypes(typeof(VBSchema))
             .AddUserContextBuilder(httpContext => new GraphQLUserContext { User = httpContext.User });

            services.AddLogging(builder => builder.AddConsole());
            services.AddHttpContextAccessor();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseWebSockets();
            // add http for Schema at default url /graphql
            app.UseGraphQL<ISchema>("/graphql"); //ISchema

            // use graphql-playground at default url /ui/playground
            //app.UseGraphQLPlayground();
            app.UseGraphiQLServer();

            // app.UseHttpsRedirection();

            // app.UseRouting();

            // app.UseAuthorization();

            // app.UseEndpoints(endpoints =>
            // {
            //     endpoints.MapControllers();
            // });
        }
    }
}
