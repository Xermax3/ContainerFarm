/*
 * Team 2 - Grass Touchers
 * Winter 2022 - May 15th
 * Application Development III
 * Dashboard for fleet manager.
*/

using GrassTouchersApp.Enums;
using System.Collections.Generic;
using System.Linq;

namespace GrassTouchersApp.ViewModels
{
    /// <summary> Dashboard for fleet manager. </summary>
    public sealed class FleetManagerDashboardViewModel : DashboardViewModel
    {
        /// <summary> Latest entry for the latitude value. Throws if not found. </summary>
        public double Latitude => GetFloatEntry("Latitude");

        /// <summary> Latest entry for the longitude value. Throws if not found. </summary>
        public double Longitude => GetFloatEntry("Longitude");

        private double GetFloatEntry(string field)
        {
            RecordViewModel record = Records.First(r => r.Field == field);
            return double.Parse(record.Entries.First().Value);
        }

        public FleetManagerDashboardViewModel() : base(
            "Manager",
            new SubsystemTypes[] { SubsystemTypes.Location, SubsystemTypes.Security },
            new List<string>() { "lock", "buzzer", }) { }

        //private bool isDisplayingLocation;
        ///// <summary> True if the location page is being shown </summary>
        //public bool IsDisplayingLocation
        //{
        //    get => isDisplayingLocation;
        //    set
        //    {
        //        isDisplayingLocation = value;
        //        if (value)
        //        {
        //            GenerateMap();
        //        }
        //    }
        //}

        //private Map map;
        ///// <summary> Map representing the current location of the geo subsystem. </summary>
        //public Map Map
        //{
        //    get => map;
        //    private set
        //    {
        //        map = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private void GenerateMap()
        //{
        //    Position position = new Position(GetFloatEntry("Latitude"), GetFloatEntry("Longitude"));
        //    MapSpan mapSpan = new MapSpan(position, 0.8, 0.8);
        //    Pin pin = new Pin()
        //    {
        //        Label = "Your Geolocation Subsystem",
        //        Type = PinType.Place,
        //        Position = position,
        //    };
        //    Map newmap = new Map()
        //    {
        //        HasScrollEnabled = true,
        //        HasZoomEnabled = true,
        //    };
        //    newmap.Pins.Add(pin);
        //    newmap.MoveToRegion(mapSpan);
        //    Map = newmap;
        //}
    }
}
