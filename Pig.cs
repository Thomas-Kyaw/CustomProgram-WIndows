using Raylib_cs;
using System.Collections.Generic;
using System.Numerics;

namespace CustomProgram
{
    public class Pig : Animal
    {
        public Pig(string _name, float _purchasePrice, string _defaultImagePath, ITimeProvider _timeProvider):base(_name, _purchasePrice, _defaultImagePath, _timeProvider)
        {
            health = 140f;
            produceTimer = 24f;
        }

        public override void Update()
        {
            double currentTime = base.timeProvider.GetCurrentTime();
            if (currentTime - lastHungerUpdateTime >= hungerDecrementTime && isAlive)
            {
                hunger -= 11f; // Decrement hunger
                lastHungerUpdateTime = currentTime; // Reset the last update time

                if (hunger <= 0)
                {
                    health -= 7f; // Decrement health if hunger is at 0
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
            Produce produce = new Produce("Pork", 50f, ProduceType.Pork);
            produces.Add(produce);
            // Debug statement
            //Console.WriteLine($"Pig '{name}' produced {produce.name} at {DateTime.Now}. Current Produce Count: {produces.Count}");
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
            return feedType == FeedType.SheepFeed;
        }

        protected override float GetFeedValue(FeedType feedType)
        {
            return 25f; // The amount hunger is increased by when fed
        }
    }
}
