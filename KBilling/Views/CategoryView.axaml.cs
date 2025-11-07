using System;
using System.IO;
using System.Xml.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using KBilling.Helper;
using KBilling.Model;
using KBilling.ViewManagement;
using KBilling.ViewModel;
using MsBox.Avalonia.Enums;

namespace KBilling;
public partial class CategoryView : UserControl {
   public CategoryView () {
      InitializeComponent ();
      Loaded += OnLoaded;
      Unloaded += OnUnload;
   }

   void OnLoaded (object? sender, RoutedEventArgs e) {
      SearchTxt.TextChanging += OnTextChanging;

      SortByCmb.SelectionChanged += OnCmbChanged;
      DGCategory.SelectionChanged += OnSelectionChanged;

      BtnSubmit.Click += OnSubmit;
      BtnCancel.Click += OnCancel;
      VM ().GetAll ();
      BindAlpha ();
   }

   void OnCancel (object? sender, RoutedEventArgs e) { }

   void OnSubmit (object? sender, RoutedEventArgs e) =>VM ().Submit (mAction, VM());

   void OnUnload (object? sender, Avalonia.Interactivity.RoutedEventArgs e) {
      SearchTxt.TextChanging -= OnTextChanging;
      SortByCmb.SelectionChanged -= OnCmbChanged;
      DGCategory.SelectionChanged -= OnSelectionChanged;
   }

   void OnCmbChanged (object? sender, SelectionChangedEventArgs e) {
      if (sender is not ComboBox cmb || cmb.SelectedItem is not string item) return;
      VM ().SoryBy (item);
   }

   void OnSelectionChanged (object? sender, SelectionChangedEventArgs e) {
      if (DGCategory.SelectedItem is not Category category) return;
      mCat = category;
   }

   void OnTextChanging (object? sender, TextChangingEventArgs e) => VM ().Filter ((sender as TextBox)?.Text?.Trim () ?? string.Empty);

   async void OnActionClick (object? sender, RoutedEventArgs e) {
      if ((sender as Button)?.Tag?.ToString () is not string action) return;
      mAction = action.GetEAction ();
      if (mAction.IsEdit () && mCat is Category category) VM ().Edit (category);
      else if (mAction.IsDelete () && mCat is Category category1) {
         var box = await AppMsg.AskDelItem ();
         if (box == ButtonResult.Yes) VM ().Delete (category1.CategoryId);
      } else await AppMsg.AskItem ();
   }

   void BindAlpha () => SortByCmb.ItemsSource = AlphaList.Get ();

   void LoadDataFromFile () {
      string path = @"C:\Users\Samyraj\Downloads\FancyStoreItemList.txt";

      foreach (string line in File.ReadLines (path)) {
         string[] parts = line.Split ('|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
         if (parts.Length == 3) {
            var numName = parts[0].Split ('.', 2, StringSplitOptions.TrimEntries);
            var category = new Model.Category {
               Name = numName.Length > 1 ? numName[1] : "",
               Prefix = parts[1],
               Code = parts[2]
            };
            App.Repo.Category.Insert (category);
         }
      }
   }

   CategoryVM VM () => DataContext is CategoryVM vm ? vm : throw new InvalidOperationException ("DataContext is not of type CategoryVM");

   #region Fields
   EAction mAction = EAction.None;
   Category? mCat;
   #endregion
}