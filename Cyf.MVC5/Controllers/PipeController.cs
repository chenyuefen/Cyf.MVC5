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
    /// ************  管道处理模型的定义  **************
    /// 所谓管道处理模型，其实就是后台如何处理一个Http请求
    /// 定义多个事件完成处理步骤，每个事件可以扩展动作(httpmodule)，
    /// 最后有个httphandler完成请求的处理，这个过程就是管道处理模型
    /// 还有一个全局的上下文环境httpcontext，无论参数，中间结果 最终结果，都保存在其中
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
    ///  
    /// 
    /// ************  Pipe HttpHandler : 主要针对链接后缀作处理  **************
    /// 1 HttpHandler及扩展，自定义后缀，图片防盗链等
    /// 2 RoutingModule,IRouteHandler、IHttpHandler
    /// 3 MVC扩展Route，扩展HttpHandle
    /// 
    /// 配置文件指定映射关系：后缀名与处理程序的关系(IHttpHandler---IHttpHandlerFactory)
    /// Http任何一个请求一定是由某一个具体的Handler来处理的
    /// 
    /// 
    /// ************  HttpHandler应用及实现  **************
    /// 自定义handler处理，就是可以处理各种后缀请求，可以加入自己的逻辑
    /// -> 自定义一个HttpModule
    /// -> Web.Config[system.webServer][handlers]节点配置文件注册
    /// -> 然后所写的自定义后缀名的链接都会跳转至所写的handler处理
    /// -> 注意配好路由避免操作被拦截
    /// 
    /// 
    /// ************  MVC框架的管道模型扩展流程  **************
    /// 网站启动时---对RouteCollection进行配置
    /// 把正则规则和RouteHandler（提供httphandler）绑定，放入RouteCollection
    /// 
    /// 请求来临时---用RouteCollection进行匹配
    /// MVC框架其实就是在Asp.Net管道上扩展的，在PostResolveCache事件扩展了UrlRoutingModule，
    /// 会在任何请求进来后，先进行路由匹配，如果匹配上了，就指定httphandler；没有匹配就还是走原始流程
    /// 
    /// </summary>
    public class PipeController : Controller
    {
        // GET: Pipe
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Handler()
        {
            base.ViewBag.HttpHandler = base.HttpContext.CurrentHandler.GetType().FullName;
            //base.RouteData.Values //路由匹配后，获取的信息
            return View();
        }

    }
}