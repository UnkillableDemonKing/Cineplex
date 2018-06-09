using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleLogin.Models
{
    public class Booking
    {
        public int MovieId { get; set; }

        public int CineplexId { get; set; }

        public int SessionId { get; set; }

        public string SeatBooking { get; set; }

        public int Cost { get; set; }

    }
}
