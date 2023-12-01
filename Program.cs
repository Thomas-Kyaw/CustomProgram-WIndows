using CustomProgram;
using Raylib_cs;
using System.Numerics;
using System.Collections.Generic;
using static System.Reflection.Metadata.BlobBuilder;

namespace CustomProgram
{
    public class Game
    {
        static GameState currentState = GameState.MainGame;
        static GameManager gameManager;
        static Vector2[] plotPositions;
        static int plotSize = 150;
        public static void Main()
        {
            gameManager = new GameManager("PlayerName");
            const int screenWidth = 650;
            const int screenHeight = 600;
            Raylib.InitWindow(screenWidth, screenHeight, "Farm Management Game");
            Raylib.SetTargetFPS(60);

            Raylib.SetExitKey(KeyboardKey.KEY_NULL);

            gameManager.LoadTextures();
            Color textColor = Color.WHITE;
            
            plotSize = 150;
            int plotSpacing = 20;
            int buttonSize = 50;
            int verticalSpacing = 10;

            int offsetX = (screenWidth - (3 * plotSize + 2 * plotSpacing)) / 2;
            int offsetY = (screenHeight - (3 * plotSize + 2 * plotSpacing)) / 2;

            plotPositions = new Vector2[9];
            for (int i = 0; i < 9; i++)
            {
                int row = i / 3;
                int col = i % 3;
                plotPositions[i] = new Vector2(
                    offsetX + col * (plotSize + plotSpacing),
                    offsetY + row * (plotSize + plotSpacing)
                );
            }

            Vector2[] buttonPositions = new Vector2[3];
            for (int i = 0; i < 3; i++)
            {
                buttonPositions[i] = new Vector2(
                    (i % 2 == 0) ? 10 : (screenWidth - buttonSize - 10),
                    offsetY + i * (buttonSize + verticalSpacing)
                );
            }
            // TODO - Need to set the plot in game manager
            while (!Raylib.WindowShouldClose())
            {
                gameManager.Update();
                // Check if the Escape key is pressed to return to the main game state
                if (Raylib.IsKeyPressed(KeyboardKey.KEY_ESCAPE))
                {
                    currentState = GameState.MainGame;
                }

                if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_LEFT_BUTTON))
                {
                    Vector2 mousePoint = Raylib.GetMousePosition();
                    if (currentState == GameState.MainGame)
                    {
                        for (int i = 0; i < plotPositions.Length; i++)
                        {
                            if (Raylib.CheckCollisionPointRec(mousePoint, new Rectangle(plotPositions[i].X, plotPositions[i].Y, plotSize, plotSize)))
                            {
                                currentState = (GameState)(i+1);
                                Console.WriteLine("Clicked on plot index: " + i);  // Debug message
                                break;
                            }
                        }

                        for (int i = 0; i < buttonPositions.Length; i++)
                        {
                            if (Raylib.CheckCollisionPointRec(mousePoint, new Rectangle(buttonPositions[i].X, buttonPositions[i].Y, buttonSize, buttonSize)))
                            {
                                currentState = (GameState)(i + 9);
                                Console.WriteLine("Clicked on plot index: " + i);  // Debug message
                            }
                        }
                    }
                    else
                    {
                        // Handle clicks when the overlay window is open
                        //currentState = GameState.MainGame; // This is a simplification. You would normally check what was clicked.
                    }
                }

                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.RAYWHITE);

                if (currentState == GameState.MainGame)
                {
                    // Draw the background texture tiled
                    for (int y = 0; y < screenHeight; y += gameManager.backgroundTexture.Height)
                    {
                        for (int x = 0; x < screenWidth; x += gameManager.backgroundTexture.Width)
                        {
                            Raylib.DrawTexture(gameManager.backgroundTexture, x, y, Color.WHITE);
                        }
                    }
                    
                    Texture2D[] textures = { gameManager.cowPlotTexture, gameManager.chickenPlotTexture, gameManager.pigPlotTexture, gameManager.goatPlotTexture, gameManager.sheepPlotTexture, gameManager.shopAnimalTexture, gameManager.buyFeedTexture, gameManager.sellMarketTexture, gameManager.inventoryTexture };
                    string[] plotNames = { "Cow", "Chicken", "Pig", "Goat", "Sheep", "Shop Animals", "Buy Feed", "Sell Produce", "Inventory" };

                    for (int i = 0; i < plotPositions.Length; i++)
                    {
                        Raylib.DrawTextureEx(textures[i], plotPositions[i], 0.0f, plotSize / (float)textures[i].Width, Color.WHITE);
                        Vector2 textSize = Raylib.MeasureTextEx(Raylib.GetFontDefault(), plotNames[i], 20, 1);
                        Raylib.DrawText(plotNames[i],
                                        (int)(plotPositions[i].X + (plotSize - textSize.X) / 2),
                                        (int)(plotPositions[i].Y + plotSize - textSize.Y - 10),
                                        20,
                                        textColor);
                    }

                    string[] buttonNames = { "Bag", "Animal", "Feed" };
                    Color[] buttonColors = { Color.GRAY, Color.LIGHTGRAY, Color.DARKGRAY };
                    for (int i = 0; i < buttonPositions.Length; i++)
                    {
                        Raylib.DrawRectangle((int)buttonPositions[i].X, (int)buttonPositions[i].Y, buttonSize, buttonSize, buttonColors[i]);
                        Vector2 textSize = Raylib.MeasureTextEx(Raylib.GetFontDefault(), buttonNames[i], 20, 1);
                        Raylib.DrawText(buttonNames[i],
                                        (int)(buttonPositions[i].X + (buttonSize - textSize.X) / 2),
                                        (int)(buttonPositions[i].Y + (buttonSize - textSize.Y) / 2),
                                        20,
                                        textColor);
                    }

                    RenderPlotsAndAnimals();
                }
                else
                {
                    // Render the overlay for the current game state
                    DrawOverlay(currentState);
                }

