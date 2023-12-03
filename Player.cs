using Raylib_cs;
using System.Numerics;

namespace CustomProgram
{
    public class Player
    {
        private string name;
        private Inventory inventory;
        private float coins;
        public int reputation;

        private List<Plot> plots;

        public Player(string _name)
        {
            name = _name;
            inventory = new Inventory();
            coins = 10000; // Initial amount of coins
            reputation = 100; // Initial reputation
            plots = new List<Plot>();
        }

        public Inventory Inventory
        {
            get{return inventory;}
        }
        public float Coins
        {
            get{return coins;}
            set{coins = value;}
        }
        public int Reputation
        {
            get{return reputation;}
            set{reputation = value;}
        }
        public List<Plot> Plots
        { get { return plots;} }

        public void AddPlot(Plot plot)
        {
            plots.Add(plot);
        }
        //test purpose
        public void RemovePlot(Plot plot)
        {
            plots.Remove(plot);
        }

        
    }
}
