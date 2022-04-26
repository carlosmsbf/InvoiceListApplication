using AS4_CBarros9148.Models;
using AS4_CBarros9148.Models.DBGenerated;
using AS4_CBarros9148.Models.Helpers;
using AS4_CBarros9148.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace AS4_CBarros9148.Controllers
{
    public class HomeController : Controller
    {
        private apContext context;

        public HomeController(apContext apContext)
        {
            context = apContext;
        }



        [HttpGet]
        public IActionResult Index(string cityFilter="All", int nameGroupFilterId = 0)
        {
            var vendorList = context.Vendors.ToList();

            List<NameGroupFilter> nameGroupFilterList = VendorHelper.GetNameGroupFilters();

            nameGroupFilterList.Insert(0, new NameGroupFilter()
            {
                NameGroupId=0,
                GroupName="All",
                LowerLetter='A',
                UpperLetter='Z'

            });

         

            if(HttpContext.Request.Cookies["groupNameFilter"] != null)
            {
                HttpContext.Response.Cookies.Append("groupNameFilter", nameGroupFilterId.ToString());
            }

            var selectedGroupNameFilterId = nameGroupFilterId;

            if (selectedGroupNameFilterId != 0)
            {
                var selectedNameGroup = nameGroupFilterList
                    .Where(w => w.NameGroupId == selectedGroupNameFilterId).FirstOrDefault();

                vendorList = vendorList.Where(w=>w.VendorName[0] >= selectedNameGroup.LowerLetter 
                && w.VendorName[0] <= selectedNameGroup.UpperLetter)
                    .OrderBy(ob => ob.VendorName).ToList();
            }

            vendorList = vendorList.Where(w => w.IsDeleted == false).ToList();
            VendorListViewModel vlvm = new VendorListViewModel()
            {
                Vendors = vendorList,
                SelectedGroupNameFilterId=selectedGroupNameFilterId,
                NameGroupFilters= nameGroupFilterList
            };
            return View(vlvm);

        }

        public IActionResult SoftDelete(int id)
        {
            var vendorRecord = context.Vendors.Find(id);

            if (vendorRecord != null)
            {
                vendorRecord.IsDeleted = true;
                context.Update(vendorRecord);
                context.SaveChanges();
                TempData["DeletedVendorName"] = vendorRecord.VendorName;
                TempData["VendorID"] = vendorRecord.VendorId;
            }

            return RedirectToAction("Index");
        }



        public IActionResult UndoDelete(int id)
        {
            var vendorDeletedRecord = context.Vendors.Find(id);

            if (vendorDeletedRecord != null)
            {
                vendorDeletedRecord.IsDeleted = false;
                context.Update(vendorDeletedRecord);
                context.SaveChanges();
            }

            return RedirectToAction("Index");
        }











        //[HttpGet]
        //public IActionResult Index(string cityFilter = "All", int nameGroupFilterId = 0)
        //{
        //    var vendorList = context.Vendors.ToList();

        //    List<string> cityFilterList = vendorList.Select(s => s.VendorCity).Distinct().ToList();
        //    List<NameGroupFilter> nameGroupFilterList = VendorHelper.GetNameGroupFilters();

        //    //City
        //    cityFilterList.Insert(0, "All");
        //    //Name
        //    nameGroupFilterList.Insert(0, new NameGroupFilter()
        //    {
        //        NameGroupId = 0,
        //        GroupName = "All",
        //        LowerLetter = 'A',
        //        UpperLetter = 'Z'

        //    });
        //    //City
        //    var selectedCityFilter = cityFilter;
        //    //Name
        //    var selectedGroupNameFilterId = nameGroupFilterId;

        //    //city
        //    if (selectedCityFilter != "All")
        //    {
        //        vendorList = vendorList.Where(w => w.VendorCity == selectedCityFilter).ToList();
        //    }
        //    //name

        //    if (selectedGroupNameFilterId != 0)
        //    {
        //        var selectedNameGroup = nameGroupFilterList
        //            .Where(w => w.NameGroupId == selectedGroupNameFilterId).FirstOrDefault();

        //        vendorList = vendorList.Where(w => w.VendorName[0] >= selectedNameGroup.LowerLetter
        //        && w.VendorName[0] <= selectedNameGroup.UpperLetter).OrderBy(ob => ob.VendorName).ToList();
        //    }

        //    //city
        //    VendorListViewModel vlvm = new VendorListViewModel()
        //    {
        //        Vendors = vendorList,
        //        CityFilters = cityFilterList,
        //        SelectedCity = selectedCityFilter,
        //        SelectedGroupNameFilterId = selectedGroupNameFilterId,
        //        NameGroupFilters = nameGroupFilterList


        //    };



        //    return View(vlvm);

        //}











        //[HttpGet]
        //public IActionResult Index(string cityFilter = "All", int nameGroupFilterId = 0)
        //{
        //    return View();
        //    List<Vendor> patientList = context.Vendors.ToList();
        //    List<NameGroupFilter> nameGroupFilters = VendorHelper.GetNameGroupFilters();
        //    List<string> cityfilterList = patientList.Select(s => s.VendorCity).Distinct().ToList();

        //    // Enhancement, don't show yet (after returning from "Edit" page
        //    if (HttpContext.Request.Cookies["groupNameFilter"] != null)
        //    {
        //        HttpContext.Response.Cookies.Append("groupNameFilter", nameGroupFilterId.ToString());
        //    }

        //    var selectedCityFilter = cityFilter;
        //    var selectedGroupNameFilterId = nameGroupFilterId;

        //    cityfilterList.Insert(0, "All");
        //    nameGroupFilters.Insert(0, new NameGroupFilter() { NameGroupId = 0, GroupName = "All", LowerLetter = 'A', UpperLetter = 'Z' });

        //    if (selectedCityFilter != "All")
        //    {
        //        patientList = patientList.Where(w => w.VendorCity == cityFilter).ToList();
        //    }

        //    var selectedNameGroup = new NameGroupFilter();

        //    selectedNameGroup = nameGroupFilters.Where(w => w.NameGroupId == nameGroupFilterId).FirstOrDefault();

        //    patientList = patientList
        //                    .Where(w => w.VendorName[0] >= selectedNameGroup.LowerLetter
        //                            && w.VendorName[0] <= selectedNameGroup.UpperLetter)
        //                    .OrderBy(ob => ob.VendorName)
        //                    .ToList();

        //    patientList = patientList.Where(w => w.IsDeleted == false).ToList();

        //    VendorListViewModel plvm = new VendorListViewModel()
        //    {
        //        Vendors = patientList,
        //        CityFilters = cityfilterList,
        //        SelectedCity = selectedCityFilter,
        //        SelectedGroupNameFilterId = selectedGroupNameFilterId,
        //        NameGroupFilters = nameGroupFilters
        //    };

        //    return View(plvm);

        //}



    }
}
