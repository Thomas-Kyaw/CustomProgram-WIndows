using Raylib_cs;
using System.Collections.Generic;
using System.Numerics;

namespace CustomProgram
{
    
    public class Sheep : Animal
    {
        private static Random random = new Random();
        public Sheep(string _name, float _purchasePrice, string _defaultImagePath, ITimeProvider _timeProvider):base(_name, _purchasePrice, _defaultImagePath, _timeProvider)
        {
            health = 120f;
            produceTimer = 22f;
        }

        public override void Update()
        {
            double currentTime = base.timeProvider.GetCurrentTime();
            if (currentTime - lastHungerUpdateTime >= hungerDecrementTime && isAlive)
            {
                hunger -= 8f; // Decrement hunger
                lastHungerUpdateTime = currentTime; // Reset the last update time

                if (hunger <= 0)
                {
                    health -= 9f; // Decrement health if hunger is at 0
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
                    ? new Produce("Lamb", 65f, ProduceType.Lamb)
                    : new Produce("Wool", 75f, ProduceType.Wool);

                produces.Add(produce);

                // Debug statement
                //Console.WriteLine($"Sheep '{name}' produced {produce.name} at {DateTime.Now}. Current Produce Count: {produces.Count}");
            }
        }
        public override void Feed(FeedType feedType)
        {
            if (IsCorrectFeedType(feedType))
            {
                hunger = Math.Min(hunger + GetFeedValue(feedType), 100f);
                lastHungerUpdateTime = Raylib.GetTime(); // Reset the hunger update timer after feeding
                if(hunger >= 85)
                {
                    health = 120f;
                }
            }
        }

        protected override bool IsCorrectFeedType(FeedType feedType)
        {
            return feedType == FeedType.SheepFeed;
        }

        protected override float GetFeedValue(FeedType feedType)
        {
            return 31f; // The amount hunger is increased by when fed
        }
    }
}
