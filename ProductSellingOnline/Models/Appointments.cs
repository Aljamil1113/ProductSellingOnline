using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductSellingOnline.Models
{
    public class Appointments
    {
        public int Id { get; set; }

        [Required]
        public DateTime AppointmentDate { get; set; }

        [NotMapped]
        [Required]
        public DateTime AppointmentTime { get; set; }

        [Required]
        public string CustomerName { get; set; }

        [Required]
        public string CustomerPhoneNumber { get; set; }

        [Required]
        public string CustomerEmail { get; set; }
        public bool IsConfirmed { get; set; }

        [Display(Name = "Sales Person")]
        public string SalesPersonId { get; set; }

        [ForeignKey("SalesPersonId")]
        public virtual ApplicationUser applicationUser { get; set; }
    }
}