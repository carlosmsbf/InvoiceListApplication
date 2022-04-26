using AS4_CBarros9148.Models.DBGenerated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AS4_CBarros9148.Models.ViewModels
{
    public class VendorRecordViewModel
    {
        public string ActionName { get; set; }

        public Vendor Vendor{ get; set; }

        public int SelectedGroupNameFilterId { get; set; }

        public List<Term> Terms { get; set; }

        public List<GeneralLedgerAccount> GeneralAccounts { get; set; }

    }
}
