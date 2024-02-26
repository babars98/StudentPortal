using StudentPortal.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentPortal.Models
{
    public class Invoice
    {
        public string InvoiceId { get; set; }
        public string StudentId { get; set; }
        public InvoiceType InvoiceType { get; set; }
        public double Fee { get; set; }
        public DateTime? DueDate { get; set; }
        public InvoicePaymentStatus IsPaid { get; set; }
        public DateTime? PaymentDate { get; set; }
    }
}