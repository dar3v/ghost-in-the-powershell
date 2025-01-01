using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Numerics;
using System.Text;
using NetCoreAudio;
using WriteFunc;

namespace RaycasterCS
{
    class InitGame
    {
        // NOTE:
        // these values must be set before calling 'Engine.Start()'
        // otherwise placeholder values will be set

        // maze map
        // make sure `initMapWidth` and `initMapHeight` checks out with MazeMap

        public static int initMapWidth { get; set; }
        public static int initMapHeight { get; set; }
        public static string[]? MazeMap { get; set; }

        // timespan
        public static int initMazeTime { get; set; }

        // initial player coords
        public static float initPlayerX { get; set; }
        public static float initPlayerY { get; set; }

        public static float initPlayerA { get; set; }
    }

    // ---class Engine---
    // the raycasting engine
    class Engine
    {
        public static Player? statueFound;
        internal int Start()
        {
            // --initialize variables--
            // booleans
            bool bForDevOnly = false; // map and coordinate
            bool bConsoleLargeEnough = true;
            bool bGameOver = false;
            bool bGameFinish = false;
            bool bQuit = false;
            bool bWarningSound = false;

            // raycasting variables
            float fFOV = MathF.PI / 4.0f;
            float fDepth = 16.0f;

            // console game screen variables
            float fSpeed = 75.0f;
            float fRotationSpeed = 0.25f;

            if (OperatingSystem.IsWindows())
            {
                fSpeed = 3.0f;
                fRotationSpeed = 0.25f;
            }

            int nConsoleWidth = Console.WindowWidth;
            int nConsoleHeight = Console.WindowHeight;

            int nScreenWidth = 125;
            int nScreenHeight = 40;

            char[,] screen = new char[nScreenWidth, nScreenHeight];
            float[,] depthBuffer = new float[nScreenWidth, nScreenHeight];

            // make sure to assign these variables in `InitGame` class

            float fPlayerX = InitGame.initPlayerX;
            float fPlayerY = InitGame.initPlayerY;
            float fPlayerA = InitGame.initPlayerA;

            int nMapWidth = InitGame.initMapWidth;
            int nMapHeight = InitGame.initMapHeight;

            string[] map = new string[nMapHeight];

            // handle null `InitGame.MazeMap`
            // just use a boilerplate map if its null
            if (InitGame.MazeMap is null)
            {
                map = [
                  "###############",
                  "#.............#",
                  "#...@....####.#",
                  "#.............#",
                  "#.#.#....##.#.#",
                  "#.#.##........#",
                  "###.##...#....#",
                  "##............#",
                  "#####.........#",
                  "#......@......#",
                  "#.........##..#",
                  "#..#########..#",
                  "#..###...@....#",
                  "#.............#",
                  "###############",
                ];

                nMapHeight = 16;
                nMapWidth = 16;

                fPlayerX = 1.5f;
                fPlayerY = 1.5f;
            }
            else map = InitGame.MazeMap;

            // statues
            List<(float x, float y)> statues = new();
            for (int y = 0; y < map.Length; y++) // get all statue position from map
            {
                for (int x = 0; x < map[0].Length; x++)
                    if (map[y][x] == '@') statues.Add((x + .5f, y + .5f));
            }

            // near statue coordinate
            List<List<(int x, int y)>> mazeFinish = new();
            foreach (var statue in statues)
            {
                List<(int x, int y)> tmp = new();
                for (int y = (int)statue.y - 1; y <= (int)statue.y + 1; y++)
                {
                    for (int x = (int)statue.x - 1; x <= (int)statue.x + 1; x++)
                    {
                        // ensure coords around (x, y) is inbounds
                        bool y_bound = y >= 0 && y < nMapHeight;
                        bool x_bound = x >= 0 && x < nMapWidth;

                        if (x_bound && y_bound) tmp.Add((x, y));
                    }
                }
                mazeFinish.Add(tmp);
            }
            bool[] foundStatues = new bool[statues.Count];

            // maze time span
            int nMazeTimeSpan = 0;
            if (InitGame.initMazeTime == 0) nMazeTimeSpan = 60;
            else nMazeTimeSpan = InitGame.initMazeTime;
            TimeSpan mazeTimeSpan = TimeSpan.FromSeconds(nMazeTimeSpan);

            // --Start Engine--
            Console.Clear();
            Stopwatch stopwatch = Stopwatch.StartNew();
            Stopwatch mazeTime = Stopwatch.StartNew();

            // --- the game loop ---
            while (!bGameOver && !bGameFinish && !bQuit)
            {
                E_HandleConsoleSize(); // handle invalid console sizes

                E_CheckGame(); // check certain player conditions
                E_UpdateFrame(); // update frames
                E_RenderFrame(); // render frames
            }
            // exit
            stopwatch.Stop();
            mazeTime.Stop();

            if (bGameOver) return 1;
            else if (bGameFinish) return 2;
            return 0;

            // update game function
            // checks certain conditions the player has met

            void E_CheckGame()
            {
                // check if time is up (game over)
                if (mazeTime.Elapsed > mazeTimeSpan)
                {
                    mazeTime.Stop();
                    bGameOver = true;
                }

                TimeSpan remainingTime = mazeTimeSpan - mazeTime.Elapsed;
                if (remainingTime.TotalSeconds <= 10 && !bWarningSound)
                {
                    bWarningSound = true;
                    Player warningSound = new();
                    if (warningSound != null) warningSound.Play("./Files/GameAboutToEnd.wav");
                }

                // winning condition
                if (foundStatues.All(x => x)) bGameFinish = true;

                // check statues found
                for (int i = 0; i < mazeFinish.Count; i++)
                {
                    if (foundStatues[i]) continue; // `continue` loop if ith statue is already found

                    for (int j = 0; j < mazeFinish[i].Count; j++)
                    {
                        bool bStatueProcd = (int)fPlayerX == mazeFinish[i][j].x && (int)fPlayerY == mazeFinish[i][j].y;
                        if (bStatueProcd)
                        {
                            foundStatues[i] = true;
                            statueFound = new Player();

                            Random randomSound = new Random();
                            int soundIndex = randomSound.Next(0, 8);
                            string selectedSound = "./Files/Statue_Sounds/";

                            switch (soundIndex)
                            {
                                case 0: selectedSound += "StatueVoice1.wav"; break;
                                case 1: selectedSound += "StatueVoice2.wav"; break;
                                case 2: selectedSound += "StatueVoice3.wav"; break;
                                case 3: selectedSound += "StatueVoice4.wav"; break;
                                case 4: selectedSound += "StatueVoice5.wav"; break;
                                case 5: selectedSound += "StatueVoice6.wav"; break;
                                case 6: selectedSound += "StatueVoice7.wav"; break;
                                case 7: selectedSound += "StatueVoice8.wav"; break;
                                case 8: selectedSound += "StatueVoice9.wav"; break;
                                case 9: selectedSound += "StatueVoice10.wav"; break;
                            }
                            statueFound.Play(selectedSound);
                        }
                    }
                }
            }

            // update frame function
            // mostly for player controls
            void E_UpdateFrame()
            {
                bool up = false, down = false, sleft = false, sright = false, lleft = false, lright = false;

                // listen for keyboard input from player
                // (syncrhonous)
                while (Console.KeyAvailable)
                {
                    switch (Console.ReadKey(true).Key)
                    {
                        // forward/backward
                        case ConsoleKey.W:
                            up = true;
                            break;
                        case ConsoleKey.S:
                            down = true;
                            break;

                        // look to left/right
                        case ConsoleKey.A:
                            lleft = true;
                            break;
                        case ConsoleKey.D:
                            lright = true;
                            break;

                        // strafing
                        case ConsoleKey.Q:
                            sleft = true;
                            break;
                        case ConsoleKey.E:
                            sright = true;
                            break;
                        // etc
                        case ConsoleKey.Escape:
                            CloseRequest();
                            break;
                    }
                }

                // timer to tie framerate to time instead of directly from thread speed
                float fIngameTime = (float)stopwatch.Elapsed.TotalSeconds;
                stopwatch.Restart();

                // HACK: use Window's API to get keyboard information directly from hardware
                // asynchronous
                if (OperatingSystem.IsWindows())
                {
                    up = up || Win.GetAsyncKeyState('W') is not 0; // forward
                    down = down || Win.GetAsyncKeyState('S') is not 0; // backward

                    lleft = lleft || Win.GetAsyncKeyState('A') is not 0; // forward
                    lright = lright || Win.GetAsyncKeyState('D') is not 0; // backward

                    sleft = sleft || Win.GetAsyncKeyState('Q') is not 0; // left
                    sright = sright || Win.GetAsyncKeyState('E') is not 0; // right
                }

                // TODO: get some library for getting `GetAsyncKeyState` for Linux as well

                // movement logic
                // handle forward movement & collision
                if (up && !down && !sleft && !sright)
                {
                    fPlayerX += MathF.Cos(fPlayerA) * fSpeed * fIngameTime;
                    fPlayerY += MathF.Sin(fPlayerA) * fSpeed * fIngameTime;
                    if (map[(int)fPlayerY][(int)fPlayerX] == '#')
                    {
                        fPlayerX -= MathF.Cos(fPlayerA) * fSpeed * fIngameTime;
                        fPlayerY -= MathF.Sin(fPlayerA) * fSpeed * fIngameTime;
                    }
                }
                // handle backward movement & collision
                if (down && !up && !sleft && !sright)
                {
                    fPlayerX -= MathF.Cos(fPlayerA) * fSpeed * fIngameTime;
                    fPlayerY -= MathF.Sin(fPlayerA) * fSpeed * fIngameTime;
                    if (map[(int)fPlayerY][(int)fPlayerX] == '#')
                    {
                        fPlayerX += MathF.Cos(fPlayerA) * fSpeed * fIngameTime;
                        fPlayerY += MathF.Sin(fPlayerA) * fSpeed * fIngameTime;
                    }
                }

                // handle strafe left movement & collision
                if (sleft && !sright && !up && !down)
                {
                    fPlayerX -= MathF.Cos(fPlayerA) * fSpeed * fIngameTime;
                    fPlayerY += MathF.Sin(fPlayerA) * fSpeed * fIngameTime;
                    if (map[(int)fPlayerY][(int)fPlayerX] == '#')
                    {
                        fPlayerX += MathF.Cos(fPlayerA) * fSpeed * fIngameTime;
                        fPlayerY -= MathF.Sin(fPlayerA) * fSpeed * fIngameTime;
                    }
                }

                // handle strafe right movement & collision
                if (sright && !sleft && !up && !down)
                {
                    fPlayerX += MathF.Cos(fPlayerA) * fSpeed * fIngameTime;
                    fPlayerY -= MathF.Sin(fPlayerA) * fSpeed * fIngameTime;
                    if (map[(int)fPlayerY][(int)fPlayerX] == '#')
                    {
                        fPlayerX -= MathF.Cos(fPlayerA) * fSpeed * fIngameTime;
                        fPlayerY += MathF.Sin(fPlayerA) * fSpeed * fIngameTime;
                    }
                }

                // handle look left movement & collision
                if (lleft && !lright)
                {
                    fPlayerA -= fSpeed * fRotationSpeed * fIngameTime;
                    if (fPlayerA < 0)
                    {
                        fPlayerA %= MathF.PI * 2;
                        fPlayerA += MathF.PI * 2;
                    }
                }
                // handle look right movement & collision
                if (lright && !lleft)
                {
                    fPlayerA += fSpeed * fRotationSpeed * fIngameTime;
                    if (fPlayerA > MathF.PI * 2)
                    {
                        fPlayerA %= MathF.PI * 2;
                    }
                }

                void CloseRequest()
                {
                    mazeTime.Stop(); // pause maze timer
                    Console.Clear();
                    string[] yesNo =
                    {

" █████╗ ██████╗ ███████╗    ██╗   ██╗ ██████╗ ██╗   ██╗    ███████╗██╗   ██╗██████╗ ███████╗██████╗ ",
"██╔══██╗██╔══██╗██╔════╝    ╚██╗ ██╔╝██╔═══██╗██║   ██║    ██╔════╝██║   ██║██╔══██╗██╔════╝╚════██╗",
"███████║██████╔╝█████╗       ╚████╔╝ ██║   ██║██║   ██║    ███████╗██║   ██║██████╔╝█████╗    ▄███╔╝",
"██╔══██║██╔══██╗██╔══╝        ╚██╔╝  ██║   ██║██║   ██║    ╚════██║██║   ██║██╔══██╗██╔══╝    ▀▀══╝ ",
"██║  ██║██║  ██║███████╗       ██║   ╚██████╔╝╚██████╔╝    ███████║╚██████╔╝██║  ██║███████╗  ██╗   ",
"╚═╝  ╚═╝╚═╝  ╚═╝╚══════╝       ╚═╝    ╚═════╝  ╚═════╝     ╚══════╝ ╚═════╝ ╚═╝  ╚═╝╚══════╝  ╚═╝   ",
"                                                                                                    ",
"                                    ██╗   ██╗ ██╗███╗   ██╗                                          ",
"                                    ╚██╗ ██╔╝██╔╝████╗  ██║                                          ",
"                                     ╚████╔╝██╔╝ ██╔██╗ ██║                                          ",
"                                      ╚██╔╝██╔╝  ██║╚██╗██║                                          ",
"                                       ██║██╔╝   ██║ ╚████║                                          ",
"                                       ╚═╝╚═╝    ╚═╝  ╚═══╝                                          "


                    };
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    CWriteFunc.RenderCenteredStrings(yesNo);

                    switch (Console.ReadKey(true).Key)
                    {
                        case ConsoleKey.Y: bQuit = true; break;
                        default:
                            Console.ResetColor();
                            bQuit = false;
                            mazeTime.Start();
                            break;
                    }
                }
            }

            // render frame function
            // 90% of the raycasting tech goodness
            void E_RenderFrame()
            {
                // sets default `depthBuffer` value
                for (int y = 0; y < nScreenHeight; y++)
                    for (int x = 0; x < nScreenWidth; x++)
                    {
                        depthBuffer[x, y] = float.MaxValue;
                    }

                // per each character in console line
                for (int x = 0; x < nScreenWidth; x++)
                {
                    // For each column, calculate the projected ray angle into world space
                    float fRayAngle = (fPlayerA - fFOV / 2.0f) + ((float)x / (float)nScreenWidth) * fFOV;
                    float fDistanceToWall = 0;
                    bool bHitWall = false;
                    bool bBoundary = false;

                    float fEyeX = MathF.Cos(fRayAngle);
                    float fEyeY = MathF.Sin(fRayAngle);

                    while (!bHitWall && fDistanceToWall < fDepth)
                    {
                        fDistanceToWall += 0.1f;

                        int nTestX = (int)(fPlayerX + fEyeX * fDistanceToWall);
                        int nTestY = (int)(fPlayerY + fEyeY * fDistanceToWall);

                        // test if ray casted is out of bounds
                        if (nTestY < 0 || nTestY >= nMapWidth || nTestY < 0 || nTestX >= nMapHeight)
                        {
                            bHitWall = true;
                            fDistanceToWall = fDepth;
                        }
                        else // ray inbounds so test to see if the ray cell is a wall block
                        {
                            if (map[nTestY][nTestX] == '#')
                            {
                                bHitWall = true;
                                List<(float, float)> p = new();
                                for (int tx = 0; tx < 2; tx++)
                                    for (int ty = 0; ty < 2; ty++)
                                    {
                                        float vy = (float)nTestY + ty - fPlayerY;
                                        float vx = (float)nTestX + tx - fPlayerX;
                                        float d = MathF.Sqrt(vx * vx + vy * vy);
                                        float dot = (fEyeX * vx / d) + (fEyeY * vy / d);
                                        p.Add((d, dot));
                                    }
                                p.Sort((a, b) => a.Item1.CompareTo(b.Item1));
                                float fBound = 0.01f;
                                if (MathF.Acos(p[0].Item2) < fBound) bBoundary = true;
                            }
                        }

                        // caclulate distance to ceiling and floor
                        int nCeiling = (int)((float)(nScreenHeight / 2.0f) - nScreenHeight / ((float)fDistanceToWall));
                        int nFloor = nScreenHeight - nCeiling;

                        char nShade = ' ';

                        // shading for walls
                        if (fDistanceToWall <= fDepth / 4.0f) nShade = '█'; // nearest
                        else if (fDistanceToWall < fDepth / 3.0f) nShade = '▓';
                        else if (fDistanceToWall < fDepth / 2.0f) nShade = '▒';
                        else if (fDistanceToWall < fDepth) nShade = '░';
                        else nShade = ' '; // farthest

                        if (bBoundary) nShade = '║';

                        for (int y = 0; y < nScreenHeight; y++)
                        {
                            depthBuffer[x, y] = fDistanceToWall;

                            if (y <= nCeiling)
                            {
                                screen[x, y] = ' ';
                            }
                            else if (y > nCeiling && y <= nFloor)
                            {
                                screen[x, y] = nShade;
                            }
                            else
                            {
                                float b = 1.0f - ((float)y - nScreenHeight / 2.0f) / ((float)nScreenHeight / 2.0f); if (b < 0.25) nShade = '#';
                                else if (b < .5) nShade = 'x';
                                else if (b < .75) nShade = '.';
                                else if (b < .9) nShade = '-';
                                else nShade = ' ';
                                screen[x, y] = nShade;
                            }
                        }
                    }
                }

                float fFOVAngle = fPlayerA - fFOV / 2;
                if (fFOVAngle < 0) fFOVAngle += MathF.PI * 2;

                // render statues
                for (int i = 0; i < statues.Count; i++)
                {
                    float fAngle = MathF.Atan2(statues[i].y - fPlayerY, statues[i].x - fPlayerX);
                    if (fAngle < 0) fAngle += 2f * MathF.PI;

                    float fDistance = Vector2.Distance(
                      new(fPlayerX, fPlayerY), new(statues[i].x, statues[i].y));

                    int nCeiling = (int)((float)(nScreenHeight / 2.0f) - nScreenHeight / ((float)fDistance));
                    int nFloor = nScreenHeight - nCeiling;

                    string[] statueSprite = [];
                    if (foundStatues[i]) // if statue is already found
                    {
                        switch (fDistance)
                        {
                            case float n when n <= 02f:
                                statueSprite = statueFoundSprite1; break;
                            case float n when n <= 04f:
                                statueSprite = statueFoundSprite2; break;
                            case float n when n <= 06f:
                                statueSprite = statueFoundSprite3; break;
                            case float n when n <= 08f:
                                statueSprite = statueFoundSprite4; break;
                            default:
                                statueSprite = []; break;
                        }
                    }
                    else // if statue is not yet found
                        switch (fDistance)
                        {
                            case float n when n <= 02f:
                                statueSprite = statueSprite1; break;
                            case float n when n <= 04f:
                                statueSprite = statueSprite2; break;
                            case float n when n <= 06f:
                                statueSprite = statueSprite3; break;
                            case float n when n <= 10f:
                                statueSprite = statueSprite4; break;
                            default:
                                statueSprite = []; break;
                        }

                    float diff = fAngle < fFOVAngle && fFOVAngle - 2f * MathF.PI + fFOV > fAngle ? fAngle + 2f * MathF.PI - fFOVAngle : fAngle - fFOVAngle;
                    float ratio = diff / fFOV;
                    int statueScreenX = (int)(nScreenWidth * ratio);
                    int statueScreenY = Math.Min(nFloor, screen.GetLength(1));

                    for (int y = 0; y < statueSprite.Length; y++)
                    {
                        for (int x = 0; x < statueSprite[y].Length; x++)
                        {
                            if (statueSprite[y][x] is not '`')
                            {
                                int screenX = x - statueSprite[y].Length / 2 + statueScreenX;
                                int screenY = y - statueSprite.Length + statueScreenY;
                                if (0 <= screenX && screenX <= nScreenWidth - 1 && 0 <= screenY && screenY <= nScreenHeight - 1 && depthBuffer[screenX, screenY] > fDistance)
                                {
                                    screen[screenX, screenY] = statueSprite[y][x];
                                    depthBuffer[screenX, screenY] = fDistance;
                                }
                            }
                        }
                    }
                }

                // print debug mode (map, coord)
                if (bForDevOnly)
                {
                    // coords
                    string[] coords = [$"{fPlayerX}", $"{fPlayerY}", $"{fPlayerA}"];

                    for (int i = 0; i < coords.Length; i++)
                    {
                        for (int j = 0; j < coords[i].Length; j++)
                        {
                            screen[nScreenWidth - coords[i].Length + j, i] = coords[i][j];
                        }
                    }

                    for (int y = 0; y < map.Length; y++)
                    {
                        for (int x = 0; x < map[y].Length; x++)
                        {
                            screen[x, y] = map[y][x];
                        }
                    }
                    screen[(int)fPlayerX, (int)fPlayerY] = 'P';
                }

                // render frame
                StringBuilder render = new();
                for (int y = 0; y < screen.GetLength(1); y++)
                {
                    // render `screen` per line
                    for (int x = 0; x < screen.GetLength(0); x++)
                    {
                        int center = ((nConsoleWidth - screen.GetLength(0)) / 2);
                        if (x == 0) render.Append(' ', center); // center screen

                        // render character
                        render.Append(screen[x, y]);
                    }
                    // append after last char in console line
                    if (y < screen.GetLength(1) - 1) render.AppendLine();
                }
                Console.SetCursorPosition(0, 0);
                Console.Write(render); // voila!
                DisplayStats(); // display stats below game screen


                // display stats
                void DisplayStats()
                {
                    float nTimeLeft = (float)nMazeTimeSpan - (float)mazeTime.Elapsed.TotalSeconds;
                    int nStatueCount = foundStatues.Count(x => x); // count all true in foundStatues bool[]
                    int nStatueCountAll = foundStatues.Length;

                    string stats =
                     $"👻 Statues: {nStatueCount}/{nStatueCountAll}\t Time: {nTimeLeft:F2}s 👻";

                    // center
                    int center = (nConsoleWidth - stats.Length) / 2;

                    StringBuilder display = new();
                    display.Append(' ', center);

                    foreach (var stat in stats) display.Append(stat);

                    Console.Write("\n" + display);
                }
            }

            // handle invalid console size
            void E_HandleConsoleSize()
            {
                while (true)
                {
                    // first, update console size if changed
                    if (nConsoleWidth != Console.WindowWidth || nConsoleHeight != Console.WindowHeight)
                    {
                        Console.Clear();
                        nConsoleWidth = Console.WindowWidth;
                        nConsoleHeight = Console.WindowHeight;
                    }

                    // handle invalid console size
                    bConsoleLargeEnough = nConsoleWidth >= nScreenWidth && nConsoleHeight >= nScreenHeight;

                    // check if console size is large enough
                    if (!bConsoleLargeEnough)
                    {
                        string[] sizeNotEnough = 
                    {
                    "Console Size Not Large Enough.",
                    "Minimum size: 125x40",
                    $"Current size: {nConsoleWidth}x{nConsoleHeight}"
                    };
                    CWriteFunc.RenderCenteredStrings(sizeNotEnough);
                    }
                    else break;
                }

            }
        }

