using MapsterMapper;
using Meteor.Search.Api.Dtos;
using Meteor.Search.Core.Contracts;
using Meteor.Search.Core.Services.Contracts;
using Meteor.Search.Infrastructure;

namespace Meteor.Search.Api.Services.Processors;

public class EmployeeChangeMessageProcessor
{
    private readonly IServiceProvider _serviceProvider;

    private readonly IMapper _mapper;

    private readonly ILogger<EmployeeChangeMessageProcessor> _logger;

    public EmployeeChangeMessageProcessor(
        IServiceProvider serviceProvider,
        IMapper mapper,
        ILogger<EmployeeChangeMessageProcessor> logger
    )
    {
        _serviceProvider = serviceProvider;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task ProcessAsync(UpdateEmployeeMessage message)
    {
        using var scope = _serviceProvider.CreateScope();

        var customersClient = scope.ServiceProvider.GetRequiredService<ICustomersClient>();

        var customer = await customersClient.GetCustomerAsync(message.CustomerId);
        if (customer is null)
        {
            _logger.LogError(
                "Unable to process employee {EmployeeId}: customer {CustomerId} was not found", 
                message.EmployeeId,
                message.CustomerId
            );
            return;
        }

        var customerSettings = await customersClient.GetCustomerSettingsAsync(customer.Id);
        if (customerSettings?.FullTextSearchApiKey is null)
        {
            _logger.LogError(
                "Unable to process employee {EmployeeId}: customer settings are not setup for customer {CustomerId}", 
                message.EmployeeId,
                message.CustomerId
            );
            return;
        }

        var customerDataAccessor = scope.ServiceProvider.GetRequiredService<SimpleCustomerDataAccessor>();
        customerDataAccessor.Customer = customer;
        customerDataAccessor.CustomerSettings = customerSettings;

        var customersService = scope.ServiceProvider.GetRequiredService<IEmployeesStore>();

        var employee = _mapper.Map<Core.Models.Employee>(message);
        await customersService.SaveEmployeeAsync(employee);
    }
}