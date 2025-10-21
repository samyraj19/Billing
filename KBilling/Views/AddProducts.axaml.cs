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

namespace KBilling;
public partial class AddProducts : UserControl {
   public AddProducts () {
      InitializeComponent ();
      Loaded += OnLoaded;
      Unloaded += OnUnLoaded;
      KeyDown += OnKeyDown;
      SearchTxt.TextChanging += SearchTextChanging;
      EnterFocusHelper.Attach (MainPanel);
      ProductNumberTextbox.AddHandler (InputElement.TextInputEvent, TextInputForInteger, RoutingStrategies.Tunnel);
      txtQty.AddHandler (InputElement.TextInputEvent, TextInputForInteger, RoutingStrategies.Tunnel);
      txtPurRate.AddHandler (InputElement.TextInputEvent, TextInputForDecimal, RoutingStrategies.Tunnel);
      txtSellRate.AddHandler (InputElement.TextInputEvent, TextInputForDecimal, RoutingStrategies.Tunnel);
   }

   void SearchTextChanging (object? sender, TextChangingEventArgs e)
      => VM?.UpdateFilter ((sender as TextBox)?.Text?.Trim () ?? string.Empty);

   void OnLoaded (object? sender, Avalonia.Interactivity.RoutedEventArgs e) {
      BtnSubmit.Click += (s, ev) => Submit();
      BtnCancel.Click += (s, ev) => VM?.Clear ();
   }

   void OnKeyDown (object? sender, KeyEventArgs e) {
      if (e.Key == Key.Enter) {
         Submit ();
         e.Handled = true;
      }
   }

   void OnUnLoaded (object? sender, Avalonia.Interactivity.RoutedEventArgs e) {
      throw new System.NotImplementedException ();
   }

   void TextInputForInteger (object? sender, TextInputEventArgs e)
      => e.Handled = !int.TryParse (e.Text, out _);

   void TextInputForDecimal (object? sender, TextInputEventArgs e) {  // Only digits or dot
      if (!string.IsNullOrEmpty (e.Text) && !e.Text.All (c => char.IsDigit (c) || c == '.')) e.Handled = true;
      // Prevent more than one dot
      if (sender is TextBox tb && e.Text.Contains ('.') && tb.Text.Contains ('.')) e.Handled = true;
   }

   async void IconButton_Click (object? sender, RoutedEventArgs e) {
      if ((sender as Button)?.Tag?.ToString () is string action) {
         mAction = action.GetEAction ();
         if (action.IsDelete ()) DeleteAsync ();
         else if (action.IsEdit () && mSelectedItem.IsProductClass()) VM?.Edit (mSelectedItem);
      }
   }

   void DataGrid_SelectionChanged (object? sender, SelectionChangedEventArgs e)
      => mSelectedItem = DataGridProduct.SelectedItem as Product;

   #region Methods
   void Submit () {
      if (VM.CanSubmit ()) {
         VM.AddOrUpdateProductInList (VM, mAction);
         VM?.Clear ();
         mAction = EAction.None;
      }
   }

   async void DeleteAsync () {

      if (!mSelectedItem.IsProductClass()) {
         await MessageBoxManager.GetMessageBoxStandard ("Select Item", "Please select an item.", ButtonEnum.Ok, Icon.Success).ShowAsync ();
         return;
      }
      var box = MessageBoxManager.GetMessageBoxStandard ("Delete Item", "Are you sure you want to delete this item?", ButtonEnum.YesNo, Icon.Success);
      var result = await box.ShowAsync ();
      if (result == ButtonResult.Yes) VM?.DeleteItem (mSelectedItem!);
   }

   #endregion

   #region Fields
   ProductVM? VM => DataContext as ProductVM;
   Product? mSelectedItem = null;
   EAction mAction = EAction.None;
   #endregion
}