        // sprites
        string[] statueSprite1 =
        [
      "````````````````⢀⡵⠀⠀⣡⡤⣤⣀⣙⢨⡿````````````````",
      "````````````````⠊⠐⡀⢠⡄⣴⠿⣟⣿⣷⣻⣤⣤⣿`````````````",
      "``````````````⣽⣿⣿⣿⠛⠛⠁⠀⠛⠉⠀⠀⠙⣿⣽⣿⠈⢿```````````",
      "````````````⣤⣾⠛⢻⣿⠁⠀⠀⠀⠀⠀⠀⣀⣀⣄⡘⣿⣿⣿⡈```````````",
      "```````````⠟⠫⠽⣷⣾⣿⡤⠶⠄⢆⠀⠀⣾⣽⣶⣶⠁⢿⣿⣛⣒⠞⣿`````````",
      "```````````⣀⣄⣠⣿⣿⡟⣰⣾⣿⠿⢷⠀⠉⠉⠉⠁⠀⢨⣿⣿⣿⣷⡼`````````",
      "```````````⢯⣾⣿⣷⣿⣧⠀⠀⠀⠀⠈⠁⠦⡄⠀⠀⠀⣸⣾⣷⢹⣨⣿`````````",
      "```````````⣿⣿⣿⣿⣿⣿⡆⠀⠀⠀⢦⣄⣰⠆⠀⠀⠀⣿⣿⣧⢼⣏``````````",
      "```````````⣽⠙⢻⣟⣿⣿⣷⡀⠀⠀⣀⣬⣵⡶⡶⠀⢠⣿⣿⣿⣴⡝⠾⢿````````",
      "```````````⣿⢸⡿⣿⣿⣿⣿⣧⠀⠀⠈⠠⢴⡿⠁⠀⣾⣿⢿⣿⣿⣧⣲⣸````````",
      "```````````⣿⣾⠟⣿⣿⣿⣷⣿⠳⢤⠀⠀⠀⠀⡴⠀⡿⣿⣿⣿⣿⣝⢿⡅````````",
      "```````````⡏⢫⣾⣿⣿⣿⣿⠃⠀⠀⠀⠀⠀⠀⠀⠀⢿⢺⠋⠙⣿⣿⣾⣿⣦```````",
      "```````````⠋⣩⣟⣿⣿⣿⡏⠀⠀⠀⠀⠀⠀⠀⠀⠀⠘⣿⡶⣾⣟⣱⣼⠿⠛```````",
      "``````````⢶⣿⣿⣿⡿⠟⠉⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⢷⣆⣛⣿⣿⣾⣦```````",
      "`````````⣷⣾⡿⠻⡅⠀⠄⠐⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣠⢟⡻⣿⣏⣝⣷⣶``````",
      "`````````⡿⠁⠀⠀⠹⣄⠀⠐⠀⠰⡄⠀⠀⠀⠀⠀⠀⠀⢀⡴⠋⠀⠀⣹⠿⢽⣿⣿``````",
      "```````⡿⠋⠀⠀⠀⠀⠀⠈⠧⠀⠀⠀⠉⠀⠐⠆⠀⠀⠀⠀⠋⠀⠀⠀⢸⡇⠀⠀⠉⢻``````",
      "`````⣿⠋⠀⠀⠀⡀⠀⠀⠀⠀⠀⠙⠦⡀⣆⠁⠀⠀⠀⠀⠀⠈⠀⠀⢀⡔⠀⠁⠀⠀⠀⠀⠙⢿````",
      "`````⠃⠀⠀⢀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠉⡟⠦⠐⢀⠀⠀⠀⠀⠀⠀⠈⠀⠀⠀⠀⠀⠀⠀⠀⠀⢻```",
      "```⣿⡏⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠰⠇⠀⠀⠘⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢿``",
      "```⣿⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⣿`",
      "```⣏⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣿`",
      "```⡇⠀⠄⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣿`",
      "``⣿⠃⠀⡄⠀⠀⢂⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⡀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢸`",
      "``⡇⠀⠀⠳⠀⠀⠈⠀⠀⠀⠀⠀⠀⠀⠀⠀⠘⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢸`",
      "``⣿⠀⠀⢀⠀⠀⠀⠀⠘⣆⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣿",
      "``⠇⠀⠀⠀⠀⠀⠀⠀⠀⠉⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠄⢸",
      "`⡟⠀⠀⠀⠰⡀⠀⠈⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣶⠀⠀⠀⠈⢣⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢸",
      "`⠇⠀⠀⠀⠀⢳⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢰⢹⡶⣠⡌⠳⣄⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢸",
      "⣿ ⠀⠀⠀⠁⠀⠐⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⢠⠀⠙⢻⣧⡄⠉⡄⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢸",
      "⣇⣤⣀⣀⣀⣀⣀⣀⣀⣠⣄⣀⣤⣀⣀⣀⣀⣀⣸⣎⣷⣤⣽⣧⣤⣇⣀⣀⣀⣀⣀⣀⣀⣀⣠⣄⣀⣀⣤⣄⣀⣀⣸",
        ];
        string[] statueSprite2 =
        [
          "`````````````⣠⠴⡛⠛⠛⠛⠒⣦⣄⢀`````````````",
          "``````````⢀⣠⢞⡉⠄⠀⠰⠀⠀⠀⠆⠸⢟⣦⡀```````````",
          "`````````⣴⡾⠛⣋⠍⠀⠀⣀⠀⡤⠘⣶⣼⢆⢁⡹⣆⡀`````````",
          "```````⢄⣦⣿⡗⢁⣿⣾⣶⠓⠋⠀⠁⠉⠀⠈⠻⣭⡋⠈⢧⡀````````",
          "``````⠐⠛⣾⣃⠐⠎⠈⣿⠁⠀⠀⠀⠀⢀⡤⠄⠠⢹⣯⡅⠸⣧````````",
          "``````⠀⣼⡾⠀⠀⡠⣶⣿⠂⣩⣿⣧⡐⠾⠾⠟⠃⠘⣷⢲⣮⠘⣧```````",
          "``````⣺⡏⣀⡖⣵⢟⣫⣿⠈⠉⠁⠀⠃⠀⠀⠀⠀⢀⣭⡟⠏⡫⣼⡇``````",
          "``````⣸⠱⡏⡺⢷⡿⣿⣮⡆⠀⠀⠠⣁⣠⠄⠀⠀⢸⢿⣧⢸⢹⣟⠃``````",
          "``````⠈⢷⣼⣧⠀⢻⡟⡽⣿⡄⠀⠀⠤⠤⢶⠂⠀⡏⣾⣷⣳⢑⢻⡇``````",
          "````````⠉⣿⣸⣼⣴⠇⡏⡗⡀⠀⠀⠚⠁⠀⢰⣿⣜⣿⡾⣦⠞⢷⡆`````",
          "````````⣮⡿⠛⣸⣿⣸⣿⠇⠀⠀⠀⠀⠀⠀⠸⣻⡿⢿⣿⣌⡷⣄⢻`````",
          "````````⣿⠇⢛⣿⣼⣿⠟⠀⠀⠀⠀⠀⠀⠀⠀⢿⣧⣴⠋⣩⠞⠞⢿⡖````",
          "``````⠐⣿⠗⣶⣿⣿⠿⠋⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⠻⣿⣟⣿⣔⣆⠞⡟````",
          "``````⢀⢽⣶⠟⠙⣄⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⡀⠋⠻⣿⡬⡙⣴⣷````",
          "``````⣸⠟⠁⠀⠀⠈⢂⠀⠀⠐⠄⠀⠀⠀⠀⠀⢀⠊⠀⠀⢠⠋⠑⠻⣷⠂````",
          "````⣠⡿⠁⠀⠀⠀⠀⠀⠀⠱⢄⠀⡀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠘⠀⠀⠀⠈⢯⡀```",
          "```⢰⠏⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠑⡥⠌⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠙⣆``",
          "```⣸⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠹⡆`",
          "``⢸⠇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢹`",
          "``⢼⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢾`",
          "``⡿⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢸`",
          "``⡇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢸`",
          "`⢸⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣿",
          "`⡼⡀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⡀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢸",
          "⢹⡧⠀⠀⠀⠀⠀⠀⠀⠐⡀⠀⠀⠀⠀⠀⢧⡀⠐⢄⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢸",
          "⢸⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠘⠒⣯⡄⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢸",
          "⣏⣀⣀⣀⣀⣀⣀⣀⣀⣀⣀⣀⣀⣀⣀⣸⣈⣒⣘⣗⣰⣁⣀⣀⣀⣀⣀⣀⣀⣀⣀⣀⣀⣀⣀⣸",
        ];
        string[] statueSprite3 =
          [
          "````````````⣀⣠⣀⡀`````````````",
          "``````````⡴⢊⠋⡀⠀⠉⡓⡦⣄``````````",
          "````````⣼⠿⢒⠂⠀⢀⢂⢐⢤⣪⠛⡳⡄````````",
          "``````⣰⣼⡗⢀⣷⣶⠓⠋⠀⠉⠀⠉⢯⣋⡘⣦```````",
          "`````⠈⣹⡯⢂⡀⢸⣇⣀⣀⠀⢠⣖⣀⡊⣿⡇⠹⣆``````",
          "`````⢰⡟⢡⢄⡾⢿⡅⠾⠛⢳⢈⠉⠁⠀⣿⣽⢳⡼⡄`````",
          "`````⣹⢰⢫⢯⡾⣯⣣⠀⠀⢔⣀⠄⠀⠀⢾⣧⡖⣾⠇`````",
          "`````⠈⢷⣟⠀⢻⢫⢻⡄⠀⠤⠴⡲⠂⢸⢾⣷⡎⡚⡇`````",
          "```````⢸⣮⢾⡟⣤⡇⠄⠀⠈⠀⠀⣿⣯⣿⡟⣖⠹⡄````",
          "``````⢸⡿⠤⣾⣮⡿⠁⠀⠀⠀⠀⠀⢱⣏⣹⠛⣾⡵⣧⠄```",
          "`````⠰⡮⢵⣾⡿⠟⠁⠀⠀⠀⠀⠀⠀⠀⠻⣿⣺⣧⣄⢸⠃```",
          "`````⢠⣧⠞⠉⢆⠀⠀⠀⠀⠀⠀⠀⠀⠀⡐⠋⢻⣷⣝⡞⠅```",
          "````⣠⠜⠁⠀⠀⠈⠢⡀⠈⢀⠀⠀⠀⠀⠊⠀⠀⡎⠀⠙⢿⠄```",
          "```⡴⠁⠀⠀⠀⠀⠀⠀⠈⠢⢀⠂⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠙⣄``",
          "``⢄⡇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠘⡄`",
          "``⢸⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣹`",
          "``⡍⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢹`",
          "``⠇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢸`",
          "`⢸⠇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⡇",
          "⢠⡜⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⡀⠀⠀⠄⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣧",
          "⠀⠋⠀⠀⠀⠀⠀⠀⠃⠀⠀⠀⠀⠣⢠⠁⠄⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⡏",
          "⢸⣀⣀⣀⣀⣀⣀⣀⣀⣀⣀⣀⣸⣐⣈⣏⣠⣀⣀⣀⣀⣀⣀⣀⣀⣀⣀⣀⡻",
              ];
        string[] statueSprite4 =
        [
            "````````⣀⡤⠤⠤⢄`````````",
            "``````⣠⠔⡉⠀⠀⣀⣂⡑⠦```````",
            "`````⣤⡟⣸⣵⠚⠃⠁⠁⠳⣅⠱⡄`````",
            "````⢠⡿⠀⣦⡾⣐⢢⠰⠷⠆⢿⣠⡱⡄````",
            "````⠘⡄⢯⢽⣶⠈⠀⡈⡀⠀⣼⣧⢩⡇````",
            "````⠘⠮⣃⢋⡞⡆⠀⠰⠒⢠⣹⣷⡙⣃````",
            "`````⢠⣟⡽⣽⠇⠀⠀⠀⢸⡷⢿⣜⣚⡆```",
            "````⢠⠫⣴⡿⠏⠀⠀⠀⠀⠀⠳⣷⡌⡠⢟```",
            "````⢰⠜⠉⠄⠁⠠⠀⠀⠀⠀⠀⠉⠗⢾⡛```",
            "```⡴⠃⠀⠀⠀⠑⠀⠁⠀⠀⠀⠀⠈⠀⠀⠑```",
            "``⢸⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⡇`",
            "``⠈⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣷`",
            "``⠇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢹`",
            "`⢰⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠘⡅",
            "`⡏⠀⠀⠀⠀⠀⠀⠀⠀⣂⡠⠁⠀⠀⠀⠀⠀⠀⠀⠀⠷",
            "⢠⣁⣀⣀⣀⣀⣀⣀⣀⣠⣈⣳⣀⣀⣀⣀⣀⣀⣀⣀⣀⡇",
            ];