                Raylib.EndDrawing();
            }

            gameManager.UnloadTextures();
            // De-Initialization
            Raylib.CloseWindow();
        }

        static void DrawOverlay(GameState state)
        {
            // Depending on the state, you will draw different overlays
            // Here is where the implementation of the overlay rendering logic is put
            // This could be a simple color change, a new set of buttons, information display, etc.
            switch (state)
            {
                case GameState.ShopAnimals:
                    // Render shop animals overlay
                    DrawShopAnimalsOverlay(gameManager.Shop, gameManager.Player);
                    break;
                case GameState.BuyFeed:
                    // Render buy feed overlay
                    DrawShopFeedOverlay(gameManager.Shop, gameManager.Player);
                    break;
                case GameState.SellProduce:
                    // Render bag overlay
                    break;
                case GameState.Inventory:
                    // Render bag overlay
                    break;
                default:
                    break;
            }
            DrawCloseButton();
        }
        static void RenderPlotsAndAnimals()
        {
            // Define the number of animals per row based on the plot size and desired animal size
            int animalsPerRow = 4; // Adjust the number based on the size of your plots

            for (int i = 0; i < gameManager.Player.Plots.Count; i++)
            {
                Plot currentPlot = gameManager.Player.Plots[i];
                Vector2 plotPosition = plotPositions[i]; // The starting position of each plot

                List<Animal> animalsInPlot = currentPlot.Animals;

                // Define the size of each animal within the plot
                // Assuming we want each animal to be 1/4th the width of a plot and considering some spacing
                float animalSize = plotSize / (animalsPerRow + 1.0f); // Adjust the divisor for the size you want

                // Calculate the horizontal and vertical spacing between the animals
                float spacing = animalSize / 4.0f; // Give some space between animals

                for (int j = 0; j < animalsInPlot.Count; j++)
                {
                    int row = j / animalsPerRow;
                    int col = j % animalsPerRow;

                    // Calculate the position for each animal
                    Vector2 animalPosition = new Vector2(
                        plotPosition.X + col * (animalSize + spacing) + spacing,
                        plotPosition.Y + row * (animalSize + spacing) + spacing
                    );

                    Texture2D animalTexture = GetTextureForAnimal(animalsInPlot[j]);

                    // Draw the animal at the calculated position, scaled to the desired size
                    Raylib.DrawTextureEx(
                        animalTexture,
                        animalPosition,
                        0.0f, // No rotation
                        animalSize / 8.0f, // Scale factor to resize the 8x8 texture to the desired size
                        Color.WHITE
                    );
                }
            }
        }

        // Function to draw the overlay for shopping animals
        static float scrollY = 0;
        static void DrawShopAnimalsOverlay(Shop shop, Player player)
        {
            // Handle mouse wheel scroll (reversed direction)
            float mouseWheelMove = Raylib.GetMouseWheelMove();
            scrollY -= mouseWheelMove * 300; // Now scrolling up moves up, and down moves down

            // Calculate the total content height
            float contentHeight = shop.itemsForSale.OfType<Animal>().Count() * 70;

            // Add extra space equal to one item height for padding at the bottom
            float extraPadding = 70;

            // Clamp scrollY to prevent scrolling beyond the content
            float maxScroll = Math.Max(0, contentHeight + extraPadding - Raylib.GetScreenHeight());
            scrollY = Math.Clamp(scrollY, -extraPadding, maxScroll);

            // Overlay background
            Raylib.DrawRectangle(0, 0, Raylib.GetScreenWidth(), Raylib.GetScreenHeight(), new Color(0, 0, 0, 128));

            int startY = 50 - (int)scrollY;
            int itemHeight = 60; // Increased height for better spacing
            int buttonWidth = 400; // Increased button width for better layout
            int purchaseButtonWidth = 100; // Width for the buy button
            int padding = 10; // Padding inside the button for layout

            foreach (var item in shop.itemsForSale.OfType<Animal>().ToList())
            {
                Texture2D texture = GetTextureForAnimal(item); // Method to retrieve the correct texture for the animal
                                                                           // Since we're only using the first frame, we can define the source rectangle for the first sprite
                Rectangle sourceRect = new Rectangle(0, 0, 8, 8); // Define the source rectangle for the first sprite (8x8)

                float scale = (itemHeight - 2 * padding) / sourceRect.Height; // Calculate scale to fit in button

                string itemDisplayText = $"{item.GetType().Name} - ${item.purchasePrice}";
                int textX = (int)(50 + padding + sourceRect.Width * scale + padding); // Position text after the texture

                Rectangle buttonRect = new Rectangle(50, startY, buttonWidth, itemHeight);
                Rectangle purchaseButtonRect = new Rectangle(buttonRect.X + buttonWidth - purchaseButtonWidth, startY, purchaseButtonWidth, itemHeight);
                Vector2 texturePosition = new Vector2(50 + padding, startY + padding); // Position texture inside the button

                if (startY + itemHeight > 0 && startY < Raylib.GetScreenHeight() || mouseWheelMove < 0)
                {
                    Raylib.DrawRectangleRec(buttonRect, Color.BLUE);
                    Raylib.DrawText(itemDisplayText, textX, startY + padding, 20, Color.WHITE);

                    // Draw only the first sprite from the sprite sheet
                    Rectangle destRect = new Rectangle(texturePosition.X, texturePosition.Y, sourceRect.Width * scale, sourceRect.Height * scale); // Destination rectangle for scaled sprite
                    Raylib.DrawTexturePro(texture, sourceRect, destRect, new Vector2(0, 0), 0.0f, Color.WHITE);

                    if (player.Coins >= item.purchasePrice)
                    {
                        Raylib.DrawRectangleRec(purchaseButtonRect, Color.GREEN);
                        int buyTextX = (int)(purchaseButtonRect.X + (purchaseButtonWidth - Raylib.MeasureText("Buy", 20)) / 2f);
                        int buyTextY = startY + (itemHeight - Raylib.MeasureText("Buy", 20)) / 2;
                        Raylib.DrawText("Buy", buyTextX, buyTextY, 20, Color.WHITE);

                        if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_LEFT_BUTTON) && Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), purchaseButtonRect))
                        {
                            shop.BuyItem(item, player, 1);
                            shop.itemsForSale.Remove(item); // Remove the purchased item
                        }
                    }
                }
                
                startY += itemHeight + padding; // Increase spacing for next item
            }

            DrawCloseButton();
        }

        // Helper function to calculate the scale factor based on desired height
        static float CalculateAnimalScaleForTexture(Texture2D texture, int desiredHeight)
        {
            return desiredHeight / (float)texture.Height; // Ensure the scale is a float
        }


        static void DrawShopFeedOverlay(Shop shop, Player player)
        {
            // Background for the overlay
            Raylib.DrawRectangle(0, 0, Raylib.GetScreenWidth(), Raylib.GetScreenHeight(), new Color(0, 0, 0, 128));

            int startY = 50;
            int itemHeight = 60; // Increased height for better spacing
            int buttonWidth = 400; // Increased button width for better layout
            int buyButtonWidth = 100; // Width for the buy button
            int padding = 10; // Padding inside the button for layout

            // Iterate over each FeedType
            foreach (FeedType feedType in Enum.GetValues(typeof(FeedType)))
            {
                Texture2D texture = GetTextureForFeed(feedType); // This method should be defined in GameManager
                float scale = CalculateFeedScaleForTexture(texture, itemHeight - 2 * padding); // Calculate scale to fit in button

                // Create a display name for the feed type
                string feedDisplayName = $"{feedType} Feed";
                // Create a new feed item for display purposes
                var feedItem = shop.CreateFeed(feedType);

                // Fetch the quantity of the feed item in the player's inventory
                int feedQuantity = player.Inventory.BuyableItems.OfType<Feed>().Count(f => f.Type == feedType);

                // Prepare texts and calculate their positions
                string priceText = $"${feedItem.purchasePrice}";
                string quantityText = $"Quantity: {feedQuantity}";

                // Calculate the positions for texts and the texture
                int priceTextX = 50 + padding;
                int quantityTextX = buttonWidth - buyButtonWidth - padding - Raylib.MeasureText(quantityText, 20); // Align to the right
                int textY = startY + padding;
                Vector2 texturePosition = new Vector2(priceTextX + Raylib.MeasureText(priceText, 20) + padding, startY + padding);

                // Rectangle positions
                Rectangle buttonRect = new Rectangle(50, startY, buttonWidth, itemHeight);
                Rectangle buyButtonRect = new Rectangle(buttonRect.X + buttonWidth - buyButtonWidth, startY, buyButtonWidth, itemHeight);

                // Draw the item button (non-clickable, just for display)
                Raylib.DrawRectangleRec(buttonRect, Color.BLUE);
                // Draw the texture scaled
                Raylib.DrawTextureEx(texture, texturePosition, 0.0f, scale, Color.WHITE);
                // Draw the price and quantity texts
                Raylib.DrawText(priceText, priceTextX, textY, 20, Color.WHITE);
                Raylib.DrawText(quantityText, quantityTextX, textY, 20, Color.WHITE);

                // Draw the "Buy" button
                int buyTextX = (int)(buyButtonRect.X + (buyButtonWidth - Raylib.MeasureText("Buy", 20)) / 2f);
                int buyTextY = startY + (itemHeight - Raylib.MeasureText("Buy", 20)) / 2;
                Raylib.DrawRectangleRec(buyButtonRect, Color.GREEN);
                Raylib.DrawText("Buy", buyTextX, buyTextY, 20, Color.WHITE);

                // Check for button click to purchase the feed
                if (player.Coins >= feedItem.purchasePrice && Raylib.IsMouseButtonPressed(MouseButton.MOUSE_LEFT_BUTTON) && Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), buyButtonRect))
                {
                    shop.BuyItem(feedItem, player, 1); // Since feed is unlimited, directly handle the purchase
                }

                startY += itemHeight + padding; // Increase the spacing for the next item
            }

            DrawCloseButton();
        }

        // Helper function to calculate the scale factor based on desired height
        static float CalculateFeedScaleForTexture(Texture2D texture, int desiredHeight)
        {
            return desiredHeight / (float)texture.Height; // Ensure the scale is a float
        }

        // Helper functions to get textures based on animal or feed type
        public static Texture2D GetTextureForAnimal(Animal animal)
        {
            if(animal is Cow)
            {
                return gameManager.cowTexture;
            }
            else if(animal is Chicken)
            {
                return gameManager.chickenTexture;
            }
            else if (animal is Pig)
            {
                return gameManager.pigTexture;
            }
            else if (animal is Goat)
            {
                return gameManager.goatTexture;
            }
            else if(animal is Sheep)
            {
                return gameManager.sheepTexture;
            }
            else
                return gameManager.defaultTexture;
        }

        static public Texture2D GetTextureForPlot(Plot plot)
        {
            switch (plot.Type)
            {
                case PlotType.CowPlot:
                    return gameManager.cowPlotTexture;
                case PlotType.ChickenPlot:
                    return gameManager.chickenPlotTexture;
                case PlotType.PigPlot:
                    return gameManager.pigPlotTexture;
                case PlotType.SheepPlot:
                    return gameManager.sheepPlotTexture;
                case PlotType.GoatPlot:
                    return gameManager.goatPlotTexture;
                default:
                    return gameManager.defaultTexture; // Some default texture if needed
            }
        }

        public static Texture2D GetTextureForFeed(FeedType feedType)
        {
            switch (feedType)
            {
                case FeedType.CowFeed:
                    return gameManager.cowFeedTexture;
                case FeedType.ChickenFeed:
                    return gameManager.chickenFeedTexture;
                case FeedType.PigFeed:
                    return gameManager.pigFeedTexture;
                case FeedType.GoatFeed:
                    return gameManager.goatFeedTexture;
                case FeedType.SheepFeed:
                    return gameManager.sheepFeedTexture;
                default:
                    return gameManager.defaultTexture;
            }
        }

        static void DrawCloseButton()
        {
            // Draw the "Close" button consistently for all overlays
            Rectangle closeButtonRect = new Rectangle(10, 10, 100, 30); // Example position and size
            Raylib.DrawRectangleRec(closeButtonRect, Color.RED);
            Raylib.DrawText("Close", (int)closeButtonRect.X + 20, (int)closeButtonRect.Y + 5, 20, Color.WHITE);

            // Check if the "Close" button is clicked
            if (Raylib.IsMouseButtonReleased(MouseButton.MOUSE_LEFT_BUTTON))
            {
                if (Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), closeButtonRect))
                {
                    currentState = GameState.MainGame;
                }
            }
        }
    }
}
