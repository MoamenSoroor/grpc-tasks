using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Inventory
{
    public class User
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public double Balance { get; set; }

    }

    public class Order
    {

        public int Id { get; set; }

        public int UserId { get; set; }

        public List<InventoryItem> Items { get; } = new List<InventoryItem>();

        public double TotalPrice { get => Items.Sum(i => i.TotalPrice); }


    }


    public class InventoryItem
    {

        public int Id { get; set; }

        public string Name { get; set; }
        
        public int Quantity { get; set; }

        public double Price { get; set; }

        public double TotalPrice { get => Price * Quantity; }


    }



}
