using Cyf.EntityFramework.Business;
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
    /// </summary>
    public class ThirdController : Controller
    {
        // GET: Third
        public ActionResult Index()
        {
            CyfDBContext context= new CyfDBContext();
            IUserService iuser = new UserService(context);
            var user = iuser.Find<employee>(1);
            return View();
        }
    }
}