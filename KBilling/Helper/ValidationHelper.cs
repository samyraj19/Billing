using System.Linq;
using System.Text.RegularExpressions;

namespace KBilling.Helper {
   public class Is {

      /// <summary>Checks if the input string is a positive integer (digits only).</summary>
      public static bool Integer (string? input) => !string.IsNullOrEmpty (input) && input.All (char.IsDigit);

      /// <summary>Checks if the input string is a valid decimal number.</summary>
      public static bool Decimal (string? input) => decimal.TryParse (input, out _);

      /// <summary>Checks if the input string is a valid email.</summary>
      public static bool Email (string? input) {
         if (string.IsNullOrWhiteSpace (input)) return false;
         var pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
         return Regex.IsMatch (input, pattern, RegexOptions.IgnoreCase);
      }

      /// <summary>Checks if the input string is a valid phone number (digits only, optional +).</summary>
      public static bool PhoneNumber (string? input) {
         if (string.IsNullOrWhiteSpace (input)) return false;
         var pattern = @"^\+?\d+$";
         return Regex.IsMatch (input, pattern);
      }

      /// <summary>Checks if the input string is not null or whitespace.</summary>
      public static bool NotEmpty (string? input) => !string.IsNullOrWhiteSpace (input);

      /// <summary>Checks if the input string contains only letters (a-z, A-Z).</summary>
      public static bool Alphabetic (string? input) => !string.IsNullOrEmpty (input) && input.All (char.IsLetter);

      /// <summary>Checks if the input string contains only letters or digits.</summary>
      public static bool Alphanumeric (string? input) => !string.IsNullOrEmpty (input) && input.All (char.IsLetterOrDigit);
   }
}
