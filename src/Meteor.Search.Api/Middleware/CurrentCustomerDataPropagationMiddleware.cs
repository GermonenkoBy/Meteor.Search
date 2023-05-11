using Meteor.Search.Api.Services.Http;
using Meteor.Search.Core.Contracts;
using Meteor.Search.Infrastructure;

namespace Meteor.Search.Api.Middleware;

public class CurrentCustomerDataPropagationMiddleware : IMiddleware
{
    private readonly ICustomersClient _customersClient;

    private readonly IEnumerable<ICustomerIdProvider> _customerIdProviders;

    private readonly ICustomerDataAccessor _customerDataAccessor;

    public CurrentCustomerDataPropagationMiddleware(
        ICustomersClient customersClient,
        IEnumerable<ICustomerIdProvider> customerIdProviders,
        ICustomerDataAccessor customerDataAccessor
    )
    {
        _customersClient = customersClient;
        _customerIdProviders = customerIdProviders;
        _customerDataAccessor = customerDataAccessor;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        await TrySetCustomerData(context);
        await next.Invoke(context);
    }

    private async Task TrySetCustomerData(HttpContext context)
    {
        int customerId = default;
        foreach (var customerIdProvider in _customerIdProviders)
        {
            if (customerIdProvider.TryGetCustomerId(context, out customerId))
            {
                break;
            }
        }

        if (customerId == default)
        {
            return;
        }

        var customer = await _customersClient.GetCustomerAsync(customerId);
        if (customer is not null)
        {
            _customerDataAccessor.Customer = customer;
        }

        var customerSettings = await _customersClient.GetCustomerSettingsAsync(customerId);
        if (customerSettings is not null)
        {
            _customerDataAccessor.CustomerSettings = customerSettings;
        }
    }
}