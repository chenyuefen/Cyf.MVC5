using Cyf.Framework.Extension;
using Cyf.Framework.ImageHelper;
using Cyf.MVC5.Utility;
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
    /// </summary>
    public class FifthController : Controller
    {
        // GET: Fifth
        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]//响应get请求
        [AllowAnonymous]
        public ViewResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
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