﻿using System;
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
    /// 
    /// ************  扩展自定义路由Route  **************
    /// 扩展自己的route，写入routecollection，可以自定义规则完成路由
    /// 扩展httphandle，就可以为所欲为，跳出MVC框架
    /// 
    /// 
    /// ************  扩展View引擎：不同浏览器访问不同页面/PC,手机端不同页面/中英文网站  **************
    /// FindView---变成了Engine的CreateView---使用构造里面的路径模型
    /// 能做到的就是在不同的浏览器，用的是一套控制器，但是可以有不同的View
    /// 中英文网站   手机/PC
    /// 
    /// 解决方案：
    /// a 覆写的是FindView而不是CreateView(迟了),而且一定得set回去
    /// b CreateView时直接修改path(狠人)
    /// 注意不同的路径如_ViewStart
    /// 
    /// 
    /// ************  Asp.Net六大内置对象：Response，Request，Application，Server，Session，Cookie  **************
    /// Asp.Net有个大佬，HttpContext:http请求的上下文，任何一个环节其实都是需要httpcontext，需要的参数信息，处理的中间结果，最终的结果，都是放在httpcontext，是一个贯穿始终的对象
    /// 所谓6大对象，其实就是HttpContext的属性
    /// Request：url参数 form参数 url地址 urlreferer content-encoding，就是http请求提供的各种信息,后台里面都是可以拿到的context.Request.Headers["User-Agent"]；包括自定义的--BasicAuth; 请求信息的解读是asp.net_isapi按照http协议解析出来的
    /// Response：响应， Response.Write   各种result扩展就是序列化+response，验证码；都是给客户端响应内容，除了body(json/html/file)，其实还有很多东西，各种header，反正都是交给浏览器去用的
    /// Application：全局的东西，多个用户共享，  统计一下网站的请求数
    /// Server：也就是个帮助类库
    /// Session:用户登录验证，登录时写入，验证时获取；验证码；跳转当前页；一个用户一个Session，字典式
    /// Cookie:用户登录验证，登录时写入，验证时获取；ValidateAntiForgeryToken；保存用户数据(记住账号；购物车；访问过那几个页面；)；一个用户一个Cookie,字典式；
    /// 
    /// 协议：就是一个约定，保证多方的信息传输(中文也是一种约定)
    /// Http协议：超文本传输协议，也就是个文本传输的规范
    ///           浏览器/客户端遵循；服务端也遵循，那么就可以发起交互了
    /// 文本有啥：
    /// 
    /// cookie: 存在客户端--不能有敏感信息-推荐加密
    ///         每次请求都提交--不能太大--jd多个域名
    ///         可以存浏览器内存没有指定expiretime--关闭浏览器就丢失了
    ///         想存在硬盘就指定expiretime---想清空就修改有效期--不会丢失
    ///         同一个浏览器登陆覆盖的问题---一个浏览器cookie只有一个地方存储
    ///         不同的浏览器登陆就不会覆盖----无痕模式是不会覆盖的
    ///         
    /// session：服务器内存(sessionstateserver/SQLServer)---体积不要太大，可以存敏感信息--重启丢失    
    ///          一个用户一条，经常做传值(tempdata)
    ///          负载均衡下session怎么处理？会话粘滞/session共享
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
        public ActionResult Refuse()
        {
            return View();
        }
    }
}