/*
 * Team 2 - Grass Touchers
 * Winter 2022 - May 15th
 * Application Development III
 * Back-end code for dashboard pages.
*/

using GrassTouchersApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GrassTouchersApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DashboardPage : ContentPage
    {
        private readonly DashboardViewModel _vm;

        /// <summary> Generate a new dashboard using a provided dashboard view model </summary>
        /// <param name="dashboardVM"> The view model from which the dashboard will display its entries </param>
        public DashboardPage(DashboardViewModel dashboardVM)
        {
            InitializeComponent();
            BindingContext = dashboardVM;
            _vm = dashboardVM;
            dashboardVM.Navigation = Navigation;

            if (dashboardVM == App.MainViewModel.FleetManagerDashboard)
            {
                ToolbarItems.Add(new ToolbarItem()
                {
                    IconImageSource = "globe90.png",
                    Command = App.MainViewModel.LocationPage,
                });
            }
        }

        protected override void OnDisappearing()
        {
            App.MainViewModel.CurrentDashboard = null;
            base.OnDisappearing();
        }

        private void OnDataSelected(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as CollectionView).SelectedItem != null)
            {
                Navigation.PushAsync(new DetailsPage(_vm.SelectedRecord));
                (sender as CollectionView).SelectedItem = null;
            }
        }
    }
}