namespace MiniOrderApp.Presentation.UI;

public static class MenuHelper
{
    public static int ShowArrowMenu(string title, string[] options)
    {
        int selectedIndex = 0;

        while (true)
        {
            Console.Clear();
            Console.WriteLine(title);
            Console.WriteLine();

            for (int i = 0; i < options.Length; i++)
            {
                if (i == selectedIndex)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("> ");
                }
                else
                {
                    Console.Write("  ");
                }

                Console.WriteLine(options[i]);
                Console.ResetColor();
            }

            var key = Console.ReadKey(true).Key;

            if (key == ConsoleKey.UpArrow && selectedIndex > 0)
            {
                selectedIndex--;
            }
            else if (key == ConsoleKey.DownArrow && selectedIndex < options.Length - 1)
            {
                selectedIndex++;
            }
            else if (key == ConsoleKey.Enter)
            {
                return selectedIndex;
            }
            else if (key == ConsoleKey.Escape)
            {
                return -1;
            }
        }
    }
}
