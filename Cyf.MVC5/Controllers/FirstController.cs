using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cyf.MVC5.Controllers
{
    /// <summary>
    /// ************  First内容  **************
    /// 1 MVC5：Controller、Action、View
    /// 2 IIS部署，Global、Log4
    /// 3 数据传值的多种方式
    /// 4 Route使用和扩展，Area
    /// ***************************************
    /// 
    /// ************  广义MVC及狭义MVC的区别  **************
    /// 广义MVC(Model--View-Controller)，
    /// V是界面  M是数据和逻辑  C是控制，把M和V链接起来
    /// 程序设计模式，一种设计理念，可以有效的分离界面和业务
    /// 
    /// 狭义MVC，是web开发框架， 
    /// V--Views   用户看到的视图内容
    /// C---Controllers  决定用户使用哪个视图，还能调用逻辑计算 
    /// 方法Action
    /// M--Models  数据传递模型，普通的实体
    /// ****************************************************
    /// 
    /// </summary>
    public class FirstController : Controller
    {
        // GET: First
        public ActionResult Index()
        {
            return View();
        }
    }
}