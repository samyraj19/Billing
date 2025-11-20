using Microsoft.Data.SqlClient;
using System.Data;
using System;

namespace KBilling.Helper;
public static class SqlParamHelper {
   // -----------------------------
   // MAIN SHORT API
   // -----------------------------

   // 1) Simple parameters: ("@Name", value)
   public static SqlParameter[] PList (
       params (string name, object? value)[] items) {
      var result = new SqlParameter[items.Length];
      for (int i = 0; i < items.Length; i++) {
         var (name, value) = items[i];
         result[i] = CreateParam (name, value);
      }
      return result;
   }

   // 2) String parameters: ("@Name", value, size)
   public static SqlParameter[] PList (
       params (string name, object? value, int size)[] items) {
      var result = new SqlParameter[items.Length];
      for (int i = 0; i < items.Length; i++) {
         var (name, value, size) = items[i];
         result[i] = CreateParam (name, value, size: size);
      }
      return result;
   }

   // 3) Decimal parameters: ("@Name", value, precision, scale)
   public static SqlParameter[] PList (
       params (string name, object? value, byte precision, byte scale)[] items) {
      var result = new SqlParameter[items.Length];
      for (int i = 0; i < items.Length; i++) {
         var (name, value, precision, scale) = items[i];
         result[i] = CreateParam (name, value, precision: precision, scale: scale);
      }
      return result;
   }

   // -----------------------------
   // CORE PARAM CREATION LOGIC
   // -----------------------------
   private static SqlParameter CreateParam (
       string name,
       object? value,
       int size = 0,
       byte precision = 0,
       byte scale = 0) {
      var type = InferType (value);

      var p = new SqlParameter (name, type) {
         Value = value ?? DBNull.Value
      };

      if (size > 0 && IsSizedType (type))
         p.Size = size;

      if (type == SqlDbType.Decimal) {
         if (precision > 0) p.Precision = precision;
         if (scale > 0) p.Scale = scale;
      }

      return p;
   }

   private static SqlDbType InferType (object? value) =>
       value switch {
          null => SqlDbType.Variant,
          string => SqlDbType.NVarChar,
          int => SqlDbType.Int,
          long => SqlDbType.BigInt,
          decimal => SqlDbType.Decimal,
          double => SqlDbType.Float,
          float => SqlDbType.Real,
          DateTime => SqlDbType.DateTime2,
          bool => SqlDbType.Bit,
          byte[] => SqlDbType.VarBinary,
          _ => SqlDbType.Variant
       };

   private static bool IsSizedType (SqlDbType type) =>
       type is SqlDbType.NVarChar
       or SqlDbType.VarChar
       or SqlDbType.NChar
       or SqlDbType.Char
       or SqlDbType.VarBinary
       or SqlDbType.Binary;
}
