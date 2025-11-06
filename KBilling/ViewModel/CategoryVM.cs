using System;
using KBilling.Core;
using KBilling.Extension;
using KBilling.Model;

namespace KBilling.ViewModel {
   public class CategoryVM : Category {
      public CategoryVM () { }

      bool Insert (Category category) => Repo.Category.Insert (category);

      public void GetAll () {
         var categories = Repo.Category.GetAll ();
         AllCategories?.Clear ();
         AllCategories?.AddRange (categories);
         Filter (string.Empty);
      }

      public void Filter (string text) {
         if (AllCategories is null) return;
         string filterValue = text?.Trim () ?? string.Empty;

         FilterCategories?.Filter (AllCategories, c => string.IsNullOrEmpty (filterValue) ||
                         c.Name.Contains (filterValue, StringComparison.OrdinalIgnoreCase) ||
                         c.Code.ToString ().Contains (filterValue, StringComparison.OrdinalIgnoreCase));
      }

      AutoNumberedCollection<Category>? AllCategories { get; } = [];
      public AutoNumberedCollection<Category>? FilterCategories { get; set; } = [];
   }
}
