/*
 * Team 2 - Grass Touchers
 * Winter 2022 - May 3rd
 * Application Development III
 * Generic view model that gets notified when a property changes. All other view models should inherit it.
*/

using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace Final_Project_Application.ViewModels
{
    /// <summary> Generic view model that gets notified when a property changes. </summary>
    public class ViewModel : INotifyPropertyChanged
    {
        /// <summary> Event triggered when a property changes. </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary> Navigation interface implemented by INotifyPropertyChanged. </summary>
        public INavigation Navigation { get; set; }

        /// <summary> Call this method from a child class to notify that a change happened on a property. </summary>
        /// <param name="name"> The name of the property that changed. </param>
        /// <returns> None </returns>
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
