using Raylib_cs;
using System.Numerics;

namespace CustomProgram
{
    public class Feed:IBuyable
    {
        public string name{ get; set; }

        public float purchasePrice{ get; set; }

        private FeedType type{get;set;}
        private string imagePath;
        public Feed(string _name, float _purchasePrice, FeedType _type)
        {
            name = _name;
            purchasePrice = _purchasePrice;
            type = _type;
            AssignImagePath(_type);
        }
        public FeedType Type
        { get { return type; } }
        private void AssignImagePath(FeedType type)
        {
            switch (type)
            {
                case FeedType.CowFeed:
                    imagePath = "assets/Radish.png";
                    break;
                case FeedType.ChickenFeed:
                    imagePath = "assets/Corn.png";
                    break;
                case FeedType.PigFeed:
                    imagePath = "assets/Taro.png";
                    break;
                case FeedType.GoatFeed:
                    imagePath = "assets/Carrot.png";
                    break;
                case FeedType.SheepFeed:
                    imagePath = "assets/Pea.png";
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
