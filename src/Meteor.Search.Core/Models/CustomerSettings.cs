namespace Meteor.Search.Core.Models;

public class CustomerSettings
{
    public string? FullTextSearchUrl { get; set; }

    public string? FullTextSearchApiKey { get; set; }

    public bool Encrypted { get; set; }
}