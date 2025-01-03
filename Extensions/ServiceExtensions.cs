using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using AutoMapper;
using Peddle.MessageBroker.Serializer;
using Microsoft.EntityFrameworkCore;
using NetCoreApp;
using NetCoreApp.Application.Interfaces.Repositories;
using NetCoreApp.Application.UseCases.Expense.SearchExpense;
using NetCoreApp.Application.UseCases.ExpenseCategory.CreateExpense;
using NetCoreApp.Application.UseCases.ExpenseCategory.DeleteExpenseCategory;
using NetCoreApp.Application.UseCases.ExpenseCategory.GetAllExpenseCategories;
using NetCoreApp.Application.UseCases.ExpenseCategory.GetExpenseCategory;
using NetCoreApp.Application.UseCases.ExpenseCategory.UpdateExpenseCategory;
using NetCoreApp.Domain.Entities;
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

        public static void AddValidators(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddTransient<IRequestHandler<CreateExpenseCategoryCommand, int>, CreateExpenseCategoryHandler>();
            services.AddTransient<IRequestHandler<UpdateExpenseCategoryCommand>, UpdateExpenseCategoryHandler>();
            services.AddTransient<IRequestHandler<DeleteExpenseCategoryCommand>, DeleteExpenseCategoryHandler>();
            services.AddTransient<IRequestHandler<GetExpenseCategoryByIdQuery, ExpenseCategory>, GetExpenseCategoryByIdHandler>();
            services.AddTransient<IRequestHandler<GetAllExpenseCategoriesQuery, List<ExpenseCategory>>, GetAllExpenseCategoriesHandler>();
            services.AddTransient<IValidator<CreateExpenseCategoryCommand>, CreateExpenseCategoryCommandValidator>();
            services.AddTransient<IValidator<UpdateExpenseCategoryCommand>, UpdateExpenseCategoryCommandValidator>();
            services.AddTransient<IValidator<SearchExpenseQuery>, SearchExpenseValidator>();
        }
        
        private static void RegisterRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IExpenseRepository), typeof(ExpenseRepository));
            services.AddScoped(typeof(IUserRepository), typeof(UserRepository));
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