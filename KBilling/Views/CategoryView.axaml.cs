using System;
using System.ComponentModel;
using System.IO;
using System.Xml.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using KBilling.Controls;
using KBilling.Helper;
using KBilling.Model;
using KBilling.ViewManagement;
using KBilling.ViewModel;
using MsBox.Avalonia.Enums;

namespace KBilling;
public partial class CategoryView : UserControl {
   public CategoryView () {
      InitializeComponent ();
      Loaded += OnLoaded;
      Unloaded += OnUnload;
   }

   void OnLoaded (object? sender, RoutedEventArgs e) {
      if (VM is not null) VM.PropertyChanged += OnPropertyChanged;
      SortByCmb.SelectionChanged += OnCmbChanged;

      VM?.GetAll ();
      BindAlpha ();
   }

   void OnUnload (object? sender, Avalonia.Interactivity.RoutedEventArgs e) {
      SortByCmb.SelectionChanged -= OnCmbChanged;
   }

   void OnPropertyChanged (object? sender, PropertyChangedEventArgs e) => VM = TryGetVM ();

   protected override void OnDataContextChanged (EventArgs e) {
      base.OnDataContextChanged (e);
      VM = TryGetVM ();
   }

   void OnCmbChanged (object? sender, SelectionChangedEventArgs e) {
      if (sender is not ComboBox cmb || cmb.SelectedItem is not string item) return;
      VM?.SoryBy (item);
   }

   async void OnActionClick (object? sender, RoutedEventArgs e) {
      if (sender is not IconButton btn) return;
      if (btn.DataContext is not Category cat) return;
      if (btn?.Tag?.ToString () is not string action) return;
      if (VM is not null) VM.CatAction = action.GetEAction ();
      if (action.IsEdit ()) VM?.Edit (cat);
      else if (action.IsDelete ()) VM?.OnDelete (cat.CategoryId);
   }

   void BindAlpha () => SortByCmb.ItemsSource = AlphaList.Get ();

   CategoryVM? TryGetVM () => DataContext is CategoryVM vm ? vm : throw new InvalidOperationException ("DataContext is not of type CategoryVM");

   #region Fields
   CategoryVM? VM;
   #endregion
}