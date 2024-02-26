using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using StudentPortal.Models;

namespace StudentPortal.BL
{
    //Finance API caller helper class
    public class FinancePortalHelper
    {
        //Read API url from config file
        private string API_URL = ConfigurationManager.AppSettings["FinanceAPIUrl"];

        //Register user with finance portal
        public string RegisterUser(string userId)
        {
            var url = $"{API_URL}CreateAccount";

            var apiHelper = new APIHelper();

            var response = apiHelper.PostMethod(url, userId);

            var result = JsonConvert.DeserializeObject<string>(response);
            return result;
        }

        public string CreateInvoice(string studentId, double fee)
        {
            var obj = new Invoice
            {
                StudentId = studentId,
                Fee = fee,
                InvoiceType = InvoiceType.CourseFee
            };

            var url = $"{API_URL}CreateInvoice";

            var apiHelper = new APIHelper();

            var response = apiHelper.PostMethod(url, obj);

            var result = JsonConvert.DeserializeObject<string>(response);

            return result;
        }

        public List<Invoice> GetAllStudentInvoice(string studentId)
        {
            var url = $"{API_URL}GetStudentInvoices?studentId={studentId}";

            var apiHelper = new APIHelper();

            var response = apiHelper.GetMethod(url).Result;

            var result = JsonConvert.DeserializeObject<List<Invoice>>(response);

            return result;
        }

        public bool CheckGraduationStatus(string studentId)
        {
            var url = $"{API_URL}CheckPendingInvoice?studentId={studentId}";

            var apiHelper = new APIHelper();

            var response = apiHelper.GetMethod(url).Result;

            var result = JsonConvert.DeserializeObject<bool>(response);

            return result;
        }

        public bool GetInvoiceDetail(string invoiceId)
        {
            var url = $"{API_URL}GetInvoice?invoiceId={invoiceId}";

            var apiHelper = new APIHelper();

            var response = apiHelper.GetMethod(url).Result;

            var result = JsonConvert.DeserializeObject<bool>(response);

            return result;
        }
    }
}