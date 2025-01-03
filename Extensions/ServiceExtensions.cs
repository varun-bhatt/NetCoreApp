using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using AutoMapper;
using Peddle.MessageBroker.Serializer;
using Microsoft.EntityFrameworkCore;
using NetCoreApp;
using NetCoreApp.Application.Interfaces.Repositories;
using NetCoreApp.Domain.ErrorResponseProvider;
using NetCoreApp.Infrastructure.Persistence;
using NetCoreApp.Infrastructure.Repositories;
using Peddle.Foundation.MediatR.Behaviours;

namespace NetCoreApplication.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddWebApiServicesExtension(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddSingleton(typeof(ISerializer<>), typeof(XmlSerializer<>));
            
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

            services.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new ApiVersion(1, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.ReportApiVersions = true;
            });
            services.RegisterRepositories();
            services.RegisterInfraStructure(configuration);
            services.AddSwaggerGen();
            services.AddTransient<IErrorResponsesProvider, ErrorResponsesProvider>();

            //Register healthchecks
            services.RegisterHealthChecks(configuration);
            services.AddAutoMapperService();
        }

        private static void RegisterRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IExpenseRepository), typeof(ExpenseRepository));
        }

        private static void RegisterInfraStructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
        }

        public static void RegisterHealthChecks(this IServiceCollection services,
            IConfiguration configuration)
        {
            const string readiness = "Readiness";

            string dbConnectionString = configuration.GetConnectionString("appconfig:DefaultConnection");
           
            services.AddHealthChecks()
                .AddSqlServer(dbConnectionString, tags: new[] { readiness });
        }
        
        public static void AddAutoMapperService(this IServiceCollection services)
        {
            var mapperConfig = new MapperConfiguration(mc => { mc.AddMaps(Assembly.GetExecutingAssembly()); });
            var mapper = mapperConfig.CreateMapper();

            services.AddSingleton(mapper);
        }
    }
}