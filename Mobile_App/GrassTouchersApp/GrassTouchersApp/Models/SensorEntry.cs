/*
 * Team 2 - Grass Touchers
 * Winter 2022 - May 15th
 * Application Development III
 * An individual entry from any subsystem.
*/

using System;

namespace GrassTouchersApp.Models
{
    /// <summary> An individual entry from any subsystem. </summary>
    public class SensorEntry
    {
        public string Value { get; set; }
        public DateTime EntryDate { get; set; }

        /// <summary> Create a new entry and assign it to a record (however it does not get added). </summary>
        /// <param name="value"> The entry's value </param>
        /// <param name="entryDate"> The time when the entry was recorded </param>
        public SensorEntry(string value, DateTime entryDate)
        {
            Value = value;
            EntryDate = entryDate;
        }
    }
}
