using static System.Console;
using WriteFunc;
using NetCoreAudio;
using Menu;
using MazeGame;

namespace Ghost_in_The_PowerShell
{
    internal class Program
    {
        private static Player? bgPlayer;

        public static void Main(string[] args)
        {
            Console.CursorVisible = false;
            Title = "Ghost In The PowerShell";

            Clear();
            // loadingBar();
            // someethingLoading();

            bgPlayer = new Player();
            bgPlayer.Play(".files/HomeMenubg.wav");

            runHomeMenu();

            // exit
            Console.CursorVisible = true;
        }

        private static void runHomeMenu()
        {
            Console.ResetColor();

            //TODO: MAKE THIS RESPONSIVE 
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

            // CWriteFunc.RenderUpperCenteredStrings(prompt);
            
            string[] options = { "Play", " About", "Exit" };
            Menu_C homeMenu = new Menu_C(prompt, options);
            int indexSelected = homeMenu.Run();

            switch (indexSelected)
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

        private static void G_playGame()
        {
            ScaryMaze Maze = new();
            Maze.LevelOne();
            runHomeMenu();
        }

        private static void G_aboutGame()
        {
            // int consoleWidth = Console.WindowWidth;
            int consoleHeight = Console.WindowHeight;

            Clear();

            // int currentRow = consoleHeight / 2 - 4;
            // Print each string centered in the console
            // NOTE: @Zeki-Zek pls make this cleaner (utilize loops and arrays)
            //DONE:

            Console.ForegroundColor = ConsoleColor.DarkRed;
            string[] aboutInfo = 
                {
                    "\"ABOUT\"",
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

            ReadKey(true);  // Wait for key press to proceed
            Console.ResetColor();
            runHomeMenu();  // Navigate back to the home menu (if needed)
        }

        private static void G_exitGame()
        {
            Clear();

            // int consoleHeight = Console.WindowHeight;
            // int currentRow = consoleHeight / 2;

            // NOTE: @Zeki-Zek pls make this cleaner (utilize loops and arrays)
            //
            ForegroundColor = ConsoleColor.DarkRed;
            string[] exitInfo = 
                {
                    "\"EXIT\"",
                    "\n",
                    "Are You Sure You Want To Exit?",
                    "If So, Press Enter Key To Escape Your Impending Doom!",
                };

                CWriteFunc.RenderCenteredStrings(exitInfo);

            ConsoleKeyInfo terminateProgram = Console.ReadKey(true);

            if (terminateProgram.Key == ConsoleKey.Enter)
            {
                if (bgPlayer != null) bgPlayer.Stop(); // assure that bgPlayer doesnt continue to run across systems
                return;
            }
            else
            {
                Console.ResetColor();
                runHomeMenu();
            }

        }

        //NOTE: commented this method so debugging has less friction (waiting for the loading screen is yes)
        
        // static void someethingLoading()
        // {
        //     //156 , 46
        //     // Console.WriteLine(Console.WindowWidth);
        //     // Console.WriteLine(Console.WindowHeight);
        //     Console.CursorVisible = false;

        //     int consoleWidth = Console.WindowWidth;
        //     int consoleHeight = Console.WindowHeight;

        //     for (int i = 0; i < consoleWidth; i++)
        //     {
        //         for (int j = 0; j < consoleHeight; j++)
        //         {
        //             if (j % 2 == 0)
        //             {
        //                 Console.SetCursorPosition(i, j);
        //                 Console.Write('█');
        //             }
        //             else
        //             {
        //                 Console.SetCursorPosition(consoleWidth - 1 - i, j);
        //                 Console.Write('█'); ;

        //             }
        //         }
        //         Thread.Sleep(1);
        //     }
        // }

        // //NOTE: yes
        
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
        //         System.Console.WriteLine(new string ('█', i)); //drawing of the progress bar

        //         //sets the position of the cursor for the progress bar percentage
        //         Console.SetCursorPosition(progressBarStart, verticalCenter + 1);
        //         System.Console.WriteLine($"{i * 2}/100");

        //         Thread.Sleep(40); //will simulate the loading time

        //     }
        // }
    }
}
