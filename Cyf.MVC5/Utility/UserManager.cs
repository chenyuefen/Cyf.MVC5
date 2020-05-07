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
                        CurrentUser currentUser = new CurrentUser()
                        {
                            Id = user.id,
                            Name = user.name,
                            Account = user.account,
                            Email = user.email,
                            Password = user.password,
                            LoginTime = DateTime.Now
                        };

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
                myCookie.Expires = DateTime.Now.AddMinutes(-1);//设置过过期
                context.Response.Cookies.Add(myCookie);
            }

            #endregion Cookie

            #region Session
            var sessionUser = context.Session["CurrentUser"];
            if (sessionUser != null && sessionUser is CurrentUser)
            {
                CurrentUser currentUser = (CurrentUser)context.Session["CurrentUser"];
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