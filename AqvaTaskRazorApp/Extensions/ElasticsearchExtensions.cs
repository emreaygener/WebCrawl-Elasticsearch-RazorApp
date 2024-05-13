using Elastic.Clients.Elasticsearch;
using Elastic.Clients;
using Elastic.Transport;

namespace AqvaTaskRazorApp;

public static class ElasticsearchExtensions
{
    public static void AddElasticsearch(this IServiceCollection services, IConfiguration configuration )
    {
        var settings = new ElasticsearchClientSettings(new Uri(configuration["ElasticsearchConfig:Url"]))
                .PrettyJson()
                .DefaultIndex(configuration["ElasticsearchConfig:DefaultIndex"])
                .DisableDirectStreaming()
                .CertificateFingerprint(configuration["ElasticsearchConfig:CertFingerprint"])
                .Authentication(new BasicAuthentication(configuration["ElasticsearchConfig:Username"], configuration["ElasticsearchConfig:Password"]));

        var client = new ElasticsearchClient(settings);
        services.AddSingleton(client);
    }
    
}
