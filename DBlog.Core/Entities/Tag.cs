using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBlog.Core.Entities
{
    public class Tag
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string TagName { get; set; }
        
        public int TagCount { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
    }
}
