using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using KBilling.Model;
using KBilling.Services;
using KBilling.ViewManagement;
using KBilling.ViewModel;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using Tmds.DBus.Protocol;

namespace KBilling;
public partial class Login : Window {
   public Login () {
      InitializeComponent ();
      BtnLogin.Click += BtnLoginClick;
   }

   void BtnLoginClick (object? sender, RoutedEventArgs e) {
      if (VM is not null) {
         if (!CanLogin ()) {
            var box = MessageBoxManager.GetMessageBoxStandard ("Login", "Username and Password are required.", ButtonEnum.Ok);
            var result = box.ShowAsync ();
            return;
         }

         bool isauthenticate = VM.Authenticate (VM?.User, VM.Password);
         if (isauthenticate) {
            AppSession.Login (new Model.UserModel { Username = VM.User, Role = VM.User.ToRole () });
            var mainwindow = WindowRegistry.Instance.Get ("MainWindow");
            mainwindow.Show ();
            Close ();
         } else {
            var box = MessageBoxManager.GetMessageBoxStandard ("Login", "Invalid Username or Password.", ButtonEnum.Ok);
            var result = box.ShowAsync ();
            return;
         }
      }
   }

   bool CanLogin () => !(string.IsNullOrWhiteSpace (VM?.User) || string.IsNullOrWhiteSpace (VM?.Password));

   #region Fields
   LoginVM? VM => DataContext as LoginVM;
   #endregion
}