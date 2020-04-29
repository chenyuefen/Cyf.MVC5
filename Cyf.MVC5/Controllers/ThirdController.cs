using Cyf.EntityFramework.Business;
using Cyf.EntityFramework.Interface;
using Cyf.EntityFramework.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cyf.MVC5.Controllers
{
    /// <summary>
    /// ************  Third内容  **************
    /// 1 IOC和MVC的结合，工厂的创建和Bussiness初始化
    /// 2 WCF搜索引擎的封装调用和AOP的整合
    /// 3 HTTP请求的本质，各种ActionResult扩展订制
    /// 
    /// ************  MVC使用Mysql EF6  **************
    /// - 依赖的EF需要引入
    /// - 安装Mysql.Data.EntityFramework
    /// - web.config数据库连接需要配置connectionStrings
    ///
    /// 
    /// ************  使用Unity实现MVC的IOC/AOP依赖注入  **************
    /// 1 创建ControllerFactory继承自DefaultControllerFactory
    /// 2 引入第三方容器--将控制器的实例化换成容器操作【这里采用Unity】
    ///    - Unity
    ///    - Unity.Abstractions
    ///    - Unity.Configuration
    ///    - Unity.Container
    ///    - Unity.Interception
    ///    - Unity.Interception.Configuration
    /// 3 需要在Global.asax里注册全局的控制器依赖ControllerBuilder.Current.SetControllerFactory
    /// 
    /// </summary>
    public class ThirdController : Controller
    {
        private IUserService _iUserService = null;
        private ICompanyService _iCompanyService = null;

        /// <summary>
        /// 构造函数注入---控制器得是由容器来实例化
        /// </summary>
        /// <param name="userService"></param>
        /// <param name="companyService"></param>
        /// <param name="userCompanyService"></param>
        public ThirdController(IUserService userService, ICompanyService companyService)
        {
            this._iUserService = userService;
            this._iCompanyService = companyService;
        }

        // GET: Third
        public ActionResult Index()
        {
            using (CyfDBContext context = new CyfDBContext())
            {
                var use = context.employees.ToList();
            }
            //CyfDBContext context= new CyfDBContext();
            //IUserService iuser = new UserService(context);
            //var user = iuser.Find<employee>(1);
            var user = _iUserService.Find<Employee>(1);
            return View();
        }
    }
}