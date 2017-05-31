using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinEcom.Oms.ElasticSearch
{
    #region Input

    public abstract class ESRequest
    {
        public ESRequest(string keyword)
        {
            this.Keyword = keyword;
        }
        public ESRequest(string keyword, DateTime fromDate, DateTime toDate)
        {
            this.Keyword = keyword;
            this.FromDate = fromDate;
            this.ToDate = toDate;
        }
        public string Keyword { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public bool IsSearchExtractly { get; set; }
    }
    public class ESRequestSearchByKeyword : ESRequest
    {
        public ESRequestSearchByKeyword(string keyword)
            : base(keyword)
        {

        }
        public string KeywordFieldName { get; set; }
    }
    public class ESRequestSearchByDate : ESRequest
    {
        public ESRequestSearchByDate(string keyword)
            : base(keyword)
        {

        }
        public ESRequestSearchByDate(string keyword, DateTime fromDate, DateTime toDate)
            : base(keyword, fromDate, toDate)
        {

        }
        public string KeywordFieldName { get; set; }
        public string DateFieldName { get; set; }
    }

    #endregion Input

    #region Output

    public class ESResponse<T> where T : class
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int TotalRows { get; set; }
        public T Data { get; set; }
    }

    #endregion

    #region Enum

    public enum SearchTypeEnum
    {
        /// <summary>
        /// Chính xác: 1
        /// </summary>
        [Description("Chính xác")]
        IsExtract = 1,
        /// <summary>
        /// Tương đối trái: 2
        /// </summary>
        [Description("Tương đối trái")]
        IsLeftRelatively = 2,
        /// <summary>
        /// Tương đối phải: 3
        /// </summary>
        [Description("Tương đối phải")]
        IsRightRelatively = 3,
        /// <summary>
        /// Tương đối: 4
        /// </summary>
        [Description("Tương đối")]
        IsRelatively = 4,
        /// <summary>
        /// Tương đối ký tự đầu: 5
        /// </summary>
        [Description("Tương đối ký tự đầu")]
        IsFirstCharacter = 5,
        /// <summary>
        /// Tương đối ký tự cuối: 6
        /// </summary>
        [Description("Tương đối ký tự cuối")]
        IsLastCharacter = 6
    }

    #endregion
}
