using AS4_CBarros9148.Models.DBGenerated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AS4_CBarros9148.Models.ViewModels
{
    public class VendorInvoiceViewModel
    {

        public string VendorName { get; set; }

        public string Address1 { get; set; }

        public string Terms { get; set; }

        public int SelectedInvoiceID { get; set; }

        public int AccountNumberID { get; set; }

        public NameGroupFilter NameGroupFilter { get; set; }

        public List<Invoice> Invoices { get; set; }

        public Invoice SelectedInvoice { get; set; }

        public List<InvoiceLineItem> InvoiceLineItems { get; set; }

        public string GetActiveInvoice(int InvoiceId)
        {
            return SelectedInvoiceID == InvoiceId ? "active" : string.Empty;
        }





    }
}
