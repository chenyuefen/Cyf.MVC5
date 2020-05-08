using Cyf.MVC5.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Cyf.MVC5
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private Logger logger = new Logger(typeof(MvcApplication));

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();//注册全部区域，扫描一遍
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);//注册
            RouteConfig.RegisterRoutes(RouteTable.Routes);//注册路由
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ControllerBuilder.Current.SetControllerFactory(new ControllerFactory());//注册自定义IOC工厂

            this.logger.Info("网站启动了。。。");
        }

        /// <summary>
        /// 全局式的异常处理，可以抓住漏网之鱼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_Error(object sender, EventArgs e)
        {
            Exception excetion = Server.GetLastError();
            this.logger.Error($"{base.Context.Request.Url.AbsoluteUri}出现异常");
            Response.Write("System is Error....");
            Server.ClearError();

            //Response.Redirect
            //base.Context.RewritePath("/Home/Error?msg=")
        }
    }
}
