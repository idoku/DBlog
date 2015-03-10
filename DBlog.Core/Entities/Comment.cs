using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBlog.Core.Entities
{
    public class Comment
    {
        
        [Key]
        public int Id { get; set; }

        [MaxLength(30)]
        public string Author { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [MaxLength(1000)]
        public string Content { get; set; }
       
        public DateTime CommentDate { get; set; }
        
        public bool IsAudit { get; set; }
        
        public bool IsDeleted { get; set; }

         public int PostId { get; set; }

        [ForeignKey("PostId")]
        public Post  Post { get; set; }

        public int? ParentId { get; set; }

        [ForeignKey("ParentId")]
        public Comment Parent { get; set; }
        

        public virtual ICollection<Comment> Children { get; set; }
    }
}
