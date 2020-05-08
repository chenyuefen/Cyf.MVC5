using Cyf.MVC5.Utility;
using Cyf.MVC5.Utility.Filter;
using Cyf.MVC5.Utility.FilterTest;
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
    /// 
    /// ************  ActionFilterAttribute系统自带AOP特性  **************
    /// -> OnActionExecuting  
    /// -> Action真实执行 
    /// -> OnActionExecuted
    /// -> OnResultExecuting
    /// -> Action跳转的视图/返回的ActionResult真实执行
    /// -> OnResultExecuted
    /// 
    /// 
    ///  ************  在不同地方（Globle/Controller/Aciton）使用ActionFilterAttribute，其执行顺序  **************
    /// 俄罗斯套娃
    /// Global OnActionExecuting
    /// Controller OnActionExecuting
    /// Action OnActionExecuting
    /// Action真实执行
    /// Action OnActionExecuted
    /// Controller OnActionExecuted
    /// Global OnActionExecuted
    /// 
    /// 不同注册位置生效顺序--全局/控制器/Action
    /// 同一位置按照先后顺序生效
    /// (不设置Order默认是1) 
    /// 
    /// ** 【设置后是按照从小到大执行】
    /// </summary>
    [TestControllerActionFilter]
    public class SixthController : Controller
    {
        private Logger logger = new Logger(typeof(SixthController));
        #region 多异常情况
        //1 Action异常,没被catch                       T 
        //2 Action异常,被catch                         F
        //3 Action调用Service异常                      T  异常冒泡 
        //4 Action正常视图出现异常                     T  ExecuteResult是包裹在try里面的
        //5 控制器构造出现异常                         F  控制器构造后才有Filter
        //6 Action名称错误                             F  因为请求其实都没进mvc流程
        //7 任意错误地址                               F
        //8 权限Filter异常                             T  权限fileter也是在try里面的
        //全局注册，能不能进入自定义的异常Filter
        //T：能被自定义错误特性捕捉
        //F：不能
        #endregion
        // GET: Sixth
        [CustomActionFilter]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 抛异常
        /// </summary>
        /// <returns></returns>
        [TestActionFilter]
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