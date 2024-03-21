using Newtonsoft.Json;
using StudentPortal.Interface;
using StudentPortal.Models;
using System.Dynamic;

namespace StudentPortal.BL
{
    //Finance API caller helper class
    public class FinancePortalHelper : IFinancePortalHelper
    {
        private readonly IConfigurationRoot configuration;
        private readonly string API_URL = string.Empty;

        public FinancePortalHelper()
        {
            configuration = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json")
               .AddEnvironmentVariables()
               .Build();

            //Read API url from config file
            API_URL = $"{configuration.GetValue<string>("FinanceAppUrl")}api/";
        }



        //Register user with finance portal
        public string RegisterUser(string userId)
        {
            var url = $"{API_URL}CreateAccount";

            var apiHelper = new APIHelper();

            dynamic obj = new ExpandoObject();
            obj.StudentId = userId;

            var response = apiHelper.PostMethod(url, obj);

            var result = JsonConvert.DeserializeObject<string>(response);
            return result;
        }

        /// <summary>
        /// Create Invoice if the student is enrolled in a course or borrows a book
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="fee"></param>
        /// <returns></returns>
        public string CreateInvoice(string studentId, double fee)
        {
            var obj = new Invoice
            {
                StudentId = studentId,
                Fee = fee,
                InvoiceType = InvoiceType.CourseFee,
                DueDate = DateTime.Now.AddDays(7)
            };

            var url = $"{API_URL}CreateInvoice";

            var apiHelper = new APIHelper();

            var response = apiHelper.PostMethod(url, obj);

            var result = JsonConvert.DeserializeObject<string>(response);

            return result;
        }

        /// <summary>
        /// Get all the invoices of a student
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        public List<Invoice> GetAllStudentInvoice(string studentId)
        {
            var url = $"{API_URL}GetStudentInvoices?studentId={studentId}";

            var apiHelper = new APIHelper();

            var response = apiHelper.GetMethod(url);

            var result = JsonConvert.DeserializeObject<List<Invoice>>(response);

            return result;
        }

        /// <summary>
        /// Check the Graduation eligibility of a student 
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        public bool CheckGraduationEligibility(string studentId)
        {
            var url = $"{API_URL}CheckPendingInvoice?studentId={studentId}";

            var apiHelper = new APIHelper();

            var response = apiHelper.GetMethod(url);

            var result = JsonConvert.DeserializeObject<bool>(response);

            return result;
        }

        /// <summary>
        /// Get details of an invoice
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <returns></returns>
        public bool GetInvoiceDetail(string invoiceId)
        {
            var url = $"{API_URL}GetInvoice?invoiceId={invoiceId}";

            var apiHelper = new APIHelper();

            var response = apiHelper.GetMethod(url);

            var result = JsonConvert.DeserializeObject<bool>(response);

            return result;
        }
    }
}
