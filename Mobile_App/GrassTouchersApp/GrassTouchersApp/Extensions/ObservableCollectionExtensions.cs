/*
 * Team 2 - Grass Touchers
 * Winter 2022 - May 15th
 * Application Development III
 * New method overrides for observable collections.
*/

using GrassTouchersApp.Enums;
using GrassTouchersApp.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace GrassTouchersApp.Extensions
{
    // Source: https://stackoverflow.com/questions/1427471/observablecollection-not-noticing-when-item-in-it-changes-even-with-inotifyprop
    /// <summary> ObervableCollection that notifies when any of the items has a property changed. </summary>
    /// <typeparam name="T"> The ObservableCollection item type </typeparam>
    public sealed class BubblingObservableCollection<T> : ObservableCollection<T>
    where T : INotifyPropertyChanged
    {
        public BubblingObservableCollection()
        {
            CollectionChanged += FullObservableCollectionCollectionChanged;
        }

        public BubblingObservableCollection(IEnumerable<T> pItems) : this()
        {
            foreach (var item in pItems)
            {
                this.Add(item);
            }
        }

        private void FullObservableCollectionCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (var item in e.OldItems)
                {
                    ((INotifyPropertyChanged)item).PropertyChanged -= ItemPropertyChanged;
                }
            }
            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems)
                {
                    ((INotifyPropertyChanged)item).PropertyChanged += ItemPropertyChanged;
                }
            }
        }

        private void ItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            NotifyCollectionChangedEventArgs args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, sender, sender, IndexOf((T)sender));
            OnCollectionChanged(args);
        }
    }

    /// <summary> New method overrides for observable collections. </summary>
    public static class ObservableCollectionExtensions
    {
        /// <summary> Retrieve every record for the desired subsystems, order by subsystem and field. </summary>
        /// <param name="collection"> Record list </param>
        /// <param name="types"> List of subsystems to get data from </param>
        /// <returns> The filtered record list. </returns>
        public static BubblingObservableCollection<RecordViewModel> Filter(
          this BubblingObservableCollection<RecordViewModel> collection, SubsystemTypes[] types
        )
        {
            return new BubblingObservableCollection<RecordViewModel>(
              collection
                .Where(r => types.Any(t => r.Subsystem == t))
                .OrderBy(r => (int)r.Field.ToEnum<Sensors>())
                .ToList()
            );
        }
    }
}
