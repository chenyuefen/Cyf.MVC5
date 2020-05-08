using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cyf.MVC5.Utility.FilterTest
{
    public class TestControllerActionFilterAttribute : ActionFilterAttribute
    {
        private Logger logger = new Logger(typeof(TestControllerActionFilterAttribute));
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            this.logger.Info($"Controller OnActionExecuting {this.Order}");
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            this.logger.Info($"Controller OnActionExecuted {this.Order}");
        }
    }
}