using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Elasticsearch.Net;
using Nest;

namespace VinEcom.Oms.ElasticSearch
{
    public class ElasticSearchClient : IElasticSearchClient
    {
        private readonly IElasticClient _client;
        private const string FormatDate = "yyyy-MM-ddTHH:mm:ss";
        private const string ElasticFormatDate = "yyyy-MM-ddTHH:mm:ss";

        #region constructor
        public ElasticSearchClient(ElasticSearchConfig elsElasticSearchConfig)
        {
            var uri = new Uri(elsElasticSearchConfig.Url);

            this._client = new ElasticClient(uri);
        }
        #endregion

        #region get

        public IEnumerable<T> SearchByDate<T>(int pageIndex, int pageSize, string field, DateTime from,
            DateTime to, string indexName, string typeName, out long total) where T : class
        {
            total = 0;

            if (!string.IsNullOrEmpty(indexName))
                indexName = indexName.ToLower();
            if (!string.IsNullOrEmpty(typeName))
                typeName = typeName.ToLower();
            if (!string.IsNullOrEmpty(field))
                field = char.ToLowerInvariant(field[0]) + field.Substring(1);

            var dateRange = new DateRangeQuery()
            {
                Field = field
            };
            if (from > DateTime.MinValue) dateRange.GreaterThanOrEqualTo = from.ToString(FormatDate);
            if (to > from) dateRange.LessThanOrEqualTo = to.ToString(FormatDate);

            var indices = Indices.Index(indexName);
            var types = Types.Type(typeName);
            var request = new SearchRequest(indices, types)
            {
                From = pageIndex,
                Size = pageSize,
                PostFilter = dateRange,
            };
            var sort = new List<ISort>
            {
                new SortField()
                {
                    Field = new Field() {Name = field},
                    Order = SortOrder.Descending
                }
            };

            request.Sort = sort;

            ISearchResponse<T> result = _client.Search<T>(request);
            if (result != null)
            {
                total = result.Total;
                return result.Documents;
            }
            return new List<T>();
        }

        public IEnumerable<T> Search<T>(int pageIndex, int pageSize, List<string> fieldsForKeyword, string keyword,
            string field, DateTime from, DateTime to, string indexName, string typeName, out long total, SearchTypeEnum searchType = SearchTypeEnum.IsExtract) where T : class
        {
            total = 0;

            if (!string.IsNullOrEmpty(indexName))
                indexName = indexName.ToLower();
            if (!string.IsNullOrEmpty(typeName))
                typeName = typeName.ToLower();
            if (!string.IsNullOrEmpty(field))
                field = Char.ToLowerInvariant(field[0]) + field.Substring(1);

            var dateRange = new DateRangeQuery()
            {
                Field = field
            };

            if (from > DateTime.MinValue) dateRange.GreaterThanOrEqualTo = from.ToString(FormatDate);
            if (to > from) dateRange.LessThanOrEqualTo = to.ToString(FormatDate);

            var indices = Indices.Index(indexName);
            var types = Types.Type(typeName);
            var request = new SearchRequest(indices, types)
            {
                From = pageIndex,
                Size = pageSize,
                PostFilter = dateRange
            };
            if (!string.IsNullOrEmpty(keyword))
            {
                switch (searchType)
                {
                    case SearchTypeEnum.IsExtract:
                        keyword = keyword.ToLower();
                        break;
                    case SearchTypeEnum.IsLeftRelatively:
                        keyword = string.Format("*{0}", keyword);
                        break;
                    case SearchTypeEnum.IsRightRelatively:
                        keyword = string.Format("{0}*", keyword);
                        break;
                    case SearchTypeEnum.IsRelatively:
                        keyword = string.Format("*{0}*", keyword);
                        break;
                    case SearchTypeEnum.IsFirstCharacter:
                        keyword = string.Format("?{0}", keyword);
                        break;
                    case SearchTypeEnum.IsLastCharacter:
                        keyword = string.Format("{0}?", keyword);
                        break;
                }

                if (fieldsForKeyword != null && fieldsForKeyword.Count > 0)
                {
                    for (var i = 0; i < fieldsForKeyword.Count; i++)
                    {
                        fieldsForKeyword[i] = Char.ToLowerInvariant(fieldsForKeyword[i][0]) + fieldsForKeyword[i].Substring(1);
                    }

                    var queryString = new QueryStringQuery
                    {
                        Query = keyword.ToLower(),
                        Fields = fieldsForKeyword.ToArray(),
                        AllowLeadingWildcard = true,
                        LowercaseExpendedTerms = true
                    };

                    request.Query = new QueryContainer(queryString);
                }
            }
            var sort = new List<ISort>
            {
                new SortField()
                {
                    Field = new Field() {Name = field},
                    Order = SortOrder.Descending
                }
            };

            request.Sort = sort;

            ISearchResponse<T> result = _client.Search<T>(request);

            if (result != null)
            {
                total = result.Total;
                return result.Documents;
            }
            return new List<T>();
        }

