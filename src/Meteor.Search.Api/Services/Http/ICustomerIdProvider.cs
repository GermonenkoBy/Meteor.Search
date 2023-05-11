namespace Meteor.Search.Api.Services.Http;

public interface ICustomerIdProvider
{
    bool TryGetCustomerId(HttpContext httpContext, out int customerId);
}