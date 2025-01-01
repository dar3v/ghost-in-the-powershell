using MazeGame;
using NetCoreAudio;
using WriteFunc;
using static System.Console;

namespace Ghost_in_The_PowerShell
{
    internal class Program
    {
        static string[] prompt =
        {
            "  ▄████  ██░ ██  ▒█████    ██████ ▄▄▄█████▓    ██▓ ███▄    █    ▄▄▄█████▓ ██░ ██ ▓█████   ",
            "██▒ ▀█▒▓██░ ██▒▒██▒  ██▒▒██    ▒ ▓  ██▒ ▓▒    ▓██▒ ██ ▀█   █    ▓  ██▒ ▓▒▓██░ ██▒▓█   ▀",
            "▒██░▄▄▄░▒██▀▀██░▒██░  ██▒░ ▓██▄   ▒ ▓██░ ▒░    ▒██▒▓██  ▀█ ██▒   ▒ ▓██░ ▒░▒██▀▀██░▒███   ",
            "▒██░▄▄▄░▒██▀▀██░▒██░  ██▒░ ▓██▄   ▒ ▓██░ ▒░    ░██░▓██▒  ▐▌██▒   ░ ▓██▓ ░ ░▓█ ░██ ▒▓█  ▄ ",
            "░▓█  ██▓░▓█ ░██ ▒██   ██░  ▒   ██▒░ ▓██▓ ░     ░██░▒██░   ▓██░     ▒██▒ ░ ░▓█▒░██▓░▒████▒",
            "░▒▓███▀▒░▓█▒░██▓░ ████▓▒░▒██████▒▒  ▒██▒ ░     ░▓  ░ ▒░   ▒ ▒      ▒ ░░    ▒ ░░▒░▒░░ ▒░ ░",
            " ░▒   ▒  ▒ ░░▒░▒░ ▒░▒░▒░ ▒ ▒▓▒ ▒ ░  ▒ ░░        ▒ ░░ ░░   ░ ▒░       ░     ▒ ░▒░ ░ ░ ░  ░",
            "  ░   ░  ▒ ░▒░ ░  ░ ▒ ▒░ ░ ░▒  ░ ░    ░         ▒ ░   ░   ░ ░      ░       ░  ░░ ░   ░   ",
            "░ ░   ░  ░  ░░ ░░ ░ ░ ▒  ░  ░  ░     ░                    ░              ░  ░  ░   ░  ",
            "   ██▓███   ▒█████   █     █░▓█████  ██▀███    ██████  ██░ ██ ▓█████  ██▓     ██▓    ",
            "▓██░  ██▒▒██▒  ██▒▓█░ █ ░█░▓█   ▀ ▓██ ▒ ██▒▒██    ▒ ▓██░ ██▒▓█   ▀ ▓██▒    ▓██▒    ",
            "▓██░ ██▓▒▒██░  ██▒▒█░ █ ░█ ▒███   ▓██ ░▄█ ▒░ ▓██▄   ▒██▀▀██░▒███   ▒██░    ▒██░    ",
            "▒██▄█▓▒ ▒▒██   ██░░█░ █ ░█ ▒▓█  ▄ ▒██▀▀█▄    ▒   ██▒░▓█ ░██ ▒▓█  ▄ ▒██░    ▒██░    ",
            "▒██▒ ░  ░░ ████▓▒░░░██▒██▓ ░▒████▒░██▓ ▒██▒▒██████▒▒░▓█▒░██▓░▒████▒░██████▒░██████▒",
            "▒▓▒░ ░  ░░ ▒░▒░▒░ ░ ▓░▒ ▒  ░░ ▒░ ░░ ▒▓ ░▒▓░▒ ▒▓▒ ▒ ░ ▒ ░░▒░▒░░ ▒░ ░░ ▒░▓  ░░ ▒░▓  ░",
            "░░       ░ ░ ░ ▒    ░   ░     ░     ░░   ░ ░  ░  ░   ░  ░░ ░   ░     ░ ░     ░ ░   ",
            "░ ░      ░       ░  ░   ░           ░   ░  ░  ░   ░  ░    ░  ░    ░  ░",
        };
        static string[] menuItems = { "Play", " About", "Exit" };
        static int selectedIndex = 0;
        static int previousConsoleWidth = Console.BufferWidth;
        public static Player? bgPlayer;

