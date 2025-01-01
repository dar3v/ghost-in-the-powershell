using static System.Console;
using System.Text;
using WriteFunc;
using System.Diagnostics;

namespace WriteFunc
{
    internal class CWriteFunc 
    {
        // public static void FallingBloodTransition(int width, int height, float fallingSpeedMilliseconds)
        // {
        //     Random random = new Random();
        //     Console.Clear();
        //     Console.CursorVisible = false; // Hide the cursor for a cleaner effect

        //     // Create an array to track where blood has "landed" for each column
        //     int[] bloodHeight = new int[width];

        //     // Initialize the stopwatch to measure time
        //     Stopwatch stopwatch = new Stopwatch();
        //     stopwatch.Start();

        //     while (true)
        //     {
        //         // Only update once the specified time has passed
        //         if (stopwatch.ElapsedMilliseconds >= fallingSpeedMilliseconds)
        //         {
        //             // Randomly select a column for the falling blood
        //             int col = random.Next(width);

        //             // If the column hasn't reached the bottom, let the blood "fall"
        //             if (bloodHeight[col] < height)
        //             {
        //                 Console.SetCursorPosition(col, bloodHeight[col]);
        //                 Console.ForegroundColor = ConsoleColor.Red;
        //                 Console.Write("█");

        //                 // Increment the height for this column
        //                 bloodHeight[col]++;
        //             }

        //             // Reset the stopwatch to start measuring for the next update
        //             stopwatch.Restart();
        //         }

        //         // Break when all columns are full
        //         if (Array.TrueForAll(bloodHeight, h => h >= height))
        //         {
        //             break;
        //         }
        //     }

        //     Console.ResetColor();
        //     Console.CursorVisible = true; // Restore cursor visibility
        // }


        public static void FallingBloodTransition(float fallingSpeedMilliseconds)
{
    Random random = new Random();
    Console.CursorVisible = false; // Hide the cursor for a cleaner effect

    // Dynamically get the current console dimensions
    int width = Console.WindowWidth;
    int height = Console.WindowHeight;

    // Create an array to track where blood has "landed" for each column
    int[] bloodHeight = new int[width];

    // Initialize the stopwatch to measure time
    Stopwatch stopwatch = new Stopwatch();
    stopwatch.Start();

    while (true)
    {
        // Dynamically update the console dimensions in case of resizing
        width = Console.WindowWidth;
        height = Console.WindowHeight;

        // Adjust the bloodHeight array if the console width changes
        if (bloodHeight.Length != width)
        {
            Array.Resize(ref bloodHeight, width);
        }

        // Only update once the specified time has passed
        if (stopwatch.ElapsedMilliseconds >= fallingSpeedMilliseconds)
        {
            // Randomly select a column for the falling blood
            int col = random.Next(width);

            // If the column hasn't reached the bottom, let the blood "fall"
            if (bloodHeight[col] < height)
            {
                Console.SetCursorPosition(col, bloodHeight[col]);
                Console.ForegroundColor = ConsoleColor.DarkRed; // Initial color of blood
                Console.Write("█");

                // Increment the height for this column
                bloodHeight[col]++;
            }

            // Reset the stopwatch to start measuring for the next update
            stopwatch.Restart();
        }

        // Break when all columns are full
        if (Array.TrueForAll(bloodHeight, h => h >= height))
        {
            break;
        }
        
    }

    Console.ResetColor();
}


    // public static void FadeEffectAndShowText()
    // {
    //     int fadeSteps = 5;  // Number of color transitions
    //     var fadeColors = new ConsoleColor[] 
    //     { 
    //         ConsoleColor.DarkRed, 
    //         ConsoleColor.Red, 
    //         ConsoleColor.DarkGray, 
    //         ConsoleColor.Gray, 
            
    //     };

    //     // Iterate through the fade colors
    //     foreach (var color in fadeColors)
    //     {
    //         Console.ForegroundColor = color;

    //         // Simulate the blood fading by overwriting with the new color
    //         for (int row = 0; row < Console.WindowHeight; row++)
    //         {
    //             for (int col = 0; col < Console.WindowWidth; col++)
    //             {
    //                 Console.SetCursorPosition(col, row);
    //                 Console.Write("█");  // Keep overwriting with the new color
    //             }
    //         }

    //         System.Threading.Thread.Sleep(200); // Adjust to control fade speed
    //     }

    //     // Clear screen after fade to black and show text
        
    // }
    public static void RenderCenteredStringsTyping(string[] titlePrompt, int speed = 40)
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
            Console.Clear();

            // Add vertical padding to center the text vertically
            Console.WriteLine(new string('\n', verticalOffset));

