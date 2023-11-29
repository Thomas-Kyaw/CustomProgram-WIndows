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
            set { Size = value; }
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
                    imagePath = "assets/CowPlot.png";
                    animalLimit = 10;
                    break;
                case PlotType.ChickenPlot:
                    imagePath = "assets/ChickenPlot.png";
                    animalLimit = 20;
                    break;
                case PlotType.SheepPlot:
                    imagePath = "assets/SheepPlot.png";
                    animalLimit = 7;
                    break;
                case PlotType.GoatPlot:
                    imagePath = "assets/GoatPlot.png";
                    animalLimit = 8;
                    break;
                case PlotType.PigPlot:
                    imagePath = "assets/PigPlot.png";
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

            animal.OnDeath += RemoveAnimal;
            animals.Add(animal);
            return true;
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

        public void RemoveAnimal(Animal animal)
        {
            animals.Remove(animal);
            animal.OnDeath -= RemoveAnimal; // Unsubscribe from the event
        }

        public void UpdateAnimals()
        {
            foreach (var animal in new List<Animal>(animals))
            {
                animal.Update();
                Console.WriteLine($"Updating animal. Produces count: {animal.Produces.Count}"); // Debug
                TransferProducesToPlayer(animal);
                if (!animal.IsAlive)
                {
                    RemoveAnimal(animal);
                }
            }
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
