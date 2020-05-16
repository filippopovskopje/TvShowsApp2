using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TvShowsApp.Models
{
    public class Customers
    {
        public Customers()
        {
            this.RentedMovie = new HashSet<RentedMovie>();
        }
        [Key]
        public int CustomersId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public int Phone { get; set; }
        [Required]
        public string E_mail { get; set; }
        public virtual ICollection<RentedMovie> RentedMovie { get; set; }

    }
}
