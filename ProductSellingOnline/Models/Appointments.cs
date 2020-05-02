using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductSellingOnline.Models
{
    public class Appointments
    {
        public int Id { get; set; }
        public DateTime AppointmentDate { get; set; }

        [NotMapped]
        public DateTime AppointmentTime { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhoneNumber { get; set; }
        public string CustomerEmail { get; set; }
        public bool IsConfirmed { get; set; }
    }
}