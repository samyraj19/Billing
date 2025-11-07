using System.Data;
using System;
using KBilling.Interfaces;
using KBilling.Model;
using Microsoft.Data.SqlClient;
using KBilling.DataBase;
using System.Collections.Generic;
using System.Linq;

namespace KBilling.Services {
   public class CategoryRepo : ICategoryRepo {

      public IEnumerable<Category> GetAll () {
         DataTable dt = App.Repo.QueryExe.QuerySP (SP.Categories.GetAll);
         if (dt.Rows.Count == 0) return Enumerable.Empty<Category> (); // safe, no nulls
         var categories = new List<Category> ();
         foreach (DataRow row in dt.Rows) {
            var cat = new Category {
               CategoryId = (int)row["CategoryId"],
               Code = (string)row["ProductCode"],
               Name = (string)row["CategoryName"],
               Prefix = (string)row["Prefix"],
               Description = (string)row["ItemDescription"],
            };
            categories.Add (cat);
         }
         return categories;
      }

      public bool Insert (Category c) {
         ArgumentNullException.ThrowIfNull (c);
         var parameters = new[]
         {
              new SqlParameter("@Name", SqlDbType.NVarChar,150) { Value = c.Name },
              new SqlParameter("@Prefix", SqlDbType.NVarChar, 150) { Value = c.Prefix ?? string.Empty },
              new SqlParameter("@Code", SqlDbType.NVarChar, 150) { Value = c.Code ?? string.Empty },
              new SqlParameter("@Description", SqlDbType.NVarChar, 150) { Value = c.Description ?? string.Empty },
         };
         App.Repo.QueryExe.ExecuteSP (SP.Categories.Insert, parameters);
         return true;
      }

      public bool Update (Category c) {
         ArgumentNullException.ThrowIfNull (c);
         var parameters = new[]{
              new SqlParameter("@Id", SqlDbType.Int) { Value = c.CategoryId},
              new SqlParameter("@Name", SqlDbType.NVarChar,100) { Value = c.Name ?? string.Empty },
              new SqlParameter("@Prefix", SqlDbType.NVarChar, 10) { Value = c.Prefix ?? string.Empty },
              new SqlParameter("@Code", SqlDbType.NVarChar,100) { Value = c.Code ?? string.Empty },
              new SqlParameter("@Description", SqlDbType.NVarChar,150) { Value = c.Description ?? string.Empty },
         };
         App.Repo.QueryExe.ExecuteSP (SP.Categories.Update, parameters);
         return true;
      }

      public bool Delete (int id) {
         var parameters = new[]{
              new SqlParameter("@Id", SqlDbType.Int) { Value = id },
         };
         App.Repo.QueryExe.ExecuteSP (SP.Categories.Delete, parameters);
         return true;
      }
   }
}
