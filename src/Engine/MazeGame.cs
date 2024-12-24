using RaycasterCS;

namespace MazeGame
{
    class InitMazeGame()
    {
        internal void StartMenu()
        {
            // TODO: add something better here
            Console.WriteLine("Maze Game start menu");
        }
    }

    class ScaryMaze
    {
        // TODO: add the actual level here
        public void LevelOne()
        {
            InitGame.MazeMap = [
                  "###############",
                  "#.............#",
                  "#.#########...#",
                  "#.#...@...#...#",
                  "#.........#...#",
                  "#.............#",
                  "#.#...........#",
                  "#.###......####",
                  "#..........#..#",
                  "#.....##...#..#",
                  "#.....##......#",
                  "#.............#",
                  "#.#####..####.#",
                  "#.............#",
                  "###############",
            ];

            InitGame.initMapWidth = 16;
            InitGame.initMapHeight = 16;
            InitGame.initPlayerX = 1;
            InitGame.initPlayerY = 1;

            // --ENGINE-- start
            Engine Engine = new();
            Engine.Start();
        }
    }
}
