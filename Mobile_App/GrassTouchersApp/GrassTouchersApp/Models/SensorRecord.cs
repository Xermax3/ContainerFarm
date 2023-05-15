/*
 * Team 2 - Grass Touchers
 * Winter 2022 - May 15th
 * Application Development III
 * A record of entries for a subsystem.
*/

using GrassTouchersApp.Enums;
using System;
using System.Collections.Generic;

namespace GrassTouchersApp.Models
{
    /// <summary> A record of entries for a subsystem. </summary>
    public class SensorRecord
    {
        /// <summary> Dictionary of unit values for every entry type. </summary>
        public static Dictionary<string, string> Units = new Dictionary<string, string>()
        {
            [Enum.GetName(typeof(Sensors), Sensors.Temperature)] = " °C",
            [Enum.GetName(typeof(Sensors), Sensors.Humidity)] = " %rh",
            [Enum.GetName(typeof(Sensors), Sensors.Water)] = " ml",
            [Enum.GetName(typeof(Sensors), Sensors.Moisture)] = " %rh",
            [Enum.GetName(typeof(Sensors), Sensors.Fan)] = "",
            [Enum.GetName(typeof(Sensors), Sensors.Light)] = "",
            [Enum.GetName(typeof(Sensors), Sensors.Latitude)] = "",
            [Enum.GetName(typeof(Sensors), Sensors.Longitude)] = "",
            [Enum.GetName(typeof(Sensors), Sensors.Pitch)] = "",
            [Enum.GetName(typeof(Sensors), Sensors.Roll)] = "",
            [Enum.GetName(typeof(Sensors), Sensors.Vibration)] = " Hz",
            [Enum.GetName(typeof(Sensors), Sensors.Motion)] = "",
            [Enum.GetName(typeof(Sensors), Sensors.Noise)] = " dB",
            [Enum.GetName(typeof(Sensors), Sensors.Luminosity)] = " Iv",
            [Enum.GetName(typeof(Sensors), Sensors.Buzzer)] = "",
            [Enum.GetName(typeof(Sensors), Sensors.Door)] = "",
            [Enum.GetName(typeof(Sensors), Sensors.Lock)] = "",
        };

        public SubsystemTypes Subsystem { get; set; }
        public string Field { get; set; }
        public string Unit { get; set; }
        public object Entries { get; internal set; }

        /// <summary> Create a new record of entries for a field. </summary>
        /// <param name="subsystem"> The subsystem that recorded the entry </param>
        /// <param name="field"> The entry's field </param>
        /// <param name="unit"> The unit for the entry's field (if set to null, will be automatically set) </param>
        public SensorRecord(SubsystemTypes subsystem, string field, string unit = null)
        {
            Subsystem = subsystem;
            Field = field;
            Unit = unit ?? Units[field] ?? "";
        }
    }
}
