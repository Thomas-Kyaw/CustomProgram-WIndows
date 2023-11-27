using Raylib_cs;
using System.Collections.Generic;
using System.Numerics;
using NUnit.Framework;

namespace CustomProgram
{
    public class TestTimeProvider : ITimeProvider
    {
        private double time;
        
        public void SetTime(double newTime)
        {
            time = newTime;
        }

        public double GetCurrentTime()
        {
            return time;
        }
    }

    [TestFixture]
    public class GameTests
    {
        private TestTimeProvider timeProvider;
        private Player player;

        [SetUp]
        public void Setup()
        {
            // Initialize with a mock or test time provider
            timeProvider = new TestTimeProvider(); 
            player = new Player("TestPlayer");
        }

        [Test]
        public void Animal_Added_To_Plot()
        {
            var plot = new Plot(PlotType.CowPlot, player);
            var animal = new Cow("Bessie", 100f, "cow.png", timeProvider);

            bool result = plot.AddAnimal(animal);

            Assert.IsTrue(result);
            Assert.AreEqual(1, plot.Animals.Count);
        }

        [Test]
        public void Animal_Dies_When_Health_Reaches_Zero()
        {
            var plot = new Plot(PlotType.CowPlot, player);
            var animal = new Cow("Bessie", 100f, "cow.png", timeProvider);
            plot.AddAnimal(animal);

            animal.ForceAnimalHealth(); // Force animal's health to zero

            plot.UpdateAnimals(); // Update should trigger Die() method

            Assert.IsFalse(animal.IsAlive);
        }

        [Test]
        public void Cannot_Add_Wrong_Animal_Type_To_Plot()
        {
            var cowPlot = new Plot(PlotType.CowPlot, player);
            var chicken = new Chicken("Clucky", 50f, "chicken.png", timeProvider); // Assuming Chicken class exists

            bool result = cowPlot.AddAnimal(chicken);

            Assert.IsFalse(result, "Should not be able to add a chicken to a cow plot.");
            Assert.AreEqual(0, cowPlot.Animals.Count, "Cow plot should have no animals.");
        }

        /*[Test]
        public void Cow_Produces_Correct_Produce()
        {
            var plot = new Plot(PlotType.CowPlot, player);
            var cow = new Cow("Bessie", 100f, "cow.png", timeProvider);
            plot.AddAnimal(cow);

            // Ensure enough time has passed for the cow to produce an item
            double newTime = cow.LastProduceUpdateTime + cow.ProduceTimer + 1;
            timeProvider.SetTime(newTime);

            plot.UpdateAnimals(); // This should trigger ProduceItem() and transfer to inventory

            Assert.IsNotEmpty(player.Inventory.SellableItems, "Player's inventory should have a produce item.");
            var producedItem = player.Inventory.SellableItems[0];
            Assert.IsTrue(producedItem.name == "Milk" || producedItem.name == "Beef", 
                        "Cow should produce either Milk or Beef in the player's inventory.");
        }*/

        // Other tests...
    }

}
