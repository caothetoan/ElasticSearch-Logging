using System;
using Infrastructure.Elastic;

namespace ElasticSearchNetClient
{
	public interface IElasticsearchMappingResolver
	{
		ElasticsearchMapping GetElasticSearchMapping(Type type);
		void AddElasticSearchMappingForEntityType(Type type, ElasticsearchMapping mapping);
	}
}