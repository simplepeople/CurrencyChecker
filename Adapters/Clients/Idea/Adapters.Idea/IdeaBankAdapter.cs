using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Adapters.Common;
using Domain.Models;

namespace Adapters.Idea
{
    public class IdeaBankAdapter : BankAdapter
    {
        public override Task<DomainBankCoursesDto> GetBankCourses()
        {
            return Task.FromResult(new DomainBankCoursesDto
            {
                Bank = BankName,
                BankCourseTime = DateTime.Now,
                CurrencyPairInfos = new List<CurrencyPairInfo>()
            });
        }

        public override string BankName { get; } = "IdeaBank";
    }
}
