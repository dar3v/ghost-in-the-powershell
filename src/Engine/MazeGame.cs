using NAudio.Wave;
using NetCoreAudio;
using RaycasterCS;
using WriteFunc;

namespace MazeGame
{
    class ScaryMaze
    {
        bool bQuit = false;
        bool bGameOver = false;
        public static int nMaxLevels = 5; // how many levels are there

        // --= Game Start =--
        public void GameStart()
        {
            Menu.StartMenu();

            Player LinGameMusic = new();
            var WinGameMusic = new WaveOutEvent();

            if (OperatingSystem.IsLinux())
                LinGameMusic.Play("./Files/GameMusic.wav");
            else if (OperatingSystem.IsWindows())
            {
                var reader = new WaveFileReader("./Files/GameMusic.wav");
                WinGameMusic.Init(reader);
                WinGameMusic.Play();
            }

            int nCurrentLevel = 1;
            while (true)
            {
                // Initalize level map
                InitLevel(nCurrentLevel);

                Engine engine = new();
                int result = engine.Start();

                // Check the result immediately
                if (result == 0)
                {
                    bQuit = true;
                    break;
                } // Exit the loop if quitting
                else if (result == 1)
                {
                    bGameOver = true;
                    break;
                } // Exit the loop if game over
                else if (result == 2)
                {
                    // Level completed, check if there are more levels
                    if (nCurrentLevel < nMaxLevels)
                    {
                        nCurrentLevel++;
                        Menu.NextLevelMenu(nCurrentLevel);
                        continue; // Continue to the next level
                    }
                    else
                        break; // Exit the loop to finish the game
                }
            }
            // stop game bg
            LinGameMusic.Stop();
            WinGameMusic.Dispose();

            // Handle end of game scenarios
            if (bGameOver)
                Menu.GameOver();
            else if (bQuit)
                return; // Quit
            else
                Menu.GameFinish(); // Game finished successfully
        }

