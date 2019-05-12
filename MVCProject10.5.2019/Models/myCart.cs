using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCProject10._5._2019.Models
{
    public class myCart
    {
        public int quantity { get; set; }
        public product prod { get; set; }

        public myCart(product p , int q)
        {
            this.prod = p;
            this.quantity = q;
        }
    }
}