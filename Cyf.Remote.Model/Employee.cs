using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyf.Remote.Model
{
    public partial class Employee
    {
        public int Id { get; set; }

        public int? Company_id { get; set; }

        public string Name { get; set; }

        public string Position { get; set; }
    }
}
