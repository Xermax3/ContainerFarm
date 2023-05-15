/*
 * Team 2 - Grass Touchers
 * Winter 2022 - May 17th
 * Application Development III
 * View model for displaying information about an entry.
*/

using Final_Project_Application.ViewModels;
using GrassTouchersApp.Enums;
using GrassTouchersApp.Models;
using System;

namespace GrassTouchersApp.ViewModels
{
    /// <summary> View model for displaying information about an entry. </summary>
    public class EntryViewModel : ViewModel
    {
        private readonly SensorEntry entry;
        private readonly RecordViewModel record;

        public string Value
        {
            get => entry.Value;
            set
            {
                if (entry.Value != value)
                {
                    entry.Value = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime EntryDate
        {
            get => entry.EntryDate;
            set
            {
                if (entry.EntryDate != value)
                {
                    entry.EntryDate = value;
                    OnPropertyChanged();
                }
            }
        }

        public string StringValue { get => entry.Value + record.Unit; }

        public SubsystemTypes Subsystem { get => record.Subsystem; }

        public string Field { get => record.Field; }

        /// <summary> Create a new view model based on the provided entry. </summary>
        /// <param name="record"> The record that contains the entry. </param>
        /// /// <param name="entry"> The entry to base the view model on. </param>
        public EntryViewModel(RecordViewModel record, SensorEntry entry)
        {
            this.entry = entry;
            this.record = record;
        }
    }
}
