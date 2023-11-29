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

        public void PopulateWithStartingStock(ITimeProvider timeProvider)
        {
            PopulateAnimalsWithStartingStock(timeProvider);
            PopulateFeedsWithStartingStock();
        }

        public void PopulateAnimalsWithStartingStock(ITimeProvider timeProvider)
        {
            foreach (var animalType in animalStock.Keys)
            {
                int stockCount = animalStock[animalType];
                for (int i = 0; i < stockCount; i++)
                {
                    IBuyable item = CreateAnimal(animalType, timeProvider);
                    itemsForSale.Add(item);
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

        private void PopulateFeedsWithStartingStock()
        {
            // Add feed items for each feed type with a specific quantity
            int quantityForEachFeed = 20; // Example quantity
            foreach (FeedType feedType in Enum.GetValues(typeof(FeedType)))
            {
                for (int i = 0; i < quantityForEachFeed; i++)
                {
                    IBuyable feedItem = CreateFeed(feedType);
                    itemsForSale.Add(feedItem);
                }
            }
        }

        private IBuyable CreateFeed(FeedType feedType)
        {
            string name = $"{feedType} Feed"; // Example naming convention
            float price = 50.0f; // Example price, can be different for each type
            return new Feed(name, price, feedType);
        }
    }
}
