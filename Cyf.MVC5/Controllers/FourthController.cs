using Cyf.EntityFramework.Interface;
using Cyf.EntityFramework.Model;
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
    /// </summary>
    public class FourthController : Controller
    {
        private IUserService _iUserService = null;
        public FourthController(IUserService iUserService)
        {
            this._iUserService = iUserService;
        }

        public ActionResult Index(string searchString, string url, int? pageIndex)
        {
            var userlist = _iUserService.Query<Employee>(user => user.id < 5);
            return View(userlist);
        }
    }
}