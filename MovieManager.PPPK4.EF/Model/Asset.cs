using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MovieManager.PPPK4.EntityFramework.Model
{
    public class Asset
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public byte[] Content { get; set; }
        [Required]
        public string ContentType { get; set; }

        public virtual ICollection<Movie> Movies { get; set; }
        public virtual ICollection<Person> People { get; set; }
    }
}