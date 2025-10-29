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
using Avalonia.Controls.Primitives;
using Avalonia.Threading;

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
      BtnClearAll.Click += BtnClearAllClick;
      BtnPayBill.Click += OnPayClick;
      DataGridProduct.SelectionChanged += DataGridSelectionChanged;
      txtSearch.KeyDown += OnSearchTextKeyDown;

      TextboxPhone.AddHandler (InputElement.KeyUpEvent, OnTxtPhoneKeyUp, RoutingStrategies.Tunnel);
      TextboxPhone.AddHandler (InputElement.TextInputEvent, OnTextBoxTextInput, RoutingStrategies.Tunnel);

      //check box
      chkReceived.IsCheckedChanged += OnReceivedAmtChanged;

      SetUpToggleEvent ();
      // focus on search box
      txtSearch.Focus ();
   }

   void OnUnloaded (object? sender, RoutedEventArgs e) {
      // detach handlers to avoid leaks and double subscriptions
      if (BtnDiscount is not null) BtnDiscount.Click -= OnDiscountClick;
      if (DataGridProduct is not null) DataGridProduct.SelectionChanged -= DataGridSelectionChanged;
      if (BtnClearAll is not null) BtnClearAll.Click -= BtnClearAllClick;
      if (txtSearch is not null) txtSearch.KeyDown -= OnSearchTextKeyDown;

      if (TextboxPhone is not null) TextboxPhone.RemoveHandler (InputElement.TextInputEvent, OnTextBoxTextInput);
      if (TextboxPhone is not null) TextboxPhone.RemoveHandler (InputElement.KeyUpEvent, OnTxtPhoneKeyUp);
   }

   void SetUpToggleEvent () {
      var toggles = PayPanel.Children.OfType<ToggleButton> ().ToList ();
      foreach (var btn in toggles) btn.IsCheckedChanged += OnToggleChecked;
   }

   private void OnToggleChecked (object? sender, RoutedEventArgs e) {
      if (sender is not ToggleButton clicked) return;

      // Uncheck all other toggles
      foreach (var other in PayPanel.Children.OfType<ToggleButton> ())
         if (!ReferenceEquals (other, clicked)) other.IsChecked = false;

      // Update payment mode
      mPaymode = ToggleCash.IsChecked == true ? EPaymentMode.Cash : EPaymentMode.Online;
      VM ().BillHeader.PaymentMethod = mPaymode.Get ();
   }

   async void OnPayClick (object? sender, RoutedEventArgs e) {
      if (!VM ().CanPay ()) return;
      var confirm = await MsgBox.ShowConfirmAsync ("Proceed to Payment", "Are you sure you want to proceed to payment?" + mPaymode.Get ());
      if (confirm == ButtonResult.Yes) {
         await MsgBox.ShowInfoAsync ("Success", "Payment processed successfully.");
         VM ().ResetBill ();
         ClearPaydetails ();
      } else await MsgBox.ShowInfoAsync ("Cancelled", "Payment was cancelled.");
   }

   void OnReceivedAmtChanged (object? sender, RoutedEventArgs e) {
      // Always get the latest value safely
      Dispatcher.UIThread.Post (() => {
         bool isChecked = chkReceived.IsChecked == true;
         txtRecvAmt.IsEnabled = !isChecked;

         if (isChecked) VM ().BillHeader.ReceivedAmount = VM ().UpdateTotal ();
      });
   }

   void OnTxtPhoneKeyUp (object? sender, KeyEventArgs e) {
      if (TextboxPhone?.Text is { Length: > 10 } text) TextboxPhone.Text = text[..10]; // take first 10 digits only
   }

   void OnTextBoxTextInput (object? sender, TextInputEventArgs e) => e.Handled = string.IsNullOrEmpty (e.Text) || !e.Text.All (char.IsDigit);

   void OnSearchTextKeyDown (object? sender, KeyEventArgs e) {
      if (mManager.GetWindow ("ProductLookupDialog") is not ProductLookupDialog dialog) return;

      // position below the search textbox
      var pos = txtSearch.PointToScreen (new Point (0, txtSearch.Bounds.Height));
      dialog.Position = pos;

      e.Handled = true;
      dialog.ShowDialog (MainWindow.Instance);
      dialog.ProductSelected += (product) => Add (product);
   }

   void BtnClearAllClick (object? sender, RoutedEventArgs e) {
      VM ().ResetBill ();
      VM ().BillItems.Clear ();
   }

   void DataGridSelectionChanged (object? sender, SelectionChangedEventArgs e) => mSelectedItem = DataGridProduct.SelectedItem as BillDetails;

   void OnDiscountClick (object? sender, RoutedEventArgs e) {
      var dialog = mManager.ShowDialog (MainWindow.Instance, "DiscountDialog") as DiscountDialog;
      if (dialog is null) return;
      dialog.DiscountApplied += (discount) => UpdateBill (discount);
   }

   void OnGridQtyTxtChaning (object? sender, TextChangedEventArgs e) {
      if (sender is not TextBox textBox) return;

      string input = textBox.Text ?? string.Empty;
      if (input.Trim ().Length > 0 && !Is.Integer (input)) textBox.Text = input[..^1]; // remove last invalid char

      if (Is.NotEmpty (textBox.Text)) UpdateBill ();
   }

   async void IconButton_Click (object? sender, RoutedEventArgs e) {
      var confirm = await MsgBox.ShowConfirmAsync ("Delete Item", "Are you sure?");
      if (confirm == ButtonResult.Yes) {
         if (mSelectedItem is not null) VM ().BillItems.Remove (mSelectedItem);
         else await MsgBox.ShowWarningAsync ("Select Item", "Please select an item.");
      }
   }
   #endregion

   #region Medthods
   void Add (Product product) {
      VM ().AddItem (product);
      txtSearch.Text = string.Empty;
      txtSearch.Focus ();
   }

   void UpdateBill (decimal? discount = null) {
      var vm = VM ();

      vm.BillHeader.SubTotal = vm.SubTotal ();
      vm.BillHeader.Discount = discount ?? vm.BillHeader.Discount;

      var total = discount.HasValue ? vm.Total (discount.Value) : vm.UpdateTotal ();
      vm.BillHeader.Total = vm.BillHeader.ReceivedAmount = total;
   }

   void ClearPaydetails () {
      VM ().BillHeader.PaymentMethod = EPaymentMode.None.Get ();
      PayPanel.Children.OfType<ToggleButton> ().ToList ().ForEach (btn => btn.IsChecked = false);
      chkReceived.IsChecked = true;
   }

   BillVM VM () => DataContext as BillVM ?? throw new InvalidOperationException ("DataContext is not of type BillVM");

   #endregion

   #region Fields
   readonly WindowManager mManager = new ();
   BillDetails? mSelectedItem;
   EPaymentMode mPaymode;
   #endregion

}