        string[] statueFoundSprite1 =
          [
      "````````````````OOOOOOOOOOO````````````````",
      "```````````````0OOOOOOOOO0000O`````````````",
      "``````````````OOOOOOOOOOO0000OOO```````````",
      "````````````00OOOOOOOOOOO0000OOO```````````",
      "```````````000OOOOOOOOOOO0000OOOOO`````````",
      "```````````000OOOOOOOOOOO0000OOOOO`````````",
      "```````````000OOOOOOOOOOO0000000OO`````````",
      "```````````000OOO000000000000000OO`````````",
      "```````````000OOOOOOOOOOO0000000OOO````````",
      "```````````000OOOOOOOOOOO0000000OOO````````",
      "```````````000OOOOOOOOOOO000000000O````````",
      "```````````000OOOOOOOOOOOOOO000000OO```````",
      "```````````000OOOOOOOOOOOOOO000000OO```````",
      "``````````0000OOOOOOOOOOOOOO000000OO```````",
      "`````````OO000OOOOOOOOOOOOOOOOOOOOOOO``````",
      "`````````OO000OOOOOOOOOOOOOOOOOOO0OO0``````",
      "```````OOOOO00OO0000000O0000000000000``````",
      "`````OOOOOOOOOOO0000000OOOO000000000000````",
      "`````OOOOOOOOOOO0000000OOOO000000000000o```",
      "```OOOOOOOOOOOOO0000000000000000000000000``",
      "```OOOOOOOOOOOOO00000000000000000000000000`",
      "```O00000000OO000000000OOOOOOOOOOOOOOOOOOO`",
      "```O00000000OO000000000OOOOOOOOOOOOOOOOOOO`",
      "``0O0000000000000000000OOOOOOOOOOOOOOOOOOO`",
      "``0O0000000000000000000OOOOOOOOOOOOOOOOOOO`",
      "``0O000000000000OOOOOOOOOOOOOOOO00000000000",
      "``0O000000000000OOOOOOOOOOOOOOOO00000000000",
      "`O0O000000000000OOOOOOOOOOOOOOOO00000000000",
      "`O00000000000000OOOOOOOOOOOOOOOO00000000000",
      "OO00000000000000OOOOOOOOOOOOOOOO00000000000",
      "OO00000000000000000000000000000000000000000",
          ];
        string[] statueFoundSprite2 =
          [
          "`````````````OOOOOOOO0⢀`````````````",
          "``````````⢀000000000000o0```````````",
          "`````````OOOOOOOOOOOOO0o00``````````",
          "````````0OOOOOO00000000000O`````````",
          "````````0OOOOOO00000000000OO````````",
          "```````o000OOOO00000000000OOO```````",
          "```````o000OOOO00000000000OOO0``````",
          "```````o000OOOO00000000000OOO0``````",
          "```````o000OOOOOOOOOOOO000OOO0``````",
          "````````000OOOOOOOOOOOO000OOO00`````",
          "````````000OOOOOOOOOOOO000O000OO````",
          "````````000000OOOOOOOOO0000000OO````",
          "``````⠐00000000000000000000000OO````",
          "``````⢀0000OOOOOOOOOOOO0000000OO````",
          "``````OOOOOOOOOOOOOO000OOOOOOOO⠂````",
          "````OOOOOOOOOOOOOOOO000OOOOOOOOOO```",
          "```0000000OOOOOOOOOO00000OOOOOOOOO``",
          "```0000000OOOOOOOOOO000000000000000`",
          "``00000000OOOOOOOOOO000000000000000`",
          "``00000OOOOOOOOOOOOO0000000OOOOOOOO`",
          "``00000OOOOOOOOOOOOO0000000OOOOOOOO`",
          "``00000OOOOOOOOOOOOO0000000OOOOOOOO`",
          "`000000OOOOOO000000000000000OOOOOOOO",
          "`0000OOOOOOOO000000000000000000OOOOO",
          "OOOOOOOOOOOOO000000000000000000OOOOO",
          "OOOOOOO000000000000000000000000OOOOO",
          "OOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO",
          ];

