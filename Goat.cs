using Raylib_cs;
using System.Collections.Generic;
using System.Numerics;

namespace CustomProgram
{
    public class Goat : Animal
    {
        private static Random random = new Random();
        public Goat(string _name, float _purchasePrice, string _defaultImagePath, ITimeProvider _timeProvider):base(_name, _purchasePrice, _defaultImagePath, _timeProvider)
        {
            health = 130f;
        }

        public override void Update()
        {
            double currentTime = Raylib.GetTime();
            if (currentTime - lastHungerUpdateTime >= hungerDecrementTime)
            {
                hunger -= 9f; // Decrement hunger
                lastHungerUpdateTime = currentTime; // Reset the last update time

                if (hunger <= 0)
                {
                    health -= 8f; // Decrement health if hunger is at 0
                    hunger = 0; // Ensure hunger does not go below 0
                }
            }

            // Update produce timer
            if (currentTime - lastProduceUpdateTime >= produceTimer)
            {
                ProduceItem(); // Produce an item
                lastProduceUpdateTime = currentTime; // Reset the last produce time
            }

            if (health <= 0)
            {
                if (isAlive)
                {
                    Die(); // Trigger the death of the animal only once
                }
            }
        }
        public override void ProduceItem()
        {
            // Randomly decide to produce milk or beef
            int produceType = random.Next(2); // Generates 0 or 1

            Produce produce = produceType == 0
                ? new Produce("Goat Milk", 1.5f, ProduceType.GoatMilk)
                : new Produce("Goat Meat", 5.0f, ProduceType.GoatMeat);

            // Add directly to the produces list for now
            produces.Add(produce);
        }

        public override void Feed(FeedType feedType)
        {
            if (IsCorrectFeedType(feedType))
            {
                hunger = Math.Min(hunger + GetFeedValue(feedType), 100f);
                lastHungerUpdateTime = Raylib.GetTime(); // Reset the hunger update timer after feeding
            }
        }

        protected override bool IsCorrectFeedType(FeedType feedType)
        {
            return feedType == FeedType.GoatFeed;
        }

        protected override float GetFeedValue(FeedType feedType)
        {
            return 29f; // The amount hunger is increased by when fed
        }
    }
}
