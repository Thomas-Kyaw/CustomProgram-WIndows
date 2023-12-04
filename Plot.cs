using Raylib_cs;
using System.Collections.Generic;
using System.Numerics;

namespace CustomProgram
{
    public class Plot
    {
        private Player owner;

        private int size;
        
        private Vector2 position;
        private string imagePath { get;  set; }
        protected List<Animal> animals { get; set; }
        private int animalLimit { get; set; }

        private PlotType type { get; set; }

        public Plot(PlotType _type,  Player _owner)
        {
            this.owner = _owner;
            type = _type;
            animals = new List<Animal>();
            SetAnimalLimitAndImagePath(_type);
        }
        // Property
        public List<Animal> Animals
        {
            get{return animals;}
        }
        public Vector2 Posiiton
        {
            get { return position; }
            set { position = value; }
        }
        public int Size
        {
            get { return size; }
            set { size = value; }
        }
        public PlotType Type
        {
            get { return type; }
        }
        private void SetAnimalLimitAndImagePath(PlotType type)
        {
            switch (type)
            {
                case PlotType.CowPlot:
                    animalLimit = 10;
                    break;
                case PlotType.ChickenPlot:
                    animalLimit = 20;
                    break;
                case PlotType.SheepPlot:
                    animalLimit = 7;
                    break;
                case PlotType.GoatPlot:
                    animalLimit = 8;
                    break;
                case PlotType.PigPlot:
                    animalLimit = 12;
                    break;
                default:
                    animalLimit = 5;
                    break;
            }
        }

        public bool AddAnimal(Animal animal)
        {
            if (Animals.Count >= animalLimit || !IsCorrectAnimalType(animal))
            {
                return false;
            }

            animal.OnDeath += Animal_OnDeath;
            animals.Add(animal);
            return true;
        }
        private void Animal_OnDeath(Animal animal)
        {
            owner.Reputation -= 7; // Decrease the player's reputation by 5
            owner.Reputation = Math.Max(0, owner.Reputation); // Ensure reputation doesn't go below 0
            RemoveAnimal(animal); // Remove the dead animal
        }

        private bool IsCorrectAnimalType(Animal animal)
        {
            // Logic to check if the animal type matches the plot type
            switch (type)
            {
                case PlotType.CowPlot:
                    return animal is Cow;
                case PlotType.ChickenPlot:
                    return animal is Chicken;
                case PlotType.PigPlot:
                    return animal is Pig;
                case PlotType.SheepPlot:
                    return animal is Sheep;
                case PlotType.GoatPlot:
                    return animal is Goat;
                default:
                    return false;
            }
        }

        public void UpdateAnimals()
        {     
            foreach (var animal in new List<Animal>(animals)) // Using a copy to safely modify the original list
            {
                animal.Update();
                TransferProducesToPlayer(animal);
                if (!animal.IsAlive)
                {
                    Console.WriteLine($"Removing dead animal: {animal.name}");
                    RemoveAnimal(animal);
                }
            }
        }

        public void RemoveAnimal(Animal animal)
        {
            Console.WriteLine($"Removing animal: {animal.name} from plot {Type}");
            animals.Remove(animal);
            animal.OnDeath -= RemoveAnimal; // Unsubscribe from the event
                                            // Additional cleanup if necessary
        }


        private void TransferProducesToPlayer(Animal animal)
        {
            foreach (var produce in animal.Produces)
            {
                owner.Inventory.AddSellableItem(produce);
                //Console.WriteLine($"Transferring produce to player: {produce.name}"); // Debug
            }
            animal.ClearProduces();
        }

    }
}
