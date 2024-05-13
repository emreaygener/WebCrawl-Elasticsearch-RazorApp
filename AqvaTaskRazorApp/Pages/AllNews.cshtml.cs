using AqvaTaskRazorApp;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using Elastic.Transport;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyApp.Namespace
{
    public class AllNewsModel : PageModel
    {
        public List<News> Documents { get; set; }
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalPages { get; set; }
        private readonly ElasticsearchClient _es;
        private readonly ILogger<SearchModel> _logger;

        public AllNewsModel(ElasticsearchClient es, ILogger<SearchModel> logger)
        {
            _es = es;
            _logger = logger;
        }

        public async Task OnGet(int currentPage = 1, int pageSize = 6)
        {
            CurrentPage = currentPage;
            PageSize = pageSize;
            
            var countResponse = await _es.CountAsync<News>(c => c
                .Indices("news")
                .Query(q => q
                    .MatchAll(new MatchAllQuery()))
            );

            int totalItems = (int)countResponse.Count;
            int totalPages = (int)Math.Ceiling((double)totalItems / PageSize);
        
            TotalPages = totalPages;

            var response = await _es.SearchAsync<News>(s => s
                .Index("news")
                .Query(q => q
                    .MatchAll(new MatchAllQuery()))
                .From(((CurrentPage<1?1:CurrentPage>TotalPages?TotalPages:CurrentPage) - 1) * PageSize)
                .Size(PageSize)
            );
        
            Documents = response.Hits.Select(h => new News
            { 
                Title = h.Source!=null?h.Source.Title:"",
                Img = h.Source!=null?h.Source.Img:"",
                Link = h.Source!=null?(h.Source.Link.StartsWith("http")?"":GlobalVariables.WEBSITE_URL)+h.Source.Link:""
            }).ToList();
        }
    }
}
