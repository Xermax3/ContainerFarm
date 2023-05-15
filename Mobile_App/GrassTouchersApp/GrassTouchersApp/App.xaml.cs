/*
 * Team 2 - Grass Touchers
 * Winter 2022 - May 3rd
 * Application Development III
 * Back-end code for the application.
*/

using Final_Project_Application.ViewModels;
using GrassTouchersApp.Views;
using System.Threading;
using Xamarin.Forms;

namespace GrassTouchersApp
{
    /// <summary> The current application running. </summary>
    public partial class App : Application
    {
        private Thread recordsThread;

        /// <summary> The application's main view model, containing every subsystem's view model. </summary>
        public static MainViewModel MainViewModel { get; private set; } = new MainViewModel();

        /// <summary> Initialize a new application, starting at the login page. </summary>
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new LoginPage());
            recordsThread = new Thread(MainViewModel.PopulateRecords);
        }

        protected override void OnStart()
        {
            recordsThread.Start();
        }

        protected override void OnSleep()
        {
            //recordsThread.Abort();
        }

        protected override void OnResume()
        {
            //if (!recordsThread.IsAlive)
            //{
            //    recordsThread = new Thread(MainViewModel.PopulateRecords);
            //    recordsThread.Start();
            //}
        }
    }
}
