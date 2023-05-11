using MapsterMapper;
using MassTransit;
using Meteor.Search.Api.Dtos;
using Meteor.Search.Core.Services.Contracts;

namespace Meteor.Search.Api.Consumers;

public class UpdateEmployeeConsumer : IConsumer<UpdateEmployeeMessage>
{
    private readonly IEmployeesService _employeesService;

    private readonly IMapper _mapper;

    public UpdateEmployeeConsumer(IEmployeesService employeesService, IMapper mapper)
    {
        _employeesService = employeesService;
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<UpdateEmployeeMessage> context)
    {
        var employee = _mapper.Map<Core.Models.Employee>(context.Message);
        await _employeesService.SaveEmployeeAsync(employee);
    }
}