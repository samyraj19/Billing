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
   }

   void OnLoaded (object? sender, RoutedEventArgs e) {
      BtnDiscount.Click += OnDiscountClick;
      DataGridProduct.SelectionChanged += DataGridSelectionChanged;
      BtnClearAll.Click += BtnClearAllClick;
      txtSearch.KeyDown += OnSearchTextKeyDown;
      txtSearch.Focus ();
   }

   async void OnSearchTextKeyDown (object? sender, KeyEventArgs e) {
      var dialog = mManager.GetWindow ("ProductLookupDialog") as ProductLookupDialog;
      var pos = txtSearch.PointToScreen (new Point (0, txtSearch.Bounds.Height));
      if (dialog is not null) {
         dialog.Position = pos;
         await dialog.ShowDialog (MainWindow.Instance);
      }
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
      TextBox? textBox = (sender as TextBox);
      if (textBox is null || VM is null) return;
      string? input = textBox?.Text ?? string.Empty;
      // Allow empty text (for deletion), otherwise check digits
      if (input.Trim ().Length > 0 && !Is.Integer (input)) {
         int index = input.Length - 1;
         textBox.Text = input.Remove (index, 1);
      }

      if (Is.NotEmpty (textBox?.Text)) {
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

   #region Fields
   readonly WindowManager mManager = new ();
   BillVM? VM => DataContext as BillVM;
   BillDetails? mSelectedItem;
   #endregion

}