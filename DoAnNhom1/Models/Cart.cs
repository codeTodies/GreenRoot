using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using DoAnNhom1.Models;
namespace DoAnNhom1.Models
{


    public class CartItem
    {
        public Tree _tree { get; set; }

        public int _quantity { get; set; }
    }
    public class Cart
    {
        List<CartItem> items = new List<CartItem>();
        public IEnumerable<CartItem> Items
        {
            get { return items; }
        }
        public void Add_Product_Cart(Tree _tr, int _quan = 1)
        {
            var item = Items.FirstOrDefault(S => S._tree.TreeID == _tr.TreeID);
            if (item == null)
            {
                items.Add(new CartItem
                {
                    _tree = _tr,
                    _quantity = _quan
                });
            }
            else
            {
                item._quantity += _quan;
            }
        }
        public int Total_quantity()
        {
            return items.Sum(s => s._quantity);
        }
        public decimal Total_money()
        {
            var total = items.Sum(s => s._quantity * s._tree.Price);
            return (decimal)total;
        }
        public void Update_quantity(int id, int _new_quan)
        {
            var item = items.Find(s => s._tree.TreeID == id);
            if (item != null)
                item._quantity = _new_quan;
        }
        public void Remove_CartItem(int id)
        {
            items.RemoveAll(s =>s._tree.TreeID == id);
        }
        public void ClearCart()
        {
            items.Clear();
        }
    }
}