using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cyf.MVC5.Controllers
{
    /// <summary>
    /// ************  Pipe HttpModule的内容  **************
    /// 1 Http请求处理流程(管道模型详解)
    /// 2 HttpApplication的事件
    /// 3 HttpModule
    /// 4 Global事件
    /// 
    /// 
    /// ************  HttpModule注册  **************
    ///  对HttpApplication里面的事件进行动作注册的，就叫IHttpModule
    ///  ->自定义一个HttpModule
    ///  ->Web.config[system.webServer][modules]节点配置文件注册
    ///  ->然后【任何】一个请求都会执行Init里面注册给Application事件的动作
    ///  ->正常流程下，会按顺序执行19个事件
    ///  
    /// 
    /// ************  HttpModule发布事件，在Global增加动作  **************
    ///  HttpModule里面发布一个事件ModuleHandler,在Global增加一个动作，
    ///  CustomHttpModuleCyf_ModuleHandler(配置文件module名称_module里面事件名称)
    ///  请求响应时，该事件会执行
    ///  
    ///  
    ///  - HttpModule是对HttpApplication的事件注册动作
    ///  - Global是对httpmodule里面的事件注册动作
    /// </summary>
    public class PipeController : Controller
    {
        // GET: Pipe
        public ActionResult Index()
        {
            return View();
        }
    }
}