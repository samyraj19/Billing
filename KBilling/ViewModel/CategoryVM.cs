using System;
using System.Collections.Generic;
using KBilling.Core;
using KBilling.Extension;
using KBilling.Helper;
using KBilling.Model;

namespace KBilling.ViewModel {
   public class CategoryVM : Category {
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

      public void Submit (EAction action, Category category) {
         var errors = GetValidationErrors ();
         if (errors.Count > 0) {
            MsgBox.ShowErrorAsync ("Error", "• " + string.Join ("\n• ", errors));
            return;
         }

         bool isEdit = action.IsEdit ();
         if (isEdit) Update (category); else Insert (category);

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

         return errors;
      }
      public void Filter (string text) {
         if (AllCategories is null || FilterCategories is null) return;
         string filterValue = text?.Trim () ?? string.Empty;

         FilterCategories.Filter (AllCategories, c => string.IsNullOrEmpty (filterValue) ||
                         (c.Name?.Contains (filterValue, StringComparison.OrdinalIgnoreCase) ?? false) ||
                         c.Code?.ToString ().Contains (filterValue, StringComparison.OrdinalIgnoreCase) == true);
      }

      public void SoryBy (string text) {
         if (AllCategories is null || FilterCategories is null) return;
         bool showAll = !Is.IsNotEmpty (text) || text.Equals ("All", StringComparison.OrdinalIgnoreCase);
         FilterCategories.Filter (AllCategories, showAll ? _ => true : c => c.Name?.StartsWith (text, StringComparison.OrdinalIgnoreCase) == true);
      }

      #region Fields
      AutoNumberedCollection<Category>? AllCategories { get; } = new AutoNumberedCollection<Category> ();
      public AutoNumberedCollection<Category>? FilterCategories { get; set; } = new AutoNumberedCollection<Category> ();
      #endregion
   }
}
