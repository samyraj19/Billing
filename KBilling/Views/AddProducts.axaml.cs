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
      SearchTxt.TextChanging += OnSearchTextChanging;
      BtnSubmit.Click += OnSubmitClicked;
      BtnCancel.Click += OnCancelClicked;
      EnterFocusHelper.Attach (MainPanel);

      txtName.KeyDown += OnNameKeyDown;
      txtQty.AddHandler (InputElement.TextInputEvent, NumHelper.OnIntOnly, RoutingStrategies.Tunnel);
      txtPurRate.AddHandler (InputElement.TextInputEvent, NumHelper.OnDecimalOnly, RoutingStrategies.Tunnel);
      txtSellRate.AddHandler (InputElement.TextInputEvent, NumHelper.OnDecimalOnly, RoutingStrategies.Tunnel);

      VM ().LoadData ();
   }

   void OnUnloaded (object? sender, RoutedEventArgs e) {
      // Detach events
      KeyDown -= OnKeyDown;
      SearchTxt.TextChanging -= OnSearchTextChanging;
      BtnSubmit.Click -= OnSubmitClicked;
      BtnCancel.Click -= OnCancelClicked;

      txtName.KeyDown -= OnNameKeyDown;
      txtQty.RemoveHandler (InputElement.TextInputEvent, NumHelper.OnIntOnly);
      txtPurRate.RemoveHandler (InputElement.TextInputEvent, NumHelper.OnDecimalOnly);
      txtSellRate.RemoveHandler (InputElement.TextInputEvent, NumHelper.OnDecimalOnly);
   }

   #region Event Handlers
   void OnSearchTextChanging (object? sender, TextChangingEventArgs e) => VM ().UpdateFilter ((sender as TextBox)?.Text?.Trim () ?? string.Empty);

   void OnSubmitClicked (object? sender, RoutedEventArgs e) => Submit ();

   void OnCancelClicked (object? sender, RoutedEventArgs e) => Clear ();

   void OnNameKeyDown (object? sender, Avalonia.Input.KeyEventArgs e) {
      if (mManager.GetWindow ("ItemListDialog") is not ItemListDialog dialog) return;

      // position below the search textbox
      var pos = txtName.PointToScreen (new Point (0, txtName.Bounds.Height));
      dialog.Position = pos;
      if (dialog.DataContext is CategoryVM vm) mCategory = vm;

      e.Handled = true;
      dialog.ShowDialog (MainWindow.Instance);
      dialog.ItemApplied += (cat) => ApplyItem (cat);
   }

   void OnKeyDown (object? sender, KeyEventArgs e) {
      if (e.Key == Key.Enter) {
         Submit ();
         e.Handled = true;
      }
   }

   void OnIconButtonClick (object? sender, RoutedEventArgs e) {
      if (sender is not IconButton btn) return;
      if (btn.DataContext is not Product product) return;
      if (btn.Tag?.ToString () is not string action) return;
      mAction = action.GetEAction ();
         if (action.IsDelete ()) DeleteAsync (product);
         else if (action.IsEdit ()) {
            textCode.IsEnabled = false;
            txtName.IsEnabled = false;
            VM ().Edit (product);
         }
   }
   #endregion

   #region Methods

   void Submit () {
      if (VM ().CanSubmit ()) {
         string? code = mAction.IsEdit () ? VM()?.ProductNumber : string.Empty;
         VM ().AddOrUpdate (VM (), code);
         Clear ();
         VM ().LoadData ();
      }
   }

   async void DeleteAsync (Product item) {
      var box = await AppMsg.AskDelItem ();
      if (box == ButtonResult.Yes) VM ().DeleteItem (item);
   }

   void ApplyItem (Category cat) {
      VM ().ProductName = cat.Name;
      VM ().ProductNumber = cat.Code;
      textCode.IsEnabled = false;
      txtName.IsEnabled = false;
   }

   ProductVM VM () => DataContext is ProductVM vm ? vm : throw new InvalidOperationException ("DataContext is not of type ProductVM");

   void Clear () {
      VM ().Clear ();
      VM ().Refersh ();
      textCode.IsEnabled = true;
      txtName.IsEnabled = true;
      mAction = EAction.None;
   }

   #endregion

   #region Fields
   EAction mAction = EAction.None;
   WindowManager mManager = new ();
   Category mCategory = new ();
   #endregion
}