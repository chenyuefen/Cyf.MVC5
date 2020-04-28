using Cyf.EntityFramework.Interface;
using Cyf.EntityFramework.Model;
using Cyf.Framework.ExtendExpression;
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
    /// </summary>
    public class FourthController : Controller
    {
        private IUserService _iUserService = null;
        private int pageSize = 2;

        public FourthController(IUserService iUserService)
        {
            this._iUserService = iUserService;
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
    }
}