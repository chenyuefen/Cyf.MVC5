using System.Web.Mvc;

namespace Cyf.MVC5.Areas.System
{
    /// <summary>
    /// 
    /// 因为一个Web项目可以非常大非常复杂，多人合作开发，命名就成问题了，Area可以把项目拆分开，方便团队合作；演变到后面可以做成插件式开发：
    /// MvcApplication--Application_Start--AreaRegistration.RegisterAllAreas()---其实就是把SystemAreaRegistration给注册下---添加URL地址规则--请求来了就匹配(area在普通的之前)
    /// 
    /// </summary>
    public class SystemAreaRegistration : AreaRegistration 
    {
        public override string AreaName
        {
            get 
            {
                return "System";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "System_default",
                "System/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "Cyf.MVC5.Areas.System.Controllers" }
                //增加区域后需要指定命名空间
            );
        }
    }
}