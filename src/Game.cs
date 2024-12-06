using NetCoreAudio;
using FPSEngine3D;
using static System.Console;

namespace Ghost_in_The_PowerShell
{
    internal class Game
    {
        private static Player? bgPlayer;
        public static Player? gameBgPlayer;


        public void Start()
        {
            Title = "Ghost In The PowerShell";
            G_playHomeMusic("./files/HomeMenubg.wav");
            runHomeMenu();
        }

        private void runHomeMenu()
        {
            
            Console.ResetColor();
            string prompt =
                @"
  ▄████  ██░ ██  ▒█████    ██████ ▄▄▄█████▓    ██▓ ███▄    █    ▄▄▄█████▓ ██░ ██ ▓█████ 
 ██▒ ▀█▒▓██░ ██▒▒██▒  ██▒▒██    ▒ ▓  ██▒ ▓▒    ▓██▒ ██ ▀█   █    ▓  ██▒ ▓▒▓██░ ██▒▓█   ▀
▒██░▄▄▄░▒██▀▀██░▒██░  ██▒░ ▓██▄   ▒ ▓██░ ▒░    ▒██▒▓██  ▀█ ██▒   ▒ ▓██░ ▒░▒██▀▀██░▒███   
▒██░▄▄▄░▒██▀▀██░▒██░  ██▒░ ▓██▄   ▒ ▓██░ ▒░    ░██░▓██▒  ▐▌██▒   ░ ▓██▓ ░ ░▓█ ░██ ▒▓█  ▄ 
░▓█  ██▓░▓█ ░██ ▒██   ██░  ▒   ██▒░ ▓██▓ ░     ░██░▒██░   ▓██░     ▒██▒ ░ ░▓█▒░██▓░▒████▒
░▒▓███▀▒░▓█▒░██▓░ ████▓▒░▒██████▒▒  ▒██▒ ░     ░▓  ░ ▒░   ▒ ▒      ▒ ░░    ▒ ░░▒░▒░░ ▒░ ░
 ░▒   ▒  ▒ ░░▒░▒░ ▒░▒░▒░ ▒ ▒▓▒ ▒ ░  ▒ ░░        ▒ ░░ ░░   ░ ▒░       ░     ▒ ░▒░ ░ ░ ░  ░
  ░   ░  ▒ ░▒░ ░  ░ ▒ ▒░ ░ ░▒  ░ ░    ░         ▒ ░   ░   ░ ░      ░       ░  ░░ ░   ░   
░ ░   ░  ░  ░░ ░░ ░ ░ ▒  ░  ░  ░     ░                    ░              ░  ░  ░   ░  ░
                 
   ██▓███   ▒█████   █     █░▓█████  ██▀███    ██████  ██░ ██ ▓█████  ██▓     ██▓    
▓██░  ██▒▒██▒  ██▒▓█░ █ ░█░▓█   ▀ ▓██ ▒ ██▒▒██    ▒ ▓██░ ██▒▓█   ▀ ▓██▒    ▓██▒    
▓██░ ██▓▒▒██░  ██▒▒█░ █ ░█ ▒███   ▓██ ░▄█ ▒░ ▓██▄   ▒██▀▀██░▒███   ▒██░    ▒██░    
▒██▄█▓▒ ▒▒██   ██░░█░ █ ░█ ▒▓█  ▄ ▒██▀▀█▄    ▒   ██▒░▓█ ░██ ▒▓█  ▄ ▒██░    ▒██░    
▒██▒ ░  ░░ ████▓▒░░░██▒██▓ ░▒████▒░██▓ ▒██▒▒██████▒▒░▓█▒░██▓░▒████▒░██████▒░██████▒
▒▓▒░ ░  ░░ ▒░▒░▒░ ░ ▓░▒ ▒  ░░ ▒░ ░░ ▒▓ ░▒▓░▒ ▒▓▒ ▒ ░ ▒ ░░▒░▒░░ ▒░ ░░ ▒░▓  ░░ ▒░▓  ░
░░       ░ ░ ░ ▒    ░   ░     ░     ░░   ░ ░  ░  ░   ░  ░░ ░   ░     ░ ░     ░ ░   
░ ░      ░       ░  ░   ░           ░   ░  ░  ░   ░  ░    ░  ░    ░  ░

";
            string[] options = { "Play", " About", "Exit" };
            Menu homeMenu = new Menu(prompt, options);
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

        private void G_playGame()
        {
            Engine Engine = new();
            Engine.Start();
            runHomeMenu();
        }

        private void G_aboutGame() //TODO: Add Game Logic here
        {
            // int consoleWidth = Console.WindowWidth;
            int consoleHeight = Console.WindowHeight;
            
            Clear(); 

            int currentRow = consoleHeight / 2 - 4;
            // Print each string centered in the console
            centeredText("\"ABOUT\"", ref currentRow);
            centeredText("\n", ref currentRow);
            centeredText("This Project is Developed by a Group of Computer Science Students as part of their Final Requirement in the Fundamentals of Programming Course.", ref currentRow);
            centeredText("\n", ref currentRow);
            centeredText("The Team consists of Students From BSCS 1B:", ref currentRow);
            centeredText("\n", ref currentRow);
            centeredText("\"Ezekiel Viray\"", ref currentRow);
            centeredText("\"Dan Rev Paco\"", ref currentRow);
            centeredText("\"John Wayne Capistrano\"", ref currentRow);
            centeredText("\n", ref currentRow);
            centeredText("Through this project, the developers aim to apply and showcase the fundamental programming", ref currentRow);
            centeredText("concepts learned throughout their course. This marks an important milestone in their academic", ref currentRow);
            centeredText("journey as they work together to build a functional application while adhering to best practices in software development.", ref currentRow);

            ReadKey(true);  // Wait for key press to proceed
            Console.ResetColor();
            runHomeMenu();  // Navigate back to the home menu (if needed)
        }

        private void G_exitGame()
        {
            Clear();
            
            int consoleHeight = Console.WindowHeight;
            int currentRow = consoleHeight / 2;

            //ForegroundColor = ConsoleColor.DarkRed;
            centeredText("\"EXIT\"", ref currentRow);
            centeredText("\n", ref currentRow);
            centeredText("Are You Sure You Want To Exit?", ref currentRow);
            centeredText("If So, Press Enter Key To Escape Your Impending Doom!", ref currentRow);
            ConsoleKeyInfo terminateProgram = Console.ReadKey(true);

            if (terminateProgram.Key == ConsoleKey.Enter) 
            {
                Environment.Exit(0);
            }
            else 
            {
                Console.ResetColor();
                runHomeMenu();
            }

        }

        public static void G_playHomeMusic(string filepath)
        {
            bgPlayer = new Player();
            bgPlayer.Play(filepath);
        }
        public static void G_playGameMusic(string filepath)
        {
            if (bgPlayer != null) 
            {
                bgPlayer.Stop();
            }
            gameBgPlayer = new Player();
            gameBgPlayer.Play(filepath);

        }


        public static void keyboardPrint(string text, int speed = 40) //A method for the keyboard typing effect
        {
            foreach (char c in text)
            {
                Console.Write(c);
                Thread.Sleep(speed);
            }
        }
        public static void centeredText(string text, ref int currentRow) 
        { //METHOD FOR CENTERING TEXT (make sure to declare int <variable_name> = Console.WindowHeight;)
          //and also declare a variable "int <name> = <variable_name above> / 2;  you can subtract it to change the height
            {
                int consoleWidth = Console.WindowWidth;
                
                int textLength = text.Length;
                int paddingWidth = (consoleWidth - textLength) / 2;
        
                // Debugging: Print padding and text length to verify
                //Console.WriteLine($"Text Length: {textLength}, Padding: {padding}");
                Console.SetCursorPosition(paddingWidth, currentRow);
                Console.WriteLine(text);
                currentRow++;
            }
        }
        // can be used in the future
        // private void CenterText(string text)
        //     {
        //         
        //         int consoleWidth = Console.WindowWidth;
        //         int consoleHeight = Console.WindowHeight;

        //         // Calculate the padding to center the text
        //         int padding = (consoleWidth - text.Length) / 2;

        //         int paddingHeight = consoleHeight / 2;

        //         
        //         Console.WriteLine(new string(' ', padding) + text);
        //     }

    } // class bracket
} //namespace bracket
