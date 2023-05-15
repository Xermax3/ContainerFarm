/*
 * Team 2 - Grass Touchers
 * Winter 2022 - May 15th
 * Application Development III
 * New method overrides for string values.
*/

using System;

namespace GrassTouchersApp.Extensions
{
    /// <summary> New method overrides for string values. </summary>
    public static class StringExtensions
    {
        /// <summary> Capitalizes the first letter of the string. </summary>
        /// <param name="s"> The uncapitalized string (leave empty to use the string itself) </param>
        /// <returns> The capitalized string </returns>
        public static string Capitalize(this string s) =>
          s switch
          {
              null => throw new ArgumentNullException(nameof(s)),
              "" => throw new ArgumentException($"{nameof(s)} cannot be empty", nameof(s)),
              _ => s[0].ToString().ToUpper() + s.Substring(1)
          };

        /// <summary> Converts the string into a value of the provided Enum. If cannot be parsed, the -1 Enum is returned. </summary>
        /// <typeparam name="T"> An Enum that contains a value equal to the string. </typeparam>
        /// <param name="value"> The string to convert into Enum (leave empty to use the string itself) </param>
        /// <returns> The Enum value matching the string </returns>
        public static T ToEnum<T>(this string value)
        {
            try
            {
                return (T)Enum.Parse(typeof(T), value, true);
            }
            catch
            {
                // If parsing doesn't work
                return (T)Enum.GetValues(typeof(T)).GetValue(-1);
            }
        }

        /// <summary> Returns a binary interpretation of the string. </summary>
        /// <param name="value"> The string to refer to (leave empty to use the string itself) </param>
        /// <returns> The binary interpretation of the string </returns>
        public static bool ToBinary(this string value)
        {
            switch (value.ToLower())
            {
                case "on":
                case "open":
                case "true":
                case "detected":
                    return true;
                case "off":
                case "closed":
                case "false":
                case "none":
                    return false;
                default:
                    throw new ArgumentException($"Error: The string {value} could not be converted into a binary value.");
            }
        }

        /// <summary> Returns a binary interpretation of the string as an int. </summary>
        /// <param name="value"> The string to refer to (leave empty to use the string itself) </param>
        /// <returns> The binary interpretation of the string (0 or 1) </returns>
        public static int ToBinaryInt(this string value)
        {
            return value.ToBinary() ? 1 : 0;
        }
    }
}