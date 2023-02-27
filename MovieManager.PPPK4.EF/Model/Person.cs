using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MovieManager.PPPK4.EntityFramework.Model
{
    public class Person
    {
        public Person()
        {
            this.Assets = new HashSet<Asset>();
            this.DirectedMovies = new HashSet<Movie>(); 
            this.ActedMovies = new HashSet<Movie>(); 
        }
        public int Id { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "First name cannot exceed 50 chars")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Last name cannot exceed 50 chars")]
        public string LastName { get; set; }
        public string FullName { get => FirstName + " " + LastName;  }
        public virtual ICollection<Asset> Assets { get; set; }
        public ICollection<Movie> DirectedMovies { get; set; }
        public ICollection<Movie> ActedMovies { get; set; }
    }
}