using Cyf.EntityFramework.Model;
using Cyf.Remote.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyf.Remote.Service
{
    /// <summary>
    /// 封装了对分布式搜索服务
    /// 支持分页查询，
    /// </summary>
    public class SearchServiceUpdate : ISearchService
    {
        public PageResult<Employee> QueryCommodityPage(int pageIndex, int pageSize, string keyword, List<int> categoryIdList, string priceFilter, string priceOrderBy)
        {
            LuceneSearchService.SearcherClient client = null;
            try
            {
                client = new LuceneSearchService.SearcherClient();
                string result = client.QueryCommodityPage(pageIndex, pageSize, keyword, categoryIdList?.ToArray(), priceFilter, priceOrderBy);

                client.Close();//会关闭链接，但是如果网络异常了，会抛出异常而且关闭不了
                return Newtonsoft.Json.JsonConvert.DeserializeObject<PageResult<Employee>>(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (client != null)
                    client.Abort();
                throw ex;
            }

        }
    }
}
