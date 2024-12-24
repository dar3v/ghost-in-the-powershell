using static System.Console;
using System.Text;
using WriteFunc;

namespace Menu
{
    internal class Menu_C
    {
        private int indexSelected;
        private string[] Options;
        private string[] Prompt;

        public Menu_C(string[] prompt, string[] options)
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

            CWriteFunc.RenderUpperCenteredStrings(Prompt);
            // Calculate padding for centering the instruction text
            int padding = (consoleWidth - instruct.Length) / 2;
            
            // Write a new line padded with spaces to center the prompt 
            System.Console.WriteLine(new string (' ', promptPadding));
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

namespace WriteFunc
{
    internal class CWriteFunc 
    {

    public static void KeyboardPrint(string text, int speed = 40) //A method for the keyboard typing effect
        {
            foreach (char c in text)
            {
                Console.Write(c);
                Thread.Sleep(speed);
            }
        }

        //NOTE: Comment cuz yeh, just made a new centering method that is responsive
        // public static void CenteredText(string text, ref int currentRow) 
        // { //METHOD FOR CENTERING TEXT (make sure to declare int <variable_name> = Console.WindowHeight;)
        //   //and also declare a variable "int <name> = <variable_name above> / 2;  you can subtract it to change the height
        //     {
        //         int consoleWidth = Console.WindowWidth;
                
        //         int textLength = text.Length;
        //         int paddingWidth = (consoleWidth - textLength) / 2;
        
        //         // Debugging: Print padding and text length to verify
        //         //Console.WriteLine($"Text Length: {textLength}, Padding: {padding}");
        //         Console.SetCursorPosition(paddingWidth, currentRow);
        //         Console.WriteLine(text);
        //         currentRow++;
        //     }
        // }

        public static void RenderUpperCenteredStrings(string[] titlePrompt)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            int lastWidth = Console.WindowWidth;
            int lastHeight = Console.WindowHeight;

            while (true)
            {
                    int currentWidth = Console.WindowWidth;
                    int currentHeight = Console.WindowHeight;

                    if (currentWidth != lastWidth || currentHeight != lastHeight)
                        {
                            Console.Clear();
                            lastWidth = currentWidth;
                            lastHeight = currentHeight;
                        }

                if (currentWidth >= titlePrompt[0].Length && currentHeight >= titlePrompt.Length)
                {
                    int verticalOffset = Math.Max(0, (currentHeight / 4) - (titlePrompt.Length / 2));
                    StringBuilder render = new();
                    render.Append('\n', verticalOffset);

                    foreach (string line in titlePrompt)
                    {
                        int horizontalOffset = Math.Max(0, (currentWidth - line.Length) / 2);
                        render.Append(' ', horizontalOffset).AppendLine(line);
                    }

                    Console.SetCursorPosition(0, 0);
                    Console.Write(render);
                    break;
                }
                else
                {
                    Console.SetCursorPosition(0, 0);
                    Console.WriteLine("Console not large enough.");
                    Console.WriteLine($"Current size: {currentWidth}x{currentHeight}");
                }

                // if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Y)
                // {
                //     Environment.Exit(0);
                // }
            }
        }
        public static void RenderCenteredStrings(string[] titlePrompt)
        {
            int lastWidth = Console.WindowWidth;
            int lastHeight = Console.WindowHeight;

            while (true)
            {
                    int currentWidth = Console.WindowWidth;
                    int currentHeight = Console.WindowHeight;

                    if (currentWidth != lastWidth || currentHeight != lastHeight)
                        {
                            Console.Clear();
                            lastWidth = currentWidth;
                            lastHeight = currentHeight;
                        } 

                if (currentWidth >= titlePrompt[0].Length && currentHeight >= titlePrompt.Length)
                {
                    int verticalOffset = Math.Max(0, (currentHeight / 2) - (titlePrompt.Length / 2));
                    StringBuilder render = new();
                    render.Append('\n', verticalOffset);

                    foreach (string line in titlePrompt)
                    {
                        int horizontalOffset = Math.Max(0, (currentWidth - line.Length) / 2);
                        render.Append(' ', horizontalOffset).AppendLine(line);
                    }

                    Console.SetCursorPosition(0, 0);
                    Console.Write(render);
                    break;
                }
                else
                {
                    Console.SetCursorPosition(0, 0);
                    Console.WriteLine("Console not large enough.");
                    Console.WriteLine($"Current size: {currentWidth}x{currentHeight}");
                }

                // if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Y)
                // {
                //     Environment.Exit(0);
                // }
            }
        }
    }
}
