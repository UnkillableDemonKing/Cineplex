using System;
using System.Collections.Generic;

namespace GoogleLogin.Models
{
    public partial class SessionMovie
    {
        public SessionMovie()
        {
            CineplexMovie = new HashSet<CineplexMovie>();
        }

        public int SessionId { get; set; }
        public DateTime? Movietime { get; set; }

        public virtual ICollection<CineplexMovie> CineplexMovie { get; set; }
    }
}
