using static System.Console;

namespace Ghost_in_The_PowerShell
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            Clear();
            loadingBar();
            someethingLoading();
            Game mygame = new Game();
            mygame.Start();

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
    }
}
