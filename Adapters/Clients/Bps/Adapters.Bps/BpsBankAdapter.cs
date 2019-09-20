using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Adapters.Common;
using Common.Extensions;
using Domain.Models;

namespace Adapters.Bps
{
    public class BpsBankAdapter : BankAdapter
    {
        private static readonly HttpClient _client = new HttpClient();

        public override string BankName { get; } = "BPS Sberbank";

        public override async Task<DomainBankCoursesDto> GetBankCourses()
        {
            string url = "https://www.bps-sberbank.by/SBOLServer/rest/currency/rates?pck=CD";
            var request = new CardlessRequest { date = DateTime.Now.Ticks.ToString() };
            var response = await _client.PostAsJsonAsync<CardlessRequest, CardlessResponse>(url, request);
            return ConvertRawCourses(response);
        }

        private DomainBankCoursesDto ConvertRawCourses(CardlessResponse rawBankCoursesDto)
        {
            return new DomainBankCoursesDto
            {
                Bank = BankName,
                BankCourseTime = rawBankCoursesDto.date.DateFromTicks(),
                RequestTime = DateTime.Now,
                CurrencyPairInfos = rawBankCoursesDto.rates.list.Select(x => new CurrencyPairInfo
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