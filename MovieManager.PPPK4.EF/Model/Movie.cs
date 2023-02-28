using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MovieManager.PPPK4.EntityFramework.Model
{
    public class Movie
    {
        public Movie()
        {
            this.Assets = new HashSet<Asset>();
            this.Actors = new HashSet<Person>();
        }

        public int Id { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Description length cannot exceed 50 chars")]
        public string Name { get; set; }
        [Required]
        [MaxLength(250, ErrorMessage = "Description length cannot exceed 250 chars")]
        public string Description { get; set; }

        [Required]
        public DateTime PublishedDate { get; set; }
        public virtual ICollection<Asset> Assets { get; set; }
        public virtual Person Director { get; set; }

        [Required(ErrorMessage = "You must pick a director")]
        public int DirectorId { get; set; }
        public ICollection<Person> Actors { get; set; }
    }
}