        // --- initialize level variables ---
        // Invokes class `InitGame`
        // takes an int paramater
        // just add another `if else` statement to add another level
        private void InitLevel(int level)
        {
            if (level == 1) // level one
            {
                InitGame.MazeMap =
                [
                    "###############",
                    "#P.....@..#...#",
                    "#.........#...#",
                    "#####.....##..#",
                    "###.....###...#",
                    "#........###..#",
                    "######.....#..#",
                    "####......@#..#",
                    "####.......#..#",
                    "#####....######",
                    "####.....#..@.#",
                    "####..####....#",
                    "###...#.......#",
                    "###...........#",
                    "###############",
                ];

                InitGame.initMazeTime = 45;

                InitGame.initMapWidth = 15;
                InitGame.initMapHeight = 15;

                InitGame.initPlayerX = 1;
                InitGame.initPlayerY = 1;
            }
            else if (level == 2) // level two
            {
                InitGame.MazeMap =
                [
                    "####################",
                    "#..P...#####.......#",
                    "#.........##.......#",
                    "#####......#########",
                    "#.........##.......#",
                    "#.@.......##.......#",
                    "#.......####.......#",
                    "#....####..##......#",
                    "#...#####...##.....#",
                    "#..###########.....#",
                    "#...########.......#",
                    "#........#####.....#",
                    "#..#...@...###.#####",
                    "#..##......#####...#",
                    "#..######....#######",
                    "#..#..@.#....#####.#",
                    "#..#....###........#",
                    "#..#...####...######",
                    "#..#............####",
                    "####################",
                ];

                InitGame.initMazeTime = 55;

                InitGame.initMapWidth = 20;
                InitGame.initMapHeight = 20;

                InitGame.initPlayerX = 3;
                InitGame.initPlayerY = 1;
            }
            else if (level == 3) // level three
            {
                InitGame.MazeMap =
                [
                    "#########################",
                    "#.P..............####...#",
                    "#...#########..@.####...#",
                    "#..##########....####...#",
                    "###########......##.....#",
                    "#####...........##......#",
                    "####...........##....#..#",
                    "#####..........#.....#..#",
                    "#############..#######..#",
                    "###.....#####...#########",
                    "##.............##########",
                    "#######........#######..#",
                    "#..######......#.....#..#",
                    "#..#...........#..@..#..#",
                    "#.......................#",
                    "#.......................#",
                    "####....#....####....####",
                    "#.....#####.....#.......#",
                    "#..###########..#.#.....#",
                    "#.....######....####....#",
                    "#########....@...##..####",
                    "#.......#........#......#",
                    "#.......#.....#########.#",
                    "#.......#...............#",
                    "#########################",
                ];

                InitGame.initMazeTime = 75;

                InitGame.initMapWidth = InitGame.MazeMap[0].Length; // lmao
                InitGame.initMapHeight = InitGame.MazeMap.Length; // lmao

                InitGame.initPlayerX = 2;
                InitGame.initPlayerY = 1;
            }
            else if (level == 4) // level four
            {
                InitGame.MazeMap =
                [
                    "#########################",
                    "#...............#####...#",
                    "#.P................##...#",
                    "#....#########......#...#",
                    "###########..###....#####",
                    "#.......#######......####",
                    "#.@.............@...##..#",
                    "#.............#######...#",
                    "############..#####.##..#",
                    "#########.....##.....####",
                    "######........#.......###",
                    "##.....................##",
                    "##.@........###......@.##",
                    "##.........####........##",
                    "###...#############...###",
                    "###################...###",
                    "##.....####.....###..####",
                    "#...@...##....#####..####",
                    "#.......#######.......###",
                    "#.....................###",
                    "###.......##...........##",
                    "#####.....#####......@.##",
                    "######......##.........##",
                    "######.............######",
                    "#########################",
                ];
                InitGame.initMazeTime = 75;

                InitGame.initMapWidth = InitGame.MazeMap[0].Length; // lmao
                InitGame.initMapHeight = InitGame.MazeMap.Length; // lmao

                InitGame.initPlayerX = 2;
                InitGame.initPlayerY = 2;
            }
            else if (level == 5) // level five
            {
                InitGame.MazeMap =
                [
                    "#########################",
                    "#........########.......#",
                    "#.P............##.......#",
                    "#...............#########",
                    "###########..@..#...@..##",
                    "#.@.##..........##.....##",
                    "#....##..........#......#",
                    "#.....##................#",
                    "#................###...##",
                    "#######.#..##..####....##",
                    "######..###########..####",
                    "####....########......###",
                    "###......######........##",
                    "##......#...@..#......###",
                    "#......##.......###..####",
                    "#......#..#.....###...###",
                    "##....#...###.....#...###",
                    "##....#...#####......####",
                    "##.........#........#####",
                    "#.................#######",
                    "########............#####",
                    "#######....#......@..####",
                    "######...@.##.......#####",
                    "###........####...#######",
                    "#########################",
                ];
                InitGame.initMazeTime = 75;

                InitGame.initMapWidth = InitGame.MazeMap[0].Length; // lmao
                InitGame.initMapHeight = InitGame.MazeMap.Length; // lmao

                InitGame.initPlayerX = 2;
                InitGame.initPlayerY = 2;
            }
            else
                Console.WriteLine("not a level yet. check `nMaxLevels` value in MazeGame.cs");
        }
    }

    // menu class
    // for outputting menus ingame
    class Menu()
    {
        public static Player? bgPlayer;
        public static Player? introSound;

