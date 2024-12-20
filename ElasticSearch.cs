using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace elasticSearchSample
{
    internal class ElasticSearch
    {
        public ElasticClient _elasticClient; 

        public ElasticSearch(string index) 
        {
            var settings = new ConnectionSettings(new Uri("http://localhost:9200")).DefaultIndex(index); //rodando em uma porta pelo docker

            _elasticClient = new ElasticClient(settings);
        }

        public ElasticClient ElasticSearchConnection() 
        { 
            return _elasticClient;
        }
    }
}
