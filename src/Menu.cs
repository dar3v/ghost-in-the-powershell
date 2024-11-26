using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;


namespace Ghost_in_The_PowerShell
{
    internal class Menu
    {
        private int indexSelected;
        private string[] Options;
        private string Prompt;

        public Menu(string prompt, string[] options)
        {
            indexSelected = 0;
            Options = options;
            Prompt = prompt;

        }
        private void optionDisplay()
        {
            WriteLine(Prompt);
            for (int i = 0; i < Options.Length; i++)
            {
                string currentOption = Options[i];
                string selectSymbol = "";

                if (i == indexSelected)
                {
                    selectSymbol = ">>";
                    ForegroundColor = ConsoleColor.Black;
                    BackgroundColor = ConsoleColor.White;
                }
                else
                {
                    selectSymbol = " ";
                    ForegroundColor = ConsoleColor.White;
                    BackgroundColor = ConsoleColor.Black;
                }

                Console.WriteLine($"{selectSymbol}[  {currentOption}  ]");
            }
            ResetColor();
        }
        public int Run()
        {
            ConsoleKey keyPressed;
            do
            {
                Clear();
                optionDisplay();

                ConsoleKeyInfo keyInfo = ReadKey(true);
                keyPressed = keyInfo.Key;
                //updating the selection based on arrow keys

                if (keyPressed == ConsoleKey.UpArrow)
                {
                    indexSelected--;
                    if (indexSelected == -1)
                    {   
                        indexSelected = Options.Length - 1;
                    }
                }
                else if (keyPressed == ConsoleKey.DownArrow)
                {
                    indexSelected++;
                    if (indexSelected == Options.Length)
                    {
                        indexSelected = 0;
                    }
                }
            } while ( keyPressed != ConsoleKey.Enter);
            return indexSelected;
        }
    }
}
