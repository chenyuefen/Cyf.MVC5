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
            AreaRegistration.RegisterAllAreas();//ע��ȫ������ɨ��һ��
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);//ע��
            RouteConfig.RegisterRoutes(RouteTable.Routes);//ע��·��
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ControllerBuilder.Current.SetControllerFactory(new ControllerFactory());//ע���Զ���IOC����

            this.logger.Info("��վ�����ˡ�����");
        }

        /// <summary>
        /// ȫ��ʽ���쳣��������ץס©��֮��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_Error(object sender, EventArgs e)
        {
            Exception excetion = Server.GetLastError();
            this.logger.Error($"{base.Context.Request.Url.AbsoluteUri}�����쳣");
            Response.Write("System is Error....");
            Server.ClearError();

            //Response.Redirect
            //base.Context.RewritePath("/Home/Error?msg=")
        }
    }
}
