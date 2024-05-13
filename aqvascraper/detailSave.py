import sys
from elasticsearch import Elasticsearch
import scrapy.crawler
import scrapy.spiderloader
import urllib3
from aqvascraper.spiders.detailspider import DetailSpider
import scrapy
import os
import json
import io , requests

urllib3.disable_warnings()

start_url = sys.argv[1] if len(sys.argv) > 1 else 'https://www.sozcu.com.tr/'

response = requests.get('https://localhost:9200/', auth=('elastic', 'lp+GZ7hO-k7yAzUX3m5X'),verify=False) 
print(response.status_code)


es = Elasticsearch(
    [{'host': 'localhost', 'port': 9200, 'scheme': 'https'}],
    basic_auth=('elastic', 'lp+GZ7hO-k7yAzUX3m5X'),
    verify_certs=False
)

es.indices.create(index='detail', ignore=400) if not es.indices.exists(index='detail') else None

if os.path.exists('detail.json'):
    os.remove('detail.json')
    
process = scrapy.crawler.CrawlerProcess({
    'USER_AGENT': 'Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1)',
    'FEED_FORMAT': 'json',
    'FEED_URI': 'detail.json',
    'FEED_EXPORT_ENCODING': 'utf-8',
    'FEED_OVERWRITE': True,
})

process.crawl(DetailSpider, start_url=start_url)
process.start()

with io.open('detail.json', 'r', encoding='utf-8') as file:
    items = json.load(file)
    file.close()

details = ""
i=1
for item in items:
    url="https://localhost:9200/"
    headers = {
    'Content-Type': 'application/json',
    }
    response = requests.put(url + 'detail/_doc/' + str(i) +"?pretty", headers=headers, json=item, auth=('elastic','lp+GZ7hO-k7yAzUX3m5X'), verify=False)
    i+=1
    print(response.text)
