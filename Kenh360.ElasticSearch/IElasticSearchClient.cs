using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinEcom.Oms.ElasticSearch
{
    /// <summary>
    /// Interface IElasticSearchClient
    /// </summary>
    public interface IElasticSearchClient
    {
        bool Index<T>(T inputObject, string id, string indexName, string typeName) where T : class;

        IEnumerable<T> SearchByDate<T>(int pageIndex, int pageSize, string field, DateTime from,
            DateTime to, string indexName, string typeName, out long total) where T : class;

        IEnumerable<T> Search<T>(int pageIndex, int pageSize, List<string> fieldsForKeyword, string keyword,
            string field, DateTime from, DateTime to, string indexName, string typeName, out long total, SearchTypeEnum searchType = SearchTypeEnum.IsExtract) where T : class;

        IEnumerable<T> SearchByQueryString<T>(string keyword,
           string indexName, string typeName, List<string> fieldsForKeyword,
           out long total, int pageIndex = 0, int pageSize = 100, SearchTypeEnum searchType = SearchTypeEnum.IsExtract) where T : class;

        IEnumerable<T> SearchByMatchPhrase<T>(string matchPhraseQuery,
            string indexName, string typeName, string field,
            out long total, int pageIndex = 0, int pageSize = 100) where T : class;

        IEnumerable<T> SearchByTerm<T>(string term,
            string indexName, string typeName, string field,
            out long total, int pageIndex = 0, int pageSize = 100) where T : class;

        T Find<T>(string id, string indexName, string typeName) where T : class;

        bool BulkIndexDesc<T>(IEnumerable<T> inputObjects, bool isAsync = false) where T : class;

        bool BulkIndex<T>(IEnumerable<T> inputObjectList, string indexName, string typeName, bool isAsync = false) where T : class;

        long Count<T>(string field, DateTime from, DateTime to, string indexName, string typeName) where T : class;

        long Count<T>(string fieldForKeyword, string keyword, string field, DateTime from, DateTime to, string indexName, string typeName) where T : class;
    }
}
