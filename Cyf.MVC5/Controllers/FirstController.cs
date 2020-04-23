using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Cyf.MVC5.Controllers
{
    /// <summary>
    /// 
    /// ************  First内容  **************
    /// 1 MVC5：Controller、Action、View
    /// 2 IIS部署，Global、Log4
    /// 3 数据传值的多种方式
    /// 4 Route使用和扩展，Area
    /// 
    /// ************  IIS两种部署方式  **************
    /// 部署IIS
    /// 1、右键发布部署
    /// 2、直接项目文件部署
    /// 程序池--CLR4.0---集成模式
    /// 
    /// ************  广义MVC及狭义MVC的区别  **************
    /// 【广义MVC】(Model--View-Controller)
    ///  V是界面  M是数据和逻辑  C是控制，把M和V链接起来
    ///  程序设计模式，一种设计理念，可以有效的分离界面和业务
    /// 
    /// 【狭义MVC】是web开发框架
    ///  V--Views   用户看到的视图内容
    ///  C---Controllers  决定用户使用哪个视图，还能调用逻辑计算 
    ///  方法Action
    ///  M--Models  数据传递模型，普通的实体
    /// 
    /// </summary>
    public class FirstController : Controller
    {
        // GET: First
        /// <summary>
        /// 在此右键添加视图，会在View下自动生成视图文件
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// /First/IndexId/4 id是路由解析出来的，只有id参数可以这样
        /// /First/IndexId?id=3 url地址传递参数
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult IndexId(int id)
        {
            //各种的数据库增删改查
            return View();
        }



        public ViewResult IndexIdNull(int? id)
        {
            return View();
        }

        /// <summary>
        /// string可以为空
        /// https://localhost:44337/First/stringname?name=小白
        /// </summary>
        public string StringName(string name)
        {
            return $"This is {name}";
        }

        /// <summary>
        /// 返回Json格式
        /// </summary>
        public string StringJson(string name)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                Id = 123,
                Name = name
            });
        }

        /// <summary>
        /// 返回Json格式
        /// </summary>
        public JsonResult Json(int id, string name, string remark)
        {
            return new JsonResult()
            {
                Data = new
                {
                    Id = id,
                    Name = name ?? "X",
                    Remark = remark ?? "这里是默认的"
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet//否则会被浏览器拦截
            };
        }
    }
}