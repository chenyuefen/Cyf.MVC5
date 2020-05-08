using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cyf.MVC5.Utility.FilterTest
{
    public class TestGlobalActionFilterAttribute : ActionFilterAttribute
    {
        private Logger logger = new Logger(typeof(TestGlobalActionFilterAttribute));
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            this.logger.Info($"Global OnActionExecuting {this.Order}");
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            this.logger.Info($"Global OnActionExecuted {this.Order}");
        }
    }
}