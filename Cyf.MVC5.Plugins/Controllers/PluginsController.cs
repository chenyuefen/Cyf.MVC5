using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cyf.MVC5.Plugins.Controllers
{
    /// <summary>
    /// ************  Plugins插件式编程  **************
    /// Plugins 控制器：
    /// ->创建新的MVC5项目
    /// ->写好控制器以后
    /// ->在主的项目里边添加引用该项目
    /// ->然后把Viws下的视图文件都拷贝过去即可
    /// </summary>
    public class PluginsController : Controller
    {
        // GET: Plugins
        public ActionResult Index()
        {
            return View();
        }
    }
}