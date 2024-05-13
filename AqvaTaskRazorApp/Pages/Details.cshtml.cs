using AqvaTaskRazorApp;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyApp.Namespace
{
    public class DetailsModel : PageModel
    {
        public string Link { get; set; }
        public NewsDetail News { get; set; }
        private readonly ElasticsearchClient _es;
        private readonly ILogger<SearchModel> _logger;

        public DetailsModel(ElasticsearchClient es, ILogger<SearchModel> logger)
        {
            _es = es;
            _logger = logger;
        }

        public async Task OnGet()
        {
            Link = Request.Query[key: "link"];
            if (string.IsNullOrEmpty(Link))
            {
                return;
            }
            await GlobalVariables.RunScraperAsync(Link);

            await _es.Indices.RefreshAsync("news");
            await _es.Indices.RefreshAsync("detail");

            if (Link.StartsWith(GlobalVariables.WEBSITE_URL))
            {
                Link = Link.Replace(GlobalVariables.WEBSITE_URL, "");
            }

            var news = await _es.SearchAsync<News>(s => s
                    .Index("news")
                    .Query(q => q
                        .MatchPhrasePrefix(m => m
                            .Field(f => f.Link)
                            .Query(Link)
                        )
                    )
                    .Size(1000)
                );

            var details = await _es.SearchAsync<Detail>(s => s
            .Index("detail")
                .Query(q => q
                    .MatchAll(new MatchAllQuery())
                )
            );

            Console.WriteLine(Link);
            Console.WriteLine("-------------------------------------------------------------------------------------------------------");
            Console.WriteLine(news);
            Console.WriteLine("(-------------------------------------------------------------------------------------------------------)");
            Console.WriteLine(details);


            News = new NewsDetail
            {
                Title = news.Documents.First().Title,
                Link = news.Documents.First().Link,
                Img = news.Documents.First().Img,
                Description = details.Documents.First().Description,
                Article = details.Documents.First().Article
            };

        }
    }
}
