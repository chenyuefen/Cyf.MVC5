using Cyf.EntityFramework.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyf.EntityFramework.Business
{
    public class CompanyService : BaseService, ICompanyService
    {
        public CompanyService(DbContext context) : base(context)
        {
        }
    }
}
