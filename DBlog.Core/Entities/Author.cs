using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBlog.Core.Entities
{
    public class Author 
    {
        [Key]
        public int Id { get; set; }

        public string PenName { get; set; }

        [InverseProperty("CreatedBy")]
        public List<Post> PostWritten { get; set; }

        [InverseProperty("UpdatedBy")]
        public List<Post> PostUpdated { get; set; }
    }
}
