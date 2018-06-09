using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleLogin.Models
{
    public class ShoppingCart
    {
        
        public int Quantity { get; set; }

        public string[] Seats { get; set; }

        public int AmountDue { get; set; }
    }
}
