using Meteor.Search.Core.Models;

namespace Meteor.Search.Core.Contracts;

public interface ICustomersClient
{
    Task<Customer?> GetCustomerAsync(int customerId);

    Task<CustomerSettings?> GetCustomerSettingsAsync(int customerId);
}