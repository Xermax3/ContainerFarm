/*
 * Team 2 - Grass Touchers
 * Winter 2022 - May 15th
 * Application Development III
 * Utility class to contain an entry's string values prior to being converted.
*/

namespace GrassTouchersApp.Models
{
    /// <summary> Utility class to contain an entry's string values prior to being converted. </summary>
    public class Payload
    {
        public string Subsystem { get; set; }
        public string Field { get; set; }
        public string Value { get; set; }
        public string EntryDate { get; set; }
    }
}
