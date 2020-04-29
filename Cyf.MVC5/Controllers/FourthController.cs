using Cyf.EntityFramework.Interface;
using Cyf.EntityFramework.Model;
using Cyf.Framework.ExtendExpression;
using Cyf.MVC5.Utility;
using Cyf.Remote.Interface;
using Unity;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace Cyf.MVC5.Controllers
{
    /// <summary>
    /// ************  Fourth内容  **************
    /// 1 列表绑定、增删改查
    /// 2 WCF搜索引擎的封装调用和AOP的整合
    /// 3 Ajax删除、Ajax表单提交、Ajax列表、Ajax三级联动
    /// 4 Http请求的本质，各种ActionResult扩展订制
    /// 
    /// 
    /// ************  前后端数据绑定  **************
    /// 前端暂时就是razor
    /// a Bussiness增加接口+实现
    /// b IOC配置文件
    /// c 注入到控制器
    /// d 查询数据库，传递到前端，绑定一下
    /// e 接受参数,拼装参数
    /// f 参数ViewBag传递到前端再绑定
    /// g 分页使用pagedlist---返回数据用StaticPagedList--前端分页url带上参数--接受分页参数
    /// h 分页点击后重置页码数，就是设置表单的action
    /// 
    /// 
    /// ************  实现搜索服务  **************
    /// 1 运行LuceneSearch文件夹下Cyf.SearchEngines项目
    /// 2 打开WCF服务
    /// 3 直接调用
    /// 
    /// 接口服务查询，建议封装一下；
    /// 建议跟数据库查询独立分开；
    /// 也是接口+实现+model,然后就IOC
    /// 应用程序的配置文件需要加上服务相关
    /// 
    /// Cyf.Framework：通用的帮助类库，这里面放的是任何一个项目都可能用上的，这个类库可以被任何类库引用，但是自身不引用任何类库；
    /// 只要是我用的东西，都得写在我自己里面；如果必须用到别的类库的东西，可以通过委托传递进来；
    /// Cyf.Web.Core：专门为MVC网站服务的通用的帮助
    /// </summary>
    public class FourthController : Controller
    {
        private IUserService _iUserService = null;
        private ISearchService _iSearchService = null;

        private int pageSize = 2;

        public FourthController(IUserService userService, ISearchService searchService)
        {
            this._iUserService = userService;
            this._iSearchService = searchService;
        }

        public ActionResult Index(string searchString,int? pageIndex)
        {
            #region 查询条件

            //searchString = base.HttpContext.Request.Form["searchString"];
            searchString = base.HttpContext.Request["searchString"];
            Expression<Func<Employee, bool>> funcWhere = null;
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                funcWhere = funcWhere.And(c => c.name.Contains(searchString));
                base.ViewBag.SearchString = searchString;
            }

            //这里还可以延伸一下，做一个动态封装查询条件--
            //var commodityList = this._iCommodityService.Query<JDCommodity>(funcWhere);
            #endregion

            #region 排序
            Expression<Func<Employee, int>> funcOrderby = c => c.id;
            #endregion

            int index = pageIndex ?? 1;

            PageResult<Employee> commodityList = this._iUserService.QueryPage(funcWhere, pageSize, index, funcOrderby, true);

            StaticPagedList<Employee> pageList = new StaticPagedList<Employee>(commodityList.DataList, index, pageSize, commodityList.TotalCount);


            return View(pageList);
        }

        public ActionResult Search()
        {
            var result = this._iSearchService.QueryCommodityPage(1, 20, "女人", null, "", "");


            ISearchService searchService1 = DIFactory.GetContainer().Resolve<ISearchService>();
            ISearchService searchService2 = DIFactory.GetContainer().Resolve<ISearchService>("update");
            return View(result);
        }
    }
}