using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Avalonia.Xaml.Interactions.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KBilling.Core;
using KBilling.Extension;
using KBilling.Model;

namespace KBilling.ViewModel;
public partial class InvoiceVM : BaseModel {

   public InvoiceVM () { }

   public void GetAll () {
      var bills = App.Repo.Bills.GetAllBills (StartDate, EndDate);
      Invoices.SetCollection (bills);

      Filter (string.Empty);
   }

   public void GetDetails (long billId) {
      var details = App.Repo.Bills.GetAllBillDetailsById (billId);
      BillDetails.SetCollection (details);
   }

   [RelayCommand]
   public async void ExportPDF () {
      if (SelectedBill is null) return;
      var pdfbytes = App.Repo.InvoiceExport.PDF (this);

      var kfile = new KFileDialogs (MainWindow.Instance.StorageProvider);
      var kdlg = kfile.SaveAsync ("Save PDF File", $"{SelectedBill?.BillNumber ?? "Invoice"}.pdf", "*.pdf");

      if (kdlg is null) return;
      await using var stream = await (await kdlg).OpenWriteAsync ();
      using var writer = new System.IO.BinaryWriter (stream);
      writer.Write (pdfbytes);
   }

   public void Filter (string text) {
      if (Invoices is null || FilterInvoices is null) return;
      string filterValue = text?.Trim () ?? string.Empty;

      FilterInvoices.Filter (Invoices, inv => string.IsNullOrEmpty (filterValue) ||
                      (inv.BillNumber?.Contains (filterValue, StringComparison.OrdinalIgnoreCase) ?? false) ||
                      inv.CustomerName?.ToString ().Contains (filterValue, StringComparison.OrdinalIgnoreCase) == true);
   }

   public ObservableCollection<BillHeader> Invoices { get; set; } = [];
   public AutoNumberedCollection<BillDetails> BillDetails { get; set; } = [];

   #region -----Observable fields & events-----
   public ObservableCollection<BillHeader> FilterInvoices { get; set; } = [];
   [ObservableProperty] string? startDate;
   [ObservableProperty] string? endDate;
   [ObservableProperty] string searchText = string.Empty;
   [ObservableProperty] bool showPlaceholder = false;

   [ObservableProperty] BillHeader? selectedBill;

   partial void OnEndDateChanged (string? value) => GetAll ();

   partial void OnSearchTextChanged (string? oldValue, string newValue) => Filter (SearchText);

   partial void OnSelectedBillChanged (BillHeader? value) {
      GetDetails (value?.BillId ?? 0);
      SelectedBill = Invoices.Where (inv => inv.BillId == selectedBill?.BillId).SingleOrDefault ();
      ShowPlaceholder = SelectedBill != null;
   }
   #endregion
}

