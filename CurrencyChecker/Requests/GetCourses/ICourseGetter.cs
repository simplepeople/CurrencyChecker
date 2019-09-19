using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BPSTests.Requests.GetCourses.BPS;

namespace BPSTests.Requests.GetCourses
{
    public interface ICourseGetter
    {
        Task<DomainBankCoursesDto> GetBankCourses();
    }

    public class BPSCourseGetter : ICourseGetter
    {
        static readonly HttpClient _client = new HttpClient();
        static readonly InfoDtoConverter _converter = new InfoDtoConverter();

        public async Task<DomainBankCoursesDto> GetBankCourses()
        {
            string url = "https://www.bps-sberbank.by/SBOLServer/rest/currency/rates?pck=CD";
            var request = new CardlessRequest { date = DateTime.Now.Ticks.ToString() };
            var response = await _client.PostAsJsonAsync<CardlessRequest, CardlessResponse>(url, request);
            return _converter.Convert(response);
        }
    }
}