        internal static void gameIntroLore()
        {
            string[] intro =
            {
                "The year is 1995. You're an investigative journalist, drawn to a string of eerie disappearances",
                "surrounding a derelict cathedral on the outskirts of town. The place, once a beacon of hope and",
                "faith, now lies in decay, shrouded in whispers of the unholy. Locals speak of \"Gabriel's Veil,\" an urban",
                "legend claiming that the Archangel Gabriel, once a messenger of light, had been corrupted. The",
                "entity now masquerades as an angel to deceive and ensnare the faithful.",
            };

            string[] intro2 =
            {
                "You discover an ancient journal buried within the rubble of the cathedral. The pages, brittle with age,",
                "detail a horrifying account of a priest who uncovered Gabriel's true nature. The entity, he claimed,",
                "had created statues of itself, scattered within labyrinths it conjured in the realm between life and",
                "death. These statues serve as conduits of its influence, strengthening its dominion over the mortal",
                "world. The priest believed that finding and destroying these statues was the only way to weaken the false angel's grasp.",
            };

            string[] intro3 =
            {
                "As you delve deeper, the journal reveals a chilling warning: “Should you enter the maze, your faith",
                "will be tested. Gabriel thrives on fear and doubt. When the clock strikes, it comes for you.”\n",
                "Determined to uncover the truth and end the disappearances, you decide to enter the maze, armed",
                "with nothing but your wits and the journal’s cryptic guidance. The maze is a place of shifting",
                "shadows, echoing whispers, and oppressive darkness. Each statue you find brings a sense of hope",
                "but also heightens Gabriel’s wrath. The closer you get to destroying its power, the more terrifying and relentless it becomes.",
            };
            string[] intro4 =
            {
                "You must act quickly, for every moment spent in the maze strengthens Gabriel’s influence.",
                "Should you fail, you risk not only your life but your very soul, doomed to wander the labyrinth forever,",
                "another victim in Gabriel's collection. Will you prevail, or will the false angel claim yet another believer?",
            };
            introSound = new Player();
            introSound.Play("./Files/introLore.wav");

            CWriteFunc.RenderCenteredStringsTyping(intro, 30);
            Console.ReadKey();
            CWriteFunc.RenderCenteredStringsTyping(intro2, 30);
            Console.ReadKey();
            CWriteFunc.RenderCenteredStringsTyping(intro3, 30);
            Console.ReadKey();
            CWriteFunc.RenderCenteredStringsTyping(intro4, 30);
        }

        internal static void StopIntroMusic()
        {
            if (introSound != null)
            {
                introSound.Stop();
            }
        }