        string[] statueFoundSprite3 =
          [
          "````````````⣀⣠⣀⡀`````````````",
          "``````````⡴⣿⣿⣿⣿⣿⣿⡦⣄``````````",
          "````````⣼⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡳⡄````````",
          "``````⣰⣿⣿⣶⠓⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿````````",
          "`````⠈⣹⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣆``````",
          "`````⢰⡟⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡄``````",
          "`````⣹⢰⢫⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⢾⣿⡖⣿⠇`````",
          "`````⠈⢷⣟⣿⣿⣿⢻⣿⣿⣿⣿⣿⣿⣿⢾⣷⡎⡚⡇`````",
          "```````⢸⣮⢾⡟⣤⣿⣿⣿⣿⣿⣿⣿⣯⣿⡟⣖⠹⡄````",
          "``````⢸⡿⠤⣾⣮⡿⣿⣿⣿⣿⣿⣿⣿⢱⣏⣹⠛⣾⡵⣧```",
          "`````⠰⡮⢵⣾⡿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣺⣧⣄⢸⠃```",
          "`````⢠⣧⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⢻⣷⣝⡞⠅```",
          "````⣠⠜⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⢿⠄```",
          "```⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣄```",
          "``⢄⡇⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡄``",
          "``⢸⣹⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿``",
          "``⡍⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿`",
          "``⠇⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿`",
          "`⢸⠇⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡇",
          "⢠⡜⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣧",
          "⠀⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡏",
          "⢸⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡻",
          ];

