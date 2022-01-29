using FluentMigrator.Runner;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;
using TesteBackendEnContact.Core.Interface.Services;
using TesteBackendEnContact.Core.Services;
using TesteBackendEnContact.Database;
using TesteBackendEnContact.Helpers;
using TesteBackendEnContact.Repository;
using TesteBackendEnContact.Repository.Interface;

namespace TesteBackendEnContact
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
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.EnableAnnotations();

                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TesteBackendEnContact", Version = "v1" });

                var basePath = AppDomain.CurrentDomain.BaseDirectory;
                var fileName = typeof(Startup).GetTypeInfo().Assembly.GetName().Name + ".xml";

                c.IncludeXmlComments(Path.Combine(basePath, fileName));
            });

            services.AddAutoMapper(typeof(AutoMapperConfig));

            services.AddFluentMigratorCore()
                    .ConfigureRunner(rb => rb
                        .AddSQLite()
                        .WithGlobalConnectionString(Configuration.GetConnectionString("DefaultConnection"))
                        .ScanIn(typeof(Startup).Assembly).For.Migrations())
                    .AddLogging(lb => lb.AddFluentMigratorConsole());

            services.AddSingleton(new DatabaseConfig { ConnectionString = Configuration.GetConnectionString("DefaultConnection") });

            //Add DI Services
            services.AddTransient<IContactBookService, ContactBookService>();
            services.AddTransient<ICompanyService, CompanyService>();
            services.AddTransient<IContactService, ContactService>();
            services.AddTransient<ICsvService, CsvService>();

            //Add DI Repositories
            services.AddTransient<IContactBookRepository, ContactBookRepository>();
            services.AddTransient<ICompanyRepository, CompanyRepository>();
            services.AddTransient<IContactRepository, ContactRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TesteBackendEnContact v1"));
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            var runner = serviceProvider.GetService<IMigrationRunner>();
            runner.MigrateUp();

        }
    }
}
