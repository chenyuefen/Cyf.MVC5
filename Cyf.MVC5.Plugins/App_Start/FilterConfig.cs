using System.Web;
using System.Web.Mvc;

namespace Cyf.MVC5.Plugins
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
