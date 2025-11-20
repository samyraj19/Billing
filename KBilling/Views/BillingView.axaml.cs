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
using KBilling.Controls;

namespace KBilling;
public partial class BillingView : UserControl {
   public BillingView () {
      InitializeComponent ();
      Loaded += OnLoaded;
      Unloaded += OnUnloaded;
   }

   #region Events
   void OnLoaded (object? sender, RoutedEventArgs e) {
      KeyDown += OnKeyDown;

      BtnDiscount.Click += OnDiscountClick;
      BtnClearAll.Click += BtnClearAllClick;
      BtnPayBill.Click += OnPayClick;

      txtSearch.TextChanging += OnSearchTextChanging;
      TextboxPhone.AddHandler (InputElement.KeyUpEvent, OnTxtPhoneKeyUp, RoutingStrategies.Tunnel);
      TextboxPhone.AddHandler (InputElement.TextInputEvent, NumHelper.OnIntOnly, RoutingStrategies.Tunnel);

      //check box
      chkReceived.IsCheckedChanged += OnReceivedChanged;

      SetUpToggleEvent ();

      // focus on search box
      txtSearch.Focus ();
   }

   void OnUnloaded (object? sender, RoutedEventArgs e) {
      // detach handlers to avoid leaks and double subscriptions
      if (BtnDiscount is not null) BtnDiscount.Click -= OnDiscountClick;
      if (BtnClearAll is not null) BtnClearAll.Click -= BtnClearAllClick;

      if (txtSearch is not null) txtSearch.TextChanging -= OnSearchTextChanging;
      if (TextboxPhone is not null) TextboxPhone.RemoveHandler (InputElement.TextInputEvent, NumHelper.OnIntOnly);
      if (TextboxPhone is not null) TextboxPhone.RemoveHandler (InputElement.KeyUpEvent, OnTxtPhoneKeyUp);

      chkReceived.IsCheckedChanged -= OnReceivedChanged;
   }

   void OnKeyDown (object? sender, KeyEventArgs e) {
      if (e.Key == Key.Escape)
         mProductDialog?.Close ();
   }

   void SetUpToggleEvent () {
      foreach (var btn in PayPanel.Children.OfType<ToggleButton> ())
         btn.IsCheckedChanged += OnToggleChecked;
   }

   void OnToggleChecked (object? sender, RoutedEventArgs e) {
      if (sender is not ToggleButton clicked || VM() is not BillVM vm) return;

      // Uncheck all other toggles
      foreach (var other in PayPanel.Children.OfType<ToggleButton> ())
         if (!ReferenceEquals (other, clicked)) other.IsChecked = false;

      // Update payment mode
      mPaymode = ToggleCash.IsChecked == true ? EPaymentMode.Cash : EPaymentMode.Online;
      if (vm.BillHeader is null) return;
      vm.BillHeader.PaymentMethod = mPaymode.Get ();
   }

   async void OnPayClick (object? sender, RoutedEventArgs e) {
      if (sender is not Button btn || VM() is not BillVM vm) return;
      btn.IsEnabled = false;

      try {
         if (!vm.CanPay ()) return;
            var confirm = await MsgBox.ShowConfirmAsync ("Proceed to Payment", $"Are you sure you want to proceed to payment:  {mPaymode.Get ()}");
            if (confirm == ButtonResult.Yes) {
               ResertBill (vm);
               await MsgBox.ShowInfoAsync ("Success", "Payment processed successfully.");
            } else await MsgBox.ShowInfoAsync ("Cancelled", "Payment was cancelled.");
      
      } catch {
         await MsgBox.ShowInfoAsync ("Cancelled", "Payment was cancelled.");
      } finally {
         if (sender is Button btn1) btn1.IsEnabled = true;
      }
   }

   void OnReceivedChanged (object? sender, RoutedEventArgs e) {
      // Always get the latest value safely
      Dispatcher.UIThread.Post (() => {
         bool isChecked = chkReceived.IsChecked == true;
         txtRecvAmt.IsEnabled = !isChecked;
         if (isChecked) VM ().UpdateBill ();
      });
   }

