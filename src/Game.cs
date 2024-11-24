using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using static System.Console;



namespace Ghost_in_The_PowerShell
{
    internal class Game
    {
        public void Start()
        {   
            Title = "Ghost In The PowerShell";
            playMusic("HomeMenubg.wav");
            runHomeMenu();


        }
        private void runHomeMenu()
        {
            string prompt = @"

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

Use Your Arrow Keys To Hover Through The Selections and Press Enter to Select";
            string[] options = { "Play", "About", "Exit" };
            Menu homeMenu = new Menu(prompt, options);
            int indexSelected = homeMenu.Run();

            switch (indexSelected) {
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
            Clear();
            WriteLine(@"
Hello
");
            Console.ReadKey();
            runHomeMenu();
        }

        private void aboutGame()
        {
            Clear();
            WriteLine(@"
Hello
");
            ReadKey(true);
            runHomeMenu();
        }

        private void exitGame()
        {
            Clear();
            Console.WriteLine("Are You Sure You Want To Exit? \nIf So, Press Enter Key To Escape Your Impending Doom!");
            Console.ReadKey(); 
            Environment.Exit(0);
        }
        private void bgSounds()
        {
            playMusic("HomeMenubg.wav");
            Console.ReadKey(true);

        }
        public static void playMusic(string filepath) 
        {
            SoundPlayer bgPlayer = new SoundPlayer(); //FIXME
            bgPlayer.SoundLocation = filepath;
            bgPlayer.Play();
        }
    }
}
