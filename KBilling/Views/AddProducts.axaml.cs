using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using KBilling.Helper;
using KBilling.ViewModel;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia;
using KBilling.Model;
using KBilling.ViewManagement;
using KBilling.Controls;

namespace KBilling;
public partial class AddProducts : UserControl {
   public AddProducts () {
      InitializeComponent ();
      Loaded += OnLoaded;
      Unloaded += OnUnloaded;
   }

   void OnLoaded (object? sender, RoutedEventArgs e) {
      // Attach events
      KeyDown += OnKeyDown;
      EnterFocusHelper.Attach (MainPanel);

      txtName.KeyDown += OnNameKeyDown;
      txtQty.AddHandler (InputElement.TextInputEvent, NumHelper.OnIntOnly, RoutingStrategies.Tunnel);
      txtPurRate.AddHandler (InputElement.TextInputEvent, NumHelper.OnDecimalOnly, RoutingStrategies.Tunnel);
      txtSellRate.AddHandler (InputElement.TextInputEvent, NumHelper.OnDecimalOnly, RoutingStrategies.Tunnel);

      if (vm is not null) vm.OnUiRequest += s => EnableControls (s);

      vm?.LoadData ();
   }

   void OnUnloaded (object? sender, RoutedEventArgs e) {
      // Detach events
      KeyDown -= OnKeyDown;

      txtName.KeyDown -= OnNameKeyDown;
      txtQty.RemoveHandler (InputElement.TextInputEvent, NumHelper.OnIntOnly);
      txtPurRate.RemoveHandler (InputElement.TextInputEvent, NumHelper.OnDecimalOnly);
      txtSellRate.RemoveHandler (InputElement.TextInputEvent, NumHelper.OnDecimalOnly);
   }

   #region Event Handlers

   protected override void OnDataContextChanged (EventArgs e) {
      base.OnDataContextChanged (e);
      vm = DataContext as ProductVM;
   }

   void OnNameKeyDown (object? sender, Avalonia.Input.KeyEventArgs e) {
      if (mManager.GetWindow ("ItemListDialog") is not ItemListDialog dialog) return;

      // position below the search textbox
      var pos = txtName.PointToScreen (new Point (0, txtName.Bounds.Height));
      dialog.Position = pos;
      if (dialog.DataContext is not CategoryVM category) return;

      e.Handled = true;
      dialog.ShowDialog (MainWindow.Instance);
      dialog.ItemApplied += (category) => vm?.ApplyItem (category);
   }

   void OnKeyDown (object? sender, KeyEventArgs e) {
      if (e.Key == Key.Enter) {
         vm?.Submit ();
         e.Handled = true;
      }
   }

   void OnIconButtonClick (object? sender, RoutedEventArgs e) {
      if (sender is not IconButton btn || btn.DataContext is not Product product) return;
      if (btn.Tag?.ToString () is not string action) return;
      if (vm is not null) vm.Action = action.GetEAction ();
      if (action.IsDelete () && vm is not null) vm.DeleteAsync (product);
      else if (action.IsEdit ()) {
         EnableControls (false);
         vm?.Edit (product);
      }
   }
   #endregion

   #region Methods

   void EnableControls (bool enable) {
      textCode.IsEnabled = enable;
      txtName.IsEnabled = enable;
   }

   #endregion

   #region Fields
   ProductVM? vm;
   WindowManager mManager = new ();
   #endregion
}