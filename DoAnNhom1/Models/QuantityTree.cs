using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DoAnNhom1.Models
{
    public class QuantityTree
    {
        public string NameTree { get; set; }
        public int Price { get; set; }
        public string ImageTree { get; set; }
        public string NameArea { get; set; }
        [System.ComponentModel.DataAnnotations.Key]
        public int? TreeID { get; set; }
        public decimal Total_money { get; set; }
        public Tree tree { get; set; }
        public Area area { get; set; }
        public OrderDetail order { get; set; }
        public IEnumerable<Tree> ListTree { get; set; }
        public int? Sum_Quantity { get; set; }
    }
}