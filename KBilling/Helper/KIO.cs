using KBilling.Model;
using System;
using System.IO;

namespace KBilling.Helper;
   public static class KIO {
   public static void LoadDataFromFile (string path) {
      path = @"C:\Users\Samyraj\Downloads\FancyStoreItemList.txt";

      foreach (string line in File.ReadLines (path)) {
         string[] parts = line.Split ('|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
         if (parts.Length == 3) {
            var numName = parts[0].Split ('.', 2, StringSplitOptions.TrimEntries);
            var category = new Category {
               Name = numName.Length > 1 ? numName[1] : "",
               Prefix = parts[1],
               Code = parts[2]
            };
            App.Repo.Category.Insert (category);
         }
      }
   }
}
