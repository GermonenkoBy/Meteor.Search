using Grpc.Core;
using MapsterMapper;
using Meteor.Search.Core.Contracts;
using Meteor.Search.Infrastructure.Grpc;

namespace Meteor.Search.Infrastructure.Contracts;

public class GrpcCustomersClient : ICustomersClient
{
    private readonly CustomersService.CustomersServiceClient _grpcClient;

    private readonly IMapper _mapper;

    public GrpcCustomersClient(CustomersService.CustomersServiceClient grpcClient, IMapper mapper)
    {
        _grpcClient = grpcClient;
        _mapper = mapper;
    }

    public async Task<Core.Models.Customer?> GetCustomerAsync(int customerId)
    {
        try
        {
            var customer = await _grpcClient.GetCustomerByIdAsync(new() { CustomerId = customerId });
            return _mapper.Map<Core.Models.Customer>(customer);
        }
        catch (RpcException e) when (e.StatusCode == StatusCode.NotFound)
        {
            return null;
        }
    }

    public async Task<Core.Models.CustomerSettings?> GetCustomerSettingsAsync(int customerId)
    {
        try
        {
            var response = await _grpcClient.GetCustomerSettingsAsync(new()
            {
                CustomerId = customerId,
            });

            return _mapper.Map<Core.Models.CustomerSettings>(response);
        }
        catch (RpcException e) when(e.Status.StatusCode == StatusCode.NotFound)
        {
            return null;
        }
    }
}