using Cyf.MVC5.Models;
using Cyf.Web.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cyf.MVC5.Utility.Filter
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        private Logger logger = new Logger(typeof(CustomAuthorizeAttribute));
        private string _LoginUrl = null;
        public CustomAuthorizeAttribute(string loginUrl = "~/Home/Login")
        {
            this._LoginUrl = loginUrl;
        }
        //public CustomAuthorizeAttribute(ICompanyUserService service)
        //{
        //}
        //不行


        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var httpContext = filterContext.HttpContext;//能拿到httpcontext 就可以为所欲为

            //检验特性
            if (filterContext.ActionDescriptor.IsDefined(typeof(CustomAllowAnonymousAttribute), true))
            {
                return;
            }
            //检验特性
            else if (filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(CustomAllowAnonymousAttribute), true))
            {
                return;
            }
            if (httpContext.Session["CurrentUser"] == null
                || !(httpContext.Session["CurrentUser"] is CurrentLoginUser))//为空了，
            {
                //这里有用户，有地址 其实可以检查权限
                //如果是Ajax请求。则不能跳转到原链接，应该返回固定格式的数据
                if (httpContext.Request.IsAjaxRequest())
                //httpContext.Request.Headers["xxx"].Equals("XMLHttpRequst")
                {
                    filterContext.Result = new NewtonJsonResult(
                        new AjaxResult()
                        {
                            Result = DoResult.OverTime,
                            DebugMessage = "登陆过期",
                            RetValue = ""
                        });
                }
                else
                {
                    //记录跳转前的绝对地址
                    httpContext.Session["CurrentUrl"] = httpContext.Request.Url.AbsoluteUri;
                    filterContext.Result = new RedirectResult(this._LoginUrl);
                    //短路器：指定了Result，那么请求就截止了，不会执行action
                }
            }
            else
            {
                CurrentLoginUser user = (CurrentLoginUser)httpContext.Session["CurrentUser"];
                //this.logger.Info($"{user.Name}登陆了系统");
                return;//继续
            }
            //base.OnAuthorization(filterContext);
        }
    }
}