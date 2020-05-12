using Cyf.EntityFramework.Interface;
using Cyf.EntityFramework.Model;
using Cyf.Framework.Encrypt;
using Cyf.Framework.Extension;
using Cyf.MVC5.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Unity;
namespace Cyf.MVC5.Utility
{
    /// <summary>
    /// UserManager在UI层，准备放入基础设施层Ruanmou.Framework
    /// Ruanmou.Framework 依赖Ruanmou.Bussiness.Interface
    /// 但是Ruanmou.Bussiness.Interface 是依赖Ruanmou.Framework
    /// 循环依赖...  
    /// 可以通过委托传递，UI引用了Interface,调用Framework里面的UserManager,
    /// 可以传递委托进去，该委托完成对数据库的查询，
    /// UserManager只需要执行委托，不需要依赖了
    /// </summary>
    public static class UserManager
    {
        private static Logger logger = new Logger(typeof(UserManager)); //Logger.CreateLogger(typeof(UserManager));
        /// <summary>
        /// 用户登录验证
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <param name="verifyCode"></param>
        /// <returns></returns>
        public static LoginResult Login(this HttpContextBase context, string name, string password, string verifyCode)
        {
            if (context.Session["CheckCode"] != null
                && !string.IsNullOrWhiteSpace(context.Session["CheckCode"].ToString())
                && context.Session["CheckCode"].ToString().Equals(verifyCode, StringComparison.CurrentCultureIgnoreCase))
            {
                using (IAcountService servcie = DIFactory.GetContainer().Resolve<IAcountService>())
                {
                    Acount user = servcie.Set<Acount>().FirstOrDefault(u => u.name.Equals(name)|| u.account.Equals(name));//账号查找
                    if (user == null)
                    {
                        return LoginResult.NoUser;
                    }
                    //else if (!user.password.Equals(MD5Encrypt.Encrypt(password)))
                    else if (!user.password.Equals(password))
                    {
                        return LoginResult.WrongPwd;
                    }
                    //else if (user.State == 1)
                    //{
                    //    return LoginResult.Frozen;
                    //}
                    else
                    {
                        //登录成功  写cookie session
                        CurrentLoginUser currentUser = new CurrentLoginUser()
                        {
                            Id = user.id,
                            Name = user.name,
                            Account = user.account,
                            Email = user.email,
                            Password = user.password,
                            LoginTime = DateTime.Now
                        };

                        //都是asp.net解析的
                        #region Request
                        //context.Request.Headers["User-Agent"];
                        //context.Request["Refer"];
                        //context.Request
                        //Request 获取请求个各种参数，
                        //Header里面的各种信息
                        //InputStream上传的文件
                        #endregion

                        #region Response
                        //context.Response
                        //Response响应的 跨域、压缩、缓存、cookie、output + contentType
                        #endregion

                        #region Application
                        context.Application.Lock();//ASP.NET 应用程序内的多个会话和请求之间共享信息
                        context.Application.Lock();//操作之前加锁
                        context.Application.Add("try", "die");
                        context.Application.UnLock();
                        object aValue = context.Application.Get("try");
                        aValue = context.Application["try"];
                        context.Application.Remove("命名对象");
                        context.Application.RemoveAt(0);
                        context.Application.RemoveAll();
                        context.Application.Clear();
                        #endregion

                        #region Items

                        context.Items["123"] = "123";//单一会话，不同环境都可以用,比如在httpmodule获取到的信息，想传递给action；随着context释放
                        #endregion

                        #region Server
                        //辅助类 Server
                        string encode = context.Server.HtmlEncode("<我爱我家>");
                        string decode = context.Server.HtmlDecode(encode);

                        string physicalPath = context.Server.MapPath("/Home/Index");//只能做物理文件的映射
                        string encodeUrl = context.Server.UrlEncode("<我爱我家>");
                        string decodeUrl = context.Server.UrlDecode(encodeUrl);
                        #endregion

                        #region Cookie
                        //context.Request.Cookies

                        //HttpCookie cookie = context.Request.Cookies.Get("CurrentUser");
                        //if (cookie == null)
                        //{
                        HttpCookie myCookie = new HttpCookie("CurrentUser");
                        myCookie.Value = JsonHelper.ObjectToString<CurrentLoginUser>(currentUser);
                        myCookie.Expires = DateTime.Now.AddMinutes(5);//保存到硬盘

                        //5分钟后  硬盘cookie
                        //不设置就是内存cookie--关闭浏览器就丢失
                        //改成过期 -1 过期
                        //修改cookie：不能修改，只能起个同名的cookie

                        //myCookie.Domain//设置cookie共享域名
                        //myCookie.Path//指定路径能享有cookie
                        context.Response.Cookies.Add(myCookie);//一定要输出
                        //}
                        //前端只能获取name-value
                        #endregion Cookie

                        #region Session
                        //context.Session.RemoveAll();
                        var sessionUser = context.Session["CurrentUser"];
                        context.Session["CurrentUser"] = currentUser;
                        context.Session.Timeout = 3;//minute  session过期等于Abandon
                        #endregion Session

                        logger.Debug(string.Format("用户id={0} Name={1}登录系统", currentUser.Id, currentUser.Name));
                        return LoginResult.Success;
                    }
                }
            }
            else
            {
                return LoginResult.WrongVerify;
            }


        }


        /// <summary>
        /// 用户注销
        /// </summary>
        /// <param name="context"></param>
        public static void UserLogout(this HttpContextBase context)
        {
            #region Cookie
            
            HttpCookie myCookie = context.Request.Cookies["CurrentUser"];
            if (myCookie != null)
            {
                myCookie.Expires = DateTime.Now.AddMinutes(-1);//退出时设置Cookie过过期
                context.Response.Cookies.Add(myCookie);
            }

            #endregion Cookie

            #region Session
            var sessionUser = context.Session["CurrentUser"];
            if (sessionUser != null && sessionUser is CurrentLoginUser)
            {
                CurrentLoginUser currentUser = (CurrentLoginUser)context.Session["CurrentUser"];
                logger.Debug(string.Format("用户id={0} Name={1}退出系统", currentUser.Id, currentUser.Name));
            }
            context.Session["CurrentUser"] = null;//表示将制定的键的值清空，并释放掉，
            context.Session.Remove("CurrentUser");
            context.Session.Clear();//表示将会话中所有的session的键值都清空，但是session还是依然存在，
            context.Session.RemoveAll();//
            context.Session.Abandon();//就是把当前Session对象删除了，下一次就是新的Session了   
            #endregion Session
        }
        public enum LoginResult
        {
            /// <summary>
            /// 登录成功
            /// </summary>
            [RemarkAttribute("登录成功")]
            Success = 0,
            /// <summary>
            /// 用户不存在
            /// </summary>
            [RemarkAttribute("用户不存在")]
            NoUser = 1,
            /// <summary>
            /// 密码错误
            /// </summary>
            [RemarkAttribute("密码错误")]
            WrongPwd = 2,
            /// <summary>
            /// 验证码错误
            /// </summary>
            [RemarkAttribute("验证码错误")]
            WrongVerify = 3,
            /// <summary>
            /// 账号被冻结
            /// </summary>
            [RemarkAttribute("账号被冻结")]
            Frozen = 4
        }
    }
}