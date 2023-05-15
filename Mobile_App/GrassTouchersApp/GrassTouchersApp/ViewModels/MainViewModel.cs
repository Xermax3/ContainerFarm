/*
 * Team 2 - Grass Touchers
 * Winter 2022 - May 3rd
 * Application Development III
 * The application's main view model, containing every subsystem's view model.
*/

using GrassTouchersApp.Enums;
using GrassTouchersApp.Extensions;
using GrassTouchersApp.Hub;
using GrassTouchersApp.Models;
using GrassTouchersApp.ViewModels;
using GrassTouchersApp.Views;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Final_Project_Application.ViewModels
{
    /// <summary> The application's main view model, containing every subsystem's view model. </summary>
    public class MainViewModel : ViewModel
    {
        private static EventHubClient _client;

        /// <summary> The list of all the records </summary>
        public BubblingObservableCollection<RecordViewModel> Records { get; private set; }

        /// <summary> This application's farm technician view model. </summary>
        public FarmTechnicianDashboardViewModel FarmTechnicianDashboard { get; private set; }

        /// <summary> This application's fleet manager view model. </summary>
        public FleetManagerDashboardViewModel FleetManagerDashboard { get; private set; }

        /// <summary> The current dashboard being displayed. Returns null if on the login screen. </summary>
        public DashboardViewModel CurrentDashboard { get; set; }

        private bool isConnected;
        /// <summary> True if the device is currently connected to the internet. Use CheckConnection to refresh the value. </summary>
        public bool IsConnected
        {
            get { return isConnected; }
            private set
            {
                if (isConnected != value)
                {
                    isConnected = value;
                    OnPropertyChanged();
                }
            }
        }


        /// <summary> Command to navigate to the farm technician dashboard. </summary>
        public ICommand FarmTechnicianLogin { get; private set; }

        /// <summary> Command to navigate to the fleet manager dashboard. </summary>
        public ICommand FleetManagerLogin { get; private set; }

        /// <summary> Command to navigate to the location page. </summary>
        public ICommand LocationPage { get; private set; }

        private async void FarmTechnicianLoginCommand()
        {
            CurrentDashboard = FarmTechnicianDashboard;
            CheckConnection();
            await Navigation.PushAsync(new DashboardPage(FarmTechnicianDashboard));
        }

        private async void FleetManagerLoginCommand()
        {
            CurrentDashboard = FleetManagerDashboard;
            CheckConnection();
            await Navigation.PushAsync(new DashboardPage(FleetManagerDashboard));
        }

        private async void LocationPageCommand()
        {
            // Proceed if there is a latitude and longitude record. Otherwise warn the user.
            if (FleetManagerDashboard.HasField("Latitude") && FleetManagerDashboard.HasField("Longitude"))
                await Navigation.PushAsync(new LocationPage());
            else
                await Application.Current.MainPage.DisplayAlert("Cannot load Location Page", "You must receive latitude and longitude data to see that page.", "OK");
        }

        /// <summary> Create a new set of subsystem view models based on the repo. </summary>
        public MainViewModel()
        {
            _client = new EventHubClient();
            Records = new BubblingObservableCollection<RecordViewModel>();
            FarmTechnicianDashboard = new FarmTechnicianDashboardViewModel();
            FleetManagerDashboard = new FleetManagerDashboardViewModel();
            FarmTechnicianLogin = new Command(FarmTechnicianLoginCommand);
            FleetManagerLogin = new Command(FleetManagerLoginCommand);
            LocationPage = new Command(LocationPageCommand);
        }

        /// <summary> Populate the entry record with the data from IoTHub. </summary>
        public async void PopulateRecords()
        {
            await foreach (List<Payload> payloads in _client.Read<List<Payload>>())
            {
                foreach (Payload payload in payloads)
                    AddEntry(payload);
            }
        }

        /// <summary> Create a new entry using a Payload utility class and add it to the record list. </summary>
        /// <param name="payload"> A payload object with valid string values for every field. </param>
        private void AddEntry(Payload payload)
        {
            // Try to convert the subsystem string into an enum variant. Otherwise, do not add the entry.
            SubsystemTypes subsystem;
            try
            {
                subsystem = payload.Subsystem.ToEnum<SubsystemTypes>();
            }
            catch
            {
                return;
            }

            // Get the date time by converting the string with the given format.
            DateTime entryDate = DateTime.Now;
            if (DateTime.TryParseExact(payload.EntryDate, "MM/dd/yyyy, HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
                entryDate = parsedDate;

            // FOR TESTING: Only include entries from the last x minutes.
            //if (parsedDate.CompareTo(DateTime.Now.AddMinutes(-20)) < 1)
            //    return;

            // Add the entry normally.
            AddRecord(subsystem, payload.Field, payload.Value, entryDate);
        }

        private void AddRecord(SubsystemTypes subsystem, string field, string value, DateTime entryDate)
        {
            RecordViewModel record = FindOrCreateRecord(subsystem, field.Capitalize());
            record.Add(FormatIfFloat(value.Capitalize()), entryDate);

            // Update the dashboard
            if (CurrentDashboard != null)
                CurrentDashboard.ManuallyRefresh();
        }

        /// <summary> Try to find a record for the desired field. If it doesn't exist, create one. </summary>
        /// <param name="subsystem"> The record's subsystem </param>
        /// <param name="field"> The record's desired field </param>
        /// <returns> The record instance for the field </returns>
        private RecordViewModel FindOrCreateRecord(SubsystemTypes subsystem, string field)
        {
            try
            {
                return Records.First(r => r.Field == field);
            }
            catch
            {
                RecordViewModel record = new RecordViewModel(subsystem, field);
                Records.Add(record);
                return record;
            }
        }

        /// <summary> If the entry value is a float, the entry should display two digits after the decimal point. </summary>
        /// <param name="value"> The potential float value </param>
        /// <returns> If a float, returns the trimmed value. Otherwise, returns the value as is. </returns>
        private static string FormatIfFloat(string value)
        {
            if (float.TryParse(value, out float trimmedFloatValue) && !int.TryParse(value, out _))
                return trimmedFloatValue.ToString("0.00");
            return value;
        }

        /// <summary> Refreshes the IsConnected property to display if the applicaton's device is currently connected to the internet. </summary>
        public void CheckConnection()
        {
            switch (Connectivity.NetworkAccess)
            {
                case NetworkAccess.Internet:
                case NetworkAccess.ConstrainedInternet:
                case NetworkAccess.Local:
                    IsConnected = true;
                    break;
                default:
                    IsConnected = false;
                    break;
            }
        }
    }
}
