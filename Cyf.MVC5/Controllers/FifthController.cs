using Cyf.Framework.Extension;
using Cyf.Framework.ImageHelper;
using Cyf.MVC5.Utility;
using Cyf.MVC5.Utility.Filter;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cyf.MVC5.Controllers
{
    /// <summary>
    /// ************  Fifth内容  **************
    /// 1 用户登录/退出功能实现
    /// 2 AuthorizeAttribute权限验证
    /// 3 Filter注册和匿名支持
    /// 4 解读Filter生效机制
    /// 
    /// 
    /// ************  页面访问权限（自定义CustomAuthorizeFilter）  **************
    /// 登陆后有权限控制，有的页面的是需要用户登录后才能访问的
    /// 需要在访问页面时增加登陆验证
    /// 也不能每个action都来一遍
    /// 
    /// 自定义CustomAuthorizeFilter，
    /// 1 方法注册----单个方法生效--------------在方法上加特性CustomAuthorize
    /// 2 控制器注册--控制器全部方法生效--------在控制器上加特性CustomAuthorize
    /// 3 全局注册----全部控制器的全部方法生效--在FilterConfig里加CustomAuthorizeAttribute
    /// 
    /// 
    /// ************  AllowAnonymous跳过权限验证  **************
    /// AllowAnonymous匿名，单独加特性是没用的
    /// 其实需要验证时支持，甚至说可以自定义一些特性一样可以生效
    /// 
    /// </summary>
    [CustomAuthorize("~/Home/Index")]
    public class FifthController : Controller
    {
        // GET: Fifth
        public ActionResult Index()
        {
            return View();
        }

        [CustomAuthorize("~/Home/Index")]
        [HttpGet]//响应get请求
        //[AllowAnonymous]
        [CustomAllowAnonymous]
        public ViewResult Login()
        {
            return View();
        }

        [HttpPost]
        //[AllowAnonymous]
        [CustomAllowAnonymous]
        public ActionResult Login(string name, string password, string verify)
        {
            string formName = base.HttpContext.Request.Form["Name"];

            var result = base.HttpContext.Login(name, password, verify);
            if (result == UserManager.LoginResult.Success)
            {
                return base.Redirect("/Home/Index");
            }
            else
            {
                ModelState.AddModelError("failed", result.GetRemark());
                return View();
            }
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Logout()
        {
            this.HttpContext.UserLogout();
            return RedirectToAction("Index", "Home"); ;
        }
        #region 生成验证码

        /// <summary>
        /// 验证码 FileContentResult
        /// </summary>
        /// <returns></returns>
        public ActionResult VerifyCode()
        {
            string code = "";
            Bitmap bitmap = VerifyCodeHelper.CreateVerifyCode(out code);
            base.HttpContext.Session["CheckCode"] = code;//Session识别用户，为单一用户有效
            MemoryStream stream = new MemoryStream();
            bitmap.Save(stream, ImageFormat.Gif);
            return File(stream.ToArray(), "image/gif");//返回FileContentResult图片
        }

        /// <summary>
        /// 验证码  直接写入Response
        /// </summary>
        public void Verify()
        {
            string code = "";
            Bitmap bitmap = VerifyCodeHelper.CreateVerifyCode(out code);
            base.HttpContext.Session["CheckCode"] = code;
            bitmap.Save(base.Response.OutputStream, ImageFormat.Gif);
            base.Response.ContentType = "image/gif";
        }

        #endregion

    }
}