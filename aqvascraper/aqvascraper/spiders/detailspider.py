import scrapy
import uuid

class DetailSpider(scrapy.Spider):
    name = 'detail'

    def __init__(self, start_url='', *args, **kwargs):
        super(DetailSpider, self).__init__(*args, **kwargs)
        self.start_urls = [start_url]

    def parse(self, response):
        for news in response.css('div.news-body div.col-lg-8'):
            try:
                yield {
                    'description': news.css('p.description::text').get().replace('\n','') if news.css('a.news-card-footer::text').get().replace('\n','') != "" else "Açıklama metnine erişilemedi",
                    'article': news.css('div.article-body *::text').getall().replace('\n',''),
                }
            except:
                data = {
                    'description': news.css('p.description::text').extract_first(),
                    'article': news.css('div.article-body *::text').extract(),                    
                }
                if data["description"] is not None and data["article"] is not None:
                    yield data
                else:
                   pass

            


