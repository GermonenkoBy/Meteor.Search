using Meteor.Common.Core.Exceptions;
using Meteor.Search.Core.Contracts;
using Meteor.Search.Core.Services.Contracts;

namespace Meteor.Search.Core.Services;

public class CustomerDataSetter : ICustomerDataSetter
{
    private readonly ICustomersClient _customersClient;

    private readonly ICustomerDataAccessor _customerDataAccessor;

    public CustomerDataSetter(ICustomersClient customersClient, ICustomerDataAccessor customerDataAccessor)
    {
        _customersClient = customersClient;
        _customerDataAccessor = customerDataAccessor;
    }

    public async Task SetCustomerData(int customerId)
    {
        var customer = await _customersClient.GetCustomerAsync(customerId);
        if (customer is null)
        {
            throw new MeteorException($"Unable to set customer data: customer {customerId} was not found.");
        }

        var settings = await _customersClient.GetCustomerSettingsAsync(customerId);
        if (settings is null)
        {
            throw new MeteorException($"Settings are not setup for customer {customer.Name} ({customer.Domain})");
        }
    }
}