using Microsoft.Data.SqlClient;
using System.Data;
using System;
using static KBilling.Helper.SqlP;

namespace KBilling.Helper;
public static class SqlP {

   // Used to define parameters in a concise way & pass to PList method
   public readonly struct Param {
      public string Name { get; }
      public object? Value { get; }
      public SqlDbType Type { get; }
      public int Size { get; }
      public byte Precision { get; }
      public byte Scale { get; }

      public Param (string name, object? value, SqlDbType type, int size = 0, byte precision = 0, byte scale = 0) {
         Name = name;
         Value = value;
         Type = type;
         Size = size;
         Precision = precision;
         Scale = scale;
      }
   }

   // Accepts array of Param structs & returns SqlParameter array
   public static SqlParameter[] PList (params Param[] items) {
      var result = new SqlParameter[items.Length];

      for (int i = 0; i < items.Length; i++) {
         var p = items[i];
         result[i] = CreateParam (p.Name, p.Value, p.Type, p.Size, p.Precision, p.Scale);
      }

      return result;
   }

   // Handles nulls, sizes, precision/scale, direction & returns SqlParameter
   static SqlParameter CreateParam (
       string name,
       object? value,
       SqlDbType dbType,
       int size = 0,
       byte precision = 0,
       byte scale = 0) {

      var p = new SqlParameter (name, dbType) {
         Value = value ?? DBNull.Value
      };

      if (size > 0 && IsSizedType (dbType))
         p.Size = size;

      if (dbType == SqlDbType.Decimal) {
         if (precision > 0) p.Precision = precision;
         if (scale > 0) p.Scale = scale;
      }

      if (value is ParameterDirection dir)
         p.Direction = dir;

      return p;
   }

   // Check if type requires size & handle accordingly
   static bool IsSizedType (SqlDbType type) =>
      type is SqlDbType.NVarChar
      or SqlDbType.VarChar
      or SqlDbType.NChar
      or SqlDbType.Char
      or SqlDbType.VarBinary
      or SqlDbType.Binary;
}

public static class P {
   // Helper Shortcuts for value, string, decimal, output params & more

   public static Param Val (string name, object? value, SqlDbType type)
       => new (name, value, type);

   public static Param Str (string name, object? value, SqlDbType type, int size)
       => new (name, value, type, size: size);

   public static Param Dec (string name, decimal? value, SqlDbType type, byte precision, byte scale)
       => new (name, value, type, precision: precision, scale: scale);

   public static Param Out (string name, ParameterDirection dir, SqlDbType type, int size = 0)
       => new (name, dir, type, size);
}
