namespace Meteor.Search.Core.Services.Contracts;

public interface ICustomerDataSetter
{
    Task SetCustomerData(int customerId);
}