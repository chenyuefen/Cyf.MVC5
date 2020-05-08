using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cyf.MVC5.Controllers
{
    /// <summary>
    /// ************  Sixth内容  **************
    /// 1 Filter原理和AOP面向切面编程
    /// 2 全局异常处理：HandleErrorAttribute
    /// 3 IActionFilter IResultFilter扩展订制
    /// 4 Filter全总结，实战框架中AOP解决方案
    /// 
    /// </summary>
    public class SixthController : Controller
    {
        // GET: Sixth
        public ActionResult Index()
        {
            return View();
        }
    }
}