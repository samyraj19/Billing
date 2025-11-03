using CommunityToolkit.Mvvm.ComponentModel;
using KBilling.Interfaces;

namespace KBilling.Model {
   public abstract class BaseModel : ObservableObject {
      protected IGlobalRepo Repo => App.Repo;
   }
}
