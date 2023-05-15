﻿namespace Meteor.Search.Api.Services.Http;

public class HeadersCustomerIdProvider : ICustomerIdProvider
{
    private const string CustomerIdHeaderName = "Meteor-Customer-Id";

    public bool TryGetCustomerId(HttpContext httpContext, out int customerId)
    {
        if (!httpContext.Request.Headers.TryGetValue(CustomerIdHeaderName, out var customerIdHeader))
        {
            customerId = 0;
            return false;
        }

        return int.TryParse(customerIdHeader, out customerId);
    }
}