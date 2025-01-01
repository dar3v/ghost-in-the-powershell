# Engine

The heart of this project. Located at `./src/Engine/Engine.cs`.

can be used on different namespaces with a `using` directive, eg:
```cs
using RaycasterCS;
```

run the engine with the method `Engine.Start()`.
```cs
using RaycasterCS;

namespace Name
{
    class Program
    {
        public static void Main(string[] args)
        {
            // some other stuff to run before running the game
            Engine engine = new();
            engine.Start();
        }
    }
}
```

but before running this method, set initial map variables first in `InitGame` class, which can be done in a file with a `using RaycasterCS` directive.
```cs
using RaycasterCS;

namespace Name
{
    class Program
    {
        public static void Main(string[] args)
        {
                   InitGame.MazeMap = [
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
                InitGame.initMazeTime = 45; // level time limit

                InitGame.initMapWidth = 15; // make sure this checks out with `MazeMap[]`
                InitGame.initMapHeight = 15; // this as well
                
                // set initial player position in the map
                InitGame.initPlayerX = 1; 
                InitGame.initPlayerY = 1;

            
            // some other stuff to run before running the game
            Engine engine = new();
            engine.Start();
        }
    }
}

```

## `class Engine()`
### `Engine.Start()`
starts the engine.

will use the initialized variables in `class InitGame`.

if variables from `class InitGame` are not Initialized, (or not initialized at all), the Engine will start a placeholder map instead.

this method returns an `int`.
- `return 0;` if player is quitting;
- `return 1;` if game over; and 
- `return 2;` if a maze level is completed.
