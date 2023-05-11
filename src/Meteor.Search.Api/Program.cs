using Meteor.Search.Api.Services.Grpc;
using Meteor.Search.Api.Services.Processors;
using Meteor.Search.Core.Contracts;
using Meteor.Search.Infrastructure;
using Microsoft.Extensions.Azure;
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

var serviceBusConnectionString = builder.Configuration.GetConnectionString("AzureServiceBus") ?? string.Empty;
builder.Services.AddAzureClients(azureBuilder =>
{
    azureBuilder.AddServiceBusClient(serviceBusConnectionString);
});

builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();

builder.Services.AddScoped<ICustomerDataAccessor, SimpleCustomerDataAccessor>();

builder.Services.AddTransient<EmployeeChangeMessageProcessor>();
builder.Services.AddTransient<EmployeeRemovedMessageProcessor>();

var app = builder.Build();

app.MapGrpcReflectionService();
app.MapGrpcService<EmployeesSearchGrpcService>();
app.Run();