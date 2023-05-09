using Meteor.Search.Core.Models;

namespace Meteor.Search.Core.Contracts;

public interface ICustomerDataAccessor
{
    public Customer? Customer { get; }

    public CustomerSettings? CustomerSettings { get; }
}