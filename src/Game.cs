using NetCoreAudio;
using static System.Console;

namespace Ghost_in_The_PowerShell
{
    internal class Game
    {
        private static Player? bgPlayer;

        public void Start()
        {
            Title = "Ghost In The PowerShell";
            playMusic("./files/HomeMenubg.wav");
            runHomeMenu();
        }

        private void runHomeMenu()
        {
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
                    playGame();
                    break;
                case 1:
                    aboutGame();
                    break;
                case 2:
                    exitGame();
                    break;
            }
        }

        private void playGame()
        {
            Engine Engine = new Engine();
            Engine.E_Start();
            runHomeMenu();
        }

        private void aboutGame()
        {
            Clear();
            keyboardPrint("Development in progress.."); //TODO: add aboutGame logic here
            ReadKey(true);
            runHomeMenu();
        }

        private void exitGame()
        {
            Clear();
            Console.WriteLine(
                "Are You Sure You Want To Exit? \nIf So, Press Enter Key To Escape Your Impending Doom!"
            );

            ConsoleKeyInfo exitGame = Console.ReadKey(true);

            if (exitGame.Key == ConsoleKey.Enter) 
            {
                Environment.Exit(0);
            }
            else 
            {
                runHomeMenu();
            }

            Console.ReadKey();
            if (bgPlayer != null)
                bgPlayer.Stop();
            Console.CursorVisible = true;
            Environment.Exit(0);
        }

        private static void playMusic(string filepath)
        {
            bgPlayer = new Player();
            bgPlayer.Play(filepath);
        }

        public static void keyboardPrint(string text, int speed = 40) //A method for the keyboard typing effect
        {
            foreach (char c in text)
            {
                Console.Write(c);
                Thread.Sleep(speed);
            }
        }
    } // class bracket
} //namespace bracket
