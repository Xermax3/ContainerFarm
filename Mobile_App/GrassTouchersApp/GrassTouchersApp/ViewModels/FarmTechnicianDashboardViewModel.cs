/*
 * Team 2 - Grass Touchers
 * Winter 2022 - May 15th
 * Application Development III
 * Dashboard for farm technician.
*/

using GrassTouchersApp.Enums;
using System.Collections.Generic;

namespace GrassTouchersApp.ViewModels
{
    /// <summary> Dashboard for farm technician. </summary>
    public sealed class FarmTechnicianDashboardViewModel : DashboardViewModel
    {
        public FarmTechnicianDashboardViewModel() : base(
            "Technician",
            new SubsystemTypes[] { SubsystemTypes.Plant, SubsystemTypes.Security },
            new List<string>() { "fan", "light", "buzzer" }
        ) { }
    }
}
