using CustomProgram;
using Raylib_cs;
using System.Numerics;

namespace CustomProgram
{
    public class Game
    {
        static GameState currentState = GameState.MainGame;   

        public static void Main()
        {
            GameManager gameManager = new GameManager("PlayerName");
            const int screenWidth = 650;
            const int screenHeight = 600;
            Raylib.InitWindow(screenWidth, screenHeight, "Farm Management Game");
            Raylib.SetTargetFPS(60);

            Raylib.SetExitKey(KeyboardKey.KEY_NULL);

            Color textColor = Color.WHITE;

            Texture2D backgroundTexture = Raylib.LoadTexture("assets/BackGroundGrass.png");
            Texture2D cowTexture = Raylib.LoadTexture("assets/CowPlot.png");
            Texture2D chickenTexture = Raylib.LoadTexture("assets/ChickenPlot.png");
            Texture2D pigTexture = Raylib.LoadTexture("assets/PigPlot.png");
            Texture2D goatTexture = Raylib.LoadTexture("assets/GoatPlot.png");
            Texture2D sheepTexture = Raylib.LoadTexture("assets/SheepPlot.png");
            Texture2D shopAnimalTexture = Raylib.LoadTexture("assets/ShopAnimal.png");
            Texture2D buyFeedTexture = Raylib.LoadTexture("assets/BuyFeed.png");
            Texture2D sellMarketTexture = Raylib.LoadTexture("assets/SellMarket.png");
            Texture2D inventoryTexture = Raylib.LoadTexture("assets/Inventory.png");

            int plotSize = 150;
            int plotSpacing = 20;
            int buttonSize = 50;
            int verticalSpacing = 10;

            int offsetX = (screenWidth - (3 * plotSize + 2 * plotSpacing)) / 2;
            int offsetY = (screenHeight - (3 * plotSize + 2 * plotSpacing)) / 2;

            Vector2[] plotPositions = new Vector2[9];
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
                    for (int y = 0; y < screenHeight; y += backgroundTexture.Height)
                    {
                        for (int x = 0; x < screenWidth; x += backgroundTexture.Width)
                        {
                            Raylib.DrawTexture(backgroundTexture, x, y, Color.WHITE);
                        }
                    }
                    
                    Texture2D[] textures = { cowTexture, chickenTexture, pigTexture, goatTexture, sheepTexture, shopAnimalTexture, buyFeedTexture, sellMarketTexture, inventoryTexture };
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
                }
                else
                {
                    // Render the overlay for the current game state
                    DrawOverlay(currentState);
                }

                Raylib.EndDrawing();
            }

            // Unload textures
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
                    //DrawShopAnimalsOverlay(shop, player);
                    break;
                case GameState.BuyFeed:
                    // Render bag overlay
                    break;
                // ... handle other cases
                default:
                    break;
            }
            DrawCloseButton();
        }
        static void DrawShopAnimalsOverlay(Shop shop, Player player)
        {
            // Draw the overlay background
            Raylib.DrawRectangle(0, 0, Raylib.GetScreenWidth(), Raylib.GetScreenHeight(), new Color(0, 0, 0, 128));

            int startY = 50;
            int itemHeight = 30;
            int buttonWidth = 200;

            foreach (var item in shop.itemsForSale)
            {
                if (item is Animal animal)
                {
                    int stock = shop.GetStockForAnimal(animal.GetType());
                    string itemDisplayText = $"{animal.name} - ${item.purchasePrice}";
                    Rectangle buttonRect = new Rectangle(50, startY, buttonWidth, itemHeight);

                    // Draw the button
                    Color buttonColor = stock > 0 ? Raylib_cs.Color.DARKBLUE : Raylib_cs.Color.GRAY;
                    Raylib.DrawRectangleRec(buttonRect, buttonColor);
                    Raylib.DrawText(itemDisplayText, (int)buttonRect.X + 5, (int)buttonRect.Y + 5, 20, Raylib_cs.Color.WHITE);

                    // Check for button click
                    if (stock > 0 && Raylib.IsMouseButtonPressed(MouseButton.MOUSE_LEFT_BUTTON) && Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), buttonRect))
                    {
                        shop.BuyItem(item, player, 1);  // Purchase the animal
                    }

                    startY += itemHeight + 10;
                }
            }

            DrawCloseButton();
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
