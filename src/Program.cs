using static System.Console;

namespace Ghost_in_The_PowerShell
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Clear();
            Game mygame = new Game();
            mygame.Start();
        }

        static void loadingBar() //added a loading in the start of the game
        {
            Console.CursorVisible = false;

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
