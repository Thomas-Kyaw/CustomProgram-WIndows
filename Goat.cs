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
            produceTimer = 21f;
        }

        public override void Update()
        {
            double currentTime = base.timeProvider.GetCurrentTime();
            if (currentTime - lastHungerUpdateTime >= hungerDecrementTime && isAlive)
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
            if (currentTime - lastProduceUpdateTime >= produceTimer && isAlive)
            {
                ProduceItem(); // Produce an item
                lastProduceUpdateTime = currentTime; // Reset the last produce time
            }

            if (health <= 0 && isAlive)
            {
                    Die(); // Trigger the death of the animal only once
            }
        }
        public override void ProduceItem()
        {
            if (isAlive)
            {
                int produceType = random.Next(2); // Generates 0 or 1
                Produce produce = produceType == 0
                    ? new Produce("GoatMeat", 55f, ProduceType.GoatMeat)
                    : new Produce("GoatMilk", 60f, ProduceType.GoatMilk);

                produces.Add(produce);
                // Debug statement
                //Console.WriteLine($"Goat '{name}' produced {produce.name} at {DateTime.Now}. Current Produce Count: {produces.Count}");
            }
        }

        public override void Feed(FeedType feedType)
        {
            if (IsCorrectFeedType(feedType))
            {
                hunger = Math.Min(hunger + GetFeedValue(feedType), 100f);
                lastHungerUpdateTime = Raylib.GetTime(); // Reset the hunger update timer after feeding
                if(hunger >= 90)
                {
                    health = 130f;
                }
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