        /// <summary>
        /// Search docs by keyword using query string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keyword"></param>
        /// <param name="indexName"></param>
        /// <param name="typeName"></param>
        /// <param name="fieldsForKeyword"></param>
        /// <param name="total"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchType"></param>
        /// <returns></returns>
        public IEnumerable<T> SearchByQueryString<T>(string keyword,
           string indexName, string typeName,  List<string> fieldsForKeyword,
           out long total, int pageIndex = 0, int pageSize = 100, SearchTypeEnum searchType = SearchTypeEnum.IsExtract) where T : class
        {
            total = 0;

            if (!string.IsNullOrEmpty(indexName))
                indexName = indexName.ToLower();
            if (!string.IsNullOrEmpty(typeName))
                typeName = typeName.ToLower();
          
         
            var indices = Indices.Index(indexName);
            var types = Types.Type(typeName);
            var request = new SearchRequest(indices, types)
            {
                From = pageIndex,
                Size = pageSize,

            };
            if (!string.IsNullOrEmpty(keyword))
            {
                switch (searchType)
                {
                    case SearchTypeEnum.IsExtract:
                        //keyword = keyword.ToLower();
                        break;
                    case SearchTypeEnum.IsLeftRelatively:
                        keyword = string.Format("*{0}", keyword);
                        break;
                    case SearchTypeEnum.IsRightRelatively:
                        keyword = string.Format("{0}*", keyword);
                        break;
                    case SearchTypeEnum.IsRelatively:
                        keyword = string.Format("*{0}*", keyword);
                        break;
                    case SearchTypeEnum.IsFirstCharacter:
                        keyword = string.Format("?{0}", keyword);
                        break;
                    case SearchTypeEnum.IsLastCharacter:
                        keyword = string.Format("{0}?", keyword);
                        break;
                }

                if (fieldsForKeyword != null && fieldsForKeyword.Count > 0)
                {
                    for (var i = 0; i < fieldsForKeyword.Count; i++)
                    {
                        fieldsForKeyword[i] = Char.ToLowerInvariant(fieldsForKeyword[i][0]) + fieldsForKeyword[i].Substring(1);
                    }

                    var queryString = new QueryStringQuery
                    {
                        Query = keyword,
                        //Fields = fieldsForKeyword.ToArray(),
                        AllowLeadingWildcard = true,
                        LowercaseExpendedTerms = true
                    };

                    request.Query = new QueryContainer(queryString);
                }
            }
            //var sort = new List<ISort>
            //{
            //    new SortField()
            //    {
                    
            //        Order = SortOrder.Descending
            //    }
            //};

            //request.Sort = sort;

            ISearchResponse<T> result = _client.Search<T>(request);

            if (result != null)
            {
                total = result.Total;
                return result.Documents;
            }
            return new List<T>();
        }

