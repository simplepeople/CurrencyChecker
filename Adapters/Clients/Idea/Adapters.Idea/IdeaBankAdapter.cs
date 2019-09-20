using System;
using System.Net.Http;
using System.Threading.Tasks;
using Adapters.Common;
using Common.Extensions;
using Domain.Models;

namespace Adapters.Clients.Idea
{
    public class IdeaBankAdapter : BankAdapter
    {
        private static readonly HttpClient _client = new HttpClient();

        public override async Task<DomainBankCoursesDto> GetBankCourses()
        {
            var date = new DateTime().Date.ToString("dd-MM-yyyy");
            string url = $"https://www.ideabank.by/private-osoby/online-bank/vigoniy-kurs/?date={date}&id=70";
            var request = new VigodniyCursRequest();
            var response = await _client.PostAsJsonAsync<VigodniyCursRequest, VigodniyCursResponse>(url, request);
            return ConvertRawCourses(response);
        }

        private DomainBankCoursesDto ConvertRawCourses(VigodniyCursResponse response)
        {
            throw new NotImplementedException();
        }

        public override string BankName { get; } = "IdeaBank";
    }

    public class VigodniyCursResponse : IResponse<VigodniyCursRequest>
    {
        public Data data { get; set; }

        public class Data
        {
            public Ratesnb ratesnb { get; set; }
            public Rates rates { get; set; }
        }

        public class Ratesnb
        {
            public string _1USD { get; set; }
            public string _1EUR { get; set; }
            public string _100RUB { get; set; }
            public string _10PLN { get; set; }
        }

        public class Rates
        {
            //todo
            //public _164500 _164500 { get; set; }
            //public _112000 _112000 { get; set; }
        }

        /*
         * {"data":{"ratesnb":{"1 USD":"2.0523","1 EUR":"2.2685","100 RUB":"3.1986","10 PLN":"5.2270"},"rates":{"16:45:00":{"1 USD":{"1":{"Value":"2.0400","Currency":"USD","Units":"1","Operation":"1"},"2":{"Value":"2.0520","Currency":"USD","Units":"1","Operation":"2"}},"1 EUR":{"1":{"Value":"2.2430","Currency":"EUR","Units":"1","Operation":"1"},"2":{"Value":"2.2590","Currency":"EUR","Units":"1","Operation":"2"}}},"11:20:00":{"1 USD":{"1":{"Value":"2.0370","Currency":"USD","Units":"1","Operation":"1"},"2":{"Value":"2.0470","Currency":"USD","Units":"1","Operation":"2"}},"1 EUR":{"1":{"Value":"2.2510","Currency":"EUR","Units":"1","Operation":"1"},"2":{"Value":"2.2620","Currency":"EUR","Units":"1","Operation":"2"}}}}}}
*/

    }

    public class VigodniyCursRequest : IRequest
    {
    }
}
