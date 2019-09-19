using System;
using System.Linq;
using BPSTests.Requests.GetCourses.BPS;

namespace BPSTests
{
    public class InfoDtoConverter
    {
        public DomainBankCoursesDto Convert(CardlessResponse bankRaw)
        {
            return new DomainBankCoursesDto
            {
                Bank = "BPS Sberbank",
                BankCourseTime = bankRaw.date.DateFromTicks(),
                RequestTime = DateTime.Now,
                CurrencyPairInfos = bankRaw.rates.list.Select(x => new CurrencyPairInfo
                {
                    SellCourse = x.sale,
                    XCurrency = x.iso,
                    YCurrency = "BYN",
                    BuyCourse = x.buy,
                    DeltaBuy = x.deltaBuy,
                    DeltaSale = x.deltaSale
                }).ToList()
            };
        }
    }
}