        string[] statueFoundSprite4 =
          [
            "````````⣀⡤⠤⠤⢄`````````",
            "``````⣠⠔⣿⣿⣿⣿⣿⣿⠦```````",
            "`````⣤⣿⣿⣿⣿⣿⣿⣿⣿⣿⠱⡄`````",
            "````⢠⡿⣿⣦⡾⣿⣿⣿⣿⣿⢿⣠⡱⡄````",
            "````⠘⣿⢯⢽⣶⣿⣿⣿⣿⣿⣿⣿⢩⡇````",
            "````⠘⠮⣃⢋⡞⣿⣿⣿⣿⣿⣹⣷⡙⣃````",
            "`````⢠⣟⡽⣽⣿⣿⣿⣿⣿⡷⢿⣜⣚⡆```",
            "````⢠⣿⣴⡿⣿⣿⣿⣿⣿⣿⣿⣷⣿⡠⢟```",
            "````⢰⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⠗⢾⡛``",
            "```⡴⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿```",
            "``⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿``",
            "``⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣷`",
            "``⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿`",
            "`⢰⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡅",
            "`⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⠷",
            "⢠⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿",
          ];

    }
    partial class Win
    {
        [DllImport("User32.dll")]
        internal static extern short GetAsyncKeyState(int vKey);
    }
}
