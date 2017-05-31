using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinEcom.Oms.ElasticSearch
{
    public class ElasticSearchConfig
    {
        public ElasticSearchConfig(string url, string accountName = null, string password = null, int shardingId = 0)
        {
            this.Url = url;
            this.AccountName = accountName;
            this.Password = password;
            this.ShardingId = shardingId;
        }
        public string Url { get; set; }
        public string AccountName { get; set; }
        public string Password { get; set; }
        public int ShardingId { get; set; }
    }
}
