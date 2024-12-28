using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Numerics;
using System.Text;

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

        // initial player coords
        public static float initPlayerX { get; set; }
        public static float initPlayerY { get; set; }
        public static float initPlayerA { get; set; }
    }

    // ---class Engine---
    // the raycasting engine
    class Engine
    {
        internal void Start()
        {
            // --initialize variables--
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
            // booleans
            bool bCloseRequest = false;
            bool bConsoleLargeEnough = true;
            bool bForDevOnly = true; // map and coordinate
            bool bGameOver = false; // TODO: implement game over logic (`void E_GameOver()`)
            bool bQuit = false;

            // raycasting variables
            float fFOV = MathF.PI / 4.0f;
            float fDepth = 16.0f;

            // console game screen variables
            float fSpeed = 100.0f;
            float fRotationSpeed = 0.25f;

            if (OperatingSystem.IsWindows())
            {
                fSpeed = 6.0f;
                fRotationSpeed = 1.25f;
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

            // --Start Engine--
            Console.Clear();
            Stopwatch stopwatch = Stopwatch.StartNew();

            // --- the game loop ---
            while (true)
            {
                E_HandleConsoleSize();

                E_UpdateFrame();
                E_RenderFrame();

                if (bCloseRequest) E_CloseRequest();
                if (bQuit) break;
            }
            // exit
            stopwatch.Stop();

            // update frame function
            // mostly for player controls
            void E_UpdateFrame()
            {
                bool up = false, down = false, left = false, right = false;

                // listen for keyboard input from player
                // (syncrhonous)
                while (Console.KeyAvailable)
                {
                    switch (Console.ReadKey(true).Key)
                    {
                        case ConsoleKey.W:
                            up = true;
                            break;
                        case ConsoleKey.A:
                            left = true;
                            break;
                        case ConsoleKey.S:
                            down = true;
                            break;
                        case ConsoleKey.D:
                            right = true;
                            break;
                        case ConsoleKey.Escape:
                            bCloseRequest = true;
                            break;
                    }
                }

                // timer to tie framerate to time instead of directly from thread speed
                float fIngameTime = (float)stopwatch.Elapsed.TotalSeconds;
                stopwatch.Restart();
                
                // use Window's API to get keyboard information directly from hardware
                // asynchronous
                if (OperatingSystem.IsWindows())
                {
                    up = up || Win.GetAsyncKeyState('W') is not 0; // forward
                    down = down || Win.GetAsyncKeyState('S') is not 0; // backward
                    left = left || Win.GetAsyncKeyState('A') is not 0; // left
                    right = right || Win.GetAsyncKeyState('D') is not 0; // right
                }

                // movement logic
                if (up && !down)
                {
                    fPlayerX += MathF.Cos(fPlayerA) * fSpeed * fIngameTime;
                    fPlayerY += MathF.Sin(fPlayerA) * fSpeed * fIngameTime;
                    if (map[(int)fPlayerY][(int)fPlayerX] == '#')
                    {
                        fPlayerX -= MathF.Cos(fPlayerA) * fSpeed * fIngameTime;
                        fPlayerY -= MathF.Sin(fPlayerA) * fSpeed * fIngameTime;
                    }
                }
                if (down && !up)
                {
                    fPlayerX -= MathF.Cos(fPlayerA) * fSpeed * fIngameTime;
                    fPlayerY -= MathF.Sin(fPlayerA) * fSpeed * fIngameTime;
                    if (map[(int)fPlayerY][(int)fPlayerX] == '#')
                    {
                        fPlayerX += MathF.Cos(fPlayerA) * fSpeed * fIngameTime;
                        fPlayerY += MathF.Sin(fPlayerA) * fSpeed * fIngameTime;
                    }
                }
                if (left && !right)
                {
                    fPlayerA -= fSpeed * fRotationSpeed * fIngameTime;
                    if (fPlayerA < 0)
                    {
                        fPlayerA %= MathF.PI * 2;
                        fPlayerA += MathF.PI * 2;
                    }
                }
                if (right && !left)
                {
                    fPlayerA += fSpeed * fRotationSpeed * fIngameTime;
                    if (fPlayerA > MathF.PI * 2)
                    {
                        fPlayerA %= MathF.PI * 2;
                    }
                }
            }

            // render frame function
            // 90% of the raycasting tech goodness
            void E_RenderFrame()
            {
                // sets default `depthBuffer` value
                for (int y = 0; y < nScreenHeight; y++)
                    for (int x = 0; x < nScreenHeight; x++)
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

                        if (bBoundary) nShade = ' ';

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
                foreach (var statue in statues)
                {
                    float fAngle = MathF.Atan2(statue.y - fPlayerY, statue.x - fPlayerX);
                    if (fAngle < 0) fAngle += 2f * MathF.PI;

                    float fDistance = Vector2.Distance(
                      new(fPlayerX, fPlayerY), new(statue.x, statue.y));

                    int nCeiling = (int)((float)(nScreenHeight / 2.0f) - nScreenHeight / ((float)fDistance));
                    int nFloor = nScreenHeight - nCeiling;

                    string[] statueSprite = statueSprite1;

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
                        Console.SetCursorPosition(0, 0);
                        Console.WriteLine("Console not large enough.");
                        Console.WriteLine($"Current size: {nConsoleWidth}x{nConsoleHeight}");
                        Console.WriteLine($"Minimum size: {nScreenWidth}x{nScreenHeight}");
                    }
                    else break;
                }
            }

            void E_CloseRequest()
            {
                Console.Clear();
                Console.WriteLine("r u sure? (y/N)"); // TODO: implement something better here

                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.Y: bQuit = true; break;
                    default: bQuit = false; break;
                }
            }

            void E_GameOver()
            {
                // TODO: implement game over logic
            }
        }
    }
    partial class Win
    {
        [DllImport("User32.dll")]
        internal static extern short GetAsyncKeyState(int vKey);
    }
}
