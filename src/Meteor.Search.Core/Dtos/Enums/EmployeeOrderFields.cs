using System.ComponentModel;

namespace Meteor.Search.Core.Dtos.Enums;

public enum EmployeeOrderFields
{
    [Description("emailAddress")]
    EmailAddress,

    [Description("phoneNumber")]
    PhoneNumber,

    [Description("firstName")]
    FirstName,

    [Description("lastName")]
    LastName
}