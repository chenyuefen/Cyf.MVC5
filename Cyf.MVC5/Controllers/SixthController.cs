using Cyf.MVC5.Utility;
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
        private Logger logger = new Logger(typeof(SixthController));

        // GET: Sixth
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 抛异常
        /// </summary>
        /// <returns></returns>
        public ActionResult Exception()
        {
            int i = 0;
            int k = 10 / i;
            return View();
        }


        /// <summary>
        /// 异常捕捉
        /// </summary>
        /// <returns></returns>
        public ActionResult ExceptionCatch()
        {
            try
            {
                int i = 0;
                int k = 10 / i;
            }
            catch (Exception ex)
            {
                this.logger.Error(ex.Message);
            }
            return View();
        }
    }
}