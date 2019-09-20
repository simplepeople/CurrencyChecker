using System.Threading.Tasks;
using Domain.Models;

namespace Adapters.Common
{
    public abstract class BankAdapter : IBankAdapter
    {
        public abstract Task<DomainBankCoursesDto> GetBankCourses();

        public abstract string BankName { get; }
    }
}