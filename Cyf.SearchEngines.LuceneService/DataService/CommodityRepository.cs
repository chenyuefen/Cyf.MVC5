using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cyf.SearchEngines.LuceneService.Model;
using Cyf.Core;
using Cyf.EntityFramework.Interface;
using Cyf.EntityFramework.Business;
using Cyf.EntityFramework.Model;
using System.Linq.Expressions;

namespace Cyf.SearchEngines.LuceneService.DataService
{
    /// <summary>
    /// 数据库查询
    /// </summary>
    public class CommodityRepository //: IRepository<Commodity>
    {
        private Logger logger = new Logger(typeof(CommodityRepository));

        public void SaveList(List<User> commodityList)
        {
            if (commodityList == null || commodityList.Count == 0) return;
            IEnumerable<IGrouping<string, User>> group = commodityList.GroupBy<User, string>(c => GetTableName(c));

            foreach (var data in group)
            {
                SqlHelper.InsertList<User>(data.ToList(), data.Key);
            }
        }

        private string GetTableName(User commodity)
        {
            return string.Format("JD_Commodity_{0}", (commodity.id % 30 + 1).ToString("000"));
        }

        /// <summary>
        /// 分页获取商品数据
        /// </summary>
        /// <param name="tableNum"></param>
        /// <param name="pageIndex">从1开始</param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<User> QueryList(int tableNum,int pageIndex, int pageSize)
        {
            string sql = string.Format("SELECT top {2} * FROM JD_Commodity_{0} WHERE id>{1};", tableNum.ToString("000"), pageSize * Math.Max(0, pageIndex - 1), pageSize);
            return SqlHelper.QueryList<User>(sql);
        }

        public List<User> QueryListByEF(int tableNum, int pageIndex, int pageSize)
        {
            CyfDBContext context = new CyfDBContext();
            IUserService iuser = new UserService(context);
            //var user = iuser.Find<Employee>(1);


            IUserService userService = new UserService(new CyfDBContext());
            Expression<Func<Employee, int>> funcOrderby = c => c.id;
            //var users =  userService.Find<Employee>(1);
            var dataList = userService.QueryPage(null, pageSize, pageIndex, funcOrderby);
            return dataList.DataList.Trans();
        }
    }
}
