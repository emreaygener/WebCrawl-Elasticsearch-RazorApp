using System.Diagnostics.CodeAnalysis;
using AqvaTaskRazorApp;
using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyApp.Namespace
{
    public class SearchModel : PageModel
    {
        public string SearchQuery { get; set; }
        public List<News> SearchResults { get; set; }
        private readonly ElasticsearchClient _es;
        private readonly ILogger<SearchModel> _logger;

        public SearchModel(ElasticsearchClient es, ILogger<SearchModel> logger)
        {
            _es = es;
            _logger = logger;
        }

        public async Task OnGet()
        {
            SearchQuery = Request.Query["query"];
            
            if (!string.IsNullOrEmpty(SearchQuery))
            {
                var response = await _es.SearchAsync<News>(s => s
                    .Index("news")
                    .Query(q => q
                        .MatchPhrasePrefix(m => m
                            .Field(f => f.Title)
                            .Query(SearchQuery)
                        )
                    )
                    .Size(1000)
                );

                // Console.WriteLine(response.Hits.Select(h => h.Source?.Title).FirstOrDefault());
                SearchResults = response.Hits.Select(h => new News
                { 
                    Title = h.Source!=null?h.Source.Title:"", 
                    Img = h.Source!=null?h.Source.Img:"", 
                    Link = h.Source!=null?GlobalVariables.WEBSITE_URL+h.Source.Link:""
                }).ToList();
            }else {
                SearchResults = new List<News>();
            }
        } 
    }
}
