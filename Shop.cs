using System;
using System.Collections.Generic;

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
            InitializeAnimalStock();
        }

        private void InitializeAnimalStock()
        {
            animalStock = new Dictionary<Type, int>
            {
                { typeof(Cow), 10 },
                { typeof(Chicken), 15 },
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
            if (player.Inventory.CheckSellableQuantity(item) >= quantity)
            {
                // Subtract from player's inventory
                player.Inventory.RemoveSellableItem(item, quantity);
                // Add the sell price to player's coins
                player.Coins += item.sellPrice * quantity;
            }
        }

        public void BuyItem(IBuyable item, Player player, int quantity)
        {
            if (player.Coins >= item.purchasePrice)
            {
                player.Coins -= item.purchasePrice;

                if (item is Animal animal)
                {
                    HandleAnimalPurchase(animal, player);
                }
                else if (item is Feed)
                {
                    // Unlimited feed stock, no need to check or update stock
                    player.Inventory.AddBuyableItem(item);
                }
                // Other types of IBuyable items can be handled here if necessary
            }
        }

        private void HandleAnimalPurchase(Animal animal, Player player)
        {
            int stock = GetStockForAnimal(animal.GetType());
            if (stock > 0)
            {
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

        public void PopulateWithStartingStock(ITimeProvider timeProvider)
        {
            PopulateAnimalsWithStartingStock(timeProvider);
        }

        private void PopulateAnimalsWithStartingStock(ITimeProvider timeProvider)
        {
            foreach (var animalType in animalStock.Keys)
            {
                int stockCount = animalStock[animalType];
                for (int i = 0; i < stockCount; i++)
                {
                    IBuyable animal = CreateAnimal(animalType, timeProvider);
                    itemsForSale.Add(animal);
                }
            }
        }

        private Dictionary<Type, int> animalCounts = new Dictionary<Type, int>();

        private IBuyable CreateAnimal(Type animalType, ITimeProvider timeProvider)
        {
            // Initialize the count for the animal type if it doesn't exist
            if (!animalCounts.ContainsKey(animalType))
            {
                animalCounts[animalType] = 0;
            }

            // Retrieve the current count for the animal type
            int count = animalCounts[animalType];

            // Increment the count for the next time an animal of this type is created
            animalCounts[animalType] = count + 1;

            // Create a unique name for the animal using the count
            string uniqueName = $"{animalType.Name} #{count}";

            // Now create the animal instance with the unique name
            IBuyable animal;
            switch (animalType.Name)
            {
                case nameof(Cow):
                    animal = new Cow(uniqueName, 150.0f, "assets/Cow.png", timeProvider);
                    break;
                case nameof(Sheep):
                    animal = new Sheep(uniqueName, 120.0f, "assets/Sheep.png", timeProvider);
                    break;
                case nameof(Chicken):
                    animal = new Chicken(uniqueName, 80.0f, "assets/Chicken.png", timeProvider);
                    break;
                case nameof(Pig):
                    animal = new Pig(uniqueName, 130.0f, "assets/Pig.png", timeProvider);
                    break;
                case nameof(Goat):
                    animal = new Goat(uniqueName, 110.0f, "assets/Goat.png", timeProvider);
                    break;
                default:
                    throw new ArgumentException("Unsupported animal type", nameof(animalType));
            }

            return animal;
        }

        public IBuyable CreateFeed(FeedType feedType)
        {
            string name = $"{feedType} Feed";
            float price = 50.0f; // Example price, can vary based on feed type
            return new Feed(name, price, feedType);
        }
    }
}
