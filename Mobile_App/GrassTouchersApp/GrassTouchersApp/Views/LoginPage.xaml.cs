/*
 * Team 2 - Grass Touchers
 * Winter 2022 - May 3rd
 * Application Development III
 * Back-end code for the login page.
*/

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GrassTouchersApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        /// <summary> Load a new login page. </summary>
        public LoginPage()
        {
            InitializeComponent();
            BindingContext = App.MainViewModel;
            App.MainViewModel.Navigation = Navigation;
        }
    }
}