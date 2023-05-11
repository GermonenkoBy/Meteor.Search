using MassTransit;
using Meteor.Common.Core.Exceptions;
using Meteor.Search.Api.Dtos;
using Meteor.Search.Core.Contracts;

namespace Meteor.Search.Api.Filters;

public class CustomerDataPropagationFilter<TMessage>
    : IFilter<ConsumeContext<TMessage>>
    where TMessage : CustomerBasedMessage
{
    private readonly ICustomerDataAccessor _customerDataAccessor;

    private readonly ICustomersClient _customersClient;

    public CustomerDataPropagationFilter(
        ICustomerDataAccessor customerDataAccessor,
        ICustomersClient customersClient
    )
    {
        _customerDataAccessor = customerDataAccessor;
        _customersClient = customersClient;
    }

    public async Task Send(ConsumeContext<TMessage> context, IPipe<ConsumeContext<TMessage>> next)
    {
        var customerId = context.Message.CustomerId;
        var customer = await _customersClient.GetCustomerAsync(customerId);
        if (customer is null)
        {
            throw new MeteorException($"Failed to process message: customer {customerId} was not found.");
        }

        var settings = await _customersClient.GetCustomerSettingsAsync(customerId);
        if (settings is null)
        {
            throw new MeteorException(
                $"Failed to process message: {customer.Name} ({customer.Domain}) settings are misconfigured."
            );
        }

        _customerDataAccessor.Customer = customer;
        _customerDataAccessor.CustomerSettings = settings;

        await next.Send(context);
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope("customerDataPropagation");
    }
}