using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cyf.MVC5.Utility.ViewExtend
{
    public static class ViewEngineExt
    {
        public static void Set(this ViewEngineCollection viewEngines, RazorViewEngine engine)
        {
            viewEngines?.Clear();
            viewEngines.Add(engine);
        }
    }

    /// <summary>
    /// 解决方案：
    /// a 覆写的是FindView而不是CreateView,而且一定得set回去
    /// b CreateView时直接修改path
    /// 注意不同的路径如_ViewStart
    /// 
    /// 用A或B都可以
    /// </summary>
    public class CustomViewEngine : RazorViewEngine
    {
        #region 构造函数
        public CustomViewEngine() : this(null)
        {
        }
        public CustomViewEngine(IViewPageActivator viewPageActivator) : base(viewPageActivator)
        {
            this.SetEngine("");
        }
        #endregion

        #region 方法A
        public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            //if (controllerContext.HttpContext.Request.UserAgent.Contains("Chrome/74.0.3729.169"))
            //{
            //    this.SetEngine("Chrome");
            //}
            //else
            //{
            //    this.SetEngine("");//一定得有，因为只有一个Engine实例
            //}
            return base.FindView(controllerContext, viewName, masterName, useCache);
        }

        public override ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
        {
            //if (controllerContext.HttpContext.Request.UserAgent.Contains("Chrome/74.0.3729.169"))
            //{
            //    this.SetEngine("Chrome");
            //}
            //else
            //{
            //    this.SetEngine("");
            //}
            return base.FindPartialView(controllerContext, partialViewName, useCache);
        }
        #endregion


        #region 方法B
        /// <summary>
        /// 
        /// </summary>
        /// <param name="controllerContext">可以为所欲为</param>
        /// <param name="partialPath"></param>
        /// <returns></returns>
        protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
        {
            if (controllerContext.HttpContext.Request.UserAgent.Contains("Chrome"))
            {
                partialPath = partialPath.Replace("/Views/", "/ChromeViews/");
            }
            return base.CreatePartialView(controllerContext, partialPath);
        }
        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            if (controllerContext.HttpContext.Request.UserAgent.Contains("Chrome"))
            {
                viewPath = viewPath.Replace("/Views/", "/ChromeViews/");
                masterPath = masterPath.Replace("/Views/", "/ChromeViews/");
            }
            return base.CreateView(controllerContext, viewPath, masterPath);
        }
        #endregion
        /// <summary>
        /// 把模板给换了
        /// 从源码拷贝过来【固定的】
        /// Views从以下路径遍历
        /// </summary>
        /// <param name="browser"></param>
        private void SetEngine(string browser)
        {
            if (!string.IsNullOrWhiteSpace(browser))
            {
                base.AreaViewLocationFormats = new string[]
                {
                    "~/Areas/{2}/"+browser+"Views/{1}/{0}.cshtml",
                    "~/Areas/{2}/"+browser+"Views/{1}/{0}.vbhtml",
                    "~/Areas/{2}/"+browser+"Views/Shared/{0}.cshtml",
                    "~/Areas/{2}/"+browser+"Views/Shared/{0}.vbhtml"
                };
                base.AreaMasterLocationFormats = new string[]
                {
                    "~/Areas/{2}/"+browser+"Views/{1}/{0}.cshtml",
                    "~/Areas/{2}/"+browser+"Views/{1}/{0}.vbhtml",
                    "~/Areas/{2}/"+browser+"Views/Shared/{0}.cshtml",
                    "~/Areas/{2}/"+browser+"Views/Shared/{0}.vbhtml"
                };
                base.AreaPartialViewLocationFormats = new string[]
                {
                    "~/Areas/{2}/"+browser+"Views/{1}/{0}.cshtml",
                    "~/Areas/{2}/"+browser+"Views/{1}/{0}.vbhtml",
                    "~/Areas/{2}/"+browser+"Views/Shared/{0}.cshtml",
                    "~/Areas/{2}/"+browser+"Views/Shared/{0}.vbhtml"
                };
                base.ViewLocationFormats = new string[]
                {
                    "~/"+browser+"Views/{1}/{0}.cshtml",
                    "~/"+browser+"Views/{1}/{0}.vbhtml",
                    "~/"+browser+"Views/Shared/{0}.cshtml",
                    "~/"+browser+"Views/Shared/{0}.vbhtml"
                };
                base.MasterLocationFormats = new string[]
                {
                    "~/"+browser+"Views/{1}/{0}.cshtml",
                    "~/"+browser+"Views/{1}/{0}.vbhtml",
                    "~/"+browser+"Views/Shared/{0}.cshtml",
                    "~/"+browser+"Views/Shared/{0}.vbhtml"
                };
                base.PartialViewLocationFormats = new string[]
                {
                    "~/"+browser+"Views/{1}/{0}.cshtml",
                    "~/"+browser+"Views/{1}/{0}.vbhtml",
                    "~/"+browser+"Views/Shared/{0}.cshtml",
                    "~/"+browser+"Views/Shared/{0}.vbhtml"
                };
            }
            else
            {
                base.AreaViewLocationFormats = new string[]
               {
                    "~/Areas/{2}/Views/{1}/{0}.cshtml",
                    "~/Areas/{2}/Views/{1}/{0}.vbhtml",
                    "~/Areas/{2}/Views/Shared/{0}.cshtml",
                    "~/Areas/{2}/Views/Shared/{0}.vbhtml"
               };
                base.AreaMasterLocationFormats = new string[]
                {
                    "~/Areas/{2}/Views/{1}/{0}.cshtml",
                    "~/Areas/{2}/Views/{1}/{0}.vbhtml",
                    "~/Areas/{2}/Views/Shared/{0}.cshtml",
                    "~/Areas/{2}/Views/Shared/{0}.vbhtml"
                };
                base.AreaPartialViewLocationFormats = new string[]
                {
                    "~/Areas/{2}/Views/{1}/{0}.cshtml",
                    "~/Areas/{2}/Views/{1}/{0}.vbhtml",
                    "~/Areas/{2}/Views/Shared/{0}.cshtml",
                    "~/Areas/{2}/Views/Shared/{0}.vbhtml"
                };
                base.ViewLocationFormats = new string[]
                {
                    "~/Views/{1}/{0}.cshtml",
                    "~/Views/{1}/{0}.vbhtml",
                    "~/Views/Shared/{0}.cshtml",
                    "~/Views/Shared/{0}.vbhtml"
                };
                base.MasterLocationFormats = new string[]
                {
                    "~/Views/{1}/{0}.cshtml",
                    "~/Views/{1}/{0}.vbhtml",
                    "~/Views/Shared/{0}.cshtml",
                    "~/Views/Shared/{0}.vbhtml"
                };
                base.PartialViewLocationFormats = new string[]
                {
                    "~/Views/{1}/{0}.cshtml",
                    "~/Views/{1}/{0}.vbhtml",
                    "~/Views/Shared/{0}.cshtml",
                    "~/Views/Shared/{0}.vbhtml"
                };
                base.FileExtensions = new string[]
                {
                    "cshtml",
                    "vbhtml"
                };
            }
        }
    }
}