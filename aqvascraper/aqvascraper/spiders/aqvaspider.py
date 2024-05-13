import scrapy
import uuid

class AqvaSpider(scrapy.Spider):
    name = 'aqva'
    start_urls = ['https://www.sozcu.com.tr/']

    def parse(self, response):
        for news in response.css('div.news-card'):
            try:
                yield {
                    'title': news.css('a.news-card-footer::text').get().replace('\n','') if news.css('a.news-card-footer::text').get().replace('\n','') != "" else news.css('a.news-card-footer span.text-truncate-3::text').get().replace('\n',''),
                    'img': news.css('a.img-holder img::attr(src)').extract_first(),
                    'link':  news.css('a.news-card-footer::attr(href)').extract_first() if news.css('a.news-card-footer::attr(href)').extract_first() != "" else news.css('a.img-holder::attr(href)').extract_first(),    
                }
            except:
                data = {
                    'title': news.css('div.news-card div a::text').extract_first(),
                    'img': news.css('a.img-holder img::attr(src)').extract_first(),
                    'link':  news.css('a.img-holder::attr(href)').extract_first(),
                    
                }
                if data["title"] is not None:
                    yield data
                else:
                   pass

            


