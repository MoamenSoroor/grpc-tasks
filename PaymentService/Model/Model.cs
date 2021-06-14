using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Payment
{
    public class User
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public double Balance { get; set; }

    }

    class Order
    {

        public int Id { get; set; }

        public int UserId { get; set; }

        public List<Item> Items { get; } = new List<Item>();

        public double TotalPrice { get => Items.Sum(i => i.TotalPrice); }


    }


    class Item
    {

        public int Id { get; set; }

        public int Quantity { get; set; }

        public double Price { get; set; }

        public double TotalPrice { get => Price * Quantity; }

    }



}
