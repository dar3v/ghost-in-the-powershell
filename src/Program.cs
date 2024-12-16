using static System.Console;
using WriteFunc;
using NetCoreAudio;
using Menu;
using RaycasterCS;
namespace Ghost_in_The_PowerShell
{
        internal class Program
        {
            private static Player? bgPlayer;
            public static Player? gameBgPlayer;
            public static void Main(string[] args)
            {
                Console.CursorVisible = false;
                Clear();
                loadingBar();
                someethingLoading();
                Program program = new();
                program.Start();

                Console.CursorVisible = true;
            }
            static void someethingLoading()
            {
                //156 , 46
                // Console.WriteLine(Console.WindowWidth);
                // Console.WriteLine(Console.WindowHeight);
                Console.CursorVisible = false;

                int consoleWidth = Console.WindowWidth;
                int consoleHeight = Console.WindowHeight;

                for (int i = 0; i < consoleWidth; i++)
                {
                    for (int j = 0; j < consoleHeight; j++)
                    {
                        if (j % 2 == 0)
                        {
                            Console.SetCursorPosition(i, j);
                            Console.Write('█');
                        }
                        else
                        {
                            Console.SetCursorPosition(consoleWidth - 1 - i, j);
                            Console.Write('█'); ;

                        }
                    }
                    Thread.Sleep(1);
                }
            }

            static void loadingBar() //added a loading in the start of the game
            {
                //setting the dimension of the console window
                int consoleWidth = Console.WindowWidth;
                int consoleHeight = Console.WindowHeight;

                //calculation for the vertical center
                int verticalCenter = (consoleHeight / 2) + 11;

                //calculating the horizontal start position for centering the bar
                int barWidth = 50;
                int barStart = (consoleWidth - barWidth) / 2;

                for (int i = 0; i <= barWidth; i++)
                {
                    // calculate the horizontal position of the text
                    int progressBarStart = (consoleWidth - 8) / 2;

                    //sets the position of the cursor for the progress bar                
                    Console.SetCursorPosition(barStart, verticalCenter);
                    System.Console.WriteLine(new string ('█', i)); //drawing of the progress bar

                    //sets the position of the cursor for the progress bar percentage
                    Console.SetCursorPosition(progressBarStart, verticalCenter + 1);
                    System.Console.WriteLine($"{i * 2}/100");

                    Thread.Sleep(40); //will simulate the loading time

                }
            }
            public void Start()
            {
                bgPlayer = new Player();
                bgPlayer.Play(".files/HomeMenubg.wav");
                Title = "Ghost In The PowerShell";
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
                // NOTE: @Zeki-Zek pls make this cleaner (utilize loops and arrays)
                CWriteFunc.CenteredText("\"ABOUT\"", ref currentRow);
                CWriteFunc.CenteredText("\n", ref currentRow);
                CWriteFunc.CenteredText("This Project is Developed by a Group of Computer Science Students as part of their Final Requirement in the Fundamentals of Programming Course.", ref currentRow);
                CWriteFunc.CenteredText("\n", ref currentRow);
                CWriteFunc.CenteredText("The Team consists of Students From BSCS 1B:", ref currentRow);
                CWriteFunc.CenteredText("\n", ref currentRow);
                CWriteFunc.CenteredText("\"Ezekiel Viray\"", ref currentRow);
                CWriteFunc.CenteredText("\"Dan Rev Paco\"", ref currentRow);
                CWriteFunc.CenteredText("\"John Wayne Capistrano\"", ref currentRow);
                CWriteFunc.CenteredText("\n", ref currentRow);
                CWriteFunc.CenteredText("Through this project, the developers aim to apply and showcase the fundamental programming", ref currentRow);
                CWriteFunc.CenteredText("concepts learned throughout their course. This marks an important milestone in their academic", ref currentRow);
                CWriteFunc.CenteredText("journey as they work together to build a functional application while adhering to best practices in software development.", ref currentRow);

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
                CWriteFunc.CenteredText("\"EXIT\"", ref currentRow);
                CWriteFunc.CenteredText("\n", ref currentRow);
                CWriteFunc.CenteredText("Are You Sure You Want To Exit?", ref currentRow);
                CWriteFunc.CenteredText("If So, Press Enter Key To Escape Your Impending Doom!", ref currentRow);
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
        }
    }