            for (int i = 0; i < titlePrompt.Length; i++)
            {
                if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Enter)
                {
                    // If Enter is pressed, print all lines instantly
                    Console.Clear();
                    Console.WriteLine(new string('\n', verticalOffset));

                    foreach (string lines in titlePrompt)
                    {
                        int horizontalOffsets = Math.Max(0, (currentWidth - lines.Length) / 2);
                        Console.WriteLine(new string(' ', horizontalOffsets) + lines);
                    }
                    return;
                }

                // Print the current line with typing effect
                string line = titlePrompt[i];
                int horizontalOffset = Math.Max(0, (currentWidth - line.Length) / 2);
                Console.SetCursorPosition(horizontalOffset, Console.CursorTop);

                foreach (char c in line)
                {
                    if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Enter)
                    {
                        // If Enter is pressed mid-line, print remaining lines instantly
                        Console.Clear();
                        Console.WriteLine(new string('\n', verticalOffset));

                        foreach (string remainingLine in titlePrompt)
                        {
                            int remainingHorizontalOffset = Math.Max(0, (currentWidth - remainingLine.Length) / 2);
                            Console.WriteLine(new string(' ', remainingHorizontalOffset) + remainingLine);
                        }
                        return;
                    }
                    Console.Write(c);
                    Thread.Sleep(speed);
                }
                Console.WriteLine(); // Move to the next line
            }

            break;
        }
        else
        {
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("Console not large enough.");
            Console.WriteLine($"Current size: {currentWidth}x{currentHeight}");
        }
    }
}

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

        // public static void RenderUpperCenteredStrings(string[] titlePrompt)
        // {
        //     Console.ForegroundColor = ConsoleColor.DarkRed;
        //     int lastWidth = Console.WindowWidth;
        //     int lastHeight = Console.WindowHeight;

        //     while (true)
        //     {
        //             int currentWidth = Console.WindowWidth;
        //             int currentHeight = Console.WindowHeight;

        //             if (currentWidth != lastWidth || currentHeight != lastHeight)
        //                 {
        //                     Console.Clear();
        //                     lastWidth = currentWidth;
        //                     lastHeight = currentHeight;
        //                 }

        //         if (currentWidth >= titlePrompt[0].Length && currentHeight >= titlePrompt.Length)
        //         {
        //             int verticalOffset = Math.Max(0, (currentHeight / 4) - (titlePrompt.Length / 2));
        //             StringBuilder render = new();
        //             render.Append('\n', verticalOffset);

        //             foreach (string line in titlePrompt)
        //             {
        //                 int horizontalOffset = Math.Max(0, (currentWidth - line.Length) / 2);
        //                 render.Append(' ', horizontalOffset).AppendLine(line);
        //             }

        //             Console.SetCursorPosition(0, 0);
        //             Console.Write(render);
        //             break;
        //         }
        //         else
        //         {
        //             Console.SetCursorPosition(0, 0);
        //             Console.WriteLine("Console not large enough.");
        //             Console.WriteLine($"Current size: {currentWidth}x{currentHeight}");
        //         }

        //         // if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Y)
        //         // {
        //         //     Environment.Exit(0);
        //         // }
        //     }
        // }

        public static void RenderCenteredStrings(string[] titlePrompt, ConsoleColor[] colors)
        {
            int lastWidth = Console.WindowWidth;
            int lastHeight = Console.WindowHeight;
        
            Random random = new Random(); // Random object to generate random colors
        
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
                    StringBuilder render = new StringBuilder();
                    render.Append('\n', verticalOffset);
        
                    for (int lineIndex = 0; lineIndex < titlePrompt.Length; lineIndex++)
                    {
                        string line = titlePrompt[lineIndex];
                        int horizontalOffset = Math.Max(0, (currentWidth - line.Length) / 2);
                        render.Append(' ', horizontalOffset);
        
                        // Loop through each character in the line
                        for (int charIndex = 0; charIndex < line.Length; charIndex++)
                        {
                            // Generate a random color for each character
                            ConsoleColor randomColor = (ConsoleColor)random.Next(1, Enum.GetValues(typeof(ConsoleColor)).Length);
        
                            // Set the random color for the current character
                            Console.ForegroundColor = randomColor;
                            render.Append(line[charIndex]);  // Append the character with the random color
                        }
        
                        render.AppendLine();
                    }
        
                    Console.SetCursorPosition(0, 0);
                    Console.Write(render.ToString());  // Output the built string to the console
                    Console.ResetColor(); // Reset to default console color
                    break;
                }
                else
                {
                    Console.SetCursorPosition(0, 0);
                    Console.WriteLine("Console not large enough.");
                    Console.WriteLine($"Current size: {currentWidth}x{currentHeight}");
                }
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
