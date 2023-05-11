using Meteor.Search.Core.Contracts;
using Meteor.Search.Core.Models;

namespace Meteor.Search.Infrastructure;

public class SimpleCustomerDataAccessor : ICustomerDataAccessor
{
    public Customer? Customer { get; set; }

    public CustomerSettings? CustomerSettings { get; set; }
}