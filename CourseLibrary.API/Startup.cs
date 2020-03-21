using AutoMapper;
using CourseLibrary.API.DbContexts;
using CourseLibrary.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
using System;

namespace CourseLibrary.API
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
           services.AddControllers(setupAction =>
           {
               setupAction.ReturnHttpNotAcceptable = true; // if = false so The API will return responses in the default                                          supported format if an unsupported media type is requested. 
           })
                // Default formatter is simply the one that was added first. Now it is Json.
                // If I swap AddXml with AddNewtonsoftJson then default formatter will be XML.
                .AddNewtonsoftJson(setupAction =>
           {
               setupAction.SerializerSettings.ContractResolver =
                  new CamelCasePropertyNamesContractResolver();
           }).AddXmlDataContractSerializerFormatters()
             .ConfigureApiBehaviorOptions(setupAction =>
           {
               setupAction.InvalidModelStateResponseFactory = context =>
               {
                   var problemDetails = new ValidationProblemDetails(context.ModelState)
                   {
                       Type = "https://courselibrary.com/modelvalidationproblem",
                       Title = "One or more model validation errors occured.",
                       Status = StatusCodes.Status422UnprocessableEntity,
                       Detail = "See the errors property for details.",
                       Instance = context.HttpContext.Request.Path
                   };

                   problemDetails.Extensions.Add("traceId", context.HttpContext.TraceIdentifier);

                   return new UnprocessableEntityObjectResult(problemDetails)
                   {
                       ContentTypes = { "application/problem+json" }
                   };
               };
           }); // Adding a possibility for XML output format

                                   // we are loading all assemblies in the current domain
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddScoped<ICourseLibraryRepository, CourseLibraryRepository>();

            services.AddDbContext<CourseLibraryContext>(options =>
            {
                options.UseSqlServer(
                    @"Server=(localdb)\mssqllocaldb;Database=CourseLibraryDB;Trusted_Connection=True;");
            }); 
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(appBuilder =>
                {
                    // Run the code we write to the run statement
                    // That piece of code will be executed when an unhandled exception happens
                    appBuilder.Run(async context =>
                    {
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("An unexpected fault happened. Try again later.");
                    });
                });
            }

            app.UseRouting(); // marks the position in the middleware pipeline where and endpoint is selected 

            app.UseAuthorization();

            app.UseEndpoints(endpoints => // marks the position in the middleware pipeline where endpoint is executed
            {
                endpoints.MapControllers();
            });
        }
    }
}
