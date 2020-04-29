using Cyf.EntityFramework.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyf.SearchEngines.LuceneService.Model
{
    public class User: Employee
    {
    }
    public static class UserExt
    {
        public static List<User> Trans(this List<Employee> datas)
        {
            return datas.Select(x => x.Trans()).ToList();
        }
        public static User Trans(this Employee data)
        {
            return new User
            {
                company_id = data.company_id,
                id = data.id,
                name = data.name,
                position = data.position
            };
        }

    }
}
