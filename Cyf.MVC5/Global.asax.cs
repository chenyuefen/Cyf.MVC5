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
            AreaRegistration.RegisterAllAreas();//ע��ȫ������ɨ��һ��
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);//ע��
            RouteConfig.RegisterRoutes(RouteTable.Routes);//ע��·��
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ControllerBuilder.Current.SetControllerFactory(new ControllerFactory());//ע���Զ���IOC����

            ViewEngines.Engines.Set(new CustomViewEngine());//ע���Զ�����ͼ����

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

        /// <summary>
        /// web.config�ļ�������_�¼�������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CustomHttpModuleCyf_ModuleHandler(object sender, EventArgs e)
        {
            this.logger.Info("this is CustomHttpModuleCyf_ModuleHandler");
        }



        /// <summary>
        /// ����ϵͳ����һ��sessionʱ�򴥷�
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
            this.logger.Debug("����ִ����Session_Start");
        }

        /// <summary>
        /// ϵͳ�ͷ�һ��session��ʱ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Session_End(object sender, EventArgs e)
        {
            this.logger.Debug("����ִ����Session_End");
        }
    }
}
