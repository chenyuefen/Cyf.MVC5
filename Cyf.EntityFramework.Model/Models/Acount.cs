namespace Cyf.EntityFramework.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("cyf_datas.acount")]
    public partial class Acount
    {
        public int id { get; set; }

        [StringLength(255)]
        public string name { get; set; }

        [StringLength(255)]
        public string account { get; set; }

        [StringLength(255)]
        public string password { get; set; }

        [StringLength(255)]
        public string email { get; set; }
    }
}
