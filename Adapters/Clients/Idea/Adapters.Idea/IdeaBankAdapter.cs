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
            //todo
            return new DomainBankCoursesDto
            {
                
                Bank = BankName,
                //BankCourseTime = date.Add(response.data.rates.Rates.Select(x=>x.CourseTime).OrderByDescending(x=>x).First()),
                //RequestTime = date,
                //CurrencyPairInfos = response.data.rates.RateList .RateList.Select(x=>new CurrencyPairInfo
                //{
                //    BuyCourse = x.buy,
                //    SellCourse = x.sell,
                //    XCurrency = x.Currency
                //}).ToList()
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
        Buy = 0,
        Sell = 1
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
                            RateList = rateList.Select(rate =>
                            {
                                var r = (JObject) rate.Value;
                                return new Rate
                                {
                                    Value = r["Value"].Value<float>(),
                                    Currency = r["Currency"].Value<string>(),
                                    Operation = (Operation)r["Operation"].Value<int>(),
                                    Units = r["Units"].Value<int>()

                                };
                            }).ToList()
                        };
                    }).ToList()
                }).ToList()
            };
        }

        //stub
        public override bool CanConvert(Type objectType) => throw new NotImplementedException();
    }
}
