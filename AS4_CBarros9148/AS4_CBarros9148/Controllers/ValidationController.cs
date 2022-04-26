using AS4_CBarros9148.Models.DBGenerated;
using AS4_CBarros9148.Models.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AS4_CBarros9148.Controllers
{
    public class ValidationController : Controller
    {

        private apContext context;

        public ValidationController(apContext dbContext)
        {
            context = dbContext;
        }

        public JsonResult CheckPhoneNumber(Vendor vendor)
        {
            string msg = ValidationHelper.PhoneNumberExists(context, vendor.VendorPhone);

            if (string.IsNullOrEmpty(msg))
            {
                TempData["okPhoneNumber"] = true;
                return Json(true);
            }
            else
            {
                return Json(msg);
            }
        }


    }
}
