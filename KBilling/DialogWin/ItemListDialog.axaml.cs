using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using KBilling.Model;
using KBilling.ViewModel;

namespace KBilling;
public partial class ItemListDialog : Window {
   public ItemListDialog () {
      InitializeComponent ();
      Loaded += OnLoaded;
      Unloaded += OnUnloaded;
   }

   void OnUnloaded (object? sender, RoutedEventArgs e) => SearchTextbox.TextChanging -= OnTextChanging;

   void OnLoaded (object? sender, RoutedEventArgs e) {
      SearchTextbox.Focus ();
      SearchTextbox.TextChanging += OnTextChanging;
      KeyDown += OnKeyDown;
      DoubleTapped += OnDoubleTapped;
      ListCategory.SelectionChanged += OnSelectionChanged;
      VM ().GetAll ();
   }

   void OnSelectionChanged (object? sender, SelectionChangedEventArgs e) => mCat = ListCategory.SelectedItem as Category ?? new Category ();

   void OnDoubleTapped (object? sender, TappedEventArgs e) {
      ItemApplied.Invoke (mCat);
      Close ();
   }

   void OnKeyDown (object? sender, KeyEventArgs e) {
      if (e.Key == Key.Escape) Close ();
   }

   void OnTextChanging (object? sender, TextChangingEventArgs e)
      => VM ().Filter ((sender as TextBox)?.Text?.Trim () ?? string.Empty);

   CategoryVM VM () => DataContext is CategoryVM vm ? vm : throw new InvalidOperationException ("DataContext is not of type CategoryVM");

   #region Fields
   public event Action<Category> ItemApplied;
   Category mCat;
   #endregion
}