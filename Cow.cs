using Raylib_cs;
using System.Collections.Generic;
using System.Numerics;

namespace CustomProgram
{
    public class Cow : Animal
    {
        private static Random random = new Random();

        public Cow(string _name, float _purchasePrice, string _defaultImagePath, ITimeProvider _timeProvider):base(_name, _purchasePrice, _defaultImagePath, _timeProvider)
        {
            health = 150f;
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
                    health -= 11f; // Decrement health if hunger is at 0
                    hunger = 0; // Ensure hunger does not go below 0
                }
            }

            // Update produce timer
            if (currentTime - lastProduceUpdateTime >= produceTimer)
            {
                ProduceItem(); // Produce an item
                lastProduceUpdateTime = currentTime; // Reset the last produce time
            }

            if (health <= 0 && isAlive)
            {
                Die(); // Trigger the death of the animal only once
            }
        }

        /*public override Produce ProduceItem()
        {
            // Randomly decide to produce milk or beef
            int produceType = random.Next(2); // Generates 0 or 1

            if (produceType == 0)
            {
                Produce milk = new Produce("Milk", 1.5f, ProduceType.Milk);
                produces.Add(milk);
                return milk;
            }
            else
            {
                Produce beef = new Produce("Beef", 5.0f, ProduceType.Beef);
                produces.Add(beef);
                return beef;
            }
        }*/

        public override void ProduceItem()
        {
            int produceType = random.Next(2); // Generates 0 or 1

            Produce produce = produceType == 0
                ? new Produce("Milk", 1.5f, ProduceType.Milk)
                : new Produce("Beef", 5.0f, ProduceType.Beef);

            produces.Add(produce);
            //Console.WriteLine($"Produced item: {produce.name}"); // Debug statement
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
            return feedType == FeedType.CowFeed;
        }

        protected override float GetFeedValue(FeedType feedType)
        {
            return 27f; // The amount hunger is increased by when fed
        }
    }
}
