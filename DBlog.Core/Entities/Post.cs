using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBlog.Core.Entities
{
    public class Post
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [MaxLength(20)]
        public string Author { get; set; }

        public Author CreatedBy { get; set; }

        public Author UpdatedBy { get; set; }

         
        public DateTime CreateDate { get; set; }

        [MaxLength]
        public string Content { get; set; }

        [Required]
        public bool HasCommentsEnabled { get; set; }

        [Required]
        [MaxLength(50)]
        public string Slug { get; set; }

        [Required]
        public bool IsTop { get; set; }

        [Required]
        public bool IsDelete { get; set; }

        [Required]
        public int Views { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<Category> Categories { get; set; }

        public virtual ICollection<Tag> Tags { get; set; }
    }
}
