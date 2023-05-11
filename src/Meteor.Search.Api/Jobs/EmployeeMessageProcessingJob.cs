using Azure.Messaging.ServiceBus;
using Meteor.Search.Api.Constants;
using Meteor.Search.Api.Dtos;
using Meteor.Search.Api.Services.Processors;

namespace Meteor.Search.Api.Jobs;

public class EmployeeMessageProcessingJob : IHostedService, IAsyncDisposable
{
    private const string EmployeeChangeTopicName = "employees.changed";

    private const string EmployeeRemoveTopicName = "employees.deleted";

    private const string SubscriptionName = "search-service";

    private readonly IServiceProvider _serviceProvider;

    private readonly ILogger<EmployeeMessageProcessingJob> _logger;

    private readonly ServiceBusProcessor _removeEmployeeProcessor;

    private readonly ServiceBusProcessor _saveEmployeeProcessor;

    public EmployeeMessageProcessingJob(
        IServiceProvider serviceProvider,
        ILogger<EmployeeMessageProcessingJob> logger,
        ServiceBusClient serviceBusClient
    )
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _saveEmployeeProcessor = serviceBusClient.CreateProcessor(EmployeeChangeTopicName, SubscriptionName);
        _removeEmployeeProcessor = serviceBusClient.CreateProcessor(EmployeeRemoveTopicName, SubscriptionName);
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _saveEmployeeProcessor.ProcessMessageAsync += ProcessUpdateAsync;
        _saveEmployeeProcessor.ProcessErrorAsync += ProcessUpdateErrorAsync;

        _removeEmployeeProcessor.ProcessMessageAsync += ProcessRemoveAsync;
        _removeEmployeeProcessor.ProcessErrorAsync += ProcessRemoveErrorAsync;

        return Task.WhenAll(
            _saveEmployeeProcessor.StartProcessingAsync(cancellationToken),
            _removeEmployeeProcessor.StartProcessingAsync(cancellationToken)
        );
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.WhenAll(
            _saveEmployeeProcessor.StopProcessingAsync(cancellationToken),
            _removeEmployeeProcessor.StopProcessingAsync(cancellationToken)
        );
    }

    private Task ProcessUpdateAsync(ProcessMessageEventArgs arg)
    {
        var processor = _serviceProvider.GetRequiredService<EmployeeChangeMessageProcessor>();
        var message = arg.Message.Body.ToObjectFromJson<UpdateEmployeeMessage>(JsonDefaults.SerializerOptions);
        return processor.ProcessAsync(message);
    }

    private Task ProcessRemoveAsync(ProcessMessageEventArgs arg)
    {
        var processor = _serviceProvider.GetRequiredService<EmployeeRemovedMessageProcessor>();
        var message = arg.Message.Body.ToObjectFromJson<RemoveEmployeeMessage>(JsonDefaults.SerializerOptions);
        return processor.ProcessAsync(message);
    }

    private Task ProcessUpdateErrorAsync(ProcessErrorEventArgs arg)
    {
        _logger.LogError(arg.Exception, "Failed to handle employee search entry update");
        return Task.CompletedTask;
    }

    private Task ProcessRemoveErrorAsync(ProcessErrorEventArgs arg)
    {
        _logger.LogError(arg.Exception, "Failed to handle employee search entry removal");
        return Task.CompletedTask;
    }

    public async ValueTask DisposeAsync()
    {
        await _saveEmployeeProcessor.DisposeAsync();
        await _removeEmployeeProcessor.DisposeAsync();
    }
}