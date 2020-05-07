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
using Cyf.Web.Core.Models;
using Cyf.Web.Core.Extension;

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
    /// 
    /// 
    /// ************  请求方法的识别（Get/Post），基础特性介绍[ChildActionOnly/Bind/ValidateAntiForgeryToken]  **************
    /// 必须通过HttpVerbs来识别，
    /// 如果没有标记，那么就用方法名称来识别
    /// [ChildActionOnly] 用来指定该Action不能被单独请求，只能是子请求
    /// [Bind]指定只从前端接收哪些字段，其他的不要，防止数据的额外提交
    /// [ValidateAntiForgeryToken] 防重复提交，在cookie里加上一个key，提交的时候先校验这个
    /// 
    /// 
    /// ************  Ajax请求详解及其规范  **************
    /// Ajax请求数据响应格式：
    /// 一个项目组必须是统一的，前端才知道怎么应付
    /// 还有很多其他情况，比如异常了--exceptionfilter--按照固定格式返回
    ///                   比如没有权限--authorization--按照固定格式返回
    ///                   
    /// 
    /// ************  Http请求的本质及不同请求contenttype的区别  **************
    /// 请求--应答式，响应可以那么丰富？
    /// 不同的类型其实方式一样的，只不过有个contenttype的差别
    /// html---text/html
    /// json---application/json
    /// xml---application/xml
    /// js----application/javascript
    /// ico----image/x-icon
    /// image/gif   image/jpeg   image/png
    /// 
    /// 
    /// ************  MVC各种Result的事儿  **************
    /// Json方法实际上是new JsonResult 然后ExecuteResult
    /// 指定ContentType-application/json  然后将Data序列化成字符串写入stream
    /// 我们可以随意扩展的，只需要把数据放入response  指定好contenttype
    /// </summary>
    public class FourthController : Controller
    {
        private IUserService _iCommodityService = null;
        private ISearchService _iSearchService = null;
        private ICompanyService _iCompanyService = null;

        private int pageSize = 2;

        public FourthController(IUserService userService, ISearchService searchService, ICompanyService companyService)
        {
            this._iCommodityService = userService;
            this._iSearchService = searchService;
            this._iCompanyService = companyService;
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
            //var commodityList = this._iCommodityService.Query<Employee>(funcWhere);
            #endregion

            #region 排序
            Expression<Func<Employee, int>> funcOrderby = c => c.id;
            #endregion

            int index = pageIndex ?? 1;

            PageResult<Employee> commodityList = this._iCommodityService.QueryPage(funcWhere, pageSize, index, funcOrderby, true);

            StaticPagedList<Employee> pageList = new StaticPagedList<Employee>(commodityList.DataList, index, pageSize, commodityList.TotalCount);


            return View(pageList);
        }


        #region Lucene查询

        public ActionResult Search()
        {
            var result = this._iSearchService.QueryCommodityPage(1, 20, "女人", null, "", "");


            ISearchService searchService1 = DIFactory.GetContainer().Resolve<ISearchService>();
            ISearchService searchService2 = DIFactory.GetContainer().Resolve<ISearchService>("update");
            return View(result);
        }
        #endregion

        #region Create
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.companyList = BuildCompanyList();

            return View();
        }
        [AcceptVerbs(HttpVerbs.Put | HttpVerbs.Post)] //[HttpPost]
        //[HttpPost]
        //[HttpGet]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "company_id, name, position")]Employee commodity)
        {
            string title1 = this.HttpContext.Request.Params["name"];
            string title2 = this.HttpContext.Request.QueryString["name"];
            string title3 = this.HttpContext.Request.Form["name"];
            if (ModelState.IsValid)//数据校验
            {
                Employee newCommodity = this._iCommodityService.Insert(commodity);
                return RedirectToAction("Index");
            }
            else
            {
                throw new Exception("ModelState未通过检测");
            }
        }

        [HttpPost]
        public ActionResult AjaxCreate([Bind(Include = "company_id, name, position")]Employee commodity)
        {
            Employee newCommodity = this._iCommodityService.Insert(commodity);
            AjaxResult ajaxResult = new AjaxResult()
            {
                Result = DoResult.Success,
                PromptMsg = "插入成功"
            };
            return Json(ajaxResult);
        }
        #endregion Create

        #region Details Edit Delete
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                throw new Exception("need commodity id");
            }
            Employee commodity = this._iCommodityService.Find<Employee>(id ?? -1);
            if (commodity == null)
            {
                throw new Exception("Not Found Commodity");
            }
            return View(commodity);
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                throw new Exception("need commodity id");
            }
            Employee commodity = this._iCommodityService.Find<Employee>(id ?? -1);
            if (commodity == null)
            {
                throw new Exception("Not Found");
            }
            ViewBag.companyList = BuildCompanyList();
            return View(commodity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Employee commodity)
        {
            if (ModelState.IsValid)
            {
                Employee commodityDB = this._iCommodityService.Find<Employee>(commodity.id);
                commodityDB.company_id = commodity.company_id;
                commodityDB.name = commodity.name;
                commodityDB.position = commodity.position;
                this._iCommodityService.Update(commodityDB);
                return RedirectToAction("Index");
            }
            else
                throw new Exception("ModelState未通过检测");
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                throw new Exception("Not Found");
            }
            Employee commodity = this._iCommodityService.Find<Employee>(id ?? -1);
            if (commodity == null)
            {
                throw new Exception("Not Found");
            }
            else
            {
                this._iCommodityService.Delete(commodity);
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// ajax删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AjaxDelete(int id)
        {
            this._iCommodityService.Delete<Employee>(id);
            AjaxResult ajaxResult = new AjaxResult()
            {
                Result = DoResult.Success,
                PromptMsg = "删除成功"
            };
            return Json(ajaxResult);
        }
        #endregion Details Edit Delete

        private IEnumerable<SelectListItem> BuildCompanyList()
        {
            Expression<Func<Company, bool>> funcWhere = null;
            funcWhere = funcWhere.And(x => x.company_id != null);
            var categoryList = this._iCompanyService.Query(funcWhere).ToList();
            if (categoryList.Count() > 0)
            {
                return categoryList.Select(c => new SelectListItem()
                {
                    Text = c.company_name,
                    Value = c.company_id.ToString(),
                });
            }
            else return null;
        }

        #region TestResult
        /// <summary>
        /// 返回ActionResult--MVC框架会执行其ExecuteResult方法---
        /// </summary>
        /// <returns></returns>
        public JsonResult JsonResultIn()
        {
            return Json(new AjaxResult()
            {
                Result = DoResult.Success,
                DebugMessage = "这里是JsonResult"
            }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 返回ActionResult--MVC框架会执行其ExecuteResult方法---
        /// </summary>
        /// <returns></returns>
        public NewtonJsonResult JsonResultNewton()
        {
            return new NewtonJsonResult(new AjaxResult()
            {
                Result = DoResult.Success,
                DebugMessage = "这里是JsonResult"
            });
        }

        /// <summary>
        /// 不是ActionResult---直接当结果写入stream，默认的contenttype是html
        /// </summary>
        /// <returns></returns>
        public string JsonResultString()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(new AjaxResult()
            {
                Result = DoResult.Success,
                DebugMessage = "这里是JsonResult"
            });
        }
        /// <summary>
        /// 没有返回--直接写入stream
        /// </summary>
        public void JsonResultVoid()
        {
            HttpResponseBase response = base.HttpContext.Response;
            response.ContentType = "application/json";
            response.Write(
                Newtonsoft.Json.JsonConvert.SerializeObject(new AjaxResult()
                {
                    Result = DoResult.Success,
                    DebugMessage = "这里是JsonResult"
                }));
        }

        public XmlResult XmlResult()
        {
            return new XmlResult(new AjaxResult()
            {
                Result = DoResult.Success,
                DebugMessage = "这里是JsonResult"
            });
        }
        //ExcelResult--NPOI---
        #endregion
    }



}

public class NewtonJsonResult : ActionResult
{
    private object Data = null;
    public NewtonJsonResult(object data)
    {
        this.Data = data;
    }
    public override void ExecuteResult(ControllerContext context)
    {
        HttpResponseBase response = context.HttpContext.Response;
        response.ContentType = "application/json";
        response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(this.Data));
    }
}