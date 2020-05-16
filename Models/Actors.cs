using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TvShowsApp.Models
{
    public class Actors
    {[Key]// vo slucaj da ti kazuva eror deka nemas primary key !
        public int ActorId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
       // [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMMM yyyy}"), ]
       [DataType(DataType.Date)]
        public DateTime Born { get; set; }
        [Required]
        [DataType(DataType.Url)]
        [Display(Name = "Imdb Link")]
        public string ImdbUrl { get; set; }
        [Required]
        [DataType(DataType.ImageUrl)]
        [Display(Name = "Poster")]
        public string ImageUrl { get; set; }

        public int TvShowId { get; set; }
        public virtual TvShow TvShow { get; set; } //Foreign Key
    }
}