        /// <summary>
        /// Query using term 
        /// term là tìm từ đơn bắt buộc dạng in thường             
        /// term query matches a single term as it is : the value is not analyzed. So, it doesn't have to be lowercased depending on what you have indexed.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="term"></param>
        /// <param name="indexName"></param>
        /// <param name="typeName"></param>
        /// <param name="field"></param>
        /// <param name="total"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IEnumerable<T> SearchByTerm<T>(string term,
         string indexName, string typeName, string field,
         out long total, int pageIndex = 0, int pageSize = 100) where T : class
        {
            total = 0;

            if (!string.IsNullOrEmpty(indexName))
                indexName = indexName.ToLower();
            if (!string.IsNullOrEmpty(typeName))
                typeName = typeName.ToLower();


            var indices = Indices.Index(indexName);
            var types = Types.Type(typeName);
            var request = new SearchRequest(indices, types)
            {
                From = pageIndex,
                Size = pageSize,

            };
            if (!string.IsNullOrEmpty(term))
            {
              
                    var queryString = new TermQuery()
                    {
                        Name = field,
                        Value = term,
                       
                    };

                    request.Query = new QueryContainer(queryString);
                
            }
            var sort = new List<ISort>
            {
                new SortField()
                {
                    Field = new Field() {Name = field},
                    Order = SortOrder.Descending
                }
            };

            request.Sort = sort;

            ISearchResponse<T> result = _client.Search<T>(request);

            if (result != null)
            {
                total = result.Total;
                return result.Documents;
            }
            return new List<T>();
        }

