/*
 * Team 2 - Grass Touchers
 * Winter 2022 - May 15th
 * Application Development III
 * Every entry type sent by all subsystems.
*/

namespace GrassTouchersApp.Enums
{
    /// <summary> Every entry type sent by all subsystems. </summary>
    public enum Sensors
    {
        // Plant
        Temperature,
        Humidity,
        Water,
        Moisture,
        Fan,
        Light,

        // Geo-location
        Latitude,
        Longitude,
        Pitch,
        Roll,
        Vibration,

        // Security
        Motion,
        Noise,
        Luminosity,
        Buzzer,
        Door,
        Lock,

        Unknown = -1
    }
}
