using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PIS_System.Models
{
    public class ProductModel
    {
        public string ID { get; set; }
        public string Category { get; set; }
        public string Name { get; set; }
        public decimal UnitPrice { get; set; }
    }
}