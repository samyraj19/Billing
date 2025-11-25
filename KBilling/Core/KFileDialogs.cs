using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using Microsoft.Identity.Client.Extensions.Msal;

namespace KBilling.Core;
public class KFileDialogs {

   public KFileDialogs (IStorageProvider? storage) => mStorage = storage;

   /// <summary>Opens a Save File dialog with custom title, suggested file name, and patterns. </summary>
   public Task<IStorageFile?> SaveAsync (string title, string suggestedName, params string[] patterns) {
      ArgumentNullException.ThrowIfNull (mStorage);

      return mStorage.SaveFilePickerAsync (new FilePickerSaveOptions {
         Title = title,
         SuggestedFileName = suggestedName,
         DefaultExtension = GetDefaultExtension (patterns),
         FileTypeChoices = new List<FilePickerFileType>
         {
            new FilePickerFileType(title) { Patterns = patterns }
         }
      });
   }

   static string GetDefaultExtension (string[] patterns) {
      if (patterns.Length == 0) return "";
      var p = patterns[0];
      return p.StartsWith ("*.") ? p.Substring (2) : "";
   }

   readonly IStorageProvider? mStorage;
}
