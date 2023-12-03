using Raylib_cs;
using System.Numerics;

namespace CustomProgram
{
    public class Inventory
    {
        private List<IBuyable> buyableItems; // Feed
        private List<ISellable> sellableItems; // Produce

        public Inventory()
        {
            buyableItems = new List<IBuyable>();
            sellableItems = new List<ISellable>();
        }

        public void AddBuyableItem(IBuyable item)
        {
            // Add item to the list of buyable items
            buyableItems.Add(item);
        }

        public void AddSellableItem(ISellable item)
        {
            // Add item to the list of sellable items
            sellableItems.Add(item);
        }

        public void RemoveBuyableItem(IBuyable item)
        {
            // Add item to the list of buyable items
            buyableItems.Remove(item);
        }

        public void RemoveSellableItem(ISellable item)
        {
            // Add item to the list of sellable items
            sellableItems.Remove(item);
        }

        public int CheckBuyableQuantity(IBuyable item)
        {
            // Return the quantity of the item in the inventory
            return buyableItems.Count; 
        }
        public int CheckSellableQuantity(ISellable item)
        {
            // Return the quantity of the specific type of item in the inventory
            return sellableItems.Count(i => i.name == item.name);
        }

        public void RemoveSellableItem(ISellable item, int quantity)
        {
            // Remove the specified quantity of the specific type of item from the inventory
            for (int q = 0; q < quantity; q++)
            {
                var itemToRemove = sellableItems.FirstOrDefault(i => i.name == item.name);
                if (itemToRemove != null)
                {
                    sellableItems.Remove(itemToRemove);
                }
            }
        }
        public List<ISellable> SellableItems
        {
            get{return sellableItems;}
        }
        public List<IBuyable> BuyableItems
        {
            get{return buyableItems;}
        }  
    }
}