        internal static void NextLevelMenu(int currentLevel)
        {
            // if adding a level, make sure to add a respective sprite here
            string[] levelTwo =
            [
                "      ┏┓             ┓   •             ",
                "      ┃ ┏┓┏┓┏┓┏┓┏┓╋┓┏┃┏┓╋┓┏┓┏┓┏        ",
                "      ┗┛┗┛┛┗┗┫┛ ┗┻┗┗┻┗┗┻┗┗┗┛┛┗┛        ",
                "             ┛                         ",
                "┏┓          ┓•           ┓       ┓  ┏┓ ",
                "┃┃┏┓┏┓┏┏┓┏┓┏┫┓┏┓┏┓  ╋┏┓  ┃ ┏┓┓┏┏┓┃  ┏┛ ",
                "┣┛┛ ┗┛┗┗ ┗ ┗┻┗┛┗┗┫  ┗┗┛  ┗┛┗ ┗┛┗ ┗  ┗━ ",
                "                 ┛                     ",
            ];
            string[] levelThree =
            [
                "      ┏┓             ┓   •             ",
                "      ┃ ┏┓┏┓┏┓┏┓┏┓╋┓┏┃┏┓╋┓┏┓┏┓┏        ",
                "      ┗┛┗┛┛┗┗┫┛ ┗┻┗┗┻┗┗┻┗┗┗┛┛┗┛        ",
                "             ┛                         ",
                "┏┓          ┓•           ┓       ┓  ┏┓ ",
                "┃┃┏┓┏┓┏┏┓┏┓┏┫┓┏┓┏┓  ╋┏┓  ┃ ┏┓┓┏┏┓┃   ┫ ",
                "┣┛┛ ┗┛┗┗ ┗ ┗┻┗┛┗┗┫  ┗┗┛  ┗┛┗ ┗┛┗ ┗  ┗┛ ",
                "                 ┛                     ",
            ];
            string[] levelFour =
            [
                "      ┏┓             ┓   •             ",
                "      ┃ ┏┓┏┓┏┓┏┓┏┓╋┓┏┃┏┓╋┓┏┓┏┓┏        ",
                "      ┗┛┗┛┛┗┗┫┛ ┗┻┗┗┻┗┗┻┗┗┗┛┛┗┛        ",
                "             ┛                         ",
                "┏┓          ┓•           ┓       ┓  ┏┓ ",
                "┃┃┏┓┏┓┏┏┓┏┓┏┫┓┏┓┏┓  ╋┏┓  ┃ ┏┓┓┏┏┓┃  ┃┃ ",
                "┣┛┛ ┗┛┗┗ ┗ ┗┻┗┛┗┗┫  ┗┗┛  ┗┛┗ ┗┛┗ ┗  ┗╋ ",
                "                 ┛                     ",
            ];
            string[] levelFive =
            [
                "      ┏┓             ┓   •             ",
                "      ┃ ┏┓┏┓┏┓┏┓┏┓╋┓┏┃┏┓╋┓┏┓┏┓┏        ",
                "      ┗┛┗┛┛┗┗┫┛ ┗┻┗┗┻┗┗┻┗┗┗┛┛┗┛        ",
                "             ┛                         ",
                "┏┓          ┓•           ┓       ┓  ┏━ ",
                "┃┃┏┓┏┓┏┏┓┏┓┏┫┓┏┓┏┓  ╋┏┓  ┃ ┏┓┓┏┏┓┃  ┗┓ ",
                "┣┛┛ ┗┛┗┗ ┗ ┗┻┗┛┗┗┫  ┗┗┛  ┗┛┗ ┗┛┗ ┗  ┗┛ ",
                "                 ┛                     ",
            ];
            // string[] levelN = [ -- your sprite here -- ];
            string[] notALevel = ["error:", "missing title"];

            Console.Clear();
            string[] writeStr = [];
            switch (currentLevel)
            {
                case 2:
                    writeStr = levelTwo;
                    break;
                case 3:
                    writeStr = levelThree;
                    break;
                case 4:
                    writeStr = levelFour;
                    break;
                case 5:
                    writeStr = levelFive;
                    break;
                // case n: writeStr = levelN; break;

                default:
                    writeStr = notALevel;
                    break;
            }
            CWriteFunc.RenderCenteredStrings(writeStr);
            Thread.Sleep(2000);
        }

        internal static void GameFinish()
        {
            // TODO: something prettier
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkRed;
            string[] congrats =
            {
                " ██████╗ ██████╗ ███╗   ██╗ ██████╗ ██████╗  █████╗ ████████╗██╗   ██╗██╗      █████╗ ████████╗██╗ ██████╗ ███╗   ██╗███████╗██╗",
                "██╔════╝██╔═══██╗████╗  ██║██╔════╝ ██╔══██╗██╔══██╗╚══██╔══╝██║   ██║██║     ██╔══██╗╚══██╔══╝██║██╔═══██╗████╗  ██║██╔════╝██║",
                "██║     ██║   ██║██╔██╗ ██║██║  ███╗██████╔╝███████║   ██║   ██║   ██║██║     ███████║   ██║   ██║██║   ██║██╔██╗ ██║███████╗██║",
                "██║     ██║   ██║██║╚██╗██║██║   ██║██╔══██╗██╔══██║   ██║   ██║   ██║██║     ██╔══██║   ██║   ██║██║   ██║██║╚██╗██║╚════██║╚═╝",
                "╚██████╗╚██████╔╝██║ ╚████║╚██████╔╝██║  ██║██║  ██║   ██║   ╚██████╔╝███████╗██║  ██║   ██║   ██║╚██████╔╝██║ ╚████║███████║██╗",
                " ╚═════╝ ╚═════╝ ╚═╝  ╚═══╝ ╚═════╝ ╚═╝  ╚═╝╚═╝  ╚═╝   ╚═╝    ╚═════╝ ╚══════╝╚═╝  ╚═╝   ╚═╝   ╚═╝ ╚═════╝ ╚═╝  ╚═══╝╚══════╝╚═╝",
            };
            CWriteFunc.RenderCenteredStrings(congrats);

            // Pause for 3 seconds before exiting
            Thread.Sleep(3000);
            // TODO: something to do after finishing the game

            // replace these
            Console.WriteLine("press anything to return to menu");
            string[] menuReturn =
            {
                "                      ┏┓                                     ",
                "                      ┃┃┏┓┏┓┏┏                               ",
                "                      ┣┛┛ ┗ ┛┛                               ",
                "                   ┏┓     ┓ •                             ",
                "                   ┣┫┏┓┓┏╋┣┓┓┏┓┏┓                         ",
                "                   ┛┗┛┗┗┫┗┛┗┗┛┗┗┫                         ",
                "                        ┛       ┛                         ",
                "                        ┏┳┓                                    ",
                "                         ┃┏┓                                   ",
                "                         ┻┗┛                                   ",
                "             ┳┓                ┳┳┓                  ",
                "             ┣┫┏┓╋┓┏┏┓┏┓  ╋┏┓  ┃┃┃┏┓┏┓┓┏            ",
                "             ┛┗┗ ┗┗┻┛ ┛┗  ┗┗┛  ┛ ┗┗ ┛┗┗┻•           ",
            };
            CWriteFunc.RenderCenteredStrings(menuReturn);
            bgPlayer = new Player();
            bgPlayer.Play("./Files/HomeMenubg.wav");

            Console.ReadKey();
        }

