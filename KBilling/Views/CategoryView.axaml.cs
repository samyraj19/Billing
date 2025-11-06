using System;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using KBilling.Helper;
using KBilling.ViewManagement;

namespace KBilling;
public partial class CategoryView : UserControl {
   public CategoryView () {
      InitializeComponent ();
      Loaded += OnLoaded;
      Unloaded += OnUnload;
   }

   void OnLoaded (object? sender, RoutedEventArgs e) {
      // LoadDataFromFile ();
      TextName.KeyDown += OnSearchKeyDown;
   }

   private void OnSearchKeyDown (object? sender, Avalonia.Input.KeyEventArgs e) {
      if (mManager.GetWindow ("ItemListDialog") is not ItemListDialog dialog) return;

      // position below the search textbox
      var pos = TextName.PointToScreen (new Point (0, TextName.Bounds.Height));
      dialog.Position = pos;

      e.Handled = true;
      dialog.ShowDialog (MainWindow.Instance);
   }

   void OnUnload (object? sender, Avalonia.Interactivity.RoutedEventArgs e) {
     // throw new System.NotImplementedException ();
   }

   void LoadDataFromFile () {
      string path = @"C:\Users\Samyraj\Downloads\FancyStoreItemList.txt";

      foreach (string line in File.ReadLines (path)) {
         string[] parts = line.Split ('|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
         if (parts.Length == 3) {
            var numName = parts[0].Split ('.', 2, StringSplitOptions.TrimEntries);
            var category = new Model.Category {
               Name = numName.Length > 1 ? numName[1] : "",
               Prefix = parts[1],
               Code = parts[2]
            };
            App.Repo.Category.Insert (category);
         }
      }
   }


   #region Fields
   WindowManager mManager = new();
   #endregion
}