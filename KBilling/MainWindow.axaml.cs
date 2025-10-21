using Avalonia.Controls;
using KBilling.ViewManagement;

namespace KBilling {
   public partial class MainWindow : Window {
      public MainWindow () {
         InitializeComponent ();

         // Default view
         var view = new ViewManager (ContentPanel);
         view.ShowView ("Dashboard");
      }
   }
}