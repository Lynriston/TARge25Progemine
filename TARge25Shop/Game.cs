using static System.Console;

namespace TARge25Shop
{
    public class Game
    {
        private World MyWorld;
        private Player CurrentPlayer;
        public void Start()
        {
            Title = "Welcome to the Maze";
            CursorVisible = false;

            string[,] grid =
            {
                {"=", "=", "=", "=", "=", "=", "=" },
                {"=", " ", "=", " ", " ", " ", "X" },
                {" ", " ", "=", " ", "=", " ", "=" },
                {"=", " ", " ", " ", "=", " ", "=" },
                {"=", " ", " ", " ", "=", " ", "=" },
                {"=", "=", "=", "=", "=", "=", "=" },
            };
            MyWorld = new World(grid);

            CurrentPlayer = new Player(0, 2);

            RunGameLoop();
        }

        private void DisplayIntro()
        {
            WriteLine("Welcome to the Maze!");
            WriteLine("\nInstructions: Use");
            WriteLine("> Use the arrow keys to move");
            Write("> Try to reach the goal, which looks like this: ");
            ForegroundColor = ConsoleColor.Green;
            WriteLine("X");
            WriteLine("> Press any key to start...");
            ReadKey(true);

        }

        private void DisplayOutro()
        {
            Clear();
            WriteLine("You escaped!");
            WriteLine("Thanks for playing!");
            WriteLine("Press any key to exit...");
            ReadKey(true);
        }

        private void DrawFrame()
        {
            Clear();
            MyWorld.Draw();
            CurrentPlayer.Draw();
        }

        private void HandlePlayerInput()
        {
            ConsoleKeyInfo keyInfo = ReadKey(true);
            ConsoleKey key = keyInfo.Key;
            switch (key)
            {
                case ConsoleKey.UpArrow:
                    if (MyWorld.IsPositionWalkable(CurrentPlayer.X, CurrentPlayer.Y - 1))
                    {
                        CurrentPlayer.Y -= 1;
                    }
                    break;
                case ConsoleKey.DownArrow:
                    if (MyWorld.IsPositionWalkable(CurrentPlayer.X, CurrentPlayer.Y + 1))
                    {
                        CurrentPlayer.Y += 1;
                    }
                    break;
                case ConsoleKey.LeftArrow: 
                    if (MyWorld.IsPositionWalkable(CurrentPlayer.X - 1, CurrentPlayer.Y))
                    {
                        CurrentPlayer.X -= 1;
                    }
                    break;
                case ConsoleKey.RightArrow:
                    if (MyWorld.IsPositionWalkable(CurrentPlayer.X + 1, CurrentPlayer.Y))
                    {
                        CurrentPlayer.X += 1;
                    }
                    break;
                default:
                    break;
            }
        }

        private void RunGameLoop()
        {
            DisplayIntro();
            while (true)
            {
                // Draw everrything
                DrawFrame();

                // Check player input and move player
                HandlePlayerInput();

                // Check if the player has reached exit and end game if true
                string elementAtPlayerPosition = MyWorld.GetElementAt(CurrentPlayer.X, CurrentPlayer.Y);
                if (elementAtPlayerPosition == "X")
                {
                    break;
                }

                // Give console chance to render
                System.Threading.Thread.Sleep(20);
            }
            DisplayOutro();
        }
    }
}
