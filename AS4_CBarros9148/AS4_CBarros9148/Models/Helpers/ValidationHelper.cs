using AS4_CBarros9148.Models.DBGenerated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AS4_CBarros9148.Models.Helpers
{
    public class ValidationHelper
    {
        public static string PhoneNumberExists(apContext context, string vendorPhone)
        {
            string msg = "";

            if (!string.IsNullOrEmpty(vendorPhone))
            {
                string numericPhoneNumber = new String(vendorPhone.Where(Char.IsDigit).ToArray());

                bool match = false;

                foreach (var patient in context.Vendors.ToList())
                {
                    if (patient.VendorPhone != null)
                    {
                        string vendorPhoneNumber = new String(patient.VendorPhone.Where(Char.IsDigit).ToArray());

                        match = vendorPhoneNumber == numericPhoneNumber;

                        if (match)
                        {
                            break;
                        }
                    }
                }

                if (match)
                    msg = $"Another Vendor has the same number as yours ( {vendorPhone} ).";
            }
            return msg;
        }

    }
}
