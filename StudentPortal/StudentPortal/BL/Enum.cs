using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentPortal.BL
{
    public enum InvoiceType
    {
        CourseFee = 1,
        Fine = 2
    }

    public enum InvoicePaymentStatus
    {
        OutStanding = 0,
        Paid = 1
    }
}