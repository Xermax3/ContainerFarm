/*
 * Team 2 - Grass Touchers
 * Winter 2022 - May 17th
 * Application Development III
 * View model for displaying information about a record.
*/

using Final_Project_Application.ViewModels;
using GrassTouchersApp.Enums;
using GrassTouchersApp.Extensions;
using GrassTouchersApp.Models;
using Microcharts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GrassTouchersApp.ViewModels
{
    /// <summary> View model for displaying information about an record. </summary>
    public class RecordViewModel : ViewModel
    {
        private readonly SensorRecord record;

        private BubblingObservableCollection<EntryViewModel> entries;
        public BubblingObservableCollection<EntryViewModel> Entries
        {
            get => entries;
            private set
            {
                entries = value;
                OnPropertyChanged();
            }
        }

        //public EntryViewModel Latest { get => Entries.First(); }

        public SubsystemTypes Subsystem
        {
            get => record.Subsystem;
            set
            {
                if (record.Subsystem != value)
                {
                    record.Subsystem = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Field
        {
            get => record.Field;
            set
            {
                if (record.Field != value)
                {
                    record.Field = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Unit
        {
            get => record.Unit;
            set
            {
                if (record.Unit != value)
                {
                    record.Unit = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary> Gets the value of the latest entry for this field. </summary>
        public string LatestValue
        {
            get
            {
                if (Entries.Count > 0)
                    return Entries.First().Value;
                return "EMPTY RECORD";
            }
        }

        /// <summary> Gets the value and unit of the latest entry for this field. </summary>
        public string LatestStringValue
        {
            get
            {
                if (Entries.Count > 0)
                    return Entries.First().StringValue; // Can also do Entries.First.Value() + Unit
                return "EMPTY RECORD";
            }
        }

        /// <summary> Gets the entry date of the latest entry for this field. </summary>
        public DateTime LatestEntryDate
        {
            get
            {
                if (Entries.Count > 0)
                    return Entries.First().EntryDate;
                return DateTime.MinValue;
            }
        }

        private bool isDisplayingDetails;
        /// <summary> True if the details page for that record is being shown </summary>
        public bool IsDisplayingDetails
        {
            get => isDisplayingDetails;
            set
            {
                isDisplayingDetails = value;
                if (value)
                {
                    GenerateLineChart();
                }
            }
        }

        private LineChart graph;
        /// <summary> SkiaChart line graph representation of the entries. </summary>
        public LineChart Graph
        {
            get => graph;
            private set
            {
                graph = value;
                OnPropertyChanged();
            }
        }

        /// <summary> Create a new record of entries for a field. </summary>
        /// <param name="subsystem"> The subsystem that recorded the entry </param>
        /// <param name="field"> The entry's field </param>
        /// <param name="unit"> The unit for the entry's field (if set to null, will be automatically set) </param>
        public RecordViewModel(SubsystemTypes subsystem, string field, string unit = null)
        {
            Entries = new BubblingObservableCollection<EntryViewModel>();
            record = new SensorRecord(subsystem, field, unit);
        }

        /// <summary> Create a new entry and add it to the record. </summary>
        /// <param name="value"> The entry's value </param>
        /// <param name="entryDate"> The time when the entry was recorded </param>
        public void Add(string value, DateTime entryDate)
        {
            // Avoid duplicates: Only add if the timestamp does not match one of the entries recorded.
            if (FindDuplicate(entryDate))
                return;

            SensorEntry entry = new SensorEntry(value, entryDate);
            Entries.Insert(0, new EntryViewModel(this, entry));

            // No need to sort entries since every new entry will always be the most recent
            //Entries = new BubblingObservableCollection<EntryViewModel>(Entries.OrderByDescending(f => f.EntryDate));

            // Update the graph if the user is one the details page
            if (IsDisplayingDetails)
                GenerateLineChart();
        }

        private bool FindDuplicate(DateTime entryDate)
        {
            foreach (EntryViewModel entry in Entries)
            {
                if (entryDate == entry.EntryDate)
                    return true;
            }
            return false;
        }

        private void GenerateLineChart()
        {
            const int MAX_ENTRIES = 8;

            List<ChartEntry> chartEntries = new List<ChartEntry>();

            for (int i = Math.Min(Entries.Count - 1, MAX_ENTRIES - 1); i >= 0; i--)
            {
                EntryViewModel entry = Entries[i];
                float result;
                // If the field uses numeric values, proceed.
                if (!float.TryParse(entry.Value, out result))
                {
                    // If the field uses binary values: Convert them to 0s and 1s.
                    result = entry.Value.ToBinaryInt();
                }
                chartEntries.Add(new ChartEntry(result)
                {
                    Label = entry.EntryDate.ToString("HH:mm"),
                    ValueLabel = entry.Value,
                });
            }

            LineChart chart = new LineChart()
            {
                Entries = chartEntries,
                BackgroundColor = SKColor.Empty,
                LabelTextSize = 32,
                LabelOrientation = Orientation.Horizontal,
                ValueLabelOrientation = Orientation.Horizontal,
            };

            Graph = chart;
        }
    }
}
