using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cyf.MVC5.Controllers
{
    /// <summary>
    /// 【First】
    /// 1 MVC5：Controller、Action、View
    /// 2 IIS部署，Global、Log4
    /// 3 数据传值的多种方式
    /// 
    /// 【Second】
    /// 1 Route使用和扩展，Area
    /// 2 Razor语法，前后端语法混编
    /// 3 Html扩展控件，后端封装前端
    /// 4 模板页Layout,局部页PartialView 
    /// 5 IOC和MVC的结合，工厂的创建和Bussiness初始化
    /// 
    /// </summary>
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}