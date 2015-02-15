using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBlog.Core.Entities
{   
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Descritption { get; set; }
        
        public Category ParentCategory { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
    }
}