        /// <summary>
        /// Search by Match Phrase Query
        /// all the terms must appear in the field
        /// they must have the same order as the input value        
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="matchPhraseQuery"></param>
        /// <param name="indexName"></param>
        /// <param name="typeName"></param>
        /// <param name="field"></param>
        /// <param name="total"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IEnumerable<T> SearchByMatchPhrase<T>(string matchPhraseQuery,
        string indexName, string typeName, string field,
        out long total, int pageIndex = 0, int pageSize = 100) where T : class
        {
            total = 0;

            if (!string.IsNullOrEmpty(indexName))
                indexName = indexName.ToLower();
            if (!string.IsNullOrEmpty(typeName))
                typeName = typeName.ToLower();


            var indices = Indices.Index(indexName);
            var types = Types.Type(typeName);
            var request = new SearchRequest(indices, types)
            {
                From = pageIndex,
                Size = pageSize,

            };
            if (!string.IsNullOrEmpty(matchPhraseQuery))
            {

                var queryString = new MatchPhraseQuery()
                {
                    Name = field,
                    Query = matchPhraseQuery,

                };

                request.Query = new QueryContainer(queryString);

            }
            var sort = new List<ISort>
            {
                new SortField()
                {
                    Field = new Field() {Name = field},
                    Order = SortOrder.Descending
                }
            };

            request.Sort = sort;

            ISearchResponse<T> result = _client.Search<T>(request);

            if (result != null)
            {
                total = result.Total;
                return result.Documents;
            }
            return new List<T>();
        }

        /// <summary>
        /// Get document by id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <param name="indexName"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public T Find<T>(string id, string indexName, string typeName) where T : class
        {
            if (!string.IsNullOrEmpty(indexName))
                indexName = indexName.ToLower();
            if (!string.IsNullOrEmpty(typeName))
                typeName = typeName.ToLower();
            IGetRequest getRequest = new GetRequest(indexName, typeName, id);
            IGetResponse<T> result = _client.Get<T>(getRequest);
            return result.Source;
        }

        /// <summary>
        /// Count total document by field date range
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="indexName"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public long Count<T>(string field, DateTime from, DateTime to, string indexName, string typeName) where T : class
        {
            long total = 0;

            if (!string.IsNullOrEmpty(indexName))
                indexName = indexName.ToLower();
            if (!string.IsNullOrEmpty(typeName))
                typeName = typeName.ToLower();
            if (!string.IsNullOrEmpty(field))
                field = Char.ToLowerInvariant(field[0]) + field.Substring(1);

            var dateRange = new DateRangeQuery()
            {
                Field = field
            };

            if (from > DateTime.MinValue) dateRange.GreaterThanOrEqualTo = from.ToString(FormatDate);
            if (to > from) dateRange.LessThanOrEqualTo = to.ToString(FormatDate);

            var indices = Indices.Index(indexName);
            var types = Types.Type(typeName);
            var request = new SearchRequest(indices, types)
            {
                PostFilter = dateRange,
                SearchType = SearchType.Count
            };

            ISearchResponse<T> result = _client.Search<T>(request);
            if (result != null)
            {
                total = result.Total;
            }
            return total;
        }

        public long Count<T>(string fieldForKeyword, string keyword, string field, DateTime from, DateTime to, string indexName, string typeName) where T : class
        {
            long total = 0;
            if (!string.IsNullOrEmpty(indexName))
                indexName = indexName.ToLower();
            if (!string.IsNullOrEmpty(typeName))
                typeName = typeName.ToLower();
            if (!string.IsNullOrEmpty(fieldForKeyword))
                fieldForKeyword = Char.ToLowerInvariant(fieldForKeyword[0]) + fieldForKeyword.Substring(1);
            else
                fieldForKeyword = "_all";
            if (!string.IsNullOrEmpty(field))
                field = Char.ToLowerInvariant(field[0]) + field.Substring(1);

            var dateRange = new DateRangeQuery()
            {
                Field = field
            };
            if (from > DateTime.MinValue) dateRange.GreaterThanOrEqualTo = from.ToString(FormatDate);
            if (to > from) dateRange.LessThanOrEqualTo = to.ToString(FormatDate);


            var request = new SearchDescriptor<T>();
            request.Index(indexName);
            request.Type(typeName);

            if (!string.IsNullOrEmpty(keyword))
            {
                string statement = fieldForKeyword + "=\"" + fieldForKeyword + "\"";
                request.Query(q => q.QueryString(qs => qs.Query(statement)));
            }
            request.SearchType(SearchType.Count);
            request.PostFilter(f => dateRange);
            request.Sort(f => f.Descending(field));

            ISearchResponse<T> result = _client.Search<T>(request);
            if (result != null)
            {
                total = result.Total;
            }
            return total;
        }

        #endregion

        #region set

        public bool Index<T>(T inputObject, string id, string indexName, string typeName) where T : class
        {
            try
            {
                if (!string.IsNullOrEmpty(indexName))
                    indexName = indexName.ToLower();
                if (!string.IsNullOrEmpty(typeName))
                    typeName = typeName.ToLower();
                var result = _client.Index(inputObject, t => t.Index(indexName).Type(typeName).Id(id));
                return result.IsValid;
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                return false;
            }
        }

        public bool BulkIndexDesc<T>(IEnumerable<T> inputObject, bool isAsync = false) where T : class
        {
            try
            {
                var descriptor = new BulkDescriptor();

                foreach (T pageview in inputObject)
                {
                    T item = pageview;
                    descriptor.Index<T>(op => op.Document(item));
                }
                if (isAsync)
                    _client.BulkAsync(descriptor);
                else
                    _client.Bulk(descriptor);
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool BulkIndex<T>(IEnumerable<T> inputObjectList, string indexName, string typeName, bool isAsync = false) where T : class
        {
            try
            {
                if (!string.IsNullOrEmpty(indexName))
                    indexName = indexName.ToLower();
                if (!string.IsNullOrEmpty(typeName))
                    typeName = typeName.ToLower();

                var operations = inputObjectList.Select(item => new BulkIndexOperation<T>(item)).Cast<IBulkOperation>().ToList();

                var request = new BulkRequest(indexName, typeName)
                {
                    Refresh = true,
                    Operations = operations
                };
                if (isAsync)
                {
                    var res = _client.BulkAsync(request);
                }
                else
                {
                    var res = _client.Bulk(request);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion
    }
}
