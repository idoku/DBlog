using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace DBlog.Core.Entities
{

    public class BlogInfo
    {
        [Key]
        public int Id { get; set; }

        [StringLength(200)]
        public string Board { get; set; }

        [EmailAddress]
        [StringLength(200)]
        public string Email { get; set; }

        [StringLength(200)]
        public string Copyright { get; set; }
    }
}
