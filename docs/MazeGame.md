# MazeGame

located at `./src/Engine/MazeGame.cs`.

this is where different levels are initialized with `RaycasterCS.InitGame`.

# `class ScaryMaze`

if adding/removing levels, modify `public static int nMaxLevels` accordingly.
you can also modify this if you only want to play certain levels. (ie. 1 - 3 levels only)

## `ScaryMaze.GameStart()`
method for Starting the maze game itself.

calls the method `Menu.StartMenu()` to start an initial menu at the start of the game.

modify `int nCurrentLevel` if u want to jump straight to a level. otherwise keep it as is (`1`).

## `ScaryMaze.InitLevel(int level)`

invokes `class InitGame` from `namespace RaycasterCS` to initialize map level variables.

takes the parameter `int level`, and tests the parameter with if statements.
```cs
private void InitLevel(int level)
{
    if (level == 1)
    {
        // initalize vars here...    
        InitGame.MazeMap = [ /* -- insert map here -- */ ];
        // more code here...
    }
    else if (level == 2)
    {
        // initalize vars here..
    }
    // so on so forth...
    else Console.WriteLine("not a level yet. check `nMaxLevels` value in MazeGame.cs");
    /// just make sure nMaxLevels checks out and this should not be invoked ever
}
```
simply add another `else if` block to add another level. Just make sure `nMaxLevels` is modified accordingly.

# `Class Menu()`
## `NextLevelMenu(int currentLevel)`

method to display a menu after completing a level.

will display a menu based on the parameter, ie. `currentLevel = 2` then display:
```

                    ┏┓             ┓   •     
                    ┃ ┏┓┏┓┏┓┏┓┏┓╋┓┏┃┏┓╋┓┏┓┏┓┏
                    ┗┛┗┛┛┗┗┫┛ ┗┻┗┗┻┗┗┻┗┗┗┛┛┗┛
                         ┛                   
                    ┏┓          ┓•           ┓       ┓  ┏┓
                    ┃┃┏┓┏┓┏┏┓┏┓┏┫┓┏┓┏┓  ╋┏┓  ┃ ┏┓┓┏┏┓┃  ┏┛
                    ┣┛┛ ┗┛┗┗ ┗ ┗┻┗┛┗┗┫  ┗┗┛  ┗┛┗ ┗┛┗ ┗  ┗━
                                   ┛              
```

this method does not generate a sprite, only a predefined sprite inside the method will be displayed.

## `GameFinish()`

will display after the game, if `bGameOver` and `bQuit` is `false`.

## GameOver()
```txt
⠀⠀⠀⠀⠀⢀⣴⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣶⡄⠀⠀⠀⠀⠀
⠀⠀⠀⠀⢀⡿⣷⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣦⠄⠀⠀⠀
⠀⠀⠀⠀⢰⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡆⠀⠀⠀
⠀⠀⠀⢠⣿⣇⢿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣇⠀⠀⠀
⠀⠀⠀⠀⣻⣿⣿⣿⣿⡿⡿⣿⣿⣿⣿⣿⣿⣿⣿⣯⣟⣿⣿⣿⣿⣷⣭⠀⠀⠀
⠀⠀⠀⠀⣻⣿⠟⠛⠉⠁⠈⠉⠻⢿⣿⣿⣿⡟⠛⠂⠉⠁⠈⠉⠁⠻⣿⠀⠀⠀
⠀⠀⠀⠀⢾⠀⠀⣠⠄⠻⣆⠀⠈⠠⣻⣿⣟⠁⠀⠀⠲⠛⢦⡀⠀⠠⠁⠀⠀⠀
⠀⠀⠀⠀⢱⣄⡀⠘⠀⠸⠉⠀⠀⢰⣿⣷⣿⠂⢀⠀⠓⡀⠞⠀⢀⣀⠀⠀⠀⠀
⠀⠀⠀⠀⠠⣿⣷⣶⣶⣶⣾⣿⠀⠸⣿⣿⣿⣶⣿⣧⣴⣴⣶⣶⣿⡟⠀⠀⠀⠀
⠀⠀⠀⠀⠀⢿⣿⣿⣿⣿⣿⣏⠇⠄⣿⣿⣿⣿⣿⣿⣿⣿⣿⣟⣾⠁⠀⠀⠀⠀
⠀⠀⠀⠀⠀⢺⣿⣿⣿⣿⣟⡿⠂⠈⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⠑⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠈⣿⣿⣿⣿⣿⠀⠀⠈⠿⣿⣿⣿⣿⣿⣿⣿⣿⠁⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠄⢻⣿⣿⣿⡗⠀⠀⠀⠀⠈⠀⢨⣿⣿⣿⣿⣿⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⡞⠷⠿⠿⠀⠀⠀⠀⢀⣘⣤⣿⣿⣿⣿⣿⡏⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠼⠉⠀⠀⠀⠀⠀⠚⢻⠿⠟⠓⠛⠂⠉⠉⠁⠀⡁⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣠⣼⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢰⣿⡿⡀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢠⢾⠻⠌⣄⡁⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢠⣀⣀⣀⡠⡲⠞⡁⠈⡈⣿⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠐⠘⠛⠻⢯⠟⠩⠀⠀⢠⣣⠈⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⡀⠄⠂⣰⣧⣾⠶⠀⠀⠀⠀⠀⠀⠀
```
