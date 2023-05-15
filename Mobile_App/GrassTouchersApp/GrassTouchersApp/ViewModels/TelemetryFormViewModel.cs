using Final_Project_Application.ViewModels;
using GrassTouchersApp.Hub;
using System.Windows.Input;
using Xamarin.Forms;

namespace GrassTouchersApp.ViewModels
{
    public class TelemetryFormViewModel : ViewModel
    {
        private int telemetryinterval;
        public int TelemetryInterval
        {
            get { return telemetryinterval; }
            set
            {
                if (telemetryinterval != value)
                {
                    telemetryinterval = value;
                    OnPropertyChanged();
                }
            }
        }
        private static EventHubClient _client;

        public TelemetryFormViewModel()
        {
            _client = new EventHubClient();
            UpdateTelemetry = new Command(UpdateIntervalCommand);
        }

        public async void GetTelemetryInterval()
        {
            TelemetryInterval = await _client.GetTelemetryInterval();
        }

        public ICommand UpdateTelemetry { get; private set; }
        private async void UpdateIntervalCommand()
        {
            if (telemetryinterval > 0)
            {
                _client.UpdateTelemetryInterval(TelemetryInterval);
            }
            else
            {
                TelemetryInterval = await _client.GetTelemetryInterval();
            }
        }
    }
}
