﻿using Cyf.MVC5.Utility.Filter;
using System.Web;
using System.Web.Mvc;

namespace Cyf.MVC5
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new HandleErrorAttribute());
            filters.Add(new CustomHandleErrorAttribute());
            //filters.Add(new CustomAuthorizeAttribute("~/Fifth/Login"));
        }
    }
}
