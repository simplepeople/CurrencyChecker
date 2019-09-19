using System;
using System.Collections.Generic;

namespace BPSTests
{
    public class DomainBankCoursesDto
    {
        public string Bank { get; set; }

        public DateTime BankCourseTime { get; set; }

        public DateTime RequestTime { get; set; }

        public List<CurrencyPairInfo> CurrencyPairInfos { get; set; }

    }
}