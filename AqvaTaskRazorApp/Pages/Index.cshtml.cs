using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using Elastic.Transport;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AqvaTaskRazorApp.Pages;

public class IndexModel : PageModel
{
    public News News { get; set; }
    public List<News> Documents { get; set; }
    private readonly ElasticsearchClient _es;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger, ElasticsearchClient es)
    {
        _logger = logger;
        _es = es;
    }

    public async Task OnGet()
    {
        var countResponse = await _es.CountAsync<News>(c => c
                .Indices("news")
                .Query(q => q
                    .MatchAll(new MatchAllQuery()))
            );

            int totalItems = (int)countResponse.Count;
            int totalPages = (int)Math.Ceiling((double)totalItems / 6);

        SearchResponse<News>? allDocuments = await _es.SearchAsync<News>(s => s
            .Index("news")
            .Query(q => q
                .MatchAll(new MatchAllQuery()))
                .From(new Random().Next(0, totalPages))
            .Size(6)
        );

        Documents = allDocuments.Hits.Select(h => 
        new News
        { 
            Title = h.Source!=null?h.Source.Title:"", 
            Img = h.Source!=null?h.Source.Img:"", 
            Link = h.Source!=null?GlobalVariables.WEBSITE_URL+h.Source.Link:""
        }).ToList();

    }
}