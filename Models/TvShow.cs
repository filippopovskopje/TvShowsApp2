﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TvShowsApp.Models
{
    public class TvShow
    {

        public TvShow()
        {
            this.Actors = new HashSet<Actors>();
            this.RentedMovie = new HashSet<RentedMovie>();
        }

        [Key]
        public int TvId { get; set; }
        [Required]
        [StringLength(60, MinimumLength =3)]
        public string Title { get; set; }
        [Required]
        public Genre Genre { get; set; }
        [Required]
        [DisplayFormat(DataFormatString ="{0:0.0#}")]
        public decimal Rating { get; set; }
        [Required]
        [DataType(DataType.Url)]
        [Display(Name = "Imdb Link")]
        public string ImdbUrl { get; set; }

        [Required]
        [DataType(DataType.ImageUrl)]
        [Display(Name = "Poster")]
        public string ImageUrl { get; set; }
        
        public Boolean Available { get; set; }
       
        public virtual ICollection<Actors> Actors { get; set; } //Foreign Key

        public virtual ICollection<RentedMovie> RentedMovie { get; set; }
    }
}
