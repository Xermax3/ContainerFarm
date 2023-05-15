/*
 * Team 2 - Grass Touchers
 * Winter 2022 - May 15th
 * Application Development III
 * Back-end code for details pages.
*/

using GrassTouchersApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GrassTouchersApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetailsPage : ContentPage
    {
        private RecordViewModel _vm;

        /// <summary> Generate a details page using for a provided record </summary>
        /// <param name="record"> The record from which the details page will display its data </param>
        public DetailsPage(RecordViewModel record)
        {
            InitializeComponent();
            BindingContext = record;
            _vm = record;
        }

        protected override void OnAppearing()
        {
            _vm.IsDisplayingDetails = true;
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            _vm.IsDisplayingDetails = false;
            base.OnDisappearing();
        }
    }
}