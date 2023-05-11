using Meteor.Search.Api.Dtos;
using Meteor.Search.Core.Contracts;
using Meteor.Search.Core.Services.Contracts;
using Meteor.Search.Infrastructure;

namespace Meteor.Search.Api.Services.Processors;

public class EmployeeRemovedMessageProcessor
{
    private readonly IServiceProvider _serviceProvider;

    private readonly ILogger<EmployeeRemovedMessageProcessor> _logger;

    public EmployeeRemovedMessageProcessor(
        IServiceProvider serviceProvider,
        ILogger<EmployeeRemovedMessageProcessor> logger
    )
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task ProcessAsync(RemoveEmployeeMessage message)
    {
        using var scope = _serviceProvider.CreateScope();

        var customersClient = scope.ServiceProvider.GetRequiredService<ICustomersClient>();

        var customer = await customersClient.GetCustomerAsync(message.CustomerId);
        if (customer is null)
        {
            _logger.LogError(
                "Unable to remove employee {EmployeeId}: customer {CustomerId} was not found",
                message.EmployeeId,
                message.CustomerId
            );
            return;
        }

        var customerSettings = await customersClient.GetCustomerSettingsAsync(customer.Id);
        if (customerSettings?.FullTextSearchApiKey is null)
        {
            _logger.LogError(
                "Unable to remove employee {EmployeeId}: customer settings are not setup for customer {CustomerId}",
                message.EmployeeId,
                message.CustomerId
            );
            return;
        }

        var customerDataAccessor = scope.ServiceProvider.GetRequiredService<ICustomerDataAccessor>();
        customerDataAccessor.Customer = customer;
        customerDataAccessor.CustomerSettings = customerSettings;

        var customersService = scope.ServiceProvider.GetRequiredService<IEmployeesService>();

        await customersService.RemoveEmployeeAsync(message.EmployeeId);
    }
}