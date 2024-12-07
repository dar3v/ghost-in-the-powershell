    using System.Diagnostics;
using System.Text;
using Ghost_in_The_PowerShell;

namespace FPSEngine3D
{
    internal class Engine
    {
        // TODO:  pass this variables out from a different class
        //        for different maps for levels
        int nMapHeight = 16;
        int nMapWidth = 16;
        string[] map =
        [
        // (0,0)      (+,0)
        "###############",
        "#.............#",
        "#.#####..####.#",
        "#.#.........#.#",
        "#.#.###..##.#.#",
        "#.#.#.....#.#.#",
        "#.#.#.###.#.#.#",
        "#.l...###.....#",
        "#.@...###.....#",
        "#.#.#.....#.#.#",
        "#.#.###..##.#.#",
        "#.#.........#.#",
        "#.#####..####.#",
        "#.............#",
        "###############",
        // (0,+)      (+,+)
        ];

        // player position, as well as angle player is looking at
        float fPlayerX = 12.0f;
        float fPlayerY = 13.0f;
        float fPlayerA = 0.0f;

        // player speed
        float fSpeed = 70.0f;
        float fRotationSpeed = 1.25f;
        // float fSpeed = 8.0f;
        // float fRotationSpeed = 0.35f;
        Stopwatch stopwatch = new Stopwatch();

        // player FOV vars for depth calculations
        float fFOV = MathF.PI / 4.0f;
        float fDepth = 16.0f;

        bool bBackToMenu = false;

        string sConfirmQuit = "Are You Sure You Want To Return To Menu?";
        string sConfirmQuit0 = "Press Y to Continue, N if otherwise.";
        string sConfirmQuit1 = "(y/N)";

        public void Start()
        {
            int consoleHeight = Console.WindowHeight;
            int currentRow = consoleHeight / 2 - 4;
            Console.Clear();
           
            Game.centeredText("Level 00: Base Engine (Testing)", ref currentRow);
            Game.centeredText("\n", ref currentRow);
            Game.centeredText("Controls:", ref currentRow);
            Game.centeredText("\n", ref currentRow);
            Game.centeredText("- WASD to move/look", ref currentRow);
            Game.centeredText("- Esc to return to Menu", ref currentRow);
            Game.centeredText("\n", ref currentRow);
            Game.centeredText("Press Any Key to Begin", ref currentRow);
            Game.centeredText("Press \"Esc\" to Return to Menu", ref currentRow);
              

            if (Console.ReadKey().Key is not ConsoleKey.Escape)
            {
                Game.G_playGameMusic("./files/GameMusic.Wav");
                while (true)
                {
                    E_Navi();
                    E_Render();

                    if (bBackToMenu) break;
                }
            }
            return;
        }

        private void E_Navi()
        {
            int consoleHeight = Console.WindowHeight;
            int currentRow = consoleHeight / 2 - 4;
            // internal clock for consistent framerate
            float fElapsedTime = (float)stopwatch.Elapsed.TotalSeconds;
            stopwatch = Stopwatch.StartNew();

            bool bUp = false, bDown = false, bLeft = false, bRight = false;
            while (Console.KeyAvailable)
            {
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.W:
                        bUp = true;
                        break;
                    case ConsoleKey.S:
                        bDown = true;
                        break;
                    case ConsoleKey.A:
                        bLeft = true;
                        break;
                    case ConsoleKey.D:
                        bRight = true;
                        break;
                    case ConsoleKey.Escape:
                        Console.Clear();
                        Console.ResetColor();
                        Game.centeredText(sConfirmQuit, ref currentRow);
                        Game.centeredText("\n", ref currentRow);
                        Game.centeredText(sConfirmQuit0, ref currentRow);
                        Game.centeredText(sConfirmQuit1, ref currentRow);
                        ConsoleKeyInfo exitGame = Console.ReadKey(true);
                        if (exitGame.Key == ConsoleKey.Y) 
                        {
                            Game.G_playHomeMusic("./files/HomeMenubg.wav");
                            bBackToMenu = true;
                        }

                        // if (Console.ReadKey(true).Key = ConsoleKey.Y)  bBackToMenu = true;
                        else continue;
                        break;
                }
                if (bUp && !bDown)
                {
                    fPlayerX += MathF.Sin(fPlayerA) * fSpeed * fElapsedTime;
                    fPlayerY += MathF.Cos(fPlayerA) * fSpeed * fElapsedTime;
                    if (map[(int)fPlayerY][(int)fPlayerX] is '#')
                    {
                        fPlayerX -= MathF.Sin(fPlayerA) * fSpeed * fElapsedTime;
                        fPlayerY -= MathF.Cos(fPlayerA) * fSpeed * fElapsedTime;
                    }
                }
                if (bDown && !bUp)
                {
                    fPlayerX -= MathF.Sin(fPlayerA) * fSpeed * fElapsedTime;
                    fPlayerY -= MathF.Cos(fPlayerA) * fSpeed * fElapsedTime;
                    if (map[(int)fPlayerY][(int)fPlayerX] is '#')
                    {
                        fPlayerX += MathF.Sin(fPlayerA) * fSpeed * fElapsedTime;
                        fPlayerY += MathF.Cos(fPlayerA) * fSpeed * fElapsedTime;
                    }
                }
                if (bLeft && !bRight)
                {
                    fPlayerA -= (fSpeed * fRotationSpeed) * fElapsedTime;
                }
                if (bRight && !bLeft)
                {
                    fPlayerA += (fSpeed * fRotationSpeed) * fElapsedTime;
                }
            }
            stopwatch.Restart();
        }

