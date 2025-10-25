using System.Linq;
using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;
using KBilling.ViewManagement;
using KBilling.ViewModel;
using KBilling.Helper;
using KBilling.Model;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using Avalonia.Input;

namespace KBilling;
public partial class BillingView : UserControl {
   public BillingView () {
      InitializeComponent ();
      Loaded += OnLoaded;
      Unloaded += OnUnloaded;
   }

   #region Events
   void OnLoaded (object? sender, RoutedEventArgs e) {
      BtnDiscount.Click += OnDiscountClick;
      DataGridProduct.SelectionChanged += DataGridSelectionChanged;
      BtnClearAll.Click += BtnClearAllClick;
      txtSearch.KeyDown += OnSearchTextKeyDown;
      txtSearch.Focus ();
      TextBoxQuantity.AddHandler (InputElement.TextInputEvent, OnQtyTxtInput, RoutingStrategies.Tunnel);
      TextboxPhone.AddHandler (InputElement.KeyUpEvent, OnTxtPhoneKeyup, RoutingStrategies.Tunnel);
      TextboxPhone.AddHandler (InputElement.TextInputEvent, OnQtyTxtInput, RoutingStrategies.Tunnel);
   }

   void OnTxtPhoneKeyup (object? sender, KeyEventArgs e) {
      var phoneno = TextboxPhone?.Text?.Trim ();
      if (!Is.IsTenDigit (phoneno)) {
         if(TextboxPhone is not null) TextboxPhone.Text = phoneno?.Remove (10,1);
      }
   }

   void OnUnloaded (object? sender, RoutedEventArgs e) {
      // detach handlers to avoid leaks and double subscriptions
      if (BtnDiscount is not null) BtnDiscount.Click -= OnDiscountClick;
      if (DataGridProduct is not null) DataGridProduct.SelectionChanged -= DataGridSelectionChanged;
      if (BtnClearAll is not null) BtnClearAll.Click -= BtnClearAllClick;
      if (txtSearch is not null) txtSearch.KeyDown -= OnSearchTextKeyDown;
      if (TextBoxQuantity is not null) TextBoxQuantity.RemoveHandler (InputElement.TextInputEvent, OnQtyTxtInput);
      if (TextboxPhone is not null) TextboxPhone.RemoveHandler (InputElement.TextInputEvent, OnQtyTxtInput);
      if (TextboxPhone is not null) TextboxPhone.RemoveHandler (InputElement.KeyUpEvent, OnTxtPhoneKeyup);
   }

   void OnQtyTxtInput (object? sender, TextInputEventArgs e) => e.Handled = string.IsNullOrEmpty (e.Text) || !e.Text.All (char.IsDigit);

   async void OnSearchTextKeyDown (object? sender, KeyEventArgs e) {
      // Only open lookup on Enter (avoid opening on every key press)
      if (e.Key != Key.Enter) return;

      var dialog = mManager.GetWindow ("ProductLookupDialog") as ProductLookupDialog;
      if (dialog is null || txtSearch is null) return;

      // position below the search textbox
      var pos = txtSearch.PointToScreen (new Point (0, txtSearch.Bounds.Height));
      dialog.Position = pos;

      e.Handled = true;
      await dialog.ShowDialog (MainWindow.Instance);
   }

   void BtnClearAllClick (object? sender, RoutedEventArgs e) => VM?.BillItems.Clear ();

   void DataGridSelectionChanged (object? sender, SelectionChangedEventArgs e) => mSelectedItem = DataGridProduct.SelectedItem as BillDetails;

   void OnDiscountClick (object? sender, RoutedEventArgs e) {
      var dialog = mManager.ShowDialog (MainWindow.Instance, "DiscountDialog") as DiscountDialog;
      if (dialog is null) return;
      dialog.DiscountApplied += (discount) => {
         if (VM != null) {
            VM.BillHeader.Discount = discount;
            VM.BillHeader.Total = VM.Total (discount);
         }
      };
   }

   void TextBox_TextChanged (object? sender, Avalonia.Controls.TextChangedEventArgs e) {
      if (sender is not TextBox textBox || VM is null) return;

      string input = textBox.Text ?? string.Empty;
      if (input.Trim ().Length > 0 && !Is.Integer (input)) {
         int index = input.Length - 1;
         if (index >= 0) textBox.Text = input.Remove (index, 1);
      }

      if (Is.NotEmpty (textBox.Text)) {
         VM.BillHeader.SubTotal = VM.SubTotal ();
         VM.BillHeader.Total = VM.UpdateTotal ();
      }
   }

   async void IconButton_Click (object? sender, RoutedEventArgs e) {
      var box = MessageBoxManager.GetMessageBoxStandard ("Delete Item", "Are you sure you want to delete this item?", ButtonEnum.YesNo, Icon.Question);
      var res = await box.ShowAsync ();
      if (res == ButtonResult.Yes) {
         if (mSelectedItem is not null) VM?.BillItems.Remove (mSelectedItem);
         else await MessageBoxManager.GetMessageBoxStandard ("Select Item", "Please select an item to delete.",
                                      ButtonEnum.Ok, Icon.Warning).ShowAsync ();
      }
   }
   #endregion

   #region Fields
   readonly WindowManager mManager = new ();
   BillVM? VM => DataContext as BillVM;
   BillDetails? mSelectedItem;
   #endregion

}