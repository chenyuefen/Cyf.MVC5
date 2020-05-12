using Cyf.MVC5.Utility;
using Cyf.MVC5.Utility.ViewExtend;
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

            ViewEngines.Engines.Set(new CustomViewEngine());//注册自定义视图引擎

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

        /// <summary>
        /// web.config文件配置名_事件处理名
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CustomHttpModuleCyf_ModuleHandler(object sender, EventArgs e)
        {
            this.logger.Info("this is CustomHttpModuleCyf_ModuleHandler");
        }



        /// <summary>
        /// 会在系统新增一个session时候触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Session_Start(object sender, EventArgs e)
        {
            HttpContext.Current.Application.Lock();
            object oValue = HttpContext.Current.Application.Get("TotalCount");
            if (oValue == null)
            {
                HttpContext.Current.Application.Add("TotalCount", 1);
            }
            else
            {
                HttpContext.Current.Application.Add("TotalCount", (int)oValue + 1);
            }
            HttpContext.Current.Application.UnLock();
            this.logger.Debug("这里执行了Session_Start");
        }

        /// <summary>
        /// 系统释放一个session的时候
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Session_End(object sender, EventArgs e)
        {
            this.logger.Debug("这里执行了Session_End");
        }
    }
}
