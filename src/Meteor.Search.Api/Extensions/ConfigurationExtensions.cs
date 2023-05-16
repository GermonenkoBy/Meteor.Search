namespace Meteor.Search.Api.Extensions;

public static class ConfigurationExtensions
{
    public static T GetRequiredValue<T>(this IConfiguration configuration, string key)
        => configuration.GetValue<T>(key) ?? throw new Exception($"Configuration item '{key}' is unset.");
}