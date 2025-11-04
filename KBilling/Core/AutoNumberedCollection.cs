using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using KBilling.Helper;
using KBilling.Model;

namespace KBilling.Core {
   public class AutoNumberedCollection<T> : ObservableCollection<T> { //BillCollection
      protected override void OnCollectionChanged (NotifyCollectionChangedEventArgs e) {
         base.OnCollectionChanged (e);
         if (e.Action is NotifyCollectionChangedAction.Add or NotifyCollectionChangedAction.Remove or NotifyCollectionChangedAction.Reset) {
            CollectionHelper.ReArrange (this.Cast<object> ());
         }
      }
   }  
}
