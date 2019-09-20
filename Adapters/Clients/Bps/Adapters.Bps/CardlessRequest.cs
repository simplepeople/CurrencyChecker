using Common.Extensions;

namespace Adapters.Bps
{
    public class CardlessRequest : IRequest
    {
        public string type { get; } = "CARD";
        public string date { get; set; }
    }
}
