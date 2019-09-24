using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Common.Extensions
{
    public static class NetworkExtensions
    {
        public class JsonContent : StringContent
        {
            public JsonContent(object obj) :
                base(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json")
            { }
        }
        public static async Task<TResponse> PostAsJsonAsync<TRequest, TResponse>(this HttpClient client, string url, TRequest request) where TRequest : IRequest where TResponse : IResponse<TRequest>, new()
        {
            var args = GetArgsDictionary(request);
            HttpResponseMessage response;
            if (args.Any())
                response = await client.PostAsync(url,  new FormUrlEncodedContent(args));
            else
                response = await client.PostAsync(url, new JsonContent(request));
            response.EnsureSuccessStatusCode();
            var responseStr = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TResponse>(responseStr);
        }

        private static Dictionary<string, string> GetArgsDictionary(IRequest request)
        {
            var argsDict = request.GetType().GetProperties().Where(x => x.GetCustomAttribute<QueryParamAttribute>() != null)
                .ToDictionary(x => x.GetCustomAttribute<QueryParamAttribute>().Name, x => x.GetValue(request).ToString());
            return argsDict;
        }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Struct, Inherited = false)]
    public class QueryParamAttribute : Attribute
    {
        public string Name { get; set; }

        public QueryParamAttribute(string name)
        {
            Name = name;
        }
    }

    public interface IRequest
    {

    }

    public interface IResponse<TRequest>
    {

    }
}