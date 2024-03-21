using StudentPortal.Models;

namespace StudentPortal.Interface
{
    public interface IFinancePortalHelper
    {
        public string RegisterUser(string userId);

        public string CreateInvoice(string studentId, double fee);

        public List<Invoice> GetAllStudentInvoice(string studentId);

        public bool CheckGraduationEligibility(string studentId);

        public bool GetInvoiceDetail(string invoiceId);
    }
}
