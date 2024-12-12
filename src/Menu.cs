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
            // for centering the instruct variable
            int consoleWidth = Console.WindowWidth;
            int promptPadding = Console.WindowHeight; 
            
            string instruct = "Use Your Arrow Keys To Hover Through The Selections and Press Enter to Select.";
            

            // Calculate padding for centering the instruction text
            int padding = (consoleWidth - instruct.Length) / 2;
            
            // Write a new line padded with spaces to center the prompt 
            System.Console.WriteLine(new string (' ', promptPadding) + Prompt);
            WriteLine("\n");
            System.Console.WriteLine(instruct.PadLeft(padding + instruct.Length));
            System.Console.WriteLine("\n");

            for (int i = 0; i < Options.Length; i++)
            {
                string currentOption = Options[i];
                string selectSymbol = "";
                string selectSymbol2 = "";


                if (i == indexSelected)
                {
                    selectSymbol = ">>";
                    selectSymbol2 = "<<";
                    ForegroundColor = ConsoleColor.Red;
                    
                }
                else
                {
                    selectSymbol = " ";
                    ForegroundColor = ConsoleColor.White;
                    
                }

                string optionText = $"{selectSymbol} {currentOption} {selectSymbol2}";
                
                // Calculate padding for centering the option text
                int optionPaddingWidth = Math.Max((consoleWidth - optionText.Length) / 2, 0);
                
                
                //print centered option 
                System.Console.WriteLine(new string (' ', optionPaddingWidth) + optionText);
                // Console.WriteLine($"{selectSymbol}[  {currentOption}  ]");
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
            } while (keyPressed != ConsoleKey.Enter);
            return indexSelected;
        }
    }
}
