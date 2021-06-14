using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gRPCAPI
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

        public List<Item> Items { get; set; }

        public double TotalPrice { get => Items?.Sum(i => i.TotalPrice) ?? 0; }


    }


    public class Item
    {

        public int Id { get; set; }

        public int Quantity { get; set; }

        public double Price { get; set; }

        public double TotalPrice { get => Price * Quantity; }

    }



}
