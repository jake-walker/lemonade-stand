using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemonade_Stand
{
    public static class UiUtils
    {
        public static Dictionary<string, ConsoleColor> ThemeColors = new Dictionary<string, ConsoleColor>()
        {
            { "Primary", ConsoleColor.Green },
            { "Secondary", ConsoleColor.Yellow },
            { "Danger", ConsoleColor.Red },
            { "Muted", ConsoleColor.Gray },
            { "Default", ConsoleColor.White }
        };

        /// <summary>
        /// Print out the header message.
        /// </summary>
        public static void WelcomeMessage()
        {
            UiUtils.Print("   __                               __      ______               __\r\n  / /  ___ __ _  ___  ___  ___ ____/ /__   / __/ /____ ____  ___/ /\r\n / /__/ -_)  ' \\/ _ \\/ _ \\/ _ `/ _  / -_) _\\ \\/ __/ _ `/ _ \\/ _  / \r\n/____/\\__/_/_/_/\\___/_//_/\\_,_/\\_,_/\\__/ /___/\\__/\\_,_/_//_/\\_,_/  \r\n                                                                   ", "Primary");
        }

        /// <summary>
        ///     Display a menu and handle user inputs
        /// </summary>
        /// <param name="prompt">The message to display to the user</param>
        /// <param name="items">The items that the user will be able to pick</param>
        /// <returns>The item that the user chose</returns>
        public static string Menu(string prompt, params string[] items)
        {
            var current = 0;
            var done = false;

            while (!done)
            {
                Console.Clear();
                Print($"{prompt}", "Primary");
                Print(
                    "Press the up and down arrow keys to change items and the enter key to confirm a selection.",
                    "Muted");

                for (var i = 0; i < items.Length; i++) Print($" {(i == current ? '>' : '-')} {items[i]}", "Secondary");

                var k = Console.ReadKey(true);

                if (k.Key == ConsoleKey.UpArrow && current > 0)
                {
                    current -= 1;
                }
                else if (k.Key == ConsoleKey.DownArrow && current + 1 < items.Length)
                {
                    current += 1;
                }
                else if (k.Key == ConsoleKey.Enter)
                {
                    done = true;
                }
            }

            return items[current];
        }

        /// <summary>
        /// Asks the user a question, validates the response then returns a validated value.
        /// </summary>
        /// <param name="prompt">The question to display to the user</param>
        /// <param name="type">The type that needs to be returned (e.g string, int, decimal)</param>
        /// <param name="checkEmpty">Whether the response shouldn't be empty</param>
        /// <param name="password">Whether the user's input should be masked</param>
        /// <returns></returns>
        public static object Field(string prompt, string type = "string", bool checkEmpty = true, bool password = false, float biggerThan = -1) 
        {
            Print(prompt, "Primary");

            var valid = false;
            object output = false;

            while (!valid)
            {
                Print(" -> ", "Muted", false);
                Console.ForegroundColor = ThemeColors["Secondary"];
                var response = password ? GetPassword() : Console.ReadLine();
                Console.ForegroundColor = ThemeColors["Default"];
                valid = true;

                if (string.IsNullOrWhiteSpace(response) && checkEmpty)
                {
                    Print("    - Your input must not be empty.", "Danger");
                    valid = false;
                }

                switch (type)
                {
                    case "int":
                    {
                        int intOutput;

                        if (!int.TryParse(response, out intOutput))
                        {
                            Print("    - Your input must be an integer.", "Danger");
                            valid = false;
                            continue;
                        }

                        output = intOutput;
                        break;
                    }
                    case "decimal":
                    {
                        decimal decimalOutput;

                        if (!decimal.TryParse(response, out decimalOutput))
                        {
                            Print("    - Your input must be a decimal.", "Danger");
                            valid = false;
                            continue;
                        }

                        if (decimalOutput < biggerThan)
                        {
                            Print($"    - You input must be more than {biggerThan}", "Danger");
                            valid = false;
                            continue;
                        }

                        output = decimalOutput;
                        break;
                    }
                    case "boolean":
                    {
                        var r = response?.ToLower();
                        if (r == "yes" || r == "y")
                        {
                            output = true;
                            break;
                        } else if (r == "no" || r == "n")
                        {
                            output = false;
                            break;
                        }
                        else
                        {
                            Print("    - Your input must be 'yes' or 'no'.", "Danger");
                            valid = false;
                            continue;
                        }
                    }
                    default:
                        output = response;
                        break;
                }
            }

            return output;
        }

        /// <summary>
        ///     Get the users input, masking the input (for passwords)
        /// </summary>
        /// <returns>A string of what the user types</returns>
        private static string GetPassword()
        {
            var done = false;
            var output = "";

            while (!done)
            {
                // Get the key that the user presses, intercepting so that it isn't shown to the user.
                var k = Console.ReadKey(true);

                // If the key is enter, break the loop
                if (k.Key == ConsoleKey.Enter)
                {
                    done = true;

                    // If the key is backspace and the user has typed something...
                }
                else if (k.Key == ConsoleKey.Backspace && output.Length > 0) {
                    // Remove the last character from the output string
                    output = output.Substring(0, output.Length - 1);
                    // Go back a character, print a space then go back another character to ensure that the character is replaced by a space.
                    Console.Write("\b \b");

                    // For any other key...
                } else
                {
                    // Add the key's character to the output
                    output += k.KeyChar;
                    // Print an asterisk to the console
                    Console.Write("*");
                }
            }

            Console.WriteLine("");
            return output;
        }

        /// <summary>
        /// Fix a string to a certain length. If the string is shorter than the length, spaces are added onto the end. If the string is too long, characters are removed.
        /// </summary>
        /// <param name="input">The string to shorten/lengthen.</param>
        /// <param name="length">The length of the new string.</param>
        /// <returns></returns>
        public static string FixStringLength(string input, int length)
        {
            return input.Substring(0, input.Length < length ? input.Length : length).PadRight(length);
        }

        public static void Print(string text, string color = "Default", bool newline = true)
        {
            Console.ForegroundColor = ThemeColors[color];
            Console.Write($"{text}{(newline ? "\n" : "")}");
            Console.ForegroundColor = ThemeColors["Default"];
        }
    }
}
