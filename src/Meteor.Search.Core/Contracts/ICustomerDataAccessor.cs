using Meteor.Search.Core.Models;

namespace Meteor.Search.Core.Contracts;

public interface ICustomerDataAccessor
{
    public Customer? Customer { get; set; }

    public CustomerSettings? CustomerSettings { get; set; }
}