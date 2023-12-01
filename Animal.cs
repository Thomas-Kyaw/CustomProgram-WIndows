using Raylib_cs;
using System.Collections.Generic;
using System.Numerics;

namespace CustomProgram
{
    public abstract class Animal:IBuyable
    {
        public string name{ get; set; }

        public float purchasePrice{ get; set; }

        protected Vector2 position{ get; set;}
        protected float hunger { get; set; } = 100f; // Starting hunger
        protected float health { get; set; } = 100f; // Starting health
        protected string imagePath { get; set; }
        protected List<Produce> produces { get; set; }
        protected double hungerDecrementTime = 10f; // Time in seconds to decrement hunger
        protected double lastHungerUpdateTime = Raylib.GetTime(); // Last time hunger was updated

        protected double produceTimer = 15f; // Time in seconds until the animal produces an item
        protected double lastProduceUpdateTime = Raylib.GetTime(); // Last time an item was produced

        protected bool isAlive = true;

        protected abstract bool IsCorrectFeedType(FeedType feedType);
        protected abstract float GetFeedValue(FeedType feedType);

        protected ITimeProvider timeProvider;

        public Animal(string _name, float _purchasePrice, string defaultImagePath, ITimeProvider timeProvider)
        {
            name = _name;
            purchasePrice = _purchasePrice;
            imagePath = defaultImagePath;
            produces = new List<Produce>();
            this.timeProvider = timeProvider;
        }
        public bool IsAlive
        {
            get{return isAlive;}
        }

        public virtual void Update()
        {
            double currentTime = timeProvider.GetCurrentTime();
            if (currentTime - lastHungerUpdateTime >= hungerDecrementTime)
            {
                hunger -= 10f; // Decrement hunger
                lastHungerUpdateTime = currentTime; // Reset the last update time

                if (hunger <= 0)
                {
                    health -= 10f; // Decrement health if hunger is at 0
                    hunger = 0; // Ensure hunger does not go below 0
                }
            }

            // Check for death
            if (health <= 0 && isAlive)
            {
                health = 0; // Ensure health doesn't go below 0
                Die();
            }
        }

        //public abstract Produce ProduceItem();
        public abstract void ProduceItem();
        // Feed method updates hunger based on FeedType
        public virtual void Feed(FeedType feedType)
        {
            // Check for correct feed type and update hunger
            if (IsCorrectFeedType(feedType))
            {
                hunger = Math.Min(hunger + 20f, 100f);
            }
        }

        // This method will be called when the animal's health reaches 0
        public virtual void Die()
        {
            isAlive = false;
            Console.WriteLine($"Animal '{name}' has died at {DateTime.Now}."); // Debug message
            // Perform any additional cleanup specific to the animal
            OnDeath?.Invoke(this);
        }

        public void ClearProduces()
        {
            produces.Clear();
        }

        // Delegate and event for handling death
        public delegate void AnimalDeathHandler(Animal animal);
        public event AnimalDeathHandler OnDeath;

        // for testing purposes
        public void ForceAnimalHealth()
        {
            health = 0;
        }

        public void ForceHunger()
        {

        }

        public List<Produce> Produces
        {
            get{return produces;}
        }
        public double LastProduceUpdateTime
        {
            get{return lastProduceUpdateTime;}
        }
        public double ProduceTimer
        {
            get{return produceTimer;}
        }

    }

}
