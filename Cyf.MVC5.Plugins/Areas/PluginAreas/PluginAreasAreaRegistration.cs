using System.Web.Mvc;

namespace Cyf.MVC5.Plugins.Areas.PluginAreas
{
    public class PluginAreasAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "PluginAreas";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "PluginAreas_default",
                "PluginAreas/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}