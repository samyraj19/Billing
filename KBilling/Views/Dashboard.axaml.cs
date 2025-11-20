using System;
using System.ComponentModel;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using KBilling.Model;
using KBilling.Services;
using KBilling.ViewManagement;
using KBilling.ViewModel;

namespace KBilling;
public partial class Dashboard : UserControl {
   public Dashboard () {
      InitializeComponent ();
      Loaded += OnLoad;
      Unloaded += OnUnloaded;
   }

   #region Events
   void OnLoad (object? sender, RoutedEventArgs e) {
      RegToggleEvents ();
      DefaultSelection ();
      VM?.LoadData ();
      if (VM is not null) VM.PropertyChanged += OnPropertyChanged;
   }

   void OnUnloaded (object? sender, RoutedEventArgs e) => UnRegToggleEvents ();

   protected override void OnDataContextChanged (EventArgs e) {
      base.OnDataContextChanged (e);
      VM = TryGetVM ();
   }

   void OnPropertyChanged (object? sender, PropertyChangedEventArgs e) => VM = TryGetVM ();

   void OnToggleClicked (object? sender, RoutedEventArgs e) {
      if (sender is not ToggleButton btn || btn.Tag is not EReportType type) return;

      if (VM is not null) VM.RType = type;
      VM?.LoadData ();

      foreach (var t in TogglePanel.Children.OfType<ToggleButton> ())
         if (t != btn) t.IsChecked = false;
   }

   #endregion

   #region Methods
   void RegToggleEvents () {
      foreach (var toggle in TogglePanel.Children.OfType<ToggleButton> ())
         toggle.Click += OnToggleClicked;
   }

   void UnRegToggleEvents () {
      foreach (var toggle in TogglePanel.Children.OfType<ToggleButton> ())
         toggle.Click -= OnToggleClicked;
   }

   void DefaultSelection () {
      if (TogglePanel.Children.Count > 0 && TogglePanel.Children[0] is ToggleButton btn)
         btn.IsChecked = true;
   }

   DashboardVM? TryGetVM () => DataContext is DashboardVM vm ? vm : throw new InvalidOperationException ("DataContext is not of type DashboardVM");

   #endregion

   #region Fields
   DashboardVM? VM { get; set; }
   #endregion
}