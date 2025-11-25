using System.Threading.Tasks;
using KBilling.ViewModel;

namespace KBilling.Interfaces;
public interface IExport {
   byte[] PDF (InvoiceVM invoice);
}
