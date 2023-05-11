using MassTransit;
using Meteor.Search.Api.Dtos;
using Meteor.Search.Core.Services.Contracts;

namespace Meteor.Search.Api.Consumers;

public class RemoveEmployeeConsumer : IConsumer<RemoveEmployeeMessage>
{
    private readonly IEmployeesService _employeesService;

    public RemoveEmployeeConsumer(IEmployeesService employeesService)
    {
        _employeesService = employeesService;
    }

    public Task Consume(ConsumeContext<RemoveEmployeeMessage> context)
    {
        return _employeesService.RemoveEmployeeAsync(context.Message.EmployeeId);
    }
}