using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace RaycasterCS
{
    /*
     *  InitGame class
     *  This class gets initial values for
     *    - map;
     *    - player position & angle
     *  this is then passed to the Engine class
     *  and it will then Render it to the console buffer
     *
     *  TODO: make variables here assignable from another class
     */

    public static class InitGame
    {
        public static int initMapHeight = 16;
        public static int initMapWidth = 16;
        public static string[] map =
        [
            // (0,0)      (+,0)
            "###############",
            "#.............#",
            "#.#####..####.#",
            "#.#.........#.#",
            "#.#.###..##.#.#",
            "#.#.#.....#.#.#",
            "#.#.#.###.#.#.#",
            "#.....###.....#",
            "#.....###.....#",
            "#.#.#.....#.#.#",
            "#.#.###..##.#.#",
            "#.#.........#.#",
            "#.#####..####.#",
            "#.............#",
            "###############",
            // (0,+)      (+,+)
        ];

        // player position, as well as angle player is looking at
        public static float initPlayerX = 1.0f;
        public static float initPlayerY = 1.0f;
        public static float initPlayerA = 5.0f;
    }

    // ---class Engine---
    // the raycasting engine
    internal class Engine
    {
        //
        // ---Level class variables---
        //

        // map
        int nMapHeight = InitGame.initMapHeight;
        int nMapWidth = InitGame.initMapWidth;

        string[] map = InitGame.map;

        // player position & angle
        float fPlayerX = InitGame.initPlayerX;
        float fPlayerY = InitGame.initPlayerY;
        float fPlayerA = InitGame.initPlayerA;

        // player speed
        // TODO: adjust game speed accordingly to each individual systems
        float fSpeed = 50.0f;
        float fRotationSpeed = 1.25f;

        internal void Start()
        {
            // --initiallize variables---

            if (OperatingSystem.IsWindows()) // adjust speed accordingly to user OS
            {
                fSpeed = 7.0f;
                fRotationSpeed = 0.125f;
            }

            // screen stuff
            int nScreenWidth = 120;
            int nScreenHeight = 40;
            char[,] screen = new char[nScreenWidth, nScreenHeight];

            int nConsoleWidth = Console.WindowWidth;
            int nConsoleHeight = Console.WindowHeight;

            // player FOV vars for player FOV calculations
            float fFOV = MathF.PI / 4.0f;
            float fDepth = 16.0f;

            // etc
            bool bQuit = false;
            bool bDisplayCoords = true;
            bool bConsoleLargeEnough = true;

            Stopwatch stopwatch = new Stopwatch();

            // ---Start---
            Console.Clear();
            stopwatch = Stopwatch.StartNew();

            while (true) // the game loop
            {
                Controls();
                Render();

                if (bQuit) break;
            }
            stopwatch.Stop();
            return;

            void Controls()
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
                if (!bConsoleLargeEnough) return;

                bool bUp = false, bDown = false, bLeft = false, bRight = false;

                // use Window's API to get keyboard information directly from hardware
                if (OperatingSystem.IsWindows())
                {
                    bUp = bUp || Win.GetAsyncKeyState('W') is not 0; // forward
                    bDown = bDown || Win.GetAsyncKeyState('S') is not 0; // backward
                    bLeft = bLeft || Win.GetAsyncKeyState('A') is not 0; // left
                    bRight = bRight || Win.GetAsyncKeyState('D') is not 0; // right
                }
                // synchrounous controls for other systems
                // as well as controls that doesnt need to be asynchrounous
                while (Console.KeyAvailable)
                {
                    switch (Console.ReadKey(true).Key)
                    {
                        case ConsoleKey.W: // forward
                            bUp = true;
                            break;
                        case ConsoleKey.S: // backward
                            bDown = true;
                            break;
                        case ConsoleKey.A: // left
                            bLeft = true;
                            break;
                        case ConsoleKey.D: // right
                            bRight = true;
                            break;
                        case ConsoleKey.Escape: // pause
                            bQuit = true;
                            break;
                    }
                }
                // ties movement to ingame time
                float fIngameTime = (float)stopwatch.Elapsed.TotalSeconds;
                stopwatch.Restart();

                //--movement logic--
                if (bUp && !bDown)
                {
                    fPlayerX += MathF.Sin(fPlayerA) * fSpeed * fIngameTime;
                    fPlayerY += MathF.Cos(fPlayerA) * fSpeed * fIngameTime;
                    if (map[(int)fPlayerY][(int)fPlayerX] == '#') // collision
                    {
                        fPlayerX -= MathF.Sin(fPlayerA) * fSpeed * fIngameTime;
                        fPlayerY -= MathF.Cos(fPlayerA) * fSpeed * fIngameTime;
                    }
                }
                if (bDown && !bUp)
                {
                    fPlayerX -= MathF.Sin(fPlayerA) * fSpeed * fIngameTime;
                    fPlayerY -= MathF.Cos(fPlayerA) * fSpeed * fIngameTime;
                    if (map[(int)fPlayerY][(int)fPlayerX] == '#') // collision
                    {
                        fPlayerX += MathF.Sin(fPlayerA) * fSpeed * fIngameTime;
                        fPlayerY += MathF.Cos(fPlayerA) * fSpeed * fIngameTime;
                    }
                }
                if (bLeft && !bRight)
                {
                    fPlayerA -= fSpeed * fRotationSpeed * fIngameTime;
                    if (fPlayerA < 0)
                    {
                        fPlayerA += MathF.PI * 2;
                        fPlayerA %= MathF.PI * 2;
                    }
                }
                if (bRight && !bLeft)
                {
                    fPlayerA += fSpeed * fRotationSpeed * fIngameTime;
                    if (fPlayerA > Math.PI * 2)
                    {
                        fPlayerA %= MathF.PI * 2;
                    }
                }
            }

            // 90% of the raycasting tech goodness
            void Render()
            {
                // check if console size is large enough
                if (!bConsoleLargeEnough)
                {
                    Console.CursorVisible = false;
                    Console.SetCursorPosition(0, 0);
                    Console.WriteLine("Console not large enough.");
                    Console.WriteLine($"Current size: {nConsoleWidth}x{nConsoleHeight}");
                    Console.WriteLine($"Minimum size: {nScreenWidth}x{nScreenHeight}");
                    return;
                }

                for (int x = 0; x < nScreenWidth; x++)
                {
                    // For each column, calculate the projected ray angle into world space
                    float fRayAngle =
                        (fPlayerA - fFOV / 2.0f) + ((float)x / (float)nScreenWidth) * fFOV;

                    float fDistanceToWall = 0;
                    bool bHitWall = false;
                    bool bBoundary = false;

                    float fEyeX = MathF.Sin(fRayAngle); // Unit vector for ray in player space
                    float fEyeY = MathF.Cos(fRayAngle);
                    while (!bHitWall && fDistanceToWall < fDepth)
                    {
                        fDistanceToWall += 0.1f;

                        int nTestX = (int)(fPlayerX + fEyeX * fDistanceToWall);
                        int nTestY = (int)(fPlayerY + fEyeY * fDistanceToWall);

                        // Test if ray is out of bounds
                        if (nTestX < 0 || nTestX >= nMapWidth || nTestY < 0 || nTestY >= nMapHeight)
                        {
                            bHitWall = true; // Just set distance to max depth
                            fDistanceToWall = fDepth;
                        }
                        else
                        {
                            // Ray is inbounds so test to see if the ray cell is a wall block
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
                                if (MathF.Acos(p[0].Item2) < fBound)
                                    bBoundary = true;
                                if (MathF.Acos(p[1].Item2) < fBound)
                                    bBoundary = true;
                                if (MathF.Acos(p[2].Item2) < fBound)
                                    bBoundary = true;
                            }
                        }
                    }
                    // calculate distance to ceiling and floor
                    int nCeiling = (int)(
                        (float)(nScreenHeight / 2.0) - nScreenHeight / ((float)fDistanceToWall)
                    );
                    int nFloor = nScreenHeight - nCeiling;

                    char nShade = ' ';

                    // shading for walls
                    if (fDistanceToWall <= fDepth / 4.0f)
                        nShade = '█'; // nearest
                    else if (fDistanceToWall < fDepth / 3.0f)
                        nShade = '▓';
                    else if (fDistanceToWall < fDepth / 2.0f)
                        nShade = '▒';
                    else if (fDistanceToWall < fDepth)
                        nShade = '░';
                    else
                        nShade = ' '; // farthest

                    if (bBoundary)
                        nShade = ' ';

                    for (int y = 0; y < nScreenHeight; y++)
                    {
                        if (y <= nCeiling)
                            screen[x, y] = ' ';
                        else if (y > nCeiling && y <= nFloor)
                            screen[x, y] = nShade;
                        else
                        {
                            float b =
                                1.0f
                                - ((float)y - nScreenHeight / 2.0f) / ((float)nScreenHeight / 2.0f);
                            if (b < 0.25)
                                nShade = '#';
                            else if (b < .5)
                                nShade = 'x';
                            else if (b < .75)
                                nShade = '.';
                            else if (b < .9)
                                nShade = '-';
                            else
                                nShade = ' ';
                            screen[x, y] = nShade;
                        }
                    }
                }

                // display map and coord
                if (bDisplayCoords)
                {
                    string[] debug = { $"x:{fPlayerX}", $"y:{fPlayerY}", $"a:{fPlayerA}" };
                    for (int i = 0; i < debug.Length; i++)
                    for (int j = 0; j < debug[i].Length; j++)
                        screen[nScreenWidth - debug[i].Length + j, i] = debug[i][j];

                    for (int y = 0; y < map.Length; y++)
                    for (int x = 0; x < map[y].Length; x++)
                        screen[x, y] = map[y][x];

                    screen[(int)fPlayerX, (int)fPlayerY] = 'P';
                }

                // display frame
                StringBuilder render = new();
                for (int y = 0; y < screen.GetLength(1); y++)
                {
                    for (int x = 0; x < screen.GetLength(0); x++)
                    {
                        int c = ((nConsoleWidth - screen.GetLength(0)) / 2); // center
                        if (x == 0)
                            render.Append(' ', c);
                        render.Append(screen[x, y]);
                    }

                    if (y < screen.GetLength(1) - 1)
                        render.AppendLine();
                }
                Console.CursorVisible = false;
                Console.SetCursorPosition(0, 0);
                Console.Write(render);
            }
        } // RaycasterCS.Engine.Start() method

        partial class Win
        {
            [DllImport("User32.dll")]
            internal static extern short GetAsyncKeyState(int vKey);
        }
    } // RaycasterCS.Engine() object
} // RaycasterCS namespace
