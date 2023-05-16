using Grpc.Core;
using Mapster;
using MapsterMapper;
using Meilisearch;
using Meteor.Search.Api.Extensions;
using Meteor.Search.Api.Middleware;
using Meteor.Search.Api.Services.Grpc;
using Meteor.Search.Api.Services.Http;
using Meteor.Search.Api.Services.Processors;
using Meteor.Search.Core.Contracts;
using Meteor.Search.Core.Exceptions;
using Meteor.Search.Core.Models;
using Meteor.Search.Core.Services;
using Meteor.Search.Core.Services.Contracts;
using Meteor.Search.Core.Services.Validators;
using Meteor.Search.Infrastructure;
using Meteor.Search.Infrastructure.Contracts;
using Meteor.Search.Infrastructure.Grpc;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;

var builder = WebApplication.CreateBuilder(args);

var appConfigurationConnectionString = builder.Configuration.GetConnectionString("AzureAppConfiguration");
if (!string.IsNullOrEmpty(appConfigurationConnectionString))
{
    builder.Configuration.AddAzureAppConfiguration(options =>
    {
        options
            .Connect(appConfigurationConnectionString)
            .UseFeatureFlags()
            .Select(KeyFilter.Any)
            .Select(KeyFilter.Any, builder.Environment.EnvironmentName)
            .Select(KeyFilter.Any, $"{builder.Environment.EnvironmentName}-Search");
    });
}

builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();

builder.Services.AddGrpcClient<CustomersService.CustomersServiceClient>(options =>
{
    var url = builder.Configuration.GetValue<string>("Routing:ControllerUrl") ?? string.Empty;
    options.Address = new Uri(url);
    if (options.Address.Scheme.Equals("http", StringComparison.OrdinalIgnoreCase))
    {
        options.ChannelOptionsActions.Add(opt => opt.Credentials = ChannelCredentials.Insecure);
    }
});

builder.Services.AddScoped<MeilisearchClient>(services =>
{
    var currentCustomerData = services.GetRequiredService<ICustomerDataAccessor>();
    var settings = currentCustomerData.CustomerSettings;

    var missingConnectionString = settings?.FullTextSearchUrl is null
                                  || settings.FullTextSearchApiKey is null;
    if (missingConnectionString)
    {
        throw new MissingCustomerDataException("Missing full text search credentials.");
    }

    return new(settings!.FullTextSearchUrl, settings.FullTextSearchApiKey);
});

builder.Services.AddScoped<CurrentCustomerDataPropagationMiddleware>();
builder.Services.AddSingleton<ICustomerIdProvider, HeadersCustomerIdProvider>();
builder.Services.AddScoped<IEmployeesSearchService, EmployeesSearchService>();
builder.Services.AddScoped<IEmployeesService, EmployeesService>();
builder.Services.AddScoped<IValidator<Employee>, EmployeePropertiesValidator>();
builder.Services.AddScoped<ICustomerDataAccessor, SimpleCustomerDataAccessor>();
builder.Services.AddScoped<IEmployeesRepository, EmployeesRepository>();
builder.Services.AddScoped<ICustomersClient, GrpcCustomersClient>();

builder.Services.AddTransient<EmployeeChangeMessageProcessor>();
builder.Services.AddTransient<EmployeeRemovedMessageProcessor>();

var mapperConfiguration = new TypeAdapterConfig();
mapperConfiguration.Apply(new Meteor.Search.Infrastructure.Mapping.MappingRegister());
builder.Services.AddSingleton<IMapper>(new Mapper(mapperConfiguration));

var app = builder.Build();

app.UseMiddleware<CurrentCustomerDataPropagationMiddleware>();
app.MapGrpcReflectionService();
app.MapGrpcService<EmployeesSearchGrpcService>();
app.Run();