        private void E_Render()
        {
            // render screen
            int nScreenWidth = Console.WindowWidth;
            int nScreenHeight = Console.WindowHeight;
            char[,] screen = new char[nScreenWidth, nScreenHeight];

            for (int x = 0; x < nScreenWidth; x++)
            {
                // For each column, calculate the projected ray angle into world space
                float fRayAngle = (fPlayerA - fFOV / 2.0f) + ((float)x / (float)nScreenWidth) * fFOV;

                float fDistanceToWall = 0;
                bool bHitWall = false;
                bool bBoundary = false;

                float fEyeX = MathF.Sin(fRayAngle); // Unit vector for ray in player space
                float fEyeY = MathF.Cos(fRayAngle);

                // check if ray has hit a wall
                while (!bHitWall && fDistanceToWall < fDepth)
                {
                    fDistanceToWall += 0.1f;

                    int nTestX = (int)(fPlayerX + fEyeX * fDistanceToWall);
                    int nTestY = (int)(fPlayerY + fEyeY * fDistanceToWall);

                    // Test if ray is out of bounds
                    if (nTestX < 0 || nTestX >= nMapWidth || nTestY < 0 || nTestY >= nMapHeight)
                    {
                        bHitWall = true;      // Just set distance to max depth
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
                            {
                                for (int ty = 0; ty < 2; ty++)
                                {
                                    float vy = (float)nTestY + ty - fPlayerY;
                                    float vx = (float)nTestX + tx - fPlayerX;
                                    float d = MathF.Sqrt(vx * vx + vy * vy);
                                    float dot = (fEyeX * vx / d) + (fEyeY * vy / d);
                                    p.Add((d, dot));
                                }
                            }
                            p.Sort((a, b) => a.Item1.CompareTo(b.Item1));
                            float fBound = 0.01f;
                            if (MathF.Acos(p[0].Item2) < fBound) bBoundary = true;
                            if (MathF.Acos(p[1].Item2) < fBound) bBoundary = true;
                        }
                    }
                }

                // calculate distance to ceiling and floor
                int nCeiling = (int)((float)(nScreenHeight / 2.0) - nScreenHeight / ((float)fDistanceToWall));
                int nFloor = nScreenHeight - nCeiling;

                char nShade = ' ';
                // Console.BackgroundColor = ConsoleColor.DarkGray;
                //Console.ForegroundColor = ConsoleColor.DarkGreen;
                if (fDistanceToWall <= fDepth / 4.0f) nShade = '█'; // nearest
                else if (fDistanceToWall < fDepth / 3.0f) nShade = '▓';
                else if (fDistanceToWall < fDepth / 2.0f) nShade = '▒';
                else if (fDistanceToWall < fDepth) nShade = '░';
                else nShade = ' '; // farthest

                if (bBoundary) nShade = ' ';

                for (int y = 0; y < nScreenHeight; y++)
                {
                    if (y <= nCeiling)
                        screen[x, y] = ' ';
                    else if (y > nCeiling && y <= nFloor)
                        screen[x, y] = nShade;
                    else
                    {
                        float b = 1.0f - ((float)y - nScreenHeight / 2.0f) / ((float)nScreenHeight / 2.0f);
                        if (b < 0.25) nShade = '#';
                        else if (b < .5) nShade = 'x';
                        else if (b < .75) nShade = '.';
                        else if (b < .9) nShade = '-';
                        else nShade = ' ';
                        screen[x, y] = nShade;
                    }
                }
            }

            // display frame
            StringBuilder render = new();
            for (int y = 0; y < screen.GetLength(1); y++)
            {
                for (int x = 0; x < screen.GetLength(0); x++)
                    render.Append(screen[x, y]);

                if (y < screen.GetLength(1) - 1)
                    render.AppendLine();
            }
            Console.CursorVisible = false;
            Console.SetCursorPosition(0, 0);
            Console.Write(render);
        }
    } // internal class bracket Engine
} // game namespace
