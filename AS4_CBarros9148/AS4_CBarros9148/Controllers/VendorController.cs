using AS4_CBarros9148.Models;
using AS4_CBarros9148.Models.DBGenerated;
using AS4_CBarros9148.Models.Helpers;
using AS4_CBarros9148.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AS4_CBarros9148.Controllers
{
    public class VendorController : Controller
    {

        private apContext context;

        public VendorController(apContext apContext)
        {
            context = apContext;
        }


        [HttpGet]
        public IActionResult Record(string actionType, int groupNameFilterId, int id = 0)
        {
            Vendor vendor = null;

            if (actionType == "Add")
            {
                vendor = new Vendor();
            }
            else if (actionType == "Edit")
            {
                vendor = context.Vendors.Find(id);
            }

            HttpContext.Response.Cookies.Append("groupNameFilter", groupNameFilterId.ToString());
            //HttpContext.Response.Cookies.Append("actionType", actionType);

            VendorRecordViewModel vrvm = new VendorRecordViewModel()
            {
                Vendor = vendor,
                ActionName = actionType,
                SelectedGroupNameFilterId = groupNameFilterId,
                Terms = context.Terms.ToList(),
                GeneralAccounts = context.GeneralLedgerAccounts.ToList()
            };

            return View(vrvm);
        }

        [HttpPost]
        public IActionResult Record(Vendor vendor, string actionName)
        {
            if (ModelState.IsValid)
            {
                if (vendor.VendorId == 0)
                {
                    context.Vendors.Add(vendor);
                    context.SaveChanges();
                }
                else
                {
                    context.Update(vendor);
                    context.SaveChanges();
                }

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Sorry, the form has an error.");

                int groupNameFilter = 0;


                if (HttpContext.Request.Cookies["groupNameFilter"] != null)
                {
                    groupNameFilter = int.Parse(HttpContext.Request.Cookies["groupNameFilter"]);
                }

                VendorRecordViewModel vrvm = new VendorRecordViewModel()
                {
                    Vendor = vendor,
                    ActionName = actionName,
                    Terms = context.Terms.ToList(),
                    GeneralAccounts = context.GeneralLedgerAccounts.ToList()
                };

                return View(vrvm);

            }

        }

        [HttpGet]
        public IActionResult InvoiceList(int id, int groupNameFilterId, int invoiceId = 0)
        {
            var vendors = context.Vendors.Include(i => i.Invoices).ThenInclude(ti => ti.InvoiceLineItems);
            var vendorRecord = vendors.Where(w => w.VendorId == id).FirstOrDefault();
            var term = context.Terms.Find(vendorRecord.DefaultTermsId);
            var accountNumber = context.GeneralLedgerAccounts.Find(vendorRecord.DefaultAccountNumber);
            var invoices = vendorRecord.Invoices.ToList();
            var nameGroupFilter = new NameGroupFilter();

            HttpContext.Response.Cookies.Append("groupNameFilter", groupNameFilterId.ToString());

            if (!string.IsNullOrEmpty(HttpContext.Request.Cookies["groupNameFilter"]))
            {
                int nameGroupFilterId = int.Parse(HttpContext.Request.Cookies["groupNameFilter"]);
                nameGroupFilter = VendorHelper.GetNameGroupFilters()
                    .Where(w => w.NameGroupId == nameGroupFilterId)
                    .FirstOrDefault();
            }

            var selectedInvoiceID = 0;
            Invoice selectedInvoice = null;
            List<InvoiceLineItem> prescriptionLineItems = new List<InvoiceLineItem>();

            if (invoiceId != 0)
            {
                selectedInvoiceID = invoiceId;
                selectedInvoice = invoices.Where(w => w.InvoiceId == selectedInvoiceID).FirstOrDefault();
                prescriptionLineItems = selectedInvoice.InvoiceLineItems.ToList();
            }
            else if (invoices.Count() > 0)
            {
                selectedInvoiceID = invoices.First().InvoiceId;
                selectedInvoice = invoices.First();
                prescriptionLineItems = selectedInvoice.InvoiceLineItems.ToList();
            }

            VendorInvoiceViewModel vivm = new VendorInvoiceViewModel()
            {
                VendorName = vendorRecord.VendorName,
                Terms = (term.TermsDueDays).ToString() + " Days",
                Address1 = vendorRecord.VendorAddress1,
                NameGroupFilter = nameGroupFilter,
                Invoices = invoices,
                AccountNumberID = vendorRecord.DefaultAccountNumber,
                SelectedInvoiceID = selectedInvoiceID,
                SelectedInvoice = selectedInvoice,
                InvoiceLineItems = prescriptionLineItems
            };

            return View(vivm);
        }

        [HttpPost]
        public IActionResult AddNewInvoiceLineItem(int id, int invoiceId, int accountNumber, string description, string amount)
        {

            var lastPrescriptionSequence = context.InvoiceLineItems
                .Where(w => w.InvoiceId == invoiceId)
                .OrderByDescending(obd => obd.InvoiceSequence)
                .Select(s => s.InvoiceSequence)
                .FirstOrDefault();

            lastPrescriptionSequence += 1;

            InvoiceLineItem lineItem = new InvoiceLineItem()
            {
                InvoiceId = invoiceId,
                InvoiceSequence = lastPrescriptionSequence,
                AccountNumber = accountNumber,
                LineItemAmount = decimal.Parse(amount),
                LineItemDescription = description
            };

            context.InvoiceLineItems.Add(lineItem);

            var invoice = context.Invoices.Find(invoiceId);
            invoice.InvoiceTotal += decimal.Parse(amount);

            context.Invoices.Update(invoice);
            context.SaveChanges();

            return RedirectToAction("InvoiceList", new { id, invoiceId });
        }

    }

}
