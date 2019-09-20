using System.Threading.Tasks;
using Domain.Models;

namespace Adapters.Common
{
    public interface ICourseGetter
    {
        Task<DomainBankCoursesDto> GetBankCourses();
    }
}
