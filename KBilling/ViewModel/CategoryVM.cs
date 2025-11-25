using System;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KBilling.Core;
using KBilling.Extension;
using KBilling.Helper;
using KBilling.Model;
using MsBox.Avalonia.Enums;

namespace KBilling.ViewModel {
   public partial class CategoryVM : Category {
      public CategoryVM () { }

      bool Insert (Category category) => Repo.Category.Insert (category);

      bool Update (Category category) => Repo.Category.Update (category);

      public bool Delete (int categoryId) {
         if (Repo?.Category == null) return false;
         bool isDeleted = Repo.Category.Delete (categoryId);
         if (!isDeleted) return false;
         else MsgBox.ShowSuccessAsync ("Success", "Category deleted successfully.");
         GetAll ();
         return true;
      }

      public void Submit () {
         var errors = GetValidationErrors ();
         if (errors.Count > 0) {
            MsgBox.ShowErrorAsync ("Error", "• " + string.Join ("\n• ", errors));
            return;
         }

         bool isEdit = CatAction.IsEdit ();
         if (isEdit) Update (this); else Insert (this);

         MsgBox.ShowSuccessAsync ("Success", isEdit ? "Category updated successfully." : "Category added successfully.");

         Clear ();
         GetAll ();
      }

      public void Edit (Category cat) {
         CategoryId = cat.CategoryId;
         Name = cat.Name;
         Prefix = cat.Prefix;
         Code = cat.Code;
         Description = cat.Description;
      }

      public void GetAll () {
         var categories = Repo.Category.GetAll ();
         AllCategories?.Clear ();
         AllCategories?.AddRange (categories);
         Filter (string.Empty);
      }

      List<string> GetValidationErrors () {
         var errors = new List<string> ();

         if (Is.IsEmpty (Name)) errors.Add ("Category Name is required.");
         if (Is.IsEmpty (Prefix)) errors.Add ("Category Prefix is required.");
         if (Is.IsEmpty (Code)) errors.Add ("Category Code is required.");
         if(CodeExistsCheck(CategoryId)) errors.Add ("Category Code already exists.");

         return errors;
      }
      public void Filter (string text) {
         if (AllCategories is null || FilterCategories is null) return;
         string filterValue = text?.Trim () ?? string.Empty;

         FilterCategories.Filter (AllCategories, c => string.IsNullOrEmpty (filterValue) ||
                         (c.Name?.Contains (filterValue, StringComparison.OrdinalIgnoreCase) ?? false) ||
                         c.Code?.ToString ().Contains (filterValue, StringComparison.OrdinalIgnoreCase) == true);

         TotalItems = $" Total Item: {AllCategories.Count}";
      }

      public void SoryBy (string text) {
         if (AllCategories is null || FilterCategories is null) return;
         bool showAll = !Is.IsNotEmpty (text) || text.Equals ("All", StringComparison.OrdinalIgnoreCase);
         FilterCategories.Filter (AllCategories, showAll ? _ => true : c => c.Name?.StartsWith (text, StringComparison.OrdinalIgnoreCase) == true);
      }

      [RelayCommand]
      public void Onsubmit () {
         Submit ();
         Clear ();
         CatAction = EAction.None;
         SearchText = string.Empty;
      }

      [RelayCommand]
      public void Oncancel () {
         Clear ();
         CatAction = EAction.None;
         SearchText = string.Empty;
      }

      public async void OnDelete (int id) {
         var box = await AppMsg.AskDelItem ();
         if (box == ButtonResult.Yes) Delete (id);
         SearchText = string.Empty;
      }

      bool CodeExistsCheck (int id) => AllCategories.Exist (c => 
         c.CategoryId != id &&c.Code?.Equals (Code, StringComparison.OrdinalIgnoreCase) == true &&
         c.Prefix?.Equals (Prefix, StringComparison.OrdinalIgnoreCase) == true);

      #region Fields
      AutoNumberedCollection<Category>? AllCategories { get; } = new AutoNumberedCollection<Category> ();
      public AutoNumberedCollection<Category>? FilterCategories { get; set; } = new AutoNumberedCollection<Category> ();

      public EAction CatAction { get; set; } = EAction.None;

      #endregion

      #region ----- Observable properties & Events -----
      [ObservableProperty] string searchText = string.Empty;
      [ObservableProperty] string? totalItems;
      [ObservableProperty] string? cmbSelectedItem;

      partial void OnSearchTextChanged (string value) => Filter (value);

      partial void OnCmbSelectedItemChanged (string? value) => SoryBy (value ?? string.Empty);
      #endregion
   }
}
