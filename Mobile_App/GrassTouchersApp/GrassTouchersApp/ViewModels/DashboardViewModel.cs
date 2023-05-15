/*
 * Team 2 - Grass Touchers
 * Winter 2022 - May 15th
 * Application Development III
 * View model for displaying a dashboard based on a user role.
*/

using Final_Project_Application.ViewModels;
using GrassTouchersApp.Enums;
using GrassTouchersApp.Extensions;
using GrassTouchersApp.Hub;
using GrassTouchersApp.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace GrassTouchersApp.ViewModels
{
    /// <summary> View model for displaying a dashboard based on a user role. </summary>
    public class DashboardViewModel : ViewModel
    {
        private bool _isBusy;
        private readonly SubsystemTypes[] _types;

        private List<string> _actuators;
        private Controller _actuatorController;
        private string _selectedActuator;
        private string _selectedState;
        private readonly Dictionary<string, List<string>> _validActuatorStates;

        public bool IsConnected
        {
            get => App.MainViewModel.IsConnected;
        }

        public BubblingObservableCollection<RecordViewModel> Records
        {
            get => App.MainViewModel.Records.Filter(_types);
            set => OnPropertyChanged();
        }

        public RecordViewModel SelectedRecord { get; set; }

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }

        public string Title { get; private set; }

        public List<string> Actuators
        {
            get => _actuators.Select(actuator => actuator.Capitalize()).ToList();
            set => _actuators = value;
        }

        public string SelectedActuator
        {
            get => _selectedActuator;
            set
            {
                _selectedActuator = value;
                OnPropertyChanged();
            }
        }

        public string SelectedState
        {
            get => _selectedState;
            set
            {
                _selectedState = value;
                OnPropertyChanged();
            }
        }

        public ICommand RefreshEntries { get; private set; }
        public ICommand ControlActuator { get; private set; }
        public ICommand UpdateTelemetry { get; private set; }

        private void RefreshEntriesExecute()
        {
            try
            {
                IsBusy = true;
                Records = null;
                //App.MainViewModel.CheckConnection();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void ControlActuatorExecute()
        {
            if (!_validActuatorStates[_selectedActuator.ToLower()].Contains(_selectedState.ToLower()))
            {
                App
                 .MainViewModel
                 .Navigation
                 .NavigationStack
                 .Last()
                 .DisplayAlert(
                   "Error",
                   $"Invalid state: {_selectedState}.\n\nPossible states: {string.Join(", ", _validActuatorStates[_selectedActuator.ToLower()])}",
                   "OK"
                 );
            }
            else
            {
                try
                {
                    _actuatorController.Invoke(
                        _selectedActuator.ToLower(),
                        new Dictionary<string, string>() { { "state", _selectedState.ToLower() }
                    });
                }
                // Hack: squash any errors
                catch (Exception e)
                {
                    Application.Current.MainPage.DisplayAlert("Direct Method Error",
                            $"An error occured while trying to send your direct method. Try again.",
                            "OK"
                        );
                    return;
                }

                Application.Current.MainPage.DisplayAlert("Direct Method Successful",
                        $"Your direct method was sent successfully!",
                        "OK"
                    );
            }
        }

        private void UpdateIntervalExecute()
        {
            Navigation.PushAsync(new TelemetryForm());
        }

        /// <summary> Create a dashboard view model based on a provided list of subsystems and actuators. </summary>
        /// <param name="title"> The title of the dashboard </param>
        /// <param name="subsystems"> List of subsystem to display records from </param>
        /// <param name="actuators"> List of actuators that the user has access to </param>
        public DashboardViewModel(string title, SubsystemTypes[] subsystems, List<string> actuators)
        {
            Title = title;
            _types = subsystems;
            _actuators = actuators;

            _actuatorController = new Controller();

            _validActuatorStates = new Dictionary<string, List<string>>() {
                { "fan", new List<string>() { "on", "off" } },
                { "light", new List<string>() { "on", "off" } },
                { "buzzer", new List<string>() { "on", "off" } },
                { "lock", new List<string>() { "open", "closed" } },
            };

            _selectedActuator = _actuators.First();
            _selectedState = "";

            RefreshEntries = new Command(RefreshEntriesExecute);
            ControlActuator = new Command(ControlActuatorExecute);
            UpdateTelemetry = new Command(UpdateIntervalExecute);
        }

        /// <summary> Returns whether the dashboard has a record for a provided field. </summary>
        /// <param name="field"> The desired field </param>
        /// <returns> True if the dashboard contains a record for the field </returns>
        public bool HasField(string field)
        {
            return Records.Any(r => r.Field == field);
        }

        /// <summary> Reload the record list. </summary>
        public void ManuallyRefresh()
        {
            Records = App.MainViewModel.Records.Filter(_types);
        }
    }
}
