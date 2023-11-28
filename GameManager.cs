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
        private ITimeProvider timeProvider;

        public GameManager(string playerName)
        {
            timeProvider = new RealTimeProvider();
            Player = new Player(playerName);
            Shop = new Shop();
            InitializeGame();
        }

        private void InitializeGame()
        {
            InitializePlayer();
            InitializeShop();
            // Any other game setup can go here.
        }

        private void InitializePlayer()
        {
            // Set up the player's starting resources, inventory, etc.
            Player.Coins = 1000; // Starting coins
                                 // More player setup...
        }

        private void InitializeShop()
        {
            // Use the stock from the Shop constructor to populate items for sale
            Shop.PopulateWithStartingStock(timeProvider);
            // More shop setup...
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