        internal static void GameOver()
        {
            // TODO: something prettier
            Console.Clear();

            string[] faceGameOver =
            {
                "⠀⠀⠀⠀⠀⢀⣴⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣶⡄⠀⠀⠀⠀⠀",
                "⠀⠀⠀⠀⢀⡿⣷⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣦⠄⠀⠀⠀",
                "⠀⠀⠀⠀⢰⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡆⠀⠀⠀",
                "⠀⠀⠀⢠⣿⣇⢿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣇⠀⠀⠀",
                "⠀⠀⠀⠀⣻⣿⣿⣿⣿⡿⡿⣿⣿⣿⣿⣿⣿⣿⣿⣯⣟⣿⣿⣿⣿⣷⣭⠀⠀⠀",
                "⠀⠀⠀⠀⣻⣿⠟⠛⠉⠁⠈⠉⠻⢿⣿⣿⣿⡟⠛⠂⠉⠁⠈⠉⠁⠻⣿⠀⠀⠀",
                "⠀⠀⠀⠀⢾⠀⠀⣠⠄⠻⣆⠀⠈⠠⣻⣿⣟⠁⠀⠀⠲⠛⢦⡀⠀⠠⠁⠀⠀⠀",
                "⠀⠀⠀⠀⢱⣄⡀⠘⠀⠸⠉⠀⠀⢰⣿⣷⣿⠂⢀⠀⠓⡀⠞⠀⢀⣀⠀⠀⠀⠀",
                "⠀⠀⠀⠀⠠⣿⣷⣶⣶⣶⣾⣿⠀⠸⣿⣿⣿⣶⣿⣧⣴⣴⣶⣶⣿⡟⠀⠀⠀⠀",
                "⠀⠀⠀⠀⠀⢿⣿⣿⣿⣿⣿⣏⠇⠄⣿⣿⣿⣿⣿⣿⣿⣿⣿⣟⣾⠁⠀⠀⠀⠀",
                "⠀⠀⠀⠀⠀⢺⣿⣿⣿⣿⣟⡿⠂⠈⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⠑⠀⠀⠀⠀⠀",
                "⠀⠀⠀⠀⠀⠈⣿⣿⣿⣿⣿⠀⠀⠈⠿⣿⣿⣿⣿⣿⣿⣿⣿⠁⠀⠀⠀⠀⠀⠀",
                "⠀⠀⠀⠀⠀⠄⢻⣿⣿⣿⡗⠀⠀⠀⠀⠈⠀⢨⣿⣿⣿⣿⣿⠀⠀⠀⠀⠀⠀⠀",
                "⠀⠀⠀⠀⠀⠀⡞⠷⠿⠿⠀⠀⠀⠀⢀⣘⣤⣿⣿⣿⣿⣿⡏⠀⠀⠀⠀⠀⠀⠀",
                "⠀⠀⠀⠀⠀⠀⠼⠉⠀⠀⠀⠀⠀⠚⢻⠿⠟⠓⠛⠂⠉⠉⠁⠀⡁⠀⠀⠀⠀⠀",
                "⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣠⣼⠀⠀⠀⠀⠀⠀",
                "⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢰⣿⡿⡀⠀⠀⠀⠀⠀",
                "⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢠⢾⠻⠌⣄⡁⠀⠀⠀⠀⠀",
                "⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢠⣀⣀⣀⡠⡲⠞⡁⠈⡈⣿⠀⠀⠀⠀⠀⠀",
                "⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠐⠘⠛⠻⢯⠟⠩⠀⠀⢠⣣⠈⠀⠀⠀⠀⠀⠀",
                "⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⡀⠄⠂⣰⣧⣾⠶⠀⠀⠀⠀⠀⠀⠀",
            };

            string[] gameOver =
            {
                " ░▒▓██████▓▒░  ░▒▓██████▓▒░ ░▒▓██████████████▓▒░ ░▒▓████████▓▒░ ",
                "░▒▓█▓▒░░▒▓█▓▒░░▒▓█▓▒░░▒▓█▓▒░░▒▓█▓▒░░▒▓█▓▒░░▒▓█▓▒░░▒▓█▓▒░        ",
                "░▒▓█▓▒░       ░▒▓█▓▒░░▒▓█▓▒░░▒▓█▓▒░░▒▓█▓▒░░▒▓█▓▒░░▒▓█▓▒░        ",
                "░▒▓█▓▒▒▓███▓▒░░▒▓████████▓▒░░▒▓█▓▒░░▒▓█▓▒░░▒▓█▓▒░░▒▓██████▓▒░   ",
                "░▒▓█▓▒░░▒▓█▓▒░░▒▓█▓▒░░▒▓█▓▒░░▒▓█▓▒░░▒▓█▓▒░░▒▓█▓▒░░▒▓█▓▒░        ",
                "░▒▓█▓▒░░▒▓█▓▒░░▒▓█▓▒░░▒▓█▓▒░░▒▓█▓▒░░▒▓█▓▒░░▒▓█▓▒░░▒▓█▓▒░        ",
                " ░▒▓██████▓▒░ ░▒▓█▓▒░░▒▓█▓▒░░▒▓█▓▒░░▒▓█▓▒░░▒▓█▓▒░░▒▓████████▓▒░ ",
                "                                                                ",
                "                                                                ",
                " ░▒▓██████▓▒░ ░▒▓█▓▒░░▒▓█▓▒░░▒▓████████▓▒░░▒▓███████▓▒░         ",
                "░▒▓█▓▒░░▒▓█▓▒░░▒▓█▓▒░░▒▓█▓▒░░▒▓█▓▒░       ░▒▓█▓▒░░▒▓█▓▒░        ",
                "░▒▓█▓▒░░▒▓█▓▒░ ░▒▓█▓▒▒▓█▓▒░ ░▒▓█▓▒░       ░▒▓█▓▒░░▒▓█▓▒░        ",
                "░▒▓█▓▒░░▒▓█▓▒░ ░▒▓█▓▒▒▓█▓▒░ ░▒▓██████▓▒░  ░▒▓███████▓▒░         ",
                "░▒▓█▓▒░░▒▓█▓▒░  ░▒▓█▓▓█▓▒░  ░▒▓█▓▒░       ░▒▓█▓▒░░▒▓█▓▒░        ",
                "░▒▓█▓▒░░▒▓█▓▒░  ░▒▓█▓▓█▓▒░  ░▒▓█▓▒░       ░▒▓█▓▒░░▒▓█▓▒░        ",
                " ░▒▓██████▓▒░    ░▒▓██▓▒░   ░▒▓████████▓▒░░▒▓█▓▒░░▒▓█▓▒░        ",
            };

            string[] menuReturn =
            {
                "                      ┏┓                            ",
                "                      ┃┃┏┓┏┓┏┏                      ",
                "                      ┣┛┛ ┗ ┛┛                      ",
                "                   ┏┓     ┓ •                       ",
                "                   ┣┫┏┓┓┏╋┣┓┓┏┓┏┓                   ",
                "                   ┛┗┛┗┗┫┗┛┗┗┛┗┗┫                   ",
                "                        ┛       ┛                   ",
                "                        ┏┳┓                         ",
                "                         ┃┏┓                        ",
                "                         ┻┗┛                        ",
                "             ┳┓                ┳┳┓                  ",
                "             ┣┫┏┓╋┓┏┏┓┏┓  ╋┏┓  ┃┃┃┏┓┏┓┓┏            ",
                "             ┛┗┗ ┗┗┻┛ ┛┗  ┗┗┛  ┛ ┗┗ ┛┗┗┻•           ",
            };
            CWriteFunc.RenderCenteredStrings(faceGameOver);
            Player gameOverSound = new Player();
            gameOverSound.Play("./Files/GameOver.wav");
            Thread.Sleep(5000);
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkRed;
            CWriteFunc.RenderCenteredStrings(gameOver);
            // Pause for 3 seconds before exiting

            Thread.Sleep(5000);
            Console.Clear();
            CWriteFunc.RenderCenteredStrings(menuReturn);
            Console.ResetColor();
            Console.ReadKey();

            int width = Console.WindowWidth;
            int height = Console.WindowHeight;
            float fallingSpeedMilliseconds = 0.10f;

            CWriteFunc.FallingBloodTransition(fallingSpeedMilliseconds);
            bgPlayer = new Player();
            bgPlayer.Play("./Files/HomeMenubg.wav");
        }

