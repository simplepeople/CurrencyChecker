using System.Net.Http;
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
        public static async Task<TResponse> PostAsJsonAsync<TRequest, TResponse>(this HttpClient client, string url, TRequest obj) where TRequest : IRequest where TResponse : IResponse<TRequest>, new()
        {
            var response = await client.PostAsync(url, new JsonContent(obj));
            response.EnsureSuccessStatusCode();
            var responseStr = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TResponse>(responseStr);
        }
    }
    public interface IRequest
    {

    }

    public interface IResponse<TRequest>
    {

    }
}