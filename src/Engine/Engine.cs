using System.Diagnostics;
using System.Text;

//
// TODO: MODULARIZE THIS
//

namespace Ghost_in_The_PowerShell
{
    public class Engine
    {
        int nMapHeight = 16;
        int nMapWidth = 16;
        string[] map =
        [
        "##########.....#",
        "#..............#",
        "#...#.......####",
        "#...###.....#..#",
        "#...###.....#..#",
        "#...........#..#",
        "#...........#..#",
        "#..............#",
        "#.......########",
        "#.......#......#",
        "#.......#....###",
        "#............###",
        "#...............",
        "#.......########",
        "#..............#",
        "################",
            ];
        // Console must be atleast this big
        int nScreenWidth = Console.WindowWidth; //changed the nScreenWidth and nScreenHeight variable
        int nScreenHeight = Console.WindowHeight;
        // int nScreenWidth = 120;
        // int nScreenHeight = 40;

        // player position, as well as angle player is looking at
        float fPlayerX = 3.0f;
        float fPlayerY = 8.0f;
        float fPlayerA = 0.0f;

        // NOTE: adjust these values if the moving is too fast/too slow
        float fSpeed = 100.0f;
        float fRotationSpeed = 0.79f;

        float fFOV = MathF.PI / 4.0f;
        float fDepth = 16.0f;

        public void Start()
        {
            Stopwatch stopwatch = new Stopwatch();
            char[,] screen = new char[nScreenWidth, nScreenHeight];

            // TODO: rewrite this bit idk
            Console.WriteLine(@"
                (under development)

                Controls:
                    - WASD to move/look
                    - Esc to Quit

                  Press any key to play the game...
                ");

            if (Console.ReadKey(true).Key is not ConsoleKey.Escape)
                while (true)
                {
                    // Controls
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
                                bRight = true; break;
                            case ConsoleKey.Escape: Console.Clear(); return;
                        }
                    }

                    float fElapsedTime = (float)stopwatch.Elapsed.TotalSeconds;
                    stopwatch.Restart();

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
                        fPlayerA -= fSpeed * fRotationSpeed * fElapsedTime;
                        if (fPlayerA < 0)
                        {
                            fPlayerA %= (float)Math.PI * 2;
                            fPlayerA += (float)Math.PI * 2;
                        }
                    }
                    if (bRight && !bLeft)
                    {
                        fPlayerA += fSpeed * fRotationSpeed * fElapsedTime;
                        if (fPlayerA > (float)Math.PI * 2)
                        {
                            fPlayerA %= (float)Math.PI * 2;
                        }
                    }

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

                    // write frame
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
                } // while loop bracket
        } // Start() method bracket
    } // class bracket
} // namespace bracket
