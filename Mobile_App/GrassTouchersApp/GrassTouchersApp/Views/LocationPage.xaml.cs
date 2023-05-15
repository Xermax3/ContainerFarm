/*
 * Team 2 - Grass Touchers
 * Winter 2022 - May 18th
 * Application Development III
 * Back-end code for location pages.
*/

using GrassTouchersApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace GrassTouchersApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LocationPage : ContentPage
    {
        private readonly FleetManagerDashboardViewModel _vm;

        public LocationPage()
        {
            InitializeComponent();
            _vm = App.MainViewModel.FleetManagerDashboard;
            BindingContext = _vm;

            // Generate map ( Source: https://docs.microsoft.com/en-us/xamarin/xamarin-forms/user-interface/map/map )
            Position position = new Position(_vm.Latitude, _vm.Longitude);
            MapSpan mapSpan = new MapSpan(position, 0.8, 0.8);
            Pin pin = new Pin()
            {
                Label = "Your Geolocation Subsystem",
                Type = PinType.Place,
                Position = position,
            };
            Map newmap = new Map()
            {
                HasScrollEnabled = true,
                HasZoomEnabled = true,
            };
            AbsoluteLayout.SetLayoutBounds(newmap, new Rectangle(0, 0, 1, 1));
            AbsoluteLayout.SetLayoutFlags(newmap, AbsoluteLayoutFlags.All);
            newmap.Pins.Add(pin);
            newmap.MoveToRegion(mapSpan);
            layout.Children.Add(newmap);
            layout.LowerChild(newmap);
        }

        //protected override void OnAppearing()
        //{
        //    _vm.IsDisplayingLocation = true;
        //    base.OnAppearing();
        //}
        //protected override void OnDisappearing()
        //{
        //    _vm.IsDisplayingLocation = false;
        //    base.OnDisappearing();
        //}
    }
}