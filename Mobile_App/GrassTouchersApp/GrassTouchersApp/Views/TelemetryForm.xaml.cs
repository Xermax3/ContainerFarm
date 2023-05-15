using GrassTouchersApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GrassTouchersApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TelemetryForm : ContentPage
    {
        public TelemetryFormViewModel model;
        public TelemetryForm()
        {
            model = new TelemetryFormViewModel();
            BindingContext = model;
            model.GetTelemetryInterval();
            InitializeComponent();
        }
    }
}