﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cyf.MVC5.Controllers
{
    /// <summary>
    /// ************  Second内容  **************
    /// 1 Route使用和扩展，Area
    /// 2 Razor语法，前后端语法混编
    /// 3 Html扩展控件，后端封装前端
    /// 4 模板页Layout,局部页PartialView 
    /// 5 IOC和MVC的结合，工厂的创建和Bussiness初始化
    /// 
    /// 
    /// ************  Route路由使用和扩展  **************
    /// MvcApplication--Application_Start--RegisterRoutes--给RouteCollection添加规则
    /// 请求进到网站--X--请求地址被路由按顺序匹配--遇到一个吻合的结束---就到对应的控制器和action
    /// 
    /// 
    /// ************  Area区域：为了拆分项目  **************
    /// 因为一个Web项目可以非常大非常复杂，多人合作开发，命名就成问题了，Area可以把项目拆分开，方便团队合作；演变到后面可以做成插件式开发：
    /// MvcApplication--Application_Start--AreaRegistration.RegisterAllAreas()
    /// 其实就是把SystemAreaRegistration给注册下---添加URL地址规则
    /// 请求来了就匹配(area在普通的之前)
    /// 
    /// 
    /// </summary>
    public class SecondController : Controller
    {
        // GET: Second
        public ActionResult Index()
        {
            return View();            
        }

        public string String()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff");
        }

        public string Time(int year, int month, int day)
        {
            return $"当前传入日期：{year}-{month}-{day}";
        }
    }
}