        public static void Main(string[] args)
        {
            Console.CursorVisible = false;
            Title = "Ghost In The PowerShell";
            int width = Console.WindowWidth;
            int height = Console.WindowHeight;
            float fallingSpeedMilliseconds = 0.000f;

            Clear();

            gamestartLoading();
            CWriteFunc.FallingBloodTransition(fallingSpeedMilliseconds);

            bgPlayer = new Player();
            bgPlayer.Play("./Files/HomeMenubg.wav");
            RunHomeMenu();

            // exit
            Console.CursorVisible = true;
        }

        private static void RunHomeMenu()
        {
            bool running = true;

            Thread resizeHandler = new Thread(MonitorResize);
            resizeHandler.IsBackground = true;
            resizeHandler.Start();

            while (running)
            {
                Console.Clear();
                DisplayTitle();
                DisplayMenu();

                ConsoleKey key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        selectedIndex--;
                        if (selectedIndex < 0)
                            selectedIndex = menuItems.Length - 1;
                        break;
                    case ConsoleKey.DownArrow:
                        selectedIndex++;
                        if (selectedIndex >= menuItems.Length)
                            selectedIndex = 0;
                        break;
                    case ConsoleKey.Enter:
                        HandleMenuSelection(ref running);
                        break;
                }
            }
        }

        static void DisplayTitle()
        {
            int lastWidth = Console.WindowWidth;
            int lastHeight = Console.WindowHeight;
            int currentWidth = Console.WindowWidth;
            int currentHeight = Console.WindowHeight;

            if (currentWidth != lastWidth || currentHeight != lastHeight)
            {
                Console.Clear();
                lastWidth = currentWidth;
                lastHeight = currentHeight;
            }
            System.Console.WriteLine("\n\n");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            if (currentWidth >= prompt[0].Length && currentHeight >= prompt.Length)
            {
                foreach (var line in prompt)
                {
                    int padding = Math.Max(0, (Console.BufferWidth - line.Length) / 2);
                    Console.WriteLine(new string(' ', padding) + line);
                }
                Console.ResetColor();
                Console.WriteLine();
            }
            else
            {
                Clear();
                Console.SetCursorPosition(0, 0);
                Console.WriteLine("Console Size Not Large Enough.");
                Console.WriteLine("Minimum size: 125x40");
                Console.WriteLine($"Current size: {currentWidth}x{currentHeight}");
            }
        }

