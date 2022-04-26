using AS4_CBarros9148.Models.DBGenerated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AS4_CBarros9148.Models.ViewModels
{
    public class VendorListViewModel
    {
        public List<Vendor> Vendors { get; set; }

        public List<string> CityFilters { get; set; }

        public string SelectedCity { get; set; }

        public List<NameGroupFilter> NameGroupFilters { get; set; }

        public int SelectedGroupNameFilterId { get; set; }

        public string GetActiveGroupNameFilter(int groupNameFilterId)
        {
            return groupNameFilterId == SelectedGroupNameFilterId ? "active" : "";
        }


    }
}
