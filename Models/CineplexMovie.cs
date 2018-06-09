using System;
using System.Collections.Generic;

namespace GoogleLogin.Models
{
    public partial class CineplexMovie
    {
        public int CineplexId { get; set; }
        public int MovieId { get; set; }
        public int SessionId { get; set; }

        public virtual Cineplex Cineplex { get; set; }
        public virtual Movie Movie { get; set; }
        public virtual SessionMovie Session { get; set; }
    }
}