        static void DisplayMenu()
        {
            int consoleHeight = Console.WindowHeight;
            int consoleWidth = Console.WindowWidth;

            // Set how much space you want to move down (ex. 5 lines down or 5 lines up)
            int verticalOffset = 23; // Adjust this value to control how much lower the menu goes

            // Adjust the starting row for the menu to ensure the instructions and menu go down
            int startingRow = verticalOffset;
            string[] instructs =
            {
                "Use Up/Down arrows to navigate and Enter to select.\n",
                "\n",
                "=== Main Menu ===\n",
            };

            if (consoleWidth < 125 || consoleHeight < 40)
            {
                Console.Clear();
                Console.SetCursorPosition(0, 0);
                string[] sizeNotEnough =
                {
                    "Console Size Not Large Enough.",
                    "Minimum size: 125x40",
                    $"Current size: {consoleWidth}x{consoleHeight}",
                };
                CWriteFunc.RenderCenteredStrings(sizeNotEnough);
                return; // exit the method early if size is insufficient
            }

            // Render the instructions with some vertical offset
            for (int i = 0; i < instructs.Length; i++)
            {
                Console.SetCursorPosition(
                    (consoleWidth - instructs[i].Length) / 2,
                    startingRow + i
                ); // Centered text with offset
                Console.WriteLine(instructs[i]);
            }

            // Move the menu items down a few rows, starting from the next line
            int currentRow = startingRow + instructs.Length + 1; // will skip the instruction rows ig, add an extra line for spacing

            // Display menu items
            for (int i = 0; i < menuItems.Length; i++)
            {
                string menuItem = menuItems[i];
                string menuText = i == selectedIndex ? $" >> {menuItem} << " : $"   {menuItem}   ";

                int padding = Math.Max(0, (consoleWidth - menuText.Length) / 2);

                // Set the cursor position for the menu item and print it
                Console.SetCursorPosition(padding, currentRow + i); // Positioning each menu item lower on the screen

                if (i == selectedIndex)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                }

                Console.WriteLine(menuText);
                Console.ResetColor();
            }
        }

        static void HandleMenuSelection(ref bool running)
        {
            switch (selectedIndex)
            {
                case 0:
                    G_playGame();
                    break;
                case 1:
                    G_aboutGame();
                    break;
                case 2:
                    G_exitGame();
                    break;
            }
        }

        static void MonitorResize()
        {
            while (true)
            {
                if (Console.BufferWidth != previousConsoleWidth)
                {
                    previousConsoleWidth = Console.BufferWidth;
                    Console.Clear();
                    DisplayTitle();
                    DisplayMenu();
                }
                Thread.Sleep(100);
            }
        }

        private static void G_playGame()
        {
            // Set the desired speed (in milliseconds between blood fall)
            float fallingSpeedMilliseconds = 0.00f; // Adjust to control the speed
            if (bgPlayer != null)
                bgPlayer.Stop();
            ScaryMaze Maze = new();
            CWriteFunc.FallingBloodTransition(fallingSpeedMilliseconds);

            Console.Clear();
            Maze.GameStart();

            if (bgPlayer != null)
                bgPlayer.Play("./Files/HomeMenubg.wav");
            // RunHomeMenu();
        }

        private static void G_aboutGame()
        {

            Clear();
            Console.ForegroundColor = ConsoleColor.DarkRed;
            string[] aboutInfo =
            {
                "┏┓┳┓┏┓┳┳┏┳┓",
                "┣┫┣┫┃┃┃┃ ┃ ",
                "┛┗┻┛┗┛┗┛ ┻ ",
                "\n",
                "This Project is Developed by a Group of Computer Science Students as part of their Final Requirement in the Fundamentals of Programming Course.",
                "\n",
                "The Team consists of Students From BSCS 1B:",
                "\"Ezekiel Viray\"",
                "\"Dan Rev Paco\"",
                "\"John Wayne Capistrano\"",
                "\n",
                "Through this project, the developers aim to apply and showcase the fundamental programming",
                "concepts learned throughout their course. This marks an important milestone in their academic",
                "journey as they work together to build a functional application while adhering to best practices in software development.",
            };
            CWriteFunc.RenderCenteredStrings(aboutInfo);

            ReadKey(true); // Wait for key press to proceed
            Console.ResetColor();
            RunHomeMenu(); // Navigate back to the home menu (if needed)
        }

        private static void G_exitGame()
        {
            Clear();
            ForegroundColor = ConsoleColor.DarkRed;
            string[] exitInfo =
            {
                "░▒▓████████▓▒░ ░▒▓█▓▒░░▒▓█▓▒░ ░▒▓█▓▒░░ ▒▓████████▓▒░ ",
                "░▒▓█▓▒░        ░▒▓█▓▒░░▒▓█▓▒░ ░▒▓█▓▒░    ░▒▓█▓▒░     ",
                "░▒▓█▓▒░        ░▒▓█▓▒░░▒▓█▓▒░ ░▒▓█▓▒░    ░▒▓█▓▒░     ",
                "░▒▓██████▓▒░    ░▒▓██████▓▒░  ░▒▓█▓▒░    ░▒▓█▓▒░     ",
                "░▒▓█▓▒░        ░▒▓█▓▒░░▒▓█▓▒░ ░▒▓█▓▒░    ░▒▓█▓▒░     ",
                "░▒▓█▓▒░        ░▒▓█▓▒░░▒▓█▓▒░ ░▒▓█▓▒░    ░▒▓█▓▒░     ",
                "░▒▓████████▓▒░ ░▒▓█▓▒░░▒▓█▓▒░ ░▒▓█▓▒░    ░▒▓█▓▒░     ",
                "\n",
                "\n",
                "Are You Sure You Want To Exit?",
                "If So, Press Enter Key To Escape Your Impending Doom!",
            };

            CWriteFunc.RenderCenteredStrings(exitInfo);

            ConsoleKeyInfo terminateProgram = Console.ReadKey(true);

            if (terminateProgram.Key == ConsoleKey.Enter)
            {
                Console.ResetColor();
                Console.CursorVisible = true;

                bgPlayer.Stop();
                Environment.Exit(0);
            }
            else
            {
                Console.ResetColor();
                RunHomeMenu();
            }
        }

        static void gamestartLoading()
        {
            ConsoleColor[] colors = new ConsoleColor[]
            {
                ConsoleColor.Red,
                ConsoleColor.Green,
                ConsoleColor.Blue,
                ConsoleColor.Cyan,
                ConsoleColor.Yellow,
                ConsoleColor.Magenta,
            };
            while (true)
            {
                // loadingBar();
                string[] loadingWelcome =
                {
                    " ▄         ▄  ▄▄▄▄▄▄▄▄▄▄▄  ▄            ▄▄▄▄▄▄▄▄▄▄▄  ▄▄▄▄▄▄▄▄▄▄▄  ▄▄       ▄▄  ▄▄▄▄▄▄▄▄▄▄▄ ",
                    "▐░▌       ▐░▌▐░░░░░░░░░░░▌▐░▌          ▐░░░░░░░░░░░▌▐░░░░░░░░░░░▌▐░░▌     ▐░░▌▐░░░░░░░░░░░▌",
                    "▐░▌       ▐░▌▐░█▀▀▀▀▀▀▀▀▀ ▐░▌          ▐░█▀▀▀▀▀▀▀▀▀ ▐░█▀▀▀▀▀▀▀█░▌▐░▌░▌   ▐░▐░▌▐░█▀▀▀▀▀▀▀▀▀ ",
                    "▐░▌       ▐░▌▐░▌          ▐░▌          ▐░▌          ▐░▌       ▐░▌▐░▌▐░▌ ▐░▌▐░▌▐░▌          ",
                    "▐░▌   ▄   ▐░▌▐░█▄▄▄▄▄▄▄▄▄ ▐░▌          ▐░▌          ▐░▌       ▐░▌▐░▌ ▐░▐░▌ ▐░▌▐░█▄▄▄▄▄▄▄▄▄ ",
                    "▐░▌  ▐░▌  ▐░▌▐░░░░░░░░░░░▌▐░▌          ▐░▌          ▐░▌       ▐░▌▐░▌  ▐░▌  ▐░▌▐░░░░░░░░░░░▌",
                    "▐░▌ ▐░▌░▌ ▐░▌▐░█▀▀▀▀▀▀▀▀▀ ▐░▌          ▐░▌          ▐░▌       ▐░▌▐░▌   ▀   ▐░▌▐░█▀▀▀▀▀▀▀▀▀ ",
                    "▐░▌▐░▌ ▐░▌▐░▌▐░▌          ▐░▌          ▐░▌          ▐░▌       ▐░▌▐░▌       ▐░▌▐░▌          ",
                    "▐░▌░▌   ▐░▐░▌▐░█▄▄▄▄▄▄▄▄▄ ▐░█▄▄▄▄▄▄▄▄▄ ▐░█▄▄▄▄▄▄▄▄▄ ▐░█▄▄▄▄▄▄▄█░▌▐░▌       ▐░▌▐░█▄▄▄▄▄▄▄▄▄ ",
                    "▐░░▌     ▐░░▌▐░░░░░░░░░░░▌▐░░░░░░░░░░░▌▐░░░░░░░░░░░▌▐░░░░░░░░░░░▌▐░▌       ▐░▌▐░░░░░░░░░░░▌",
                    " ▀▀       ▀▀  ▀▀▀▀▀▀▀▀▀▀▀  ▀▀▀▀▀▀▀▀▀▀▀  ▀▀▀▀▀▀▀▀▀▀▀  ▀▀▀▀▀▀▀▀▀▀▀  ▀         ▀  ▀▀▀▀▀▀▀▀▀▀▀ ",
                };

                string[] loadingTo =
                {
                    " ▄▄▄▄▄▄▄▄▄▄▄  ▄▄▄▄▄▄▄▄▄▄▄ ",
                    "▐░░░░░░░░░░░▌▐░░░░░░░░░░░▌",
                    " ▀▀▀▀█░█▀▀▀▀ ▐░█▀▀▀▀▀▀▀█░▌",
                    "     ▐░▌     ▐░▌       ▐░▌",
                    "     ▐░▌     ▐░▌       ▐░▌",
                    "     ▐░▌     ▐░▌       ▐░▌",
                    "     ▐░▌     ▐░▌       ▐░▌",
                    "     ▐░▌     ▐░▌       ▐░▌",
                    "     ▐░▌     ▐░█▄▄▄▄▄▄▄█░▌",
                    "     ▐░▌     ▐░░░░░░░░░░░▌",
                    "      ▀       ▀▀▀▀▀▀▀▀▀▀▀ ",
                };

                string[] prompt =
                {
                    "  ▄████  ██░ ██  ▒█████    ██████ ▄▄▄█████▓    ██▓ ███▄    █    ▄▄▄█████▓ ██░ ██ ▓█████   ",
                    "██▒ ▀█▒▓██░ ██▒▒██▒  ██▒▒██    ▒ ▓  ██▒ ▓▒    ▓██▒ ██ ▀█   █    ▓  ██▒ ▓▒▓██░ ██▒▓█   ▀",
                    "▒██░▄▄▄░▒██▀▀██░▒██░  ██▒░ ▓██▄   ▒ ▓██░ ▒░    ▒██▒▓██  ▀█ ██▒   ▒ ▓██░ ▒░▒██▀▀██░▒███   ",
                    "▒██░▄▄▄░▒██▀▀██░▒██░  ██▒░ ▓██▄   ▒ ▓██░ ▒░    ░██░▓██▒  ▐▌██▒   ░ ▓██▓ ░ ░▓█ ░██ ▒▓█  ▄ ",
                    "░▓█  ██▓░▓█ ░██ ▒██   ██░  ▒   ██▒░ ▓██▓ ░     ░██░▒██░   ▓██░     ▒██▒ ░ ░▓█▒░██▓░▒████▒",
                    "░▒▓███▀▒░▓█▒░██▓░ ████▓▒░▒██████▒▒  ▒██▒ ░     ░▓  ░ ▒░   ▒ ▒      ▒ ░░    ▒ ░░▒░▒░░ ▒░ ░",
                    " ░▒   ▒  ▒ ░░▒░▒░ ▒░▒░▒░ ▒ ▒▓▒ ▒ ░  ▒ ░░        ▒ ░░ ░░   ░ ▒░       ░     ▒ ░▒░ ░ ░ ░  ░",
                    "  ░   ░  ▒ ░▒░ ░  ░ ▒ ▒░ ░ ░▒  ░ ░    ░         ▒ ░   ░   ░ ░      ░       ░  ░░ ░   ░   ",
                    "░ ░   ░  ░  ░░ ░░ ░ ░ ▒  ░  ░  ░     ░                    ░              ░  ░  ░   ░  ",
                    "   ██▓███   ▒█████   █     █░▓█████  ██▀███    ██████  ██░ ██ ▓█████  ██▓     ██▓    ",
                    "▓██░  ██▒▒██▒  ██▒▓█░ █ ░█░▓█   ▀ ▓██ ▒ ██▒▒██    ▒ ▓██░ ██▒▓█   ▀ ▓██▒    ▓██▒    ",
                    "▓██░ ██▓▒▒██░  ██▒▒█░ █ ░█ ▒███   ▓██ ░▄█ ▒░ ▓██▄   ▒██▀▀██░▒███   ▒██░    ▒██░    ",
                    "▒██▄█▓▒ ▒▒██   ██░░█░ █ ░█ ▒▓█  ▄ ▒██▀▀█▄    ▒   ██▒░▓█ ░██ ▒▓█  ▄ ▒██░    ▒██░    ",
                    "▒██▒ ░  ░░ ████▓▒░░░██▒██▓ ░▒████▒░██▓ ▒██▒▒██████▒▒░▓█▒░██▓░▒████▒░██████▒░██████▒",
                    "▒▓▒░ ░  ░░ ▒░▒░▒░ ░ ▓░▒ ▒  ░░ ▒░ ░░ ▒▓ ░▒▓░▒ ▒▓▒ ▒ ░ ▒ ░░▒░▒░░ ▒░ ░░ ▒░▓  ░░ ▒░▓  ░",
                    "░░       ░ ░ ░ ▒    ░   ░     ░     ░░   ░ ░  ░  ░   ░  ░░ ░   ░     ░ ░     ░ ░   ",
                    "░ ░      ░       ░  ░   ░           ░   ░  ░  ░   ░  ░    ░  ░    ░  ░",
                };
                bgPlayer = new Player();
                // Task loadingBarTask = Task.Run(() => loadingBar()); (this lambda expression is killing me)

                bgPlayer.Play("./Files/ThudsoundTitle.wav");
                CWriteFunc.RenderCenteredStrings(loadingWelcome, colors);
                Thread.Sleep(1900);

                Console.Clear();
                bgPlayer.Play("./Files/ThudsoundTitle.wav");
                CWriteFunc.RenderCenteredStrings(loadingTo, colors);
                Thread.Sleep(1900);

                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkRed;
                bgPlayer.Play("./Files/GameTitleSound.wav");
                CWriteFunc.RenderCenteredStrings(prompt);
                Thread.Sleep(1900);
                break;
            }
        }

        // static void loadingBar() //added a loading in the start of the game
        // {
        //     //setting the dimension of the console window
        //     int consoleWidth = Console.WindowWidth;
        //     int consoleHeight = Console.WindowHeight;

        //     //calculation for the vertical center
        //     int verticalCenter = (consoleHeight / 2) + 11;

        //     //calculating the horizontal start position for centering the bar
        //     int barWidth = 50;
        //     int barStart = (consoleWidth - barWidth) / 2;

        //     for (int i = 0; i <= barWidth; i++)
        //     {
        //         // calculate the horizontal position of the text
        //         int progressBarStart = (consoleWidth - 8) / 2;

        //         //sets the position of the cursor for the progress bar
        //         Console.SetCursorPosition(barStart, verticalCenter);
        //         System.Console.WriteLine(new string('█', i)); //drawing of the progress bar

        //         //sets the position of the cursor for the progress bar percentage
        //         Console.SetCursorPosition(progressBarStart, verticalCenter + 1);
        //         System.Console.WriteLine($"{i * 2}/100");

        //         Thread.Sleep(40); //will simulate the loading time
        //     }
        // }
    }
}