   void OnTxtPhoneKeyUp (object? sender, KeyEventArgs e) {
      if (TextboxPhone?.Text is { Length: > 10 } text)
         TextboxPhone.Text = text[..10];// take first 10 digits only
   }

   void OnSearchTextChanging (object? sender, TextChangingEventArgs e) {
      if (mIgnoretxtchange || sender is not TextBox text) return;

      mProductDialog ??= mManager.GetWindow ("ProductLookupDialog") as ProductLookupDialog;
      if (mProductDialog is null) return;

      mProductDialog.UpdateRefresh (text?.Text?.Trim ());
      mProductDialog.ClosedEvent += (product) => Add (product);

      // Position dialog below search box
      if (text is null) return;
      mProductDialog.Position = text.PointToScreen (new Point (0, text.Bounds.Height));
      e.Handled = true;

      if (!mProductDialog.mIsShow) mProductDialog.Show (MainWindow.Instance);

      txtSearch.Focus ();
   }

   void BtnClearAllClick (object? sender, RoutedEventArgs e) => ResertBill (VM());

   void OnDiscountClick (object? sender, RoutedEventArgs e) {
      if (VM ()?.BillItems?.Count is <= 0) return;
      if (mManager.ShowDialog (MainWindow.Instance, "DiscountDialog") is DiscountDialog dialog)
         dialog.DiscountApplied += (discount) => VM ().UpdateBill (discount);
   }

   void OnGridQtyTxtChaning (object? sender, TextChangedEventArgs e) {
      if (sender is not TextBox textBox || textBox.DataContext is not BillDetails item) return;

      lblError.Content = string.Empty;
      string input = textBox.Text ?? string.Empty;
      // Ensure only integer input
      if (input.Length > 0 && !Is.Integer (input)) {
         textBox.Text = input[..^1];
         textBox.CaretIndex = textBox.Text.Length; // Keep cursor at end
         return;
      }

      if (int.TryParse (textBox.Text, out var qty) &&
          int.TryParse (textBox.Tag?.ToString (), out var availableQty)) {
         if (qty > availableQty) {
            MsgBox.ShowErrorAsync ("Info", "Quantity exceeds available stock.");
            item.Quantity = 0;
         }
      }

      VM ()?.UpdateBill ();
   }

   async void IconButton_Click (object? sender, RoutedEventArgs e) {
      if (sender is not IconButton btn) return;
      if (btn.DataContext is not BillDetails detail) return;
      var confirm = await MsgBox.ShowConfirmAsync ("Delete Item", "Are you sure?");
      if (confirm == ButtonResult.Yes) VM ()?.BillItems?.Remove (detail);

   }
   #endregion

   #region Medthods

   /// <summary>Adds the selected product to the bill.</summary>
   void Add (Product product) {
      mProductDialog = null;
      if (VM ().AddItem (product)) {
         txtSearch.Focus ();
         mIgnoretxtchange = true;
         txtSearch.Text = string.Empty;
         mIgnoretxtchange = false;
      }
   }

   /// <summary>Resets the bill to its initial state.</summary>
   void ResertBill (BillVM vm) {
      if (vm is null || vm.BillHeader is null) return;
      vm.Reset ();
      vm.BillHeader.PaymentMethod = EPaymentMode.None.Get ();
      PayPanel.Children.OfType<ToggleButton> ().ToList ().ForEach (btn => btn.IsChecked = false);
      chkReceived.IsChecked = true;
   }

   BillVM VM () => DataContext as BillVM ?? throw new InvalidOperationException ("DataContext is not of type BillVM");

   #endregion

   #region Fields
   readonly WindowManager mManager = new ();
   EPaymentMode mPaymode;
   ProductLookupDialog? mProductDialog;
   bool mIgnoretxtchange;
   #endregion

}