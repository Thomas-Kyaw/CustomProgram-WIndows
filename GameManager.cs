using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomProgram
{
    public class GameManager
    {
        public Player Player { get; private set; }
        public Shop Shop { get; private set; }

        public ITimeProvider timeProvider;

        public List<Plot> plots;

        public Texture2D defaultTexture;
        public Texture2D backgroundTexture;
        public Texture2D cowTexture;
        public Texture2D chickenTexture;
        public Texture2D pigTexture;
        public Texture2D sheepTexture;
        public Texture2D goatTexture;
        public Texture2D shopAnimalTexture;
        public Texture2D buyFeedTexture;
        public Texture2D sellMarketTexture;
        public Texture2D inventoryTexture;

        public GameManager(string playerName)
        {
            timeProvider = new RealTimeProvider();
            Player = new Player(playerName);
            Shop = new Shop();
            plots = new List<Plot>();
            InitializeGame();
        }

        private void InitializeGame()
        {
            InitializePlayer();
            InitializeShop();
            InitializePlot();
            // Any other game setup can go here.
        }

        private void InitializePlayer()
        {
            // Set up the player's starting resources, inventory, etc.
            Player.Coins = 1000; // Starting coins
            Player.Reputation = 100;                     // More player setup...
        }

        private void InitializeShop()
        {
            // Use the stock from the Shop constructor to populate items for sale
            Shop.PopulateWithStartingStock(timeProvider);
            // More shop setup...
        }
        private void InitializePlot()
        {
            Plot cowPlot = new Plot(PlotType.CowPlot, Player);
            plots.Add(cowPlot);
            Plot pigPlot = new Plot(PlotType.PigPlot, Player);
            plots.Add(pigPlot);
            Plot sheepPlot = new Plot(PlotType.SheepPlot, Player);
            plots.Add(sheepPlot);
            Plot goatPlot = new Plot(PlotType.GoatPlot, Player);
            plots.Add(goatPlot);
            Plot chickenPlot = new Plot(PlotType.ChickenPlot, Player);
            plots.Add(chickenPlot);
        }
        public void LoadTextures()
        {
             defaultTexture = Raylib.LoadTexture("assets/BackGroundGrass.png");
             backgroundTexture = Raylib.LoadTexture("assets/BackGroundGrass.png");
             cowTexture = Raylib.LoadTexture("assets/CowPlot.png");
             chickenTexture = Raylib.LoadTexture("assets/ChickenPlot.png");
             pigTexture = Raylib.LoadTexture("assets/PigPlot.png");
             goatTexture = Raylib.LoadTexture("assets/GoatPlot.png");
             sheepTexture = Raylib.LoadTexture("assets/SheepPlot.png");
             shopAnimalTexture = Raylib.LoadTexture("assets/ShopAnimal.png");
             buyFeedTexture = Raylib.LoadTexture("assets/BuyFeed.png");
             sellMarketTexture = Raylib.LoadTexture("assets/SellMarket.png");
             inventoryTexture = Raylib.LoadTexture("assets/Inventory.png");
        }

        public void UnloadTextures()
        {
            // Unload textures
            Raylib.UnloadTexture(defaultTexture);
            Raylib.UnloadTexture(backgroundTexture);
            Raylib.UnloadTexture(cowTexture);
            Raylib.UnloadTexture(chickenTexture);
            Raylib.UnloadTexture(pigTexture);
            Raylib.UnloadTexture(goatTexture);
            Raylib.UnloadTexture(sheepTexture);
            Raylib.UnloadTexture(shopAnimalTexture);
            Raylib.UnloadTexture(buyFeedTexture);
            Raylib.UnloadTexture(sellMarketTexture);
            Raylib.UnloadTexture(inventoryTexture);
        }

        public void Update()
        {
            // Update game logic
            //Player.Update();
            //Shop.Update();
            // Any other updates for your game entities
        }

        public void Render()
        {
            // Render the game state to the screen
            //Player.Render();
            //Shop.Render();
            // Any other rendering code for your game entities
        }
    }

}
