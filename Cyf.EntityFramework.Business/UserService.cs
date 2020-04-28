using Cyf.EntityFramework.Interface;
using Cyf.EntityFramework.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyf.EntityFramework.Business
{
    public class UserService : BaseService, IUserService
    {
        public UserService(DbContext context) : base(context)
        {
        }

        public void UpdateLastLogin(Employee user)
        {
            Employee userDB = base.Find<Employee>(user.id);
            //userDB.LastLoginTime = DateTime.Now;
            this.Commit();
        }
    }

}
