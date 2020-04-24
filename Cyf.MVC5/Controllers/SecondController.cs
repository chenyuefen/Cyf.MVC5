using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cyf.MVC5.Controllers
{
    public class SecondController : Controller
    {
        // GET: Second
        public ActionResult Index()
        {
            return View();
        }

        public string String()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff");
        }

        public string Time(int year, int month, int day)
        {
            return $"当前传入日期：{year}-{month}-{day}";
        }
    }
}