        internal static void StartMenu()
        {
            string[] instructGame =
            [
                "┓       ┓  ┓",
                "┃ ┏┓┓┏┏┓┃  ┃",
                "┗┛┗ ┗┛┗ ┗  ┻",
                "┏┓         ┓",
                "  ┃ ┏┓┏┓╋┏┓┏┓┃┏•",
                "  ┗┛┗┛┛┗┗┛ ┗┛┗┛•",
                "┳┳┏┓┏┓  ┓ ┏ ┏┓ ┏┓ ┳┓  ┏┳┓   ┳┳┓      ",
                "┃┃┗┓┣   ┃┃┃ ┣┫ ┗┓ ┃┃   ┃┏┓  ┃┃┃┏┓┓┏┏┓",
                "┗┛┗┛┗┛  ┗┻┛╻┛┗╻┗┛╻┻┛   ┻┗┛  ┛ ┗┗┛┗┛┗ ",
            ];
            Console.Clear();
            gameIntroLore();
            Thread.Sleep(3000);
            Console.Clear();
            CWriteFunc.RenderCenteredStrings(instructGame);

            // pause for 3 seconds before proceeding
            Thread.Sleep(3000);

            string[] pressStart =
            [
                "┏┓        ┏┓     ┓ •     ┏┳┓   ┏┓      ",
                "┃┃┏┓┏┓┏┏  ┣┫┏┓┓┏╋┣┓┓┏┓┏┓  ┃┏┓  ┗┓╋┏┓┏┓╋",
                "┣┛┛ ┗ ┛┛  ┛┗┛┗┗┫┗┛┗┗┛┗┗┫  ┻┗┛  ┗┛┗┗┻┛ ┗",
            ];
            Console.Clear();
            CWriteFunc.RenderCenteredStrings(pressStart);
            // wait for user input to start
            Console.ReadKey();
            int width = Console.WindowWidth;
            int height = Console.WindowHeight;
            float fallingSpeedMilliseconds = 0.000f;
            CWriteFunc.FallingBloodTransition(fallingSpeedMilliseconds);
            StopIntroMusic();
        }
    }
}
