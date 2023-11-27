namespace CustomProgram
{
    public class Shop
    {
        private Dictionary<Type, int> animalStock;
        public List<ISellable> produceAvailable { get; private set; }
        public List<IBuyable> itemsForSale { get; private set; } // Animals and Feed

        public Shop()
        {
            produceAvailable = new List<ISellable>();
            itemsForSale = new List<IBuyable>();

            // Initialize animal stock
            animalStock = new Dictionary<Type, int>
        {
            { typeof(Cow), 10 }, 
            { typeof(Chicken), 20 },
            { typeof(Sheep), 7 },
            { typeof(Goat), 8 },
            { typeof(Pig), 12 }
        };
        }

        public int GetStockForAnimal(Type animalType)
        {
            return animalStock.TryGetValue(animalType, out int stock) ? stock : 0;
        }

        public void SellItem(ISellable item, Player player, int quantity)
        {
            // Check if the player has enough of the item to sell
            if(player.Inventory.CheckSellableQuantity(item) > 0)
            {
                // Subtract from player's inventory
                player.Inventory.RemoveSellableItem(item);
                // Add the sell price to player's coins
                player.Coins += 10;
            }
        }

        public void BuyItem(IBuyable item, Player player, int quantity)
        {
            if (player.Coins >= item.purchasePrice)
            {
                player.Coins -= item.purchasePrice;

                if (item is Animal animal)
                {
                    int stock = GetStockForAnimal(animal.GetType());
                    if (stock > 0)
                    {
                        player.Coins -= item.purchasePrice;
                        foreach (var plot in player.Plots)
                        {
                            if (plot.AddAnimal(animal))
                            {
                                animalStock[animal.GetType()]--;
                                return;
                            }
                        }
                        // Handle case where no plot is available
                    }
                    // Handle case where stock is 0
                }
                else if (item is Feed feed)
                {
                    // Add feed to player's inventory
                    player.Inventory.AddBuyableItem(feed);
                    return;
                }
                // Handle other types of IBuyable if necessary
            }
        }
    }
}
