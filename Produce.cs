using Raylib_cs;
using System.Numerics;

namespace CustomProgram
{
    public class Produce:ISellable
    {
        public string name{ get; set; }

        public float sellPrice{ get; set; }

        private ProduceType type;
        private string imagePath;
        public Produce(string _name, float _sellPrice, ProduceType _type)
        {
            name = _name;
            sellPrice = _sellPrice;
            type = _type;
            AssignImagePath(_type);
        }
        private void AssignImagePath(ProduceType type)
        {
            switch (type)
            {
                case ProduceType.Beef:
                    imagePath = "assets/Beef.png";
                    break;
                case ProduceType.Milk:
                    imagePath = "assets/CowMilk.png";
                    break;
                case ProduceType.Pork:
                    imagePath = "assets/Pork.png";
                    break;
                case ProduceType.GoatMeat:
                    imagePath = "assets/GoatMeat.png";
                    break;
                case ProduceType.GoatMilk:
                    imagePath = "assets/GoatMilk.png";
                    break;
                case ProduceType.ChickenMeat:
                    imagePath = "assets/ChickenMeat.png";
                    break;
                case ProduceType.Egg:
                    imagePath = "assets/Egg.png";
                    break;
                case ProduceType.Wool:
                    imagePath = "assets/Wool.png";
                    break;
                case ProduceType.Lamb:
                    imagePath = "assets/Lamb.png";
                    break;
                default:
                    imagePath = "assets/Default.png";
                    break;
            }
        }
        public string ImagePath
        {
            get { return imagePath; }
        }
    }
}
