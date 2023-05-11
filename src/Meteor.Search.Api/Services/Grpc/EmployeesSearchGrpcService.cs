using Grpc.Core;
using MapsterMapper;
using Meteor.Search.Api.Grpc;
using Meteor.Search.Core.Dtos;
using Meteor.Search.Core.Services.Contracts;

namespace Meteor.Search.Api.Services.Grpc;

public class EmployeesSearchGrpcService : EmployeesSearchService.EmployeesSearchServiceBase
{
    private readonly IEmployeesSearchService _employeesSearchService;

    private readonly IMapper _mapper;

    public EmployeesSearchGrpcService(IEmployeesSearchService employeesSearchService, IMapper mapper)
    {
        _employeesSearchService = employeesSearchService;
        _mapper = mapper;
    }

    public override async Task<EmployeesPage> SearchEmployees(
        SearchEmployeesRequest request,
        ServerCallContext context
    )
    {
        var filter = _mapper.Map<EmployeesFilter>(request);
        var searchResult = await _employeesSearchService.SearchAsync(filter);

        var response = new EmployeesPage
        {
            Total = searchResult.Total
        };

        var employeeModels = _mapper.Map<List<Employee>>(searchResult.Items);
        response.Employees.AddRange(employeeModels);

        return response;
    }
}