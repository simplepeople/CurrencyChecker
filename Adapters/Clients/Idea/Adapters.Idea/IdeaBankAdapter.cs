using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Adapters.Common;
using Common.Extensions;
using Domain.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Adapters.Clients.Idea
{
    public class IdeaBankAdapter : BankAdapter
    {
        private static readonly HttpClient _client;

        static IdeaBankAdapter()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
        }

        public override async Task<DomainBankCoursesDto> GetBankCourses()
        {
            string url = "https://www.ideabank.by/o-banke/kursy-valyut/";
            var date = DateTime.Now;
            var request = new VigodniyCursRequest
            {
                Date = date.Date.ToString("dd-MM-yyyy"),
                Id = 70
            };
            var response = await _client.PostAsJsonAsync<VigodniyCursRequest, VigodniyCursResponse>(url, request);
            return ConvertRawCourses(date, response);
        }

        private DomainBankCoursesDto ConvertRawCourses(DateTime date, VigodniyCursResponse response)
        {
            var time = response.data.times.TimeList.OrderByDescending(x => x.CourseTime).First();
            return new DomainBankCoursesDto
            {
                Bank = BankName,
                BankCourseTime = date.Date.Add(time.CourseTime),
                RequestTime = date,
                CurrencyPairInfos = time.CurrencyList.Select(x =>
                {
                    var buy = x.RateList.First(y => y.Operation == Operation.Buy);
                    var sell = x.RateList.First(y => y.Operation == Operation.Sell);
                    return new CurrencyPairInfo
                    {
                        BuyCourse = buy.Value,
                        SellCourse = sell.Value,
                        XCurrency = buy.Currency
                    };
                }).ToList()
            };
        }

        public override string BankName { get; } = "IdeaBank";
    }

    public class VigodniyCursRequest : IRequest
    {
        [QueryParam("date")]
        public string Date { get; set; }

        [QueryParam("id")]
        public int Id { get; set; }
    }

    public class VigodniyCursResponse : IResponse<VigodniyCursRequest>
    {
        public Data data { get; set; }

        public class Data
        {
            [JsonProperty("rates")]
            public Times times { get; set; }
        }


        [JsonConverter(typeof(IdeaTimesConverter))]
        public class Times
        {
            public List<Time> TimeList { get; set; }
        }

        public class Time
        {
            public TimeSpan CourseTime { get; set; }
            public List<Currency> CurrencyList { get; set; }
        }

        public class Currency
        {
            public List<Rate> RateList { get; set; }
        }
    }

    public class Rate
    {
        public string Currency { get; set; }

        public float Value { get; set; }

        public Operation Operation { get; set; }

        public int Units { get; set; }
    }

    public enum Operation
    {
        Buy = 1,
        Sell = 2
    }


    public class IdeaTimesConverter :JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var times = JObject.Load(reader).Properties();
            return new VigodniyCursResponse.Times
            {
                TimeList = times.Select(time => new VigodniyCursResponse.Time
                {
                    CourseTime = TimeSpan.Parse(time.Name),
                    CurrencyList = ((JObject) time.Value).Properties().Select(currency =>
                    {
                        var rateList = ((JObject) currency.Value).Properties();
                        return new VigodniyCursResponse.Currency
                        {
                            RateList = rateList.Select(rate => rate.Value.ToObject<Rate>()).ToList()
                        };
                    }).ToList()
                }).ToList()
            };
        }

        //stub
        public override bool CanConvert(Type objectType) => throw new NotImplementedException();
    }
}
