using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemonade_Stand
{
    public static class UiUtils
    {
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
                Console.WriteLine($"{prompt}");
                Console.WriteLine(
                    "Press the up and down arrow keys to change items and the enter key to confirm a selection.");

                for (var i = 0; i < items.Length; i++) Console.WriteLine($" {(i == current ? '>' : '-')} {items[i]}");

                var k = Console.ReadKey(true);

                switch (k.Key)
                {
                    case ConsoleKey.UpArrow when current > 0:
                        current -= 1;
                        break;
                    case ConsoleKey.DownArrow when current + 1 < items.Length:
                        current += 1;
                        break;
                    case ConsoleKey.Enter:
                        done = true;
                        break;
                }
            }

            return items[current];
        }

        /// <summary>
        /// Asks the user a question, validates the response then returns a validated value.
        /// </summary>
        /// <param name="prompt">The question to display to the user</param>
        /// <param name="type">The type that needs to be returned (e.g string, int, float)</param>
        /// <param name="checkEmpty">Whether the response shouldn't be empty</param>
        /// <param name="password">Whether the user's input should be masked</param>
        /// <returns></returns>
        public static object Field(string prompt, string type = "string", bool checkEmpty = true, bool password = false)
        {
            Console.WriteLine(prompt);

            var valid = false;
            object output = false;

            while (!valid)
            {
                Console.Write(" -> ");
                var response = password ? GetPassword() : Console.ReadLine();
                valid = true;

                if (string.IsNullOrWhiteSpace(response) && checkEmpty)
                {
                    Console.WriteLine("     - Your input must not be empty.");
                    valid = false;
                }

                switch (type)
                {
                    case "int":
                    {
                        if (!int.TryParse(response, out var intOutput))
                        {
                            Console.WriteLine("     - Your input must be an integer.");
                            valid = false;
                            continue;
                        }

                        output = intOutput;
                        break;
                    }
                    case "float":
                    {
                        if (!float.TryParse(response, out var floatOutput))
                        {
                            Console.WriteLine("     - Your input must be a float.");
                            valid = false;
                            continue;
                        }

                        output = floatOutput;
                        break;
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

                switch (k.Key)
                {
                    // If the key is enter, break the loop
                    case ConsoleKey.Enter:
                        done = true;
                        break;
                    // If the key is backspace and the user has typed something...
                    case ConsoleKey.Backspace when output.Length > 0:
                        // Remove the last character from the output string
                        output = output.Substring(0, output.Length - 1);
                        // Go back a character, print a space then go back another character to ensure that the character is replaced by a space.
                        Console.Write("\b \b");
                        break;
                    // For any other key...
                    default:
                        // Add the key's character to the output
                        output += k.KeyChar;
                        // Print an asterisk to the console
                        Console.Write("*");
                        break;
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
    }
}
