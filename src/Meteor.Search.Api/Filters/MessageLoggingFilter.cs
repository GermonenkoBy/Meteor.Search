using MassTransit;

namespace Meteor.Search.Api.Filters;

public class MessageLoggingFilter : IFilter<ConsumeContext>
{
    private readonly ILogger<MessageLoggingFilter> _logger;

    public MessageLoggingFilter(ILogger<MessageLoggingFilter> logger)
    {
        _logger = logger;
    }

    public async Task Send(ConsumeContext context, IPipe<ConsumeContext> next)
    {
        var message = context.ReceiveContext.Body.GetString();

        try
        {
            _logger.LogInformation("Received message: {Message}", message);
            await next.Send(context);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to process message {Message}", message);
        }
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope("logging");
    }
}