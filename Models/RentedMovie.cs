using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TvShowsApp.Models;
using System.ComponentModel.DataAnnotations.Schema;


namespace TvShowsApp.Models
{
    public class RentedMovie
    {
        [Key]
        public int RentedMovieId { get; set; }

        public int CusId { get; set; }
        public int TvShowId { get; set; }

      
        public DateTime  RentalDate { get; set; }

        public DateTime? ReturnDate { get; set; }

        
        public virtual TvShow TvShow { get; set; }
        public  Customers Cus { get; set; } 
    }
}
