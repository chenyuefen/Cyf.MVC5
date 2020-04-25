using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cyf.MVC5.Plugins.Areas.PluginAreas.Controllers
{
    /// <summary>
    /// ************  Plugins插件式编程  **************
    /// Area 区域：
    /// ->创建新的MVC5项目
    /// ->写好控制器以后
    /// ->在主的项目里边添加引用该项目
    /// ->然后把Views下的视图文件都拷贝过去即可【注意只拷贝Views】
    /// </summary>
    public class HomeController : Controller
    {
        // GET: PluginAreas/Home
        public ActionResult Index()
        {
            return View();
        }
    }
}