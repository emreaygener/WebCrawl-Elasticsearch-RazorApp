from elasticsearch import Elasticsearch
import scrapy.crawler
import scrapy.spiderloader
import urllib3
from aqvascraper.spiders.aqvaspider import AqvaSpider
import scrapy
import os
import json
import io , requests

urllib3.disable_warnings()

response = requests.get('https://localhost:9200/', auth=('elastic', 'lp+GZ7hO-k7yAzUX3m5X'),verify=False) 
print(response.status_code)


es = Elasticsearch(
    [{'host': 'localhost', 'port': 9200, 'scheme': 'https'}],
    basic_auth=('elastic', 'lp+GZ7hO-k7yAzUX3m5X'),
    verify_certs=False
)

es.indices.create(index='news', ignore=400) if not es.indices.exists(index='news') else None

if os.path.exists('aqva.json'):
    os.remove('aqva.json')
    
process = scrapy.crawler.CrawlerProcess({
    'USER_AGENT': 'Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1)',
    'FEED_FORMAT': 'json',
    'FEED_URI': 'aqva.json',
    'FEED_EXPORT_ENCODING': 'utf-8',
    'FEED_OVERWRITE': True,
})

process.crawl(AqvaSpider)
process.start()

with io.open('aqva.json', 'r', encoding='utf-8') as file:
    items = json.load(file)
    file.close()

documents = []
i=1
for item in items:
    url="https://localhost:9200/"
    headers = {
    'Content-Type': 'application/json',
    }
    response = requests.put(url + 'news/_doc/' + str(i) +"?pretty", headers=headers, json=item, auth=('elastic','lp+GZ7hO-k7yAzUX3m5X'), verify=False)
    i+=1
    print(response.text)
